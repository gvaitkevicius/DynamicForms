using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Models;
using DynamicForms.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DynamicForms.Areas.PlugAndPlay.Controllers
{
    [Authorize]
    [Area("plugandplay")]
    public class ItensCalendarioController : BaseController
    {
        private readonly JSgi db;
        public ItensCalendarioController()
        {
            this.db = new ContextFactory().CreateDbContext(new string[] { });
        }
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult CadastrarItensCalendario(string CalendarioId)
        {
            T_Usuario user = ObterUsuarioLogado();
            ViewBag.UserName = user.USE_NOME;

            ViewBag.CalendarioId = CalendarioId;
            return View();

        }
        [HttpGet]
        public JsonResult ObterTurnos()
        {
            var lista = db.Turno.AsNoTracking().Select(x => new { x.Id, x.Descricao }).ToList();
            return Json(lista);
        }
        [HttpGet]
        public JsonResult ObterTurmas()
        {
            var lista = db.Turma.AsNoTracking().Select(x => new { x.Id, x.Descricao }).ToList();
            return Json(lista);
        }

        [HttpGet]
        public JsonResult ObterMaquinas()
        {
            var lista = db.Maquina.AsNoTracking().Select(x => new { x.MAQ_ID, x.MAQ_DESCRICAO, x.CAL_ID }).ToList();
            return Json(lista);
        }

        [HttpGet]
        public JsonResult ObterEquipes()
        {
            var lista = db.T_MAQUINAS_EQUIPES.AsNoTracking().GroupBy(x => x.EQU_ID).Select(x => x.FirstOrDefault());
            return Json(lista);
        }

        [HttpGet]
        public JsonResult ObterItensCalendario(string dataReferencia, string calId)
        {
            List<List<ItensCalendario>> lista_ordenada = new List<List<ItensCalendario>>();
            DateTime data = Convert.ToDateTime(dataReferencia);
            if (calId != null)
            {
                int convertedId = Convert.ToInt32(calId);
                data = data == DateTime.MinValue ? DateTime.Now : data; //se não estiver nada preenchido na data de referencia, usa como referencia a data de hoje

                //pesquisa o ultimo item de calendário em um intervalo de 7 dias
                //se não encontrar nenhum item de calendário próximo, irá retornar a data 01/01/0001, a função deve então retornar uma lista vazia
                var data_minima = data.AddDays(-7);
                var ultima_data = db.ItensCalendario.AsNoTracking().Where(x => x.ICA_DATA_DE.Date <= data.Date && x.ICA_DATA_DE.Date >= data_minima && x.CAL_ID == convertedId).OrderByDescending(x => x.ICA_DATA_DE).Select(x => x.ICA_DATA_DE).FirstOrDefault();
                data = Convert.ToDateTime(ultima_data);

                //se a data de referencia for diferente de 01/01/0001, da um select nos itens calendario, se não, deve retornar uma lista vazia
                if (data != DateTime.MinValue)
                {
                    DateTime primeiro_dia_semana = data.StartOfWeek(DayOfWeek.Sunday);
                    DateTime ultimo_dia_semana = data.EndOfWeek();

                    //seleciona itens cujo a data de inicio esteja between primeiro_dia_semana e ultimo_dia_semana
                    var lista = db.ItensCalendario.AsNoTracking().Where(x => x.CAL_ID == convertedId &&
                    (x.ICA_DATA_DE >= primeiro_dia_semana && x.ICA_DATA_DE <= ultimo_dia_semana)).OrderBy(x => x.ICA_DATA_DE).ToList(); //retorna 10 linhas do itens calendario

                    lista_ordenada = DatasPorDiaSemana(lista);
                }

                return Json(lista_ordenada);
            }

            return Json(null);
        }

        [HttpGet]
        public JsonResult ObterCalId(string MaqId)
        {
            var calId = db.Maquina.Where(x => x.MAQ_ID == MaqId).Select(x => x.CAL_ID).FirstOrDefault();
            return Json(calId);
        }

        [HttpPost]
        public ActionResult CadastrarItensCalendario(string Calendario, string ItensDoCalendario, string jsonMaquinas, string jsonEquipes, int preset)
        {
            var ListaMaquinas = JsonConvert.DeserializeObject<dynamic>(jsonMaquinas);
            var ListaEquipes = JsonConvert.DeserializeObject<dynamic>(jsonEquipes);
            List<dynamic> ListaCalendarios = new List<dynamic>();
            List<int> cal_ja_visitado = new List<int>();

            //percorre cada maquina/equipe e adiciona na lista de objetos dde calendario contendo o valor do id ad maquina ou id da equipe
            for (int i = 0; i < ListaMaquinas.Count; i++)
            {
                dynamic calendario_da_maquina = new ExpandoObject();
                calendario_da_maquina.CAL_ID = ListaMaquinas[i].calId;
                calendario_da_maquina.MAQ_ID = ListaMaquinas[i].maqId;
                calendario_da_maquina.EQU_ID = null;

                ListaCalendarios.Add(calendario_da_maquina);
            }

            for (int i = 0; i < ListaEquipes.Count; i++)
            {
                dynamic calendario_da_equipe = new ExpandoObject();
                calendario_da_equipe.CAL_ID = ListaEquipes[i].calId;
                calendario_da_equipe.MAQ_ID = null;
                calendario_da_equipe.EQU_ID = ListaEquipes[i].equId;

                ListaCalendarios.Add(calendario_da_equipe);
            }

            for (int i = 0; i < ListaCalendarios.Count; i++)
            {
                string calIdString = ListaCalendarios[i].CAL_ID;
                int calId = Convert.ToInt32(calIdString);

                if (calId == 0)
                { //se o calId for zero, então gera um novo calendario com a maquina/equipe correspondente

                    //se o maq_id for diferente dennullo, entao o calendario pertence a uma maquina
                    if (ListaCalendarios[i].MAQ_ID != null)
                    {
                        string maqId = ListaCalendarios[i].MAQ_ID;
                        calId = GerarCalendarioMaquina(maqId);
                    }
                    else if (ListaCalendarios[i].EQU_ID != null)
                    {
                        string equId = ListaCalendarios[i].EQU_ID;
                        calId = GerarCalendarioEquipe(equId);
                    }

                }
                if (cal_ja_visitado.Contains(calId))
                { //se o cal_id da maquina atual ja tiver sido visitado, continua para a proxima maquina
                    continue;
                }
                if (calId != 0 && !cal_ja_visitado.Contains(calId))
                {
                    //se não for == 0 e nao tiver sido visitado, adiciona ele na lista de calendarios visitados e continua o código
                    cal_ja_visitado.Add(calId);
                }

                dynamic _dadosCalendario = JsonConvert.DeserializeObject<dynamic>(Calendario);
                dynamic _itensCalendario = JsonConvert.DeserializeObject<dynamic>(ItensDoCalendario);
                string auxDini = _dadosCalendario.DataInicio;
                string auxDfim = _dadosCalendario.DataFim;
                IList<ItensCalendario> ListaItens = new List<ItensCalendario>();
                DateTime inicio = Convert.ToDateTime(auxDini.Trim());
                DateTime fim = Convert.ToDateTime(auxDfim.Trim());
                MasterController mc = new MasterController();
                List<List<object>> ListOfListObjects;

                #region Gerando lista
                ///region description:
                ///essa região gera os items de calendário

                //Gerando Calendario para o Range inicio...fim
                while (inicio.CompareTo(fim) <= 0)
                {
                    switch (inicio.DayOfWeek)//Identificando qual o dia da semana do item do range.
                    {
                        case DayOfWeek.Sunday:
                            NovoItem(calId, _itensCalendario, inicio, ListaItens, "0", preset);
                            break;
                        case DayOfWeek.Monday:
                            NovoItem(calId, _itensCalendario, inicio, ListaItens, "1", preset);
                            break;
                        case DayOfWeek.Tuesday:
                            NovoItem(calId, _itensCalendario, inicio, ListaItens, "2", preset);
                            break;
                        case DayOfWeek.Wednesday:
                            NovoItem(calId, _itensCalendario, inicio, ListaItens, "3", preset);
                            break;
                        case DayOfWeek.Thursday:
                            NovoItem(calId, _itensCalendario, inicio, ListaItens, "4", preset);
                            break;
                        case DayOfWeek.Friday:
                            NovoItem(calId, _itensCalendario, inicio, ListaItens, "5", preset);
                            break;
                        case DayOfWeek.Saturday:
                            NovoItem(calId, _itensCalendario, inicio, ListaItens, "6", preset);
                            break;
                    }

                    inicio = inicio.AddDays(1);
                }

                #endregion

                #region Corrigindo linhas conflitantes
                ///region description:
                ///essa região detecta quando existe uma linha com a data de inicio e termino dentro da nova data de inicio e termino
                ///linha1 05:00 -> 10:00 linha2 08:00 -> 09:00 //essas linhas estão conflitando, então, é preciso excluir a linha antiga
                ///se houver conflito apenas no horário de inicio ou término, não é preciso excluir, basta alterar o horário que conflitou (inicio o termino)

                List<object> itens_linhas_conflitantes = new List<object>();
                int? tipo_linha_sobreposta = null; //se uma linha sobreposta tiver sua data alterada, irá lembrar o seu tipo
                string turno_linha_sobreposta = null;
                string turma_linha_sobreposta = null;

                //procura linhas dentro do range do item
                Dictionary<string, List<ItensCalendario>> linhas_conflitantes = ItensCalendario.ProcuraLinhasBetween(ListaItens.First().ICA_DATA_DE, ListaItens.Last().ICA_DATA_ATE, calId, preset, db);
                if (linhas_conflitantes.Count() > 0)
                {
                    List<ItensCalendario> conflitosInicio = linhas_conflitantes.ContainsKey("conflitos_no_inicio") ? linhas_conflitantes["conflitos_no_inicio"] : null;
                    List<ItensCalendario> conflitosTermino = linhas_conflitantes.ContainsKey("conflitos_no_termino") ? linhas_conflitantes["conflitos_no_termino"] : null;
                    List<ItensCalendario> conflitosInicioTermino = linhas_conflitantes.ContainsKey("conflitos_inicio_termino") ? linhas_conflitantes["conflitos_inicio_termino"] : null;
                    List<ItensCalendario> conflitosSobrepondo = linhas_conflitantes.ContainsKey("conflitos_sobrepondo") ? linhas_conflitantes["conflitos_sobrepondo"] : null;

                    //verifica se veio linhas com conflitos no inicio e termino
                    if (conflitosInicioTermino != null && conflitosInicioTermino.Count() > 0)
                    {
                        //deleta essas linhas e remove da lista
                        foreach (ItensCalendario linha in conflitosInicioTermino)
                        {
                            linha.PlayAction = "delete";
                            conflitosInicio = conflitosInicio.Where(x => x.ICA_ID != linha.ICA_ID).ToList();
                            conflitosTermino = conflitosTermino.Where(x => x.ICA_ID != linha.ICA_ID).ToList();
                            itens_linhas_conflitantes.Add(linha);
                        }
                    }

                    //se teve conflito com a data de termino/inicio, atualiza a parte conflitante da linha
                    if (conflitosTermino != null && conflitosTermino.Count() > 0)
                    {
                        if (conflitosTermino.First().ICA_DATA_DE != ListaItens.First().ICA_DATA_DE)
                        {
                            conflitosTermino.First().ICA_DATA_ATE = ListaItens.First().ICA_DATA_DE;
                            conflitosTermino.First().PlayAction = "update";
                        }
                        else //se a atualização da data de término for igual a data de ínicio, então exclui a linha ao invés de atualizar
                        {
                            conflitosTermino.First().PlayAction = "delete";
                        }

                        itens_linhas_conflitantes.Add(conflitosTermino.First());
                    }

                    if (conflitosInicio != null && conflitosInicio.Count() > 0)
                    {
                        if (conflitosInicio.First().ICA_DATA_ATE != ListaItens.Last().ICA_DATA_ATE)
                        {
                            conflitosInicio.First().ICA_DATA_DE = ListaItens.Last().ICA_DATA_ATE;
                            conflitosInicio.First().PlayAction = "update";
                        }
                        else //se a atualização da data de inicio for igual a data de ínicio, então exclui a linha ao invés de atualizar
                        {
                            conflitosInicio.First().PlayAction = "delete";
                        }

                        itens_linhas_conflitantes.Add(conflitosInicio.First());
                    }

                    if (conflitosSobrepondo != null && conflitosSobrepondo.Count() > 0)
                    {
                        conflitosSobrepondo.First().ICA_DATA_DE = ListaItens.Last().ICA_DATA_ATE;
                        conflitosSobrepondo.First().PlayAction = "update";

                        tipo_linha_sobreposta = conflitosSobrepondo.First().ICA_TIPO;
                        turno_linha_sobreposta = conflitosSobrepondo.First().URN_ID;
                        turma_linha_sobreposta = conflitosSobrepondo.First().URM_ID;

                        itens_linhas_conflitantes.Add(conflitosSobrepondo.First());
                    }

                    ListOfListObjects = new List<List<object>> { itens_linhas_conflitantes };
                    List<LogPlay> logs_conflito = mc.UpdateData(ListOfListObjects, 0, true);
                }

                #endregion

                #region Inserindo e atualizando linhas
                ///region description:
                ///essa região detecta quando uma linha já existe, se a linha existir, atualiza seus dados, se não, cria uma nova linha

                List<object> itens = new List<object>();
                for (int x = 0; x < ListaItens.Count(); x++)
                {
                    ItensCalendario itemAtual = ListaItens[x];

                    //se já existe uma linha no intervalo atualiza ela, senao, cria uma nova
                    ItensCalendario linha_mesmo_intervalo = new ItensCalendario();
                    linha_mesmo_intervalo = linha_mesmo_intervalo.ProcuraLinhaPorData(itemAtual.ICA_DATA_DE, itemAtual.ICA_DATA_ATE, calId, preset, db);
                    if (linha_mesmo_intervalo != null)
                    {
                        ItensCalendario linhaAtualizada = new ItensCalendario(
                            preset,
                            itemAtual.ICA_DATA_DE,
                            itemAtual.ICA_DATA_ATE,
                            "Essa linha foi atualizada.",
                            itemAtual.ICA_TIPO,
                            itemAtual.URM_ID,
                            itemAtual.URN_ID,
                            itemAtual.ICA_LIMPESA_MAQUINA,
                            itemAtual.CAL_ID,
                            linha_mesmo_intervalo.ICA_ID
                        );
                        linhaAtualizada.PlayAction = "update";
                        itens.Add(linhaAtualizada);
                    }
                    else
                    {
                        itemAtual.PlayAction = "insert";
                        itens.Add(itemAtual);
                    }
                }

                ListOfListObjects = new List<List<object>> { itens };
                List<LogPlay> logs = mc.UpdateData(ListOfListObjects, 0, true);

                #endregion

                #region Gerando itens de preenchimento
                ///region description:
                ///essa região gera items de preenchimento para preencher lacunas da tabela.
                ///se a linha acabar no horário 05:00 e a próxima só começar as 10:00, então existe uma lacuna


                //gera itens de preenchimento para horários antes e depois do novo horário a ser inserido
                List<object> itens_preenchimento = new List<object>();
                IList<ItensCalendario> ListaItensPreenchimento = new List<ItensCalendario>();

                ItensCalendario item_de_preenchimento_inicio = new ItensCalendario();
                item_de_preenchimento_inicio = item_de_preenchimento_inicio.GerarItemPreenchimentoInicio(ListaItens.First().ICA_DATA_DE, calId, preset, db, tipo_linha_sobreposta, turno_linha_sobreposta, turma_linha_sobreposta); //passa o tipo da linha sobreposta se houver, pois ao mudar a data de inicio da linha sobreposta, o sistema irá criar um item de preenchimento no inicio, que precisará ter o mesmo tipo da linha sobreposta
                if (item_de_preenchimento_inicio != null)
                {
                    ListaItensPreenchimento.Add(item_de_preenchimento_inicio);
                }

                ItensCalendario item_de_preenchimento_fim = new ItensCalendario();
                item_de_preenchimento_fim = item_de_preenchimento_fim.GerarItemPreenchimentoFim(ListaItens.Last().ICA_DATA_ATE, calId, preset, db);
                if (item_de_preenchimento_fim != null)
                {
                    ListaItensPreenchimento.Add(item_de_preenchimento_fim);
                }

                foreach (ItensCalendario ic in ListaItensPreenchimento)
                {
                    ic.PlayAction = "insert";
                    itens_preenchimento.Add(ic);
                }

                if (itens_preenchimento.Count() > 0)
                {
                    ListOfListObjects = new List<List<object>> { itens_preenchimento };
                    List<LogPlay> logsPreenchimento = mc.UpdateData(ListOfListObjects, 0, true);
                }

                #endregion
            }

            return View();
        }

        /// <summary>
        /// Gera um novo calendário para a máquina, depois de criar o calendário, vincula uma máqunia a ele.
        /// </summary>
        /// <param name="maqId"></param>
        /// <returns>Retorna o ID do novo calendário</returns>
        public int GerarCalendarioMaquina(string maqId)
        {
            dynamic maqNome = db.Maquina.AsNoTracking().Where(y => y.MAQ_ID == maqId).Select(y => y.MAQ_DESCRICAO).FirstOrDefault().ToString();
            dynamic maxId = db.Calendario.Max(c => c.CAL_ID);
            int newId = Convert.ToInt32(maxId) + 1;

            //gera o novo calendário
            Calendario cal = new Calendario();
            cal.CAL_DESCRICAO = "CAL_" + maqNome;
            cal.CAL_DIVIDE_DIA_EM = 0;
            cal.CAL_ID = newId;
            cal.PlayAction = "insert";

            List<object> item = new List<object> { cal };
            List<List<object>> ListOfListObjects = new List<List<object>>() { item };

            MasterController mc = new MasterController();
            List<LogPlay> logs = mc.UpdateData(ListOfListObjects, 0, true);

            //recupera a máquina e atualiza sua coluna de CAL_ID
            Maquina maquina = new Maquina();
            dynamic m = db.Maquina.Where(x => x.MAQ_ID == maqId).FirstOrDefault();
            if (m != null)
            {
                maquina.MAQ_ID = m.MAQ_ID;
                maquina.MAQ_DESCRICAO = m.MAQ_DESCRICAO;
                maquina.CAL_ID = newId;
                maquina.MAQ_CONTROL_IP = m.MAQ_CONTROL_IP;
                maquina.GMA_ID = m.GMA_ID;
                maquina.MAQ_STATUS = m.MAQ_STATUS;
                maquina.MAQ_ULTIMA_ATUALIZACAO = m.MAQ_ULTIMA_ATUALIZACAO;
                maquina.MAQ_SIRENE_SEMAFORO = m.MAQ_SIRENE_SEMAFORO;
                maquina.MAQ_COR_SEMAFORO = m.MAQ_COR_SEMAFORO;
                maquina.MAQ_ID_MAQ_PAI = m.MAQ_ID_MAQ_PAI;
                maquina.MAQ_TIPO_CONTADOR = m.MAQ_TIPO_CONTADOR;
                maquina.MAQ_TIPO_PLANEJAMENTO = m.MAQ_TIPO_PLANEJAMENTO;
                maquina.MAQ_AVALIA_CUSTO = m.MAQ_AVALIA_CUSTO;
                maquina.FPR_ID_OP_PRODUZINDO = m.FPR_ID_OP_PRODUZINDO;
                maquina.MAQ_CONGELA_FILA = m.MAQ_CONGELA_FILA;
                maquina.MAQ_TEMPO_MIN_PARADA = m.MAQ_TEMPO_MIN_PARADA;
                maquina.MAQ_QTD_CORES = m.MAQ_QTD_CORES;
                maquina.MAQ_ID_INTEGRACAO = m.MAQ_ID_INTEGRACAO;
                maquina.MAQ_ID_INTEGRACAO_ERP = m.MAQ_ID_INTEGRACAO_ERP;
                maquina.MAQ_HIERARQUIA_SEQ_TRANSFORMACAO = m.MAQ_HIERARQUIA_SEQ_TRANSFORMACAO;
                maquina.EQU_ID = m.EQU_ID;
                maquina.PlayAction = "update";
                //maquina.MAQ_CALCULA_CUSTO = m.MAQ_CALCULA_CUSTO;

                List<object> item_maq = new List<object>() { maquina };
                ListOfListObjects = new List<List<object>>() { item_maq };
                List<LogPlay> logMaq = mc.UpdateData(ListOfListObjects, 0, true);
            }

            return newId;
        }

        /// <summary>
        /// Gera um novo calendário para a equipe, depois de criar o calendário, vincula uma equipe a ele.
        /// </summary>
        /// <param name="equId"></param>
        /// <returns>Retorna o ID do novo calendário</returns>
        public int GerarCalendarioEquipe(string equId)
        {
            dynamic equNome = equId;
            dynamic maxId = db.Calendario.Max(c => c.CAL_ID);
            int newId = Convert.ToInt32(maxId) + 1;

            //gera o novo calendário
            Calendario cal = new Calendario();
            cal.CAL_DESCRICAO = "CAL_" + equNome;
            cal.CAL_DIVIDE_DIA_EM = 0;
            cal.CAL_ID = newId;
            cal.PlayAction = "insert";

            List<object> item = new List<object> { cal };
            List<List<object>> ListOfListObjects = new List<List<object>>() { item };

            MasterController mc = new MasterController();
            List<LogPlay> logs = mc.UpdateData(ListOfListObjects, 0, true);

            ////recupera a máquina e atualiza sua coluna de CAL_ID
            T_MAQUINAS_EQUIPES equipe = new T_MAQUINAS_EQUIPES();
            dynamic equipes = db.T_MAQUINAS_EQUIPES.Where(x => x.EQU_ID == equId).ToList();

            if (equipes != null && equipes.Count > 0)
            {
                List<object> item_equ = new List<object>();

                for (int i = 0; i < equipes.Count; i++)
                {
                    equipe = (T_MAQUINAS_EQUIPES)equipes[i];
                    equipe.CAL_ID = newId;
                    equipe.PlayAction = "update";

                    item_equ.Add(equipe);
                }

                ListOfListObjects = new List<List<object>>() { item_equ };
                List<LogPlay> logMaq = mc.UpdateData(ListOfListObjects, 0, true);

                return newId;
            }
            else
            {
                return 0;
            }
        }

        private IList<DateTime> GerarDatas(DateTime inicio, DateTime fim)
        {
            List<DateTime> datas = new List<DateTime>();
            DateTime data = inicio;
            while (data <= fim)
            {
                datas.Add(data);
                data = data.AddDays(1);
            }
            return datas;
        }

        private void NovoItem(int CalendarioId, dynamic _itensCalendario, DateTime inicio, IList<ItensCalendario> ListaItens, string ds, int preset)
        {
            DateTime dataDe;
            DateTime dataAte;
            int add_Day = 0;
            foreach (var item in _itensCalendario)
            {
                if (item.diaSem == ds)
                {
                    try
                    {
                        //Calculando Hora de inicio e Fim do item do calendário
                        ExtrairDatas(inicio, out dataDe, out dataAte, add_Day, item);
                        //intervalo entre datas
                        TimeSpan deltaHoras;

                        if (dataDe.CompareTo(dataAte) != 0)
                        {
                            if (dataAte.Hour < 12 && dataAte < dataDe)
                            {
                                //SE dataAte for maior que dataDe então deltaHoras sera dataAte-dataDe / senao dataDe-dataAte
                                deltaHoras = (dataAte.CompareTo(dataDe) > 0) ? dataAte.Subtract(dataDe) : dataDe.Subtract(dataAte);
                                //DataAux meio dia
                                DateTime dataAux = new DateTime(inicio.Year, inicio.Month, inicio.Day, 12, 00, 0);
                                //Meio dia menos meia noite
                                TimeSpan deltaDois = dataAux.Subtract(new DateTime(inicio.Year, inicio.Month, inicio.Day, deltaHoras.Hours, deltaHoras.Minutes, 0));
                                //Intervalo de 12 horas
                                TimeSpan tsAux = new TimeSpan(12, 0, 0);
                                ///
                                deltaDois = deltaDois.Add(tsAux);
                                dataAte = dataDe.AddHours(deltaDois.Hours);
                                dataAte = dataAte.AddMinutes(deltaDois.Minutes);
                            }
                            else
                            {
                                TimeSpan tsAux = new TimeSpan(24, 0, 0);

                                //dataAte: 01/01/2020 18:00 dataDe: 01/01/2020 22:00
                                //deltaHoras: 4
                                deltaHoras = (dataAte.CompareTo(dataDe) > 0) ? dataAte.Subtract(dataDe) : tsAux - (dataDe.Subtract(dataAte));
                                dataAte = dataDe.AddHours(deltaHoras.Hours);
                                dataAte = dataAte.AddMinutes(deltaHoras.Minutes);
                            }

                        }
                        else
                        {
                            dataAte = dataAte.AddHours(24);
                        }
                        //--
                        int tipo = Convert.ToInt32(item.tipo);
                        ItensCalendario novoHorario = new ItensCalendario(preset, dataDe, dataAte, "", tipo, item.turma, item.turno, item.limp, CalendarioId);

                        ListaItens.Add(novoHorario);
                        if (dataAte.Day != inicio.Day)
                        {
                            add_Day = 1;
                        }
                    }
                    catch (Exception ex) { string erro = "Erro:-->" + ex.InnerException.Message + "\n" + ex.Message; }

                }
            }
        }
        private void ExtrairDatas(DateTime inicio, out DateTime dataDe, out DateTime dataAte, int add_Day, dynamic item)
        {
            string horarioUm = item.hInicio;
            string horarioDois = item.hFim;
            string[] hAux = horarioUm.Trim().Split(':');
            string[] hAux1 = horarioDois.Trim().Split(':');
            int[] hhMM = new int[4] { Convert.ToInt32(hAux[0]), Convert.ToInt32(hAux[1]), Convert.ToInt32(hAux1[0]), Convert.ToInt32(hAux1[1]) };
            dataDe = new DateTime(inicio.Year, inicio.Month, inicio.Day, hhMM[0], hhMM[1], 0);
            dataAte = new DateTime(inicio.Year, inicio.Month, inicio.Day, hhMM[2], hhMM[3], 0);
            dataDe = dataDe.AddDays(add_Day);
            dataAte = dataAte.AddDays(add_Day);
        }

        /// <summary>
        /// Retorna uma lista de uma lista de itens calendários. A primeira lista terá indexes de 0 a 7, representando os dias das semanas.
        /// Cada index terá uma lista de um ou mais itens calendários, que representa os horários cadastrados para aquele dia.
        /// IMPORTANT: Na representação, o dia só começa a partir das 6h, por isso, horários que estiverem entre 00:00 e 05:59 são considerados na mesma linha do dia anterior.
        /// </summary>
        /// <param name="itens"></param>
        /// <returns></returns>
        private List<List<ItensCalendario>> DatasPorDiaSemana(List<ItensCalendario> itens)
        {
            List<List<ItensCalendario>> semana = new List<List<ItensCalendario>>();
            List<ItensCalendario> itens_na_semana = new List<ItensCalendario>();

            for (int x = 0; x < itens.Count; x++)
            {
                ItensCalendario itemAtual = itens[x];
                ItensCalendario proximoItem = x + 1 < itens.Count ? itens[x + 1] : null;

                DateTime dInicio = itemAtual.ICA_DATA_DE;
                DateTime dTermino = itemAtual.ICA_DATA_ATE;

                itens_na_semana.Add(itemAtual);

                //checa se começa em um dia e termina em outro
                if (dInicio.Date != dTermino.Date)
                {
                    //se houver um proximo item, verifica se o horario de inicio dele é depois das 6h, se for, adiciona a lista e vai para o proximo dia da semana
                    if (proximoItem != null)
                    {
                        if (proximoItem.ICA_DATA_DE.Hour >= 6)
                        {
                            semana.Add(itens_na_semana);
                            itens_na_semana = new List<ItensCalendario>();
                            continue;
                        }
                    }
                    else //se não houver um próximo item, quer dizer que está no último item da lista, entao, adiciona ele na lista.
                    {
                        semana.Add(itens_na_semana);
                        itens_na_semana = new List<ItensCalendario>();
                        continue;
                    }
                }

                //verifica se a data de inicio está entre meia noite e 6h, e se a data de término é maior ou igual as 6
                //se for, quer dizer que pode ir para o proximo dia da semana.
                if ((dInicio.Hour > 0 && dInicio.Hour < 6) &&
                    dTermino.Hour >= 6)
                {
                    semana.Add(itens_na_semana);
                    itens_na_semana = new List<ItensCalendario>();
                    continue;
                }
            }

            return semana;
        }
    }

}
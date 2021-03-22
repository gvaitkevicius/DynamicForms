using DynamicForms.Context;
using DynamicForms.Models;
using DynamicForms.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "ITENS DOS CALENDÁRIOS")]
    public class ItensCalendario
    {
        [READ] [TAB(Value = "PRINCIPAL")] [Display(Name = "COD ITENS CALEND")] public int ICA_ID { get; set; }
        [SEARCH] [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA INICIO")] [Required(ErrorMessage = "Campo ICA_DATA_DE requirido.")] public DateTime ICA_DATA_DE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA FIM")] [Required(ErrorMessage = "Campo ICA_DATA_ATE requirido.")] public DateTime ICA_DATA_ATE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OBSERVACAO")] [MaxLength(200, ErrorMessage = "")] public string ICA_OBSERVACAO { get; set; }
        [Combobox(Description = "Expediente Normal", Value = "1")]
        [Combobox(Description = "Sem Expediente de trabalho", Value = "2")]
        [Combobox(Description = "Folga", Value = "3")]
        [Combobox(Description = "Feriado", Value = "4")]
        [Combobox(Description = "Troca de Feriado", Value = "5")]
        [Combobox(Description = "Ferias Coletivas", Value = "6")]
        [Combobox(Description = "O", Value = "7")]
        [Combobox(Description = "Indisponível", Value = "8")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO")] [Required(ErrorMessage = "Campo ICA_TIPO requirido.")] public int ICA_TIPO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD TURMA")] [MaxLength(10, ErrorMessage = "")] public string URM_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD TURNO")] [MaxLength(10, ErrorMessage = "")] public string URN_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD CALENDÁRIO")] [Required(ErrorMessage = "Campo CAL_ID requirido.")] [Range(1, 99999)] public int CAL_ID { get; set; }
        [Combobox(Description = "Não Limpou", Value = "0")]
        [Combobox(Description = "Limpou", Value = "1")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LIMPEZA DE MÁQUINA")] public int? ICA_LIMPESA_MAQUINA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD MÁQUINA")] public string MAQ_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PRODUTO")] public string PRO_ID { get; set; }

        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        public virtual Turno Turno { get; set; }
        public virtual Turma Turma { get; set; }
        public virtual Calendario Calendario { get; set; }
        public virtual Maquina Maquina { get; set; }
        public virtual Produto Produto { get; set; }


        #region Construtores

        public ItensCalendario()
        {
        }

        public ItensCalendario(int preset, DateTime dataDe, DateTime dataAte, string observacao, int icaTipo, dynamic urmId, dynamic urnId, dynamic icaLimpesaMaquina, int calId, int icaId)
        {
            this.ICA_DATA_DE = dataDe;
            this.ICA_DATA_ATE = dataAte;
            this.ICA_OBSERVACAO = observacao;
            this.ICA_TIPO = (preset == 1) ? 0 : (preset == 2) ? 2 : (preset == 0) ? icaTipo : 1; //preset == 1 (improditivo) preset == 2 (manutencao) preset == 0 ()
            this.URM_ID = (this.ICA_TIPO != 1) ? null : urmId;
            this.URN_ID = (this.ICA_TIPO != 1) ? null : urnId;
            this.ICA_LIMPESA_MAQUINA = (preset == 2) ? 0 : (preset == 1) ? 0 : (preset == 0) ? icaLimpesaMaquina : 1; //preset == 2 (NAO) preset == 1(NAO) preset == 0 ()
            this.CAL_ID = calId;
            this.ICA_ID = icaId;
        }

        public ItensCalendario(int preset, DateTime dataDe, DateTime dataAte, string observacao, int icaTipo, dynamic urmId, dynamic urnId, dynamic icaLimpesaMaquina, int calId)
        {
            this.ICA_DATA_DE = dataDe;
            this.ICA_DATA_ATE = dataAte;
            this.ICA_OBSERVACAO = observacao;
            this.ICA_TIPO = (preset == 1) ? 0 : (preset == 2) ? 2 : (preset == 0) ? icaTipo : 0; //preset == 1 (improditivo) preset == 2 (manutencao) preset == 0 (expediente)
            this.URM_ID = (this.ICA_TIPO != 1) ? null : urmId;
            this.URN_ID = (this.ICA_TIPO != 1) ? null : urnId;
            this.ICA_LIMPESA_MAQUINA = (preset == 2) ? 0 : (preset == 1) ? 0 : (preset == 0) ? icaLimpesaMaquina : 1; //preset == 2 (NAO) preset == 1(NAO) preset == 0 ()
            this.CAL_ID = calId;
        }

        #endregion

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            bool check = true;
            //using (JSgi db = new ContextFactory().CreateDbContext(Array.Empty<string>()))
            //{
            //    List<object> ItensCalendario = new List<object>();
            //    
            //}

            return check;
        }

        /// <summary>
        /// Gera um item calendário que serve como preenchimento da tabela de itens calendario.
        /// Se houver uma lacuna entre os horários de inicio e término, ele irá receber o último horário de termino, e o primeiro horário de inicio dos itens atuais a serem adicionados.
        /// </summary>
        /// <param name="primeiraDataInicio">Data de inicio do primeiro elemento da lista a ser adicionada</param>
        /// <param name="calId">ID do calendário</param>
        /// <returns>Retorna um objeto ItensCalendario preenchido com ICA_DATA_DE = ultima data de termino da tabela e ICA_DATA_ATE com a primeira data de inicio dos itens a serem adicionados. 
        /// Retorna nulo caso não haja lacunas.</returns>
        public ItensCalendario GerarItemPreenchimentoInicio(DateTime primeiraDataInicio, int calId, int preset, JSgi db, int? tipo_linha_sobreposta = null, string turno_linha_sobreposta = null, string turma_linha_sobreposta = null)
        {
            ItensCalendario item_de_preenchimento = null;

            //pega a data anterior em relação a data de inicio atual
            var listaHorarios = db.ItensCalendario.AsNoTracking().Where(x => x.ICA_DATA_ATE <= primeiraDataInicio && x.CAL_ID == calId).OrderBy(x => x.ICA_DATA_ATE).ToList();
            ItensCalendario ultimaDataTermino = listaHorarios.Count() > 0 ? listaHorarios.LastOrDefault() : new ItensCalendario();

            //verifica se a ultima data de termino da tabela é menor que a data de inicio atual
            //se for, quer dizer que existe uma lacuna
            //ex: ultimaDataTermino: 20/04 05:00 MENOR QUE dataInicio: 20/04 06:00 (existe uma lacuna de uma hora entre o termino e o inicio)
            if (ultimaDataTermino.ICA_DATA_ATE < primeiraDataInicio && listaHorarios != null && listaHorarios.Count() > 0)
            {
                item_de_preenchimento = new ItensCalendario(
                    0,
                    ultimaDataTermino.ICA_DATA_ATE,
                    primeiraDataInicio,
                    "Gerada automaticamente para preencher lacunas.",
                    tipo_linha_sobreposta != null ? (int)tipo_linha_sobreposta : ultimaDataTermino.ICA_TIPO,
                    turma_linha_sobreposta != null ? turma_linha_sobreposta : ultimaDataTermino.URM_ID,
                    turno_linha_sobreposta != null ? turno_linha_sobreposta : ultimaDataTermino.URN_ID,
                    ultimaDataTermino.ICA_LIMPESA_MAQUINA,
                    ultimaDataTermino.CAL_ID
                );
            }

            return item_de_preenchimento;
        }

        /// <summary>
        /// Gera um item calendário que serve como preenchimento da tabela de itens calendario.
        /// Se houver uma lacuna entre o horário de termino a ser adicionado, e o próximo horário de inicio da tabela
        /// </summary>
        /// <param name="ultimaDataTermino">Última data de término dos itens a serem adicionados</param>
        /// <param name="calId">ID do calendário</param>
        /// <returns>Retorna um objeto ItensCalendario preenchido com ICA_DATA_DE = ultima data de termino a ser adicionada e ICA_DATA_ATE a proxima data de inicio da tabela. 
        /// Retorna nulo caso não haja lacunas.</returns>
        public ItensCalendario GerarItemPreenchimentoFim(DateTime ultimaDataTermino, int calId, int preset, JSgi db)
        {
            ItensCalendario item_de_preenchimento = null;

            //pega as proximas datas de inicio
            var listaHorarios = db.ItensCalendario.AsNoTracking().Where(x => x.ICA_DATA_DE >= ultimaDataTermino && x.CAL_ID == calId).OrderBy(x => x.ICA_DATA_DE).ToList();
            ItensCalendario proximaDataInicio = listaHorarios.Count() > 0 ? listaHorarios.FirstOrDefault() : new ItensCalendario();

            //verifica se a proxima data de inicio é maior que a ultima data de termino a ser adicionada
            //se for, quer dizer que existe uma lacuna
            //ex: proximaDataInicio: 01/04 07:00 MAIOR QUE ultimaDataTermino: 01/04 06:00 (existe uma lacuna de uma hora entre o termino dos itens e o proximo inicio da tabela)
            if (proximaDataInicio.ICA_DATA_DE > ultimaDataTermino && listaHorarios != null && listaHorarios.Count() > 0)
            {
                item_de_preenchimento = new ItensCalendario(
                    0,
                    ultimaDataTermino,
                    proximaDataInicio.ICA_DATA_DE,
                    "Gerada automaticamente para preencher lacunas.",
                    proximaDataInicio.ICA_TIPO,
                    proximaDataInicio.URM_ID,
                    proximaDataInicio.URN_ID,
                    proximaDataInicio.ICA_LIMPESA_MAQUINA,
                    proximaDataInicio.CAL_ID
                );
            }

            return item_de_preenchimento;
        }

        /// <summary>
        /// Procura uma linha por uma data e horário
        /// </summary>
        /// <param name="inicio">Data de inicio da linha</param>
        /// <param name="fim">Data de término da linha</param>
        /// <param name="calId">ID do calendário</param>
        /// <returns>Retorna nulo ou a linha encontrada.</returns>
        public ItensCalendario ProcuraLinhaPorData(DateTime inicio, DateTime fim, int calId, int preset, JSgi db)
        {
            dynamic linhas_na_mesma_data = db.ItensCalendario.AsNoTracking().Where(x => x.CAL_ID == calId &&
                    (
                        x.ICA_DATA_DE == inicio && x.ICA_DATA_ATE == fim
                    )
            );

            List<ItensCalendario> list_linhas_mesma_data = new List<ItensCalendario>();
            foreach (dynamic l in linhas_na_mesma_data)
            {
                ItensCalendario item = new ItensCalendario(
                    0,
                    l.ICA_DATA_DE,
                    l.ICA_DATA_ATE,
                    l.ICA_OBSERVACAO,
                    l.ICA_TIPO,
                    l.URM_ID,
                    l.URN_ID,
                    l.ICA_LIMPESA_MAQUINA,
                    l.CAL_ID,
                    l.ICA_ID
                );

                list_linhas_mesma_data.Add(item);
            }

            if (list_linhas_mesma_data.Count() > 0 && list_linhas_mesma_data != null)
                return list_linhas_mesma_data.FirstOrDefault();
            else
                return null;
        }

        /// <summary>
        /// Recebe um intervalo de tempo e procura linhas que tenham a coluna de ICA_DATA_DE ou ICA_DATA_ATE dentro desse intervalo.
        /// </summary>
        /// <param name="inicio">Data de ínicio do intervalo</param>
        /// <param name="fim">Data de término do intervalo</param>
        /// <param name="calId">ID do calendário</param>
        /// <param name="preset">Preset escolhido: 0 - normal, 1 - feriado, 2 - manutencao </param>
        /// <param name="db"></param>
        /// <returns>Retorna um dicionário que pode conter: um item calendário na chave "conflito_no_termino" e/ou "conflito_no_inicio" e/ou um item "conflitos_inicio_termino" que contém linhas que conflitaram nas duas colunas</returns>
        public static Dictionary<string, List<ItensCalendario>> ProcuraLinhasBetween(DateTime inicio, DateTime fim, int calId, int preset, JSgi db)
        {
            //procura todas as colunas de inicio que estejam between as datas, todas as colunas de término, e por fim, todas linhas que a coluna de inicio E a coluna de término estão dentro do between.
            var listaColunaInicio = db.ItensCalendario.Where(x => x.CAL_ID == calId
            && (x.ICA_DATA_DE >= inicio && x.ICA_DATA_DE <= fim));

            var listaColunaTermino = db.ItensCalendario.Where(x => x.CAL_ID == calId
            && (x.ICA_DATA_ATE >= inicio && x.ICA_DATA_ATE <= fim));

            var listaColunaTermino_Inicio = db.ItensCalendario.Where(x => x.CAL_ID == calId
            && (x.ICA_DATA_ATE >= inicio && x.ICA_DATA_ATE <= fim)
            && (x.ICA_DATA_DE >= inicio && x.ICA_DATA_DE <= fim));

            var listaLinhasSobrepondo = db.ItensCalendario.Where(x => x.CAL_ID == calId
            && (x.ICA_DATA_DE < inicio && x.ICA_DATA_ATE > fim));

            //valida se não é nulo
            dynamic linhasDataInicio = listaColunaInicio != null ? listaColunaInicio : null;
            dynamic linhasDataTermino = listaColunaTermino != null ? listaColunaTermino : null;
            dynamic linhasDataInicio_Termino = listaColunaTermino_Inicio != null ? listaColunaTermino_Inicio : null;
            dynamic linhasSobrepondo = listaLinhasSobrepondo != null ? listaLinhasSobrepondo : null;

            //adiciona elas em um dicionário
            Dictionary<string, List<ItensCalendario>> linhasConflitantes = new Dictionary<string, List<ItensCalendario>>();
            List<ItensCalendario> linhas_com_inicio_conflitante = new List<ItensCalendario>();
            foreach (dynamic linha in linhasDataInicio)
            {
                ItensCalendario i = linha != null ? new ItensCalendario(
                    0,
                    linha.ICA_DATA_DE,
                    linha.ICA_DATA_ATE,
                    "Essa linha teve sua coluna de inicio modificada.",
                    linha.ICA_TIPO,
                    linha.URM_ID,
                    linha.URN_ID,
                    linha.ICA_LIMPESA_MAQUINA,
                    linha.CAL_ID,
                    linha.ICA_ID
                ) : null;

                linhas_com_inicio_conflitante.Add(i);
            }

            List<ItensCalendario> linhas_com_termino_conflitante = new List<ItensCalendario>();
            foreach (dynamic linha in linhasDataTermino)
            {
                ItensCalendario i = linha != null ? new ItensCalendario(
                    0,
                    linha.ICA_DATA_DE,
                    linha.ICA_DATA_ATE,
                    "Essa linha teve sua coluna de termino modificada.",
                    linha.ICA_TIPO,
                    linha.URM_ID,
                    linha.URN_ID,
                    linha.ICA_LIMPESA_MAQUINA,
                    linha.CAL_ID,
                    linha.ICA_ID
                ) : null;

                linhas_com_termino_conflitante.Add(i);
            }

            List<ItensCalendario> inicio_e_termino_conflitantes = new List<ItensCalendario>();
            foreach (dynamic linha in linhasDataInicio_Termino)
            {
                ItensCalendario i = linha != null ? new ItensCalendario(
                    0,
                    linha.ICA_DATA_DE,
                    linha.ICA_DATA_ATE,
                    "Essa linha teve sua coluna de inicio modificada.",
                    linha.ICA_TIPO,
                    linha.URM_ID,
                    linha.URN_ID,
                    linha.ICA_LIMPESA_MAQUINA,
                    linha.CAL_ID,
                    linha.ICA_ID
                ) : null;

                inicio_e_termino_conflitantes.Add(i);
            }

            List<ItensCalendario> linhas_sobrepondo = new List<ItensCalendario>();
            foreach (dynamic linha in linhasSobrepondo)
            {
                ItensCalendario i = linha != null ? new ItensCalendario(
                    0,
                    linha.ICA_DATA_DE,
                    linha.ICA_DATA_ATE,
                    "Essa linha teve sua coluna de inicio modificada.",
                    linha.ICA_TIPO,
                    linha.URM_ID,
                    linha.URN_ID,
                    linha.ICA_LIMPESA_MAQUINA,
                    linha.CAL_ID,
                    linha.ICA_ID
                ) : null;

                linhas_sobrepondo.Add(i);
            }


            //se a lista for diferente de nula, então, adiciona ela ao dicionário
            if (linhas_com_inicio_conflitante != null && linhas_com_inicio_conflitante.Count() > 0)
                linhasConflitantes.Add("conflitos_no_inicio", linhas_com_inicio_conflitante);
            if (linhas_com_termino_conflitante != null && linhas_com_termino_conflitante.Count() > 0)
                linhasConflitantes.Add("conflitos_no_termino", linhas_com_termino_conflitante);
            if (inicio_e_termino_conflitantes != null && inicio_e_termino_conflitantes.Count() > 0)
                linhasConflitantes.Add("conflitos_inicio_termino", inicio_e_termino_conflitantes);
            if (linhas_sobrepondo != null && linhas_sobrepondo.Count() > 0)
                linhasConflitantes.Add("conflitos_sobrepondo", linhas_sobrepondo);

            return linhasConflitantes;
        }

    }
}
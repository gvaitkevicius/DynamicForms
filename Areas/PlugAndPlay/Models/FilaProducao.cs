using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using DynamicForms.Models;
using DynamicForms.Util;
using DynamicForms.Context;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using DynamicForms.Controllers;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "FILA DE PRODUÇÃO")]
    public class FilaProducao
    {
        public FilaProducao()
        {

        }
        // DETERMINA A SEQUENCIA DE PRODUCAO POR EXEMPLO UM PRODUTO ACABADO QUE TEM UMA UNICA TRANSFORMACAO A SEQUENCIA SERA 99 (SEQUENCIA 99 DETERMINA ULTIMA PORCESSO ANTES DE VIRAR PRODUTO ACABADO)
        // PRODUTOS COM MAIS DE QUE UMA FAZE DE TRANSFORMACAO EXEMPLO  UMA CAIXA    PARA FAZER UMA CAIXA DEVEMOS FAZER UMA CHAPA PARA FAZER CHAPA DEVEMOS COMPRAR MATERIA PRIMA ENTAO A SEQUENCIA FICA
        // 01 PRODUCAO DA CHAPA 99 PRODUCAO DA CAIXA. OUTRO EXEMPLO PARA FAZER UMA CAIXA DEVEMOS PASSAR POR DUAS MAQUINAS PARA FINALIZAR O PRODUTO ACABADO FICARIA ASSIM
        // SEQUENCIA 01 FABRICACAO DA CHAPA
        // SEQUENCIA 02 FABRICA CAIXA
        // SEQUENCIA 99 GRANPEIA PARA FINALIZAR A PRODUCAO.
        // DETERMINA A ORDEM NA FILA  O OPERADOR DEVE SEGUIR ESTA ORDEM
        [TAB(Value = "PRINCIPAL")] [READ] [Display(Name = "ID")] [Required(ErrorMessage = "Campo FPR_ID requirido.")] public int FPR_ID { get; set; }
        [GRID] [TAB(Value = "PRINCIPAL")] [READ] [Display(Name = "PEDIDO")] [Required(ErrorMessage = "Campo ORD_ID requirido.")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [GRID] [TAB(Value = "PRINCIPAL")] [Display(Name = "MAQUINA")] [Required(ErrorMessage = "Campo ROT_MAQ_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo ROT_MAQ_ID")] public string ROT_MAQ_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRODUTO")] [Required(ErrorMessage = "Campo ROT_PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo ROT_PRO_ID")] public string ROT_PRO_ID { get; set; }
        //[TAB(Value = "PRINCIPAL")] [Display(Name = "IMG_LASTRO")] [MaxLength(50, ErrorMessage = "Maximode 50 caracteres, campo PRO_IMG_LASTRO")] public string PRO_IMG_LASTRO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ. TRANFORMACAO")] [Required(ErrorMessage = "Campo ROT_SEQ_TRANFORMACAO requirido.")] public int ROT_SEQ_TRANFORMACAO { get; set; }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID ORIGEM")] public int? FPR_ID_ORIGEM { get; set; }
        // EDITAVEL APENAS PARA RETRABALHO
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE")] [Required(ErrorMessage = "Campo FPR_QUANTIDADE_PREVISTA requirido.")] public double FPR_QUANTIDADE_PREVISTA { get; set; }
        // ADITAVEL PARA TODOS 
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PREVISÃO MATERIA PRIMA")] [Required(ErrorMessage = "Campo FPR_PREVISAO_MATERIA_PRIMA requirido.")] public DateTime FPR_PREVISAO_MATERIA_PRIMA { get; set; }
        [TextArea] [TAB(Value = "PRINCIPAL")] [Display(Name = "OBS PRODUCAO")] [MaxLength(4000, ErrorMessage = "Maximode * caracteres, campo FPR_OBS_PRODUCAO")] public string FPR_OBS_PRODUCAO { get; set; }
        [SEARCH_NOT_FK(namespaceOfForeignClass = "DynamicForms.Areas.PlugAndPlay.Models.Maquina")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID MAQUINA MANUAL")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MAQ_ID_MANUAL")] public string MAQ_ID_MANUAL { get; set; }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "GRUPO PRODUTIVO")] public double? FPR_GRUPO_PRODUTIVO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "GRUPO PRODUTIVO MANUAL")] public double? FPR_GRUPO_PRODUTIVO_MANUAL { get; set; }
        [TAB(Value = "PRINCIPAL")] [READ] [Display(Name = "SEQ REPETICAO")] [Required(ErrorMessage = "Campo FPR_SEQ_REPETICAO requirido.")] public int FPR_SEQ_REPETICAO { get; set; }
        [TAB(Value = "PRINCIPAL")]  [Display(Name = "INICIO PREVISTA")] [Required(ErrorMessage = "Campo FPR_DATA_INICIO_PREVISTA requirido.")] public DateTime FPR_DATA_INICIO_PREVISTA { get; set; }
        [TAB(Value = "PRINCIPAL")] [READ] [Display(Name = "FIM PREVISTA")] [Required(ErrorMessage = "Campo FPR_DATA_FIM_PREVISTA requirido.")] public DateTime FPR_DATA_FIM_PREVISTA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA ENTREGA")] public DateTime FPR_DATA_ENTREGA { get; set; }
        [TAB(Value = "PRINCIPAL")] [READ] [Display(Name = "NECESSIDADE INICIO PRODUCAO")] public DateTime FPR_DATA_NECESSIDADE_INICIO_PRODUCAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [READ] [Display(Name = "NECESSIDADE FIM PRODUCAO")] public DateTime FPR_DATA_NECESSIDADE_FIM_PRODUCAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [READ] [Display(Name = "POSIÇÃO NA FILA")] public double? FPR_ORDEM_NA_FILA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD. EQUIPE")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo COD. EQUIPE")] public string EQU_ID { get; set; }

        //[Combobox(Description = "Não priorizar", Value = "0")]
        //[Combobox(Description = "Ultrapassa Congeladas", Value = "1")]
        //[Combobox(Description = "Logo após congeladas ", Value = "2")]
        //[Combobox(Description = "Encaixar no grupo produtivo atual ", Value = "3")]
        //[Combobox(Description = "Primeira do proximo grupo produtivo ", Value = "4")]
        //[Combobox(Description = "Encaixar no proximo grupo produtivo ", Value = "5")]
        [Combobox(Description = "NÃO PRIORIZAR", ValueInt = 0)]
        [Combobox(Description = "ULTRAPASSA CONGELADAS", ValueInt = 1)]
        [Combobox(Description = "LOGO APÓS CONGELADAS", ValueInt = 2)]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRIORIDADE")] public int FPR_PRIORIDADE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "HIERARQUIA CALCULADA")] public int? FPR_HIERARQUIA_SEQ_TRANSFORMACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA DE EMISSÃO")] public DateTime? FPR_EMISSAO { get; set; }


        [Combobox(Description = "ABERTO ", Value = "  ")]
        [Combobox(Description = "ENC. PELA REGRA", Value = "ER")]
        [Combobox(Description = "ENC. PRODUÇÃO TOTAL", Value = "ET")]
        [Combobox(Description = "ENC. PARCIALMENTE", Value = "EP")]
        [Combobox(Description = "ENC. POR GOTO", Value = "EG")]
        [Combobox(Description = "ENC. POR DIVISÃO", Value = "ED")]
        [Combobox(Description = "MATERIA PRIMA ALTERADA MANUALMENTE", Value = "M ")]
        [Combobox(Description = "MATERIA PRIMA FIRME", Value = "F ")]
        [Combobox(Description = "ENCERRADO NORMALMENTE", Value = "EN")]
        [Combobox(Description = "ENCERRADA", Value = "E")]

        [Combobox(Description = "ELIMINADO POR RESÍDUO", Value = "EL")]
        [Combobox(Description = "PRODUZINDO", Value = "AP")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "STATUS")] [MaxLength(2, ErrorMessage = "Maximode 2 caracteres, campo FPR_STATUS")] public string FPR_STATUS { get; set; }
        [TAB(Value = "REALIZADO")] [READ] [Display(Name = "META_SETUP")] public double? FPR_META_SETUP { get; set; }
        [TAB(Value = "REALIZADO")] [READ] [Display(Name = "QTD_PRODUZIDA")] public double? FPR_QTD_PRODUZIDA { get; set; }
        [TAB(Value = "REALIZADO")] [READ] [Display(Name = "QTD_PERFORMANCE")] public double? FPR_QTD_PERFORMANCE { get; set; }
        [TAB(Value = "REALIZADO")] [READ] [Display(Name = "QTD_SETUP")] public double? FPR_QTD_SETUP { get; set; }
        [TAB(Value = "REALIZADO")] [READ] [Display(Name = "QTD_RESTANTE")] public double? FPR_QTD_RESTANTE { get; set; }
        [TAB(Value = "REALIZADO")] [READ] [Display(Name = "TEMPO_DECORRIDO_SETUP")] public double? FPR_TEMPO_DECORRIDO_SETUP { get; set; }
        [TAB(Value = "REALIZADO")] [READ] [Display(Name = "TEMPO_DECORRIDO_SETUPA")] public double? FPR_TEMPO_DECORRIDO_SETUPA { get; set; }
        [TAB(Value = "REALIZADO")] [READ] [Display(Name = "TEMPO_DECORRIDO_PERFORMANC")] public double? FPR_TEMPO_DECORRIDO_PERFORMANC { get; set; }
        [TAB(Value = "REALIZADO")] [READ] [Display(Name = "TEMPO_DECO_PEQUENA_PARADA")] public double? FPR_TEMPO_DECO_PEQUENA_PARADA { get; set; }
        [TAB(Value = "REALIZADO")] [READ] [Display(Name = "TEMPO_TEORICO_PERFORMANCE")] public double? FPR_TEMPO_TEORICO_PERFORMANCE { get; set; }
        [TAB(Value = "REALIZADO")] [READ] [Display(Name = "TEMPO_RESTANTE_PERFORMANC")] public double? FPR_TEMPO_RESTANTE_PERFORMANC { get; set; }
        [TAB(Value = "REALIZADO")] [READ] [Display(Name = "VELOCIDADE_P_ATINGIR_META")] public double? FPR_VELOCIDADE_P_ATINGIR_META { get; set; }
        [TAB(Value = "REALIZADO")] [READ] [Display(Name = "VELO_ATU_PC_SEGUNDO")] public double? FPR_VELO_ATU_PC_SEGUNDO { get; set; }
        [TAB(Value = "REALIZADO")] [READ] [Display(Name = "PERFORMANCE_PROJETADA")] public double? FPR_PERFORMANCE_PROJETADA { get; set; }
        [TAB(Value = "REALIZADO")] [READ] [Display(Name = "TEMPO_RESTANTE_TOTAL")] public double? FPR_TEMPO_RESTANTE_TOTAL { get; set; }
        [TAB(Value = "REALIZADO")] [READ] [Display(Name = "PRODUZINDO")] public int? FPR_PRODUZINDO { get; set; }

        [TAB(Value = "OUTROS")] [READ] [Display(Name = "FIM_PREVISTO_ATUAL")] public DateTime FPR_FIM_PREVISTO_ATUAL { get; set; }
        [TAB(Value = "OUTROS")] [READ] [Display(Name = "TRUNCADO")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo FPR_TRUNCADO")] public string FPR_TRUNCADO { get; set; }
        [TAB(Value = "OUTROS")] [READ] [Display(Name = "DATA_TRUNC_INI")] public DateTime FPR_DATA_TRUNC_INI { get; set; }
        [TAB(Value = "OUTROS")] [READ] [Display(Name = "DATA_TRUNC_FIM")] public DateTime FPR_DATA_TRUNC_FIM { get; set; }

        [TAB(Value = "OUTROS")] [READ] [Display(Name = "DATA_FIM_MAXIMA")] [Required(ErrorMessage = "Campo FPR_DATA_FIM_MAXIMA requirido.")] public DateTime FPR_DATA_FIM_MAXIMA { get; set; }
        [TAB(Value = "OUTROS")] [READ] [Display(Name = "ID_INTEGRACAO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo FPR_ID_INTEGRACAO")] public string FPR_ID_INTEGRACAO { get; set; }
        [TAB(Value = "OUTROS")] [READ] [Display(Name = "COR_FILA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo FPR_COR_FILA")] public string FPR_COR_FILA { get; set; }
        [TAB(Value = "OUTROS")] [READ] [Display(Name = "COR_BICO1")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo FPR_COR_BICO1")] public string FPR_COR_BICO1 { get; set; }
        [TAB(Value = "OUTROS")] [READ] [Display(Name = "COR_BICO2")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo FPR_COR_BICO2")] public string FPR_COR_BICO2 { get; set; }
        [TAB(Value = "OUTROS")] [READ] [Display(Name = "COR_BICO3")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo FPR_COR_BICO3")] public string FPR_COR_BICO3 { get; set; }
        [TAB(Value = "OUTROS")] [READ] [Display(Name = "COR_BICO4")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo FPR_COR_BICO4")] public string FPR_COR_BICO4 { get; set; }
        [TAB(Value = "OUTROS")] [READ] [Display(Name = "COR_BICO5")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo FPR_COR_BICO5")] public string FPR_COR_BICO5 { get; set; }
        [TAB(Value = "OUTROS")] [READ] [Display(Name = "ORD_ID_REPROGRAMADO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo FPR_ORD_ID_REPROGRAMADO")] public string FPR_ORD_ID_REPROGRAMADO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "MOTIVO_PULA_FILA")] [MaxLength(140, ErrorMessage = "Maximode 140 caracteres, campo FPR_MOTIVO_PULA_FILA")] public string FPR_MOTIVO_PULA_FILA { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "ID")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo OCO_ID")] public string OCO_ID { get; set; }

        public virtual Produto Produto { get; set; }
        public virtual Maquina Maquina { get; set; }
        public virtual Roteiro Roteiro { get; set; }
        public virtual Order Order { get; set; }
        public virtual OcorrenciaPularOrdemFila OcorrenciaPularOrdemFila { get; set; }

        /// <summary>
        /// Esta propriedade foi criada para representar o estado do objeto
        /// insert, update, delete ou unchanged 
        /// </summary>
        [NotMapped]
        public string PlayAction { get; set; }

        /// <summary>
        /// Deve seguir a seguinte convecão: NameProperty:MsgErro;NameProperty:MsgErro; ...
        /// Representa os erros de validacao deste objeto
        /// </summary>
        [NotMapped]
        public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (var item in objects)
                {

                    if (item.GetType().Name == "FilaProducao")
                    {
                        //Objeto retornado da View
                        FilaProducao _FilaProd = (FilaProducao)item;
                        //Setando action do objeto da view como OK para que seja ignorado pela  funçao UpdateData
                        //Validaçoes --
                        if (_FilaProd.PlayAction.ToLower() == "update")
                        {
                            // desabilitado antigamente al mudar a materia prima o sistema alterava o status mas implementamos o conceito do F materia prima firme agora exigimos que alem de alterar a data se altere o status 
                            object cloneDb = cloneObjeto.GetClone(item);
                            if (cloneObjeto.getChangedPoperties(item, cloneDb).Contains(nameof(FPR_PREVISAO_MATERIA_PRIMA)))
                            {
                                if (_FilaProd.ROT_MAQ_ID.Contains("SRV_COMPRAS_01") && !_FilaProd.FPR_STATUS.Contains("F"))
                                {
                                    _FilaProd.FPR_STATUS = "M";
                                }
                            }
                        }
                        if (_FilaProd.PlayAction.ToLower() == "insert")
                        {
                            int? ordTipo = db.Order.AsNoTracking().Where(x => x.ORD_ID == _FilaProd.ORD_ID)
                                            .Select(x => x.ORD_TIPO).FirstOrDefault();

                            if (ordTipo != null && (ordTipo == 4 || ordTipo == 5))
                            {// É um pedido de retrabalho
                                int seqRepeticao = db.FilaProducao.AsNoTracking()
                                                    .Where(x => x.ORD_ID == _FilaProd.ORD_ID && x.ROT_SEQ_TRANFORMACAO == _FilaProd.ROT_SEQ_TRANFORMACAO)
                                                    .OrderByDescending(x => x.FPR_SEQ_REPETICAO).Select(x => x.FPR_SEQ_REPETICAO).FirstOrDefault();

                                _FilaProd.ROT_MAQ_ID = "PLAYSIS";
                                _FilaProd.FPR_SEQ_REPETICAO = seqRepeticao + 1;
                                _FilaProd.FPR_STATUS = "";
                            }
                        }
                    }
                }
            }
            return true;
        }

        public bool DividirOP(List<object> objects, ref List<LogPlay> Logs)
        {
            foreach (var item in objects)
            {
                if (item.GetType().Name == "FilaProducao")
                {
                    FilaProducao fp = (FilaProducao)item;
                    List<string> parametros = new List<string>
                    {
                        "Qtd. de OPs#int",
                        "Intervalo (em dias) entre as OPs#int"
                    };
                    string strParametros = JsonConvert.SerializeObject(parametros);
                    Logs.Add(new LogPlay(this.ToString() + "#EfetuarDivisaoOP", "PROTOCOLO", "ListarParametros", strParametros, fp.FPR_ID.ToString()));
                }
            }
            return true;
        }

        [HIDDEN]
        public bool EfetuarDivisaoOP(object objects, ref List<LogPlay> Logs)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                dynamic aux = objects;
                int fprId = int.Parse(aux.primaryKey);
                List<string> parametros = aux.parametros;

                int qtdNovasOps = 0;
                int.TryParse(parametros[0], out qtdNovasOps);

                int intervaloEntreOps = 0;
                int.TryParse(parametros[1], out intervaloEntreOps);

                if (qtdNovasOps <= 0)
                    Logs.Add(new LogPlay() { MsgErro = "A Quantidade de novas OPs deve ser maior que zero.", Status = "ERRO" });
                if (intervaloEntreOps <= 0)
                    Logs.Add(new LogPlay() { MsgErro = "O intervalo entre as novas OPs deve maior que zero.", Status = "ERRO" });

                if (Logs.Count > 0)
                    return true;

                List<List<object>> objetos = new List<List<object>>();
                List<object> filasDeProducao = new List<object>();
                FilaProducao opParaEncerrar = db.FilaProducao.AsNoTracking().Where(x => x.FPR_ID == fprId).FirstOrDefault();

                if (opParaEncerrar.FPR_STATUS.StartsWith("E"))
                {
                    Logs.Add(new LogPlay() { MsgErro = "Não é possível dividir uma OP que já está encerrada.", Status = "ERRO" });
                    return true;
                }

                int seqRepeticao;

                if (opParaEncerrar.FPR_ID_ORIGEM > 0)
                {// é uma OP que já foi dividida
                    var opsDivididasParaEncerrar = db.FilaProducao.Where(x => x.FPR_ID_ORIGEM == opParaEncerrar.FPR_ID_ORIGEM &&
                        x.FPR_DATA_ENTREGA > opParaEncerrar.FPR_DATA_ENTREGA).ToList();
                    bool existeMovimentacao = opsDivididasParaEncerrar.Any(x => x.FPR_STATUS.StartsWith("E"));
                    if (existeMovimentacao)
                    {
                        Logs.Add(new LogPlay() { MsgErro = "Não é possível dividir esta OP, existem outras sequências de repetição já encerradas.", Status = "ERRO" });
                        return true;
                    }
                    else
                    {
                        opsDivididasParaEncerrar.ForEach(x =>
                        {
                            x.PlayAction = "delete";
                        });
                        filasDeProducao.AddRange(opsDivididasParaEncerrar);

                        seqRepeticao = opParaEncerrar.FPR_SEQ_REPETICAO + 1;
                    }
                }
                else
                {
                    seqRepeticao = db.FilaProducao.AsNoTracking()
                                        .Where(x => x.ORD_ID == opParaEncerrar.ORD_ID && x.ROT_SEQ_TRANFORMACAO == opParaEncerrar.ROT_SEQ_TRANFORMACAO)
                                        .Max(x => x.FPR_SEQ_REPETICAO);
                    seqRepeticao += 1;
                }

                double[] quantidadesProduzir = new double[qtdNovasOps];
                int resto = (int)opParaEncerrar.FPR_QUANTIDADE_PREVISTA % qtdNovasOps;
                int auxQtd = (int)opParaEncerrar.FPR_QUANTIDADE_PREVISTA / qtdNovasOps;

                quantidadesProduzir[0] = auxQtd + resto;
                for (int i = 1; i < qtdNovasOps; i++)
                {
                    quantidadesProduzir[i] = auxQtd;
                }

                Order pedido = db.Order.Where(x => x.ORD_ID == opParaEncerrar.ORD_ID)
                    .Select(x => new Order
                    {
                        ORD_DATA_ENTREGA_DE = x.ORD_DATA_ENTREGA_DE
                    }).FirstOrDefault();
                DateTime inicioOp = pedido.ORD_DATA_ENTREGA_DE;

                DateTime dataFimQuery = inicioOp.AddDays(qtdNovasOps * 3);

                HashSet<int> calendarioIds = db.V_ROTEIROS_POSSIVEIS_DO_PRODUTO.Where(x => x.PRO_ID == opParaEncerrar.ROT_PRO_ID).Select(x => x.CAL_ID).ToHashSet();
                List<ItensCalendario> itensCalendario = db.ItensCalendario
                    .Where(x => calendarioIds.Contains(x.CAL_ID) && x.ICA_DATA_DE >= inicioOp && x.ICA_DATA_ATE <= dataFimQuery).OrderBy(x => x.ICA_DATA_DE).ToList();

                filasDeProducao.Add(opParaEncerrar);
                for (int i = 0; i < qtdNovasOps; i++)
                {
                    DateTime dataDe = new DateTime(inicioOp.Year, inicioOp.Month, inicioOp.Day, 0, 0, 0);
                    DateTime dataAte = new DateTime(inicioOp.Year, inicioOp.Month, inicioOp.Day, 23, 59, 59);
                    DateTime proximaData = UtilPlay.ProximaDataProdutiva(true, dataDe, dataAte, itensCalendario);
                    DateTime dataEntrega = new DateTime(proximaData.Year, proximaData.Month, proximaData.Day, 0, 0, 0);

                    FilaProducao nova = (FilaProducao)opParaEncerrar.MemberwiseClone();
                    nova.FPR_ID = 0;
                    nova.FPR_ID_ORIGEM = opParaEncerrar.FPR_ID;
                    nova.FPR_DATA_ENTREGA = dataEntrega;
                    nova.FPR_QUANTIDADE_PREVISTA = quantidadesProduzir[i];
                    nova.FPR_SEQ_REPETICAO = seqRepeticao++;
                    nova.PlayAction = "insert";
                    filasDeProducao.Add(nova);

                    inicioOp = dataEntrega.AddDays(intervaloEntreOps);
                }
                opParaEncerrar.FPR_STATUS = "ED";
                opParaEncerrar.PlayAction = "update";
                objetos.Add(filasDeProducao);
                MasterController mc = new MasterController();
                List<LogPlay> logsUpdateData = mc.UpdateData(objetos, 0, true);
                foreach (LogPlay log in logsUpdateData)
                {
                    log.MsgErro = $"Ordem de Produção dividida com sucesso.";
                }
                Logs.AddRange(logsUpdateData);



            }
            return true;
        }

        [HIDDEN]
        public IEnumerable<FilaProducao> AtualizarDatasDasOPs(Order order, JSgi db)
        {
            List<FilaProducao> ordensProducao = db.FilaProducao.AsNoTracking().Where(f => f.ORD_ID == order.ORD_ID).ToList();

            foreach (var op in ordensProducao)
            {
                op.FPR_DATA_INICIO_PREVISTA = order.ORD_DATA_ENTREGA_DE;
                op.FPR_DATA_FIM_PREVISTA = order.ORD_DATA_ENTREGA_DE;
                op.PlayAction = "update";
            }

            return ordensProducao;
        }

        /// <summary>
        /// Retorna verdadeiro caso esta OP tenha passado pelo processo de divisão
        /// </summary>
        /// <returns></returns>
        [HIDDEN]
        public bool IsDivided(JSgi db)
        {
            if (db != null)
            {
                var Db_OpDividida = db.ViewFilaProducao.AsNoTracking().Where(o => o.OrdId.Equals(this.ORD_ID) && o.PaProId.Equals(this.ROT_PRO_ID) && o.RotMaqId.Equals(this.ROT_MAQ_ID) && o.RotSeqTransformacao == this.ROT_SEQ_TRANFORMACAO && o.FprSeqRepeticao == this.FPR_SEQ_REPETICAO).FirstOrDefault();
                if (Db_OpDividida != null)
                {
                    return Db_OpDividida.FPR_ID_ORIGEM > 0;
                }
            }
            return false;
        }
        [HIDDEN]
        public bool IsLastSequence(JSgi db)
        {
            if (db != null)
            {
                var Db_UltimaSeq = db.ViewFilaProducao.Where(o => o.OrdId.Equals(this.ORD_ID) && o.PaProId.Equals(this.ROT_PRO_ID) && o.FprSeqRepeticao == this.FPR_SEQ_REPETICAO).Max(o => o.RotSeqTransformacao);
                if (Db_UltimaSeq == this.ROT_SEQ_TRANFORMACAO)
                {
                    return true;
                }
            }
            return false;
        }

    }
    public class V_PEDIDOS_PARCIALMENTE_PRODUZIDOS
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "")] [Required(ErrorMessage = "Campo FASE requirido.")] [MaxLength(15, ErrorMessage = "Maximode 15 caracteres, campo FASE")] public string FASE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "MAQ_ID")] [Required(ErrorMessage = "Campo ROT_MAQ_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo ROT_MAQ_ID")] public string ROT_MAQ_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo ORD_ID requirido.")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRO_ID")] [Required(ErrorMessage = "Campo ROT_PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo ROT_PRO_ID")] public string ROT_PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ_TRANFORMACAO")] [Required(ErrorMessage = "Campo ROT_SEQ_TRANFORMACAO requirido.")] public int ROT_SEQ_TRANFORMACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ_REPETICAO")] [Required(ErrorMessage = "Campo FPR_SEQ_REPETICAO requirido.")] public int FPR_SEQ_REPETICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE_PREVISTA")] [Required(ErrorMessage = "Campo FPR_QUANTIDADE_PREVISTA requirido.")] public double FPR_QUANTIDADE_PREVISTA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TOLERANCIA_MENOS")] public double? ORD_TOLERANCIA_MENOS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "_PRODUCAO")] public DateTime DATA_PRODUCAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "O_A_PRODUZIR")] public double? SALDO_A_PRODUZIR { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) { return true; }
    }
    public class LogScheduleProducao
    {

        public string ORD_ID { get; set; }
        public string PRO_ID { get; set; }
        public string ROT_SEQ_TRANSFORMACAO { get; set; }

        public double CUSTO_DIREITA_LIVRE { get; set; }// CUSTO DA COLOCAÇÃO A DIREITA LIVRE 
        public string ORD_ID_DIREITA_LIVRE { get; set; }

        public string ORD_ID_DIREITA_NAO_LIVRE { get; set; }
        public double CUSTO_DIREITA_NAO_LIVRE { get; set; }
        public string ORD_ID_ESQUERDA { get; set; }
        public double CUSTO_ESQUERDA { get; set; }
        public int VENCEDOR { get; set; }// 0 FALSE 1 TRUE
        //PENDENCIA IMPLEMENTAR LISTA DE T_CUSTO_ENTRE_OPS
        public double TEMPO_SETUP_ESQUERDA { get; set; }

        /// <summary>
        /// Esta propriedade foi criada para representar o estado do objeto
        /// insert, update, delete ou unchanged 
        /// </summary>
        [NotMapped]
        public string PlayAction { get; set; }

        /// <summary>
        /// Deve seguir a seguinte convecão: NameProperty:MsgErro;NameProperty:MsgErro; ...
        /// Representa os erros de validacao deste objeto
        /// </summary>
        [NotMapped]
        public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

    }
    public class EstruturaDoSchedule
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_VALIDADE")] [Required(ErrorMessage = "Campo EST_DATA_VALIDADE requirido.")] public DateTime EST_DATA_VALIDADE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID_PRODUTO")] [Required(ErrorMessage = "Campo PRO_ID_PRODUTO requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID_PRODUTO")] public string PRO_ID_PRODUTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID_COMPONENTE")] [Required(ErrorMessage = "Campo PRO_ID_COMPONENTE requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID_COMPONENTE")] public string PRO_ID_COMPONENTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANT")] [Required(ErrorMessage = "Campo EST_QUANT requirido.")] public double EST_QUANT { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_INCLUSAO")] [Required(ErrorMessage = "Campo EST_DATA_INCLUSAO requirido.")] public DateTime EST_DATA_INCLUSAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "BASE_PRODUCAO")] [Required(ErrorMessage = "Campo EST_BASE_PRODUCAO requirido.")] public double EST_BASE_PRODUCAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO_REQUISICAO")] [Required(ErrorMessage = "Campo EST_TIPO_REQUISICAO requirido.")] [MaxLength(2, ErrorMessage = "Maximode 2 caracteres, campo EST_TIPO_REQUISICAO")] public string EST_TIPO_REQUISICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO_P")] [Required(ErrorMessage = "Campo GRP_TIPO_P requirido.")] public double GRP_TIPO_P { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID_P")] [Required(ErrorMessage = "Campo GRP_ID_P requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo GRP_ID_P")] public string GRP_ID_P { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO_C")] [Required(ErrorMessage = "Campo GRP_TIPO_C requirido.")] public double GRP_TIPO_C { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID_C")] [Required(ErrorMessage = "Campo GRP_ID_C requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo GRP_ID_C")] public string GRP_ID_C { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ESCALA_COR")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ESCALA_COR")] public string PRO_ESCALA_COR { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SUB_ESCALA_COR")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo SUB_PRO_ESCALA_COR")] public string PRO_SUB_ESCALA_COR { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CUSTO_SUBIDA_ESCALA_COR")] public double? PRO_CUSTO_SUBIDA_ESCALA_COR { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CUSTO_DECIDA_ESCALA_COR")] public double? PRO_CUSTO_DECIDA_ESCALA_COR { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COMPRIMENTO_PECA")] public double? PRO_COMPRIMENTO_PECA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LARGURA_PECA")] public double? PRO_LARGURA_PECA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ALTURA_PECA")] public double? PRO_ALTURA_PECA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CODIGO_DE_EXCECAO")] [Required(ErrorMessage = "Campo EST_CODIGO_DE_EXCECAO requirido.")] [MaxLength(3000, ErrorMessage = "Maximode * caracteres, campo EST_CODIGO_DE_EXCECAO")] public string EST_CODIGO_DE_EXCECAO { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
    }
    public class SequenciaBicosTintas
    {
        public SequenciaBicosTintas()
        {
            eCorBico1 = null;
            eCorBico2 = null;
            eCorBico3 = null;
            eCorBico4 = null;
            eCorBico5 = null;

            CorBico1 = "";
            CorBico2 = "";
            CorBico3 = "";
            CorBico4 = "";
            CorBico5 = "";
        }


        public EstruturaDoSchedule eCorBico1 { get; set; }
        public EstruturaDoSchedule eCorBico2 { get; set; }
        public EstruturaDoSchedule eCorBico3 { get; set; }
        public EstruturaDoSchedule eCorBico4 { get; set; }
        public EstruturaDoSchedule eCorBico5 { get; set; }

        public string CorBico1 { get; set; }
        public string CorBico2 { get; set; }
        public string CorBico3 { get; set; }
        public string CorBico4 { get; set; }
        public string CorBico5 { get; set; }

        /// <summary>
        /// Esta propriedade foi criada para representar o estado do objeto
        /// insert, update, delete ou unchanged 
        /// </summary>
        [NotMapped]
        public string PlayAction { get; set; }

        /// <summary>
        /// Deve seguir a seguinte convecão: NameProperty:MsgErro;NameProperty:MsgErro; ...
        /// Representa os erros de validacao deste objeto
        /// </summary>
        [NotMapped]
        public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
    }
    public class FilaDoSchedule
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRIORIDADE")] public int? FPR_PRIORIDADE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "STATUS")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo ORD_STATUS")] public string ORD_STATUS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "STATUS")] [MaxLength(2, ErrorMessage = "Maximode 2 caracteres, campo FPR_STATUS")] public string FPR_STATUS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "inaId")] [Required(ErrorMessage = "Campo MaquinaId requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MaquinaId")] public string MaquinaId { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "rId")] [Required(ErrorMessage = "Campo OrderId requirido.")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo OrderId")] public string OrderId { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRO_ID")] [Required(ErrorMessage = "Campo ORD_PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo ORD_PRO_ID")] public string ORD_PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "utoId")] [Required(ErrorMessage = "Campo ProdutoId requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo ProdutoId")] public string ProdutoId { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "enciaTransformacao")] [Required(ErrorMessage = "Campo SequenciaTransformacao requirido.")] public int SequenciaTransformacao { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "MAX SEQ. TRANFORMACAO")] public int MAX_ROT_SEQ_TRANFORMACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "enciaRepeticao")] [Required(ErrorMessage = "Campo SequenciaRepeticao requirido.")] public int SequenciaRepeticao { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "inaIdManual")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MaquinaIdManual")] public string MaquinaIdManual { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "InicioPrevista")] [Required(ErrorMessage = "Campo DataInicioPrevista requirido.")] public DateTime DataInicioPrevista { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FimPrevista")] [Required(ErrorMessage = "Campo DataFimPrevista requirido.")] public DateTime DataFimPrevista { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FimMaxima")] [Required(ErrorMessage = "Campo DataFimMaxima requirido.")] public DateTime DataFimMaxima { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "isaoMateriaPrima")] [Required(ErrorMessage = "Campo PrevisaoMateriaPrima requirido.")] public DateTime PrevisaoMateriaPrima { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "rvacaoProducao")] [Required(ErrorMessage = "Campo ObservacaoProducao requirido.")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo ObservacaoProducao")] public string ObservacaoProducao { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "tidadePrevista")] [Required(ErrorMessage = "Campo QuantidadePrevista requirido.")] public double QuantidadePrevista { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "us")] [Required(ErrorMessage = "Campo Status requirido.")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo Status")] public string Status { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "uzindo")] [Required(ErrorMessage = "Campo Produzindo requirido.")] public int Produzindo { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "tegracao")] [Required(ErrorMessage = "Campo IdIntegracao requirido.")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo IdIntegracao")] public string IdIntegracao { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "tidadeProduzida")] public double? QuantidadeProduzida { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "tidadeRestante")] public double? QuantidadeRestante { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "oRestanteTotal")] public double? TempoRestanteTotal { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_ENTREGA_DE")] public DateTime ORD_DATA_ENTREGA_DE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_ENTREGA_ATE")] public DateTime ORD_DATA_ENTREGA_ATE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ATRAZADO_UTILIZA_EMBARQUE")] public string ATRAZADO_UTILIZA_EMBARQUE { get; set; }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "TRANSLADO")] [Required(ErrorMessage = "Campo CLI_TRANSLADO requirido.")] public double CLI_TRANSLADO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "oProducao")] public double? TempoProducao { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ormance")] [Required(ErrorMessage = "Campo Performance requirido.")] public double Performance { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "oSetup")] [Required(ErrorMessage = "Campo TempoSetup requirido.")] public double TempoSetup { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "oSetupAjuste")] [Required(ErrorMessage = "Campo TempoSetupAjuste requirido.")] public double TempoSetupAjuste { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "sPorPulso")] [Required(ErrorMessage = "Campo PecasPorPulso requirido.")] public double PecasPorPulso { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ARQUIA_SEQ_TRANSFORMACAO")] [Required(ErrorMessage = "Campo HIERARQUIA_SEQ_TRANSFORMACAO requirido.")] public double HIERARQUIA_SEQ_TRANSFORMACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "IA_CUSTO")] [Required(ErrorMessage = "Campo AVALIA_CUSTO requirido.")] public int AVALIA_CUSTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "cado")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo Truncado")] public string Truncado { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "InicioTrunc")] public DateTime DataInicioTrunc { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FimTrunc")] public DateTime DataFimTrunc { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "mDaFila")] public double? OrdemDaFila { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "GRP_PERFORMANCE_METRO_LINEAR_POR_SEGUNDO")] public double? GRP_PERFORMANCE_METRO_LINEAR_POR_SEGUNDO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "")] [Required(ErrorMessage = "Campo Id requirido.")] public int Id { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LOTE_PILOTO")] public int? ORD_LOTE_PILOTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "INICIO_GRUPO_PRODUTIVO")] public DateTime FPR_INICIO_GRUPO_PRODUTIVO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FIM_GRUPO_PRODUTIVO")] public DateTime FPR_FIM_GRUPO_PRODUTIVO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "HoraNecessidadeInicioProducao")] public DateTime DataHoraNecessidadeInicioProducao { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "HoraNecessidadeFimProducao")] public DateTime DataHoraNecessidadeFimProducao { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "oDecorridoSetup")] public double? TempoDecorridoSetup { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "oDecorridoSetupAjuste")] public double? TempoDecorridoSetupAjuste { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "oDecorridoPerformacace")] public double? TempoDecorridoPerformacace { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "tidadePerformace")] public double? QuantidadePerformace { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "tidadeSetup")] public double? QuantidadeSetup { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "oTeoricoPerformace")] public double? TempoTeoricoPerformace { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "oRestantePerformace")] public double? TempoRestantePerformace { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "cidadeAtingirMeta")] public double? VelocidadeAtingirMeta { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "AtuPcSegundo")] public double? VeloAtuPcSegundo { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ormaceProjetada")] public double? PerformaceProjetada { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "oDecorridoPequenasParadas")] public double? TempoDecorridoPequenasParadas { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "adaEmMaquina")] public int? AlocadaEmMaquina { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "GRUPO_PRODUTIVO")] public double? FPR_GRUPO_PRODUTIVO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "INICIO_JANELA_EMBARQUE")] [Required(ErrorMessage = "Campo CAR_INICIO_JANELA_EMBARQUE requirido.")] public DateTime CAR_INICIO_JANELA_EMBARQUE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FIM_JANELA_EMBARQUE")] [Required(ErrorMessage = "Campo CAR_FIM_JANELA_EMBARQUE requirido.")] public DateTime CAR_FIM_JANELA_EMBARQUE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "RQUE_ALVO")] [Required(ErrorMessage = "Campo EMBARQUE_ALVO requirido.")] public DateTime EMBARQUE_ALVO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "EXIGENTE_NA_IMPRESSAO")] public int? CLI_EXIGENTE_NA_IMPRESSAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COR_BICO1")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo FPR_COR_BICO1")] public string FPR_COR_BICO1 { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COR_BICO2")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo FPR_COR_BICO2")] public string FPR_COR_BICO2 { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COR_BICO3")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo FPR_COR_BICO3")] public string FPR_COR_BICO3 { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COR_BICO4")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo FPR_COR_BICO4")] public string FPR_COR_BICO4 { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COR_BICO5")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo FPR_COR_BICO5")] public string FPR_COR_BICO5 { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo GRP_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo GRP_ID")] public string GRP_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PAP_ONDA")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo GRP_PAP_ONDA")] public string GRP_PAP_ONDA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO")] public int? ORD_TIPO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PREVISAO_MATERIA_PRIMA")] [Required(ErrorMessage = "Campo FPR_PREVISAO_MATERIA_PRIMA requirido.")] public DateTime FPR_PREVISAO_MATERIA_PRIMA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO")] public double? GRP_TIPO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID_ORIGEM")] public int? FPR_ID_ORIGEM { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_ENTREGA")] public DateTime FPR_DATA_ENTREGA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo EQU_ID")] public string EQU_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COMPRIMENTO_PECA")] public double? PRO_COMPRIMENTO_PECA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LARGURA_PECA")] public double? PRO_LARGURA_PECA { get; set; }
        //[TAB(Value = "PRINCIPAL")] [Display(Name = "VIR_M3_UNITARIO")] public double? VIR_M3_UNITARIO { get; set; }
        //[TAB(Value = "PRINCIPAL")] [Display(Name = "VIR_M3_UE")] public double? VIR_M3_UE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORD_M2_UNITARIO")] public double? ORD_M2_UNITARIO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORD_PESO_UNITARIO")] public double? ORD_PESO_UNITARIO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORD_PESO_UNITARIO_BRUTO")] public double? ORD_PESO_UNITARIO_BRUTO { get; set; }

        // CAMPOS APENAS VIRTUAIS 
        [NotMapped] public string GrupoMaquinaId { get; set; }
        [NotMapped] public int? IDCalculo { get; set; } // guarda sequencia original que respeita OP e sequencia de tranaformaçao assim fica facil de saber quem é susesora e antecessora quando for mesma OP
        [NotMapped] public int? IDFilaOrdenada { get; set; } // Guarda id da OP dentra da lista Ordenada para facilitar atualização da lista
        [NotMapped] public int? ID_seq_anterior { get; set; }
        [NotMapped] public int? ID_seq_posterior { get; set; }
        [NotMapped] public FilaDoSchedule OPAnterior { get; set; }
        [NotMapped] public FilaDoSchedule OPPosterior { get; set; }
        //futuramente este passo pode ser considerado como uma maquina e seguir todos os criterios como se fose maquina
        [NotMapped] public DateTime DtOptPrioridadeAlocacaono { get; set; }
        //public string LIST_GRUPO_PRODUTIVO { get; set; }
        [NotMapped] public double TempoOciosoEntreOps { get; set; }
        //public List<ItensCalendario> ItemCalendario { get; set; } // utilizado para quardar os itens do calendario entre o fim da OP anterior e o inicio da OP atual      os itens de calendario  serão asociados na OP atual OP que esta entrando na fila 
        [NotMapped] public bool RecetouSetupoAposFimDeProducao { get; set; } // utilizado para definir se a OP iniciou com o setup recetado ou seja se foi feita uma limpesa da maquina apos a fabricação da OP anterior 
        //0-não avaliado    1- não iniciou IniciouComSetupRecetado    2- iniciou com setup resetado 
        [NotMapped] public double ORD_TOLERANCIA_MAIS { get; set; }
        [NotMapped] public double ORD_TOLERANCIA_MENOS { get; set; }
        [NotMapped] public double OND_TOLERANCIA_MAIS { get; set; }
        [NotMapped] public double OND_TOLERANCIA_MENOS { get; set; }
        [NotMapped] public List<EstruturaDoSchedule> EstruturaTintas { get; set; }
        [NotMapped] public EstruturaDoSchedule EstruturaPapelao { get; set; }
        [NotMapped] public List<EstruturaDoSchedule> EstruturaFerramentas { get; set; }
        [NotMapped] public List<V_ROTEIROS_POSSIVEIS_DO_PRODUTO> RoteirosPociveis { get; set; }
        [NotMapped] public V_ROTEIROS_POSSIVEIS_DO_PRODUTO RoteiroMaiorPerformance { get; set; }
        [NotMapped] public List<Operacoes> Operacoes { get; set; }
        [NotMapped] public custo Custo { get; set; }
        [NotMapped] public custo CustoE { get; set; }
        [NotMapped] public EstruturaDoSchedule eCorBico1 { get; set; }
        [NotMapped] public EstruturaDoSchedule eCorBico2 { get; set; }
        [NotMapped] public EstruturaDoSchedule eCorBico3 { get; set; }
        [NotMapped] public EstruturaDoSchedule eCorBico4 { get; set; }
        [NotMapped] public EstruturaDoSchedule eCorBico5 { get; set; }
        [NotMapped] public SequenciaBicosTintas TintasAvaliacaoEsquerda { get; set; }
        [NotMapped] public string ALGORITIMO { get; set; }
        [NotMapped] public int ORDEM_COLOCACAO_FILA { get; set; }
        [NotMapped] public string TIPO_COLOCACAO_FILA { get; set; }
        [NotMapped] public bool ConsideraCongelada { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        public FilaDoSchedule DeepCopy()
        {
            FilaDoSchedule other = (FilaDoSchedule)this.MemberwiseClone();
            return other;
        }
    }
    public class custo
    {
        public custo()
        {
            this.ListIndexEspacosAEsquerda = new List<int>();
            this.ListDetalhesCusto = new List<DetalhesCusto>();
            this.Ferramentas = new List<Maquina>();
        }
        public int IndexOP { get; set; }
        public int IndexFila { get; set; }

        public int IndexFilaEquipe { get; set; }
        public int PermiteDisputa { get; set; }// 0 - sim; 1 - não equipe alocada; 2 - não ferramenta alocada
        public Maquina Equipe { get; set; }// 0 - sim; 1 - não equipe alocada; 2 - não ferramenta alocada
        public Maquina Maquina { get; set; }// 0 - sim; 1 - não equipe alocada; 2 - não ferramenta alocada
        public List<Maquina> Ferramentas { get; set; }

        public double MValorAvaliacao { get; set; }
        public DateTime DataInicioPrevista { get; set; }
        public DateTime DataFimPrevista { get; set; }
        public double TempoTotalProducao { get; set; }
        public DateTime FimJanelaEmbarque { get; set; }
        public SequenciaBicosTintas SeqBicosTintas { get; set; }
        public double TempoSetup { get; set; }
        public DateTime DataInicioTrunc { get; set; }
        public DateTime DataFimTrunc { get; set; }
        public string Truncado { get; set; }
        public List<int> ListIndexEspacosAEsquerda { get; set; } // lista de indices da fila dos espacos a esquerda ja calculados 
        //public int imv { get; set; } // index maquina vencedora representa a maquina vencedora para aquela OP em questão assim conseguiremos isolar os efeitos de algumas parametros de custo que quando são somados e comparadas com outros geram distorções como exemplo a performance somar o tempo total irá privilegiar OPs menores 
        public V_ROTEIROS_POSSIVEIS_DO_PRODUTO imv_roteiro { get; set; }
        public double tempoEspacoEsquerda { get; set; }
        public string OrderId { get; set; }
        public int SequenciaTransformacao { get; set; }
        public int SequenciaRepeticao { get; set; }
        public int Id { get; set; }  // id da OP
        public List<DetalhesCusto> ListDetalhesCusto { get; set; }
        public List<Operacoes> ListOperacoes { get; set; }
        public int indexEspacoEsquerdaEquipe { get; set; }
        //SequenciaBicosTintas imv_sequenciaBicosTintas_por_OP = new SequenciaBicosTintas();
        //DateTime imv_fimPrevisto_por_OP = new DateTime(1970, 1, 1, 0, 0, 0); ;
        //double imv_tempoTotal_por_OP = 0;
        //double imv_tempoSetup_por_OP = 0;
        //double imv_custo_por_OP = -1;
        //double imv_tempoTotal_por_OP_e = 0;
    }
    public class DetalhesCusto
    {
        public DetalhesCusto(string IDCusto, double Custo, double Tempo, string OPA, string OPU, string OPP, int TruOrFalse, string valorOPA, string valorOPU, string valorOPP)
        {
            this.IDCusto = IDCusto;
            this.Custo = Custo;
            this.Tempo = Tempo;
            this.OPA = OPA;
            this.OPU = OPU;
            this.OPP = OPP;
            this.TruOrFalse = TruOrFalse;
            this.valorOPA = valorOPA;
            this.valorOPU = valorOPU;
            this.valorOPP = valorOPP;
        }
        public string IDCusto { get; set; }
        public double Custo { get; set; }
        public double Tempo { get; set; }
        public string OPA { get; set; }
        public string OPU { get; set; }
        public string OPP { get; set; }
        public int TruOrFalse { get; set; }// 0 falso;  1 verdade;  2 não se aplica 
        public string valorOPA { get; set; }
        public string valorOPU { get; set; }
        public string valorOPP { get; set; }

    }
    public class FilaWeb
    {

        public string maquina { get; set; }
        public string grupoMaquinaId { get; set; }
        public string maquinaId { get; set; }
        public string produtoId { get; set; }
        public string pedidoId { get; set; }
        public string produto { get; set; }
        public string grpDescricao { get; set; }
        public DateTime inicioPrevisto { get; set; }
        public DateTime fimPrevisto { get; set; }
        public string equipeId { get; set; }
        public int seqTransform { get; set; }
        public int seqRepet { get; set; }
        public string truncado { get; set; }
        public DateTime dataInicioTrunc { get; set; }
        public DateTime dataFimTrunc { get; set; }
        public DateTime EntregaDe { get; set; }
        public DateTime EntregaAte { get; set; }
        public double qtd { get; set; }
        public double MinutoIni { get; set; }
        public double MinutoFim { get; set; }
        public double MinutoIniTrunc { get; set; }
        public double MinutoFimTrunc { get; set; }
        public string CorFila { get; set; }
        public string CorOrd { get; set; }
        public int Id { get; internal set; }
        public int CongelaFila { get; internal set; }
        public double OrdemDaFila { get; internal set; }
        public string TipoCarregamento { get; set; }
        public int OrdTipo { get; set; }
        public string Status { get; set; }
        public DateTime PrevisaoMateriaPrima { get; internal set; }
        public DateTime InicioJanelaEmbarque { get; internal set; }
        public DateTime FimJanelaEmbarque { get; internal set; }
        public DateTime EmbarqueAlvo { get; internal set; }
        public DateTime InicioGrupoProdutivo { get; internal set; }
        public DateTime FimGrupoProdutivo { get; internal set; }
        public DateTime DataNecessidadeInicioProducao { get; set; }
        public DateTime DataNecessidadeFimProducao { get; set; }
        public string OrdOpIntegracao { get; set; }
        public string CliId { get; set; }
        public string CliNome { get; set; }
        public double? M2Unitario { get; set; }
        public double? M2Total { get; set; }
        public double? Performance { get; set; }
        public string PerformanceString { get; set; }
        public double? PecasPorPulso { get; set; }
        public double? TempoSetup { get; set; }
        public string TempoSetupString { get; set; }
        public double? TempoSetupA { get; set; }
        public string TempoSetupAString { get; set; }
        public double? GrupoProdutivo { get; set; }

        public string CorBico1 { get; set; }
        public string CorBico2 { get; set; }
        public string CorBico3 { get; set; }
        public string CorBico4 { get; set; }
        public string CorBico5 { get; set; }
        public int? hieranquiaSeqTransf { get; set; }

        /// <summary>
        /// Esta propriedade foi criada para representar o estado do objeto
        /// insert, update, delete ou unchanged 
        /// </summary>
        [NotMapped]
        public string PlayAction { get; set; }

        /// <summary>
        /// Deve seguir a seguinte convecão: NameProperty:MsgErro;NameProperty:MsgErro; ...
        /// Representa os erros de validacao deste objeto
        /// </summary>
        [NotMapped]
        public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

    }
}
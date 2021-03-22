using DynamicForms.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class ViewFilaProducao
    {
        [READ] [TAB(Value = "PRINCIPAL")] public int Id { get; set; }
        //cliente
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD CLIENTE")] public string CliId { get; set; }
        [TAB(Value = "OUTROS")] public string CliNome { get; set; }
        [TAB(Value = "OUTROS")] public string CliFone { get; set; }
        [TAB(Value = "OUTROS")] public string CliObs { get; set; }
        //produto componente
        [TAB(Value = "OUTROS")] public string PcProId { get; set; }
        [TAB(Value = "OUTROS")] public string PcProDescricao { get; set; }
        [TAB(Value = "OUTROS")] public string PcUniId { get; set; }
        [TAB(Value = "OUTROS")] public string GRP_DESCRICAO { get; set; }
        [TAB(Value = "OUTROS")] public double GRP_TIPO { get; set; }
        //maquina
        [TAB(Value = "PRINCIPAL")] [READ] [Display(Name = "MAQUINA")] public string RotMaqId { get; set; }
        [TAB(Value = "OUTROS")] public string MaqDescricao { get; set; }
        [TAB(Value = "OUTROS")] public string MAQ_TIPO_PLANEJAMENTO { get; set; }

        //order
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PEDIDO")] public string OrdId { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PRODUTO")] public string PaProId { get; set; }
        [TAB(Value = "OUTROS")] public string PaProDescricao { get; set; }
        [TAB(Value = "OUTROS")] public string PA_PRO_INTEGRACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "IMG_LASTRO")] [MaxLength(50, ErrorMessage = "Maximode 50 caracteres, campo PRO_IMG_LASTRO")] public string PRO_IMG_LASTRO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "TIPO CARREGAMENTO")] public string OrdTipoCarregamento { get; set; }
        [TAB(Value = "OUTROS")] public string PaUniId { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "PREÇO UNI")] public double? OrdPrecoUnitario { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE")] public double OrdQuantidade { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA DE ENTREGA")] public DateTime OrdDataEntregaDe { get; set; }
        [TAB(Value = "OUTROS")] public DateTime OrdDataEntregaAte { get; set; }
        [Combobox(Value = "1", Description = "PRODUÇÃO E EXPEDIÇÃO")]
        [Combobox(Value = "2", Description = "SOMENTE PRODUÇÃO")]
        [Combobox(Value = "3", Description = "SOMENTE EXPEDIÇÃO")]
        [Combobox(Value = "4", Description = "RETRABALHO E EXPEDIÇÃO")]
        [Combobox(Value = "5", Description = "SOMENTE RETRABALHO")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO PEDIDO")] public int OrdTipo { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "TOL +")] public double? OrdToleranciaMais { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "TOL -")] public double? OrdToleranciaMenos { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "INÍCIO JANELA EMB")] public DateTime OrdInicioJanelaEmbarque { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "FIM JANELA EMB")] public DateTime OrdFimJanelaEmbarque { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "EMBARQUE ALVO")] public DateTime OrdEmbarqueAlvo { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "INICIO GRUPO PRODUTIVO")] public DateTime OrdInicioGrupoProdutivo { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "FIM GRUPO PRODUTIVO")] public DateTime OrdFimGrupoProdutivo { get; set; }
        [Combobox(Value = "A", Description = "ABERTO")]
        [Combobox(Value = "S", Description = "SUSPENSO")]
        [Combobox(Value = "E", Description = "ENCERRADO")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "STATUS PEDIDO")] public string ORD_STATUS { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "ÁREA UNI (M²)")] public double? ORD_M2_UNITARIO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "ÁREA TOTAL (M²)")] public double? ORD_M2_TOTAL { get; set; }
        [TAB(Value = "OUTROS")] public int? ORD_LOTE_PILOTO { get; set; }
        //fila de producao

        [TAB(Value = "PRINCIPAL")] [READ] [Display(Name = "DATA_INICIO_PREVISTA")] public DateTime FprDataInicioPrevista { get; set; }
        [TAB(Value = "PRINCIPAL")] [READ] [Display(Name = "DATA_FIM_PREVISTA")] public DateTime FprDataFimPrevista { get; set; }
        [TAB(Value = "OUTROS")] public DateTime FprDataFimMaxima { get; set; }
        [TAB(Value = "PRINCIPAL")] [READ] [Display(Name = "QUANTIDADE_PREVISTA")] public double FprQuantidadePrevista { get; set; }
        [TAB(Value = "PRINCIPAL")] [READ] [Display(Name = "SEQ. DE TRANSFORMAÇÃO")] public int RotSeqTransformacao { get; set; }
        [TAB(Value = "OUTROS")] public int FprSeqRepeticao { get; set; }
        [TAB(Value = "OUTROS")] public string FprObsProducao { get; set; }
        [TAB(Value = "OUTROS")] public string FprStatus { get; set; }
        [TAB(Value = "OUTROS")] public int? Produzindo { get; set; }

        [TAB(Value = "OUTROS")] public string CorBico1 { get; set; }
        [TAB(Value = "OUTROS")] public string CorBico2 { get; set; }
        [TAB(Value = "OUTROS")] public string CorBico3 { get; set; }
        [TAB(Value = "OUTROS")] public string CorBico4 { get; set; }
        [TAB(Value = "OUTROS")] public string CorBico5 { get; set; }
        [TAB(Value = "OUTROS")] public double FPR_META_SETUP { get; set; }
        [TAB(Value = "PRINCIPAL")] public double? FPR_GRUPO_PRODUTIVO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "ID_ORIGEM")] public int? FPR_ID_ORIGEM { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] public string EQU_ID { get; set; }
        //roteiro
        [TAB(Value = "OUTROS")] public double? RotQuantPecasPulso { get; set; }
        [TAB(Value = "OUTROS")] public double? ROT_PERFORMANCE { get; set; }
        //medicoes variaveis

        [TAB(Value = "OUTROS")] public double? FprTempoDecorridoSetup { get; set; }
        [TAB(Value = "OUTROS")] public double? FprTempoDecorridoSetupAjuste { get; set; }
        [TAB(Value = "OUTROS")] public double? FprTempoDecorridoPerformacace { get; set; }
        [TAB(Value = "OUTROS")] public double? FprQuantidadePerformace { get; set; }
        [TAB(Value = "OUTROS")] public double? FprQuantidadeSetup { get; set; }
        [TAB(Value = "OUTROS")] public double? FprQuantidadeProduzida { get; set; }
        [TAB(Value = "OUTROS")] public double? FprQuantidadeRestante { get; set; }
        [TAB(Value = "OUTROS")] public double? FprTempoTeoricoPerformace { get; set; }
        [TAB(Value = "OUTROS")] public double? FprTempoRestantePerformace { get; set; }
        [TAB(Value = "OUTROS")] public double? FprVelocidadeAtingirMeta { get; set; }
        [TAB(Value = "OUTROS")] public double? FprVeloAtuPcSegundo { get; set; }
        [TAB(Value = "OUTROS")] public double? FprPerformaceProjetada { get; set; }
        [TAB(Value = "OUTROS")] public double? TempoDecorridoPequenasParadas { get; set; }


        [TAB(Value = "OUTROS")] public double OrdemDaFila { get; set; }

        [TAB(Value = "OUTROS")] public string CorFila { get; set; }
        [TAB(Value = "OUTROS")] public string CorOrd { get; set; }
        [TAB(Value = "OUTROS")] public string MaquinaIdManual { get; set; }
        [TAB(Value = "OUTROS")] public DateTime PrevisaoMateriaPrima { get; set; }

        //--
        [TAB(Value = "OUTROS")] public string Truncado { get; set; }
        [TAB(Value = "OUTROS")] public DateTime DataInicioTrunc { get; set; }
        [TAB(Value = "OUTROS")] public DateTime DataFimTrunc { get; set; }
        [TAB(Value = "OUTROS")] public DateTime DataHoraNecessidadeInicioProducao { get; set; }
        [TAB(Value = "OUTROS")] public DateTime DataHoraNecessidadeFimProducao { get; set; }
        [TAB(Value = "OUTROS")] public int CongelaFila { get; set; }
        [TAB(Value = "OUTROS")] public double? ETI_QUANTIDADE_PALETE { get; set; }
        [TAB(Value = "OUTROS")] public int ETI_IMPRIMIR_DE { get; set; }
        [TAB(Value = "OUTROS")] public double? ETI_IMPRIMIR_ATE { get; set; }
        [TAB(Value = "OUTROS")] public int ETI_NUMERO_COPIAS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NUM.OP")] public string ORD_OP_INTEGRACAO { get; set; }
        //[Combobox(Description = "Não priorizar", Value = "0")]
        //[Combobox(Description = "Ultrapassa Congeladas", Value = "1")]
        //[Combobox(Description = "Logo após congeladas ", Value = "2")]
        //[Combobox(Description = "Encaixar no grupo produtivo atual ", Value = "3")]
        //[Combobox(Description = "Primeira do proximo grupo produtivo ", Value = "4")]
        //[Combobox(Description = "Encaixar no proximo grupo produtivo ", Value = "5")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRIORIDADE")] public int FPR_PRIORIDADE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "HIERARQUIA_SEQ_TRANSFORMACAO")] public int? FPR_HIERARQUIA_SEQ_TRANSFORMACAO { get; set; }
        [TAB(Value = "OUTROS")] public string GMA_ID { get; set; }
        [TAB(Value = "QUALIDADE")] public int? TEMPLATE_TESTES { get; set; }

        public virtual Order Order { get; set; }
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
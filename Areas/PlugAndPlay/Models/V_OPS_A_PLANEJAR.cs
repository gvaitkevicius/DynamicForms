using DynamicForms.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class V_OPS_A_PLANEJAR
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRIORIDADE")] public int? FPR_PRIORIDADE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "STATUS")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo ORD_STATUS")] public string ORD_STATUS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "STATUS")] [MaxLength(2, ErrorMessage = "Maximode 2 caracteres, campo FPR_STATUS")] public string FPR_STATUS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "inaId")] [Required(ErrorMessage = "Campo MaquinaId requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MaquinaId")] public string MaquinaId { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "rId")] [Required(ErrorMessage = "Campo OrderId requirido.")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo OrderId")] public string OrderId { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRO_ID")] [Required(ErrorMessage = "Campo ORD_PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo ORD_PRO_ID")] public string ORD_PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "utoId")] [Required(ErrorMessage = "Campo ProdutoId requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo ProdutoId")] public string ProdutoId { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "enciaTransformacao")] [Required(ErrorMessage = "Campo SequenciaTransformacao requirido.")] public int SequenciaTransformacao { get; set; }
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
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PERFORMANCE_METRO_LINEAR_POR_SEGUNDO")] [Required(ErrorMessage = "Campo GRP_PERFORMANCE_METRO_LINEAR_POR_SEGUNDO requirido.")] public double GRP_PERFORMANCE_METRO_LINEAR_POR_SEGUNDO { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs, ref int modo_insert) {  } 

        [NotMapped] public int IDCalculo { get; set; }
        [NotMapped] public int ID_seq_anterior { get; set; }
        [NotMapped] public int ID_seq_posterior { get; set; }
        [NotMapped] public V_OPS_A_PLANEJAR OPAnterior { get; set; }
        [NotMapped] public V_OPS_A_PLANEJAR OPPosterior { get; set; }
        [NotMapped] public Maquina Maquina { get; set; }
        [NotMapped] public DateTime NecessidadeInicioProducaoOrden { get; set; }
        [NotMapped] public DateTime NecessidadeFimProducaoOrden { get; set; }
        [NotMapped] public DateTime DtOptPrioridadeAlocacaono { get; set; }
        [NotMapped] public bool StatusCargaMaquina { get; set; }
        [NotMapped] public double TempoTotal { get; set; }
        [NotMapped] public double MAQ_LARGURA_UTIL { get; set; }

        
    }
}

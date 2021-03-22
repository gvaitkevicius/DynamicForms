using DynamicForms.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class V_PAINEL_GESTOR_STATUS_MAQUINAS
    {
        public V_PAINEL_GESTOR_STATUS_MAQUINAS() { }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "MAQ_ID")] [Required(ErrorMessage = "Campo ROT_MAQ_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo ROT_MAQ_ID")] public string ROT_MAQ_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO")] [Required(ErrorMessage = "Campo MAQ_DESCRICAO requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MAQ_DESCRICAO")] public string MAQ_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "STATUS")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MAQ_STATUS")] public string MAQ_STATUS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "US_COR_MAQUINA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo STATUS_COR_MAQUINA")] public string STATUS_COR_MAQUINA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "MA_ATUALIZACAO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo ULTIMA_ATUALIZACAO")] public string ULTIMA_ATUALIZACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo OCO_ID requirido.")] [MaxLength(131, ErrorMessage = "Maximode 131 caracteres, campo OCO_ID")] public string OCO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo OCO_DESCRICAO")] public string OCO_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OBSERVACOES")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo FEE_OBSERVACOES")] public string FEE_OBSERVACOES { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "BACKS_PENDENTES")] public int? FEEDBACKS_PENDENTES { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "O_SEM_FEEDBACK")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo TEMPO_SEM_FEEDBACK")] public string TEMPO_SEM_FEEDBACK { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PARCIAIS")] [Required(ErrorMessage = "Campo OPS_PARCIAIS requirido.")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo OPS_PARCIAIS")] public string OPS_PARCIAIS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo CLI_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo CLI_ID")] public string CLI_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NOME")] [Required(ErrorMessage = "Campo CLI_NOME requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo CLI_NOME")] public string CLI_NOME { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FONE")] [MaxLength(68, ErrorMessage = "Maximode 68 caracteres, campo CLI_FONE")] public string CLI_FONE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OBS")] [MaxLength(3000, ErrorMessage = "Maximode * caracteres, campo CLI_OBS")] public string CLI_OBS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "RO_ID")] [Required(ErrorMessage = "Campo PC_PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PC_PRO_ID")] public string PC_PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "RO_DESCRICAO")] [Required(ErrorMessage = "Campo PC_PRO_DESCRICAO requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PC_PRO_DESCRICAO")] public string PC_PRO_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NI_ID")] [Required(ErrorMessage = "Campo PC_UNI_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PC_UNI_ID")] public string PC_UNI_ID { get; set; }
        //[TAB(Value = "PRINCIPAL")] [Display(Name = "IA_TURMA")] [MaxLength(8, ErrorMessage = "Maximode 8 caracteres, campo DS_DIA_TURMA")] public string DS_DIA_TURMA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "RO_ID")] [Required(ErrorMessage = "Campo PA_PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PA_PRO_ID")] public string PA_PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "RO_DESCRICAO")] [Required(ErrorMessage = "Campo PA_PRO_DESCRICAO requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PA_PRO_DESCRICAO")] public string PA_PRO_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NI_ID")] [Required(ErrorMessage = "Campo PA_UNI_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PA_UNI_ID")] public string PA_UNI_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRECO_UNITARIO")] public double? ORD_PRECO_UNITARIO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE")] [Required(ErrorMessage = "Campo ORD_QUANTIDADE requirido.")] public double ORD_QUANTIDADE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_ENTREGA_DE")] [Required(ErrorMessage = "Campo ORD_DATA_ENTREGA_DE requirido.")] public DateTime ORD_DATA_ENTREGA_DE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_ENTREGA_ATE")] [Required(ErrorMessage = "Campo ORD_DATA_ENTREGA_ATE requirido.")] public DateTime ORD_DATA_ENTREGA_ATE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO")] public int? ORD_TIPO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TOLERANCIA_MAIS")] public double? ORD_TOLERANCIA_MAIS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TOLERANCIA_MENOS")] public double? ORD_TOLERANCIA_MENOS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_INICIO_PREVISTA")] [Required(ErrorMessage = "Campo FPR_DATA_INICIO_PREVISTA requirido.")] public DateTime FPR_DATA_INICIO_PREVISTA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_FIM_PREVISTA")] [Required(ErrorMessage = "Campo FPR_DATA_FIM_PREVISTA requirido.")] public DateTime FPR_DATA_FIM_PREVISTA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_FIM_MAXIMA")] [Required(ErrorMessage = "Campo FPR_DATA_FIM_MAXIMA requirido.")] public DateTime FPR_DATA_FIM_MAXIMA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE_PREVISTA")] [Required(ErrorMessage = "Campo FPR_QUANTIDADE_PREVISTA requirido.")] public double FPR_QUANTIDADE_PREVISTA { get; set; }
        //[TAB(Value = "PRINCIPAL")] [Display(Name = "OBS_PRODUCAO")] [MaxLength(4000, ErrorMessage = "Maximode * caracteres, campo FPR_OBS_PRODUCAO")] public string FPR_OBS_PRODUCAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "STATUS")] [MaxLength(2, ErrorMessage = "Maximode 2 caracteres, campo FPR_STATUS")] public string FPR_STATUS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TEMPO_DECORRIDO_SETUP")] public double? FPR_TEMPO_DECORRIDO_SETUP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TEMPO_DECORRIDO_SETUPA")] public double? FPR_TEMPO_DECORRIDO_SETUPA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TEMPO_DECORRIDO_PERFORMANC")] public double? FPR_TEMPO_DECORRIDO_PERFORMANC { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD_PERFORMANCE")] public double? FPR_QTD_PERFORMANCE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD_SETUP")] public double? FPR_QTD_SETUP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD_PRODUZIDA")] public double? FPR_QTD_PRODUZIDA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TEMPO_TEORICO_PERFORMANCE")] public double? FPR_TEMPO_TEORICO_PERFORMANCE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TEMPO_RESTANTE_PERFORMANC")] public double? FPR_TEMPO_RESTANTE_PERFORMANC { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VELOCIDADE_P_ATINGIR_META")] public double? FPR_VELOCIDADE_P_ATINGIR_META { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD_RESTANTE")] public double? FPR_QTD_RESTANTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VELO_ATU_PC_SEGUNDO")] public double? FPR_VELO_ATU_PC_SEGUNDO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TEMPO_DECO_PEQUENA_PARADA")] public double? FPR_TEMPO_DECO_PEQUENA_PARADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PERFORMANCE_PROJETADA")] public double? FPR_PERFORMANCE_PROJETADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "US_FILA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo STATUS_FILA")] public string STATUS_FILA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PECAS_POR_PULSO")] public double? ROT_PECAS_POR_PULSO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PROXIMA_META_PERFORMANCE")] public double? TAR_PROXIMA_META_PERFORMANCE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ENTUAL_PERFORMANCE")] public double? PERCENTUAL_PERFORMANCE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "US_COR_PERFORMANCE")] [MaxLength(8, ErrorMessage = "Maximode 8 caracteres, campo STATUS_COR_PERFORMANCE")] public string STATUS_COR_PERFORMANCE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PROXIMA_META_TEMPO_SETUP")] public double? TAR_PROXIMA_META_TEMPO_SETUP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ENTUAL_SETUP")] public double? PERCENTUAL_SETUP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "US_COR_SETUP")] [MaxLength(8, ErrorMessage = "Maximode 8 caracteres, campo STATUS_COR_SETUP")] public string STATUS_COR_SETUP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PROXIMA_META_TEMPO_SETUP_AJUSTE")] public double? TAR_PROXIMA_META_TEMPO_SETUP_AJUSTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ENTUAL_SETUP_AJUSTE")] public double? PERCENTUAL_SETUP_AJUSTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "US_COR_SETUP_AJUSTE")] [MaxLength(8, ErrorMessage = "Maximode 8 caracteres, campo STATUS_COR_SETUP_AJUSTE")] public string STATUS_COR_SETUP_AJUSTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ENTUAL_SETUP_GERAL")] public double? PERCENTUAL_SETUP_GERAL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "US_COR_SETUP_GERAL")] [MaxLength(8, ErrorMessage = "Maximode 8 caracteres, campo STATUS_COR_SETUP_GERAL")] public string STATUS_COR_SETUP_GERAL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ULTIMA_ATUALIZACAO")] public DateTime MAQ_ULTIMA_ATUALIZACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ENTUAL_CONCLUIDO_SETUP")] public double? PERCENTUAL_CONCLUIDO_SETUP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ENTUAL_CONCLUIDO_SETUP_AJUSTE")] public double? PERCENTUAL_CONCLUIDO_SETUP_AJUSTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ENTUAL_CONCLUIDO_PERFORMANCE")] public double? PERCENTUAL_CONCLUIDO_PERFORMANCE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "_ATU_PC_SEGUNDO_ROUD_1")] public double? VELO_ATU_PC_SEGUNDO_ROUD_1 { get; set; }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "PERCENTUAL_REALIZADO_PERFORMANCE")] public double? TAR_PERCENTUAL_REALIZADO_PERFORMANCE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID_PERFORMANCE")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo OCO_ID_PERFORMANCE")] public string OCO_ID_PERFORMANCE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OBS_PERFORMANCE")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo TAR_OBS_PERFORMANCE")] public string TAR_OBS_PERFORMANCE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID_SETUP")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo OCO_ID_SETUP")] public string OCO_ID_SETUP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OBS_SETUP")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo TAR_OBS_SETUP")] public string TAR_OBS_SETUP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID_SETUPA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo OCO_ID_SETUPA")] public string OCO_ID_SETUPA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OBS_SETUPA")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo TAR_OBS_SETUPA")] public string TAR_OBS_SETUPA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO_FEEDBACK_PERFORMANCE")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo TAR_TIPO_FEEDBACK_PERFORMANCE")] public string TAR_TIPO_FEEDBACK_PERFORMANCE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO_FEEDBACK_SETUP")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo TAR_TIPO_FEEDBACK_SETUP")] public string TAR_TIPO_FEEDBACK_SETUP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO_FEEDBACK_SETUP_AJUSTE")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo TAR_TIPO_FEEDBACK_SETUP_AJUSTE")] public string TAR_TIPO_FEEDBACK_SETUP_AJUSTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD_SETUP_AJUSTE")] public double? TAR_QTD_SETUP_AJUSTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD")] public double? TAR_QTD { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PARAMETRO_TIME_WORK_STOP_MACHINE")] public int? TAR_PARAMETRO_TIME_WORK_STOP_MACHINE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PARAMETRO_TEMPO_QUEBRA_DE_LOTE")] public int? TAR_PARAMETRO_TEMPO_QUEBRA_DE_LOTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PERFORMANCE_MAX_VERDE")] public double? TAR_PERFORMANCE_MAX_VERDE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PERFORMANCE_MIN_VERDE")] public double? TAR_PERFORMANCE_MIN_VERDE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SETUP_MAX_VERDE")] public double? TAR_SETUP_MAX_VERDE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SETUP_MIN_VERDE")] public double? TAR_SETUP_MIN_VERDE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SETUPA_MAX_VERDE")] public double? TAR_SETUPA_MAX_VERDE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SETUPA_MIN_VERDE")] public double? TAR_SETUPA_MIN_VERDE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PERFORMANCE_MIN_AMARELO")] public double? TAR_PERFORMANCE_MIN_AMARELO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SETUP_MAX_AMARELO")] public double? TAR_SETUP_MAX_AMARELO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SETUPA_MAX_AMARELO")] public double? TAR_SETUPA_MAX_AMARELO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo TAR_ID requirido.")] public int TAR_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "P_GERAL_AZUL")] [Required(ErrorMessage = "Campo SETUP_GERAL_AZUL requirido.")] public int SETUP_GERAL_AZUL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "P_GERAL_VERDE")] [Required(ErrorMessage = "Campo SETUP_GERAL_VERDE requirido.")] public int SETUP_GERAL_VERDE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "P_GERAL_AMARELO")] [Required(ErrorMessage = "Campo SETUP_GERAL_AMARELO requirido.")] public int SETUP_GERAL_AMARELO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "P_GERAL_VERMELHO")] [Required(ErrorMessage = "Campo SETUP_GERAL_VERMELHO requirido.")] public int SETUP_GERAL_VERMELHO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORMANCE_AZUL")] [Required(ErrorMessage = "Campo PERFORMANCE_AZUL requirido.")] public int PERFORMANCE_AZUL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORMANCE_VERDE")] [Required(ErrorMessage = "Campo PERFORMANCE_VERDE requirido.")] public int PERFORMANCE_VERDE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORMANCE_AMARELO")] [Required(ErrorMessage = "Campo PERFORMANCE_AMARELO requirido.")] public int PERFORMANCE_AMARELO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORMANCE_VERMELHO")] [Required(ErrorMessage = "Campo PERFORMANCE_VERMELHO requirido.")] public int PERFORMANCE_VERMELHO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "O_PARADAS_NAO_PROGRAMADAS")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo TEMPO_PARADAS_NAO_PROGRAMADAS")] public string TEMPO_PARADAS_NAO_PROGRAMADAS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PARADAS_NAO_PROGRAMADAS")] [Required(ErrorMessage = "Campo QTD_PARADAS_NAO_PROGRAMADAS requirido.")] public double QTD_PARADAS_NAO_PROGRAMADAS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "O_PARADAS_NAO_PROGRAMADAS_EXETO_SETUP")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo TEMPO_PARADAS_NAO_PROGRAMADAS_EXETO_SETUP")] public string TEMPO_PARADAS_NAO_PROGRAMADAS_EXETO_SETUP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PARADAS_NAO_PROGRAMADAS_EXETO_SETUP")] [Required(ErrorMessage = "Campo QTD_PARADAS_NAO_PROGRAMADAS_EXETO_SETUP requirido.")] public double QTD_PARADAS_NAO_PROGRAMADAS_EXETO_SETUP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "O_PEQUENAS_PARADAS")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo TEMPO_PEQUENAS_PARADAS")] public string TEMPO_PEQUENAS_PARADAS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PEQUENAS_PARADAS")] [Required(ErrorMessage = "Campo QTD_PEQUENAS_PARADAS requirido.")] public double QTD_PEQUENAS_PARADAS { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        [NotMapped] public string VELO_ATU_PC_SEGUNDO_ROUD_1_STRING { get; set; }
        [NotMapped] public string UN_TEMPO { get; set; }
        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs) {  } 
    }
}

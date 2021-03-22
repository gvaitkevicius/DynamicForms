using DynamicForms.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class V_PAINEL_GESTOR_DESEMPENHO_TURNOS_MAQUINA
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DIA_TURMA")] [MaxLength(8, ErrorMessage = "Maximode 8 caracteres, campo FEE_DIA_TURMA")] public string FEE_DIA_TURMA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo MAQ_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MAQ_ID")] public string MAQ_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "_ID")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo TURN_ID")] public string TURN_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "_ID")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo TURM_ID")] public string TURM_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "O_PARADAS_NAO_PROGRAMADAS")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo TEMPO_PARADAS_NAO_PROGRAMADAS")] public string TEMPO_PARADAS_NAO_PROGRAMADAS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PARADAS_NAO_PROGRAMADAS")] public double? QTD_PARADAS_NAO_PROGRAMADAS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "O_PARADAS_NAO_PROGRAMADAS_EXETO_SETUP")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo TEMPO_PARADAS_NAO_PROGRAMADAS_EXETO_SETUP")] public string TEMPO_PARADAS_NAO_PROGRAMADAS_EXETO_SETUP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PARADAS_NAO_PROGRAMADAS_EXETO_SETUP")] public double? QTD_PARADAS_NAO_PROGRAMADAS_EXETO_SETUP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "O_PEQUENAS_PARADAS")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo TEMPO_PEQUENAS_PARADAS")] public string TEMPO_PEQUENAS_PARADAS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PEQUENAS_PARADAS")] public double? QTD_PEQUENAS_PARADAS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "O_PLANEJADO")] public double? TEMPO_PLANEJADO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "O_PRODUZINDO")] public double? TEMPO_PRODUZINDO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "IA_TURMA")] [MaxLength(8, ErrorMessage = "Maximode 8 caracteres, campo DS_DIA_TURMA")] public string DS_DIA_TURMA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "URM_ID")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo DS_TURM_ID")] public string DS_TURM_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "URN_ID")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo DS_TURN_ID")] public string DS_TURN_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "AQ_ID")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo DS_MAQ_ID")] public string DS_MAQ_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "P_GERAL_AZUL")] public double? SETUP_GERAL_AZUL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "P_GERAL_VERDE")] public double? SETUP_GERAL_VERDE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "P_GERAL_AMARELO")] public double? SETUP_GERAL_AMARELO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "P_GERAL_VERMELHO")] public double? SETUP_GERAL_VERMELHO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "P_AZUL")] public double? SETUP_AZUL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "P_VERDE")] public double? SETUP_VERDE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "P_AMARELO")] public double? SETUP_AMARELO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "P_VERMELHO")] public double? SETUP_VERMELHO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PA_AZUL")] public double? SETUPA_AZUL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PA_VERDE")] public double? SETUPA_VERDE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PA_AMARELO")] public double? SETUPA_AMARELO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PA_VERMELHO")] public double? SETUPA_VERMELHO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORMANCE_AZUL")] public double? PERFORMANCE_AZUL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORMANCE_VERDE")] public double? PERFORMANCE_VERDE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORMANCE_AMARELO")] public double? PERFORMANCE_AMARELO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORMANCE_VERMELHO")] public double? PERFORMANCE_VERMELHO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SETUPS")] public double? QTD_SETUPS { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs) {  } 
    }
}

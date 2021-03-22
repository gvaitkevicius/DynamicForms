using DynamicForms.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class V_PAINEL_GESTOR_DESEMPENHO_TURNOS
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DIA_TURMA")] [MaxLength(8, ErrorMessage = "Maximode 8 caracteres, campo FEE_DIA_TURMA")] public string FEE_DIA_TURMA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "_ID")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo TURN_ID")] public string TURN_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "_ID")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo TURM_ID")] public string TURM_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "O_PARADAS_NAO_PROGRAMADAS")] [Required(ErrorMessage = "Campo TEMPO_PARADAS_NAO_PROGRAMADAS requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo TEMPO_PARADAS_NAO_PROGRAMADAS")] public string TEMPO_PARADAS_NAO_PROGRAMADAS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PARADAS_NAO_PROGRAMADAS")] [Required(ErrorMessage = "Campo QTD_PARADAS_NAO_PROGRAMADAS requirido.")] public double QTD_PARADAS_NAO_PROGRAMADAS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "O_PARADAS_NAO_PROGRAMADAS_EXETO_SETUP")] [Required(ErrorMessage = "Campo TEMPO_PARADAS_NAO_PROGRAMADAS_EXETO_SETUP requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo TEMPO_PARADAS_NAO_PROGRAMADAS_EXETO_SETUP")] public string TEMPO_PARADAS_NAO_PROGRAMADAS_EXETO_SETUP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PARADAS_NAO_PROGRAMADAS_EXETO_SETUP")] [Required(ErrorMessage = "Campo QTD_PARADAS_NAO_PROGRAMADAS_EXETO_SETUP requirido.")] public double QTD_PARADAS_NAO_PROGRAMADAS_EXETO_SETUP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "O_PEQUENAS_PARADAS")] [Required(ErrorMessage = "Campo TEMPO_PEQUENAS_PARADAS requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo TEMPO_PEQUENAS_PARADAS")] public string TEMPO_PEQUENAS_PARADAS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PEQUENAS_PARADAS")] [Required(ErrorMessage = "Campo QTD_PEQUENAS_PARADAS requirido.")] public double QTD_PEQUENAS_PARADAS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "O_PLANEJADO")] [Required(ErrorMessage = "Campo TEMPO_PLANEJADO requirido.")] public double TEMPO_PLANEJADO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "O_PRODUZINDO")] [Required(ErrorMessage = "Campo TEMPO_PRODUZINDO requirido.")] public double TEMPO_PRODUZINDO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "IA_TURMA")] [MaxLength(8, ErrorMessage = "Maximode 8 caracteres, campo DS_DIA_TURMA")] public string DS_DIA_TURMA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "URM_ID")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo DS_TURM_ID")] public string DS_TURM_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "URN_ID")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo DS_TURN_ID")] public string DS_TURN_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "P_GERAL_AZUL")] [Required(ErrorMessage = "Campo SETUP_GERAL_AZUL requirido.")] public double SETUP_GERAL_AZUL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "P_GERAL_VERDE")] [Required(ErrorMessage = "Campo SETUP_GERAL_VERDE requirido.")] public double SETUP_GERAL_VERDE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "P_GERAL_AMARELO")] [Required(ErrorMessage = "Campo SETUP_GERAL_AMARELO requirido.")] public double SETUP_GERAL_AMARELO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "P_GERAL_VERMELHO")] [Required(ErrorMessage = "Campo SETUP_GERAL_VERMELHO requirido.")] public double SETUP_GERAL_VERMELHO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "P_AZUL")] [Required(ErrorMessage = "Campo SETUP_AZUL requirido.")] public double SETUP_AZUL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "P_VERDE")] [Required(ErrorMessage = "Campo SETUP_VERDE requirido.")] public double SETUP_VERDE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "P_AMARELO")] [Required(ErrorMessage = "Campo SETUP_AMARELO requirido.")] public double SETUP_AMARELO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "P_VERMELHO")] [Required(ErrorMessage = "Campo SETUP_VERMELHO requirido.")] public double SETUP_VERMELHO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PA_AZUL")] [Required(ErrorMessage = "Campo SETUPA_AZUL requirido.")] public double SETUPA_AZUL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PA_VERDE")] [Required(ErrorMessage = "Campo SETUPA_VERDE requirido.")] public double SETUPA_VERDE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PA_AMARELO")] [Required(ErrorMessage = "Campo SETUPA_AMARELO requirido.")] public double SETUPA_AMARELO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PA_VERMELHO")] [Required(ErrorMessage = "Campo SETUPA_VERMELHO requirido.")] public double SETUPA_VERMELHO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORMANCE_AZUL")] [Required(ErrorMessage = "Campo PERFORMANCE_AZUL requirido.")] public double PERFORMANCE_AZUL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORMANCE_VERDE")] [Required(ErrorMessage = "Campo PERFORMANCE_VERDE requirido.")] public double PERFORMANCE_VERDE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORMANCE_AMARELO")] [Required(ErrorMessage = "Campo PERFORMANCE_AMARELO requirido.")] public double PERFORMANCE_AMARELO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORMANCE_VERMELHO")] [Required(ErrorMessage = "Campo PERFORMANCE_VERMELHO requirido.")] public double PERFORMANCE_VERMELHO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SETUPS")] [Required(ErrorMessage = "Campo QTD_SETUPS requirido.")] public double QTD_SETUPS { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs) {  } 
    }
}

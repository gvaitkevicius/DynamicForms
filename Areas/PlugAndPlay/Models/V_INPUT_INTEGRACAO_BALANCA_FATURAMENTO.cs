using DynamicForms.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "INTEGRAÇÃO BALANÇA FATURAMENTO")]
    public class V_INPUT_INTEGRACAO_BALANCA_FATURAMENTO
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID DA CARGA")] [Required(ErrorMessage = "Campo CAR_ID_INTEGRACAO_BALANCA requirido.")] public string CAR_ID_INTEGRACAO_BALANCA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PLACA")] [Required(ErrorMessage = "Campo VEI_PLACA requirido.")] [MaxLength(8, ErrorMessage = "Maximode 8 caracteres, campo VEI_PLACA")] public string VEI_PLACA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PESO DE ENTRADA")] [Required(ErrorMessage = "Campo CAR_PESO_ENTRADA requirido.")] public double CAR_PESO_ENTRADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PESO DE SAIDA")] [Required(ErrorMessage = "Campo CAR_PESO_SAIDA requirido.")] public double CAR_PESO_SAIDA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA ENTRADA VEICULO")] [Required(ErrorMessage = "Campo CAR_DATA_ENTRADA_VEICULO requirido.")] public DateTime CAR_DATA_ENTRADA_VEICULO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA SAIDA VEICULO")] [Required(ErrorMessage = "Campo CAR_DATA_SAIDA_VEICULO requirido.")] public DateTime CAR_DATA_SAIDA_VEICULO { get; set; }
        

        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs, ref int modo_insert) {  } 
    }
}

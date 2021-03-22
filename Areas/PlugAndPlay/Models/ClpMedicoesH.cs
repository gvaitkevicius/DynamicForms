using DynamicForms.Models;
using DynamicForms.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class ClpMedicoesH
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE")] [Required(ErrorMessage = "Campo QTD requirido.")] public double QTD { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA INICIO")] [Required(ErrorMessage = "Campo DATA_INI requirido.")] public DateTime DATA_INI { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA FIM")] [Required(ErrorMessage = "Campo DATA_FIM requirido.")] public DateTime DATA_FIM { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "MAQUINA")] [Required(ErrorMessage = "Campo MAQUINA_ID requirido.")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo MAQUINA_ID")] public string MAQUINA_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "GRUPO")] public double? GRUPO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD. REGISTROS")] public int? QTD_REGS { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            return true;
        }
    }
}

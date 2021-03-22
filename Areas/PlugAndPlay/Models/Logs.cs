using DynamicForms.Models;
using DynamicForms.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class Logs
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LOG_ID")] [Required(ErrorMessage = "Campo LOG_ID requirido.")] public int LOG_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CHAVE")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo LOG_CHAVE")] public string LOG_CHAVE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CONTEXTO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo LOG_CONTEXTO")] public string LOG_CONTEXTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CONTEUDO")] [MaxLength(3500, ErrorMessage = "Maximode * caracteres, campo LOG_CONTEUDO")] public string LOG_CONTEUDO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "EMISSAO")] public DateTime LOG_EMISSAO { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        public Logs()
        {

        }
        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            return true;
        }
    }
}

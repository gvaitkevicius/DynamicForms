using DynamicForms.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "ESTRUTURA DA ETIQUETA")]
    public class EstruturaEtiqueta
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo EST_ID requirido.")] [READ] public int EST_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "HTML DA ESTRUTURA")] [MaxLength(8000, ErrorMessage = "Maximo de 8000 caracteres, campo HTML_ESTRUTURA")] public string HTML_ESTRUTURA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID DO CLIENTE")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo CLI_ID")] public string CLI_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRIÇÃO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo EST_DESCRICAO")] public string EST_DESCRICAO { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        public virtual Cliente Cliente { get; set; }

        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs, ref int modo_insert) {  } 
    }
}

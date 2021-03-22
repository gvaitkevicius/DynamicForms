using DynamicForms.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class TipoABNT
    {
        public TipoABNT()
        {
            ProdutoCaixa = new HashSet<ProdutoCaixa>();
        }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID TIPO ABNT")] [Required(ErrorMessage = "Campo ABN_ID requirido.")] [MaxLength(50, ErrorMessage = "Maximode 50 caracteres, campo ABN_ID")] public string ABN_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO")] [MaxLength(50, ErrorMessage = "Maximode 50 caracteres, campo ABN_DESCRICAO")] public string ABN_DESCRICAO { get; set; }
        public virtual ICollection<ProdutoCaixa> ProdutoCaixa { get; set; }

        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs, ref int modo_insert) {  } 
    }
}

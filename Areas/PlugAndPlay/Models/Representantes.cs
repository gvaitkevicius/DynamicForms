using DynamicForms.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class Representantes
    {
        public Representantes()
        {
            Cotas = new HashSet<Cotas>();
            Order = new HashSet<Order>();
            Cliente = new HashSet<Cliente>();
        }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo REP_ID requirido.")] public int REP_ID { get; set; }
        [SEARCH] [TAB(Value = "PRINCIPAL")] [Display(Name = "NOME")] [MaxLength(80, ErrorMessage = "Maximode 80 caracteres, campo REP_NOME")] public string REP_NOME { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs, ref int modo_insert) {  } 
        public virtual ICollection<Cotas> Cotas { get; set; }
        public virtual ICollection<Order> Order{ get; set; }
        public virtual ICollection<Cliente> Cliente{ get; set; }

    }
}

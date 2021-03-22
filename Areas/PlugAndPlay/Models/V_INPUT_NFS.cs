using DynamicForms.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "NFs")]
    public class V_INPUT_NFS
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo ORD_ID requirido.")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo CAR_ID requirido.")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo CAR_ID")] public string CAR_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "UMERO")] [Required(ErrorMessage = "Campo NF_NUMERO requirido.")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo NF_NUMERO")] public string NF_NUMERO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ERIE")] [Required(ErrorMessage = "Campo NF_SERIE requirido.")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo NF_SERIE")] public string NF_SERIE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "MISSAO")] [Required(ErrorMessage = "Campo NF_EMISSAO requirido.")] public DateTime NF_EMISSAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TD")] [Required(ErrorMessage = "Campo NF_QTD requirido.")] public decimal NF_QTD { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        //RELAÇÕES
        public virtual V_CONSULTA_PEDIDO ConsultaPedido { get; set; }

        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs, ref int modo_insert) {  } 
    }
}

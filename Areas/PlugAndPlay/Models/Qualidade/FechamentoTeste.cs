using DynamicForms.Models;
using DynamicForms.Util;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class FechamentoTeste
    {
        public FechamentoTeste() { }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [READ] public int FEC_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD")] public int? FEC_QTD { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo GRP_ID")] public string GRP_ID { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        public virtual GrupoProduto GrupoProduto { get; set; }

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) { return true; }

    }
}

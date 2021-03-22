using DynamicForms.Models;
using DynamicForms.Util;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class TipoAvaliacao
    {
        public TipoAvaliacao()
        {
            TesteFisico = new HashSet<TesteFisico>();
            TipoTeste = new HashSet<TipoTeste>();
        }

        [TAB(Value = "PRINCIPAL")] [READ] [Display(Name = "ID")] [Required(ErrorMessage = "Campo TA_ID requirido.")] public int TA_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESC")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo TA_DESC")] public string TA_DESC { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        public ICollection<TipoTeste> TipoTeste { get; set; }
        public ICollection<TesteFisico> TesteFisico { get; set; }

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) { return true; }
    }
}

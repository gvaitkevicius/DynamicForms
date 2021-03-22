using DynamicForms.Models;
using DynamicForms.Util;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class PlanoAmostralTeste
    {
        public PlanoAmostralTeste() { }

        [TAB(Value = "PRINCIPAL")] [READ] [Display(Name = "ID")] public int PAT_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD_CAIXAS_DE")] public int? PAT_QTD_CAIXAS_DE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD_CAIXAS_ATE")] public int? PAT_QTD_CAIXAS_ATE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "N_AMOSTRAGEM")] public int? PAT_N_AMOSTRAGEM { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "GRP_TIPO")] public double? GRP_TIPO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PERCENT_ESPECIF")] public double? PAT_PERCENT_ESPECIF { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) { return true; }
    }
}

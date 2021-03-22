using DynamicForms.Models;
using DynamicForms.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "WMS EXPEDIÇÃO")]
    public class ProdutoWMSExpedicao : ProdutoAbstrato
    {
        public ProdutoWMSExpedicao() { }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD GRUPO PRODUTO")] [Required(ErrorMessage = "Campo GRP_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo GRP_ID")] public string GRP_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD UNIDADE MEDIDA")] [Required(ErrorMessage = "Campo UNI_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo UNI_ID")] public string UNI_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PEÇAS/FARDO")] public double? PRO_PECAS_POR_FARDO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FARDOS/CAMADA")] public double? PRO_FARDOS_POR_CAMADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CAMADAS/PALETE")] public double? PRO_CAMADAS_POR_PALETE { get; set; }

        public virtual GrupoProdutoWMSExpedicao GrupoProdutoWMSExpedicao { get; set; }
        public virtual UnidadeMedida UnidadeMedida { get; set; }

        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) { }
    }
}

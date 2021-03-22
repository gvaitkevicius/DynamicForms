using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Areas.PlugAndPlay.Models.Estoque
{
    public class ResumoInventario
    {
        public string MOV_ENDERECO { get; set; }
        public string MOV_LOTE { get; set; }
        public string MOV_SUB_LOTE { get; set; }
        public string ORD_ID { get; set; }
        public double SALDO_SISTEMA { get; set; }
        public double SALDO_AFERIDO { get; set; }
        public string STATUS { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
    }
}

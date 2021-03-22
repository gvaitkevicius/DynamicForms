using System;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class ViewEstoquePA
    {
        public ViewEstoquePA()
        {

        }
        public string ORD_ID { get; set; }
        public string PRO_ID { get; set; }
        public string PRO_DESCRICAO { get; set; }
        public string GRP_ID { get; set; }
        public string GRP_DESCRICAO { get; set; }
        public double GRP_TIPO { get; set; }
        public DateTime DATA_MOVIMENTO { get; set; }
        public double SALDO_RETIDO { get; set; }
        public double DISPONIVEL { get; set; }
        public double COMPROMISSADO { get; set; }
        public int QTD_PALETES { get; set; }
        public double SOBRA_PRODUCAO { get; set; }
        public double SOBRA_EXPEDICAO { get; set; }
        public double DEVOLUCAO { get; set; }
        public double PEDIDOS_FUTUROS { get; set; }

    }
}

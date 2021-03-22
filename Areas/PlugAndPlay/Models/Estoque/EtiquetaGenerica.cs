using System;

namespace DynamicForms.Areas.PlugAndPlay.Models.Estoque
{
    public class EtiquetaGenerica
    {
        public string CLI_NOME { get; set; }
        public string REFERENCIA { get; set; }
        public string PRO_ID { get; set; }
        public string PRO_DESCRICAO { get; set; }
        public string OF { get; set; }
        public string ETI_QUANTIDADE_PALETE { get; set; }
        public DateTime ORD_DATA_ENTREGA_ATE { get; set; }
        public int QTD_AMARRADOS { get; set; }
        public string MAQ_ID { get; set; }
        public string TURMA { get; set; }
        public string MESA { get; set; }
        public string ETI_DATA_FABRICACAO { get; set; }
        public string CLI_ENDERECO_ENTREGA { get; set; }
        public string PROXIMA_MAQUINA { get; set; }
        public string ETI_CODIGO_BARRAS { get; set; }
        public string ETI_LOTE { get; set; }
        public string ETI_SUB_LOTE { get; set; }
        public EtiquetaGenerica()
        {

        }
    }
}

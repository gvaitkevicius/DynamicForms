using DynamicForms.Models;
using DynamicForms.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class V_ITEM_CARGA
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CARGA")] [Required(ErrorMessage = "Campo CAR_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo CAR_ID")] public string CAR_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PEDIDO")] [Required(ErrorMessage = "Campo ORD_ID requirido.")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ENTREGA PLANEJADA")] [Required(ErrorMessage = "Campo ITC_ENTREGA_PLANEJADA requirido.")] public DateTime ITC_ENTREGA_PLANEJADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORDEM ENTREGA")] [Required(ErrorMessage = "Campo ITC_ORDEM_ENTREGA requirido.")] public int ITC_ORDEM_ENTREGA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD PLANEJADA")] [Required(ErrorMessage = "Campo ITC_QTD_PLANEJADA requirido.")] public double ITC_QTD_PLANEJADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_ENTREGA_DE")] public DateTime ORD_DATA_ENTREGA_DE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_ENTREGA_ATE")] public DateTime ORD_DATA_ENTREGA_ATE { get; set; }

        [READ] [TAB(Value = "PRINCIPAL")] [Display(Name = "UF")] [MaxLength(2, ErrorMessage = "Maximode 2 caracteres, campo UF_COD")] public string UF_COD { get; set; }
        [READ] [TAB(Value = "PRINCIPAL")] [Display(Name = "MUNICÍPIO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MUN_NOME")] public string MUN_NOME { get; set; }
        [READ] [TAB(Value = "PRINCIPAL")] [Display(Name = "REGIÃO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PON_DESCRICAO")] public string PON_DESCRICAO { get; set; }
        [READ] [TAB(Value = "PRINCIPAL")] [Display(Name = "COD CLIENTE")] [Required(ErrorMessage = "Campo CLI_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo CLI_ID")] public string CLI_ID { get; set; }
        [READ] [TAB(Value = "PRINCIPAL")] [Display(Name = "CLIENTE")] [Required(ErrorMessage = "Campo CLI_NOME requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo CLI_NOME")] public string CLI_NOME { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ENTREGA_REALIZADA")] [Required(ErrorMessage = "Campo ITC_ENTREGA_REALIZADA requirido.")] public DateTime ITC_ENTREGA_REALIZADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD_REALIZADA")] [Required(ErrorMessage = "Campo ITC_QTD_REALIZADA requirido.")] public double ITC_QTD_REALIZADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "HASH_KEY")] [Required(ErrorMessage = "Campo ORD_HASH_KEY requirido.")] [MaxLength(300, ErrorMessage = "Maximode 300 caracteres, campo ORD_HASH_KEY")] public string ORD_HASH_KEY { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LARGURA_EMBALADA")] public double? PRO_LARGURA_EMBALADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COMPRIMENTO_EMBALADA")] public double? PRO_COMPRIMENTO_EMBALADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ALTURA_EMBALADA")] public double? PRO_ALTURA_EMBALADA { get; set; }

        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        public virtual Carga Carga { get; set; }
        public virtual Order Order { get; set; }
        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            List<object> itensCarga = new List<object>();
            List<object> otherObjects = new List<object>();
            foreach (var item in objects)
            {
                if (item.ToString() != "DynamicForms.Areas.PlugAndPlay.Models.V_ITEM_CARGA")
                {
                    otherObjects.Add(item);
                    continue;
                }

                V_ITEM_CARGA vItemCarga = (V_ITEM_CARGA)item;

                itensCarga.Add(this.V_ITEM_CARGAToItenCarga(vItemCarga));

                vItemCarga.PlayAction = "OK";
                otherObjects.Add(item);

            }
            objects.RemoveRange(0, objects.Count);
            objects.AddRange(itensCarga);
            //objects.AddRange(otherObjects);
            return true;
        }

        private ItenCarga V_ITEM_CARGAToItenCarga(V_ITEM_CARGA vItemCarga)
        {
            ItenCarga itemCarga = new ItenCarga();

            itemCarga.CAR_ID = vItemCarga.CAR_ID;
            itemCarga.ORD_ID = vItemCarga.ORD_ID;
            itemCarga.ITC_ENTREGA_PLANEJADA = vItemCarga.ITC_ENTREGA_PLANEJADA;
            itemCarga.ITC_ORDEM_ENTREGA = vItemCarga.ITC_ORDEM_ENTREGA;
            itemCarga.ITC_QTD_PLANEJADA = vItemCarga.ITC_QTD_PLANEJADA;
            itemCarga.ITC_ENTREGA_REALIZADA = vItemCarga.ITC_ENTREGA_REALIZADA;
            itemCarga.ITC_QTD_REALIZADA = vItemCarga.ITC_QTD_REALIZADA;
            itemCarga.ORD_HASH_KEY = vItemCarga.ORD_HASH_KEY;
            itemCarga.PlayAction = vItemCarga.PlayAction;

            return itemCarga;
        }
    }
}

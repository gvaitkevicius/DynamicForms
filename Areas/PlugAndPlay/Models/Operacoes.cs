
using DynamicForms.Models;
using DynamicForms.Util;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "OPERAÇÕES")]

    public class Operacoes
    {

        [TAB(Value = "PRINCIPAL")] [Display(Name = "CÓDIGO")] [Required(ErrorMessage = "Campo OPE_ID requirido.")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo OPE_ID")] public string OPE_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO REGISTRO")] [Required(ErrorMessage = "Campo OPE_TIPO_REGISTRO requirido.")] [MaxLength(2, ErrorMessage = "Maximode 2 caracteres, campo OPE_TIPO_REGISTRO")] public string OPE_TIPO_REGISTRO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD GRUPO DE MÁQUNA")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo GMA_ID")] public string GMA_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD MÁQUINA")] [Required(ErrorMessage = "Campo MAQ_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MAQ_ID")] public string MAQ_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PRODUTO")] [Required(ErrorMessage = "Campo PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ TRANSFORMAÇÃO")] [Required(ErrorMessage = "Campo ROT_SEQ_TRANSFORMACAO requirido.")] public int? ROT_SEQ_TRANFORMACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PEDIDO")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ REPETIÇÃO")] public int? FPR_SEQ_REPETICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "EXCEÇÃO")] public string OPE_EXCECAO { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        public virtual Roteiro Roteiro { get; set; }

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            foreach (var item in objects)
            {
                if (item.GetType().Name == "Operacoes")
                {
                    Operacoes operacao = (Operacoes)item;

                    if (operacao.PlayAction == "insert")
                    {
                        if (operacao.OPE_ID.Length < 3)
                        {
                            operacao.PlayMsgErroValidacao = "O código da operação precisa ter mais que 3 caracteres";
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}

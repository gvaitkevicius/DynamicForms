using DynamicForms.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class V_MOTIVOS_DE_REPROGRAMACAO
    {
        public V_MOTIVOS_DE_REPROGRAMACAO()
        {

        }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MAQ_ID")] public string MAQ_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ_REPETICAO")] public int? FPR_SEQ_REPETICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ_TRANFORMACAO")] public int? FPR_SEQ_TRANFORMACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OCO_ID_OP_PARCIAL")] [Required(ErrorMessage = "Campo MOV_OCO_ID_OP_PARCIAL requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_OCO_ID_OP_PARCIAL")] public string MOV_OCO_ID_OP_PARCIAL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OBS_OP_PARCIAL")] [Required(ErrorMessage = "Campo MOV_OBS_OP_PARCIAL requirido.")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo MOV_OBS_OP_PARCIAL")] public string MOV_OBS_OP_PARCIAL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO")] [Required(ErrorMessage = "Campo OCO_DESCRICAO requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo OCO_DESCRICAO")] public string OCO_DESCRICAO { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
    }
}

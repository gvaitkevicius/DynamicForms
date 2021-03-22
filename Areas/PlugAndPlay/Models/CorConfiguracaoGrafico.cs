using DynamicForms.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class CorConfiguracaoGrafico
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD COR")] [Required(ErrorMessage = "Campo COR_ID requirido.")] [MaxLength(2, ErrorMessage = "Maximode 2 caracteres, campo COR_ID")] public string COR_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PERCENTUAL INI")] [Required(ErrorMessage = "Campo COR_PERCENTUAL_INI requirido.")] public double COR_PERCENTUAL_INI { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PERCENTUAL FIM")] [Required(ErrorMessage = "Campo COR_PERCENTUAL_FIM requirido.")] public double COR_PERCENTUAL_FIM { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRIÇÃO")] [Required(ErrorMessage = "Campo COR_DESCRICAO requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo COR_DESCRICAO")] public string COR_DESCRICAO { get; set; }

        /// <summary>
        /// Esta propriedade foi criada para representar o estado do objeto
        /// insert, update, delete ou unchanged 
        /// </summary>
        [NotMapped]
        public string PlayAction { get; set; }

        /// <summary>
        /// Deve seguir a seguinte convecão: NameProperty:MsgErro;NameProperty:MsgErro; ...
        /// Representa os erros de validacao deste objeto
        /// </summary>
        [NotMapped]
        public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
    }
}
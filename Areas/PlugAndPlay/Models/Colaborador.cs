using DynamicForms.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class Colaborador
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CPF")] [Required(ErrorMessage = "Campo COL_CPF requirido.")] [MaxLength(14, ErrorMessage = "Maximode 14 caracteres, campo COL_CPF")] public string COL_CPF { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NOME")] [Required(ErrorMessage = "Campo COL_NOME requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo COL_NOME")] public string COL_NOME { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA NASCIMENTO")] [Required(ErrorMessage = "Campo COL_NASCIMENTO requirido.")] public DateTime COL_NASCIMENTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "EMAIL")] [Required(ErrorMessage = "Campo COL_EMAIL requirido.")] [MaxLength(150, ErrorMessage = "Maximode 150 caracteres, campo COL_EMAIL")] public string COL_EMAIL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "MATRICULA")] [Required(ErrorMessage = "Campo COL_MATRICULA requirido.")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo COL_MATRICULA")] public string COL_MATRICULA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TURMA")] [Required(ErrorMessage = "Campo TURM_id requirido.")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo TURM_id")] public string TURM_id { get; set; }
        public virtual Turma Turma { get; set; }

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
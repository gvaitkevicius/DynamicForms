
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    /*
     * Esta entidade foi criada para representar a relacao many-to-many entre as classes Feedback e MovimentoEstoque 
     * 
     */
    public class T_FeedbackMovEstoque
    {
        public T_FeedbackMovEstoque() { }
        public int FeedbackId { get; set; }
        public Feedback Feedback { get; set; }

        public int MovimentoEstoqueId { get; set; }
        public MovimentoEstoque MovimentoEstoque { get; set; }

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

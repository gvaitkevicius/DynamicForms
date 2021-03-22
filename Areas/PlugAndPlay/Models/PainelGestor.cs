using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class PainelGestor
    {
        public int MaqID { get; set; }
        public string MaqDescricao { get; set; }
        public string MaqStatus { get; set; }
        public string UltimaAtutlizacao { get; set; }
        public string OcorenciaID { get; set; }
        public string OcorenciaDescricao { get; set; }
        public string Obs { get; set; }
        public string StatusCor { get; set; }
        public string FeedbacksPendentes { get; set; }
        public string TempoSemFeedback { get; set; }
        public string OpsParciais { get; set; }

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
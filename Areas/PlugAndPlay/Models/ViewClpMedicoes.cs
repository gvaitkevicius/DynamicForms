using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class ViewClpMedicoes
    {
        public string MaquinaId { get; set; }
        public DateTime DataIni { get; set; }
        public DateTime DataFim { get; set; }
        public double Quantidade { get; set; }
        public double? Grupo { get; set; }
        public string TurnoId { get; set; }
        public string TurmaId { get; set; }
        public string FeedbackObs { get; set; }
        public string OcoId { get; set; }
        public int? FeedBackId { get; set; }
        public int? FeedBackIdMov { get; set; }
        public string Clp_Origem { get; set; }

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
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class VwPainelGestor
    {
        public double OrdQuantidade { get; set; }
        public DateTime OrdDataEntregaDe { get; set; }
        public DateTime OrdDataEntregaAte { get; set; }
        public string OrdTipo { get; set; }
        public double? OrdToleranciaMais { get; set; }
        public double? OrdToleranciaMenos { get; set; }
        //fila de producao
        public DateTime FprDataInicioPrevista { get; set; }
        public DateTime FprDataFimPrevista { get; set; }
        public DateTime FprDataFimMaxima { get; set; }
        public double FprQuantidadePrevista { get; set; }
        public int RotSeqTransformacao { get; set; }

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
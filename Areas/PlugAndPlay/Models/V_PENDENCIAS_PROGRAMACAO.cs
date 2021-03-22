using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class V_PENDENCIAS_PROGRAMACAO
    {
        public string ORD_ID { get; set; }
        public string PRO_ID { get; set; }
        public string T_PRO_ID { get; set; }
        public DateTime ENTREGA { get; set; }
        public double ORD_QUANTIDADE { get; set; }
        public string CLI_ID { get; set; }
        public string CLI_NOME { get; set; }
        public string ROTEIRO { get; set; }
        public string PRODUTO { get; set; }
        public string ESTRUTURA { get; set; }
        public string CLIENTE { get; set; }
        public string HORARIO_ENTREGA { get; set; }
        public string FILA { get; set; }
        public string ORD_M3_UNITARIO { get; set; }

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

        public V_PENDENCIAS_PROGRAMACAO() { }
    }
}

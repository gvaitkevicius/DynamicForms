using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class TPStatusCarga
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public TPStatusCarga()
        {

        }
        public TPStatusCarga(int id, string descricao)
        {
            Id = id;
            Descricao = descricao;
        }

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
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class ViewFeedback
    {
        public int Id { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime Datafinal { get; set; }
        public string Observacoes { get; set; }
        public double Grupo { get; set; }
        public string ProdutoId { get; set; }
        public string MaquinaId { get; set; }
        public string OcorrenciaId { get; set; }
        public string TurnoId { get; set; }
        public string TurmaId { get; set; }
        public string DiaTurma { get; set; }
        public int UsuarioId { get; set; }
        public string OrderId { get; set; }
        public int? SequenciaTransformacao { get; set; }
        public int? SequenciaRepeticao { get; set; }
        public double QuantidadePulsos { get; set; }
        public double? QuantidadePecasPorPulso { get; set; }
        public int? FeeIdMovEstoque { get; set; }
        public string ProdutoDescricao { get; set; }

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
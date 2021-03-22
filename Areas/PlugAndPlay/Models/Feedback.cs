using DynamicForms.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class Feedback
    {
        public Feedback()
        {
            /* Relacionamento many-to-may Entity Framework 6 */
            // MovimentosEstoque = new HashSet<MovimentoEstoque>();

            /* Relacionamento many-to-many Entity Framework Core 2*/
            T_FeedbackMovEstoque = new HashSet<T_FeedbackMovEstoque>();
        }
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
        public virtual Maquina Maquina { get; set; }
        public virtual Ocorrencia Ocorrencia { get; set; }
        public virtual Produto Produto { get; set; }
        public virtual Turno Turno { get; set; }
        public virtual Turma Turma { get; set; }
        public virtual Order Order { get; set; }
        public virtual T_Usuario Usuario { get; set; }

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

        //public virtual ICollection<MovimentoEstoque> MovimentosEstoque { get; set; } /* Relacionamento many-to-may Entity Framework 6 */
        public virtual ICollection<T_FeedbackMovEstoque> T_FeedbackMovEstoque { get; set; } /* Relacionamento many-to-many Entity Framework Core 2*/
    }
}
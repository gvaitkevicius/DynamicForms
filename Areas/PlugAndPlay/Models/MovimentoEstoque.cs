using DynamicForms.Areas.SGI.Models;
using DynamicForms.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class MovimentoEstoque
    {
        public MovimentoEstoque()
        {
            /* Relacionamento many-to-may Entity Framework 6 */
            // Feedbacks = new HashSet<Feedback>();

            /* Relacionamento many-to-many Entity Framework Core 2*/
            T_FeedbackMovEstoque = new HashSet<T_FeedbackMovEstoque>();

            TargetsProduto = new HashSet<TargetProduto>();
        }
        public int Id { get; set; }
        public double Quantidade { get; set; }
        public DateTime DataHoraEmissao { get; set; }
        public DateTime DataHoraCriacao { get; set; }
        public string DiaTurma { get; set; }
        public string Lote { get; set; }
        public string SubLote { get; set; }
        public string Observacao { get; set; }
        public string MaquinaId { get; set; }
        public string OcorrenciaId { get; set; }
        public int T_UsuarioId { get; set; }
        public string ProdutoId { get; set; }
        public string OrderId { get; set; }
        public string Tipo { get; set; }
        public string Armazem { get; set; }
        public string Endereco { get; set; }
        public string Estorno { get; set; }
        public int SequenciaTransformacao { get; set; }
        public int SequenciaRepeticao { get; set; }
        public string TurmaId { get; set; }
        public string TurnoId { get; set; }
        public string ObsOpParcial { get; set; }
        public string OcoIdOpParcial { get; set; }
        //public virtual Ocorrencia Ocorrencia { get; set; }
        //public virtual Ocorrencia OcorrenciaOpParcial { get; set; }
        public virtual Maquina Maquina { get; set; }
        public virtual T_Usuario Usuario { get; set; }
        public virtual Produto Produto { get; set; }
        public virtual Order Order { get; set; }
        //public virtual ICollection<Feedback> Feedbacks { get; set; } /* Relacionamento many-to-may Entity Framework 6 */
        public virtual ICollection<T_FeedbackMovEstoque> T_FeedbackMovEstoque { get; set; } /* Relacionamento many-to-many Entity Framework Core 2*/
        public virtual ICollection<TargetProduto> TargetsProduto { get; set; }

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
    }
}
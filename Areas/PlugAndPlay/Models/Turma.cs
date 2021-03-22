using DynamicForms.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class Turma
    {
        public Turma()
        {
            Calendarios = new HashSet<ItensCalendario>();
            Colaboradores = new HashSet<Colaborador>();
            Feedbacks = new HashSet<Feedback>();
            //Usuarios = new HashSet<T_Usuario>();
            TargetsProduto = new HashSet<TargetProduto>();
            MovimentoEstoqueVendas = new HashSet<MovimentoEstoqueVendas>();
            MovimentoEstoqueProducao = new HashSet<MovimentoEstoqueProducao>();
            MovimentoEstoque = new HashSet<MovimentoEstoque>();
            MovimentoEstoqueReservaDeEstoque = new HashSet<MovimentoEstoqueReservaDeEstoque>();
            MovimentoEstoqueTransferenciaSimples = new HashSet<MovimentoEstoqueTransferenciaSimples>();
            MovimentoEstoqueSaidaInventario = new HashSet<MovimentoEstoqueSaidaInventario>();
            MovimentoEstoqueEntradaInventario = new HashSet<MovimentoEstoqueEntradaInventario>();
            MovimentoEstoqueConsumoMateriaPrima = new HashSet<MovimentoEstoqueConsumoMateriaPrima>();
            TesteFisico = new HashSet<TesteFisico>();
        }
        [Required(ErrorMessage = "Campo ID requerido.")]
        [MaxLength(10, ErrorMessage = "Permitido no máximo 10 caracteres.")]
        [Display(Name = "ID")]
        public string Id { get; set; }
        [Required(ErrorMessage = "Campo Descricao requerido.")]
        [MaxLength(100, ErrorMessage = "Permitido no máximo 100 caracteres.")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        public virtual ICollection<ItensCalendario> Calendarios { get; set; }
        public virtual ICollection<Colaborador> Colaboradores { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<T_Usuario> Usuarios { get; set; }
        public virtual ICollection<TargetProduto> TargetsProduto { get; set; }
        public virtual ICollection<MovimentoEstoqueProducao> MovimentoEstoqueProducao { get; set; }
        public virtual ICollection<MovimentoEstoqueReservaDeEstoque> MovimentoEstoqueReservaDeEstoque { get; set; }
        public virtual ICollection<MovimentoEstoqueVendas> MovimentoEstoqueVendas { get; set; }
        public virtual ICollection<MovimentoEstoque> MovimentoEstoque { get; set; }
        public virtual ICollection<MovimentoEstoqueTransferenciaSimples> MovimentoEstoqueTransferenciaSimples { get; set; }
        public virtual ICollection<MovimentoEstoqueSaidaInventario> MovimentoEstoqueSaidaInventario { get; set; }
        public virtual ICollection<MovimentoEstoqueEntradaInventario> MovimentoEstoqueEntradaInventario { get; set; }
        public virtual ICollection<MovimentoEstoqueConsumoMateriaPrima> MovimentoEstoqueConsumoMateriaPrima { get; set; }
        public virtual ICollection<MovimentoEstoquePerdas> MovimentoEstoquePerdas { get; set; }
        public virtual ICollection<TesteFisico> TesteFisico { get; set; }


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
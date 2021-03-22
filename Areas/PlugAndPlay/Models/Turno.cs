using DynamicForms.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class Turno
    {
        public Turno()
        {
            Medicoes = new HashSet<Feedback>();
            Calendarios = new HashSet<ItensCalendario>();
            TargetsProduto = new HashSet<TargetProduto>();
            MovimentoEstoqueProducao = new HashSet<MovimentoEstoqueProducao>();
            MovimentoEstoqueReservaDeEstoque = new HashSet<MovimentoEstoqueReservaDeEstoque>();
            MovimentoEstoqueVendas = new HashSet<MovimentoEstoqueVendas>();
            MovimentoEstoque = new HashSet<MovimentoEstoque>();
            MovimentoEstoqueTransferenciaSimples = new HashSet<MovimentoEstoqueTransferenciaSimples>();
            MovimentoEstoqueSaidaInventario = new HashSet<MovimentoEstoqueSaidaInventario>();
            MovimentoEstoqueEntradaInventario = new HashSet<MovimentoEstoqueEntradaInventario>();
            MovimentoEstoqueConsumoMateriaPrima = new HashSet<MovimentoEstoqueConsumoMateriaPrima>();
            MovimentoEstoquePerdas = new HashSet<MovimentoEstoquePerdas>();
            TesteFisico = new HashSet<TesteFisico>();
        }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo TURN_ID requirido.")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo TURN_ID")] public string Id { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO")] [Required(ErrorMessage = "Campo TURN_DESCRICAO requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo TURN_DESCRICAO")] public string Descricao { get; set; }

        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  } 
        public virtual ICollection<Feedback> Medicoes { get; set; }
        public virtual ICollection<ItensCalendario> Calendarios { get; set; }
        public virtual ICollection<TargetProduto> TargetsProduto { get; set; }
        public virtual ICollection<MovimentoEstoqueProducao> MovimentoEstoqueProducao { get; set; }
        public virtual ICollection<MovimentoEstoqueReservaDeEstoque> MovimentoEstoqueReservaDeEstoque { get; set; }
        public virtual ICollection<MovimentoEstoqueVendas> MovimentoEstoqueVendas { get; set; }
        public virtual ICollection<MovimentoEstoque> MovimentoEstoque { get; set; }
        public virtual ICollection<MovimentoEstoqueTransferenciaSimples> MovimentoEstoqueTransferenciaSimples { get; set; }
        public virtual ICollection<MovimentoEstoqueSaidaInventario> MovimentoEstoqueSaidaInventario { get; set; }
        public virtual ICollection<MovimentoEstoqueEntradaInventario> MovimentoEstoqueEntradaInventario { get; set; }
        public virtual ICollection<MovimentoEstoquePerdas> MovimentoEstoquePerdas { get; set; }
        public virtual ICollection<MovimentoEstoqueConsumoMateriaPrima> MovimentoEstoqueConsumoMateriaPrima { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<TesteFisico> TesteFisico { get; set; }
        //public virtual ICollection<MovimentoEstoqueAbstrata> MovimentoEstoqueAbstrata { get; set; }


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
using DynamicForms.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public abstract class TipoMovimentoEstoque
    {
        public TipoMovimentoEstoque()
        {

        }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD TIPO")] [Required(ErrorMessage = "Campo TIP_ID requirido.")] [MaxLength(3, ErrorMessage = "Maximode 3 caracteres, campo TIP_ID")] public string TIP_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRIÇÃO")] [Required(ErrorMessage = "Campo TIP_DESCRICAO requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo TIP_DESCRICAO")] public string TIP_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SPR")] [Required(ErrorMessage = "Campo SPR requirido.")] public int SPR { get; set; } //Sistema proprietario: Indica se o valor do campo pode ou não ser manipulado
                                                                                                                                          //public int TIP_TYPE { get; set; }

        [NotMapped]
        public string PlayAction { get; set; }
        [NotMapped]
        public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
    }

    public class TipoMovEntradaProducao : TipoMovimentoEstoque
    {
        public TipoMovEntradaProducao()
        {
            MovimentoEstoqueProducao = new HashSet<MovimentoEstoqueProducao>();
        }
        public virtual ICollection<MovimentoEstoqueProducao> MovimentoEstoqueProducao { get; set; }
    }

    public class TipoMovEntradaInventario : TipoMovimentoEstoque
    {
        public TipoMovEntradaInventario()
        {
            MovimentoEstoqueEntradaInventario = new HashSet<MovimentoEstoqueEntradaInventario>();
        }

        public virtual ICollection<MovimentoEstoqueEntradaInventario> MovimentoEstoqueEntradaInventario { get; set; }
    }
    public class TipoMovSaidaInventario : TipoMovimentoEstoque
    {
        public TipoMovSaidaInventario()
        {
            MovimentoEstoqueSaidaInventario = new HashSet<MovimentoEstoqueSaidaInventario>();
        }
        public virtual ICollection<MovimentoEstoqueSaidaInventario> MovimentoEstoqueSaidaInventario { get; set; }
    }
    public class TipoMovEntradaCompras : TipoMovimentoEstoque { }
    public class TipoMovEntradaDevolucoes : TipoMovimentoEstoque { }
    public class TipoMovTransferenciaSimples : TipoMovimentoEstoque
    {
        public TipoMovTransferenciaSimples()
        {
            MovimentoEstoqueTransferenciaSimples = new HashSet<MovimentoEstoqueTransferenciaSimples>();
        }
        public virtual ICollection<MovimentoEstoqueTransferenciaSimples> MovimentoEstoqueTransferenciaSimples { get; set; }
    }
    public class TipoMovEntredaTransferenciaInterna : TipoMovimentoEstoque { }
    public class TipoMovSaidaTransferenciaInterna : TipoMovimentoEstoque { }
    public class TipoMovSaidaVendas : TipoMovimentoEstoque
    {
        public TipoMovSaidaVendas()
        {
            MovimentoEstoqueVendas = new HashSet<MovimentoEstoqueVendas>();
        }
        public virtual ICollection<MovimentoEstoqueVendas> MovimentoEstoqueVendas { get; set; }
    }
    public class TipoMovSaidaPerdas : TipoMovimentoEstoque
    {
        public TipoMovSaidaPerdas()
        {
            MovimentoEstoquePerdas = new HashSet<MovimentoEstoquePerdas>();
        }
        public virtual ICollection<MovimentoEstoquePerdas> MovimentoEstoquePerdas { get; set; }
    }
    public class TipoMovSaidaConsumo : TipoMovimentoEstoque
    {
        public TipoMovSaidaConsumo()
        {
            MovimentoEstoqueConsumoMateriaPrima = new HashSet<MovimentoEstoqueConsumoMateriaPrima>();
        }
        public virtual ICollection<MovimentoEstoqueConsumoMateriaPrima> MovimentoEstoqueConsumoMateriaPrima { get; set; }
    }
    public class TipoMovRetencao : TipoMovimentoEstoque
    {
        public TipoMovRetencao()
        {
            MovimentoEstoqueReservaDeEstoque = new HashSet<MovimentoEstoqueReservaDeEstoque>();
        }
        public virtual ICollection<MovimentoEstoqueReservaDeEstoque> MovimentoEstoqueReservaDeEstoque { get; set; }
    }
    public class TipoMovEstorno : TipoMovimentoEstoque { }
    /*
        >> ENTRADAS <<
        >  0   <  100 => PRODUCAO 
        >= 100 <= 199 => MOVIMENTOS INTERNOS GERALMENTE DE AJUSTES DE ESTOQUE  
        >= 200 <= 299 => ENTRADAS POR COMPRA 
        >= 300 <= 399 => ENTRADA POR INVENTARIO
        >= 400 <= 449 => ENTRADAS POR TRANSFORMAÇÃO
        >= 450 <= 499 => ENTRADAS POR DEVOLUCAO
        =  499        => ESTORNO DE SAIDA 

        >> SAÍDAS <<
        >= 500 <= 549 => PERDAS NA PRODUÇÃO
        >= 550 <= 559 => PERDAS NA MOVIMENTAÇÃO INTERNA
        >= 560 <= 599 => PERDAS NA MOVIMENTAÇÃO EXTERNA
        >= 600 <= 699 => SAIDAS POR INVENTARIO 
        >= 700 <= 799 => SAIDAS POR VENDAS
        >= 800 <= 899 => SAIDAS POR DESMONTAGEM
        =  998        => RESERVA / EMPENHO
        =  999        => ESTORNO DE ENTRADA

        >> OUTROS <<
        = 000         => PRÉ APONTAMENTO
        = 998         => RESERVA DE ESTOQUE
     
     */
}
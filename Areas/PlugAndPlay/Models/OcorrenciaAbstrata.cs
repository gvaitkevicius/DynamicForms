
using DynamicForms.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public abstract class OcorrenciaAbstrata
    {
        public OcorrenciaAbstrata() { }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD OCORRÊNCIA")] [Required(ErrorMessage = "OCO_DESCRICAO")] [MaxLength(30, ErrorMessage = "OCO_ID")] public string OCO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRIÇÃO")] [Required(ErrorMessage = "OCO_DESCRICAO")] [MaxLength(100, ErrorMessage = "OCO_DESCRICAO")] public string OCO_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD TIPO OCORRÊNCIA")] [Required(ErrorMessage = "TIP_ID")] public int TIP_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SUB TIPO")] public string OCO_SUB_TIPO { get; set; }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD GRUPO MÁQUINA")] public string GMA_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD MÁQUINA")] [MaxLength(30, ErrorMessage = "MAQ_ID")] public string MAQ_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "")] public int? SPR { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  } 

        public virtual GrupoMaquina GrupoMaquina { get; set; }
        public virtual Maquina Maquina { get; set; }
    }

    public class OcorrenciaPularOrdemFila : OcorrenciaAbstrata
    {
        public OcorrenciaPularOrdemFila()
        {
            FilaProducao = new HashSet<FilaProducao>();
        }

        public virtual ICollection<FilaProducao> FilaProducao { get; set; }
    }

    public class OcorrenciaMotivosDeParadas : OcorrenciaAbstrata
    {

    }

    public class OcorrenciaProducaoParciais : OcorrenciaAbstrata
    {
        public OcorrenciaProducaoParciais()
        {
            MovimentoEstoqueProducao = new HashSet<MovimentoEstoqueProducao>();
            MovimentoEstoque = new HashSet<MovimentoEstoque>();
            MovimentoEstoqueReservaDeEstoque = new HashSet<MovimentoEstoqueReservaDeEstoque>();
            MovimentoEstoqueVendas = new HashSet<MovimentoEstoqueVendas>();
            MovimentoEstoqueSaidaInventario = new HashSet<MovimentoEstoqueSaidaInventario>();
            MovimentoEstoqueEntradaInventario = new HashSet<MovimentoEstoqueEntradaInventario>();
            MovimentoEstoquePerdas = new HashSet<MovimentoEstoquePerdas>();
            MovimentoEstoqueConsumoMateriaPrima = new HashSet<MovimentoEstoqueConsumoMateriaPrima>();

        }
        public virtual ICollection<MovimentoEstoqueReservaDeEstoque> MovimentoEstoqueReservaDeEstoque { get; set; }
        public virtual ICollection<MovimentoEstoqueProducao> MovimentoEstoqueProducao { get; set; }
        public virtual ICollection<MovimentoEstoqueVendas> MovimentoEstoqueVendas { get; set; }
        public virtual ICollection<MovimentoEstoque> MovimentoEstoque { get; set; }
        public virtual ICollection<MovimentoEstoqueTransferenciaSimples> MovimentoEstoqueTransferenciaSimples { get; set; }
        public virtual ICollection<MovimentoEstoqueSaidaInventario> MovimentoEstoqueSaidaInventario { get; set; }
        public virtual ICollection<MovimentoEstoqueEntradaInventario> MovimentoEstoqueEntradaInventario { get; set; }
        public virtual ICollection<MovimentoEstoquePerdas> MovimentoEstoquePerdas { get; set; }
        public virtual ICollection<MovimentoEstoqueConsumoMateriaPrima> MovimentoEstoqueConsumoMateriaPrima { get; set; }

    }

    public class OcorrenciaTransporte : OcorrenciaAbstrata
    {
        public OcorrenciaTransporte()
        {
            Carga = new HashSet<Carga>();
            CargaPesagem = new HashSet<Carga>();
        }
        public virtual ICollection<Carga> Carga { get; set; }
        public virtual ICollection<Carga> CargaPesagem { get; set; }
    }

    public class OcorrenciaConsumoMateriaPrima : OcorrenciaAbstrata
    {
        public OcorrenciaConsumoMateriaPrima()
        {
            MovimentoEstoqueConsumoMateriaPrima = new HashSet<MovimentoEstoqueConsumoMateriaPrima>();
        }

        public virtual ICollection<MovimentoEstoqueConsumoMateriaPrima> MovimentoEstoqueConsumoMateriaPrima { get; set; }

    }

    public class OcorrenciaMotivosDasPerdas : OcorrenciaAbstrata
    {
        public OcorrenciaMotivosDasPerdas()
        {
            MovimentoEstoquePerdas = new HashSet<MovimentoEstoquePerdas>();
        }
        public virtual ICollection<MovimentoEstoquePerdas> MovimentoEstoquePerdas { get; set; }
    }

    public class OcorrenciaProducao : OcorrenciaAbstrata
    {
        public OcorrenciaProducao()
        {
            MovimentoEstoqueProducao = new HashSet<MovimentoEstoqueProducao>();
            MovimentoEstoqueTransferenciaSimples = new HashSet<MovimentoEstoqueTransferenciaSimples>();
        }
        public virtual ICollection<MovimentoEstoqueProducao> MovimentoEstoqueProducao { get; set; }
        public virtual ICollection<MovimentoEstoqueTransferenciaSimples> MovimentoEstoqueTransferenciaSimples { get; set; }

    }

    public class OcorrenciaRetencaoLotes : OcorrenciaAbstrata
    {
        public OcorrenciaRetencaoLotes()
        {
            MovimentoEstoqueReservaDeEstoque = new HashSet<MovimentoEstoqueReservaDeEstoque>();
        }
        public virtual ICollection<MovimentoEstoqueReservaDeEstoque> MovimentoEstoqueReservaDeEstoque { get; set; }
    }

    public class OcorrenciaVendas : OcorrenciaAbstrata
    {
        public OcorrenciaVendas()
        {
            MovimentoEstoqueVendas = new HashSet<MovimentoEstoqueVendas>();
        }
        public virtual ICollection<MovimentoEstoqueVendas> MovimentoEstoqueVendas { get; set; }
    }

    public class OcorrenciaSaidaInventario : OcorrenciaAbstrata
    {
        public OcorrenciaSaidaInventario()
        {
            MovimentoEstoqueSaidaInventario = new HashSet<MovimentoEstoqueSaidaInventario>();
        }
        public virtual ICollection<MovimentoEstoqueSaidaInventario> MovimentoEstoqueSaidaInventario { get; set; }
    }

    public class OcorrenciaEntradaInventario : OcorrenciaAbstrata
    {
        public OcorrenciaEntradaInventario()
        {
            MovimentoEstoqueEntradaInventario = new HashSet<MovimentoEstoqueEntradaInventario>();
        }
        public virtual ICollection<MovimentoEstoqueEntradaInventario> MovimentoEstoqueEntradaInventario { get; set; }
    }


    /*PARADAS NÃO PROGRAMADAS
    PARADA PROGRAMADA
    BAIXA PERFORMACE
    ALTA PERFORMACE
    PRODUCAO
    OP PARCIAL*/
}
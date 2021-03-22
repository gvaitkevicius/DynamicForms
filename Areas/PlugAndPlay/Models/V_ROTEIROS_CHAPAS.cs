using DynamicForms.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class V_ROTEIROS_CHAPAS
    {
        [Display(Name = "COD MÁQUINA")]
        [Required]
        [TAB(Value = "PRINCIPAL")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MAQ_ID")] public string MAQ_ID { get; set; }
        [Display(Name = "COD PRODUTO")]
        [Required]
        [TAB(Value = "PRINCIPAL")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [Required(ErrorMessage = "Sequencia de Transformação é a ordem em que o produto é produzido nas diferentes maquinas. A primeira maquina é sequencia 1 a segunda 2 e assim sucessivamente.")]
        [Display(Name = "SEQ TRANSFORMAÇÃO")]
        [TAB(Value = "PRINCIPAL")] public int ROT_SEQ_TRANFORMACAO { get; set; }
        [Required(ErrorMessage = "Performance deve ser preenchida. A performance é utilizada para definir a primera meta de performance bem como para calcular carga maquina. Este campo sera atualizado a cada produção.")]
        [Display(Name = "PERFORMANCE PÇ/SEG")]
        [Range(double.MinValue, double.PositiveInfinity, ErrorMessage = "A performance do roteiro não pode ser igual ou menor que 0.0 ")]
        [TAB(Value = "PRINCIPAL")] public double? ROT_PERFORMANCE { get; set; }
        [Combobox(Description = "ATIVA", Value = "A")]
        [Combobox(Description = "DESATIVADA", Value = "D")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "STATUS")] [MaxLength(2, ErrorMessage = "Maximode 2 caracteres, campo ROT_STATUS")] public string ROT_STATUS { get; set; }
        [Display(Name = "GRUPO MÁQUINAS")]
        [TAB(Value = "PRINCIPAL")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo GMA_ID")] public string GMA_ID { get; set; }
        //[Range(1, 50000, ErrorMessage = "Peças por Pulso representa, a quantidade de produtos produzidos a cada contagem do censor. Seu valor deve ser maior que zero.")]
        //[Required(ErrorMessage = "Peças por Pulso representa, a quantidade de produtos produzidos a cada contagem do censor. Seu valor deve ser maior que zero.")]
        [Display(Name = "QUANT PEÇAS/PULSO")]
        [TAB(Value = "PRINCIPAL")] public double? ROT_PECAS_POR_PULSO { get; set; }
        [Range(1, 5, ErrorMessage = "A prioridade deve ser entre 1 ate 5. Quanto menor maior a prioridade. Caso deixo todas as maquinas com mesma prioridade o sistema automaticamente verificará as disponibilidades e performances de cada maquina. ")]
        [Display(Name = "NÍVEL PRIORIDADE")]
        [TAB(Value = "PRINCIPAL")] public double? ROT_PRIORIDADE_INFORMADA { get; set; }
        [Display(Name = "MÁQUINA EXCEÇÃO")]
        [Combobox(Value = "", Description = "NÃO")]
        [Combobox(Value = "E", Description = "SIM")]
        [TAB(Value = "PRINCIPAL")] [MaxLength(2, ErrorMessage = "Maximode 2 caracteres, campo ROT_ACAO")] public string ROT_ACAO { get; set; }
        [Required(ErrorMessage = "Setup deve ser preenchido. O setup é utilizado para definir a primera meta de setup bem como para calcular carga maquina. Este campo sera atualizado a cada produção.")]
        [Display(Name = "Setup (tempo total em segundos)")]
        [TAB(Value = "PRINCIPAL")] public double? ROT_TEMPO_SETUP { get; set; }
        [Required(ErrorMessage = "Setup Ajuste deve ser preenchido. O setup Ajuste é utilizado para definir a primera meta de setup bem como para calcular carga maquina. Este campo sera atualizado a cada produção.")]
        [Display(Name = "TEMPO SETUP AJUSTE SEG")]
        [TAB(Value = "PRINCIPAL")] public double? ROT_TEMPO_SETUP_AJUSTE { get; set; }
        [Display(Name = "PRÓXIMA SEQ TRNSFORM")]
        [TAB(Value = "PRINCIPAL")] public int? ROT_VA_PARA_SEQ_TRANSFORMACAO { get; set; }
        [Display(Name = "HIERARQUIA CALCULO")]
        [TAB(Value = "PRINCIPAL")] public double? ROT_HIERARQUIA_SEQ_TRANSFORMACAO { get; set; }
        [Display(Name = "AVALIA CUSTO")]
        [TAB(Value = "PRINCIPAL")] public int? ROT_AVALIA_CUSTO { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  } 

        public virtual ProdutoChapaIntermediaria ProdutoChapaIntermediaria { get; set; }
        public virtual Maquina Maquina { get; set; }
        public virtual GrupoMaquina GrupoMaquina { get; set; }
    }
}

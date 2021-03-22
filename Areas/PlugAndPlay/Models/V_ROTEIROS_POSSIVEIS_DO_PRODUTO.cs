using DynamicForms.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class V_ROTEIROS_POSSIVEIS_DO_PRODUTO
    {
        public V_ROTEIROS_POSSIVEIS_DO_PRODUTO()
        {
            MaquiasEquivalentes = new List<V_ROTEIROS_POSSIVEIS_DO_PRODUTO>();
        }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "US_CADASTRO")] [Required(ErrorMessage = "Campo STATUS_CADASTRO requirido.")] [MaxLength(8, ErrorMessage = "Maximode 8 caracteres, campo STATUS_CADASTRO")] public string STATUS_CADASTRO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO_PLANEJAMENTO")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo MAQ_TIPO_PLANEJAMENTO")] public string MAQ_TIPO_PLANEJAMENTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "IA_CUSTO")] public int? AVALIA_CUSTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ENTUAL_INICIO_PASSO_ANTERIOR")] public double? PERCENTUAL_INICIO_PASSO_ANTERIOR { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo CAL_ID requirido.")] public int CAL_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo MAQ_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MAQ_ID")] public string MAQ_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ_TRANFORMACAO")] [Required(ErrorMessage = "Campo ROT_SEQ_TRANFORMACAO requirido.")] public int ROT_SEQ_TRANFORMACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ARQUIA_SEQ_TRANSFORMACAO")] public double? HIERARQUIA_SEQ_TRANSFORMACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VA_PARA_SEQ_TRANSFORMACAO")] public int? ROT_VA_PARA_SEQ_TRANSFORMACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PERFORMANCE")] public double? ROT_PERFORMANCE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TEMPO_SETUP")] public double? ROT_TEMPO_SETUP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TEMPO_SETUP_AJUSTE")] public double? ROT_TEMPO_SETUP_AJUSTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PECAS_POR_PULSO")] public double? ROT_PECAS_POR_PULSO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRIORIDADE_INFORMADA")] public double? ROT_PRIORIDADE_INFORMADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "MAQ_LARGURA_UTIL")] public double? MAQ_LARGURA_UTIL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "GRP_PERFORMANCE_METRO_LINEAR_POR_SEGUNDO")] public double? GRP_PERFORMANCE_METRO_LINEAR_POR_SEGUNDO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "GRP_TIPO")] public double? GRP_TIPO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "STATUS")] [MaxLength(2, ErrorMessage = "Maximode 2 caracteres, campo ROT_STATUS")] public string ROT_STATUS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OPERACOES")] [Required(ErrorMessage = "Campo ROT_OPERACOES requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo ROT_OPERACOES")] public string ROT_OPERACOES { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "EXCECAO_OPERACOES")] [Required(ErrorMessage = "Campo ROT_EXCECAO_OPERACOES requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo ROT_EXCECAO_OPERACOES")] public string ROT_EXCECAO_OPERACOES { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ROT_LINHA_DIRETA")] [Required(ErrorMessage = "Campo ROT_LINHA_DIRETA requirido.")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo ROT_LINHA_DIRETA")] public string ROT_LINHA_DIRETA { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        public virtual Maquina Maquina { get; set; }
        public virtual Produto Produto { get; set; }
        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs, ref int modo_insert) {  } 
        [NotMapped]
        public List<V_ROTEIROS_POSSIVEIS_DO_PRODUTO> MaquiasEquivalentes { get; set; }
    }
}

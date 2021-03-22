using DynamicForms.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "CUSTO ENTRE OPs")]
    public class CustoEntreOps
    {
        [TAB(Value = "PRINCIPAL")] [READ] [Display(Name = "COD CUSTO UNI")] [Required(ErrorMessage = "Campo CUS_UNIC_ID requirido.")] public int CUS_UNIC_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD CUSTO")] [Required(ErrorMessage = "Campo CUS_ID requirido.")] [MaxLength(150, ErrorMessage = "Maximode 50 caracteres, campo CUS_ID")] public string CUS_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PRODUTO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD GRUPO PRODUTO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo GRP_ID")] public string GRP_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD MÁQUINA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MAQ_ID")] public string MAQ_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRIÇÃO")] [MaxLength(3000, ErrorMessage = "Maximode * caracteres, campo CUS_DESCRICAO")] public string CUS_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PESO CASO VERDADEIRO")] public double? CUS_PESO_CASO_VERDADEIRO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PESO CASO FALSO")] public double? CUS_PESO_CASO_FALSO { get; set; }
        [Combobox(Description = "VALOR IGUAL", Value = "=")]
        [Combobox(Description = "VALOR DO ITEM A SER AVALIADO", Value = "V")]
        [Combobox(Description = "BINÁRIA", Value = "B")]
        [Combobox(Description = "APROXIMACAO", Value = "A")]
        [Combobox(Description = "ENTRE VALORES", Value = "E")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO AVALIAÇÃO")] [Required(ErrorMessage = "Campo CUS_TIPO_AVALIACAO requirido.")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo CUS_TIPO_AVALIACAO")] public string CUS_TIPO_AVALIACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TEMPO SETUP")] [MaxLength(50, ErrorMessage = "Maximode 50 caracteres, campo CUS_GRUPO_TEMPO_SETUP")] public string CUS_GRUPO_TEMPO_SETUP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TEMPO CASO VERDADEIRO")] public double? CUS_TEMPO_CASO_VERDADEIRO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TEMPO CASO FALSO")] public double? CUS_TEMPO_CASO_FALSO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CUSTO OPERACOES")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo CUS_OPERACOES")] public string CUS_OPERACOES { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) 
        //{
        //    foreach (var item in objects)
        //    {
        //        CustoEntreOps cus = (CustoEntreOps) item;
        //        if (cus.PlayAction.ToUpper() == "INSERT")
        //        {
        //            string aux_id = $"{cus.PRO_ID};{cus.GRP_ID};{cus.MAQ_ID}|{cus.CUS_ID}";
        //            cus.CUS_ID = aux_id;
        //        }
        //    }
        //    return true;
        //} 

        public virtual Produto Produto { get; set; }
        public virtual GrupoProdutoOutros GrupoProdutoOutros { get; set; }
        public virtual Maquina Maquina { get; set; }
    }

    public class EscalaDeCores
    {
        public string PRO_ID { get; set; }
        public string PRO_ESCALA_COR { get; set; }
        public double PRO_CUSTO_SUBIDA_ESCALA_COR { get; set; }
        public double PRO_CUSTO_DECIDA_ESCALA_COR { get; set; }

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
    public class ParametroDeCusto
    {
        public string PRO_ID { get; set; }
        public int PAR_ID { get; set; }
        public string CUS_ID { get; set; }
        public string PAR_VALOR { get; set; }

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
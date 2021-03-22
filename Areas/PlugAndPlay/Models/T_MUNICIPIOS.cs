using DynamicForms.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "MUNICÍPIOS")]
    public class Municipio
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD MUNICÍPIO")] [Required(ErrorMessage = "Campo MUN_ID requirido.")] [MaxLength(50, ErrorMessage = "Maximode 50 caracteres, campo MUN_ID")] public string MUN_ID { get; set; }
        [SEARCH] [TAB(Value = "PRINCIPAL")] [Display(Name = "MUNICÍPIO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MUN_NOME")] public string MUN_NOME { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "UF")] [MaxLength(2, ErrorMessage = "Maximode 2 caracteres, campo UF_COD")] public string UF_COD { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD IBGE")] [MaxLength(50, ErrorMessage = "Maximode 50 caracteres, campo MUN_CODIGO_IBGE")] public string MUN_CODIGO_IBGE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LATITUDE")] public double? MUN_LATITUDE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LONGITUDE")] public double? MUN_LONGITUDE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD INTEGRACAO ERP")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MUN_ID_INTEGRACAO_ERP")] public string MUN_ID_INTEGRACAO_ERP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD SIAFI")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MUN_CODIGO_SIAFI")] public string MUN_CODIGO_SIAFI { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CNPJ")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MUN_CODIGO_CNPJ")] public string MUN_CODIGO_CNPJ { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DISTANCIA KM")]  public double MUN_DISTANCIA_KM { get; set; }
        
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  } 

        public ICollection<Cliente> Clientes { get; set; }
        public ICollection<Order> Order { get; set; }

        public Municipio()
        {
            MUN_ID = "";
            MUN_NOME = "";
            UF_COD = "";
            MUN_CODIGO_IBGE = "";
            Clientes = new HashSet<Cliente>();
            Order = new HashSet<Order>();
        }
        public Municipio(string id, string nome, string uf)
        {
            MUN_ID = id;
            MUN_NOME = nome;
            UF_COD = uf;
            MUN_CODIGO_IBGE = "";
        }
        public Municipio(string id, string nome, string uf, string codigo_ibge, double latitute, double longitude)
        {
            MUN_ID = id;
            MUN_NOME = nome;
            UF_COD = uf;
            MUN_CODIGO_IBGE = codigo_ibge;
            MUN_LATITUDE = latitute;
            MUN_LONGITUDE = MUN_LONGITUDE;
        }
        public Municipio(string id, string nome, string uf, string codigo_ibge, double latitute, double longitude, string idIntegracao)
        {
            MUN_ID = id;
            MUN_NOME = nome;
            UF_COD = uf;
            MUN_CODIGO_IBGE = codigo_ibge;
            MUN_LATITUDE = latitute;
            MUN_LONGITUDE = MUN_LONGITUDE;
            MUN_ID_INTEGRACAO_ERP = idIntegracao;
        }


    }
}
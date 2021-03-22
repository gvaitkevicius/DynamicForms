using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Models;
using DynamicForms.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "CLIENTES")]
    public class Cliente
    {
        public Cliente()
        {
            Ordens = new HashSet<Order>();
            HorariosRecebimentos = new HashSet<T_HORARIO_RECEBIMENTO>();
            Observacoes = new HashSet<Observacoes>();
            EstruturaEtiqueta = new HashSet<EstruturaEtiqueta>();
        }

        [TAB(Value = "PRINCIPAL")]
        [Display(Name = "COD CLIENTE")]
        [Required(ErrorMessage = "Campo CLI_ID requirido.")]
        [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo CLI_ID")] public string CLI_ID { get; set; }

        [SEARCH]
        [TAB(Value = "PRINCIPAL")]
        [Display(Name = "NOME")]
        [Required(ErrorMessage = "Campo CLI_NOME requirido.")]
        [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo CLI_NOME")] public string CLI_NOME { get; set; }

        [TAB(Value = "PRINCIPAL")]
        [Display(Name = "FONE")]
        [MaxLength(68, ErrorMessage = "Maximode 68 caracteres, campo CLI_FONE")]
        public string CLI_FONE { get; set; }

        [TAB(Value = "PRINCIPAL")]
        [Display(Name = "OBSERVAÇÃO")]
        [MaxLength(3000, ErrorMessage = "Maximode * caracteres, campo CLI_OBS")]
        public string CLI_OBS { get; set; }

        [TAB(Value = "Endereço")]
        [Display(Name = "ENDEREÇO")]
        [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo CLI_ENDERECO_ENTREGA")]
        public string CLI_ENDERECO_ENTREGA { get; set; }

        [TAB(Value = "Endereço")]
        [Display(Name = "BAIRRO")]
        [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo CLI_BAIRRO_ENTREGA")]
        public string CLI_BAIRRO_ENTREGA { get; set; }

        [TAB(Value = "Endereço")]
        [Display(Name = "CEP")]
        [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo CLI_CEP_ENTREGA")]
        public string CLI_CEP_ENTREGA { get; set; }

        [TAB(Value = "PRINCIPAL")]
        [Display(Name = "E-MAIL")]
        [MaxLength(3000, ErrorMessage = "Maximode * caracteres, campo CLI_EMAIL")]
        public string CLI_EMAIL { get; set; }

        [TAB(Value = "PRINCIPAL")]
        [Display(Name = "CPF/CNPJ")]
        [MaxLength(18, ErrorMessage = "Maximode 18 caracteres, campo CLI_CPF_CNPJ")]
        public string CLI_CPF_CNPJ { get; set; }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "TEMPO TRANSLADO")] public double? CLI_TRANSLADO { get; set; }

        [TAB(Value = "PRINCIPAL")]
        [Display(Name = "COD INTEGRAÇÃO")]
        [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo CLI_INTEGRACAO")]
        public string CLI_INTEGRACAO { get; set; }

        [TAB(Value = "Endereço")]
        [Display(Name = "MUNICÍPIO")]
        [Required(ErrorMessage = "Campo MUN_ID requerido.")]
        [MaxLength(50, ErrorMessage = "Maximode 50 caracteres, campo MUN_ID_ENTREGA")]
        public string MUN_ID { get; set; }

        [TAB(Value = "Outras informações")]
        [Display(Name = "REGIÃO DE ENTREGA")]
        [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo CLI_REGIAO_ENTREGA")] public string CLI_REGIAO_ENTREGA { get; set; }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "CLIENTE EXIGENTE")] public int? CLI_EXIGENTE_NA_IMPRESSAO { get; set; }

        [TAB(Value = "PRINCIPAL")]
        [Display(Name = "TEMPO ESPERA DESCARREGAMENTO:")]
        public double? CLI_TEMPO_MEDIO_ESPERA_DE_DESCARREGAMENTO { get; set; }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "TEMPO DESCARREGAMENTO/UNI")] public double? CLI_TEMPO_DESCARREGAMENTO_UNITARIO { get; set; }

        [HIDDEN]
        [TAB(Value = "PRINCIPAL")]
        [Display(Name = "PERCENTUAL JANELA EMBARQUE")]
        public double? CLI_PERCENTUAL_JANELA_EMBARQUE { get; set; }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "REPRESENTANTE ID")] public int? REP_ID { get; set; }
        public virtual Representantes Representantes { get; set; }

        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs, int modo_insert) {  } 

        public Municipio Municipio { get; set; }

        public ICollection<Order> Ordens { get; set; }

        public ICollection<T_HORARIO_RECEBIMENTO> HorariosRecebimentos { get; set; }

        public ICollection<Observacoes> Observacoes { get; set; }
        public ICollection<EstruturaEtiqueta> EstruturaEtiqueta { get; set; }
        //METODOS DE CLASSE
        public bool Inserir_Horarios_Recebimento(List<object> objects, ref List<LogPlay> Logs)
        {
            List<object> ObjetosProcessados = new List<object>();
            List<List<object>> ListObjectsToUpdate = new List<List<object>>();
            MasterController mc = new MasterController();
            //Criando um objeto para a nova carga
            string CliId = null;
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                //Para cada item da lista
                foreach (var item in objects)
                {
                    Cliente _Cliente = (Cliente)item;
                    CliId = _Cliente.CLI_ID;
                    _Cliente.PlayAction = "OK";
                    ObjetosProcessados.Add(_Cliente);
                }
            }
            ListObjectsToUpdate.Add(ObjetosProcessados);
            //Concatenando Logs por se tratar de um objeto de interface
            Logs.Add(new LogPlay(this.ToString(), "PROTOCOLO", "LINK", "/PlugAndPlay/HorariosRecebimento/Create?idCliente=", "" + CliId + ""));
            Logs.AddRange(mc.UpdateData(ListObjectsToUpdate, 4, true));

            return true;
        }

    }

    public class T_HORARIO_RECEBIMENTO
    {
        [TAB(Value = "PRINCIPAL")]
        [READ]
        [Display(Name = "COD HORÁRIO RECEBIMENTO")]
        [Required(ErrorMessage = "Campo HRE_ID requirido.")]
        public int HRE_ID { get; set; }

        [TAB(Value = "PRINCIPAL")]
        [Display(Name = "COD CLIENTE")]
        [Required(ErrorMessage = "Campo CLI_ID requirido.")]
        [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo CLI_ID")] public string CLI_ID { get; set; }

        [TAB(Value = "PRINCIPAL")]
        [Display(Name = "DIA DA SEMANA")]
        [Required(ErrorMessage = "Campo HRE_DIA_DA_SEMANA requirido.")]
        [Combobox(Description = "Segunda", Value = "2")]
        [Combobox(Description = "Terça", Value = "3")]
        [Combobox(Description = "Quarta", Value = "4")]
        [Combobox(Description = "Quinta", Value = "5")]
        [Combobox(Description = "Sexta", Value = "6")]
        [Combobox(Description = "Sabado", Value = "7")]
        [Combobox(Description = "Domingo", Value = "1")]
        public int HRE_DIA_DA_SEMANA { get; set; }

        [TAB(Value = "PRINCIPAL")]
        [Display(Name = "HORA INICIAL")]
        [Required(ErrorMessage = "Campo HRE_HORA_INICIAL requirido.")]
        public DateTime HRE_HORA_INICIAL { get; set; }

        [TAB(Value = "PRINCIPAL")]
        [Display(Name = "HORA FINAL")]
        [Required(ErrorMessage = "Campo HRE_HORA_FINAL requirido.")]
        public DateTime HRE_HORA_FINAL { get; set; }

        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs, int modo_insert) {  } 

        public Cliente Cliente { get; set; }
    }
}



using DynamicForms.Models;
using DynamicForms.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "CONSULTA DE PEDIDO")]
    public class V_CONSULTA_PEDIDO
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NOME DO CLIENTE")] [Required(ErrorMessage = "Campo CLI_NOME requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo CLI_NOME")] public string CLI_NOME { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO DO PRODUTO")] [Required(ErrorMessage = "Campo PRO_DESCRICAO requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PRO_DESCRICAO")] public string PRO_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PEDIDO")] [Required(ErrorMessage = "Campo ORD_ID requirido.")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD CLIENTE")] [Required(ErrorMessage = "Campo CLI_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo CLI_ID")] public string CLI_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PRODUTO")] [Required(ErrorMessage = "Campo PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE")] [Required(ErrorMessage = "Campo ORD_QUANTIDADE requirido.")] public double ORD_QUANTIDADE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA DE ENTREGA")] [Required(ErrorMessage = "Campo ORD_DATA_ENTREGA_DE requirido.")] public DateTime ORD_DATA_ENTREGA_DE { get; set; }
        [Combobox(Value = "1", Description = "PRODUÇÃO E EXPEDIÇÃO")]
        [Combobox(Value = "2", Description = "SOMENTE PRODUÇÃO")]
        [Combobox(Value = "3", Description = "SOMENTE EXPEDIÇÃO")]
        [Combobox(Value = "4", Description = "RETRABALHO E EXPEDIÇÃO")]
        [Combobox(Value = "5", Description = "SOMENTE RETRABALHO")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO PEDIDO")] public int? ORD_TIPO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NUM.OP")] public string ORD_OP_INTEGRACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PEDIDO CLIENTE")] public string ORD_PED_CLI { get; set; }
        [Combobox(Description = "ABERTO ", Value = "")]
        [Combobox(Value = "SS", Description = "SUSPENSO PRODUÇÃO E EXPEDIÇÃO")]
        // valida se não esta dentro dos congelados, ja produzido ou em carga consolidada
        // filtra fila de producao na view 
        // filtra V_ops_A_planejar na aplicação 
        // Filtra v_pedidos_a_planejar_expedicao na view
        [Combobox(Value = "R1", Description = "RESERVA PRODUÇÃO 1")]
        [Combobox(Value = "R2", Description = "RESERVA PRODUÇÃO 2")]

        [Combobox(Value = "SE", Description = "SUSPENSO EXPEDIÇÃO")]
        // valida se não esta em carga consolidada
        // Filtra v_pedidos_a_planejar_expedicao na view 

        [Combobox(Value = "E", Description = "ENCERRADO")]
        [Combobox(Value = "EI", Description = "ENCERRADO PELA INTERFACE")]
        [Combobox(Value = "EC", Description = "ENCERRADO POR CANCELAMENTO")]
        [Combobox(Value = "EV", Description = "ENCERRADO RESERVA VIROU PEDIDO")]
        [Combobox(Value = "E1", Description = "ENCERRADO RESERVA 1 EXPIROU ")]
        [Combobox(Value = "E2", Description = "ENCERRADO RESERVA 2 EXPIROU ")]

        [TAB(Value = "PRINCIPAL")] [Display(Name = "STATUS PEDIDO")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo ORD_STATUS")] public string ORD_STATUS { get; set; }
        [TextArea] [TAB(Value = "PRINCIPAL")] [Display(Name = "OBSERVAÇÃO DO OTIMIZADOR")] [MaxLength(1000, ErrorMessage = "Maximo de 250 caracteres, campo ORD_ID_INTEGRACAO")] public string ORD_OBSERVACAO_OTIMIZADOR { get; set; }
        [Combobox(Description = "NÃO PRIORIZAR", ValueInt = 0)]
        [Combobox(Description = "ULTRAPASSA CONGELADAS", ValueInt = 1)]
        [Combobox(Description = "LOGO APÓS CONGELADAS", ValueInt = 2)]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRIORIDADE")] public int ORD_PRIORIDADE { get; set; }

        [Combobox(Description = "NÃO", ValueInt = 0)]
        [Combobox(Description = "SIM", ValueInt = 1)]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LOTE PILOTO")] public int ORD_LOTE_PILOTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA DE EMISSÃO")] public DateTime? ORD_EMISSAO { get; set; }


        /* entrega */
        [TAB(Value = "ENTREGA")] [Display(Name = "ENDEREÇO")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo ORD_ENDERECO_ENTREGA")] public string ORD_ENDERECO_ENTREGA { get; set; }
        [TAB(Value = "ENTREGA")] [Display(Name = "BAIRRO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo ORD_BAIRRO_ENTREGA")] public string ORD_BAIRRO_ENTREGA { get; set; }
        [TAB(Value = "ENTREGA")] [Display(Name = "UF")] [MaxLength(2, ErrorMessage = "Maximode 2 caracteres, campo UF_ID_ENTREGA")] public string UF_ID_ENTREGA { get; set; }
        [TAB(Value = "ENTREGA")] [Display(Name = "CEP")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo ORD_CEP_ENTREGA")] public string ORD_CEP_ENTREGA { get; set; }
        [Required(ErrorMessage = "Campo ORD_CEP_ENTREGA requerido.")]
        [TAB(Value = "ENTREGA")] [Display(Name = "COD MUNICÍPIO")] [MaxLength(50, ErrorMessage = "Maximo de 50 caracteres, campo MUN_ID_ENTREGA")] public string MUN_ID_ENTREGA { get; set; }
        [TAB(Value = "ENTREGA")] [Display(Name = "REGIÃO")] [MaxLength(100, ErrorMessage = "Maximo de 100 caracteres, campo ORD_REGIAO_ENTREGA")] public string ORD_REGIAO_ENTREGA { get; set; }
        /* outros */
        [TAB(Value = "OUTROS")] [Display(Name = "PREÇO UNI")] public double? ORD_PRECO_UNITARIO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "PESO UNI")] public double? ORD_PESO_UNITARIO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "PESO UNI BRUTO")] public double? ORD_PESO_UNITARIO_BRUTO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "ÁREA UNI (M²)")] public double? ORD_M2_UNITARIO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "EMBARQUE ALVO")] public DateTime ORD_EMBARQUE_ALVO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "INÍCIO JANELA EMB")] public DateTime ORD_INICIO_JANELA_EMBARQUE { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "FIM JANELA EMB")] public DateTime ORD_FIM_JANELA_EMBARQUE { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "INICIO GRUPO PRODUTIVO")] public DateTime ORD_INICIO_GRUPO_PRODUTIVO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "FIM GRUPO PRODUTIVO")] public DateTime ORD_FIM_GRUPO_PRODUTIVO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "TIPO CARREGAMENTO")] [MaxLength(50, ErrorMessage = "Maximode 50 caracteres, campo CAR_TIPO_CARREGAMENTO")] public string CAR_TIPO_CARREGAMENTO { get; set; }
        [Combobox(Value = "FOB", Description = "FOB")]
        [Combobox(Value = "CIF", Description = "CIF")]
        [TAB(Value = "OUTROS")] [Display(Name = "FRETE")] [MaxLength(3, ErrorMessage = "Maximode 3 caracteres, campo ORD_TIPO_FRETE")] public string ORD_TIPO_FRETE { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "LARGURA")] public double? ORD_LARGURA { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "COMPRIMENTO")] public double? ORD_COMPRIMENTO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "GRAMATURA")] public double? ORD_GRAMATURA { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "COD GRUPO PRODUTO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo GRP_ID")] public string GRP_ID { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "COD CONJ PEDIDO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo ORD_ID_CONJUNTO")] public string ORD_ID_CONJUNTO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "COD CONJ PRODUTO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID_CONJUNTO")] public string PRO_ID_CONJUNTO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "COD INTEGRACAO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo ORD_ID_INTEGRACAO")] public string ORD_ID_INTEGRACAO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "ENTREGA ATE")] public DateTime ORD_DATA_ENTREGA_ATE { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "MIT")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo ORD_MIT")] public string ORD_MIT { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "TOL +")] public double? ORD_TOLERANCIA_MAIS { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "TOL -")] public double? ORD_TOLERANCIA_MENOS { get; set; }

        [Combobox(Value = "", Description = "Atualiza pelo sistema de origem")]
        [Combobox(Value = "-1", Description = "Atualiza pelo PLAYSIS")]
        [TAB(Value = "OUTROS")] [Display(Name = "Interface")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo HASH_KEY")] public string HASH_KEY { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "REPRESENTANTE ID")] public int? REP_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID_RESERVA")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID_RESERVA")] public string ORD_ID_RESERVA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COR_FILA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo ORD_COR_FILA")] public string ORD_COR_FILA { get; set; }


        //RELAÇÕES
        [Display(Name = "ESTOQUE")] public virtual ICollection<SaldosEmEstoquePorLote> SaldosEmEstoquePorLote { get; set; }
        [Display(Name = "NFs")] public virtual ICollection<V_INPUT_NFS> NFS { get; set; }

        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        public V_CONSULTA_PEDIDO()
        {
            SaldosEmEstoquePorLote = new HashSet<SaldosEmEstoquePorLote>();
            NFS = new HashSet<V_INPUT_NFS>();
        }

        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs, ref int modo_insert) {  } 
    }
}

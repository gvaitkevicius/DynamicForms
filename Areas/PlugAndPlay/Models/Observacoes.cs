using DynamicForms.Models;
using DynamicForms.Util;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "OBSERVAÇÕES")]
    public class Observacoes
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CODIGO")] [READ] [Required(ErrorMessage = "Campo OBS_ID requirido.")] public int OBS_ID { get; set; }
        [Combobox(Description = "FATURAMENTO", Value = "F")]
        [Combobox(Description = "OBS. GERAIS DE PRODUCAO", Value = "PG")]
        [Combobox(Description = "PRODUÇÃO ONDULADEIRA", Value = "PO")]
        [Combobox(Description = "PRODUÇÃO CONVERSÃO", Value = "PC")]
        [Combobox(Description = "PRODUÇÃO ACABAMENTO", Value = "PA")]
        [Combobox(Description = "ENGENHARIA", Value = "E")]
        [Combobox(Description = "EXPEDICAO", Value = "EP")]
        [Combobox(Description = "ACABAMENTO", Value = "AC")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo OBS_TIPO")] public string OBS_TIPO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO")] [MaxLength(300, ErrorMessage = "Maximode 300 caracteres, campo OBS_DESCRICAO")] public string OBS_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD CLIENTE")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo CLI_ID")] public string CLI_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD MAQUINA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MAQ_ID")] public string MAQ_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PRODUTO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ_TRANSFORMACAO")] public int? ROT_SEQ_TRANFORMACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "INTEGRAÇÃO")] [MaxLength(50, ErrorMessage = "Maximode 50 caracteres, campo OBS_INTEGRACAO")] public string OBS_INTEGRACAO { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            foreach (object obj in objects)
            {
                Observacoes observacoes = (Observacoes)obj;
                if (observacoes.CLI_ID != "" || observacoes.MAQ_ID == "" || observacoes.PRO_ID == "" || observacoes.ROT_SEQ_TRANFORMACAO != 0)
                {
                    if (observacoes.CLI_ID == "")
                        observacoes.CLI_ID = null;
                    if (observacoes.MAQ_ID == "")
                        observacoes.MAQ_ID = null;
                    if (observacoes.PRO_ID == "")
                        observacoes.PRO_ID = null;
                    if (observacoes.ROT_SEQ_TRANFORMACAO == 0)
                        observacoes.ROT_SEQ_TRANFORMACAO = null;
                }
                else
                {
                    observacoes.PlayMsgErroValidacao = "Um dos seguintes campos devem ser preenchidos: COD CLIENTE, COD MAQUINA, COD PRODUTO ou ROT_SEQ_TRANSFORMACAO!";
                    return false;
                }

            }
            return true;
        }

        public Cliente Cliente { get; set; }
        public Roteiro Roteiro { get; set; }
        public Produto Produto { get; set; }
    }
}

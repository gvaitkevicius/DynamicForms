using DynamicForms.Models;
using DynamicForms.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class ViewControleQABobinas
    {
        public ViewControleQABobinas()
        {

        }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PEDIDO")] [Required(ErrorMessage = "Campo ORD_ID requirido.")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_ENTREGA_DE")] [Required(ErrorMessage = "Campo ORD_DATA_ENTREGA_DE requirido.")] public DateTime ORD_DATA_ENTREGA_DE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_ENTREGA_ATE")] [Required(ErrorMessage = "Campo ORD_DATA_ENTREGA_ATE requirido.")] public DateTime ORD_DATA_ENTREGA_ATE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRODUTO")] [Required(ErrorMessage = "Campo PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO")] [Required(ErrorMessage = "Campo PRO_DESCRICAO requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PRO_DESCRICAO")] public string PRO_DESCRICAO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "CLIENTE")] [Required(ErrorMessage = "Campo CLI_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo CLI_ID")] public string CLI_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NOME CLIENTE")] [Required(ErrorMessage = "Campo CLI_NOME requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo CLI_NOME")] public string CLI_NOME { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LAUDO")] public int? LTF_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ULTIMA_ALTERACAO LAUDO")] public DateTime LTF_DATA_ULTIMA_ALTERACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "STATUS DO LAUDO")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo STATUS_LAUDO")] public string STATUS_LAUDO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "STATUS DE TESTAGEM")] [Required(ErrorMessage = "Campo STATUS_TESTAGEM requirido.")] [MaxLength(15, ErrorMessage = "Maximode 15 caracteres, campo STATUS_TESTAGEM")] public string STATUS_TESTAGEM { get; set; }
        [NotMapped] public T_Usuario UsuarioLogado { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) { return true; }
        public bool RealizarTestesQA(List<object> objects, ref List<LogPlay> Logs)
        {
            string dados;
            foreach (var item in objects)
            {
                ViewControleQABobinas _ControleQABobinas = (ViewControleQABobinas)item;
                dados = $"{_ControleQABobinas.ORD_ID },{_ControleQABobinas.PRO_ID}";
                Logs.Add(new LogPlay(this.ToString(), "PROTOCOLO", "LINK", "/PlugAndPlay/Qualidade/TesteQABobinas?dados=", $"{dados}"));
            }
            return true;
        }
    }
}

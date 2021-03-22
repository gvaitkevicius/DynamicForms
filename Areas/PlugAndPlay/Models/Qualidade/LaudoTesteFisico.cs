using DynamicForms.Context;
using DynamicForms.Models;
using DynamicForms.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class LaudoTesteFisico
    {
        public LaudoTesteFisico()
        {

        }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CODIGO")] [Required(ErrorMessage = "Campo LTF_ID requirido.")] public int LTF_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA EMISSAO")] public DateTime LTF_EMISSAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VALOR")] public double? LTF_VALOR { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OBS")] [MaxLength(120, ErrorMessage = "Maximode 120 caracteres, campo LTF_OBS")] public string LTF_OBS { get; set; }
        [
            Combobox(Value = "APROVADO_USUARIO", Description = "Aprovado Manualmente"),
            Combobox(Value = "REPROVADO_USUARIO", Description = "Reprovado Manualmente"),
            Combobox(Value = "APROVADO_SISTEMA", Description = "Aprovado automaticamente"),
            Combobox(Value = "REPROVADO_SISTEMA", Description = "Reprovado automaticamente")
        ]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "STATUS")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo LTF_STATUS")] public string LTF_STATUS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PEDIDO")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRODUTO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo ROT_PRO_ID")] public string ROT_PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ_REPETICAO")] public int? FPR_SEQ_REPETICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "USE_ID")] public int? USE_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA ULTIMA ALTERACAO")] public DateTime LTF_DATA_ULTIMA_ALTERACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA EMISSAO TESTES")] public DateTime LTF_DATA_EMISSAO_TESTES { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_EMISSAO INSPEÇOES VISUAIS")] public DateTime LTF_DATA_EMISSAO_INSPECOES { get; set; }

        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public T_Usuario UsuarioLogado { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        public virtual T_Usuario Usuario { get; set; }

        //Metodos de Classe
        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) { return true; }
        public bool EmitirLAudo(List<object> objects, ref List<LogPlay> Logs)
        {
            foreach (var item in objects)
            {
                LaudoTesteFisico _LaudoTesteFisico = (LaudoTesteFisico)item;
                //Validações do modelo
                if (String.IsNullOrEmpty(_LaudoTesteFisico.ORD_ID))
                {
                    _LaudoTesteFisico.PlayMsgErroValidacao = " Pedido deve ser informados corretamente, verifique os dados.";
                    return false;
                }
                if (String.IsNullOrEmpty(_LaudoTesteFisico.ROT_PRO_ID))
                {
                    _LaudoTesteFisico.PlayMsgErroValidacao = "Produto deve ser informado corretamente, verifique os dados.";
                    return false;
                }
                if (_LaudoTesteFisico.FPR_SEQ_REPETICAO == null)
                {
                    _LaudoTesteFisico.PlayMsgErroValidacao = "Sequencia de transformação deve se informada corretamente, verifique os dados.";
                    return false;
                }
                if (_LaudoTesteFisico.LTF_EMISSAO == null)
                {
                    _LaudoTesteFisico.PlayMsgErroValidacao = "Informe a data e hora  de emissão dos testes fisicos laudo, verifique os dados.";
                    return false;
                }
                Logs.Add(new LogPlay(this.ToString(), "PROTOCOLO", "LINK", "/PlugAndPlay/ReportLaudoTesteFisico/LaudoUm?LTFId=", $"{_LaudoTesteFisico.LTF_ID}"));

            }
            return true;
        }
        [HIDDEN]
        public LaudoTesteFisico GerarLaudoTesteFisico(string ORD_ID, string ROT_PRO_ID, string FPR_SEQ_REPETICAO, int USE_ID, DateTime Data, string ORIGEM)
        {
            StringBuilder msg = new StringBuilder();
            bool flag = true;
            if (String.IsNullOrEmpty(ORD_ID))
            {
                msg.AppendLine(" Pedido deve ser informados corretamente, verifique os dados.");
                flag = false;
            }
            if (String.IsNullOrEmpty(ROT_PRO_ID))
            {
                msg.AppendLine("Produto deve ser informado corretamente, verifique os dados.");
                flag = false;
            }
            if (FPR_SEQ_REPETICAO == null)
            {
                msg.AppendLine("Sequencia de transformação deve se informada corretamente, verifique os dados.");
                flag = false;
            }
            if (Data == null)
            {
                msg.AppendLine("Informe a data e hora  de emissão dos testes fisicos laudo, verifique os dados.");
                flag = false;
            }
            if (flag)
            {
                using (JSgi db = new ContextFactory().CreateDbContext(Array.Empty<string>()))
                {
                    string Origem = (ORIGEM.Equals("S")) ? "APROVADO_USUARIO" : "APROVADO_SISTEMA";
                    DateTime DataAutual = DateTime.Now;
                    var Db_TestesFisicos = db.TesteFisico.Include(x => x.ResultLote).AsNoTracking().Where(x => x.ORD_ID.Equals(ORD_ID) &&
                                                         x.FPR_SEQ_REPETICAO == Convert.ToInt32(FPR_SEQ_REPETICAO) &&
                                                         x.ROT_PRO_ID.Equals(ROT_PRO_ID) &&
                                                         x.TES_EMISSAO.CompareTo(Data) == 0)
                                                       .FirstOrDefault();
                    LaudoTesteFisico _LaudoTesteFisico = new LaudoTesteFisico()
                    {
                        LTF_ID = 0,
                        LTF_EMISSAO = DataAutual,
                        LTF_OBS = "",
                        LTF_STATUS = Origem,
                        LTF_VALOR = Db_TestesFisicos.ResultLote.RL_VALOR_ENCONTRADO,
                        LTF_DATA_ULTIMA_ALTERACAO = DataAutual,
                        ORD_ID = ORD_ID,
                        ROT_PRO_ID = ROT_PRO_ID,
                        FPR_SEQ_REPETICAO = Convert.ToInt16(FPR_SEQ_REPETICAO),
                        USE_ID = USE_ID,
                        LTF_DATA_EMISSAO_TESTES = Db_TestesFisicos.TES_EMISSAO,
                        LTF_DATA_EMISSAO_INSPECOES = Db_TestesFisicos.TES_EMISSAO,
                        PlayAction = "insert"
                    };
                    return _LaudoTesteFisico;
                }
            }
            return null;
        }
        [HIDDEN]
        public bool EncerrarLaudo(string ORD_ID, string ROT_PRO_ID, string FPR_SEQ_REPETICAO)
        {
            StringBuilder msg = new StringBuilder();
            bool flag = true;

            return flag;

        }
    }
}

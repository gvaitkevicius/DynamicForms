using DynamicForms.Context;
using DynamicForms.Models;
using DynamicForms.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "MOVIMENTOS DE ESTOQUE")]
    public class MovimentoEstoque
    {
        public MovimentoEstoque()
        {

        }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD MOVIMENTO")] [Required(ErrorMessage = "Campo MOV_ID requirido.")] public int MOV_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO MOVIMENTAÇÃO")] [Required(ErrorMessage = "Campo TIP_ID requirido.")] [MaxLength(3, ErrorMessage = "Maximode 3 caracteres, campo TIP_ID")] public string TIP_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PRODUTO")] [Required(ErrorMessage = "Campo PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE")] [Required(ErrorMessage = "Campo MOV_QUANTIDADE requirido.")] public double MOV_QUANTIDADE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NUM DOCUMENTO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MOV_DOC")] public string MOV_DOC { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LOTE")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MOV_LOTE")] public string MOV_LOTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SUB LOTE")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_SUB_LOTE")] public string MOV_SUB_LOTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PEDIDO")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ TRANFORMAÇÃO")] public int? FPR_SEQ_TRANFORMACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ REPETIÇÃO")] public int? FPR_SEQ_REPETICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD OCORRÊNCIA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo OCO_ID")] public string OCO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OBSERVAÇÕES")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo MOV_OBS")] public string MOV_OBS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ARMAZÉM")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_ARMAZEM")] public string MOV_ARMAZEM { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ENDEREÇO ESTOQUE")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_ENDERECO")] public string MOV_ENDERECO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OBSERVAÇÃO OP PARCIAL")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo MOV_OBS_OP_PARCIAL")] public string MOV_OBS_OP_PARCIAL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD OCORRÊNCIA OP PARCIAL")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_OCO_ID_OP_PARCIAL")] public string MOV_OCO_ID_OP_PARCIAL { get; set; }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA MOVIMENTO")] [Required(ErrorMessage = "Campo MOV_DATA_HORA_EMISSAO requirido.")] public DateTime MOV_DATA_HORA_EMISSAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA CRIAÇÃO")] public DateTime MOV_DATA_HORA_CRIACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD MÁQUINA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MAQ_ID")] public string MAQ_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TURNO")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo TURN_ID")] public string TURN_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TURMA")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo TURM_ID")] public string TURM_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DIA TURMA")] [MaxLength(8, ErrorMessage = "Maximode 8 caracteres, campo MOV_DIA_TURMA")] public string MOV_DIA_TURMA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ESTORNO")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo MOV_ESTORNO")] public string MOV_ESTORNO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "USUÁRIO")] public int? USE_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD INTEGRAÇÃO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MOV_ID_INTEGRACAO")] public string MOV_ID_INTEGRACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD INTEGRAÇÃO ERP")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MOV_ID_INTEGRACAO_ERP")] public string MOV_ID_INTEGRACAO_ERP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO REGISTRO")] public int? MOV_TYPE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD CARGA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo CAR_ID")] public string CAR_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD MOV DESTINO")] public int? MOV_ID_DESTINO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PRODUTO DESTINO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID_DESTINO")] public string PRO_ID_DESTINO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LOTE MOV DESTINO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MOV_LOTE_DESTINO")] public string MOV_LOTE_DESTINO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SUB LOTE MOV DESTINO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_SUB_LOTE_DESTINO")] public string MOV_SUB_LOTE_DESTINO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD MOV ORIGEM")] public int? MOV_ID_ORIGEM { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PRODUTO ORIGEM")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID_ORIGEM")] public string PRO_ID_ORIGEM { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LOTE MOV ORIGEM")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MOV_LOTE_ORIGEM")] public string MOV_LOTE_ORIGEM { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SUB LOTE MOV ORIGEM")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_SUB_LOTE_ORIGEM")] public string MOV_SUB_LOTE_ORIGEM { get; set; }

        public virtual Maquina Maquina { get; set; }
        public virtual Produto Produto { get; set; }
        public virtual OcorrenciaProducaoParciais OcorrenciaOpParcial { get; set; }
        public virtual Turno Turno { get; set; }
        public virtual Turma Turma { get; set; }
        [NotMapped] public T_Usuario UsuarioLogado { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        public virtual ICollection<T_FeedbackMovEstoque> T_FeedbackMovEstoque { get; set; } /* Relacionamento many-to-many Entity Framework Core 2*/
        public virtual ICollection<TargetProduto> TargetsProduto { get; set; }

        //--MÉTODOS DE CLASSE
        public bool ImprimirEtiqueta(List<object> objects, ref List<LogPlay> Logs)
        {
            foreach (var item in objects)
            {
                MovimentoEstoque movimentoEstoque = (MovimentoEstoque)item;
                movimentoEstoque.PlayAction = "OK";
                using (var db = new ContextFactory().CreateDbContext(new string[] { }))
                {
                    var etiquetaexistente = db.Etiqueta.AsNoTracking().Where(x => x.ETI_LOTE.Equals(movimentoEstoque.MOV_LOTE) && x.ETI_SUB_LOTE.Equals(movimentoEstoque.MOV_SUB_LOTE)).OrderByDescending(x=>x.ETI_EMISSAO).FirstOrDefault();
                    if (etiquetaexistente != null)
                    {
                        InterfaceTelaImpressaoEtiquetas et = new InterfaceTelaImpressaoEtiquetas();
                        Logs.Add(et.ImprimirEt($"{etiquetaexistente.ETI_ID}"));
                        etiquetaexistente.PlayAction = "OK";
                    }
                    else
                    {
                        Logs.Add(new LogPlay() { Status = "ERRO", MsgErro = "NÃO EXISTE UMA ETIQUETA GERADA PARA ESTE MOVIMENTO DE ESTOQUE." });
                        return false;
                    }
                }
            }
            return true;
        }
        public bool GerarEtiquetaProduto(List<object> objects, ref List<LogPlay> Logs)
        {
            List<Etiqueta> etiquetas = new List<Etiqueta>();
            foreach (var item in objects)
            {
                MovimentoEstoque mov = (MovimentoEstoque)item;
                mov.PlayAction = "OK";
                using (var db = new ContextFactory().CreateDbContext(new string[] { }))
                {
                    // gerando etiqueta do lote
                    var etiqueta = new Etiqueta()
                        .GerarEtiquetaLoteProduto(mov.PRO_ID, mov.MOV_LOTE, mov.MOV_SUB_LOTE, mov.FPR_SEQ_REPETICAO.Value, Logs, mov.UsuarioLogado.USE_ID);

                    etiquetas.Add(etiqueta);
                }
            }

            if (!Logs.Any(x => x.Status.Equals("ERRO")))
            {
                var Ids = String.Join(",", etiquetas.Select(x => x.ETI_ID).ToArray());
                Logs.Add(new LogPlay(this.ToString(), "PROTOCOLO", "LINK", "/PlugAndPlay/ReportEtiqueta/EtiquetaIndividualLoteProduto?etiqueta=", $"{Ids}"));
            }

            return true;
        }
    }
}
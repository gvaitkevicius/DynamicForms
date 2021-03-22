using DynamicForms.Context;
using DynamicForms.Controllers;
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
    [Display(Name = "CONSUMO MATÉRIA PRIMA")]
    public class MovimentoEstoqueConsumoMateriaPrima : MovimentoEstoqueAbstrata
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD. PRODUTO")] [Required(ErrorMessage = "Campo PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE")] [Required(ErrorMessage = "Campo MOV_QUANTIDADE requirido.")] public double MOV_QUANTIDADE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LOTE")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MOV_LOTE")] public string MOV_LOTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SUB_LOTE")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_SUB_LOTE")] public string MOV_SUB_LOTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD. PEDIDO")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ_TRANFORMACAO")] public int? FPR_SEQ_TRANFORMACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ_REPETICAO")] public int? FPR_SEQ_REPETICAO { get; set; }

        [TAB(Value = "OUTROS")] [Display(Name = "COD. OCORRÊNCIA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo OCO_ID")] public string OCO_ID { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "OBSERVAÇÕES")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo MOV_OBS")] public string MOV_OBS { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "ARMAZÉM")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_ARMAZEM")] public string MOV_ARMAZEM { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "ENDEREÇO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_ENDERECO")] public string MOV_ENDERECO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "OBSERVAÇÕES OP PARCIAL")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo MOV_OBS_OP_PARCIAL")] public string MOV_OBS_OP_PARCIAL { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "COD OCORRÊNCIA OP PARCIAL")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_OCO_ID_OP_PARCIAL")] public string MOV_OCO_ID_OP_PARCIAL { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "USUÁRIO")] public int? USE_ID { get; set; }
        [NotMapped] public T_Usuario UsuarioLogado { get; set; }
        public virtual T_Usuario Usuario { get; set; }
        public virtual Produto Produto { get; set; }
        public virtual Turno Turno { get; set; }
        public virtual Turma Turma { get; set; }
        public virtual Carga Carga { get; set; }
        public virtual Order Order { get; set; }
        public virtual TipoMovSaidaConsumo TipoMovSaidaConsumo { get; set; }
        public virtual OcorrenciaConsumoMateriaPrima OcorrenciaConsumoMateriaPrima { get; set; }
        public virtual OcorrenciaProducaoParciais OcorrenciaProducaoParciais { get; set; }
        ///MÉTODOS DE CLASSE
        ///
        public override bool ImprimirEtiqueta(List<object> objects, ref List<LogPlay> Logs)
        {
            foreach (var item in objects)
            {
                MovimentoEstoqueConsumoMateriaPrima movConsumo = (MovimentoEstoqueConsumoMateriaPrima)item;
                movConsumo.PlayAction = "OK";
                using (var db = new ContextFactory().CreateDbContext(new string[] { }))
                {
                    var etiquetaexistente = db.Etiqueta.AsNoTracking().Where(x => x.ETI_LOTE.Equals(movConsumo.MOV_LOTE) && x.ETI_SUB_LOTE.Equals(movConsumo.MOV_SUB_LOTE)).OrderByDescending(x => x.ETI_EMISSAO).FirstOrDefault();
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
        //--
        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            List<object> ConsumoMateriaPrima = new List<object>();
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (var item in objects)
                {
                    if (item.GetType().Name != nameof(MovimentoEstoqueConsumoMateriaPrima))
                        continue;

                    MovimentoEstoqueConsumoMateriaPrima consumoMp = (MovimentoEstoqueConsumoMateriaPrima)item;
                    #region Validacoes info. do formulario
                    if (String.IsNullOrEmpty(consumoMp.MOV_LOTE?.Trim()))
                    {
                        consumoMp.PlayMsgErroValidacao = "Lote  nao pode ser vazio.";
                        consumoMp.PlayAction = "ERRO";
                        return false;
                    }
                    if (String.IsNullOrEmpty(consumoMp.MOV_SUB_LOTE?.Trim()))
                    {
                        consumoMp.PlayMsgErroValidacao = "Sub Lote nao pode ser vazio.";
                        consumoMp.PlayAction = "ERRO";
                        return false;
                    }
                    if (consumoMp.MOV_QUANTIDADE <= 0)
                    {
                        consumoMp.PlayMsgErroValidacao = "A quantidade informada para saida deve ser maior que 0";
                        consumoMp.PlayAction = "ERRO";
                        return false;
                    }
                    #endregion
                    #region Consultando Saldo do Lote Informado
                    var saldoMovimentoOriginal = db.SaldosEmEstoquePorLote.AsNoTracking()
                        .Where(x => x.MOV_LOTE == consumoMp.MOV_LOTE && x.MOV_SUB_LOTE == consumoMp.MOV_SUB_LOTE)
                        .FirstOrDefault();
                    if (saldoMovimentoOriginal == null)
                    {
                        consumoMp.PlayMsgErroValidacao = $"O Lote informado [{ consumoMp.MOV_LOTE }-{ consumoMp.MOV_SUB_LOTE }] não possui saldo.";
                        consumoMp.PlayAction = "ERRO";
                        return false;
                    }
                    #endregion
                    //-----
                    var reserva = new MovimentoEstoqueReservaDeEstoque();
                    reserva = reserva.GetReserva(consumoMp.MOV_LOTE, consumoMp.MOV_SUB_LOTE, consumoMp.PRO_ID);
                    
                    if (reserva.PlayAction == "insert")
                    {
                        consumoMp.PlayMsgErroValidacao = "Não há reserva para o lote.";
                        consumoMp.PlayAction = "ERRO";
                        return false;
                    }

                    consumoMp.TIP_ID = "610";
                    consumoMp.MAQ_ID = reserva.MAQ_ID;
                    consumoMp.ORD_ID = (String.IsNullOrEmpty(consumoMp.ORD_ID?.Trim())) ? consumoMp.ORD_ID : reserva.ORD_ID;
                    consumoMp.MOV_OBS = reserva.MOV_OBS;
                    consumoMp.MOV_ARMAZEM = reserva.MOV_ARMAZEM;
                    consumoMp.MOV_ENDERECO = reserva.MOV_ENDERECO;
                    consumoMp.MOV_OBS_OP_PARCIAL = reserva.MOV_OBS_OP_PARCIAL;
                    consumoMp.MOV_OCO_ID_OP_PARCIAL = reserva.MOV_OCO_ID_OP_PARCIAL;
                    consumoMp.PlayAction = "insert";
                }
            }
            return true;
        }
        public bool AfterChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert, JSgi db = null)
        {
            List<Etiqueta> etiquetas = new List<Etiqueta>();

            foreach (var item in objects)
            {
                if (item.GetType().Name != nameof(MovimentoEstoqueConsumoMateriaPrima))
                    continue;

                MovimentoEstoqueConsumoMateriaPrima mov = (MovimentoEstoqueConsumoMateriaPrima)item;
                if (mov.PlayAction.Equals("insert", StringComparison.OrdinalIgnoreCase))
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

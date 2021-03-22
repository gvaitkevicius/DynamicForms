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
    [Display(Name = "CONSULTAR PERDAS")]
    public class MovimentoEstoquePerdas : MovimentoEstoqueAbstrata
    {
        
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PRODUTO")] [Required(ErrorMessage = "Campo PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE")] [Required(ErrorMessage = "Campo MOV_QUANTIDADE requirido.")] public double MOV_QUANTIDADE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LOTE")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MOV_LOTE")] public string MOV_LOTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SUB LOTE")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_SUB_LOTE")] public string MOV_SUB_LOTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PEDIDO")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ TRANFORMACAO")] public int? FPR_SEQ_TRANFORMACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ REPETICAO")] public int? FPR_SEQ_REPETICAO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "COD CORRÊNCIA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo OCO_ID")] public string OCO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OBSERVAÇÕES")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo MOV_OBS")] public string MOV_OBS { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "ARMAZÉM")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_ARMAZEM")] public string MOV_ARMAZEM { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "ENDEREÇO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_ENDERECO")] public string MOV_ENDERECO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "OBSERVAÇÕES OP PARCIAL")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo MOV_OBS_OP_PARCIAL")] public string MOV_OBS_OP_PARCIAL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD OCORRÊNCIA OP PARCIAL")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_OCO_ID_OP_PARCIAL")] public string MOV_OCO_ID_OP_PARCIAL { get; set; }

        [TAB(Value = "OUTROS")] [Display(Name = "USUÁRIO")] public int? USE_ID { get; set; }

        [NotMapped] public T_Usuario UsuarioLogado { get; set; }
        public virtual Produto Produto { get; set; }
        public virtual Turno Turno { get; set; }
        public virtual Turma Turma { get; set; }
        public virtual Order Order { get; set; }
        public virtual Carga Carga { get; set; }
        public virtual TipoMovSaidaPerdas TipoMovSaidaPerdas { get; set; }
        public virtual OcorrenciaMotivosDasPerdas OcorrenciaMotivosDasPerdas { get; set; }
        public virtual OcorrenciaProducaoParciais OcorrenciaProducaoParciais { get; set; }
        public T_Usuario Usuario { get; set; }
        public override bool ImprimirEtiqueta(List<object> objects, ref List<LogPlay> Logs)
        {
            foreach (var item in objects)
            {
                MovimentoEstoquePerdas movPerdas = (MovimentoEstoquePerdas)item;
                movPerdas.PlayAction = "OK";
                using (var db = new ContextFactory().CreateDbContext(new string[] { }))
                {
                    var etiquetaexistente = db.Etiqueta.AsNoTracking().Where(x => x.ETI_LOTE.Equals(movPerdas.MOV_LOTE) && x.ETI_SUB_LOTE.Equals(movPerdas.MOV_SUB_LOTE)).OrderByDescending(x => x.ETI_EMISSAO).FirstOrDefault();
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
        ///MÉTODOS DE CLASSE
        ///
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) 
        //{
        //    foreach (var item in objects)
        //    {
        //        if (item.GetType().Name == "MovimentoEstoquePerdas") 
        //        {
        //            MovimentoEstoquePerdas movSaida = (MovimentoEstoquePerdas)item;

        //        }
        //    }
        //    return true;
        //}
        public bool AfterChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert, JSgi db = null)
        {
            List<Etiqueta> etiquetas = new List<Etiqueta>();

            foreach (var item in objects)
            {
                if (item.GetType().Name == "MovimentoEstoquePerdas")
                {
                    MovimentoEstoquePerdas mov = (MovimentoEstoquePerdas)item;
                    
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

        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        //{
        //    List<object> PerdaEstoque = new List<object>();
        //    using (var db = new ContextFactory().CreateDbContext(new string[] { }))
        //    {
        //        foreach (var item in objects)
        //        {
        //            //Objeto retornado da View
        //            PedasProducao _SaidaPerdas = (PedasProducao)item;
        //            //Setando action do objeto da view como OK para que seja ignorado pela  funçao UpdateData
        //            _SaidaPerdas.PlayAction = "OK";
        //            //Validaçoes --
        //            if (String.IsNullOrEmpty(_SaidaPerdas.MOV_LOTE))
        //            {
        //                _SaidaPerdas.PlayMsgErroValidacao = "Lote  nao pode ser vazio.";
        //                return false;
        //            }
        //            if (String.IsNullOrEmpty(_SaidaPerdas.TIP_ID))
        //            {
        //                _SaidaPerdas.PlayMsgErroValidacao = "Voce deve informar o código do tipo da perda.";
        //                return false;
        //            }
        //            else
        //            {
        //                int aux = Convert.ToInt32(_SaidaPerdas.TIP_ID);
        //                if (aux < 500 || aux > 561)
        //                {
        //                    _SaidaPerdas.PlayMsgErroValidacao = "Voce deve informar um código válido para o tipo da perda.";
        //                    return false;
        //                }
        //            }
        //            if (String.IsNullOrEmpty(_SaidaPerdas.MOV_SUB_LOTE))
        //            {
        //                _SaidaPerdas.PlayMsgErroValidacao = "Sub Lote nao pode ser vazio.";
        //                return false;
        //            }
        //            if (_SaidaPerdas.MOV_QUANTIDADE <= 0)
        //            {
        //                _SaidaPerdas.PlayMsgErroValidacao = "A quantidade informada para saida deve ser maior que 0";
        //                return false;
        //            }
        //            SaldosEmEstoquePorLote Db_saldoMovOriginal = db.SaldosEmEstoquePorLote.Where(x => x.MOV_LOTE == _SaidaPerdas.MOV_LOTE && x.MOV_SUB_LOTE == _SaidaPerdas.MOV_SUB_LOTE).FirstOrDefault();
        //            if (Db_saldoMovOriginal == null)
        //            {
        //                _SaidaPerdas.PlayMsgErroValidacao = "O Lote informado [" + _SaidaPerdas.MOV_LOTE + "" + _SaidaPerdas.MOV_SUB_LOTE + "] não possui saldo.";
        //                return false;
        //            }
        //            //-----
        //            //CONSULTA RESERVAS
        //            MovimentoEstoqueReservaDeEstoque Db_ResLoteOriginal = db.MovimentoEstoqueReservaDeEstoque.Where(x => x.MOV_LOTE == _SaidaPerdas.MOV_LOTE && x.MOV_SUB_LOTE == _SaidaPerdas.MOV_SUB_LOTE).FirstOrDefault();
        //            if (Db_ResLoteOriginal != null)
        //            {
        //                //Criando o movimento de entrada
        //                PerdaEstoque.Add(new MovimentoEstoquePerdas
        //                {
        //                    TIP_ID = _SaidaPerdas.TIP_ID,
        //                    MAQ_ID = Db_ResLoteOriginal.MAQ_ID,
        //                    TURM_ID = null,
        //                    TURN_ID = null,
        //                    PRO_ID = Db_ResLoteOriginal.PRO_ID,
        //                    MOV_QUANTIDADE = _SaidaPerdas.MOV_QUANTIDADE,
        //                    MOV_LOTE = Db_ResLoteOriginal.MOV_LOTE,
        //                    MOV_SUB_LOTE = Db_ResLoteOriginal.MOV_SUB_LOTE,
        //                    ORD_ID = Db_ResLoteOriginal.ORD_ID,
        //                    FPR_SEQ_TRANFORMACAO = Db_ResLoteOriginal.FPR_SEQ_TRANFORMACAO,
        //                    FPR_SEQ_REPETICAO = Db_ResLoteOriginal.FPR_SEQ_REPETICAO,
        //                    OCO_ID = null,
        //                    MOV_OBS = Db_ResLoteOriginal.MOV_OBS,
        //                    MOV_ARMAZEM = Db_ResLoteOriginal.MOV_ARMAZEM,
        //                    MOV_ENDERECO = Db_ResLoteOriginal.MOV_ENDERECO,
        //                    MOV_OBS_OP_PARCIAL = Db_ResLoteOriginal.MOV_OBS_OP_PARCIAL,
        //                    MOV_OCO_ID_OP_PARCIAL = Db_ResLoteOriginal.MOV_OCO_ID_OP_PARCIAL,
        //                    PlayAction = "insert"
        //                });
        //            }
        //            else
        //            {
        //                _SaidaPerdas.PlayMsgErroValidacao = "Não há reserva para o lote.";
        //                return false;
        //            }
        //            //Adicionando movimento a lista de objetos da funcao BeforeCheanges

        //        }
        //        objects.AddRange(PerdaEstoque);
        //    }
        //    return true;
        //}
    }
    [Display(Name = "Perdas na Produção")]
    public class PedasProducao : InterfaceDeTelas
    {
        [SEARCH_NOT_FK(namespaceOfForeignClass = "DynamicForms.Areas.PlugAndPlay.Models.Produto")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PRODUTO")] [Required(ErrorMessage = "Campo PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE")] [Required(ErrorMessage = "Campo MOV_QUANTIDADE requirido.")] public double MOV_QUANTIDADE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LOTE")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MOV_LOTE")] public string MOV_LOTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SUB LOTE")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_SUB_LOTE")] public string MOV_SUB_LOTE { get; set; }

        [Combobox(Value = "500", Description = "PERDAS NA PRODUÇÃO")]
        [Combobox(Value = "551", Description = "PERDAS NA MOV. INTERNA")]
        [Combobox(Value = "561", Description = "PERDAS NA MOV. EXTERNA")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO PERDA")] [MaxLength(4, ErrorMessage = "TIPO PERDA")] public string TIP_ID { get; set; }
        [NotMapped] public T_Usuario UsuarioLogado { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            List<object> PerdaEstoque = new List<object>();
            bool check = true;
            SaldosEmEstoquePorLote saldoOriginalLote = null;
            MovimentoEstoqueReservaDeEstoque reservaOriginalLote = null;
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (var item in objects)
                {
                    //Objeto retornado da View
                    PedasProducao movSaida = (PedasProducao)item;
                    //Setando action do objeto da view como OK para que seja ignorado pela  funçao UpdateData
                    movSaida.PlayAction = "OK";
                    #region Validações Entradas informações do formulario
                    if (String.IsNullOrEmpty(movSaida.MOV_LOTE))
                    {
                        movSaida.PlayMsgErroValidacao = "Lote  nao pode ser vazio.";
                        check = false;
                    }
                    if (String.IsNullOrEmpty(movSaida.TIP_ID))
                    {
                        movSaida.PlayMsgErroValidacao = "Voce deve informar o código do tipo da perda.";
                        check = false;
                    }
                    else
                    {
                        int aux = Convert.ToInt32(movSaida.TIP_ID);
                        if (aux < 500 || aux > 561)
                        {
                            movSaida.PlayMsgErroValidacao = "Voce deve informar um código válido para o tipo da perda.";
                            check = false;
                        }
                    }
                    if (String.IsNullOrEmpty(movSaida.MOV_SUB_LOTE))
                    {
                        movSaida.PlayMsgErroValidacao = "Sub Lote nao pode ser vazio.";
                        check = false;
                    }
                    if (movSaida.MOV_QUANTIDADE <= 0)
                    {
                        movSaida.PlayMsgErroValidacao = "A quantidade informada para saida deve ser maior que 0";
                        check = false;
                    }
                    #endregion
                    #region Verificando saldo e realizando movimtaçao
                    if (check)
                        saldoOriginalLote = db.SaldosEmEstoquePorLote.Where(x => x.MOV_LOTE == movSaida.MOV_LOTE && x.MOV_SUB_LOTE == movSaida.MOV_SUB_LOTE).FirstOrDefault();
                    if (saldoOriginalLote == null || saldoOriginalLote.SALDO<movSaida.MOV_QUANTIDADE)
                    {
                        movSaida.PlayMsgErroValidacao = $"O Lote informado [{ movSaida.MOV_LOTE } {movSaida.MOV_SUB_LOTE}] não possui saldo suficiente.";
                        check = false;
                    }
                    
                    if (check)
                    {
                        //CONSULTA RESERVAS
                        reservaOriginalLote = db.MovimentoEstoqueReservaDeEstoque.AsNoTracking().
                            Where(x => x.MOV_LOTE == movSaida.MOV_LOTE && x.MOV_SUB_LOTE == movSaida.MOV_SUB_LOTE && String.IsNullOrEmpty(x.MOV_ESTORNO)).FirstOrDefault();

                        if (reservaOriginalLote != null)
                        {
                            //Criando o movimento 
                            PerdaEstoque.Add(new MovimentoEstoquePerdas
                            {
                                TIP_ID = movSaida.TIP_ID,
                                MAQ_ID = reservaOriginalLote.MAQ_ID,
                                TURM_ID = null,
                                TURN_ID = null,
                                PRO_ID = reservaOriginalLote.PRO_ID,
                                MOV_QUANTIDADE = movSaida.MOV_QUANTIDADE,
                                MOV_LOTE = reservaOriginalLote.MOV_LOTE,
                                MOV_SUB_LOTE = reservaOriginalLote.MOV_SUB_LOTE,
                                ORD_ID = reservaOriginalLote.ORD_ID,
                                FPR_SEQ_TRANFORMACAO = reservaOriginalLote.FPR_SEQ_TRANFORMACAO,
                                FPR_SEQ_REPETICAO = reservaOriginalLote.FPR_SEQ_REPETICAO,
                                OCO_ID = null,
                                MOV_OBS = reservaOriginalLote.MOV_OBS,
                                MOV_ARMAZEM = reservaOriginalLote.MOV_ARMAZEM,
                                MOV_ENDERECO = reservaOriginalLote.MOV_ENDERECO,
                                MOV_OBS_OP_PARCIAL = reservaOriginalLote.MOV_OBS_OP_PARCIAL,
                                MOV_OCO_ID_OP_PARCIAL = reservaOriginalLote.MOV_OCO_ID_OP_PARCIAL,
                                PlayAction = "insert"
                            });
                        }
                        else
                        {
                            movSaida.PlayMsgErroValidacao = $"Não há reserva para o lote[{movSaida.MOV_LOTE}] sub lote [{movSaida.MOV_SUB_LOTE}] ou ela foi estornada.";
                            check = false;
                        }
                    }
                    #endregion
                   
                }
                objects.AddRange(PerdaEstoque);
            }
            return check;
        }
        
    }

}

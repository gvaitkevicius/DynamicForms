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
    public class MovimentoEstoqueTransferenciaSimples : MovimentoEstoqueAbstrata
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PRODUTO")] [Required(ErrorMessage = "Campo PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE")] [Required(ErrorMessage = "Campo MOV_QUANTIDADE requirido.")] public double MOV_QUANTIDADE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NÚM DOCUMENTO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MOV_DOC")] public string MOV_DOC { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LOTE")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MOV_LOTE")] public string MOV_LOTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SUB LOTE")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_SUB_LOTE")] public string MOV_SUB_LOTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PEDIDO")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ TRANFORMAÇÃO")] public int? FPR_SEQ_TRANFORMACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ REPETIÇÃO")] public int? FPR_SEQ_REPETICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD OCORRÊNCIA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo OCO_ID")] public string OCO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OBSERVAÇÕES")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo MOV_OBS")] public string MOV_OBS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ARMAZÉMM")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_ARMAZEM")] public string MOV_ARMAZEM { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ENDEREÇO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_ENDERECO")] public string MOV_ENDERECO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OBSERVAÇÕES OP PARCIAL")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo MOV_OBS_OP_PARCIAL")] public string MOV_OBS_OP_PARCIAL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD OCORRÊNCIA OP PARCIAL")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_OCO_ID_OP_PARCIAL")] public string MOV_OCO_ID_OP_PARCIAL { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "USUÁRIO")] public int? USE_ID { get; set; }
        [NotMapped] public T_Usuario UsuarioLogado { get; set; }
        public virtual Turno Turno { get; set; }
        public virtual Turma Turma { get; set; }
        public virtual Carga Carga { get; set; }
        public virtual T_Usuario Usuario { get; set; }
        public virtual OcorrenciaProducao OcorrenciaProducao { get; set; }
        public virtual TipoMovTransferenciaSimples TipoMovTransferenciaInterna { get; set; }
        public virtual OcorrenciaProducaoParciais OcorrenciaProducaoParciais { get; set; }
        public virtual Order Order { get; set; }
        public MovimentoEstoqueTransferenciaSimples()
        {

        }

        public override bool ImprimirEtiqueta(List<object> objects, ref List<LogPlay> Logs)
        {
            foreach (var item in objects)
            {
                MovimentoEstoqueTransferenciaSimples movimentoEstoque = (MovimentoEstoqueTransferenciaSimples)item;
                movimentoEstoque.PlayAction = "OK";
                using (var db = new ContextFactory().CreateDbContext(new string[] { }))
                {
                    var etiquetaexistente = db.Etiqueta.AsNoTracking().Where(x => x.ETI_LOTE.Equals(movimentoEstoque.MOV_LOTE) && x.ETI_SUB_LOTE.Equals(movimentoEstoque.MOV_SUB_LOTE)).OrderByDescending(x => x.ETI_EMISSAO).FirstOrDefault();
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
        
        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            List<object> Transferencia = new List<object>();
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (var item in objects)
                {
                    if (item.GetType().Name != nameof(MovimentoEstoqueTransferenciaSimples))
                        continue;

                    // DESMONTAGEM DE LOTES - PARTE 02

                    MovimentoEstoqueTransferenciaSimples mov = (MovimentoEstoqueTransferenciaSimples)item;
                    switch (mov.PlayAction) 
                    {
                        case "insert":
                            mov.PlayAction = "OK"; 
                            
                            #region Validacoes inf. do formulario
                            if (String.IsNullOrEmpty(mov.MOV_LOTE_ORIGEM?.Trim()))
                            {
                                mov.PlayMsgErroValidacao = "Lote origem nao pode ser vazio.";
                                return false;
                            }
                            if (String.IsNullOrEmpty(mov.MOV_SUB_LOTE_ORIGEM?.Trim()))
                            {
                                mov.PlayMsgErroValidacao = "Sub Lote origem nao pode ser vazio.";
                                return false;
                            }
                            if (String.IsNullOrEmpty(mov.MOV_LOTE_DESTINO?.Trim()))
                            {
                                mov.PlayMsgErroValidacao = "Sub Lote destino nao pode ser vazio.";
                                return false;
                            }
                            if (String.IsNullOrEmpty(mov.MOV_SUB_LOTE_DESTINO?.Trim()))
                            {
                                mov.PlayMsgErroValidacao = "Sub Lote destino nao pode ser vazio.";
                                return false;
                            }
                            if (mov.MOV_LOTE_ORIGEM.Equals(mov.MOV_LOTE_DESTINO, StringComparison.OrdinalIgnoreCase) && 
                                mov.MOV_SUB_LOTE_ORIGEM.Equals(mov.MOV_SUB_LOTE_DESTINO, StringComparison.OrdinalIgnoreCase))
                            {
                                mov.PlayMsgErroValidacao = "Você não pode efetuar uma transferência para lotes iguais!";
                                return false;
                            }
                            if (mov.MOV_QUANTIDADE <= 0)
                            {
                                mov.PlayMsgErroValidacao = "A quantidade precisa ser maior que zero.";
                                return false;
                            }

                            if (!String.IsNullOrEmpty(mov.ORD_ID?.Trim()))
                            {
                                bool pedidoDestinoExiste = db.Order.Any(o => o.ORD_ID.Equals(mov.ORD_ID));
                                if (!pedidoDestinoExiste)
                                {
                                    mov.PlayMsgErroValidacao = "O pedido informado não foi encontrado,verifique a informação!";
                                    return false;
                                }
                                if (String.IsNullOrEmpty(mov.MOV_ENDERECO?.Trim()))
                                {
                                    mov.PlayMsgErroValidacao = "Você deve informar um endereço de estoque para o destino!";
                                    return false;
                                }
                            }
                            #endregion Validacoes inf. do formulario

                            #region Consultando saldo em estoque
                            SaldosEmEstoquePorLote saldoLoteOrigem = db.SaldosEmEstoquePorLote.AsNoTracking()
                                .Where(x => x.MOV_LOTE.Equals(mov.MOV_LOTE_ORIGEM) && x.MOV_SUB_LOTE.Equals(mov.MOV_SUB_LOTE_ORIGEM))
                                .FirstOrDefault();

                            SaldosEmEstoquePorLote saldoLoteDestino = db.SaldosEmEstoquePorLote.AsNoTracking()
                                .Where(x => x.MOV_LOTE.Equals(mov.MOV_LOTE_DESTINO) && x.MOV_SUB_LOTE.Equals(mov.MOV_SUB_LOTE_DESTINO))
                                .FirstOrDefault();
                            #endregion Consultando saldo em estoque

                            if (saldoLoteOrigem == null)
                            {
                                mov.PlayMsgErroValidacao = "Lote origem não existe.";
                                return false;
                            }

                            if (saldoLoteOrigem.SALDO < mov.MOV_QUANTIDADE)
                            {// transferre saldo parcial 
                                mov.PlayMsgErroValidacao = "Quantidade maior que o saldo do lote.";
                                return false;
                            }

                            if (!String.IsNullOrEmpty(mov.PRO_ID_DESTINO?.Trim()) && mov.PRO_ID_DESTINO != saldoLoteOrigem.PRO_ID)
                            {// transfere para outro produto 
                                // validar produto 
                                string produtoDb = db.Produto.AsNoTracking().Where(x => x.PRO_ID == mov.PRO_ID_DESTINO)
                                    .Select(x => x.PRO_ID).FirstOrDefault();
                                if (String.IsNullOrEmpty(produtoDb?.Trim()))
                                {
                                    mov.PlayMsgErroValidacao = "Produto destino invalido.";
                                    return false;
                                }
                                mov.PRO_ID_DESTINO = mov.PRO_ID_DESTINO;
                                mov.PRO_ID = saldoLoteOrigem.PRO_ID;
                                mov.PRO_ID_ORIGEM = saldoLoteOrigem.PRO_ID;
                            }
                            else
                            {// transfere para mesmo produto
                                mov.PRO_ID = saldoLoteOrigem.PRO_ID;
                                mov.PRO_ID_ORIGEM = saldoLoteOrigem.PRO_ID;
                                mov.PRO_ID_DESTINO = saldoLoteOrigem.PRO_ID;
                            }

                            //Consultando reserva do lote de Origem
                            var reservaOrigem = db.MovimentoEstoqueReservaDeEstoque.AsNoTracking()
                                .Where(r => r.MOV_LOTE == mov.MOV_LOTE_ORIGEM && r.MOV_SUB_LOTE == mov.MOV_SUB_LOTE_ORIGEM && r.MOV_ESTORNO != "E")
                                .FirstOrDefault();
                            
                            var reservaDestino = db.MovimentoEstoqueReservaDeEstoque.AsNoTracking().
                                Where(r => r.MOV_LOTE == mov.MOV_LOTE_DESTINO && r.MOV_SUB_LOTE == mov.MOV_SUB_LOTE_DESTINO && r.MOV_ESTORNO != "E")
                                .FirstOrDefault();
                            
                            if (reservaOrigem != null)
                            {
                                //transferencia -- lote origem tiver reserva - chamar set rteserva com a quantidade restante dpo lote origem
                                /* PENDENCIA
                                 * Atualizar peso da reserva com a média ponderada do lote origem e destino
                                 */
                                reservaOrigem.USE_ID = mov.UsuarioLogado.USE_ID;
                                reservaOrigem.MOV_QUANTIDADE = saldoLoteOrigem.SALDO.Value - mov.MOV_QUANTIDADE;
                                reservaOrigem.PlayAction = "update";
                                Transferencia.Add(reservaOrigem);
                            }
                            else
                            {
                                mov.PlayMsgErroValidacao = $"Não existe a reserva de origem para o produto: {mov.PRO_ID_ORIGEM}, " +
                                    $"lote {mov.MOV_LOTE_ORIGEM} e sublote {mov.MOV_SUB_LOTE}.";
                                mov.PlayAction = "ERRO";
                                return false;
                            }

                            if (reservaDestino != null)
                            {
                                /* PENDENCIA
                                 * Atualizar peso da reserva com a média ponderada do lote origem e destino
                                 */

                                ///se o lote destino tiver reserva char set reserva com a quantidade do lote destino
                                reservaDestino.USE_ID = mov.UsuarioLogado.USE_ID;
                                reservaDestino.MOV_QUANTIDADE = saldoLoteDestino.SALDO.Value + mov.MOV_QUANTIDADE;
                                reservaDestino.PlayAction = "update";
                                Transferencia.Add(reservaDestino);
                            }
                            else //Caso não exista reserva para o lote de destino
                            {
                                reservaDestino = new MovimentoEstoqueReservaDeEstoque()
                                    .GetReserva(mov.MOV_LOTE_DESTINO, mov.MOV_SUB_LOTE_DESTINO, mov.PRO_ID_DESTINO);

                                if (reservaDestino.ORD_ID == null || reservaDestino.ORD_ID.Trim() == "")
                                {
                                    /* No método GetReserva a etiqueta não foi encontrada
                                     * portanto o ORD_ID e FPR_SEQ_REPETICAO serão preenchidos aqui.
                                     */
                                    reservaDestino.ORD_ID = mov.ORD_ID;
                                    reservaDestino.FPR_SEQ_REPETICAO = 1;
                                }

                                reservaDestino.USE_ID = mov.UsuarioLogado.USE_ID;
                                reservaDestino.PRO_ID_ORIGEM = mov.PRO_ID_ORIGEM;
                                reservaDestino.MOV_LOTE_ORIGEM = mov.MOV_LOTE_ORIGEM;
                                reservaDestino.MOV_SUB_LOTE_ORIGEM = mov.MOV_SUB_LOTE_ORIGEM;
                                reservaDestino.MOV_QUANTIDADE = mov.MOV_QUANTIDADE;
                                reservaDestino.MOV_ENDERECO = mov.MOV_ENDERECO;
                                reservaDestino.MOV_PESO_UNITARIO = reservaOrigem.MOV_PESO_UNITARIO;

                                Transferencia.Add(reservaDestino);
                            }
                            
                            // executa transferencia 
                            Transferencia.Add(new MovimentoEstoqueTransferenciaSimples()
                            {// movimento de saida do lote 
                                TIP_ID = "800",
                                PRO_ID = mov.PRO_ID_ORIGEM,
                                MOV_QUANTIDADE = mov.MOV_QUANTIDADE,
                                MOV_LOTE = mov.MOV_LOTE_ORIGEM,
                                MOV_SUB_LOTE = mov.MOV_SUB_LOTE_ORIGEM,
                                ORD_ID = reservaOrigem.ORD_ID,
                                FPR_SEQ_TRANFORMACAO = reservaOrigem.FPR_SEQ_TRANFORMACAO,
                                FPR_SEQ_REPETICAO = reservaOrigem.FPR_SEQ_REPETICAO.Value,
                                OCO_ID = null,
                                MOV_ARMAZEM = reservaOrigem.MOV_ARMAZEM,
                                MOV_ENDERECO = reservaOrigem.MOV_ENDERECO,
                                PRO_ID_DESTINO = mov.PRO_ID_DESTINO,
                                MOV_LOTE_DESTINO = mov.MOV_LOTE_DESTINO,
                                MOV_SUB_LOTE_DESTINO = mov.MOV_SUB_LOTE_DESTINO,
                                TURM_ID = reservaOrigem.TURM_ID,
                                TURN_ID = null,
                                MAQ_ID = "PLAYSIS",
                                PlayAction = "insert"
                            });

                            Transferencia.Add(new MovimentoEstoqueTransferenciaSimples()
                            {// movimento de entrada do lote
                                TIP_ID = "100",
                                PRO_ID = mov.PRO_ID_DESTINO,
                                MOV_QUANTIDADE = mov.MOV_QUANTIDADE,
                                MOV_LOTE = mov.MOV_LOTE_DESTINO,
                                MOV_SUB_LOTE = mov.MOV_SUB_LOTE_DESTINO,
                                ORD_ID = reservaDestino.ORD_ID ?? mov.ORD_ID,
                                FPR_SEQ_TRANFORMACAO = reservaDestino.FPR_SEQ_TRANFORMACAO ?? 0,
                                FPR_SEQ_REPETICAO = reservaDestino.FPR_SEQ_REPETICAO ?? 1,
                                OCO_ID = null,
                                MOV_ARMAZEM = reservaDestino.MOV_ARMAZEM ?? "",
                                MOV_ENDERECO = reservaDestino.MOV_ENDERECO ?? "",
                                PRO_ID_ORIGEM = mov.PRO_ID_ORIGEM,
                                MOV_LOTE_ORIGEM = mov.MOV_LOTE_ORIGEM,
                                MOV_SUB_LOTE_ORIGEM = mov.MOV_SUB_LOTE_ORIGEM,
                                TURM_ID = reservaDestino.TURM_ID ?? "",
                                TURN_ID = null,
                                MAQ_ID = "PLAYSIS",
                                PlayAction = "insert"
                            });
                            break;
                        case "update":
                            break;
                        case "delete":
                            break;
                    }
                        
                        
                }
                
                objects.AddRange(Transferencia);
            }
            return true;
        }
        public bool AfterChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert, JSgi db = null) 
        {
            List<Etiqueta> etiquetas = new List<Etiqueta>();

            foreach (var item in objects)
            {
                if (item.GetType().Name != nameof(MovimentoEstoqueTransferenciaSimples))
                    continue;

                MovimentoEstoqueTransferenciaSimples mov = (MovimentoEstoqueTransferenciaSimples)item;

                if (!mov.PlayAction.Equals("insert", StringComparison.OrdinalIgnoreCase) &&
                    !mov.PlayAction.Equals("update", StringComparison.OrdinalIgnoreCase))
                    continue;

                // gerando etiqueta do lote
                var etiqueta = new Etiqueta()
                    .GerarEtiquetaLoteProduto(mov.PRO_ID, mov.MOV_LOTE, mov.MOV_SUB_LOTE, mov.FPR_SEQ_REPETICAO.Value, Logs, mov.UsuarioLogado.USE_ID);

                etiquetas.Add(etiqueta);
            }

            if (!Logs.Any(x => x.Status.Equals("ERRO")))
            {
                var Ids = String.Join(",", etiquetas.Select(x => x.ETI_ID).ToArray());
                Logs.Add(new LogPlay(this.ToString(), "PROTOCOLO", "LINK", "/PlugAndPlay/ReportEtiqueta/EtiquetaIndividualLoteProduto?etiqueta=", $"{Ids}"));
            }

            return true;
        }
        public void MovimentoEstoqueTransferenciaSimplesToInterfaceTelaTransferenciaSimples(List<object> objects)
        {
            List<object> newList = new List<object>();
            foreach (var obj in objects)
            {
                if (obj.GetType().Name != nameof(MovimentoEstoqueTransferenciaSimples))
                    continue;

                var movTransferencia = (MovimentoEstoqueTransferenciaSimples)obj;

                var movTranfInterface = new InterfaceTelaTransferenciaSimples();
                movTranfInterface.MOV_LOTE_ORIGEM = String.IsNullOrEmpty(movTransferencia.MOV_LOTE_ORIGEM?.Trim()) ? 
                    movTransferencia.MOV_LOTE : movTransferencia.MOV_LOTE_ORIGEM;
                
                movTranfInterface.MOV_SUB_LOTE_ORIGEM = String.IsNullOrEmpty(movTransferencia.MOV_SUB_LOTE_ORIGEM?.Trim()) ?
                    movTransferencia.MOV_SUB_LOTE : movTransferencia.MOV_SUB_LOTE_ORIGEM;
                
                movTranfInterface.MOV_LOTE_DESTINO = String.IsNullOrEmpty(movTransferencia.MOV_LOTE_DESTINO?.Trim()) ?
                    movTransferencia.MOV_LOTE : movTransferencia.MOV_LOTE_DESTINO;
                
                movTranfInterface.MOV_SUB_LOTE_DESTINO = String.IsNullOrEmpty(movTransferencia.MOV_SUB_LOTE_DESTINO?.Trim()) ?
                    movTransferencia.MOV_SUB_LOTE : movTransferencia.MOV_SUB_LOTE_DESTINO;
                
                movTranfInterface.PRO_ID_DESTINO = String.IsNullOrEmpty(movTransferencia.PRO_ID_DESTINO?.Trim()) ?
                    movTransferencia.PRO_ID : movTransferencia.PRO_ID_DESTINO;
                
                movTranfInterface.ORD_ID = movTransferencia.ORD_ID;
                
                movTranfInterface.MOV_QUANTIDADE = movTransferencia.MOV_QUANTIDADE;
                
                movTranfInterface.MOV_DOC = movTransferencia.MOV_DOC;
                
                movTranfInterface.MOV_ENDERECO = movTransferencia.MOV_ENDERECO;
                
                movTranfInterface.MOV_ID = movTransferencia.MOV_ID;

                newList.Add(movTranfInterface);
            }
            objects.Clear();
            objects.AddRange(newList);
        }
    }
    [Display(Name = "DESMONTAGEM DE LOTES")]
    public class InterfaceTelaTransferenciaSimples : InterfaceDeTelas
    {
        public InterfaceTelaTransferenciaSimples()
        {
            NamespaceOfClassMapped = typeof(MovimentoEstoqueTransferenciaSimples).FullName;
        }
        [SEARCH_NOT_FK(namespaceOfForeignClass = "DynamicForms.Areas.PlugAndPlay.Models.MovimentoEstoque")]
        [TAB(Value = "TRANSFERE LOTE")] [Display(Name = "LOTE ORIGEM")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MOV_LOTE_ORIGEM")] public string MOV_LOTE_ORIGEM { get; set; }
        [SEARCH_NOT_FK(namespaceOfForeignClass = "DynamicForms.Areas.PlugAndPlay.Models.MovimentoEstoque")]
        [TAB(Value = "TRANSFERE LOTE")] [Display(Name = "SUB LOTE ORIGEM")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_SUB_LOTE_ORIGEM")] public string MOV_SUB_LOTE_ORIGEM { get; set; }
        [SEARCH_NOT_FK(namespaceOfForeignClass = "DynamicForms.Areas.PlugAndPlay.Models.MovimentoEstoque")]
        [TAB(Value = "TRANSFERE LOTE")] [Display(Name = "LOTE DESTINO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MOV_LOTE_DESTINO")] public string MOV_LOTE_DESTINO { get; set; }
        [SEARCH_NOT_FK(namespaceOfForeignClass = "DynamicForms.Areas.PlugAndPlay.Models.MovimentoEstoque")]
        [TAB(Value = "TRANSFERE LOTE")] [Display(Name = "SUB LOTE DESTINO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_SUB_LOTE_DESTINO")] public string MOV_SUB_LOTE_DESTINO { get; set; }
        [SEARCH_NOT_FK(namespaceOfForeignClass = "DynamicForms.Areas.PlugAndPlay.Models.Produto")]
        [TAB(Value = "TRANSFERE LOTE")] [Display(Name = "PROD. DESTINO:")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID_DESTINO")] public string PRO_ID_DESTINO { get; set; }
        [SEARCH_NOT_FK(namespaceOfForeignClass = "DynamicForms.Areas.PlugAndPlay.Models.Order")]
        [TAB(Value = "TRANSFERE LOTE")] [Display(Name = "PEDIDO DESTINO")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "TRANSFERE LOTE")] [Display(Name = "QUANTIDADE")] [Required(ErrorMessage = "Campo MOV_QUANTIDADE requirido.")] public double MOV_QUANTIDADE { get; set; }
        [TAB(Value = "TRANSFERE LOTE")] [Display(Name = "NÚM DOCUMENTO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MOV_DOC")] public string MOV_DOC { get; set; }
        [TAB(Value = "TRANSFERE LOTE")] [Display(Name = "ENDEREÇO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_ENDERECO")] public string MOV_ENDERECO { get; set; }
        [TAB(Value = "OUTROS")] [READ] [Display(Name = "COD. MOVIMENTO")] public int MOV_ID { get; set; }
        [NotMapped] public string MOV_LOTE { get; set; }
        [NotMapped] public string MOV_SUB_LOTE { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        [NotMapped] public string NamespaceOfClassMapped { get; set; }
        [NotMapped] public T_Usuario UsuarioLogado { get; set; }

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            List<object> Transferencias = new List<object>();
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                int mov_id = 0;
                foreach (var item in objects)
                {
                    if (item.GetType().Name != nameof(InterfaceTelaTransferenciaSimples))
                        continue;

                    // DESMONTAGEM DE LOTES - PARTE 01
                    InterfaceTelaTransferenciaSimples itTransferencia = (InterfaceTelaTransferenciaSimples)item;
                    if (String.IsNullOrEmpty(itTransferencia.MOV_DOC?.Trim()))
                    {
                        itTransferencia.MOV_DOC = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
                        do
                        {
                            itTransferencia.MOV_DOC = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
                        } while (db.MovimentoEstoqueAbstrata.Count(mv => mv.MOV_DOC == itTransferencia.MOV_DOC) != 0);
                    }

                    var movAux = db.MovimentoEstoqueReservaDeEstoque.AsNoTracking().
                            Where(x => x.MOV_LOTE == itTransferencia.MOV_LOTE_ORIGEM && x.MOV_SUB_LOTE == itTransferencia.MOV_SUB_LOTE_ORIGEM && x.TIP_ID == "998")
                            .Select(m => new MovimentoEstoqueReservaDeEstoque
                            {
                                PRO_ID = m.PRO_ID,
                                ORD_ID = m.ORD_ID,
                                MOV_ENDERECO = m.MOV_ENDERECO
                            })
                            .FirstOrDefault();

                    if (movAux == null)
                    {// verifica se existe a etiqueta
                        itTransferencia.PlayMsgErroValidacao = " Etiqueta Não Existe";
                        itTransferencia.PlayAction = "ERRO";
                        return false;
                    }

                    if (String.IsNullOrEmpty(itTransferencia.PRO_ID_DESTINO?.Trim()))
                    {
                        itTransferencia.PRO_ID_DESTINO = movAux.PRO_ID;
                    }

                    if (String.IsNullOrEmpty(itTransferencia.ORD_ID?.Trim()))
                    {
                        itTransferencia.ORD_ID = movAux.ORD_ID;
                    }

                    if (String.IsNullOrEmpty(itTransferencia.MOV_ENDERECO?.Trim()))
                    {
                        itTransferencia.MOV_ENDERECO = movAux.MOV_ENDERECO;
                    }

                    if (itTransferencia.PlayAction.Equals("update", StringComparison.OrdinalIgnoreCase))
                    {
                        mov_id = db.MovimentoEstoque.AsNoTracking().Where(m => m.MOV_ID == itTransferencia.MOV_ID).Select(m => m.MOV_ID).FirstOrDefault();
                    }

                    Transferencias.Add(new MovimentoEstoqueTransferenciaSimples()
                    {
                        MOV_ID = mov_id,
                        PRO_ID_ORIGEM = movAux.PRO_ID,
                        MOV_LOTE_ORIGEM = itTransferencia.MOV_LOTE_ORIGEM,
                        MOV_SUB_LOTE_ORIGEM = itTransferencia.MOV_SUB_LOTE_ORIGEM,
                        
                        PRO_ID_DESTINO = itTransferencia.PRO_ID_DESTINO,
                        MOV_LOTE_DESTINO = itTransferencia.MOV_LOTE_DESTINO,
                        MOV_SUB_LOTE_DESTINO = itTransferencia.MOV_SUB_LOTE_DESTINO,
                        
                        ORD_ID = itTransferencia.ORD_ID,
                        MOV_QUANTIDADE = itTransferencia.MOV_QUANTIDADE,
                        MOV_DOC = itTransferencia.MOV_DOC,
                        MOV_ENDERECO = itTransferencia.MOV_ENDERECO,
                        
                        UsuarioLogado = itTransferencia.UsuarioLogado,
                        PlayAction = itTransferencia.PlayAction
                    });
                    itTransferencia.PlayAction = "OK";

                }
            }
            ////transferencia -- lote origem tiver reserva - chamar set rteserva com a quantidade restante dpo lote origem
            ///se o lote destino tiver reserva char set reserva com a quantidade do lote destino
            objects.AddRange(Transferencias);
            return true;
        }
        public bool EstornarTransferencia(List<object> objects, List<LogPlay> Logs)
        {
            bool check = true;
            List<object> listaAuxiliar = new List<object>();
            List<List<object>> ListOfListObjects = new List<List<object>>();
            MasterController mc = new MasterController();
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (var item in objects)
                {

                    if (item.GetType().Name != nameof(InterfaceTelaTransferenciaSimples))
                        continue;

                    //Objeto  retornado da View
                    InterfaceTelaTransferenciaSimples mov = (InterfaceTelaTransferenciaSimples)item;
                    mov.PlayAction = "OK";

                    /* PENDENCIA!!!
                     * Abaixo eu fiz a atribuição dos campos MOV_LOTE_ORIGEM e MOV_SUB_LOTE_ORIGEM 
                     * a partir dos campos MOV_LOTE e MOV_SUB_LOTE, porque quando este método é 
                     * chamado a partir do formulário dynamic, os campos MOV_LOTE_ORIGEM e MOV_SUB_LOTE_ORIGEM 
                     * não estão vindo preenchidos, por isso criei os campos MOV_LOTE, MOV_SUB_LOTE nesta classe.
                     * 
                     * O certo seria os campos MOV_LOTE e MOV_SUB_LOTE virem preenchidos, portando isso precisa ser revisado
                     * futuramente. 
                     */
                    mov.MOV_LOTE_ORIGEM = mov.MOV_LOTE;
                    mov.MOV_SUB_LOTE_ORIGEM = mov.MOV_SUB_LOTE;

                    if (String.IsNullOrEmpty(mov.MOV_LOTE_ORIGEM?.Trim()))
                    {
                        mov.PlayMsgErroValidacao = "Por favor, informe o lote de origem.";
                        return false;
                    }
                    if (String.IsNullOrEmpty(mov.MOV_SUB_LOTE_ORIGEM?.Trim()))
                    {
                        mov.PlayMsgErroValidacao = "Por favor, informe o sublote de origem.";
                        return false;
                    }
                    if (String.IsNullOrEmpty(mov.MOV_LOTE_DESTINO?.Trim()))
                    {
                        mov.PlayMsgErroValidacao = "Por favor, informe o lote de destino.";
                        return false;
                    }
                    if (String.IsNullOrEmpty(mov.MOV_SUB_LOTE_DESTINO?.Trim()))
                    {
                        mov.PlayMsgErroValidacao = "Por favor, informe o sublote de destino";
                        return false;
                    }
                    if (String.IsNullOrEmpty(mov.PRO_ID_DESTINO?.Trim()))
                    {
                        mov.PlayMsgErroValidacao = "Por favor, informe o produto de destino";
                        return false;
                    }

                    string prodOrigem = db.MovimentoEstoqueReservaDeEstoque.AsNoTracking()
                        .Where(m => m.MOV_LOTE == mov.MOV_LOTE_ORIGEM && m.MOV_SUB_LOTE == mov.MOV_SUB_LOTE_ORIGEM)
                        .Select(m => m.PRO_ID).FirstOrDefault();

                    if (String.IsNullOrEmpty(prodOrigem?.Trim()))
                    {
                        mov.PlayMsgErroValidacao = "Não foi encontrado o produto para o lote e sublote de origem";
                        return false;
                    }

                    var movParaEstornar = db.MovimentoEstoqueTransferenciaSimples.AsNoTracking()
                        .Where(m => (m.PRO_ID_ORIGEM == prodOrigem && m.MOV_LOTE_ORIGEM == mov.MOV_LOTE_ORIGEM && m.MOV_SUB_LOTE_ORIGEM == mov.MOV_SUB_LOTE_ORIGEM && m.TIP_ID == "100") ||
                            (m.PRO_ID_DESTINO == mov.PRO_ID_DESTINO && m.MOV_LOTE_DESTINO == mov.MOV_LOTE_DESTINO && m.MOV_SUB_LOTE_DESTINO == mov.MOV_SUB_LOTE_DESTINO && m.TIP_ID == "800"))
                        .ToList();

                    if (!movParaEstornar.Any(m => m.TIP_ID == "100"))
                    {
                        mov.PlayMsgErroValidacao = "Não foi encontrado o lote e sublote de origem informado, portanto a operação não pode ser completada.";
                        return false;
                    }
                    if (!movParaEstornar.Any(m => m.TIP_ID == "800"))
                    {
                        mov.PlayMsgErroValidacao = "Não foi encontrado o lote e sublote de destino informado, portanto a operação não pode ser completada.";
                        return false;
                    }

                    foreach (var m in movParaEstornar)
                    {
                        m.PlayAction = "update";
                        //Alterando STATUS da movimentação como estornada.
                        m.MOV_ESTORNO = "E";
                    }

                    listaAuxiliar.AddRange(movParaEstornar);
                }
            }
            //Adicionando objetos para o update data.
            ListOfListObjects.Add(listaAuxiliar);
            //Concatenando Logs por se tratar de um objeto de interface
            Logs.AddRange(mc.UpdateData(ListOfListObjects, 0, true));
            var resp = new LogPlay().GetLogsErro(Logs);
            if (resp.Count > 0)
            {
                check = false;
            }
            Logs.Add(new LogPlay() { Status = "OK" });
            return check;
        }
    }
}


using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Models;
using DynamicForms.Util;
using Itinero.IO.Xml;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "RESERVAS DE ESTOQUE")]
    public class MovimentoEstoqueReservaDeEstoque : MovimentoEstoqueAbstrata
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LOTE")] [MaxLength(100, ErrorMessage = "Maximo de 100 caracteres, campo MOV_LOTE")] public string MOV_LOTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SUB LOTE")] [MaxLength(30, ErrorMessage = "Maximo de 30 caracteres, campo MOV_SUB_LOTE")] public string MOV_SUB_LOTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PEDIDO")] [MaxLength(60, ErrorMessage = "Maximo de 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PRODUTO")] [Required(ErrorMessage = "Campo PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE")] [Required(ErrorMessage = "Campo MOV_QUANTIDADE requirido.")] public double MOV_QUANTIDADE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PESO UNT.")] [Required(ErrorMessage = "Campo MOV_PESO_UNITARIO requirido.")] public double MOV_PESO_UNITARIO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "SEQ TRANFORMAÇÃO")] public int? FPR_SEQ_TRANFORMACAO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "SEQ REPETIÇÃO")] public int? FPR_SEQ_REPETICAO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "COD OCORRÊNCIA")] [MaxLength(30, ErrorMessage = "Maximo de 30 caracteres, campo OCO_ID")] public string OCO_ID { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "OBSERVAÇÕES")] [MaxLength(200, ErrorMessage = "Maximo de 200 caracteres, campo MOV_OBS")] public string MOV_OBS { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "ARMAZÉM")] [MaxLength(30, ErrorMessage = "Maximo de 30 caracteres, campo MOV_ARMAZEM")] public string MOV_ARMAZEM { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "ENDEREÇO")] [MaxLength(30, ErrorMessage = "Maximo de 30 caracteres, campo MOV_ENDERECO")] public string MOV_ENDERECO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "OBSERVAÇÕES OP PARCIAL")] [MaxLength(200, ErrorMessage = "Maximo de 200 caracteres, campo MOV_OBS_OP_PARCIAL")] public string MOV_OBS_OP_PARCIAL { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "COD OCORRÊNCIA OP PARCIAL")] [MaxLength(30, ErrorMessage = "Maximo de 30 caracteres, campo MOV_OCO_ID_OP_PARCIAL")] public string MOV_OCO_ID_OP_PARCIAL { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "USUÁRIO")] public int? USE_ID { get; set; }
        [Combobox(Description = "NÃO", Value = "N")]
        [Combobox(Description = "SIM", Value = "S")]
        [TAB(Value = "OUTROS")] [Display(Name = "APROVEITAMENTO")] [MaxLength(1, ErrorMessage = "Maximo de 1 caracteres, campo MOV_APROVEITAMENTO")] public string MOV_APROVEITAMENTO { get; set; }
        [Combobox(Description = "AGUARDANDO LIBERAÇÃO", Value = "A")]
        [Combobox(Description = "LOTE RETIDO", Value = "R")]
        [Combobox(Description = "LOTE LIBERADO", Value = "L")]
        [TAB(Value = "OUTROS")] [Display(Name = "LOTE RETIDO(Qualidade)")] [MaxLength(1, ErrorMessage = "Maximo de 1 caracteres, campo MOV_RETIDO")] public string MOV_RETIDO { get; set; }
        [NotMapped] public T_Usuario UsuarioLogado { get; set; }
        public virtual Carga Carga { get; set; }
        public virtual Turno Turno { get; set; }
        public virtual Turma Turma { get; set; }
        public virtual TipoMovRetencao TipoMovRetencao { get; set; }
        public virtual Order Order { get; set; }
        public virtual Maquina Maquina { get; set; }
        public virtual Produto Produto { get; set; }
        public OcorrenciaRetencaoLotes OcorrenciaRetencaoLotes { get; set; }
        public OcorrenciaProducaoParciais OcorrenciaProducaoParciais { get; set; }
        public virtual T_Usuario Usuario { get; set; }

        public override bool ImprimirEtiqueta(List<object> objects, ref List<LogPlay> Logs)
        {
            bool check = true;
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (var item in objects)
                {
                    MovimentoEstoqueReservaDeEstoque _Reserva = (MovimentoEstoqueReservaDeEstoque)item;
                    var Db_Etiqueta = db.Etiqueta.AsNoTracking().Where(x => x.ETI_LOTE == _Reserva.MOV_LOTE && x.ETI_SUB_LOTE == _Reserva.MOV_SUB_LOTE).Select(x => x.ETI_ID).FirstOrDefault();
                    if (Db_Etiqueta == 0)
                    {
                        Logs.Add(new LogPlay(this.ToString(), "ERRO", "Não existe uma etiqueta associada a esta reserva"));
                    }
                    else
                    {
                        InterfaceTelaImpressaoEtiquetas et = new InterfaceTelaImpressaoEtiquetas();
                        Logs.Add(et.ImprimirEt($"{Db_Etiqueta}"));
                    }
                }
            }
            return check;
        }
        //--
        public List<MovimentoEstoqueReservaDeEstoque> TransferirLotesReservados(List<SaldosEmEstoquePorLote> lotesOrigem, string iDpedidoFuturo)
        {
            List<MovimentoEstoqueReservaDeEstoque> _ReservasPedidoFuturo = new List<MovimentoEstoqueReservaDeEstoque>();
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (var item in lotesOrigem)
                {
                    // REAPROVEITAMENTO DE LOTE
                    MovimentoEstoqueReservaDeEstoque reserva = new MovimentoEstoqueReservaDeEstoque();
                    reserva = reserva.GetReserva(item.MOV_LOTE, item.MOV_SUB_LOTE, item.PRO_ID);

                    reserva.ORD_ID = iDpedidoFuturo;
                    reserva.MOV_APROVEITAMENTO = "S";
                    reserva.MOV_QUANTIDADE = item.SALDO.Value;

                    _ReservasPedidoFuturo.Add(reserva);
                }
                return _ReservasPedidoFuturo;
            }
        }
        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (var item in objects)
                {
                    if (item.GetType().Name != nameof(MovimentoEstoqueReservaDeEstoque))
                        continue;

                    MovimentoEstoqueReservaDeEstoque reserva = (MovimentoEstoqueReservaDeEstoque)item;

                    #region Validações da reserva
                    if (String.IsNullOrEmpty(reserva.MOV_LOTE?.Trim()))
                    {
                        reserva.PlayMsgErroValidacao = "O lote deve ser informado.";
                        reserva.PlayAction = "ERRO";
                        return false;
                    }
                    if (String.IsNullOrEmpty(reserva.MOV_SUB_LOTE?.Trim()))
                    {
                        reserva.PlayMsgErroValidacao = "O sublote deve ser informado.";
                        reserva.PlayAction = "ERRO";
                        return false;
                    }
                    if (String.IsNullOrEmpty(reserva.PRO_ID?.Trim()))
                    {
                        reserva.PlayMsgErroValidacao = "O produto deve ser informado.";
                        reserva.PlayAction = "ERRO";
                        return false;
                    }
                    
                    if (reserva.PlayAction == "update")
                    {
                        object cloneDb = cloneObjeto.GetClone(reserva);
                        if (cloneObjeto.getChangedPoperties(reserva, cloneDb).Contains(nameof(CAR_ID)))
                        {
                            var valorAtual = cloneDb.GetType().GetProperty(nameof(CAR_ID))
                                .GetValue(new MovimentoEstoqueReservaDeEstoque())?.ToString();
                            
                            if (!String.IsNullOrEmpty(valorAtual?.Trim()) && !String.IsNullOrEmpty(reserva.CAR_ID?.Trim()))
                            {// Não pode alterar a carga. Ex: Carga01 para Carga02
                                reserva.PlayMsgErroValidacao = $"Não é permitido alterar a carga de {valorAtual} para {reserva.CAR_ID} desta reserva.";
                                reserva.PlayAction = "ERRO";
                                return false;
                            }
                        }
                    }

                    #endregion Validações da reserva
                }
            }
            return true;
        }

        /// <summary>
        /// Retorna o objeto do movimento de reserva para este lote, sub lote e produto.
        /// Se o registro já existir na base de dados, o mesmo será retornado com <see cref="PlayAction"/> "update".
        /// Se o registro não existir na base de dados, será retornado um com <see cref="PlayAction"/> "insert" e 
        /// os valores padrões preenchidos.
        /// </summary>
        /// <param name="lote">Lote do movimento de reserva.</param>
        /// <param name="subLote">Sub lote do movimento de reserva.</param>
        /// <param name="proId">Produto do movimento de reserva.</param>
        /// <param name="etiqueta">(Opcional) Etiqueta que será utilizada para criar o movimento de reserva.</param>
        /// <returns></returns>
        [HIDDEN]
        public MovimentoEstoqueReservaDeEstoque GetReserva(string lote, string subLote, string proId, Etiqueta etiqueta = null)
        {
            MovimentoEstoqueReservaDeEstoque reserva = null;

            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                reserva = db.MovimentoEstoqueReservaDeEstoque.AsNoTracking()
                    .Where(m => m.MOV_LOTE == lote && m.MOV_SUB_LOTE == subLote && m.PRO_ID == proId && m.MOV_ESTORNO != "E")
                    .FirstOrDefault();

                if (reserva != null)
                { // A reserva já existe na base de dados.
                    reserva.PlayAction = "update";
                }
                else
                { // A reserva não existe na base de dados.

                    if (etiqueta == null)
                    {// buscar a etiqueta na base de dados.
                        etiqueta = db.Etiqueta.AsNoTracking()
                            .Where(e => e.ETI_LOTE == lote && e.ETI_SUB_LOTE == subLote && e.ROT_PRO_ID == proId)
                            .FirstOrDefault();
                    }

                    var produto = db.Produto.AsNoTracking()
                        .Where(p => p.PRO_ID == proId)
                        .Select(p => new Produto
                        {
                            PRO_ID = proId,
                            PRO_PESO = p.PRO_PESO
                        }).FirstOrDefault();

                    // criando o movimento de reserva
                    reserva = new MovimentoEstoqueReservaDeEstoque();
                    reserva.TIP_ID = "998";
                    reserva.PRO_ID = proId;
                    reserva.MOV_LOTE = lote;
                    reserva.MOV_SUB_LOTE = subLote;
                    reserva.MOV_PESO_UNITARIO = produto.PRO_PESO.GetValueOrDefault(0);
                    reserva.MOV_DATA_HORA_CRIACAO = DateTime.Now;
                    reserva.MOV_DATA_HORA_EMISSAO = DateTime.Now;//ParametrosSingleton.Instance.DataBase;
                    reserva.MOV_DIA_TURMA = ParametrosSingleton.DiaTurmaS();
                    reserva.MOV_ARMAZEM = ParametrosSingleton.Instance.Armazem;
                    reserva.PlayAction = "insert";
                    
                    if (etiqueta != null)
                    {
                        reserva.ORD_ID = etiqueta.ORD_ID;
                        reserva.FPR_SEQ_REPETICAO = etiqueta.FPR_SEQ_REPETICAO;
                        reserva.MAQ_ID = etiqueta.MAQ_ID;
                    }
                    /* Se não existir etiqueta para este lote, sublote e produto,
                     * o chamador deste método ficará responsável por preencher os 
                     * campos que seriam preenchidos com os dados da etiqueta.
                     */
                }
            }

            return reserva;
        }

        private InterfaceTelaEnderecamento MovimentoEstoqueReservaDeEstoqueToInterfaceTelaEnderecamento()
        {
            return new InterfaceTelaEnderecamento()
            {
                MOV_LOTE = this.MOV_LOTE,
                MOV_SUB_LOTE = this.MOV_SUB_LOTE,
                MOV_ENDERECO = this.MOV_ENDERECO,
                UsuarioLogado = UsuarioLogado
            };
        }
    }
    [Display(Name = "ROMANEAR CARGA")]
    public class Romaneio : InterfaceDeTelas
    {
        [Display(Name = "CÓDIGO BARRAS ETIQUETA")]
        [TAB(Value = "PRINCIPAL")] [SEARCH] public string CodigoDeBarras { get; set; }
        [SEARCH_NOT_FK(namespaceOfForeignClass = "DynamicForms.Areas.PlugAndPlay.Models.Carga")]
        [Persistent]
        [Display(Name = "CÓDIGO DA CARGA")]
        [TAB(Value = "PRINCIPAL")] public string CargaId { get; set; }
        [SEARCH_NOT_FK(namespaceOfForeignClass = "DynamicForms.Areas.PlugAndPlay.Models.Order")]
        [Persistent]
        [Display(Name = "CÓDIGO DO PEDIDO")]
        [MaxLength(30, ErrorMessage = "Codigo do pedido de 30 caracteres, campo ORD_ID")]
        [TAB(Value = "PRINCIPAL")] public string PedidoId { get; set; }
        [Combobox(Value = "S", Description = "INCLUIR")]
        [Combobox(Value = "N", Description = "EXCLUIR")]
        [Persistent]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "INCLUIR NO ROMANEIO")] [MaxLength(1, ErrorMessage = "INLCUIR S/N?")] public string INCLUIR { get; set; }
        [NotMapped] public int TIPO { get; set; }
        //[NotMapped] public string PedidoOriginal { get; set; } //Utilizado para o Romaneio simplificado na substituição de pedidos tipo faturamento
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        [NotMapped] public T_Usuario UsuarioLogado { get; set; }

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                List<object> objetosParaPersistir = new List<object>();
                foreach (var item in objects)
                {
                    if (item.GetType().Name != nameof(Romaneio))
                        continue;

                    // ROMANEIO DE CARGA

                    Romaneio romaneio = (Romaneio)item;

                    var etiqueta = db.Etiqueta.Where(x => x.ETI_CODIGO_BARRAS.Equals(romaneio.CodigoDeBarras)).FirstOrDefault();
                    if (etiqueta == null)
                    {
                        romaneio.PlayMsgErroValidacao = "A etiqueta não existe, informe um código de barras válido.";
                        romaneio.PlayAction = "ERRO";
                        return false;
                    }

                    SaldosEmEstoquePorLote saldoEmEstoque = db.SaldosEmEstoquePorLote.AsNoTracking()
                            .Where(x => x.MOV_LOTE == etiqueta.ETI_LOTE && x.MOV_SUB_LOTE == etiqueta.ETI_SUB_LOTE && x.PRO_ID == etiqueta.ROT_PRO_ID)
                            .FirstOrDefault();

                    // validação de saldo em estoque
                    if (saldoEmEstoque == null || saldoEmEstoque.SALDO <= 0)
                    {
                        romaneio.PlayMsgErroValidacao = $"Não há saldo em estoque para este lote:[{etiqueta.ETI_LOTE}] Sublote:[{etiqueta.ETI_SUB_LOTE}]";
                        romaneio.PlayAction = "ERRO";
                        return false;
                    }

                    var reserva = new MovimentoEstoqueReservaDeEstoque();
                    reserva = reserva.GetReserva(etiqueta.ETI_LOTE, etiqueta.ETI_SUB_LOTE, etiqueta.ROT_PRO_ID, etiqueta);
                    if (reserva.PlayAction == "insert")
                    {
                        /* PENDENCIA
                            * A reserva não existia e foi criada automaticamente,
                            * mas precisa apontar a produção caso esteja apontada.
                            * 
                        */
                        romaneio.PlayMsgErroValidacao = $"Não existe reserva para o PRODUTO:[{etiqueta.ROT_PRO_ID}] LOTE: [{etiqueta.ETI_LOTE}], SUBLOTE: [{etiqueta.ETI_SUB_LOTE}]. ";
                        romaneio.PlayAction = "ERRO";
                        return false;
                    }

                    var ordinalCase = StringComparison.OrdinalIgnoreCase;
                    if (romaneio.INCLUIR.Equals("S", ordinalCase))
                    {//Inclusão do romaneio

                        // validação de lote romaneado
                        if (!String.IsNullOrEmpty(saldoEmEstoque.CAR_ID?.Trim()))
                        {// Este lote está romaneado em outra carga.
                            romaneio.PlayMsgErroValidacao = $"Este lote:[{etiqueta.ETI_LOTE}] e sublote:[{etiqueta.ETI_SUB_LOTE}] já está romaneado para a Carga: {saldoEmEstoque.CAR_ID}";
                            romaneio.PlayAction = "ERRO";
                            return false;
                        }

                        // nesta view vem todos os itens da carga junto com o valor já romaneado de cada item 
                        var itensDaCarga = db.V_ITENS_ROMANEADOS.AsNoTracking().Where(i => i.CAR_ID == romaneio.CargaId).ToList();

                        //Verifica se exite uma carga associada ao pedido do lote desta etiqueta.
                        if (itensDaCarga.Count == 0)
                        {
                            romaneio.PlayMsgErroValidacao = $"Codigo da carga [{romaneio.CargaId}] não possui itens associados ou não foi encontrado\n " +
                                                            $"Verifique a situação dos itens planejados  no menu de cargas do sistema.";
                            romaneio.PlayAction = "ERRO";
                            return false;
                        }

                        bool loteDisponivel;
                        if (saldoEmEstoque.ORD_ID != null && saldoEmEstoque.ORD_ID.Trim() != "")
                        {// descobrir se é disponivel ou compromissado
                            
                            var tipoPedido = db.Order.AsNoTracking().Where(o => o.ORD_ID == saldoEmEstoque.ORD_ID).Select(o => o.ORD_TIPO).FirstOrDefault();
                            var tiposDisponiveis = new List<int?> { 2, 3, 5 };
                            
                            if (tiposDisponiveis.Any(x => x.Value == tipoPedido))
                            {
                                /* Se o pedido da reserva for do tipo
                                 * 2 [SOMENTE PRODUÇÃO], 3 [SOMENTE EXPEDIÇÃO] ou 5 [SOMENTE RETRABALHO], automaticamente torna o lote disponível
                                 */
                                loteDisponivel = true;
                            }
                            else
                            {
                                var saldoPedido = db.SaldoPedido.AsNoTracking()
                                    .Where(s => s.ORD_ID == saldoEmEstoque.ORD_ID)
                                    .Select(s => new SaldoPedido
                                    {
                                        SALDO = s.SALDO
                                    })
                                    .FirstOrDefault();

                                if (saldoPedido.SALDO > 0)
                                {// está compromissado, pois o pedido não está totalmente atendido
                                    loteDisponivel = false;
                                }
                                else
                                {// está disponível, pois o pedido já está atendido
                                 
                                    loteDisponivel = true;
                                }
                            }
                        }
                        else
                        {
                            loteDisponivel = true;
                        }

                        V_ITENS_ROMANEADOS pedidoRomaneado = null;
                        double PlanejadoMaisTolerancia = 0.0;
                        if (loteDisponivel)
                        {// lote disponivel
                            if (romaneio.PedidoId != null && romaneio.PedidoId.Trim() != "")
                            {
                                pedidoRomaneado = itensDaCarga
                                    .Where(x => x.CAR_ID == romaneio.CargaId && x.ORD_ID == romaneio.PedidoId && x.PRO_ID == saldoEmEstoque.PRO_ID)
                                    .FirstOrDefault();
                            }
                            else
                            {
                                var itensCargaComMesmoProduto = itensDaCarga.Where(i => i.CAR_ID == romaneio.CargaId && i.PRO_ID == saldoEmEstoque.PRO_ID).ToList();
                                if (itensCargaComMesmoProduto.Count == 1)
                                {
                                    pedidoRomaneado = itensCargaComMesmoProduto.First();
                                }
                                else if (itensCargaComMesmoProduto.Count > 1)
                                {
                                    foreach (var itemCarga in itensCargaComMesmoProduto)
                                    {
                                        //escolhe o pedido que nao foi atendido entre os pedidos da carga com o mesmo produto 
                                        PlanejadoMaisTolerancia = Math.Round(((itemCarga.ORD_TOLERANCIA_MAIS.Value / 100.0) + 1) * itemCarga.ITC_QTD_PLANEJADA);
                                        if (itemCarga.QTD_ROMANEADA + saldoEmEstoque.SALDO <= PlanejadoMaisTolerancia)
                                        {
                                            pedidoRomaneado = itemCarga;
                                            break;
                                        }
                                    }
                                    if (pedidoRomaneado == null)
                                    {// todos os pedidos estão atendidos
                                        romaneio.PlayMsgErroValidacao = $"Você nao pode carregar uma quantidade maior que a planejada {PlanejadoMaisTolerancia} para o pedido.";
                                        romaneio.PlayAction = "ERRO";
                                        return false;
                                    }
                                }
                                else
                                {
                                    romaneio.PlayMsgErroValidacao = $"Este lote {saldoEmEstoque.MOV_LOTE} sub-lote: ${saldoEmEstoque.MOV_SUB_LOTE} não pertence a carga.";
                                    romaneio.PlayAction = "ERRO";
                                    return false;
                                }
                            }
                        }
                        else
                        {// lote compromissado
                            pedidoRomaneado = itensDaCarga.Where(i => i.CAR_ID == romaneio.CargaId && i.ORD_ID == saldoEmEstoque.ORD_ID)
                                .FirstOrDefault();
                        }

                        if (pedidoRomaneado == null)
                        {
                            romaneio.PlayMsgErroValidacao = $"O pedido {saldoEmEstoque.ORD_ID} do lote {saldoEmEstoque.MOV_LOTE} não faz parte desta carga.";
                            romaneio.PlayAction = "ERRO";
                            return false;
                        }

                        // verficar se já passou da tolerância máxima do pedido
                        PlanejadoMaisTolerancia = Math.Round(((pedidoRomaneado.ORD_TOLERANCIA_MAIS.Value / 100.0) + 1) * pedidoRomaneado.ITC_QTD_PLANEJADA);

                        if (pedidoRomaneado.QTD_ROMANEADA + saldoEmEstoque.SALDO > PlanejadoMaisTolerancia)
                        {
                            romaneio.PlayMsgErroValidacao = "Você nao pode carregar uma quantidade maior que a planejada para o pedido.";
                            romaneio.PlayAction = "ERRO";
                            return false;
                        }

                        reserva.CAR_ID = romaneio.CargaId;
                        reserva.ORD_ID = pedidoRomaneado.ORD_ID;
                        objetosParaPersistir.Add(reserva);

                        var CargaEmRomaneio = db.Carga.AsNoTracking()
                                            .Where(c => c.CAR_ID.Equals(romaneio.CargaId))
                                            .FirstOrDefault();

                        CargaEmRomaneio.CAR_STATUS = 5;
                        CargaEmRomaneio.PlayAction = "update";
                        objetosParaPersistir.Add(CargaEmRomaneio);

                        //Alterando PLayAction para que o sistema não tente persistir o objeto de interface 
                        romaneio.PlayAction = "OK";
                    }
                    else if (romaneio.INCLUIR.Equals("N", ordinalCase))
                    {//Exclusão do romaneio
                        if (saldoEmEstoque.CAR_ID != romaneio.CargaId)
                        {
                            romaneio.PlayMsgErroValidacao = $"Este lote {saldoEmEstoque.MOV_LOTE}.{reserva.FPR_SEQ_REPETICAO}.{saldoEmEstoque.MOV_SUB_LOTE} não faz parte desta carga.";
                            romaneio.PlayAction = "ERRO";
                            return false;
                        }

                        //if (romaneio.TIPO == 3)
                        //{
                        //    var idPedidosNacarga = db.ItenCarga.Where(x => x.CAR_ID.Equals(romaneio.CargaId)).Select(x => x.ORD_ID).ToList();
                        //    var infoPed = db.Order.AsNoTracking().Where(x => idPedidosNacarga.Contains(x.ORD_ID) && x.PRO_ID.Equals(etiqueta_Db.ROT_PRO_ID))
                        //        .Select(x => new { x.ORD_ID, x.ORD_TIPO }).FirstOrDefault();
                        //    auxOrd = infoPed.ORD_ID;
                        //}

                        reserva.CAR_ID = null;
                        objetosParaPersistir.Add(reserva);
                        romaneio.PlayAction = "OK";
                    }
                    else
                    {
                        romaneio.PlayMsgErroValidacao = "Você deve selecionar Incluir ou Excluir na tela de Romaneio.";
                        objetosParaPersistir.Add(romaneio);
                        return true;
                    }
                }
                objects.AddRange(objetosParaPersistir);
            }
            return true;
        }
    }

    [Display(Name = "ENDEREÇAMENTO")]
    public class InterfaceTelaEnderecamento : InterfaceDeTelas
    {
        public InterfaceTelaEnderecamento()
        {
            NamespaceOfClassMapped = typeof(MovimentoEstoqueReservaDeEstoque).FullName;
        }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ENDEREÇO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_ENDERECO")] public string MOV_ENDERECO { get; set; }
        [SEARCH] [TAB(Value = "PRINCIPAL")] [Display(Name = "CODIGO DE BARRAS")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_SUB_LOTE")] public string ETI_CODIGO_BARRAS { get; set; }
        [NotMapped] public string MOV_LOTE { get; set; }
        [NotMapped] public string MOV_SUB_LOTE { get; set; }
        [NotMapped] public string NamespaceOfClassMapped { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        [NotMapped] public T_Usuario UsuarioLogado { get; set; }

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            List<object> Enderecos = new List<object>();
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (var item in objects)
                {
                    if (item.GetType().Name != nameof(InterfaceTelaEnderecamento))
                        continue;

                    // ENDEREÇAMENTO DE LOTE

                    //Objeto retornado da Interface
                    InterfaceTelaEnderecamento enderecamento = (InterfaceTelaEnderecamento)item;

                    #region Validações
                    if (String.IsNullOrEmpty(enderecamento.ETI_CODIGO_BARRAS?.Trim()))
                    {
                        enderecamento.PlayMsgErroValidacao = "Você deve informar o código de barras do lote para efetuar o endereçamento!";
                        enderecamento.PlayAction = "ERRO";
                        return false;
                    }
                    if (String.IsNullOrEmpty(enderecamento.MOV_ENDERECO?.Trim()))
                    {
                        enderecamento.PlayMsgErroValidacao = "Você deve informar o endereço para o lote!";
                        enderecamento.PlayAction = "ERRO";
                        return false;
                    }

                    Etiqueta Db_Etiqueta = db.Etiqueta.AsNoTracking().Where(e => e.ETI_CODIGO_BARRAS.Equals(enderecamento.ETI_CODIGO_BARRAS)).FirstOrDefault();
                    if (Db_Etiqueta == null)
                    {
                        enderecamento.PlayMsgErroValidacao = "Lote não encontrado, verifique o código de barras informado!";
                        enderecamento.PlayAction = "ERRO";
                        return false;
                    }
                    #endregion Validações

                    enderecamento.PlayAction = "OK";
                    enderecamento.MOV_LOTE = Db_Etiqueta.ETI_LOTE;
                    enderecamento.MOV_SUB_LOTE = Db_Etiqueta.ETI_SUB_LOTE;
                    
                    MovimentoEstoqueReservaDeEstoque movReserva = new MovimentoEstoqueReservaDeEstoque();
                    movReserva = movReserva.GetReserva(Db_Etiqueta.ETI_LOTE, Db_Etiqueta.ETI_SUB_LOTE, Db_Etiqueta.ROT_PRO_ID, Db_Etiqueta);
                    movReserva.MOV_ENDERECO = enderecamento.MOV_ENDERECO;
                    Enderecos.Add(movReserva);

                }
                objects.AddRange(Enderecos);
            }
            return true;
        }

        MovimentoEstoqueReservaDeEstoque InterfaceTelaEnderecamentoToMovimentoEstoqueReservaDeEstoque()
        {
            return new MovimentoEstoqueReservaDeEstoque()
            {
                MOV_LOTE = this.MOV_LOTE,
                MOV_SUB_LOTE = this.MOV_SUB_LOTE,
                MOV_ENDERECO = this.MOV_ENDERECO,
                UsuarioLogado = UsuarioLogado
            };
        }
    }
}

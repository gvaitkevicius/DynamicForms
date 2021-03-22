
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
using System.Text;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "PEDIDOS")]
    public class Order
    {
        public Order()
        {
            this.Medicoes = new HashSet<Feedback>();
            this.FilasProducao = new HashSet<FilaProducao>();
            this.TargetsProduto = new HashSet<TargetProduto>();
            this.ItensCarga = new HashSet<ItenCarga>();
            this.V_ITEM_CARGA = new HashSet<V_ITEM_CARGA>();
            this.Etiquetas = new HashSet<Etiqueta>();
            this.MovimentoEstoqueProducao = new HashSet<MovimentoEstoqueProducao>();
            this.MovimentoEstoqueReservaDeEstoque = new HashSet<MovimentoEstoqueReservaDeEstoque>();
            this.MovimentoEstoqueVendas = new HashSet<MovimentoEstoqueVendas>();
            this.MovimentoEstoqueEntradaInventario = new HashSet<MovimentoEstoqueEntradaInventario>();
            this.MovimentoEstoqueSaidaInventario = new HashSet<MovimentoEstoqueSaidaInventario>();
            this.MovimentoEstoqueTransferenciaSimples = new HashSet<MovimentoEstoqueTransferenciaSimples>();
            this.MovimentoEstoqueConsumoMateriaPrima = new HashSet<MovimentoEstoqueConsumoMateriaPrima>();
            this.MovimentoEstoquePerdas = new HashSet<MovimentoEstoquePerdas>();
            this.MovimentoEstoqueDevolucao = new HashSet<MovimentoEstoqueDevolucao>();
            this.ViewFilaProducao = new HashSet<ViewFilaProducao>();
            this.ImpressaoEtiquetasOnd = new HashSet<V_IMPRESSAO_ETIQUETAS_OND>();
        }
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
        [Required(ErrorMessage = "Campo MUN_ID_ENTREGA requerido.")]
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
        [TAB(Value = "OUTROS")] [Display(Name = "COD PROD INTEGRACAO ERP ")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID_INTEGRACAO_ERP")] public string PRO_ID_INTEGRACAO_ERP { get; set; }
        

        [TAB(Value = "OUTROS")] [Display(Name = "COD INTEGRACAO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo ORD_ID_INTEGRACAO")] public string ORD_ID_INTEGRACAO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "ENTREGA ATE")] public DateTime ORD_DATA_ENTREGA_ATE { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "MIT")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo ORD_MIT")] public string ORD_MIT { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "TOL +")] public double? ORD_TOLERANCIA_MAIS { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "TOL -")] public double? ORD_TOLERANCIA_MENOS { get; set; }

        [TAB(Value = "OUTROS")] [Display(Name = "VINCOS ONDULADEIRA")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo VINCOS_ONDULADEIRA")] public string ORD_VINCOS_ONDULADEIRA { get; set; }
        
        [Combobox(Value = "", Description = "Atualiza pelo sistema de origem")]
        [Combobox(Value = "-1", Description = "Atualiza pelo PLAYSIS")]
        [TAB(Value = "OUTROS")] [Display(Name = "Interface")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo HASH_KEY")] public string HASH_KEY { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "REPRESENTANTE ID")] public int? REP_ID { get; set; }
        public virtual Representantes Representantes { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual Produto Produto { get; set; }
        public virtual Municipio Municipio { get; set; }
        public virtual PontosMapa PontosMapa { get; set; }
        [Display(Name = "CARGA")] public virtual ICollection<V_ITEM_CARGA> V_ITEM_CARGA { get; set; }
        [Display(Name = "ORDENS DE PRODUÇÃO/SERVIÇO")] public virtual ICollection<FilaProducao> FilasProducao { get; set; }
        [TAB(Value = "FILA")] [Display(Name = "FILA")] public virtual ICollection<ViewFilaProducao> ViewFilaProducao { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<MovimentoEstoqueReservaDeEstoque> MovimentoEstoqueReservaDeEstoque { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<Feedback> Medicoes { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<TargetProduto> TargetsProduto { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<ItenCarga> ItensCarga { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<Etiqueta> Etiquetas { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<MovimentoEstoqueProducao> MovimentoEstoqueProducao { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<MovimentoEstoqueVendas> MovimentoEstoqueVendas { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<MovimentoEstoqueEntradaInventario> MovimentoEstoqueEntradaInventario { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<MovimentoEstoqueSaidaInventario> MovimentoEstoqueSaidaInventario { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<MovimentoEstoqueTransferenciaSimples> MovimentoEstoqueTransferenciaSimples { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<MovimentoEstoqueConsumoMateriaPrima> MovimentoEstoqueConsumoMateriaPrima { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<MovimentoEstoquePerdas> MovimentoEstoquePerdas { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<MovimentoEstoqueDevolucao> MovimentoEstoqueDevolucao { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<V_IMPRESSAO_ETIQUETAS_OND> ImpressaoEtiquetasOnd { get; set; }


        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                List<object> newList = new List<object>(objects);

                foreach (var item in objects)
                {
                    if (item.GetType().Name == nameof(Order))
                    {
                        Order _order = (Order)item;

                        if (_order.ORD_TIPO == 4 || _order.ORD_TIPO == 5)
                        {// Pedido de retrabalho

                            if (_order.PlayAction.ToLower() == "insert")
                            {
                                MovimentoEstoqueDevolucao movimentoDevolucao = new MovimentoEstoqueDevolucao
                                {
                                    PRO_ID = _order.PRO_ID,
                                    MOV_QUANTIDADE = _order.ORD_QUANTIDADE,
                                    MOV_LOTE = _order.ORD_ID.Trim() + "." + _order.PRO_ID.Trim(),
                                    MOV_SUB_LOTE = "1",
                                    ORD_ID = _order.ORD_ID,
                                    MOV_ENDERECO = "DEV",
                                    TIP_ID = "400",
                                    PlayAction = "insert"
                                };

                                int index = newList.IndexOf(_order);
                                newList.Insert(index + 1, movimentoDevolucao);
                            }
                            else if (_order.PlayAction.ToLower() == "update")
                            {
                                if (PermitirExcluirMovimento(db, _order))
                                {
                                    MovimentoEstoqueDevolucao movimentoDevolucaoDelete = GetMovimentoEstoqueDevolucao(db, _order);
                                    int index = newList.IndexOf(_order);
                                    if (movimentoDevolucaoDelete != null)
                                    {
                                        index += 1;
                                        movimentoDevolucaoDelete.PlayAction = "delete";
                                        newList.Insert(index, movimentoDevolucaoDelete);
                                    }

                                    MovimentoEstoqueDevolucao movimentoDevolucaoInsert = new MovimentoEstoqueDevolucao
                                    {
                                        PRO_ID = _order.PRO_ID,
                                        MOV_QUANTIDADE = _order.ORD_QUANTIDADE,
                                        MOV_LOTE = _order.ORD_ID.Trim() + "." + _order.PRO_ID.Trim(),
                                        MOV_SUB_LOTE = "1",
                                        ORD_ID = _order.ORD_ID,
                                        MOV_ENDERECO = "DEV",
                                        TIP_ID = "400",
                                        PlayAction = "insert"
                                    };
                                    newList.Insert(index + 1, movimentoDevolucaoInsert);
                                }
                                else
                                {
                                    _order.PlayMsgErroValidacao = string.Format("Não é permitido alterar o pedido ({0}) ele já possui mais de 1 (um) movimento de estoque", _order.ORD_ID);
                                }
                            }
                            else if (_order.PlayAction.ToLower() == "delete")
                            {
                                if (PermitirExcluirMovimento(db, _order))
                                {
                                    int index = newList.IndexOf(_order);

                                    List<FilaProducao> filaProducao = db.FilaProducao.AsNoTracking()
                                                        .Where(x => x.ORD_ID == _order.ORD_ID && x.ROT_PRO_ID == _order.PRO_ID).ToList();

                                    foreach (var fp in filaProducao)
                                    {
                                        fp.PlayAction = "delete";
                                        newList.Insert(index, fp);
                                    }

                                    MovimentoEstoqueDevolucao movimentoDevolucaoDelete = GetMovimentoEstoqueDevolucao(db, _order);
                                    if (movimentoDevolucaoDelete != null)
                                    {
                                        movimentoDevolucaoDelete.PlayAction = "delete";
                                        newList.Insert(index, movimentoDevolucaoDelete);
                                    }
                                }
                                else
                                {
                                    _order.PlayMsgErroValidacao = string.Format("Não é permitido excluir o pedido ({0}) ele já possui mais de 1 (um) movimento de estoque", _order.ORD_ID);
                                }
                            }
                            modo_insert = 3;
                        }

                        if (_order.PlayAction.ToLower() == "insert" || _order.PlayAction.ToLower() == "update")
                        {
                            #region Validação cliente
                            if (string.IsNullOrEmpty(_order.CLI_ID))
                            {
                                _order.PlayMsgErroValidacao = "Por favor, informe o cliente deste pedido";
                                return false;
                            }
                            else
                            {
                                #region Vericacao do codigo do municipio
                                if (string.IsNullOrEmpty(_order.MUN_ID_ENTREGA))
                                {
                                    Cliente cliente = db.Cliente.AsNoTracking()
                                        .Where(c => c.CLI_ID.Equals(_order.CLI_ID))
                                        .Select(c => new Cliente()
                                        {
                                            MUN_ID = c.MUN_ID
                                        }).FirstOrDefault();

                                    _order.MUN_ID_ENTREGA = cliente.MUN_ID;
                                }
                                #endregion Vericacao do codigo do municipio
                            }
                            #endregion Validação cliente

                            #region Validação da suspenção do pedido
                            if (_order.ORD_STATUS == "SS")
                            {
                                List<MovimentoEstoque> apontamentos = db.MovimentoEstoque.AsNoTracking()
                                    .Where(x => x.ORD_ID == _order.ORD_ID && (x.MOV_ESTORNO == null || x.MOV_ESTORNO != "S")).ToList();
                                if (apontamentos.Count > 0)
                                {
                                    _order.PlayMsgErroValidacao = "Não é possível suspender um pedido que já possui apontamentos de produção.";
                                }
                            }
                            else if (_order.ORD_STATUS == "SS" || _order.ORD_STATUS == "SE")
                            {
                                var cargasConsolidadas = (from itensCarga in db.ItenCarga
                                                          join carga in db.Carga
                                                              on itensCarga.CAR_ID equals carga.CAR_ID
                                                          where itensCarga.ORD_ID == _order.ORD_ID && carga.CAR_STATUS == 6
                                                          select new { itensCarga.CAR_ID }
                                            ).ToList();
                                if (cargasConsolidadas.Count > 0)
                                {
                                    _order.PlayMsgErroValidacao = "Não é possível suspender um pedido que já está em uma carga consolidada.";
                                }
                            }

                            #endregion Validação da suspenção do pedido

                            #region Atualizar Prioridade das OPs
                            _order.ORD_DATA_ENTREGA_ATE = _order.ORD_DATA_ENTREGA_DE;

                            List<FilaProducao> OPs = db.FilaProducao.AsNoTracking().Where(fp => fp.ORD_ID == _order.ORD_ID &&
                                                        !fp.FPR_STATUS.StartsWith("E") && fp.FPR_PRIORIDADE != _order.ORD_PRIORIDADE)
                                                        .ToList();
                            foreach (FilaProducao op in OPs)
                            {
                                op.FPR_PRIORIDADE = _order.ORD_PRIORIDADE;
                                op.PlayAction = "update";
                            }
                            newList.AddRange(OPs);
                            #endregion Atualizar Prioridade das OPs
                        }

                        if (_order.PlayAction.ToLower() == "update")
                        {
                            #region Atualizar inicio e fim previsto das OPs

                            object cloneDbOrder = cloneObjeto.GetClone(_order);
                            IEnumerable<string> changedPropertes = cloneObjeto.getChangedPoperties(_order, cloneDbOrder);
                            if (changedPropertes.Contains(nameof(ORD_DATA_ENTREGA_DE)))
                            {
                                /* A data de entrega do pedido foi alterada.
                                 * Verificar se a nova data de entrega está dentro do intervalo do Otimizador.
                                 * Se estiver dentro do intervalo, não precisa atualizar as datas de inicio e fim previsto das OPs.
                                 * Se estiver dentro do intervalo, precisa verificar se não possui movimentos de estoque antes de 
                                 * atualizar as datas de inicio e fim previsto das OPs.
                                 */

                                // Verificar se a nova data de entrega está dentro do intervalo do Otimizador.
                                double otimizaNumDias = db.Param.AsNoTracking().Where(p => p.PAR_ID == "OTIMIZA_NUMERO_DIAS")
                                    .Select(p => p.PAR_VALOR_N).FirstOrDefault();
                                DateTime ultimoDiaOtimizador = DateTime.Now.AddDays(otimizaNumDias);
                                if (_order.ORD_DATA_ENTREGA_DE > ultimoDiaOtimizador)
                                {
                                    // verificar se não possui movimentos de estoque
                                    bool possuiMovEstoque = db.MovimentoEstoque.Any(m => m.ORD_ID == _order.ORD_ID);
                                    if (!possuiMovEstoque)
                                    {
                                        // Precisa atualizar as datas de inicio e fim previsto das OPs
                                        IEnumerable<FilaProducao> ordensProducao = new FilaProducao().AtualizarDatasDasOPs(_order, db);
                                        newList.AddRange(ordensProducao);
                                    }
                                }
                            }

                            if (changedPropertes.Contains(nameof(PRO_ID)))
                            {
                                var possuiOpProduzindo = db.MovimentoEstoque.AsNoTracking()
                                    .Any(m => m.ORD_ID == _order.ORD_ID && (m.TIP_ID == "000" || m.TIP_ID == "001") && m.MOV_ESTORNO != "E");

                                if (possuiOpProduzindo)
                                {
                                    _order.PlayAction = "ERRO";
                                    _order.PlayMsgErroValidacao = "Não é permitido alterar o produto de um pedido que está sendo produzido";
                                    return false;
                                }
                            }

                            #endregion Atualizar inicio e fim previsto das OPs
                        }

                        #region pedido simulado
                        if (_order.PlayAction.ToLower() == "insert" && _order.ORD_STATUS == "S0")
                        {// é um pedido simulado

                            #region calculando m2 unitario do pedido
                            Produto produto = db.Produto.Where(p => p.PRO_ID == _order.PRO_ID)
                            .Select(p => new Produto
                            {
                                PRO_ID_CHAPA = p.PRO_ID_CHAPA
                            }).FirstOrDefault();

                            if (string.IsNullOrEmpty(produto.PRO_ID_CHAPA))
                            {// é pedido de chapa
                                _order.ORD_M2_UNITARIO = new ProdutoChapaIntermediaria().ObterM2Unitario(_order.PRO_ID);
                            }
                            else
                            {// é um pedido de caixa
                                _order.ORD_M2_UNITARIO = new ProdutoCaixa().ObterM2Unitario(_order.PRO_ID);
                            }
                            #endregion calculando m2 unitario do pedido

                            #region explodindo OPs do pedido
                            List<LogPlay> logsExplosaoOPs = new List<LogPlay>();
                            List<object> opsExplodidas = ExplodirOPs(_order, ref logsExplosaoOPs);
                            newList.AddRange(opsExplodidas);
                            #endregion explodindo OPs do pedido
                        }
                        #endregion pedido simulado
                    }
                }

                if (newList.Count > 0)
                {
                    objects.Clear();
                    objects.AddRange(newList);
                }
            }

            return true;
        }

        [HIDDEN]
        public List<Order> GetPedidosEmAberto(string PRO_ID)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                List<Order> pedidosEmAberto = db.Order.AsNoTracking()
                                                .Where(x => x.PRO_ID.Equals(PRO_ID) && !x.ORD_STATUS.StartsWith("E")).ToList();
                return pedidosEmAberto;
            }
        }


        public bool EncerrarPedidosAposInterface(List<object> objects, ref List<LogPlay> Logs)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                // Encerrando os pedidos (status = EI) que não viram na interface 
                List<object> pedidosParaEncerrar = new List<object>();

                List<V_PEDIDOS_PARA_ENCERRAR> v_orders = db.GetPedidosParaEncerrar().Result.ToList();
                v_orders.ForEach(o =>
                {
                    var order = o.ToOrder();
                    order.ORD_STATUS = "EI";
                    order.PlayAction = "update";
                    pedidosParaEncerrar.Add(order);
                });

                if (pedidosParaEncerrar.Count > 0)
                {
                    List<List<object>> ll = new List<List<object>>
                    {
                        pedidosParaEncerrar
                    };

                    List<LogPlay> logsUpdateData = new MasterController().UpdateData(ll, 2, true, db);
                    IEnumerable<LogPlay> logsComErro = logsUpdateData.Where(l => l.Status.Equals("ERRO", StringComparison.OrdinalIgnoreCase));
                    if (logsComErro.Count() > 0)
                    {
                        StringBuilder sb = new StringBuilder("Alguns pedidos não foram encerrados corretamente...");
                        sb.AppendLine("Pedidos que não foram encerrados: ");
                        sb.AppendLine();
                        foreach (LogPlay logErro in logsComErro.Take(50))
                        {
                            sb.Append($"[{logErro.PrimaryKey}];");
                        }

                        Logs.Add(new LogPlay("ERRO", sb.ToString()));
                    }
                    else
                    {
                        Logs.Add(new LogPlay("OK", "Pedidos encerrados com sucesso!"));
                    }
                }
                return true;
            }
        }

        private bool PermitirExcluirMovimento(JSgi db, Order order)
        {
            string lote = order.ORD_ID.Trim() + "." + order.PRO_ID.Trim();
            string subLote = "1";
            string proId = order.PRO_ID;

            if (db == null || String.IsNullOrEmpty(lote) || String.IsNullOrEmpty(subLote) || String.IsNullOrEmpty(proId))
                return false;

            int count = db.MovimentoEstoque.AsNoTracking()
                            .Where(x => x.MOV_LOTE == lote && x.MOV_SUB_LOTE == subLote && x.PRO_ID == proId)
                            .Count();

            if (count <= 1)
                return true;
            return false;
        }

        private MovimentoEstoqueDevolucao GetMovimentoEstoqueDevolucao(JSgi db, Order order)
        {
            string lote = order.ORD_ID.Trim() + "." + order.PRO_ID.Trim();
            string subLote = "1";
            string proId = order.PRO_ID;

            return db.MovimentoEstoqueDevolucao.AsNoTracking()
                    .Where(x => x.MOV_LOTE == lote && x.MOV_SUB_LOTE == subLote && x.PRO_ID == proId)
                    .FirstOrDefault();
        }

        /// <summary>
        /// Retorna true se o produto é composto por um conjunto de produtos
        /// </summary>
        /// <returns></returns>
        public bool IsKit(List<object> objects, ref List<LogPlay> Logs)
        {
            return !String.IsNullOrEmpty(PRO_ID_CONJUNTO);
        }

        public bool ImprimirEtiqueta(List<object> objects, ref List<LogPlay> Logs)
        {
            List<object> ObjetosProcessados = new List<object>();
            List<List<object>> ListObjectsToUpdate = new List<List<object>>();
            MasterController mc = new MasterController();
            //Criando um objeto para a nova carga
            string arrayDeValoresDefault = null;
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                //Para cada item da lista
                foreach (var item in objects)
                {
                    Order _Pedido = (Order)item;
                    var OProducao = db.FilaProducao.Include(f => f.Order).ThenInclude(o => o.Produto).Where(f => f.ORD_ID.Equals(_Pedido.ORD_ID) && f.ROT_PRO_ID.Equals(_Pedido.PRO_ID)).FirstOrDefault();
                    //Validando e extraindo dados para Etiqueta
                    //pedido,produto,seq_rep,maquina,qtd_palete,de ate, num_copias
                    double? qtd_palete = (OProducao.Order.Produto.QTD_PALETE == null || OProducao.Order.Produto.QTD_PALETE == 0) ? (OProducao.Order.Produto.PRO_FARDOS_POR_CAMADA * OProducao.Order.Produto.PRO_CAMADAS_POR_PALETE * OProducao.Order.Produto.PRO_PECAS_POR_FARDO) : OProducao.Order.Produto.QTD_PALETE;
                    var imprimirAte = OProducao.FPR_QTD_PRODUZIDA / qtd_palete;
                    //Formatando dados par Etiqueta

                    arrayDeValoresDefault = "ORD_ID:" + _Pedido.ORD_ID + "," +
                       "ROT_PRO_ID:" + _Pedido.PRO_ID + "," +
                       "FPR_SEQ_REPETICAO:" + OProducao.FPR_SEQ_REPETICAO + "," +
                       "ETI_QUANTIDADE_PALETE:" + qtd_palete + "," +
                       "MAQ_ID:" + OProducao.ROT_MAQ_ID + "," +
                       "ETI_IMPRIMIR_DE:" + 1 + "," +
                       "ETI_IMPRIMIR_ATE:" + imprimirAte + "," +
                       "ETI_NUMERO_COPIAS:" + 2 + "";
                }
            }
            ListObjectsToUpdate.Add(ObjetosProcessados);
            //Concatenando Logs por se tratar de um objeto de interface
            Logs.Add(new LogPlay(this.ToString(), "PROTOCOLO", "_ETIQUETA", "", "" + arrayDeValoresDefault + ""));
            return true;
        }

        public void OrderToInterfaceTelaOrder(List<object> objects)
        {
            List<object> newList = new List<object>();
            foreach (var obj in objects)
            {
                if (obj.GetType().Name != nameof(Order))
                    continue;

                Order order = (Order)obj;
                InterfaceTelaOrder interfaceTelaOrder = new InterfaceTelaOrder();
                interfaceTelaOrder.ORD_ID = order.ORD_ID;
                interfaceTelaOrder.PRO_ID = order.PRO_ID;
                interfaceTelaOrder.ORD_TIPO_FRETE = order.ORD_TIPO_FRETE;
                interfaceTelaOrder.ORD_DATA_ENTREGA_DE = order.ORD_DATA_ENTREGA_DE;
                interfaceTelaOrder.ORD_DATA_ENTREGA_ATE = order.ORD_DATA_ENTREGA_ATE;
                interfaceTelaOrder.ORD_QUANTIDADE = order.ORD_QUANTIDADE;
                interfaceTelaOrder.CLI_ID = order.CLI_ID;
                interfaceTelaOrder.MUN_ID_ENTREGA = order.MUN_ID_ENTREGA;
                interfaceTelaOrder.ORD_OBSERVACAO_OTIMIZADOR = order.ORD_OBSERVACAO_OTIMIZADOR;

                newList.Add(interfaceTelaOrder);
            }
            objects.RemoveRange(0, objects.Count);
            objects.AddRange(newList);
        }

        [HIDDEN]
        public List<object> ExplodirOPs(Order pedido, ref List<LogPlay> logs, JSgi db = null)
        {
            List<object> retorno = new List<object>();
            //List<LogPlay> logs = new List<LogPlay>();
            if (pedido.ORD_STATUS.StartsWith("E"))
            {
                logs.Add(new LogPlay(pedido, "ERRO", $"Não é possível explodir OPs de um pedido: {pedido.ORD_ID} encerrado"));
            }
            else if (pedido.ORD_STATUS.Equals("S1"))
            {
                logs.Add(new LogPlay(pedido, "ERRO", $"Não é possível explodir OPs de um pedido: {pedido.ORD_ID} com status igual a S1"));
            }
            else if (pedido.ORD_TIPO != 1 && pedido.ORD_TIPO != 2)
            {
                logs.Add(new LogPlay(pedido, "ERRO", $"Não é possível explodir OPs que não são de produção. Verifique o tipo deste pedido: {pedido.ORD_ID}"));
            }

            List<object> opsParaCriar = new List<object>();
            List<object> opsParaExcluir = new List<object>();

            if (db == null)
            {
                db = new ContextFactory().CreateDbContext(new string[] { });
            }
            Produto viewCaixa = null;
            HashSet<int> seqTransfRoteiroCaixa = null;
            HashSet<int> seqTransfRoteiroChapa = null;
            List<FilaProducao> ops = null;

            int qtdMovimentos = db.MovimentoEstoque.Count(x => x.ORD_ID.Equals(pedido.ORD_ID));
            if (qtdMovimentos > 0)
            {
                logs.Add(new LogPlay(pedido, "ERRO", $"Não é possível explodir este pedido: {pedido.ORD_ID}, o mesmo já possui movimentos"));
            }

            if (logs.Count > 0)
            {// possui erros de validação
                return retorno;
            }

            ops = db.FilaProducao.AsNoTracking().Where(x => x.ORD_ID.Equals(pedido.ORD_ID)).ToList();

            seqTransfRoteiroCaixa = db.V_ROTEIROS_POSSIVEIS_DO_PRODUTO.AsTracking()
                                                .Where(r => r.PRO_ID.Equals(pedido.PRO_ID))
                                                .Select(r => r.ROT_SEQ_TRANFORMACAO)
                                                .ToHashSet();

            viewCaixa = db.Produto.AsNoTracking().FirstOrDefault(P => P.PRO_ID == pedido.PRO_ID);
            seqTransfRoteiroChapa = db.V_ROTEIROS_POSSIVEIS_DO_PRODUTO.AsTracking()
                                        .Where(r => r.PRO_ID.Equals(viewCaixa.PRO_ID_CHAPA))
                                        .Select(r => r.ROT_SEQ_TRANFORMACAO)
                                        .ToHashSet();

            ops.ForEach(op =>
            {
                op.PlayAction = "delete";
                opsParaExcluir.Add(op);
            });

            foreach (int seqTransf in seqTransfRoteiroCaixa)
            {
                FilaProducao novaOPCaixa = new FilaProducao()
                {
                    ROT_MAQ_ID = "PLAYSIS",
                    ORD_ID = pedido.ORD_ID,
                    ROT_PRO_ID = pedido.PRO_ID,
                    ROT_SEQ_TRANFORMACAO = seqTransf,
                    FPR_SEQ_REPETICAO = 1,
                    FPR_DATA_INICIO_PREVISTA = new DateTime(1970, 01, 01, 00, 00, 00),
                    FPR_DATA_FIM_PREVISTA = new DateTime(1970, 01, 01, 00, 00, 00),
                    FPR_DATA_FIM_MAXIMA = new DateTime(1970, 01, 01, 00, 00, 00),
                    FPR_QUANTIDADE_PREVISTA = pedido.ORD_QUANTIDADE,
                    FPR_OBS_PRODUCAO = string.Empty,
                    FPR_STATUS = string.Empty,
                    PlayAction = "insert"
                };
                opsParaCriar.Add(novaOPCaixa);
            }

            foreach (var seqTransf in seqTransfRoteiroChapa)
            {
                FilaProducao novaOPChapa = new FilaProducao()
                {
                    ROT_MAQ_ID = "PLAYSIS",
                    ORD_ID = pedido.ORD_ID,
                    ROT_PRO_ID = viewCaixa.PRO_ID_CHAPA,
                    ROT_SEQ_TRANFORMACAO = seqTransf,
                    FPR_SEQ_REPETICAO = 1,
                    FPR_DATA_INICIO_PREVISTA = new DateTime(1970, 01, 01, 00, 00, 00),
                    FPR_DATA_FIM_PREVISTA = new DateTime(1970, 01, 01, 00, 00, 00),
                    FPR_DATA_FIM_MAXIMA = new DateTime(1970, 01, 01, 00, 00, 00),
                    FPR_QUANTIDADE_PREVISTA = (pedido.ORD_QUANTIDADE * viewCaixa.QTD_CHAPA.Value) / viewCaixa.BASE_PRODUCAO_CHAPA.Value,
                    FPR_OBS_PRODUCAO = string.Empty,
                    FPR_STATUS = string.Empty,
                    PlayAction = "insert"
                };
                opsParaCriar.Add(novaOPChapa);
            }


            retorno.AddRange(opsParaExcluir);
            retorno.AddRange(opsParaCriar);
            return retorno;
        }
    }

    public class V_PEDIDOS_PARA_ENCERRAR
    {
        public string ORD_ID { get; set; }
        public string CLI_ID { get; set; }
        public string PRO_ID { get; set; }
        public double ORD_QUANTIDADE { get; set; }
        public DateTime ORD_DATA_ENTREGA_DE { get; set; }
        public int? ORD_TIPO { get; set; }
        public string ORD_OP_INTEGRACAO { get; set; }
        public string ORD_PED_CLI { get; set; }
        public string ORD_STATUS { get; set; }
        public string ORD_OBSERVACAO_OTIMIZADOR { get; set; }
        public int ORD_PRIORIDADE { get; set; }
        /* entrega */
        public string ORD_ENDERECO_ENTREGA { get; set; }
        public string ORD_BAIRRO_ENTREGA { get; set; }
        public string UF_ID_ENTREGA { get; set; }
        public string ORD_CEP_ENTREGA { get; set; }
        public string MUN_ID_ENTREGA { get; set; }
        public string ORD_REGIAO_ENTREGA { get; set; }
        /* outros */
        public double? ORD_PRECO_UNITARIO { get; set; }
        public double? ORD_PESO_UNITARIO { get; set; }
        public double? ORD_PESO_UNITARIO_BRUTO { get; set; }
        public double? ORD_M2_UNITARIO { get; set; }
        public DateTime ORD_EMBARQUE_ALVO { get; set; }
        public DateTime ORD_INICIO_JANELA_EMBARQUE { get; set; }
        public DateTime ORD_FIM_JANELA_EMBARQUE { get; set; }
        public DateTime ORD_INICIO_GRUPO_PRODUTIVO { get; set; }
        public DateTime ORD_FIM_GRUPO_PRODUTIVO { get; set; }
        public string CAR_TIPO_CARREGAMENTO { get; set; }
        public string ORD_TIPO_FRETE { get; set; }
        public double? ORD_LARGURA { get; set; }
        public double? ORD_COMPRIMENTO { get; set; }
        public double? ORD_GRAMATURA { get; set; }
        public string GRP_ID { get; set; }
        public string ORD_ID_CONJUNTO { get; set; }
        public string PRO_ID_CONJUNTO { get; set; }
        public string ORD_ID_INTEGRACAO { get; set; }
        public DateTime ORD_DATA_ENTREGA_ATE { get; set; }
        public string ORD_MIT { get; set; }
        public double? ORD_TOLERANCIA_MAIS { get; set; }
        public double? ORD_TOLERANCIA_MENOS { get; set; }
        public string HASH_KEY { get; set; }

        public Order ToOrder()
        {
            Order o = new Order
            {
                ORD_ID = this.ORD_ID,
                CLI_ID = this.CLI_ID,
                PRO_ID = this.PRO_ID,
                ORD_QUANTIDADE = this.ORD_QUANTIDADE,
                ORD_DATA_ENTREGA_DE = this.ORD_DATA_ENTREGA_DE,
                ORD_TIPO = this.ORD_TIPO,
                ORD_OP_INTEGRACAO = this.ORD_OP_INTEGRACAO,
                ORD_PED_CLI = this.ORD_PED_CLI,
                ORD_STATUS = this.ORD_STATUS,
                ORD_OBSERVACAO_OTIMIZADOR = this.ORD_OBSERVACAO_OTIMIZADOR,
                ORD_PRIORIDADE = this.ORD_PRIORIDADE,
                /* entrega */
                ORD_ENDERECO_ENTREGA = this.ORD_ENDERECO_ENTREGA,
                ORD_BAIRRO_ENTREGA = this.ORD_BAIRRO_ENTREGA,
                UF_ID_ENTREGA = this.UF_ID_ENTREGA,
                ORD_CEP_ENTREGA = this.ORD_CEP_ENTREGA,
                MUN_ID_ENTREGA = this.MUN_ID_ENTREGA,
                ORD_REGIAO_ENTREGA = this.ORD_REGIAO_ENTREGA,
                /* outros */
                ORD_PRECO_UNITARIO = this.ORD_PRECO_UNITARIO,
                ORD_PESO_UNITARIO = this.ORD_PESO_UNITARIO,
                ORD_PESO_UNITARIO_BRUTO = this.ORD_PESO_UNITARIO_BRUTO,
                ORD_M2_UNITARIO = this.ORD_M2_UNITARIO,
                ORD_EMBARQUE_ALVO = this.ORD_EMBARQUE_ALVO,
                ORD_INICIO_JANELA_EMBARQUE = this.ORD_INICIO_JANELA_EMBARQUE,
                ORD_FIM_JANELA_EMBARQUE = this.ORD_FIM_JANELA_EMBARQUE,
                ORD_INICIO_GRUPO_PRODUTIVO = this.ORD_INICIO_GRUPO_PRODUTIVO,
                ORD_FIM_GRUPO_PRODUTIVO = this.ORD_FIM_GRUPO_PRODUTIVO,
                CAR_TIPO_CARREGAMENTO = this.CAR_TIPO_CARREGAMENTO,
                ORD_TIPO_FRETE = this.ORD_TIPO_FRETE,
                ORD_LARGURA = this.ORD_LARGURA,
                ORD_COMPRIMENTO = this.ORD_COMPRIMENTO,
                ORD_GRAMATURA = this.ORD_GRAMATURA,
                GRP_ID = this.GRP_ID,
                ORD_ID_CONJUNTO = this.ORD_ID_CONJUNTO,
                PRO_ID_CONJUNTO = this.PRO_ID_CONJUNTO,
                ORD_ID_INTEGRACAO = this.ORD_ID_INTEGRACAO,
                ORD_DATA_ENTREGA_ATE = this.ORD_DATA_ENTREGA_ATE,
                ORD_MIT = this.ORD_MIT,
                ORD_TOLERANCIA_MAIS = this.ORD_TOLERANCIA_MAIS,
                ORD_TOLERANCIA_MENOS = this.ORD_TOLERANCIA_MENOS,
                HASH_KEY = this.HASH_KEY
            };
            return o;
        }
    }

    [Display(Name = "SIMULAR PEDIDO")]
    [ClassMapped(nameOfClassMapped = nameof(Order))]
    public class InterfaceTelaOrder : InterfaceDeTelas
    {
        public InterfaceTelaOrder()
        {
            NamespaceOfClassMapped = typeof(Order).FullName;
        }

        [READ] [TAB(Value = "PRINCIPAL")] [Display(Name = "PEDIDO")] public string ORD_ID { get; set; }
        [SEARCH_NOT_FK(namespaceOfForeignClass = "DynamicForms.Areas.PlugAndPlay.Models.Produto")]
        [Required(ErrorMessage = "Campo PRO_ID requerido.")] [TAB(Value = "PRINCIPAL")] [Display(Name = "PRODUTO")] public string PRO_ID { get; set; }
        [Combobox(Value = "FOB", Description = "FOB")]
        [Combobox(Value = "CIF", Description = "CIF")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FRETE")] [MaxLength(3, ErrorMessage = "Maximode 3 caracteres, campo ORD_TIPO_FRETE")] public string ORD_TIPO_FRETE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ENTREGA DE")] [Required(ErrorMessage = "Campo ORD_DATA_ENTREGA_DE requirido.")] public DateTime ORD_DATA_ENTREGA_DE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ENTREGA ATE")] [Required(ErrorMessage = "Campo ORD_DATA_ENTREGA_ATE requirido.")] public DateTime ORD_DATA_ENTREGA_ATE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE")] [Required(ErrorMessage = "Campo ORD_QUANTIDADE requirido.")] public double ORD_QUANTIDADE { get; set; }
        [SEARCH_NOT_FK(namespaceOfForeignClass = "DynamicForms.Areas.PlugAndPlay.Models.Cliente")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD CLIENTE")] [Required(ErrorMessage = "Campo CLI_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo CLI_ID")] public string CLI_ID { get; set; }
        [SEARCH_NOT_FK(namespaceOfForeignClass = "DynamicForms.Areas.PlugAndPlay.Models.Municipio")]
        [TAB(Value = "PRINCIPAL")] [Required(ErrorMessage = "Preenchimento Obrigatório.")] [Display(Name = "COD MUNICÍPIO")] [MaxLength(50, ErrorMessage = "Maximo de 50 caracteres, campo MUN_ID_ENTREGA")] public string MUN_ID_ENTREGA { get; set; }
        [TextArea] [TAB(Value = "PRINCIPAL")] [Display(Name = "OBSERVAÇÃO DO OTIMIZADOR")] [MaxLength(250, ErrorMessage = "Maximo de 250 caracteres, campo ORD_ID_INTEGRACAO")] public string ORD_OBSERVACAO_OTIMIZADOR { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual Produto Produto { get; set; }
        public virtual Municipio Municipio { get; set; }

        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        [NotMapped] public string NamespaceOfClassMapped { get; set; }

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            List<object> lista_aux = new List<object>();
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (var item in objects)
                {
                    if (item.GetType().Name != nameof(InterfaceTelaOrder))
                        continue;

                    InterfaceTelaOrder interfaceTelaOrder = (InterfaceTelaOrder)item;
                    Order order = InterfaceTelaToOrder(interfaceTelaOrder);
                    order.PlayAction = interfaceTelaOrder.PlayAction;
                    interfaceTelaOrder.PlayAction = "OK";

                    if (order.PlayAction.Equals("insert", StringComparison.OrdinalIgnoreCase))
                    {
                        order.ORD_ID = getNextID();

                    }

                    if (order.PlayAction.Equals("insert", StringComparison.OrdinalIgnoreCase) || order.PlayAction.Equals("update", StringComparison.OrdinalIgnoreCase))
                    {
                        #region Vericacao do codigo do municipio
                        if (!string.IsNullOrEmpty(order.CLI_ID) && string.IsNullOrEmpty(order.MUN_ID_ENTREGA))
                        {
                            Cliente cliente = db.Cliente.AsNoTracking()
                                .Where(c => c.CLI_ID.Equals(order.CLI_ID))
                                .Select(c => new Cliente()
                                {
                                    MUN_ID = c.MUN_ID
                                }).FirstOrDefault();

                            order.MUN_ID_ENTREGA = cliente.MUN_ID;
                        }
                        #endregion Vericacao do codigo do municipio

                        order.ORD_TIPO = 1;
                    }

                    lista_aux.Add(order);
                }

                objects.AddRange(lista_aux);
            }
            return true;
        }

        public string getNextID()
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                var obj = db.Param.Where(x => x.PAR_ID.Equals("ORDER_ID")).FirstOrDefault();
                obj.PAR_VALOR_N++;
                int id = (int)obj.PAR_VALOR_N;

                db.Param.Update(obj);
                db.SaveChanges();


                string next_id = "P";
                next_id += $"{id:D5}";
                return next_id;
            }
        }

        private Order InterfaceTelaToOrder(InterfaceTelaOrder interfaceTelaOrder)
        {
            Order order = new Order();
            order.ORD_ID = interfaceTelaOrder.ORD_ID;
            order.PRO_ID = interfaceTelaOrder.PRO_ID;
            order.ORD_TIPO_FRETE = interfaceTelaOrder.ORD_TIPO_FRETE;
            order.ORD_DATA_ENTREGA_DE = interfaceTelaOrder.ORD_DATA_ENTREGA_DE;
            order.ORD_DATA_ENTREGA_ATE = interfaceTelaOrder.ORD_DATA_ENTREGA_ATE;
            order.ORD_QUANTIDADE = interfaceTelaOrder.ORD_QUANTIDADE;
            order.CLI_ID = interfaceTelaOrder.CLI_ID;
            order.MUN_ID_ENTREGA = interfaceTelaOrder.MUN_ID_ENTREGA;
            order.ORD_OBSERVACAO_OTIMIZADOR = interfaceTelaOrder.ORD_OBSERVACAO_OTIMIZADOR;
            order.ORD_STATUS = "S0";

            return order;
        }
    }



    public class OrderOpt
    {

        public int TIP_ID { get; set; }
        public string CAR_ID { get; set; }
        public double ITC_QTD_PLANEJADA { get; set; }
        public double ITC_QTD_REALIZADA { get; set; }
        public double QTD_TOTAL_PLANEJADO_E_EXPEDIDO { get; set; }
        public double CAR_STATUS { get; set; }
        public DateTime CAR_EMBARQUE_ALVO { get; set; }
        public string CLI_REGIAO_ENTREGA { get; set; }
        public string CLI_MUN_ID_ENTREGA { get; set; }
        public string UF_COD { get; set; }
        public string MUN_NOME { get; set; }
        public string MUN_ID { get; set; }
        public string BairroEntrega { get; set; }
        public string EnderecoEntrega { get; set; }
        public string CEPEntrega { get; set; }
        public string PON_ID_MUN { get; set; }
        public string PON_ID_REG { get; set; }
        public double PON_DISTANCIA_KM_MUN { get; set; }
        public double PON_DISTANCIA_KM_REG { get; set; }
        public string Id { get; set; }
        public string MIT { get; set; }
        public double? M2_Unitario { get; set; }
        public double? Peso_Unitario { get; set; }
        public DateTime FimJanelaEmbarque { get; set; }
        public DateTime InicioJanelaEmbarque { get; set; }
        public DateTime EmbarqueAlvo { get; set; }
        public string ProdutoId { get; set; }
        public string ClienteId { get; set; }
        public string CLI_NOME { get; set; }
        public double? ToleranciaMenos { get; set; }
        public double? ToleranciaMais { get; set; }
        public int? Tipo { get; set; }
        public double? PrecoUnitario { get; set; }
        public double Quantidade { get; set; }
        public DateTime DataEntregaDe { get; set; }
        public DateTime DataEntregaAte { get; set; }
        public double? VIR_QTD_SALDO_A_EXPEDIR { get; set; }
        public double? VIR_QTD_SALDO_UE { get; set; }
        public string PRO_GRUPO_PALETIZACAO { get; set; }
        public int VIR_METODO_CUBAGEM { get; set; }
        public double? VIR_M3_UE { get; set; }
        public double? VIR_M3_UNITARIO { get; set; }
        public double? PRO_CAMADAS_POR_PALETE { get; set; }
        public double? PRO_FARDOS_POR_CAMADA { get; set; }
        public double? PRO_PECAS_POR_FARDO { get; set; }
        public double? PRO_LARGURA_EMBALADA { get; set; }
        public double? PRO_COMPRIMENTO_EMBALADA { get; set; }
        public double? PRO_ALTURA_EMBALADA { get; set; }
        public double? VIR_PECAS_POR_UE { get; set; }
        public double Translado { get; set; }
        public string PRO_FRENTE { get; set; }
        public string PRO_ROTACIONA_COMPRIMENTO { get; set; }
        public string PRO_ROTACIONA_LARGURA { get; set; }
        public string PRO_ROTACIONA_ALTURA { get; set; }
        public double? PRO_TEMPO_DESCARREGAMENTO_UNITARIO { get; set; }
        public double? PRO_TEMPO_CARREGAMENTO_UNITARIO { get; set; }
        public double? CLI_TEMPO_MEDIO_ESPERA_DE_DESCARREGAMENTO { get; set; }
        public double? PRO_PERCENTUAL_JANELA_EMBARQUE { get; set; }
        public double? GrupoEmbarque { get; set; }
        public string TMP_TIPO_CARGA { get; set; }
        public double? TEMPO_CARREGAMENTO { get; set; }
        public double? TEMPO_DESCARREGAMENTO { get; set; }
        public string IDsCargasPotenciaisParaAntecipacoes { get; set; }
        public string HashKey { get; set; }

        public string PON_DESCRICAO_MUN { get; set; }
        public string PON_DESCRICAO_REG { get; set; }
        public string PRO_DESCRICAO { get; set; }
        public string FPR_COR_FILA { get; set; }
        public string VIR_COR_OTIF { get; set; }
        //public string PON_DESTINO { get; set; }
        //public string PON_DESCRICAO { get; set; }
        public double SALDO_ESTOQUE { get; set; }
        public double SALDO_ESTOQUE_UE { get; set; }
        public double PECENT_ESTOQUE_PRONTO { get; set; }
        public string ORD_ID_CARGA { get; set; }

    }
}
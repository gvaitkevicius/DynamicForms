using DynamicForms.Areas.PlugAndPlay.Map;
using DynamicForms.Areas.PlugAndPlay.Map.Estoque;
using DynamicForms.Areas.PlugAndPlay.Map.Qualidade;
using DynamicForms.Areas.PlugAndPlay.Map.Schedule;
using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Areas.PlugAndPlay.Models.Estoque;
using DynamicForms.Areas.SGI.Maps;
using DynamicForms.Areas.SGI.Model;
using DynamicForms.Interfaces;
using DynamicForms.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System.Collections.Generic;
using System.Data.SqlClient;

using System.Linq;
using System.Threading.Tasks;

using static DynamicForms.Interfaces.EstruturaProdutoI;
using static DynamicForms.Interfaces.GrupoProdutoChapaI;
using static DynamicForms.Interfaces.GrupoProdutoPapelI;
using static DynamicForms.Interfaces.MaquinaI;
using static DynamicForms.Interfaces.OrdensI;
using static DynamicForms.Interfaces.ProdutoCaixaI;
using static DynamicForms.Interfaces.ProdutoChapasIntermediariasI;
using static DynamicForms.Interfaces.ProdutoChapaVendaI;
using static DynamicForms.Interfaces.ProdutoPapelI;
using static DynamicForms.Interfaces.RoteirosI;
using static DynamicForms.Interfaces.TintasI;

namespace DynamicForms.Context
{
    public class JSgi : DbContext
    {

        public JSgi()
        {

        }
        public JSgi(DbContextOptions<JSgi> options) : base(options)
        {
            this.Database.SetCommandTimeout(ParametrosSingleton.Instance.TimOut);
        }


        public void DetachAllEntities()
        {
            var changedEntriesCopy = this.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }

        // Utilizado para exibir SQL na janela Saída aba Depurar do Visual Studio
        public static readonly LoggerFactory DbCommandDebugLoggerFactory =
            new LoggerFactory(new[] {
                new DebugLoggerProvider((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
          });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Utilizado para exibir SQL na janela Saída aba Depurar do Visual Studio
            optionsBuilder.UseLoggerFactory(DbCommandDebugLoggerFactory).EnableSensitiveDataLogging();

            base.OnConfiguring(optionsBuilder);
        }


        /* Adicao  das classes Map ao Context usando Entity Framework Core */
        //Fonte: https://rsamorim.azurewebsites.net/2017/11/20/mapeando-suas-entidades-com-entity-framework-core-2-0/
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Database.Log = (query) => Debug.Write("\n\n SQL GERADO \n\n" + query + "\n\n");
            // Remove unused conventions

            modelBuilder.Ignore<FilaDoSchedule>();
            //Fonte: https://entityframeworkcore.com/knowledge-base/46837617/where-are-entity-framework-core-conventions-
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // equivalent of modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
                entityType.Relational().TableName = entityType.DisplayName();

                // equivalent of modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
                // and modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
                entityType.GetForeignKeys()
                    .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
                    .ToList()
                    .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict);
            }
            base.OnModelCreating(modelBuilder);

            #region EstruturaIndicadores
            modelBuilder.ApplyConfiguration(new T_Auditoria_ResultConfiguration());
            modelBuilder.ApplyConfiguration(new T_GrupoMap());
            modelBuilder.ApplyConfiguration(new T_Configuracoes_ResultConfiguration());
            modelBuilder.ApplyConfiguration(new T_DepartamentosMap());
            modelBuilder.ApplyConfiguration(new T_Grupo_Indicador_ResultConfiguration());
            modelBuilder.ApplyConfiguration(new T_IndicadoresMap());
            modelBuilder.ApplyConfiguration(new T_Indicadores_Departamentos_ResultConfiguration());
            modelBuilder.ApplyConfiguration(new DimensaoMap());
            modelBuilder.ApplyConfiguration(new T_Informacoes_Complementares_ResultConfiguration());
            modelBuilder.ApplyConfiguration(new T_MESES_ResultConfiguration());
            modelBuilder.ApplyConfiguration(new T_MetasMap());
            modelBuilder.ApplyConfiguration(new T_NegocioMap());
            modelBuilder.ApplyConfiguration(new T_Perfil_ResultConfiguration());
            modelBuilder.ApplyConfiguration(new T_PlanoAcao_ResultConfiguration());
            modelBuilder.ApplyConfiguration(new T_Tabela_ResultConfiguration());
            modelBuilder.ApplyConfiguration(new T_Unidade_ResultConfiguration());
            modelBuilder.ApplyConfiguration(new T_UNIUSER_ResultConfiguration());
            modelBuilder.ApplyConfiguration(new T_User_GrupoMap());
            modelBuilder.ApplyConfiguration(new T_Usuario_ResultConfiguration());
            #endregion EstruturaIndicadores

            #region EstruturaDre
            // As classes desta regiao EstruturaDre foram excluidas
            //modelBuilder.ApplyConfiguration(new Tr_Movimentos_ResultConfiguration());
            //modelBuilder.ApplyConfiguration(new Tr_PlanoContas_ResultConfiguration());
            //modelBuilder.ApplyConfiguration(new Tr_Unidade_ResultConfiguration());
            //modelBuilder.ApplyConfiguration(new TR_CABVISAO_ResultConfiguration());
            //modelBuilder.ApplyConfiguration(new Tr_Visoes_ResultConfiguration());
            #endregion EstruturaDre

            #region EstruturaPlugAndPlay

            //Schedule
            modelBuilder.ApplyConfiguration(new T_AGENDA_SCHEDULE_MAP());


            //-- Observações
            modelBuilder.ApplyConfiguration(new ObservacoesMap());

            //-- Preferências
            modelBuilder.ApplyConfiguration(new T_PREFERENCIAS_MAP());

            //-- Administrativo
            modelBuilder.ApplyConfiguration(new LogsMap());
            modelBuilder.ApplyConfiguration(new ConsultasMap());
            modelBuilder.ApplyConfiguration(new ConsultasGruposMap());
            //-- Inicial
            modelBuilder.ApplyConfiguration(new ImpressoraMap());
            //--
            modelBuilder.ApplyConfiguration(new CalendarioMap());
            modelBuilder.ApplyConfiguration(new ItensCalendarioMap());
            modelBuilder.ApplyConfiguration(new ColaboradorMap());
            modelBuilder.ApplyConfiguration(new EquipeMap());
            modelBuilder.ApplyConfiguration(new MaquinaMap());
            modelBuilder.ApplyConfiguration(new T_MAQUINAS_EQUIPESMap());
            modelBuilder.ApplyConfiguration(new MaquinaImpressoraMap());
            modelBuilder.ApplyConfiguration(new FeedbackMap());
            // Estoque 
            modelBuilder.ApplyConfiguration(new MovimentoEstoqueAbstrataMap());
            modelBuilder.ApplyConfiguration(new MovimentoEstoqueMap());
            modelBuilder.ApplyConfiguration(new MovimentoEstoqueProducaoMap());
            modelBuilder.ApplyConfiguration(new MovimentoEstoquePerdasMap());
            modelBuilder.ApplyConfiguration(new MovimentoEstoqueTransferenciaSimplesMap());
            modelBuilder.ApplyConfiguration(new MovimentoEstoqueReservaDeEstoqueMap());
            modelBuilder.ApplyConfiguration(new MovimentoEstoqueDevolucaoMap());
            modelBuilder.ApplyConfiguration(new EtiquetaMap());
            modelBuilder.ApplyConfiguration(new ViewEstoquePAMap());
            modelBuilder.ApplyConfiguration(new ViewEstoqueMPMap());
            modelBuilder.ApplyConfiguration(new ViewEstoqueIntermediarioMap());
            modelBuilder.ApplyConfiguration(new ViewPedidosFuturosMap());
            modelBuilder.ApplyConfiguration(new ViewPedidosFuturosPIMap());
            modelBuilder.ApplyConfiguration(new ViewPerdasProducaoMap());
            modelBuilder.ApplyConfiguration(new SaldoEmEstoquePorPedidoMap());
            modelBuilder.ApplyConfiguration(new V_IMPRESSAO_ETIQUETAS_OND_MAP());
            modelBuilder.ApplyConfiguration(new V_INPUT_NFS_MAP());

            modelBuilder.ApplyConfiguration(new T_FeedbackMovEstoqueMap());
            modelBuilder.ApplyConfiguration(new MovimentoEstoqueConsumoMateriaPrimaMap());
            modelBuilder.ApplyConfiguration(new MovimentoEstoqueSaidaInventarioMap());
            modelBuilder.ApplyConfiguration(new MovimentoEstoqueEntradaInventarioMap());
            modelBuilder.ApplyConfiguration(new MovimentoEstoqueReservaDeEstoqueMap());
            modelBuilder.ApplyConfiguration(new MovimentoEstoqueVendasMap());
            modelBuilder.ApplyConfiguration(new SaldosEmEstoquePorLoteMap());
            modelBuilder.ApplyConfiguration(new SaldoPedidoMap());
            modelBuilder.ApplyConfiguration(new V_PEDIDOS_COM_LOTES_DISPONIVEISMap());
            
            modelBuilder.ApplyConfiguration(new V_MOTIVOS_DE_REPROGRAMACAOMap());
            modelBuilder.ApplyConfiguration(new TipoMovimentoEstoqueMap());
            modelBuilder.ApplyConfiguration(new OcorrenciaAbstrataMap());
            modelBuilder.ApplyConfiguration(new TipoOcorrenciaMap());
            modelBuilder.ApplyConfiguration(new OcorrenciaMap());
            modelBuilder.ApplyConfiguration(new OrderMap());
            modelBuilder.ApplyConfiguration(new V_CONSULTA_PEDIDO_MAP());
            modelBuilder.ApplyConfiguration(new OrderOptMap());

            // transporte
            modelBuilder.ApplyConfiguration(new EstruturaEtiquetaMap());
            modelBuilder.ApplyConfiguration(new CargasWebMap());
            modelBuilder.ApplyConfiguration(new CargaMap());
            modelBuilder.ApplyConfiguration(new ItenCargaMap());
            modelBuilder.ApplyConfiguration(new V_ITEM_CARGAMap());
            modelBuilder.ApplyConfiguration(new VeiculoMap());
            modelBuilder.ApplyConfiguration(new TipoVeiculoMap());
            modelBuilder.ApplyConfiguration(new MapaMap());
            modelBuilder.ApplyConfiguration(new PontosMapaMap());
            modelBuilder.ApplyConfiguration(new RotasFactiveisMap());
            modelBuilder.ApplyConfiguration(new RodoviaMap());
            modelBuilder.ApplyConfiguration(new TransportadoraMap());
            modelBuilder.ApplyConfiguration(new Municipios_CoordenadasMap());
            modelBuilder.ApplyConfiguration(new TemposLogisticosMap());
            modelBuilder.ApplyConfiguration(new RestricoesDeRodagemMap());
            modelBuilder.ApplyConfiguration(new CalendarioDisponibilidadeVeiculosMap());
            modelBuilder.ApplyConfiguration(new ItenCalendarioDisponibilidadeVeiculosMap());
            modelBuilder.ApplyConfiguration(new V_ITENS_ROMANEADOSMap());
            modelBuilder.ApplyConfiguration(new PontosEntregaMap());
            modelBuilder.ApplyConfiguration(new V_DISPONIBILIDADE_VEICULOMap());
            modelBuilder.ApplyConfiguration(new V_INPUT_INTEGRACAO_BALANCA_FATURAMENTO_MAP());

            //--QUALIDADE
            modelBuilder.ApplyConfiguration(new TemplateTipoTesteMap());
            modelBuilder.ApplyConfiguration(new TemplateTipoInspecaoVisualMap());
            modelBuilder.ApplyConfiguration(new TipoAvaliacaoMap());
            modelBuilder.ApplyConfiguration(new TipoTesteMap());
            modelBuilder.ApplyConfiguration(new TesteFisicoMap());
            modelBuilder.ApplyConfiguration(new ResultLoteMap());
            modelBuilder.ApplyConfiguration(new TipoInspecaoVisualMap());
            modelBuilder.ApplyConfiguration(new InspecaoVisualMap());
            modelBuilder.ApplyConfiguration(new FechamentoTesteMap());
            modelBuilder.ApplyConfiguration(new LaudoTesteFisicoMap());
            modelBuilder.ApplyConfiguration(new ViewControleQABobinasMap());
            modelBuilder.ApplyConfiguration(new PlanoAmostralTesteMap());

            //--
            //CARGA_MAQUINA
            modelBuilder.ApplyConfiguration(new CargaMaquinaMap());
            modelBuilder.ApplyConfiguration(new ViewCargaMaquinasPedidosMap());
            modelBuilder.ApplyConfiguration(new V_OPS_A_PLANEJAR_MAP());


            // PRODUTO 
            modelBuilder.ApplyConfiguration(new GrupoProdutoMap());
            modelBuilder.ApplyConfiguration(new GrupoProdutoAbstratoMap());
            modelBuilder.ApplyConfiguration(new GrupoProdutoWMSExpedicaoMap());
            modelBuilder.ApplyConfiguration(new GrupoProdutoOutrosMap());
            modelBuilder.ApplyConfiguration(new GrupoProdutoConjuntoMap());
            modelBuilder.ApplyConfiguration(new GrupoProdutoComposicaoMap());
            modelBuilder.ApplyConfiguration(new GrupoProdutoPaleteMap());
            modelBuilder.ApplyConfiguration(new ProdutoMap());
            modelBuilder.ApplyConfiguration(new TipoABNTMap());
            modelBuilder.ApplyConfiguration(new ProdutoCaixaMap());
            modelBuilder.ApplyConfiguration(new ProdutoTintaMap());
            modelBuilder.ApplyConfiguration(new ProdutoAbstratoMap());
            modelBuilder.ApplyConfiguration(new ProdutoClichesMap());
            modelBuilder.ApplyConfiguration(new ProdutoPaleteMap());
            modelBuilder.ApplyConfiguration(new ProdutoFacaMap());
            modelBuilder.ApplyConfiguration(new ProdutoConjuntoMap());
            modelBuilder.ApplyConfiguration(new ProdutoChapaIntermediariaMap());
            modelBuilder.ApplyConfiguration(new ProdutoChapaVendaMap());
            modelBuilder.ApplyConfiguration(new ProdutoPapelMap());
            modelBuilder.ApplyConfiguration(new ProdutoWMSExpedicaoMap());
            modelBuilder.ApplyConfiguration(new EstruturaProdutoMap());
            modelBuilder.ApplyConfiguration(new TemplateDeTestesMap());

            modelBuilder.ApplyConfiguration(new OperacoesMap());
            modelBuilder.ApplyConfiguration(new RoteiroMap());
            modelBuilder.ApplyConfiguration(new V_ROTEIROS_CHAPASMap());
            modelBuilder.ApplyConfiguration(new TargetProdutoMap());
            modelBuilder.ApplyConfiguration(new TurmaMap());
            modelBuilder.ApplyConfiguration(new TurnoMap());
            modelBuilder.ApplyConfiguration(new UnidadeMedidaMap());
            modelBuilder.ApplyConfiguration(new ViewClpMedicoesMap());
            modelBuilder.ApplyConfiguration(new GrupoMaquinaMap());
            modelBuilder.ApplyConfiguration(new EstruturaDoScheduleMap());
            modelBuilder.ApplyConfiguration(new FilaProducaoMap());
            modelBuilder.ApplyConfiguration(new FilaDoScheduleMap());
            modelBuilder.ApplyConfiguration(new CorridasOnduladeiraMap());
            //modelBuilder.ApplyConfiguration(new CorridasOnduladeiraTesteMap());
            modelBuilder.ApplyConfiguration(new V_PEDIDOS_PARCIALMENTE_PRODUZIDOS_MAP());
            modelBuilder.ApplyConfiguration(new V_ROTEIROS_POSSIVEIS_DO_PRODUTO_MAP());
            modelBuilder.ApplyConfiguration(new V_SALDO_PRODUCAO_DE_OPSMap());
            modelBuilder.ApplyConfiguration(new ClpMedicoesMap());
            modelBuilder.ApplyConfiguration(new CorConfiguracaoGraficoMap());
            modelBuilder.ApplyConfiguration(new CustoEntreOpsMap());
            modelBuilder.ApplyConfiguration(new ViewFilaProducaoMap());
            modelBuilder.ApplyConfiguration(new MunicipioMap());

            modelBuilder.ApplyConfiguration(new T_HORARIO_RECEBIMENTO_MAP());
            modelBuilder.ApplyConfiguration(new ClienteMap());
            modelBuilder.ApplyConfiguration(new RepresentantesMap());
            modelBuilder.ApplyConfiguration(new CotasMap());
            modelBuilder.ApplyConfiguration(new ViewFeedbackMap());
            modelBuilder.ApplyConfiguration(new MensagemMap());
            modelBuilder.ApplyConfiguration(new ParamMap());

            modelBuilder.ApplyConfiguration(new T_Usuario_ResultConfiguration());
            modelBuilder.ApplyConfiguration(new T_Usuario_Perfil_ResultConfiguration());
            modelBuilder.ApplyConfiguration(new T_Perfil_ResultConfiguration());
            modelBuilder.ApplyConfiguration(new T_PERFIL_OBJETO_CONTROLAVEL_MAP());
            modelBuilder.ApplyConfiguration(new T_USUARIO_OBJETO_CONTROLAVEL_MAP());
            modelBuilder.ApplyConfiguration(new T_Objeto_Controlavel_ResultConfiguration());
            modelBuilder.ApplyConfiguration(new T_LOGS_DATABASEMap());

            // views
            modelBuilder.ApplyConfiguration(new V_FILA_PRODUCAO_HISTORICOMap());
            modelBuilder.ApplyConfiguration(new V_TARGET_PENDENTESMap());

            //QUERY PARA ACESSO AS viEWS DE iNTERFACE
            //
            modelBuilder.Query<V_INPUT_T_MUNICIPIOS>();
            modelBuilder.Query<V_INPUT_T_CLIENTES>();
            modelBuilder.Query<V_INPUT_T_ORDENS>();
            modelBuilder.Query<V_INPUT_T_OBSERVACOES>();
            modelBuilder.Query<V_INPUT_T_MAQUINAS>();
            modelBuilder.Query<V_INPUT_T_PRODUTO_CAIXAS>();
            modelBuilder.Query<V_INPUT_T_PRODUTO_CHAPAS_INTERMEDIARIAS>();
            modelBuilder.Query<V_INPUT_T_PRODUTO_CHAPAS_VENDAS>();
            modelBuilder.Query<V_INPUT_T_PRODUTO_CONJUNTO>();
            modelBuilder.Query<V_INPUT_T_PRODUTO_PAPEL>();
            modelBuilder.Query<V_INPUT_T_PRODUTO_TINTAS>();
            modelBuilder.Query<V_INPUT_T_ROTEIROS>();
            modelBuilder.Query<V_INPUT_T_ESTRUTURA_PRODUTO>();
            modelBuilder.Query<V_INPUT_T_PRODUTO_FACAS>();
            modelBuilder.Query<V_INPUT_T_PRODUTO_PALETE>();
            modelBuilder.Query<V_INPUT_T_PRODUTO_CLICHES>();
            //--Grupos
            modelBuilder.Query<V_INPUT_T_GRUPO_CHAPA>();
            modelBuilder.Query<V_INPUT_T_GRUPO_FERRAMENTAL>();
            modelBuilder.Query<V_INPUT_T_GRUPO_PRODUTO_TINTA>();
            modelBuilder.Query<V_INPUT_T_GRUPO_PRODUTO_CAIXA>();
            modelBuilder.Query<V_INPUT_T_GRUPO_PALETE>();
            modelBuilder.Query<V_INPUT_T_GRUPO_MAQUINAS>();
            modelBuilder.Query<V_INPUT_T_GRUPO_CONJUNTO>();
            modelBuilder.Query<V_INPUT_T_GRUPO_PRODUTO_PAPEL>();
            //--Pendencias gerais do sistema
            modelBuilder.Query<V_PENDENCIAS_GERAIS>();
            modelBuilder.Query<V_PENDENCIAS_PROGRAMACAO>();
            //--Relatorio
            modelBuilder.ApplyConfiguration(new RelatoriosMap());
            //--Painel do Gestor
            modelBuilder.Query<Areas.PlugAndPlay.Models.V_PAINEL_GESTOR_STATUS_MAQUINAS>();
            modelBuilder.Query<Areas.PlugAndPlay.Models.V_PAINEL_GESTOR_DESEMPENHO_TURNOS_MAQUINA>();
            modelBuilder.Query<Areas.PlugAndPlay.Models.V_PAINEL_GESTOR_DESEMPENHO_TURNOS>();
            modelBuilder.Query<Areas.PlugAndPlay.Models.V_PAINEL_GESTOR_RANKING>();

            // View que rotorna os pedidos para serem encerrados após terminar a interface
            modelBuilder.Query<V_PEDIDOS_PARA_ENCERRAR>();

            //view de disponibilidade do do cliche e faca
            modelBuilder.ApplyConfiguration(new V_DISPONIBILIDADE_CLICHE_MAP());
            modelBuilder.ApplyConfiguration(new V_DISPONIBILIDADE_FACA_MAP());
            
            //--Views de Estoque
            //--Estoque de produto acabado
            //modelBuilder.Query<ViewEstoquePA>();
            //

            #endregion

            #region SGI
            modelBuilder.ApplyConfiguration(new T_FavoritosMap());
            modelBuilder.ApplyConfiguration(new T_MedicoesMap());
            modelBuilder.ApplyConfiguration(new vw_SGI_PARAMETRO_RELMEDICOES_MAP());
            modelBuilder.ApplyConfiguration(new T_GrupoMap());
            modelBuilder.Query<V_PAINEL_LISTA_METAS>();
            #endregion

        }

        // View que rotorna os pedidos para serem encerrados após terminar a interface
        public async Task<List<V_PEDIDOS_PARA_ENCERRAR>> GetPedidosParaEncerrar()
        {
            string sql = @"SELECT * FROM V_PEDIDOS_PARA_ENCERRAR";
            List<V_PEDIDOS_PARA_ENCERRAR> lista = await this.Query<V_PEDIDOS_PARA_ENCERRAR>().FromSql(sql).ToListAsync();
            return lista;
        }

        //Metodo para View de estoque de produto acabado
        public async Task<List<ViewEstoquePA>> GetEstoquePA()
        {
            string sql = @"SELECT * FROM V_ESTOQUE_PA";
            List<ViewEstoquePA> lista = await this.Query<ViewEstoquePA>().FromSql(sql).ToListAsync();
            return lista;
        }
        //Metodo para View de Pendencias gerais de cadastros e importaçoes pendentes
        public async Task<List<V_PENDENCIAS_GERAIS>> GetPendenciasGerais()
        {
            string sql = @"SELECT TOP 50 * FROM V_PENDENCIAS_GERAIS";
            List<V_PENDENCIAS_GERAIS> lista = await this.Query<V_PENDENCIAS_GERAIS>().FromSql(sql).ToListAsync();
            return lista;
        }
        //Metodos de interface
        public async Task<List<V_INPUT_T_MUNICIPIOS>> GetMunicipiosInterface()
        {
            string sql = @"SELECT * FROM V_INPUT_T_MUNICIPIOS WHERE ACTION <> 'OK'";
            List<V_INPUT_T_MUNICIPIOS> lista = await this.Query<V_INPUT_T_MUNICIPIOS>().FromSql(sql).ToListAsync();
            return lista;
        }
        public async Task<List<V_INPUT_T_GRUPO_PRODUTO_PAPEL>> GetGrupoProdutoPapelInterface()
        {
            string sql = @"SELECT * FROM V_INPUT_T_GRUPO_PRODUTO_PAPEL WHERE ACTION <> 'OK'";
            List<V_INPUT_T_GRUPO_PRODUTO_PAPEL> lista = await this.Query<V_INPUT_T_GRUPO_PRODUTO_PAPEL>().FromSql(sql).ToListAsync();
            return lista;
        }
        public async Task<List<V_INPUT_T_CLIENTES>> GetClientesInterface()
        {
            string sql = @"SELECT * FROM V_INPUT_T_CLIENTES WHERE ACTION <> 'OK'  ";
            List<V_INPUT_T_CLIENTES> lista = await this.Query<V_INPUT_T_CLIENTES>().FromSql(sql).ToListAsync();
            return lista;
        }
        public async Task<List<V_INPUT_T_GRUPO_CONJUNTO>> GetGrupoConjuntoInterface()
        {
            string sql = @"SELECT * FROM V_INPUT_T_GRUPO_CONJUNTO WHERE ACTION <> 'OK'";
            List<V_INPUT_T_GRUPO_CONJUNTO> lista = await this.Query<V_INPUT_T_GRUPO_CONJUNTO>().FromSql(sql).ToListAsync();
            return lista;
        }
        public async Task<List<V_INPUT_T_GRUPO_MAQUINAS>> GetGrupoMAquinaInterface()
        {
            string sql = @"SELECT * FROM  V_INPUT_T_GRUPO_MAQUINAS WHERE ACTION <> 'OK'";
            List<V_INPUT_T_GRUPO_MAQUINAS> lista = await this.Query<V_INPUT_T_GRUPO_MAQUINAS>().FromSql(sql).ToListAsync();
            return lista;
        }
        public async Task<List<V_INPUT_T_GRUPO_PALETE>> GetGrupoProdutoPaleteInterface()
        {
            string sql = @"SELECT * FROM V_INPUT_T_GRUPO_PALETE WHERE ACTION <> 'OK'";
            List<V_INPUT_T_GRUPO_PALETE> lista = await this.Query<V_INPUT_T_GRUPO_PALETE>().FromSql(sql).ToListAsync();
            return lista;
        }
        public async Task<List<V_INPUT_T_GRUPO_PRODUTO_CAIXA>> GetGrupoProdutoCaixaInterface()
        {
            string sql = @"SELECT * FROM V_INPUT_T_GRUPO_PRODUTO_CAIXA WHERE ACTION <> 'OK'";
            List<V_INPUT_T_GRUPO_PRODUTO_CAIXA> lista = await this.Query<V_INPUT_T_GRUPO_PRODUTO_CAIXA>().FromSql(sql).ToListAsync();
            return lista;
        }
        public async Task<List<V_INPUT_T_GRUPO_PRODUTO_TINTA>> GetGrupoTintaInterface()
        {
            string sql = @"SELECT * FROM V_INPUT_T_GRUPO_PRODUTO_TINTA WHERE ACTION <> 'OK' ";
            List<V_INPUT_T_GRUPO_PRODUTO_TINTA> lista = await this.Query<V_INPUT_T_GRUPO_PRODUTO_TINTA>().FromSql(sql).ToListAsync();
            return lista;
            //

        }
        public async Task<List<V_INPUT_T_GRUPO_FERRAMENTAL>> GetGrupoFerramentalInterface()
        {
            string sql = @"SELECT * FROM V_INPUT_T_GRUPO_FERRAMENTAL WHERE ACTION <> 'OK'";
            List<V_INPUT_T_GRUPO_FERRAMENTAL> lista = await this.Query<V_INPUT_T_GRUPO_FERRAMENTAL>().FromSql(sql).ToListAsync();
            return lista;
        }
        public async Task<List<V_INPUT_T_GRUPO_CHAPA>> GetGrupoProdutoChapaInterface()
        {
            string sql = @"SELECT * FROM V_INPUT_T_GRUPO_CHAPA WHERE ACTION <> 'OK'";
            List<V_INPUT_T_GRUPO_CHAPA> lista = await this.Query<V_INPUT_T_GRUPO_CHAPA>().FromSql(sql).ToListAsync();
            return lista;
        }
        public async Task<List<V_INPUT_T_ESTRUTURA_PRODUTO>> GetEstruturasProdutoInterface()
        {
            string sql = @"SELECT * FROM V_INPUT_T_ESTRUTURA_PRODUTO WHERE ACTION <> 'OK'    ";
            List<V_INPUT_T_ESTRUTURA_PRODUTO> lista = await this.Query<V_INPUT_T_ESTRUTURA_PRODUTO>().FromSql(sql).ToListAsync();
            return lista;
        }
        public async Task<List<V_INPUT_T_ROTEIROS>> GetRoteirosInterface()
        {
            string sql = @"SELECT * FROM V_INPUT_T_ROTEIROS  WHERE ACTION <> 'OK'    ";
            List<V_INPUT_T_ROTEIROS> lista = await this.Query<V_INPUT_T_ROTEIROS>().FromSql(sql).ToListAsync();
            return lista;
        }
        public async Task<List<V_INPUT_T_PRODUTO_TINTAS>> GetProdutoTintalInterface()
        {
            string sql = @"SELECT * FROM V_INPUT_T_PRODUTO_TINTAS WHERE ACTION <> 'OK'";
            List<V_INPUT_T_PRODUTO_TINTAS> lista = await this.Query<V_INPUT_T_PRODUTO_TINTAS>().FromSql(sql).ToListAsync();
            return lista;
        }
        public async Task<List<V_INPUT_T_PRODUTO_PAPEL>> GetProdutoPapelInterface()
        {
            string sql = @"SELECT * FROM V_INPUT_T_PRODUTO_PAPEL WHERE ACTION <> 'OK'";
            List<V_INPUT_T_PRODUTO_PAPEL> lista = await this.Query<V_INPUT_T_PRODUTO_PAPEL>().FromSql(sql).ToListAsync();
            return lista;
        }
        public async Task<List<V_INPUT_T_PRODUTO_CONJUNTO>> GetProdutoConjuntoInterface()
        {
            string sql = @"SELECT * FROM V_INPUT_T_PRODUTO_CONJUNTO WHERE ACTION <> 'OK'";
            List<V_INPUT_T_PRODUTO_CONJUNTO> lista = await this.Query<V_INPUT_T_PRODUTO_CONJUNTO>().FromSql(sql).ToListAsync();
            return lista;
        }
        public async Task<List<V_INPUT_T_PRODUTO_CHAPAS_VENDAS>> GetProdutoChapasVendaInterface()
        {
            string sql = @"SELECT * FROM V_INPUT_T_PRODUTO_CHAPAS_VENDAS WHERE ACTION <> 'OK'";
            List<V_INPUT_T_PRODUTO_CHAPAS_VENDAS> lista = await this.Query<V_INPUT_T_PRODUTO_CHAPAS_VENDAS>().FromSql(sql).ToListAsync();
            return lista;
        }
        public async Task<List<V_INPUT_T_PRODUTO_CAIXAS>> GetCaixasInterface()
        {
            string sql = @"SELECT * FROM V_INPUT_T_PRODUTO_CAIXAS WHERE ACTION <> 'OK'   ";
            List<V_INPUT_T_PRODUTO_CAIXAS> lista = await this.Query<V_INPUT_T_PRODUTO_CAIXAS>().FromSql(sql).ToListAsync();
            return lista;
        }
        public async Task<List<V_INPUT_T_MAQUINAS>> GetMaquinasInterface()
        {
            string sql = @"SELECT * FROM V_INPUT_T_MAQUINAS WHERE ACTION <> 'OK'";
            List<V_INPUT_T_MAQUINAS> lista = await this.Query<V_INPUT_T_MAQUINAS>().FromSql(sql).ToListAsync();
            return lista;
        }
        public async Task<List<V_INPUT_T_ORDENS>> GetOrdensInterface()
        {
            //string sql = @"SELECT * FROM V_INPUT_T_ORDENS WHERE ACTION <> 'OK' and ord_id in('77305104','77305105','77289805','77315001') ";
            string sql = @"SELECT * FROM V_INPUT_T_ORDENS WHERE ACTION <> 'OK' ";
            List<V_INPUT_T_ORDENS> lista = await this.Query<V_INPUT_T_ORDENS>().FromSql(sql).ToListAsync();
            return lista;
        }
        public async Task<List<V_INPUT_T_OBSERVACOES>> GetObservacoesInterface()
        {
            string sql = @"SELECT * FROM V_INPUT_T_OBSERVACOES WHERE ACTION <> 'OK'";
            List<V_INPUT_T_OBSERVACOES> lista = await this.Query<V_INPUT_T_OBSERVACOES>().FromSql(sql).ToListAsync();
            return lista;
        }
        public async Task<List<V_INPUT_T_PRODUTO_CHAPAS_INTERMEDIARIAS>> GetProdutoChapasIntermediariasInterface()
        {
            string sql = @"SELECT * FROM V_INPUT_T_PRODUTO_CHAPAS_INTERMEDIARIAS WHERE ACTION <> 'OK'";
            List<V_INPUT_T_PRODUTO_CHAPAS_INTERMEDIARIAS> lista = await this.Query<V_INPUT_T_PRODUTO_CHAPAS_INTERMEDIARIAS>().FromSql(sql).ToListAsync();
            return lista;
        }
        public async Task<List<V_INPUT_T_PRODUTO_FACAS>> GetProdutoFacasInterface()
        {
            string sql = @"SELECT * FROM V_INPUT_T_PRODUTO_FACAS WHERE ACTION <> 'OK'";
            List<V_INPUT_T_PRODUTO_FACAS> lista = await this.Query<V_INPUT_T_PRODUTO_FACAS>().FromSql(sql).ToListAsync();
            return lista;
        }
        public async Task<List<V_INPUT_T_PRODUTO_PALETE>> GetProdutoPaleteInterface()
        {
            string sql = @"SELECT * FROM V_INPUT_T_PRODUTO_PALETE WHERE ACTION <> 'OK'";
            List<V_INPUT_T_PRODUTO_PALETE> lista = await this.Query<V_INPUT_T_PRODUTO_PALETE>().FromSql(sql).ToListAsync();
            return lista;
        }
        public async Task<List<V_INPUT_T_PRODUTO_CLICHES>> GetProdutoClicheInterface()
        {
            string sql = @"SELECT * FROM V_INPUT_T_PRODUTO_CLICHES WHERE ACTION <> 'OK'";
            List<V_INPUT_T_PRODUTO_CLICHES> lista = await this.Query<V_INPUT_T_PRODUTO_CLICHES>().FromSql(sql).ToListAsync();
            return lista;
        }

        #region Indiadores
        public DbSet<T_Auditoria> T_Auditoria { get; set; }
        public DbSet<T_Configuracoes> T_Configuracoes { get; set; }
        public DbSet<T_Grupo> T_Grupo { get; set; }
        public DbSet<T_Departamentos> T_Departamentos { get; set; }
        public DbSet<T_Grupo_Indicador> T_Grupo_Indicador { get; set; }
        public DbSet<T_Indicadores> T_Indicadores { get; set; }
        public DbSet<T_Indicadores_Departamentos> T_Indicadores_Departamentos { get; set; }
        public DbSet<Dimensao> Dimensao { get; set; }
        public DbSet<T_Informacoes_Complementares> T_Informacoes_Complementares { get; set; }
        public DbSet<T_MESES> T_MESES { get; set; }
        public DbSet<T_Metas> T_Metas { get; set; }
        public DbSet<T_Negocio> T_Negocio { get; set; }
        public DbSet<T_PlanoAcao> T_PlanoAcao { get; set; }
        public DbSet<T_Tabela> T_Tabela { get; set; }
        public DbSet<T_Unidade> T_Unidade { get; set; }
        public DbSet<T_UNIUSER> T_UNIUSER { get; set; }
        public DbSet<T_USER_GRUPO> T_USER_GRUPO { get; set; }
        public DbSet<T_Objeto_Controlavel> T_Objeto_Controlavel { get; set; }
        public DbSet<T_Perfil_Objeto_Controlavel> T_PERFIL_OBJETO_CONTROLAVEL { get; set; }
        public DbSet<T_USUARIO_OBJETO_CONTROLAVEL> T_USUARIO_OBJETO_CONTROLAVEL { get; set; }
        public DbSet<T_Perfil> T_Perfil { get; set; }
        public DbSet<T_Usuario_Perfil> T_Usuario_Perfil { get; set; }
        public DbSet<T_Usuario> T_Usuario { get; set; }
        #endregion Indiadores

        #region DRE
        // As classes desta regiao DRE foram excluidas
        //public DbSet<Tr_PlanoContas> Tr_PlanoContas { get; set; }
        //public DbSet<Tr_Unidade> Tr_Unidade { get; set; }
        //public DbSet<Tr_Movimentos> Tr_Movimentos { get; set; }
        //public DbSet<Tr_CabViscao> Tr_CabViscao { get; set; }
        //public DbSet<Tr_Visoes> Tr_Visoes { get; set; }
        #endregion DRE

        #region PlugAndPlay

        public DbSet<T_AGENDA_SCHEDULE> T_AGENDA_SCHEDULE { get; set; }
        public DbSet<T_PREFERENCIAS> T_PREFERENCIAS { get; set; }
        public DbSet<Observacoes> Observacoes { get; set; }
        public DbSet<Calendario> Calendario { get; set; }
        public DbSet<ItensCalendario> ItensCalendario { get; set; }
        public DbSet<Colaborador> Colaborador { get; set; }
        public DbSet<EstruturaProduto> EstruturaProduto { get; set; }
        public DbSet<Equipe> Equipe { get; set; }
        public DbSet<Maquina> Maquina { get; set; }
        public DbSet<T_MAQUINAS_EQUIPES> T_MAQUINAS_EQUIPES { get; set; }
        public DbSet<MaquinaImpressora> MaquinaImpressora { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        // estoque 
        public DbSet<MovimentoEstoque> MovimentoEstoque { get; set; }
        public DbSet<ViewEstoquePA> ViewEstoquePA { get; set; }
        public DbSet<ViewEstoqueMP> ViewEstoqueMP { get; set; }
        public DbSet<ViewEstoqueIntermediario> ViewEstoqueIntermediario { get; set; }
        public DbSet<ViewPedidosFuturos> ViewPedidosFuturos { get; set; }
        public DbSet<ViewPedidosFuturosPI> ViewPedidosFuturosPI { get; set; }
        public DbSet<MovimentoEstoqueAbstrata> MovimentoEstoqueAbstrata { get; set; }
        public DbSet<MovimentoEstoqueProducao> MovimentoEstoqueProducao { get; set; }
        public DbSet<MovimentoEstoquePerdas> MovimentoEstoquePerdas { get; set; }
        public DbSet<MovimentoEstoqueTransferenciaSimples> MovimentoEstoqueTransferenciaSimples { get; set; }
        public DbSet<MovimentoEstoqueConsumoMateriaPrima> MovimentoEstoqueConsumoMateriaPrima { get; set; }
        public DbSet<MovimentoEstoqueEntradaInventario> MovimentoEstoqueEntradaInventario { get; set; }
        public DbSet<MovimentoEstoqueSaidaInventario> MovimentoEstoqueSaidaInventario { get; set; }
        public DbSet<MovimentoEstoqueReservaDeEstoque> MovimentoEstoqueReservaDeEstoque { get; set; }
        public DbSet<MovimentoEstoqueDevolucao> MovimentoEstoqueDevolucao { get; set; }
        public DbSet<MovimentoEstoqueVendas> MovimentoEstoqueVendas { get; set; }
        public DbSet<SaldosEmEstoquePorLote> SaldosEmEstoquePorLote { get; set; }
        public DbSet<SaldoPedido> SaldoPedido { get; set; }
        public DbSet<V_PEDIDOS_COM_LOTES_DISPONIVEIS> V_PEDIDOS_COM_LOTES_DISPONIVEIS { get; set; }
        public DbSet<V_INPUT_NFS> V_INPUT_NFS { get; set; }


        public DbSet<T_FeedbackMovEstoque> T_FeedbackMovEstoque { get; set; }
        public DbSet<ViewPerdasProducao> ViewPerdasProducao { get; set; }
        public DbSet<V_MOTIVOS_DE_REPROGRAMACAO> V_MOTIVOS_DE_REPROGRAMACAO { get; set; }
        public DbSet<SaldoEmEstoquePorPedido> SaldoEmEstoquePorPedido { get; set; }

        public DbSet<Etiqueta> Etiqueta { get; set; }

        public DbSet<TipoMovimentoEstoque> TipoMovimentoEstoque { get; set; }
        public DbSet<TipoMovEntradaProducao> TipoMovEntradaProducao { get; set; }
        public DbSet<TipoMovEntradaInventario> TipoMovEntradaInventario { get; set; }
        public DbSet<TipoMovEntradaCompras> TipoMovEntradaCompras { get; set; }
        public DbSet<TipoMovEntradaDevolucoes> TipoMovEntradaDevolucoes { get; set; }
        public DbSet<TipoMovEntredaTransferenciaInterna> TipoMovEntredaTransferenciaInterna { get; set; }
        public DbSet<TipoMovSaidaInventario> TipoMovSaidaInventario { get; set; }
        public DbSet<TipoMovTransferenciaSimples> TipoMovTransferenciaSimples { get; set; }
        public DbSet<TipoMovSaidaTransferenciaInterna> TipoMovSaidaTransferenciaInterna { get; set; }
        public DbSet<TipoMovSaidaVendas> TipoMovSaidaVendas { get; set; }
        public DbSet<TipoMovSaidaPerdas> TipoMovSaidaPerdas { get; set; }
        public DbSet<TipoMovSaidaConsumo> TipoMovSaidaConsumo { get; set; }
        public DbSet<TipoMovRetencao> TipoMovRetencao { get; set; }

        public DbSet<Ocorrencia> Ocorrencia { get; set; }
        public DbSet<OcorrenciaAbstrata> OcorrenciaAbstrata { get; set; }
        public DbSet<OcorrenciaMotivosDasPerdas> OcorrenciaMotivosDasPerdas { get; set; }
        public DbSet<OcorrenciaMotivosDeParadas> OcorrenciaMotivosDeParadas { get; set; }
        public DbSet<OcorrenciaPularOrdemFila> OcorrenciaPularOrdemFila { get; set; }

        public DbSet<V_CONSULTA_PEDIDO> V_CONSULTA_PEDIDO { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderOpt> OrderOpt { get; set; }
        public DbSet<GrupoProduto> GrupoProduto { get; set; }
        public DbSet<GrupoProdutoAbstrato> GrupoProdutoAbstrato { get; set; }
        public DbSet<GrupoProdutoWMSExpedicao> GrupoProdutoWMSExpedicao { get; set; }
        public DbSet<GrupoProdutoOutros> GrupoProdutoOutros { get; set; }
        public DbSet<GrupoProdutoConjunto> GrupoProdutoConjunto { get; set; }
        public DbSet<GrupoProdutoComposicao> GrupoProdutoComposicao { get; set; }
        public DbSet<GrupoProdutoPalete> GrupoProdutoPalete { get; set; }
        public DbSet<Produto> Produto { get; set; }
        public DbSet<TipoABNT> TipoABNT { get; set; }
        public DbSet<ProdutoAbstrato> ProdutoAbstrato { get; set; }
        public DbSet<ProdutoCliches> ProdutoCliches { get; set; }
        public DbSet<ProdutoPalete> ProdutoPalete { get; set; }
        public DbSet<ProdutoFaca> ProdutoFaca { get; set; }
        public DbSet<ProdutoConjunto> ProdutoConjunto { get; set; }
        public DbSet<ProdutoCaixa> ProdutoCaixa { get; set; }
        public DbSet<ProdutoTinta> ProdutoTinta { get; set; }
        public DbSet<ProdutoChapaIntermediaria> ProdutoChapaIntermediaria { get; set; }
        public DbSet<ProdutoChapaVenda> ProdutoChapaVenda { get; set; }
        public DbSet<ProdutoPapel> ProdutoPapel { get; set; }
        public DbSet<ProdutoWMSExpedicao> ProdutoWMSExpedicao { get; set; }

        public DbSet<Roteiro> Roteiro { get; set; }
        public DbSet<V_ROTEIROS_CHAPAS> V_ROTEIROS_CHAPAS { get; set; }
        public DbSet<TargetProduto> TargetProduto { get; set; }
        public DbSet<TipoOcorrencia> TipoOcorrencia { get; set; }
        public DbSet<Turma> Turma { get; set; }
        public DbSet<Turno> Turno { get; set; }
        public DbSet<UnidadeMedida> UnidadeMedida { get; set; }
        public DbSet<ViewClpMedicoes> ViewClpMedicoes { get; set; }
        public DbSet<GrupoMaquina> GrupoMaquina { get; set; }
        public DbSet<FilaProducao> FilaProducao { get; set; }
        public DbSet<FilaDoSchedule> FilaDoSchedule { get; set; }
        public DbSet<EstruturaDoSchedule> EstruturaDoSchedule { get; set; }

        public DbSet<CorridasOnduladeira> CorridasOnduladeira { get; set; }
        //public DbSet<CorridasOnduladeiraTeste> CorridasOnduladeiraTeste { get; set; }
        public DbSet<V_PEDIDOS_PARCIALMENTE_PRODUZIDOS> V_PEDIDOS_PARCIALMENTE_PRODUZIDOS { get; set; }
        public DbSet<V_ROTEIROS_POSSIVEIS_DO_PRODUTO> V_ROTEIROS_POSSIVEIS_DO_PRODUTO { get; set; }
        public DbSet<V_SALDO_PRODUCAO_DE_OPS> V_SALDO_PRODUCAO_DE_OPS { get; set; }
        public DbSet<V_DISPONIBILIDADE_CLICHE> V_DISPONIBILIDADE_CLICHE { get; set; }
        public DbSet<V_DISPONIBILIDADE_FACA> V_DISPONIBILIDADE_FACA { get; set; }
        public DbSet<V_IMPRESSAO_ETIQUETAS_OND> V_IMPRESSAO_ETIQUETAS_OND { get; set; }
        public DbSet<ClpMedicoes> ClpMedicoes { get; set; }
        public DbSet<CorConfiguracaoGrafico> CorConfiguracaoGrafico { get; set; }
        public DbSet<CustoEntreOps> CustoEntreOps { get; set; }
        public DbSet<ViewFilaProducao> ViewFilaProducao { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Representantes> Representantes { get; set; }
        public DbSet<Cotas> Cotas { get; set; }
        public DbSet<Operacoes> Operacoes { get; set; }
        public DbSet<T_HORARIO_RECEBIMENTO> T_HORARIO_RECEBIMENTO { get; set; }


        // transporte

        public DbSet<EstruturaEtiqueta> EstruturaEtiqueta { get; set; }
        public DbSet<CargasWeb> CargasWeb { get; set; }

        public DbSet<Carga> Carga { get; set; }
        public DbSet<ItenCarga> ItenCarga { get; set; }
        public DbSet<V_ITEM_CARGA> V_ITEM_CARGA { get; set; }
        public DbSet<V_ITENS_ROMANEADOS> V_ITENS_ROMANEADOS { get; set; }
        public DbSet<Veiculo> Veiculo { get; set; }
        public DbSet<TipoVeiculo> TipoVeiculo { get; set; }
        public DbSet<Mapa> Mapa { get; set; }
        public DbSet<PontosMapa> PontosMapa { get; set; }
        public DbSet<RotaPontosMapa> RotaPontosMapa { get; set; }
        public DbSet<Rodovias> Rodovias { get; set; }
        public DbSet<Transportadora> Transportadora { get; set; }
        public DbSet<Municipios_Coordenadas> Municipios_Coordenadas { get; set; }
        public DbSet<TemposLogisticos> TemposLogisticos { get; set; }
        public DbSet<RestricoesDeRodagem> RestricoesDeRodagem { get; set; }
        public DbSet<CalendarioDisponibilidadeVeiculos> CalendarioDisponibilidadeVeiculos { get; set; }
        public DbSet<ItenCalendarioDisponibilidadeVeiculos> ItenCalendarioDisponibilidadeVeiculos { get; set; }
        public DbSet<PontosEntrega> PontosEntrega { get; set; }
        public DbSet<V_DISPONIBILIDADE_VEICULO> V_DISPONIBILIDADE_VEICULO { get; set; }
        public DbSet<V_INPUT_INTEGRACAO_BALANCA_FATURAMENTO> V_INPUT_INTEGRACAO_BALANCA_FATURAMENTO { get; set; }


        // views
        public DbSet<ViewFeedback> ViewFeedback { get; set; }
        public DbSet<V_FILA_PRODUCAO_HISTORICO> V_FILA_PRODUCAO_HISTORICO { get; set; }
        public DbSet<V_TARGET_PENDENTES> V_TARGET_PENDENTES { get; set; }
        //-- Qualidade

        public DbSet<TipoAvaliacao> TipoAvaliacao { get; set; }
        public DbSet<TipoTeste> TipoTeste { get; set; }
        public DbSet<TesteFisico> TesteFisico { get; set; }
        public DbSet<TemplateTipoTeste> TemplateTipoTeste { get; set; }
        public DbSet<ResultLote> ResultLote { get; set; }
        public DbSet<InspecaoVisual> InspecaoVisual { get; set; }
        public DbSet<TipoInspecaoVisual> TipoInspecaoVisual { get; set; }
        public DbSet<TemplateTipoInspecaoVisual> TemplateTipoInspecaoVisual { get; set; }
        public DbSet<LaudoTesteFisico> LaudoTesteFisico { get; set; }
        public DbSet<FechamentoTeste> FechamentoTeste { get; set; }
        public DbSet<PlanoAmostralTeste> PlanoAmostralTeste { get; set; }
        public DbSet<ViewControleQABobinas> ViewControleQABobinas { get; set; }

        //--
        //Carga Maquina
        public DbSet<CargaMaquina> CargaMaquina { get; set; }
        public DbSet<ViewCargaMaquinasPedidos> ViewCargaMaquinasPedidos { get; set; }
        public DbSet<V_OPS_A_PLANEJAR> V_OPS_A_PLANEJAR { get; set; }

        //public DbSet<GrupoProdutivo> GrupoProdutivo { get; set; }
        //public DbSet<GruposProdutivosExpedicao> GruposProdutivosExpedicao { get; set; }
        //Relatorio
        public DbSet<Relatorios> Relatorios { get; set; }
        //public DbSet<V_MOTIVO_FEEDBACKS_DESEMPENHO> V_MOTIVOS_FEEDBACKS { get; set; } PENDENCIA AJUSTAR
        public DbSet<Mensagem> Mensagem { get; set; }
        public DbSet<Param> Param { get; set; } /* Parametros */
        public DbSet<Municipio> Municipio { get; set; }
        public DbSet<TemplateDeTestes> TemplateDeTestes { get; set; }
        //Inicial
        public DbSet<Impressora> Impressora { get; set; }
        //Administrativo
        public DbSet<Logs> Logs { get; set; }
        public DbSet<Consultas> Consultas { get; set; }
        public DbSet<ConsultasGrupos> ConsultasGrupos { get; set; }
        #endregion

        #region SGI
        public DbSet<T_Favoritos> T_favoritos { get; set; }
        public DbSet<T_Medicoes> T_Medicoes { get; set; }
        public DbSet<vw_SGI_PARAMETRO_RELMEDICOES> VW_SGI_PARAMETRO_RELMEDICOES { get; set; }
        #endregion

        public DbSet<T_LOGS_DATABASE> T_LOGS_DATABASE { get; set; }
        public IEnumerable<object> Cargas { get; internal set; }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                foreach (var eve in e.Entries)
                {
                    System.Diagnostics.Debug.WriteLine("Entidade do tipo \"{0}\" no estado \"{1}\" tem os seguintes erros de validação:",
                        eve.Entity.GetType().Name, eve.State);
                }
                throw;
            }
            catch (SqlException s)
            {
                System.Diagnostics.Debug.WriteLine("- Message: \"{0}\", Data: \"{1}\"", s.Message, s.Data);
                throw;
            }
        }
    }
}
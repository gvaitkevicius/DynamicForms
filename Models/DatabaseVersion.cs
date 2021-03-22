using System.Collections.Generic;

namespace DynamicForms.Models
{
    public class DatabaseVersion
    {
        public List<T_Objeto_Controlavel> ObjetosControlaveis { get; set; }
        public DatabaseVersion()
        {
            this.GetObjetosControlaveis();
        }

        private void GetObjetosControlaveis()
        {
            ObjetosControlaveis = new List<T_Objeto_Controlavel>()
            {
                //#################################   MENUS #################################################################################################################################################3
                // inicial
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.GrupoMaquina",         OBJ_DESCRICAO = "Grupos de Máquinas",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "INICIAL"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.Observacoes",         OBJ_DESCRICAO = "Observações",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "INICIAL"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.Maquina",              OBJ_DESCRICAO = "Máquinas",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "INICIAL"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.Equipe",              OBJ_DESCRICAO = "Equipes",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "INICIAL"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.Impressora",           OBJ_DESCRICAO = "Impressoras",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "INICIAL"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.Calendario",           OBJ_DESCRICAO = "Calendários",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "INICIAL"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.ItensCalendario",      OBJ_DESCRICAO = "Itens dos Calendários",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "INICIAL"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.Municipio",            OBJ_DESCRICAO = "Municípios",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "INICIAL"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.Ocorrencia",           OBJ_DESCRICAO = "Ocorrências",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "INICIAL"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.Param",                OBJ_DESCRICAO = "Parâmetros",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "INICIAL"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.CustoEntreOps",        OBJ_DESCRICAO = "Custo Entre OPs",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "INICIAL"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.Consultas",            OBJ_DESCRICAO = "Consultas",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "INICIAL"},

                // TRANSPORTE
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.TipoVeiculo",          OBJ_DESCRICAO = "Tipos de Veículos",   OBJ_TIPO = "CLASSE",OBJ_GRUPO = "TRANSPORTE"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.Veiculo",              OBJ_DESCRICAO = "Veículos",       OBJ_TIPO = "CLASSE",OBJ_GRUPO = "TRANSPORTE"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.Carga",                OBJ_DESCRICAO = "Cargas",   OBJ_TIPO = "CLASSE",OBJ_GRUPO = "TRANSPORTE"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.Mapa",                 OBJ_DESCRICAO = "Mapa",   OBJ_TIPO = "CLASSE",OBJ_GRUPO = "TRANSPORTE"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.PontosMapa",           OBJ_DESCRICAO = "Pontos do Mapa",   OBJ_TIPO = "CLASSE",OBJ_GRUPO = "TRANSPORTE"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.RotaPontosMapa",       OBJ_DESCRICAO = "Rotas Factíveis",   OBJ_TIPO = "CLASSE",OBJ_GRUPO = "TRANSPORTE"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.Transportadora",       OBJ_DESCRICAO = "Transportadoras",   OBJ_TIPO = "CLASSE",OBJ_GRUPO = "TRANSPORTE"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.TemposLogisticos",     OBJ_DESCRICAO = "Tempos Logísticos",   OBJ_TIPO = "CLASSE",OBJ_GRUPO = "TRANSPORTE"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.RestricoesDeRodagem",  OBJ_DESCRICAO = "Restrições de Rodagem",   OBJ_TIPO = "CLASSE",OBJ_GRUPO = "TRANSPORTE"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.InterfaceTelaCalendarioDisponibilidadeVeiculos",      OBJ_DESCRICAO = "Disponibilidade de Veículos",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "TRANSPORTE"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.Romaneio",                             OBJ_DESCRICAO = "Romaneios",OBJ_TIPO = "INTERFACE",OBJ_GRUPO = "TRANSPORTE"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.ItenCalendarioDisponibilidadeVeiculos",OBJ_DESCRICAO = "Item Cal Dispo Veículos",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "TRANSPORTE"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.CalendarioDisponibilidadeVeiculos",OBJ_DESCRICAO = "Cal Disp Veículos",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "TRANSPORTE"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.V_INPUT_INTEGRACAO_BALANCA_FATURAMENTO",OBJ_DESCRICAO = "Integração Balança Faturamento",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "TRANSPORTE"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.EstruturaEtiqueta",OBJ_DESCRICAO = "Estrutura da Etiqueta",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "TRANSPORTE"},
                
                // Engenharia
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.FilaProducao",                         OBJ_DESCRICAO = "Fila de Produção",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ENGENHARIA"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.GrupoProdutoComposicao",               OBJ_DESCRICAO = "Grupos de Composição",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ENGENHARIA"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.GrupoProdutoPalete",               OBJ_DESCRICAO = "Grupos de Paletes",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ENGENHARIA"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.GrupoProdutoConjunto",               OBJ_DESCRICAO = "Grupos de Conjuntos",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ENGENHARIA"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.GrupoProdutoOutros",               OBJ_DESCRICAO = "Grupos de Produtos",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ENGENHARIA"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.EstruturaProduto",               OBJ_DESCRICAO = "Estrutura do Produto",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ENGENHARIA"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.InterfaceTelaProdutoCaixa",               OBJ_DESCRICAO = "Caixas e Acessórios",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ENGENHARIA"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.ProdutoChapaIntermediaria",               OBJ_DESCRICAO = "Chapas Intermediárias",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ENGENHARIA"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.ProdutoTinta",               OBJ_DESCRICAO = "Tintas",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ENGENHARIA"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.ProdutoPalete",               OBJ_DESCRICAO = "Paletes",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ENGENHARIA"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.ProdutoConjunto",               OBJ_DESCRICAO = "Conjuntos",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ENGENHARIA"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.UnidadeMedida",              OBJ_DESCRICAO = "Unidades de Medida",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ENGENHARIA"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.Roteiro",                    OBJ_DESCRICAO = "Roteiros",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ENGENHARIA"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.Operacoes",                    OBJ_DESCRICAO = "Operações",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ENGENHARIA"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.T_MAQUINAS_EQUIPES",                    OBJ_DESCRICAO = "Maquinas Equipes",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ENGENHARIA"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.ProdutoCliches",           OBJ_DESCRICAO = "Clichê",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ENGENHARIA"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.ProdutoFaca",           OBJ_DESCRICAO = "Faca",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ENGENHARIA"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.V_DISPONIBILIDADE_CLICHE",           OBJ_DESCRICAO = "Disponibilidade de Cliche",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ENGENHARIA"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.V_DISPONIBILIDADE_FACA",           OBJ_DESCRICAO = "Disponibilidade de Faca",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ENGENHARIA"},
                // Estoque
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.ProducaoCodigoBarras",                 OBJ_DESCRICAO = "Apont Caixas e Acessórios",OBJ_TIPO = "INTERFACE",OBJ_GRUPO = "ESTOQUE"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.ProducaoCodBar_Quantidade",            OBJ_DESCRICAO = "Apontamento de Chapas",OBJ_TIPO = "INTERFACE",OBJ_GRUPO = "ESTOQUE"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.InterfaceTelaImpressaoEtiquetas",  OBJ_DESCRICAO = "Gerar Etiquetas",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ESTOQUE"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.MovimentoEstoqueConsumoMateriaPrima",  OBJ_DESCRICAO = "Consumo Matéria Prima",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ESTOQUE"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.MovimentoEstoquePerdas",               OBJ_DESCRICAO = "Consultar Perdas",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ESTOQUE"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.MovimentoEstoque",                     OBJ_DESCRICAO = "Movimentos de Estoque",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ESTOQUE"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.MovimentoEstoqueEntradaInventario",    OBJ_DESCRICAO = "Entradas no Inventário",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ESTOQUE"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.MovimentoEstoqueProducao",             OBJ_DESCRICAO = "Produção",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ESTOQUE"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.PedasProducao",                        OBJ_DESCRICAO = "Perdas na Produção",OBJ_TIPO = "INTERFACE",OBJ_GRUPO = "ESTOQUE"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.MovimentoEstoqueReservaDeEstoque",     OBJ_DESCRICAO = "Reservas de Estoque",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ESTOQUE"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.MovimentoEstoqueSaidaInventario",      OBJ_DESCRICAO = "Saídas no Inventário",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ESTOQUE"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.SaldosEmEstoquePorLote",               OBJ_DESCRICAO = "Saldos em Estoque",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ESTOQUE"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.InterfaceTelaTransferenciaSimples",    OBJ_DESCRICAO = "Desmontagem de Lotes",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ESTOQUE"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.InterfaceTelaEnderecamento",           OBJ_DESCRICAO = "Endereçamento",OBJ_TIPO = "INTERFACE",OBJ_GRUPO = "ESTOQUE"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.V_IMPRESSAO_ETIQUETAS_OND",           OBJ_DESCRICAO = "Etiqueta/Boletim",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ESTOQUE"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.V_INPUT_NFS",           OBJ_DESCRICAO = "NFS",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ESTOQUE"},

                
                // Qualidade
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.PlanoAmostralTeste", OBJ_DESCRICAO = "Plano Amostral",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "QA"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.TipoTeste", OBJ_DESCRICAO = "Tipos Testes",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "QA"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.TesteFisico", OBJ_DESCRICAO = "Testes Fisicos",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "QA"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.TipoAvaliacao", OBJ_DESCRICAO = "Tipos Avaliacao",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "QA"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.TemplateDeTestes", OBJ_DESCRICAO = "Templates de Testes",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "QA"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.InspecaoVisual", OBJ_DESCRICAO = "Inspecao Visual",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "QA"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.TipoInspecaoVisual", OBJ_DESCRICAO = "Tipo Inspecao Visual",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "QA"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.LaudoTesteFisico", OBJ_DESCRICAO = "Laudo Teste Físico",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "QA"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.ViewControleQABobinas", OBJ_DESCRICAO = "QA Bobinas",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "QA"},
                
                //Vendas
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.Order",          OBJ_DESCRICAO = "Pedidos",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "VENDAS"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.V_CONSULTA_PEDIDO",          OBJ_DESCRICAO = "Consulta de Pedido",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "VENDAS"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.InterfaceTelaOrder",          OBJ_DESCRICAO = "Simular Pedido",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "VENDAS"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.Cliente",        OBJ_DESCRICAO = "Clientes",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "VENDAS"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.Representantes",        OBJ_DESCRICAO = "Representantes",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "VENDAS"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.Cotas",        OBJ_DESCRICAO = "Cotas",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "VENDAS"},

                //Operador
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Controllers.MedicoesController", OBJ_DESCRICAO = "Tela Operador", OBJ_TIPO = "CONTROLLER", OBJ_GRUPO = "OPERADOR"},

                //APS
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Controllers.APSController", OBJ_DESCRICAO = "Tela APS",OBJ_TIPO = "CONTROLLER",OBJ_GRUPO = "APS"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.CargasWeb", OBJ_DESCRICAO = "CargasWeb",OBJ_TIPO = "CONTROLLER",OBJ_GRUPO = "APS"},
                
                //Painel Gestor
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Controllers.PainelGestorController", OBJ_DESCRICAO = "Tela Painel Gestor",OBJ_TIPO = "CONTROLLER",OBJ_GRUPO = "P Gestor"},
                
                //Dashboard
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.SGI.Controllers.HomeController", OBJ_DESCRICAO = "Tela Dashboard",OBJ_TIPO = "CONTROLLER",OBJ_GRUPO = "Dashboard"},
                
                //Interface
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Controllers.InterfaceController", OBJ_DESCRICAO = "Tela Interface",OBJ_TIPO = "CONTROLLER",OBJ_GRUPO = "Interface"},

                //Preferencias do usuário
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.T_PREFERENCIAS", OBJ_DESCRICAO = "Preferencias do Usuário",OBJ_TIPO = "PREF",OBJ_GRUPO = "PREF"},

                //Gestão
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.TargetProduto",                    OBJ_DESCRICAO = "Targets dos Produtos",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "GESTAO"},
                
                //Admin
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Models.T_Usuario",                        OBJ_DESCRICAO = "Usuario",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ADMIN"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Models.T_Perfil",                         OBJ_DESCRICAO = "Perfil",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ADMIN"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Models.T_Objeto_Controlavel",             OBJ_DESCRICAO = "Objeto Controlavel",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ADMIN"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.Logs",           OBJ_DESCRICAO = "Logs do CLP",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ADMIN"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.ClpMedicoes",    OBJ_DESCRICAO = "Medições do CLP",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ADMIN"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.ClpMedicoesH",    OBJ_DESCRICAO = "Medições do CLP (H)",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ADMIN"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.ConsultasGrupos",         OBJ_DESCRICAO = "Consultas Grupos",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ADMIN"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.ConsultasIndicadores",    OBJ_DESCRICAO = "Consulta Indicadores",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ADMIN"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.Relatorios",    OBJ_DESCRICAO = "Cadastro de Relatórios",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ADMIN"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.SGI.Model.T_Grupo",    OBJ_DESCRICAO = "Grupos",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ADMIN"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.SGI.Model.T_Departamentos",    OBJ_DESCRICAO = "Departamentos",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ADMIN"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.SGI.Model.T_Indicadores",    OBJ_DESCRICAO = "Indicadores",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ADMIN"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.SGI.Model.T_Metas",    OBJ_DESCRICAO = "Metas",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ADMIN"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.SGI.Model.T_Negocio",    OBJ_DESCRICAO = "Negócio",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ADMIN"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.Turma",    OBJ_DESCRICAO = "Turma",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ADMIN"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Models.Turno",    OBJ_DESCRICAO = "Turno",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ADMIN"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Models.T_LOGS_DATABASE",    OBJ_DESCRICAO = "Logs Database",OBJ_TIPO = "CLASSE",OBJ_GRUPO = "ADMIN"},

                // Relatorios
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Controllers.Reports.ReportPlanoCargaController",    OBJ_DESCRICAO = "ReportPlanoCarga",OBJ_TIPO = "CONTROLLER",OBJ_GRUPO = "REPORT"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Controllers.Reports.ReportOrdemFabricacaoController",    OBJ_DESCRICAO = "ReportOrdemFabricacaoController",OBJ_TIPO = "CONTROLLER",OBJ_GRUPO = "REPORT"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Controllers.Reports.ReportRomaneioController",    OBJ_DESCRICAO = "ReportRomaneioController",OBJ_TIPO = "CONTROLLER",OBJ_GRUPO = "REPORT"},
                new T_Objeto_Controlavel(){OBJ_ID = "DynamicForms.Areas.PlugAndPlay.Controllers.Reports.ReportEtiquetaController",    OBJ_DESCRICAO = "ReportEtiquetaController",OBJ_TIPO = "CONTROLLER",OBJ_GRUPO = "REPORT"}

            };
        }
    }
}

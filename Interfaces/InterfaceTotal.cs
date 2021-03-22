using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Models;
using DynamicForms.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DynamicForms.Interfaces
{
    public class InterfaceTotal
    {
        private MasterController _masterController;
        private List<string[]> _className;
        private List<LogPlay> _logMc;
        private List<LogPlay> _logI;
        private List<LogPlay> _logImportacaoFinal;
        public List<LogPlay> LogMasterController { get { return this._logMc; } }
        public List<LogPlay> LogInterface { get { return this._logI; } }

        public InterfaceTotal()
        {
            this._masterController = new MasterController();
            this._className = new List<string[]>();
            this._logMc = new List<LogPlay>();
            this._logI = new List<LogPlay>();
            this._logImportacaoFinal = new List<LogPlay>();
        }

        #region modo de importação antigo
        //public double IniciarInterfaceGrupos(int forceInsert)
        //{
        //    var stopwatch = new Stopwatch();
        //    stopwatch.Start();
        //    GrupoFerramentalI gfi = new GrupoFerramentalI();
        //    GrupoProdutoCaixaI gpci = new GrupoProdutoCaixaI();
        //    GrupoProdutoTintaI gpti = new GrupoProdutoTintaI();
        //    GrupoProdutoPapelI gppi = new GrupoProdutoPapelI();
        //    GrupoProdutoChapaI gchvi = new GrupoProdutoChapaI();
        //    GrupoConjuntoI gci = new GrupoConjuntoI();
        //    GrupoMaquinaI gmi = new GrupoMaquinaI();
        //    GrupoProdutoPaleteI gpli = new GrupoProdutoPaleteI();
        //    ProdutoPapelI ppi = new ProdutoPapelI();
        //    ////GRUPO FERRAMENTAL
        //    //_className = new List<string[]> { new string[] { "DynamicForms.Areas.PlugAndPlay.Models.GrupoProduto" } };
        //    //_jSonAux = gfi.ImportarGrupoFerramental(ref _logI);
        //    //if (_jSonAux.Length > 0 && _jSonAux != "[]")
        //    //    _logMc.AddRange(_masterController.UpdateData(new string[] { _jSonAux }, _className, forceInsert, false));
        //    ////GRUPO PALETE
        //    //_jSonAux = gpli.ImportarGrupoProdutoPalete(ref _logI);
        //    //if (_jSonAux.Length > 0 && _jSonAux != "[]")
        //    //    _logMc.AddRange(_masterController.UpdateData(new string[] { _jSonAux }, _className, forceInsert, false));
        //    ////GRUPO PAPEL
        //    //_jSonAux = gppi.ImportarGrupoPapel(ref _logI);
        //    //if (_jSonAux.Length > 0 && _jSonAux != "[]")
        //    //    _logMc.AddRange(_masterController.UpdateData(new string[] { _jSonAux }, _className, forceInsert, false));
        //    ////PRODUTO PAPEL
        //    //_jSonAux = ppi.ImportarProdutoPapel(ref _logI);
        //    //if (_jSonAux.Length > 0 && _jSonAux != "[]")
        //    //{
        //    //    _className = new List<string[]> { new string[] { "DynamicForms.Areas.PlugAndPlay.Models.Produto" } };
        //    //    _logMc.AddRange(_masterController.UpdateData(new string[] { _jSonAux }, _className, 2, false));
        //    //}
        //    ////GRUPO TINTA
        //    //_className = new List<string[]> { new string[] { "DynamicForms.Areas.PlugAndPlay.Models.GrupoProduto" } };
        //    //_jSonAux = gpti.ImportarGrupoProdutoTinta(ref _logI);
        //    //if (_jSonAux.Length > 0 && _jSonAux != "[]")
        //    //{
        //    //    _logMc.AddRange(_masterController.UpdateData(new string[] { _jSonAux }, _className, forceInsert, false));
        //    //}
        //    ////GRUPO CHAPA
        //    //_jSonAux = gchvi.ImportarGrupoChapa(ref _logI);
        //    //if (_jSonAux.Length > 0 && _jSonAux != "[]")
        //    //    _logMc.AddRange(_masterController.UpdateData(new string[] { _jSonAux }, _className, forceInsert, false));
        //    ////GRUPO CAIXA
        //    //_jSonAux = gpci.ImportarGrupoProdutoCaixa(ref _logI);
        //    //if (_jSonAux.Length > 0 && _jSonAux != "[]")
        //    //    _logMc.AddRange(_masterController.UpdateData(new string[] { _jSonAux }, _className, forceInsert, false));
        //    ////GRUPO CONJUNTO
        //    //_jSonAux = gci.ImportarGrupoConjunto(ref _logI);
        //    //if (_jSonAux.Length > 0 && _jSonAux != "[]")
        //    //    _logMc.AddRange(_masterController.UpdateData(new string[] { _jSonAux }, _className, forceInsert, false));
        //    ////GRUPO MAQUINA
        //    //this._className = new List<string[]> { new string[] { "DynamicForms.Areas.PlugAndPlay.Models.GrupoMaquina" } };
        //    //_jSonAux = gmi.ImportarGrupoMaquina(ref _logI);
        //    //if (_jSonAux.Length > 0 && _jSonAux != "[]")
        //    //    _logMc.AddRange(_masterController.UpdateData(new string[] { _jSonAux }, _className, forceInsert, false));

        //    stopwatch.Stop();
        //    double aux = Convert.ToDouble(stopwatch.Elapsed.TotalSeconds.ToString());
        //    return aux;
        //}
        //public double IniciarInterfaceProdutos(int forceInsert)
        //{
        //    ProdutoPaleteI ppli = new ProdutoPaleteI();
        //    TintasI pdti = new TintasI();
        //    ProdutoConjuntoI pdcji = new ProdutoConjuntoI();
        //    ProdutoChapasIntermediariasI pchii = new ProdutoChapasIntermediariasI();
        //    ProdutoChapaVendaI pchvi = new ProdutoChapaVendaI();
        //    EstruturaProdutoI epi = new EstruturaProdutoI();
        //    ProdutoCaixaI pci = new ProdutoCaixaI();
        //    this._className = new List<string[]> { new string[] { "DynamicForms.Areas.PlugAndPlay.Models.Produto" } };
        //    var stopwatch = new Stopwatch();
        //    stopwatch.Start();

        //    ////PALETES
        //    //_jSonAux = ppli.ImportarProdutoPalete(ref _logI);
        //    //if (_jSonAux.Length > 0 && _jSonAux != "[]")
        //    //    _logMc.AddRange(_masterController.UpdateData(new string[] { _jSonAux }, _className, forceInsert, false));
        //    ////FACAS
        //    //_jSonAux = pdfi.ImportarProdutoFaca(ref _logI);
        //    //if (_jSonAux.Length > 0 && _jSonAux != "[]")
        //    //    _logMc.AddRange(_masterController.UpdateData(new string[] { _jSonAux }, _className, forceInsert, false));
        //    ////CLICHES
        //    //_jSonAux = pdcli.ImportarProdutoCliche(ref _logI);
        //    //if (_jSonAux.Length > 0 && _jSonAux != "[]")
        //    //    _logMc.AddRange(_masterController.UpdateData(new string[] { _jSonAux }, _className, forceInsert, false));
        //    ////TINTAS
        //    //_jSonAux = pdti.InportarTintas(ref _logI);
        //    //if (_jSonAux.Length > 0 && _jSonAux != "[]")
        //    //    _logMc.AddRange(_masterController.UpdateData(new string[] { _jSonAux }, _className, forceInsert, false));
        //    ////CHAPS INTERMEDIÁRIAS
        //    //_jSonAux = pchii.ImportarChapasIntermediaria(ref _logI);
        //    //if (_jSonAux.Length > 0 && _jSonAux != "[]")
        //    //    _logMc.AddRange(_masterController.UpdateData(new string[] { _jSonAux }, _className, forceInsert, false));
        //    ////CHAPAS VENDA
        //    //_jSonAux = pchvi.ImportarChapasVenda(ref _logI, false);
        //    //if (_jSonAux.Length > 0 && _jSonAux != "[]")
        //    //    _logMc.AddRange(_masterController.UpdateData(new string[] { _jSonAux }, _className, 2, false));
        //    ////PRODUTO CONJUNTO
        //    //_jSonAux = pdcji.ImportarProdutoConjunto(ref _logI);
        //    //if (_jSonAux.Length > 0 && _jSonAux != "[]")
        //    //    _logMc.AddRange(_masterController.UpdateData(new string[] { _jSonAux }, _className, forceInsert, false));
        //    ////ESTRUTURA DE PRODUTOS
        //    //_jSonAux = epi.ImportarEstruturaProdutos(ref _logI);
        //    //if (_jSonAux.Length > 0 && _jSonAux != "[]")
        //    //{
        //    //    this._className = new List<string[]> { new string[] { "DynamicForms.Areas.PlugAndPlay.Models.EstruturaProduto" } };
        //    //    _logMc.AddRange(_masterController.UpdateData(new string[] { _jSonAux }, _className, 2, false));
        //    //}
        //    ////CAIXAS
        //    //_jSonAux = pci.ImportarProdutoCaixas(ref _logI, true);
        //    //if (_jSonAux.Length > 0 && _jSonAux != "[]")
        //    //{
        //    //    this._className = new List<string[]> { new string[] { "DynamicForms.Areas.PlugAndPlay.Models.Produto" } };
        //    //    _logMc.AddRange(_masterController.UpdateData(new string[] { _jSonAux }, _className, forceInsert, false));
        //    //}

        //    stopwatch.Stop();
        //    double aux = Convert.ToDouble(stopwatch.Elapsed.TotalSeconds.ToString());
        //    return aux;
        //}
        //public double IniciarInterfaceMaquina(int forceInsert)
        //{
        //    MaquinaI maqi = new MaquinaI();
        //    this._className = new List<string[]> { new string[] { "DynamicForms.Areas.PlugAndPlay.Models.Maquina" } };
        //    var stopwatch = new Stopwatch();
        //    stopwatch.Start();
        //    //MAQUINAS E GRUPOS DE MAQUINA
        //    //_jSonAux = maqi.ImportarMaquinas(ref _logI);
        //    //if (_jSonAux.Length > 0 && _jSonAux != "[]")
        //    //    _logMc.AddRange(_masterController.UpdateData(new string[] {_jSonAux }, _className, 1, false));

        //    stopwatch.Stop();
        //    double aux = Convert.ToDouble(stopwatch.Elapsed.TotalSeconds.ToString());
        //    return aux;
        //}
        //public double IniciarInterfaceClientes(int forceInsert)
        //{
        //    ClientesI clii = new ClientesI();
        //    this._className = new List<string[]> { new string[] { "DynamicForms.Areas.PlugAndPlay.Models.Cliente" } };
        //    var stopwatch = new Stopwatch();

        //    stopwatch.Start();
        //    //_jSonAux = clii.InportarClientes(ref _logI);
        //    //_logMc.AddRange(_masterController.UpdateData(new string[] { _jSonAux }, _className, forceInsert, false));
        //    stopwatch.Stop();
        //    double aux = Convert.ToDouble(stopwatch.Elapsed.TotalSeconds.ToString());
        //    return aux;
        //}
        //public double IniciarInterfacePedidos(int forceInsert)
        //{
        //    OrdensI intefaceOrdens = new OrdensI();
        //    var stopwatch = new Stopwatch();
        //    stopwatch.Start();
        //    intefaceOrdens.ImportarOrdensProducao(ref _logI, forceInsert);
        //    stopwatch.Stop();
        //    double aux = Convert.ToDouble(stopwatch.Elapsed.TotalSeconds.ToString());
        //    return aux;
        //}
        #endregion modo de importação antigo

        public List<LogPlay> InterfaceStart(int forceInsert)
        {
            var stopwatchTotal = new Stopwatch();
            stopwatchTotal.Start();
            lock (ParametrosSingleton.Instance.semaforoInterface)
            {
                try
                {
                    var stopwatch = new Stopwatch();
                    using (var db = new ContextFactory().CreateDbContext(new string[] { }))
                    {
                        try
                        {
                            Console.WriteLine("Database: " + db.Database.GetDbConnection().Database);
                            ParametrosSingleton.Instance.Menssagens.RemoveAll(x => x.MEN_TYPE.EndsWith("INTERFACE"));

                            Console.WriteLine($"Executando procedure SP_PLUG_INTERFACE_BEFORE_INPUTS...");
                            stopwatch.Start();
                            db.Database.ExecuteSqlCommand(" EXEC SP_PLUG_INTERFACE_BEFORE_INPUTS ");
                            stopwatch.Stop();
                            Console.WriteLine($"Fim da procedure SP_PLUG_INTERFACE_BEFORE_INPUTS: {stopwatch.Elapsed}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Falha na execucao da procedure SP_PLUG_INTERFACE_BEFORE_INPUTS: ");
                            Console.WriteLine($"{UtilPlay.getErro(ex)} \n");
                            _logImportacaoFinal.Add(new LogPlay("EXEC SP_PLUG_INTERFACE_BEFORE_INPUTS", "ERRO", UtilPlay.getErro(ex)));
                        }

                        ParametrosSingleton.Instance.semaforoInterface = "USING";
                        OrdensI intefaceOrdens = new OrdensI();
                        intefaceOrdens.ImportarOrdensProducao(ref _logI, forceInsert, db);
                        ObservacoesI interfaceObservacoes = new ObservacoesI();
                        interfaceObservacoes.ImportarObservacoes(ref _logI, forceInsert, db);

                        #region Ordenando os logs por grau de importancia

                        _logImportacaoFinal.AddRange(_logI.Where(x => x.NomeClasse == nameof(Municipio)).ToList());
                        _logImportacaoFinal.AddRange(_logI.Where(x => x.NomeClasse == nameof(Cliente)).ToList());
                        _logImportacaoFinal.AddRange(_logI.Where(x => x.NomeClasse == nameof(GrupoMaquina)).ToList());
                        _logImportacaoFinal.AddRange(_logI.Where(x => x.NomeClasse == nameof(Maquina)).ToList());
                        _logImportacaoFinal.AddRange(_logI.Where(x => x.NomeClasse == nameof(GrupoProdutoOutros)).ToList()); // Grupo Papel
                        _logImportacaoFinal.AddRange(_logI.Where(x => x.NomeClasse == nameof(GrupoProdutoComposicao)).ToList());
                        _logImportacaoFinal.AddRange(_logI.Where(x => x.NomeClasse == nameof(GrupoProdutoOutros)).ToList()); // Grupo Ferramental
                        _logImportacaoFinal.AddRange(_logI.Where(x => x.NomeClasse == nameof(GrupoProdutoPalete)).ToList());
                        _logImportacaoFinal.AddRange(_logI.Where(x => x.NomeClasse == nameof(GrupoProdutoOutros)).ToList()); // Grupo Tinta
                        _logImportacaoFinal.AddRange(_logI.Where(x => x.NomeClasse == nameof(GrupoProdutoConjunto)).ToList());
                        _logImportacaoFinal.AddRange(_logI.Where(x => x.NomeClasse == nameof(ProdutoPapel)).ToList());
                        _logImportacaoFinal.AddRange(_logI.Where(x => x.NomeClasse == nameof(ProdutoChapaVenda)).ToList());
                        _logImportacaoFinal.AddRange(_logI.Where(x => x.NomeClasse == nameof(ProdutoPalete)).ToList());
                        _logImportacaoFinal.AddRange(_logI.Where(x => x.NomeClasse == nameof(ProdutoTinta)).ToList());
                        _logImportacaoFinal.AddRange(_logI.Where(x => x.NomeClasse == nameof(ProdutoCaixa)).ToList());
                        _logImportacaoFinal.AddRange(_logI.Where(x => x.NomeClasse == nameof(EstruturaProduto)).ToList());
                        _logImportacaoFinal.AddRange(_logI.Where(x => x.NomeClasse == nameof(Roteiro)).ToList());
                        _logImportacaoFinal.AddRange(_logI.Where(x => x.NomeClasse == nameof(Order)).ToList());
                        _logImportacaoFinal.AddRange(_logI.Where(x => x.NomeClasse == nameof(Observacoes)).ToList());

                        #endregion Ordenando os logs por grau de importancia

                        try
                        {
                            Console.WriteLine($"Executando procedure SP_PLUG_INTERFACE_AFTER_INPUTS...");
                            stopwatch.Start();
                            db.Database.ExecuteSqlCommand(" EXEC SP_PLUG_INTERFACE_AFTER_INPUTS '', '', '',0, 0 ");
                            stopwatch.Stop();
                            Console.WriteLine($"Fim da procedure SP_PLUG_INTERFACE_AFTER_INPUTS: {stopwatch.Elapsed}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Falha na execucao da procedure SP_PLUG_INTERFACE_AFTER_INPUTS: ");
                            Console.WriteLine($"{UtilPlay.getErro(ex)} \n");
                            _logImportacaoFinal.Add(new LogPlay("EXEC SP_PLUG_INTERFACE_AFTER_INPUTS", "ERRO", UtilPlay.getErro(ex)));
                        }
                    }
                    ParametrosSingleton.Instance.semaforoInterface = "FREE";
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Falha na execucao da interface: ");
                    Console.WriteLine($"{UtilPlay.getErro(ex)} \n");
                    ParametrosSingleton.Instance.semaforoInterface = "FREE";
                    _logImportacaoFinal.Add(new LogPlay("InterfaceTotal", "ERRO", UtilPlay.getErro(ex)));
                }

                stopwatchTotal.Stop();
                Console.WriteLine($"Tempo de execucao da interface: {stopwatchTotal.Elapsed}");
                return _logImportacaoFinal;
            }
        }
    }
}


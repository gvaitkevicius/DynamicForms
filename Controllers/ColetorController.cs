using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Areas.PlugAndPlay.Models.Estoque;
using DynamicForms.Context;
using DynamicForms.Models;
using DynamicForms.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DynamicForms.Controllers
{
    //para acessar o coletor  acesse *url*/Coletor/Index
    public class ColetorController : BaseController
    {
        #region Views

        public IActionResult Index()
        {
            //verifica se o usuário já está logado
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("LoginColetor", "Acesso");

            //Controle Acesso
            if (!ValidacoesUsuario.ValidarAcessoTela(ObterUsuarioLogado(), typeof(ColetorController).FullName))
                return RedirectToAction("SemAcesso", "Acesso", new { area = "" });

            return View();
        }

        public IActionResult ApontamentoChapas(string codigo_de_barras, string quantidade)
        {
            //verifica se o usuário já está logado
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("LoginColetor", "Acesso");

            //Se houver um conteúdo, significa que está tentando inserir um novo registro
            if (codigo_de_barras != null || quantidade != null)
            {
                //Chama o método para salvar o endereçamento, que irá retornar uma lista de objetos e modificará a variável logs
                List<LogPlay> logs = new List<LogPlay>();
                List<object> list_objetos = SalvarApontamentoChapas(codigo_de_barras, quantidade, ref logs);

                //Coloca os dados na ViewBag para serem acessados na View
                ViewBag.Logs = logs;
                ViewBag.ListObjetos = list_objetos;
            }

            return View();
        }

        public IActionResult Enderecamento(string endereco, string codigo_de_barras)
        {
            //verifica se o usuário já está logado
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("LoginColetor", "Acesso");

            //Se houver um conteúdo, significa que está tentando inserir um novo registro
            if (endereco != null || codigo_de_barras != null)
            {
                //Chama o método para salvar o endereçamento, que irá retornar uma lista de objetos e modificará a variável logs
                List<LogPlay> logs = new List<LogPlay>();
                List<object> list_objetos = SalvarEnderecamento(endereco, codigo_de_barras, ref logs);

                //Coloca os dados na ViewBag para serem acessados na View
                ViewBag.Logs = logs;
                ViewBag.ListObjetos = list_objetos;
            }

            return View();
        }

        public IActionResult ApontamentoCaixasEAcessorios(string codigo_de_barras)
        {
            //verifica se o usuário já está logado
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("LoginColetor", "Acesso");

            //Se houver um conteúdo, significa que está tentando inserir um novo registro
            if (codigo_de_barras != null)
            {
                List<LogPlay> logs = new List<LogPlay>();
                List<object> list_objetos = SalvarApontamentoCaixasEAcessorios(codigo_de_barras, ref logs);

                ViewBag.Logs = logs;
                ViewBag.ListObjetos = list_objetos;
            }

            return View();
        }

        public IActionResult Romaneio(string car_id, string ord_id, string codigo_de_barras, string incluir)
        {
            //verifica se o usuário já está logado
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("LoginColetor", "Acesso");

            ViewBag.Dados = new Romaneio() { CargaId = car_id, INCLUIR = incluir, PedidoId = ord_id };

            //Se houver um conteúdo, significa que está tentando inserir um novo registro
            if (car_id != null || ord_id != null || codigo_de_barras != null || incluir != null)
            {
                List<LogPlay> logs = new List<LogPlay>();
                List<object> list_objetos = SalvarRomaneio(car_id, ord_id, codigo_de_barras, incluir, ref logs);

                ViewBag.Logs = logs;
                ViewBag.ListObjetos = list_objetos;
            }

            return View();
        }
        
        public IActionResult ApontamentoPerdasNaProducao(string pro_id, string quantidade, string lote, string sublote, string tip_id)
        {
            //verifica se o usuário já está logado
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("LoginColetor", "Acesso");

            //Se houver um conteúdo, significa que está tentando inserir um novo registro
            if (pro_id != null || quantidade != null || lote != null || sublote != null || tip_id != null)
            {
                List<LogPlay> logs = new List<LogPlay>();
                List<object> list_objetos = SalvarApontamentoPerdasNaProducao(pro_id, quantidade, lote, sublote, tip_id, ref logs);

                ViewBag.Logs = logs;
                ViewBag.ListObjetos = list_objetos;
            }

            return View();
        }

        #endregion

        #region Métodos de Salvar

        private List<object> SalvarApontamentoChapas(string codigo_de_barras, string quantidade, ref List<LogPlay> logs)
        {
            var db = new ContextFactory().CreateDbContext(new string[] { });
            T_Usuario usuario = ObterUsuarioLogado();
            var list_objetos = new List<object>() { new ProducaoCodBar_Quantidade() { CodigoDeBarras = codigo_de_barras,
                Quantidade = Double.Parse(quantidade),
                PlayAction = "insert",
                UsuarioLogado = usuario } };

            MasterController mc = new MasterController();
            mc.UsuarioLogado = usuario;

            logs = mc.UpdateData(new List<List<object>>() { list_objetos }, 0, true);

            return list_objetos;
        }

        public List<object> SalvarEnderecamento(string endereco, string codigo_de_barras, ref List<LogPlay> logs)
        {
            var db = new ContextFactory().CreateDbContext(new string[] { });
            T_Usuario usuario = ObterUsuarioLogado();
            var list_objetos = new List<object>() { new InterfaceTelaEnderecamento() { MOV_ENDERECO = endereco,
                ETI_CODIGO_BARRAS = codigo_de_barras,
                PlayAction = "insert",
                UsuarioLogado = usuario } };

            MasterController mc = new MasterController();
            mc.UsuarioLogado = usuario;

            logs = mc.UpdateData(new List<List<object>>() { list_objetos }, 0, true);

            return list_objetos;
        }

        private List<object> SalvarApontamentoCaixasEAcessorios(string codigo_de_barras, ref List<LogPlay> logs)
        {
            var db = new ContextFactory().CreateDbContext(new string[] { });
            T_Usuario usuario = ObterUsuarioLogado();
            MasterController mc = new MasterController();
            mc.UsuarioLogado = usuario;

            var list_objetos = new List<object>() { new ProducaoCodigoBarras() {  CodigoDeBarras = codigo_de_barras,
                PlayAction = "insert",
                UsuarioLogado = usuario } };

            logs = mc.UpdateData(new List<List<object>>() { list_objetos }, 0, true);

            return list_objetos;
        }

        public List<object> SalvarRomaneio(string car_id, string ord_id, string codigo_de_barras, string incluir, ref List<LogPlay> logs)
        {
            var db = new ContextFactory().CreateDbContext(new string[] { });
            T_Usuario usuario = ObterUsuarioLogado();
            var list_objetos = new List<object>() { new Romaneio() {  CargaId = car_id, PedidoId = ord_id, CodigoDeBarras = codigo_de_barras, INCLUIR = incluir,
                PlayAction = "insert",
                UsuarioLogado = usuario } };

            MasterController mc = new MasterController();
            mc.UsuarioLogado = usuario;

            logs = mc.UpdateData(new List<List<object>>() { list_objetos }, 0, true);

            return list_objetos;
        }

        public List<object> SalvarApontamentoPerdasNaProducao(string ord_id, string quantidade, string lote, string sublote, string tip_id, ref List<LogPlay> logs)
        {
            var db = new ContextFactory().CreateDbContext(new string[] { });
            T_Usuario usuario = ObterUsuarioLogado();
            var list_objetos = new List<object>() { new PedasProducao() { PRO_ID = ord_id, MOV_QUANTIDADE = Double.Parse(quantidade), MOV_LOTE = lote, MOV_SUB_LOTE = sublote, TIP_ID = tip_id } };

            MasterController mc = new MasterController();
            mc.UsuarioLogado = usuario;

            logs = mc.UpdateData(new List<List<object>>() { list_objetos }, 0, true);

            return list_objetos;
        }

        #endregion
    }
}

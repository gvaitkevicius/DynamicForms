using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Areas.PlugAndPlay.Models.Estoque;
using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Models;
using DynamicForms.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicForms.Areas.ApiEstoque
{
    [Area("ApiEstoque")]
    [Route("ApiEstoque/[controller]")]
    [ApiController]
    public class EstoqueController : ControllerBase
    {
        [HttpPost]
        [Route("ApontarProducaoCaixaAcessorios")]
        public string ApontarProducaoCaixaAcessorios(string leitura)
        {
            try
            {
                var db = new ContextFactory().CreateDbContext(new string[] { });
                MasterController mc = new MasterController();
                List<LogPlay> logs = new List<LogPlay>();

                string codigoBarrrasEtiqueta;
                T_Usuario usuario = new T_Usuario();

                var t = new Coleta() { Endereco = "RUA 1", SaldoAferido = 10, UserId = 27, Etiquetas = new List<string>() { "76753402#1#1000#15#OCXM069177" } };
                #region Convertendo dados recebidos
                var dadosColetor = t; //JsonConvert.DeserializeObject<Coleta>(leitura);
                if (dadosColetor == null)
                {
                    return $"Erro ao converter os dados recebidos, verifique a formatação dos dados.";
                }
                #endregion
                #region Validandoe recuperando usuário
                usuario = GetUsuario(db, dadosColetor.UserId);
                if (usuario.USE_ID == -1)
                {
                    return $"Usuario nao encontrado para o USE_ID:[{dadosColetor.UserId}]";
                }
                #endregion
                #region Gerando apontamento de produção
                mc.UsuarioLogado = usuario;
                codigoBarrrasEtiqueta = dadosColetor.Etiquetas.First();
                var apontamentoProducao = new List<object>() { new ProducaoCodigoBarras() { CodigoDeBarras = codigoBarrrasEtiqueta, UsuarioLogado = usuario } };
                logs.AddRange(mc.UpdateData(new List<List<object>>() { apontamentoProducao }, 0, true));
                if (logs.Any(x => x.Status.Equals("ERRO")))
                {
                    return JsonConvert.SerializeObject(new { ETI_ID = codigoBarrrasEtiqueta, STATUS = "ERRO" });
                }
                #endregion

                return JsonConvert.SerializeObject(new { ETI_ID = codigoBarrrasEtiqueta, STATUS = "OK" });
            }
            catch (Exception ex)
            {
                return $"Erro:[{ex.Message}]";
            }

        }
        [HttpPost]
        [Route("EnderecamentoLote")]
        public string EnderecamentoLote(string leitura)
        {
            var db = new ContextFactory().CreateDbContext(new string[] { });
            MasterController mc = new MasterController();
            List<LogPlay> logs = new List<LogPlay>();
            string codigoBarrrasEtiqueta;
            T_Usuario usuario = new T_Usuario();
            var t = new Coleta() { Endereco = "RUA 1", SaldoAferido = 10, UserId = 27, Etiquetas = new List<string>() { "76753402#1#1000#15#OCXM069177" } };
            #region Convertendo dados recebidos
            var dadosColetor = t; //JsonConvert.DeserializeObject<Coleta>(leitura);
            if (dadosColetor == null)
            {
                return $"Erro ao converter os dados recebidos, verifique a formatação dos dados.";
            }
            #endregion

            #region Validandoe recuperando usuário
            usuario = GetUsuario(db, dadosColetor.UserId);
            if (usuario.USE_ID == -1)
            {
                return $"Usuario nao encontrado para o USE_ID:[{dadosColetor.UserId}]";
            }
            #endregion


            #region Enderecamento do lote
            mc.UsuarioLogado = usuario;
            codigoBarrrasEtiqueta = dadosColetor.Etiquetas.First();
            var enderecamento = new List<object>() { new InterfaceTelaEnderecamento() { MOV_ENDERECO = dadosColetor.Endereco, ETI_CODIGO_BARRAS = codigoBarrrasEtiqueta, UsuarioLogado = usuario } };
            logs.AddRange(mc.UpdateData(new List<List<object>>() { enderecamento }, 0, false));
            if (logs.Any(x => x.Status.Equals("ERRO")))
            {
                return JsonConvert.SerializeObject(new { ETI_ID = codigoBarrrasEtiqueta, STATUS = "ERRO" });
            }
            #endregion
            return JsonConvert.SerializeObject(new { ETI_ID = codigoBarrrasEtiqueta, STATUS = "OK" });
        }
        [HttpPost]
        [Route("ApontamentoEnderecamentoCaixaAcessorios")]
        public string ApontamentoEnderecamentoCaixaAcessorios(string leitura)
        {
            var db = new ContextFactory().CreateDbContext(new string[] { });
            MasterController mc = new MasterController();
            List<LogPlay> logs = new List<LogPlay>();
            string codigoBarrrasEtiqueta;
            T_Usuario usuario = new T_Usuario();
            #region Convertendo dados recebidos
            var t = new Coleta() { Endereco = "RUA 1", SaldoAferido = 10, UserId = 27, Etiquetas = new List<string>() { "76753402#1#1000#15#OCXM069177" } };
            var dadosColetor = t; //JsonConvert.DeserializeObject<Coleta>(leitura);
            if (dadosColetor == null)
            {
                return $"Erro ao converter os dados recebidos, verifique a formatação dos dados.";
            }
            #endregion
            #region Validandoe recuperando usuário
            usuario = GetUsuario(db, dadosColetor.UserId);
            if (usuario.USE_ID == -1)
            {
                return $"Usuario nao encontrado para o USE_ID:[{dadosColetor.UserId}]";
            }
            #endregion
            mc.UsuarioLogado = usuario;
            codigoBarrrasEtiqueta = dadosColetor.Etiquetas.First();

            return JsonConvert.SerializeObject(new { ETI_ID = codigoBarrrasEtiqueta, STATUS = "OK" });
        }
        [HttpPost]
        [Route("ApontarProducaoChapas")]
        public string ApontarProducaoChapas(string leitura)
        {
            try
            {
                var db = new ContextFactory().CreateDbContext(new string[] { });
                MasterController mc = new MasterController();
                List<LogPlay> logs = new List<LogPlay>();

                string codigoBarrrasEtiqueta;
                T_Usuario usuario = new T_Usuario();

                var t = new Coleta() { Endereco = "RUA 1", SaldoAferido = 10, UserId = 27, Etiquetas = new List<string>() { "76753402#1#1000#15#OCXM069177" } };
                #region Convertendo dados recebidos
                var dadosColetor = t; //JsonConvert.DeserializeObject<Coleta>(leitura);
                if (dadosColetor == null)
                {
                    return $"Erro ao converter os dados recebidos, verifique a formatação dos dados.";
                }
                #endregion
                #region Validandoe recuperando usuário
                usuario = GetUsuario(db, dadosColetor.UserId);
                if (usuario.USE_ID == -1)
                {
                    return $"Usuario nao encontrado para o USE_ID:[{dadosColetor.UserId}]";
                }
                #endregion
                #region Gerando apontamento de produção
                mc.UsuarioLogado = usuario;
                codigoBarrrasEtiqueta = dadosColetor.Etiquetas.First();
                var apontamentoProducao = new List<object>() { new ProducaoCodBar_Quantidade() { CodigoDeBarras = codigoBarrrasEtiqueta,Quantidade=dadosColetor.SaldoAferido, UsuarioLogado = usuario } };
                logs.AddRange(mc.UpdateData(new List<List<object>>() { apontamentoProducao }, 0, true));
                if (logs.Any(x => x.Status.Equals("ERRO")))
                {
                    return JsonConvert.SerializeObject(new { ETI_ID = codigoBarrrasEtiqueta, STATUS = "ERRO" });
                }
                #endregion

                return JsonConvert.SerializeObject(new { ETI_ID = codigoBarrrasEtiqueta, STATUS = "OK" });
            }
            catch (Exception ex)
            {
                return $"Erro:[{ex.Message}]";
            }

        }
        [HttpPost(Name = "Consolidar Inventario")]
        [Route("ConsolidarInventario")]
        public string ConsolidarInventario(string leituraConsolidada)
        {
            try
            {
               
                MasterController mc = new MasterController();
                var db = new ContextFactory().CreateDbContext(new string[] { });
                List<ResumoInventario> inventarioResumido;
                List<LogPlay> logs = new List<LogPlay>();
                #region Dicionario etiquetas/enderecos
                var enderecosInventariados = new Coleta().LoadData();//JsonConvert.DeserializeObject<List<Coleta>>(leituraConsolidada);
                mc.UsuarioLogado = GetUsuario(db, enderecosInventariados.First().UserId);
                var etiquetaEndereco = new Dictionary<string, string>();
                var loteEtiqueta = new Dictionary<string, string>();
                foreach (var endereco in enderecosInventariados)
                {
                    foreach (var etiqueta in endereco.Etiquetas)
                    {
                        etiquetaEndereco.Add(etiqueta, endereco.Endereco);
                        loteEtiqueta.Add(etiqueta.Replace("#","."), etiqueta);
                    }
                }
                #endregion
                #region Recuperando e Limpando os enderecos de todos os lotes
                var reservas = (from reserva in db.MovimentoEstoqueReservaDeEstoque
                                where !String.IsNullOrEmpty(reserva.MOV_ENDERECO)
                                 && !reserva.MOV_ESTORNO.Equals("E")
                                select reserva).AsNoTracking().ToList();
                foreach (var reserva in reservas)
                {
                    reserva.MOV_ENDERECO = "";
                    reserva.PlayAction = "update";
                }
                #endregion
                
                inventarioResumido = ResumirInventario(db, $"'{String.Join("','", etiquetaEndereco.Select(x => x.Key.Replace("#", ".")).ToList())}'");
                
                var reservaLotesResumoOk = GetReservasResumoOk(db, inventarioResumido, etiquetaEndereco);

                foreach (var reserva in reservaLotesResumoOk)
                {
                    loteEtiqueta.GetValueOrDefault(reserva.MOV_LOTE);
                }
                

                #region Transacao das modificações
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.AddRange(reservas);
                        db.SaveChanges();
                        db.AddRange(reservaLotesResumoOk);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        logs.Add(new LogPlay() {Status="ERRO",MsgErro=ex.Message });
                    }
                }
                #endregion
                var mr = JsonConvert.SerializeObject(inventarioResumido);
                ///return JsonConvert.SerializeObject(new { ETI_ID = "7777-00#1#1#652#CH020504531117'", STATUS = "OK" });
                return mr;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return "Erro na solicitação:" + msg;
            }
        }


        private T_Usuario GetUsuario(JSgi db, int useId)
        {
            var usuario = db.T_Usuario.AsNoTracking().Where(x => x.USE_ID == useId).FirstOrDefault();
            return usuario ?? new T_Usuario() { USE_ID = -1 };
        }

        private static List<MovimentoEstoqueReservaDeEstoque> GetReservasResumoOk(JSgi db, List<ResumoInventario> inventarioResumido, Dictionary<string, string> etiquetasLidas)
        {
            var etiquetasComResumoOK = etiquetasLidas.Where(x => inventarioResumido.Where(z => z.STATUS.Equals("OK")).Select(z => z.MOV_LOTE).ToList().Contains(x.Key.Replace("#", "."))).ToList();
            string queryStr = $"SELECT M.*  FROM T_MOVIMENTOS_ESTOQUE M  WHERE  M.TIP_ID = '998' AND ISNULL(MOV_ESTORNO,'') <> 'E' AND M.MOV_LOTE IN('{String.Join("','", etiquetasComResumoOK.Select(x => x.Key.Replace("#", ".")).ToList())}')";
            var reservasLotesComStatusOk = db.MovimentoEstoqueReservaDeEstoque.FromSql(queryStr).ToList();
            return reservasLotesComStatusOk;
        }

        private static List<ResumoInventario> ResumirInventario(JSgi db, string codigosBarra)
        {
            db.Database.OpenConnection();
            using (DbCommand command = db.Database.GetDbConnection().CreateCommand())
            {
                #region Consulta SQL
                StringBuilder sql = new StringBuilder();
                sql.Append($"SELECT   MV.MOV_ENDERECO " +
                    $"         , MV.MOV_LOTE " +
                    $"         , MV.MOV_SUB_LOTE " +
                    $"         , MV.ORD_ID " +
                    $"         , VS.SALDO AS SALDO_SISTEMA " +
                    $"         , 0        AS SALDO_AFERIDO " +
                    $"         , CASE " +
                    $"                WHEN MV.MOV_LOTE = MVU.MOV_LOTE " +
                    $"                    THEN 'OK' " +
                    $"            ELSE " +
                    $"           CASE " +
                    $"                WHEN MV.MOV_LOTE IS NOT NULL AND MVU.MOV_LOTE IS NULL " +
                    $"                    THEN 'ESTA NO SISTEMA E NÃO ESTÁ NO ESTOQUE' " +
                    $"            END " +
                    $"          END AS STATUS " +
                    $"                         FROM T_MOVIMENTOS_ESTOQUE MV " +
                    $"                         LEFT JOIN V_SALDO_ESTOQUE_POR_LOTE VS ON VS.MOV_LOTE = MV.MOV_LOTE " +
                    $"                          LEFT JOIN(SELECT MOV_LOTE FROM T_MOVIMENTOS_ESTOQUE " +
                    $"                                              WHERE TIP_ID = '998' AND ISNULL(MOV_ESTORNO, '') <> 'E' AND " +
                    $"                                                    MOV_LOTE IN(SELECT ETI_LOTE FROM T_ETIQUETAS)) " +
                    $"                                              AS MVU ON MVU.MOV_LOTE = MV.MOV_LOTE " +
                    $"                                            WHERE MV.TIP_ID = '998' AND " +
                    $"                                                 ISNULL(MV.MOV_ESTORNO, '') <> 'E' AND " +
                    $"                                                 ISNULL(MV.MOV_ENDERECO, '') <> '' " +
                    $"                             GROUP BY MV.ORD_ID " +
                    $"                                , MV.MOV_ENDERECO" +
                    $"                                , MV.MOV_LOTE" +
                    $"                                , MV.MOV_SUB_LOTE" +
                    $"                                , MV.ORD_ID" +
                    $"                                , VS.SALDO" +
                    $"                                , MVU.MOV_LOTE; ");
                #endregion
                command.CommandText = sql.ToString();
                command.CommandType = CommandType.Text;
                try
                {
                    List<ResumoInventario> lista = new List<ResumoInventario>();
                    using (DbDataReader result = command.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            lista.Add(new ResumoInventario()
                            {
                                MOV_ENDERECO = result["MOV_ENDERECO"].ToString(),
                                MOV_LOTE = result["MOV_LOTE"].ToString(),
                                MOV_SUB_LOTE = result["MOV_SUB_LOTE"].ToString(),
                                ORD_ID = result["ORD_ID"].ToString(),
                                SALDO_SISTEMA = Convert.ToDouble(result["SALDO_SISTEMA"]),
                                SALDO_AFERIDO = Convert.ToDouble(result["SALDO_AFERIDO"]),
                                STATUS = result["STATUS"].ToString()
                            });
                        }
                        db.Database.CloseConnection();
                        return lista;
                    }
                }
                catch (Exception ex)
                {
                    return new List<ResumoInventario>() { new ResumoInventario { PlayMsgErroValidacao = $"Erro ao executar a consulta: {sql} \n {ex.Message}" } };
                }
            }
        }
    }
}

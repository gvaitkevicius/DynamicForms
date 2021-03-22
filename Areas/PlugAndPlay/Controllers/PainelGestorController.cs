using System;
using System.Collections.Generic;
using System.Linq;
using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Models;
using DynamicForms.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DynamicForms.Areas.PlugAndPlay.Controllers
{
    [Authorize]
    [Area("plugandplay")]
    public class PainelGestorController : BaseController
    {
        // GET: PainelGestor
        public ActionResult Index(string Filtro) // estrutura da pagina
        {
            T_Usuario user = ObterUsuarioLogado();
            ViewBag.UserName = user.USE_NOME;

            //Controle Acesso
            if (!ValidacoesUsuario.ValidarAcessoTela(user, typeof(PainelGestorController).FullName))
                return RedirectToAction("SemAcesso", "Acesso", new { area = "" });

            ViewBag.m = PainelGestorListaMaquinasE(Filtro);
            ViewBag.t = PainelGestorListaTurnoE(Filtro);
            ViewBag.r = PainelGestorRankingE();
            //ViewBag.r = PainelGestorRanking(); Pendência (Reescrever essa função usando o padrão Entity Core)
            //ViewBag.oee = PainelGestorOEEChart(Filtro);
            ViewBag.tm = PainelGestorListaTurnoMaquinas(Filtro);
            ViewBag.d = DateTime.Now;
            return View();
        }


        public ActionResult GestorAjax(string Filtro) // estrutura da pagina
        {
            List<V_PAINEL_GESTOR_STATUS_MAQUINAS> m = PainelGestorListaMaquinasE(Filtro);
            var t = PainelGestorListaTurnoE(Filtro);
            //var r = PainelGestorRanking(); Pendência (Reescrever essa função usando o padrão Entity Core)
            var r = PainelGestorRankingE();
            //oee = PainelGestorOEEChart(Filtro),
            var tm = PainelGestorListaTurnoMaquinas(Filtro);
            var d = DateTime.Now;

            JsonSerializerSettings settingsJSON = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver()
            };

            return Json(new
            {
                m = m,
                t = t,
                r = r, //Pendência (Reescrever essa função usando o padrão Entity Core)
                tm = tm,
                d = d
            }, settingsJSON);
        }



        public List<V_PAINEL_GESTOR_STATUS_MAQUINAS> PainelGestorListaMaquinasE(string Filtro)
        {
            UsuarioSingleton usuarioSingleton = UsuarioSingleton.Instance;
            T_Usuario usuario = usuarioSingleton.ObterUsuario(ObterUsuarioLogado().USE_ID);

            List<T_PREFERENCIAS> pref = usuario.T_PREFERENCIAS.ToList();
            string fat_tempo = "SEGUNDO";
            string usuario_logado = usuario.USE_NOME + "_" + usuario.USE_ID;
            int i = 0;
            while (i < pref.Count && !pref[i].PRE_TIPO.Equals("FAT_TEMPO"))
                i++;

            if (i < pref.Count)
                fat_tempo = pref[i].PRE_VALOR;


            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                var xd = getFiltro(Filtro, "DS_DIA_TURMA");
                //--,FPR_OBS_PRODUCAO
                var sql = "select * from (select ROT_MAQ_ID, M.MAQ_DESCRICAO, M.MAQ_STATUS, STATUS_COR_MAQUINA, ULTIMA_ATUALIZACAO, OCO_ID, OCO_DESCRICAO, FEE_OBSERVACOES, FEEDBACKS_PENDENTES, TEMPO_SEM_FEEDBACK, OPS_PARCIAIS, CLI_ID " +
                ",CLI_NOME, CLI_FONE, CLI_OBS, PC_PRO_ID, PC_PRO_DESCRICAO, PC_UNI_ID, PA_PRO_ID, PA_PRO_DESCRICAO, PA_UNI_ID, ORD_PRECO_UNITARIO, ORD_QUANTIDADE, ORD_DATA_ENTREGA_DE, ORD_DATA_ENTREGA_ATE " +
                ", ORD_TIPO, ORD_TOLERANCIA_MAIS, ORD_TOLERANCIA_MENOS, FPR_DATA_INICIO_PREVISTA, FPR_DATA_FIM_PREVISTA, FPR_DATA_FIM_MAXIMA, FPR_QUANTIDADE_PREVISTA,FPR_STATUS,FPR_TEMPO_DECORRIDO_SETUP,FPR_TEMPO_DECORRIDO_SETUPA " +
                ",FPR_TEMPO_DECORRIDO_PERFORMANC,FPR_QTD_PERFORMANCE,FPR_QTD_SETUP,FPR_QTD_PRODUZIDA,FPR_TEMPO_TEORICO_PERFORMANCE,FPR_TEMPO_RESTANTE_PERFORMANC,FPR_VELOCIDADE_P_ATINGIR_META,FPR_QTD_RESTANTE,FPR_VELO_ATU_PC_SEGUNDO " +
                ",FPR_TEMPO_DECO_PEQUENA_PARADA,FPR_PERFORMANCE_PROJETADA,STATUS_FILA,ROT_PECAS_POR_PULSO,TAR_PROXIMA_META_PERFORMANCE,PERCENTUAL_PERFORMANCE,STATUS_COR_PERFORMANCE,TAR_PROXIMA_META_TEMPO_SETUP,PERCENTUAL_SETUP,STATUS_COR_SETUP " +
                ",TAR_PROXIMA_META_TEMPO_SETUP_AJUSTE,PERCENTUAL_SETUP_AJUSTE,STATUS_COR_SETUP_AJUSTE,PERCENTUAL_SETUP_GERAL,STATUS_COR_SETUP_GERAL,M.MAQ_ULTIMA_ATUALIZACAO,PERCENTUAL_CONCLUIDO_SETUP,PERCENTUAL_CONCLUIDO_SETUP_AJUSTE " +
                ",PERCENTUAL_CONCLUIDO_PERFORMANCE,VELO_ATU_PC_SEGUNDO_ROUD_1,TAR_PERCENTUAL_REALIZADO_PERFORMANCE,OCO_ID_PERFORMANCE,TAR_OBS_PERFORMANCE,OCO_ID_SETUP,TAR_OBS_SETUP,OCO_ID_SETUPA,TAR_OBS_SETUPA,TAR_TIPO_FEEDBACK_PERFORMANCE " +
                ",TAR_TIPO_FEEDBACK_SETUP,TAR_TIPO_FEEDBACK_SETUP_AJUSTE,TAR_QTD_SETUP_AJUSTE,TAR_QTD,TAR_PARAMETRO_TIME_WORK_STOP_MACHINE,TAR_PARAMETRO_TEMPO_QUEBRA_DE_LOTE,TAR_PERFORMANCE_MAX_VERDE,TAR_PERFORMANCE_MIN_VERDE,TAR_SETUP_MAX_VERDE " +
                ",TAR_SETUP_MIN_VERDE,TAR_SETUPA_MAX_VERDE,TAR_SETUPA_MIN_VERDE,TAR_PERFORMANCE_MIN_AMARELO,TAR_SETUP_MAX_AMARELO,TAR_SETUPA_MAX_AMARELO,TAR_ID " +
                ",sum(isnull(SETUP_GERAL_AZUL, 0))SETUP_GERAL_AZUL,sum(isnull(SETUP_GERAL_VERDE, 0))SETUP_GERAL_VERDE,sum(isnull(SETUP_GERAL_AMARELO, 0))SETUP_GERAL_AMARELO,sum(isnull(SETUP_GERAL_VERMELHO, 0))SETUP_GERAL_VERMELHO,sum(isnull(PERFORMANCE_AZUL, 0))PERFORMANCE_AZUL,sum(isnull(PERFORMANCE_VERDE, 0))PERFORMANCE_VERDE " +
                ",sum(isnull(PERFORMANCE_AMARELO, 0))PERFORMANCE_AMARELO,sum(isnull(PERFORMANCE_VERMELHO, 0))PERFORMANCE_VERMELHO,dbo.FP_CONVTIME(sum(isnull(TEMPO_PARADAS_NAO_PROGRAMADAS, 0)))TEMPO_PARADAS_NAO_PROGRAMADAS,sum(isnull(QTD_PARADAS_NAO_PROGRAMADAS, 0))QTD_PARADAS_NAO_PROGRAMADAS,dbo.FP_CONVTIME(sum(isnull(TEMPO_PARADAS_NAO_PROGRAMADAS_EXETO_SETUP, 0)))TEMPO_PARADAS_NAO_PROGRAMADAS_EXETO_SETUP " +
                ",sum(isnull(cast(QTD_PARADAS_NAO_PROGRAMADAS_EXETO_SETUP as float), 0.0))QTD_PARADAS_NAO_PROGRAMADAS_EXETO_SETUP,dbo.FP_CONVTIME(sum(isnull(TEMPO_PEQUENAS_PARADAS, 0)))TEMPO_PEQUENAS_PARADAS,sum(isnull(cast(QTD_PEQUENAS_PARADAS as float), 0.0))QTD_PEQUENAS_PARADAS " +
                " from T_MAQUINA M LEFT JOIN V_PAINEL_GESTOR_STATUS_MAQUINAS P ON P.ROT_MAQ_ID =M.MAQ_ID LEFT JOIN V_PAINEL_GESTOR_DESEMPENHO_MAQUINAS D ON D.MAQ_ID = ROT_MAQ_ID AND " + xd + " WHERE  M.MAQ_CONTROL_IP <> ''  group by " +

                " ROT_MAQ_ID, M.MAQ_DESCRICAO, M.MAQ_STATUS, STATUS_COR_MAQUINA, ULTIMA_ATUALIZACAO, OCO_ID, OCO_DESCRICAO, FEE_OBSERVACOES, FEEDBACKS_PENDENTES, TEMPO_SEM_FEEDBACK, OPS_PARCIAIS, CLI_ID " +
                ",CLI_NOME, CLI_FONE, CLI_OBS, PC_PRO_ID, PC_PRO_DESCRICAO, PC_UNI_ID, PA_PRO_ID, PA_PRO_DESCRICAO, PA_UNI_ID, ORD_PRECO_UNITARIO, ORD_QUANTIDADE, ORD_DATA_ENTREGA_DE, ORD_DATA_ENTREGA_ATE " +
                ", ORD_TIPO, ORD_TOLERANCIA_MAIS, ORD_TOLERANCIA_MENOS, FPR_DATA_INICIO_PREVISTA, FPR_DATA_FIM_PREVISTA, FPR_DATA_FIM_MAXIMA, FPR_QUANTIDADE_PREVISTA,FPR_STATUS,FPR_TEMPO_DECORRIDO_SETUP,FPR_TEMPO_DECORRIDO_SETUPA " +
                ",FPR_TEMPO_DECORRIDO_PERFORMANC,FPR_QTD_PERFORMANCE,FPR_QTD_SETUP,FPR_QTD_PRODUZIDA,FPR_TEMPO_TEORICO_PERFORMANCE,FPR_TEMPO_RESTANTE_PERFORMANC,FPR_VELOCIDADE_P_ATINGIR_META,FPR_QTD_RESTANTE,FPR_VELO_ATU_PC_SEGUNDO " +
                ",FPR_TEMPO_DECO_PEQUENA_PARADA,FPR_PERFORMANCE_PROJETADA,STATUS_FILA,ROT_PECAS_POR_PULSO,TAR_PROXIMA_META_PERFORMANCE,PERCENTUAL_PERFORMANCE,STATUS_COR_PERFORMANCE,TAR_PROXIMA_META_TEMPO_SETUP,PERCENTUAL_SETUP,STATUS_COR_SETUP " +
                ",TAR_PROXIMA_META_TEMPO_SETUP_AJUSTE,PERCENTUAL_SETUP_AJUSTE,STATUS_COR_SETUP_AJUSTE,PERCENTUAL_SETUP_GERAL,STATUS_COR_SETUP_GERAL,M.MAQ_ULTIMA_ATUALIZACAO,PERCENTUAL_CONCLUIDO_SETUP,PERCENTUAL_CONCLUIDO_SETUP_AJUSTE " +
                ",PERCENTUAL_CONCLUIDO_PERFORMANCE,VELO_ATU_PC_SEGUNDO_ROUD_1,TAR_PERCENTUAL_REALIZADO_PERFORMANCE,OCO_ID_PERFORMANCE,TAR_OBS_PERFORMANCE,OCO_ID_SETUP,TAR_OBS_SETUP,OCO_ID_SETUPA,TAR_OBS_SETUPA,TAR_TIPO_FEEDBACK_PERFORMANCE " +
                ",TAR_TIPO_FEEDBACK_SETUP,TAR_TIPO_FEEDBACK_SETUP_AJUSTE,TAR_QTD_SETUP_AJUSTE,TAR_QTD,TAR_PARAMETRO_TIME_WORK_STOP_MACHINE,TAR_PARAMETRO_TEMPO_QUEBRA_DE_LOTE,TAR_PERFORMANCE_MAX_VERDE,TAR_PERFORMANCE_MIN_VERDE,TAR_SETUP_MAX_VERDE " +
                ",TAR_SETUP_MIN_VERDE,TAR_SETUPA_MAX_VERDE,TAR_SETUPA_MIN_VERDE,TAR_PERFORMANCE_MIN_AMARELO,TAR_SETUP_MAX_AMARELO,TAR_SETUPA_MAX_AMARELO,TAR_ID ) as tb1 where ROT_MAQ_ID is not null";

                //return db.Database.SqlQuery<V_PAINEL_GESTOR_STATUS_MAQUINAS>(sql).ToList();
                List<V_PAINEL_GESTOR_STATUS_MAQUINAS> lista = db.Query<V_PAINEL_GESTOR_STATUS_MAQUINAS>().FromSql(sql).ToList();
                for (i = 0; i < lista.Count; i++)
                {
                    lista[i].UN_TEMPO = fat_tempo.ToLower();
                    lista[i].VELO_ATU_PC_SEGUNDO_ROUD_1_STRING = BaseController.ConversorUnidades(Convert.ToDouble(lista[i].VELO_ATU_PC_SEGUNDO_ROUD_1), fat_tempo);
                }

                return lista;
            }
        }
        public List<V_PAINEL_GESTOR_DESEMPENHO_TURNOS_MAQUINA> PainelGestorListaTurnoMaquinas(string Filtro)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                return db.Query<V_PAINEL_GESTOR_DESEMPENHO_TURNOS_MAQUINA>().FromSql("select * from V_PAINEL_GESTOR_DESEMPENHO_TURNOS_MAQUINA WHERE " + getFiltro(Filtro, "FEE_DIA_TURMA")).ToList();
            }
        }
        public List<V_PAINEL_GESTOR_DESEMPENHO_TURNOS> PainelGestorListaTurnoE(string Filtro)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                return db.Query<V_PAINEL_GESTOR_DESEMPENHO_TURNOS>().FromSql("select * from V_PAINEL_GESTOR_DESEMPENHO_TURNOS  WHERE  " + getFiltro(Filtro, "FEE_DIA_TURMA")).ToList();
            }
        }

        public string getFiltro(string Filtro, string campo)
        {

            if (Filtro == null)
            {
                Filtro = "1";
            }
            if (Filtro == "1")
            {
                //
                Filtro = campo + "  = dbo.DIATURMA(GETDATE()) ";
            }
            if (Filtro == "2")
            {
                Filtro = campo + "  = dbo.DIATURMA(GETDATE() - 1) ";
            }
            if (Filtro == "3")
            {
                Filtro = campo + "  >= dbo.DIATURMA(dateadd(DAY, -(DATEPART(weekday, getdate()) - 1), getdate())) ";
            }
            if (Filtro == "4")
            {
                Filtro = campo + "  BETWEEN dbo.DIATURMA(dateadd(DAY, -(DATEPART(weekday, getdate()) - 1), getdate())) " +
                        " AND dbo.DIATURMA(dateadd(DAY, (7 - DATEPART(weekday, getdate())), getdate())) ";
            }
            if (Filtro == "5")
            {
                Filtro = " LEFT(" + campo + ", 6) = LEFT(dbo.DIATURMA(GETDATE()), 6) ";
            }
            if (Filtro == "6")
            {
                Filtro = " LEFT(" + campo + ", 6) = LEFT(dbo.DIATURMA(DATEADD(MONTH, -1, GETDATE())), 6) ";
            }
            return Filtro;
        }

        /* Funções comentadas
        public Resultquery PainelGestorOEEChart(string Filtro)
        {
            var queryResult = new Resultquery();
            string[,] dados = new string[0, 0];
            int cont = 0;
            var cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PlayConect"].ConnectionString);
            var cmd = new SqlCommand();
            cmd.CommandText = " SELECT * FROM V_PAINEL_GESTOR_OEE  WHERE PER_ID = 'M' AND LEFT(MED_DATAMEDICAO,6) = FORMAT( GETDATE(), 'yyyyMM','en-US') ";

            if (cnn.State != ConnectionState.Closed) cnn.Close();
            cnn.Open();
            try
            {
                cmd.CommandTimeout = 99999;
                cmd.Connection = cnn;
                DataSet ds = new DataSet();
                DataAdapter data = new SqlDataAdapter(cmd);
                data.Fill(ds);
                queryResult.Id = 0;
                queryResult.Titulo = "";
                queryResult.Descricao = "";
                queryResult.Tipo = "SQL";
                queryResult.Qtdlinhas = ds.Tables[0].Columns.Count;
                queryResult.Dados = new string[ds.Tables[0].Rows.Count, ds.Tables[0].Columns.Count];
                queryResult.Colunas = new List<Coluna>();
                queryResult.Linhas = new List<LineData>();

                //public List<Colunas> Colunas { get; set; }
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    Coluna Col = new Coluna()
                    {
                        Nome = ds.Tables[0].Columns[i].ColumnName,
                        Tipo = ds.Tables[0].Columns[i].DataType.ToString()
                    };
                    queryResult.Colunas.Add(Col);
                }

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    LineData line = new LineData();
                    for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                    {
                        queryResult.Dados[cont, i] = row[i].ToString();
                        line.linha.Add(row[i].ToString());
                    }
                    queryResult.Linhas.Add(line);
                    cont++;
                }
            }
            catch (Exception erro)
            {
                throw new Exception("Erro ao listar tipo embalagem: " + erro.Message);
            }
            finally
            {
                cnn.Close();
            }

            return queryResult;
        }

        public Resultquery PainelGestorListaMaquinas()
        {
            var queryResult = new Resultquery();
            string[,] dados = new string[0, 0];
            int cont = 0;
            var cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PlayConect"].ConnectionString);
            var cmd = new SqlCommand();
            cmd.CommandText = " SELECT * FROM V_PAINEL_GESTOR_STATUS_MAQUINAS ";

            if (cnn.State != ConnectionState.Closed) cnn.Close();
            cnn.Open();
            try
            {
                cmd.CommandTimeout = 99999;
                cmd.Connection = cnn;
                DataSet ds = new DataSet();
                DataAdapter data = new SqlDataAdapter(cmd);
                data.Fill(ds);
                queryResult.Id = 0;
                queryResult.Titulo = "";
                queryResult.Descricao = "";
                queryResult.Tipo = "SQL";
                queryResult.Qtdlinhas = ds.Tables[0].Columns.Count;
                queryResult.Dados = new string[ds.Tables[0].Rows.Count, ds.Tables[0].Columns.Count];
                queryResult.Colunas = new List<Coluna>();
                queryResult.Linhas = new List<LineData>();

                //public List<Colunas> Colunas { get; set; }
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    Coluna Col = new Coluna()
                    {
                        Nome = ds.Tables[0].Columns[i].ColumnName,
                        Tipo = ds.Tables[0].Columns[i].DataType.ToString()
                    };
                    queryResult.Colunas.Add(Col);
                }

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    LineData line = new LineData();
                    for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                    {
                        queryResult.Dados[cont, i] = row[i].ToString();
                        line.linha.Add(row[i].ToString());
                    }
                    queryResult.Linhas.Add(line);
                    cont++;
                }
            }
            catch (Exception erro)
            {
                throw new Exception("Erro ao listar tipo embalagem: " + erro.Message);
            }
            finally
            {
                cnn.Close();
            }

            return queryResult;
        }*/

        public List<V_PAINEL_GESTOR_RANKING> PainelGestorRankingE()
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                var sql = "SELECT * FROM V_PAINEL_GESTOR_RANKING ORDER BY ORDEM, round(MED_VALOR,2) DESC";
                return db.Query<V_PAINEL_GESTOR_RANKING>().FromSql(sql).ToList();
                //return db.Query<V_PAINEL_GESTOR_DESEMPENHO_TURNOS_MAQUINA>().FromSql("select * from V_PAINEL_GESTOR_DESEMPENHO_TURNOS_MAQUINA WHERE " + getFiltro(Filtro, "FEE_DIA_TURMA")).ToList();
            }
        }
        /*

    public Resultquery PainelGestorRanking()
    {
        var queryResult = new Resultquery();
        string[,] dados = new string[0, 0];-
        int cont = 0;
        var cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PlayConect"].ConnectionString);
        var cmd = new SqlCommand();
        cmd.CommandText = " SELECT * FROM V_PAINEL_GESTOR_RANKING ORDER BY ORDEM, round(MED_VALOR,2) DESC ";

        if (cnn.State != ConnectionState.Closed) cnn.Close();
        cnn.Open();
        try
        {
            cmd.CommandTimeout = 99999;
            cmd.Connection = cnn;
            DataSet ds = new DataSet();
            DataAdapter data = new SqlDataAdapter(cmd);
            data.Fill(ds);
            queryResult.Id = 0;
            queryResult.Titulo = "";
            queryResult.Descricao = "";
            queryResult.Tipo = "SQL";
            queryResult.Qtdlinhas = ds.Tables[0].Columns.Count;
            queryResult.Dados = new string[ds.Tables[0].Rows.Count, ds.Tables[0].Columns.Count];
            queryResult.Colunas = new List<Coluna>();
            queryResult.Linhas = new List<LineData>();

            //public List<Colunas> Colunas { get; set; }
            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            {
                Coluna Col = new Coluna()
                {
                    Nome = ds.Tables[0].Columns[i].ColumnName,
                    Tipo = ds.Tables[0].Columns[i].DataType.ToString()
                };
                queryResult.Colunas.Add(Col);
            }

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                LineData line = new LineData();
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    queryResult.Dados[cont, i] = row[i].ToString();
                    line.linha.Add(row[i].ToString());
                }
                queryResult.Linhas.Add(line);
                cont++;
            }
        }
        catch (Exception erro)
        {
            throw new Exception("Erro ao listar tipo embalagem: " + erro.Message);
        }
        finally
        {
            cnn.Close();
        }

        return queryResult;
    }

    public Resultquery PainelGestorDesempenhoPorTurno()
    {
        var queryResult = new Resultquery();
        string[,] dados = new string[0, 0];
        int cont = 0;
        var cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PlayConect"].ConnectionString);
        var cmd = new SqlCommand();
        cmd.CommandText = " SELECT * FROM V_PAINEL_GESTOR_DESEMPENHO_TURNOS ";

        if (cnn.State != ConnectionState.Closed) cnn.Close();
        cnn.Open();
        try
        {
            cmd.CommandTimeout = 99999;
            cmd.Connection = cnn;
            DataSet ds = new DataSet();
            DataAdapter data = new SqlDataAdapter(cmd);
            data.Fill(ds);
            queryResult.Id = 0;
            queryResult.Titulo = "";
            queryResult.Descricao = "";
            queryResult.Tipo = "SQL";
            queryResult.Qtdlinhas = ds.Tables[0].Columns.Count;
            queryResult.Dados = new string[ds.Tables[0].Rows.Count, ds.Tables[0].Columns.Count];
            queryResult.Colunas = new List<Coluna>();
            queryResult.Linhas = new List<LineData>();

            //public List<Colunas> Colunas { get; set; }
            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            {
                Coluna Col = new Coluna()
                {
                    Nome = ds.Tables[0].Columns[i].ColumnName,
                    Tipo = ds.Tables[0].Columns[i].DataType.ToString()
                };
                queryResult.Colunas.Add(Col);
            }

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                LineData line = new LineData();
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    queryResult.Dados[cont, i] = row[i].ToString();
                    line.linha.Add(row[i].ToString());
                }
                queryResult.Linhas.Add(line);
                cont++;
            }
        }
        catch (Exception erro)
        {
            throw new Exception("Erro ao listar tipo embalagem: " + erro.Message);
        }
        finally
        {
            cnn.Close();
        }

        return queryResult;
    }
    */
    }
}
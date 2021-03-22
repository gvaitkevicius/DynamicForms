using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Areas.SGI.Model;
using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DynamicForms.Areas.PlugAndPlay.Controllers
{
    [Authorize]
    [Area("plugandplay")]
    public class ConsultasController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        public ConsultasController(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }


        //[HttpPost]
        public IActionResult IniciarConsultaFila()
        {
            List<LogPlay> log = new List<LogPlay>();
            try
            {
                string class_name = "DynamicForms.Areas.PlugAndPlay.Models.Consultas";
                Type type = Type.GetType(class_name);

                ConstructorInfo obj_constructor = type.GetConstructor(Type.EmptyTypes);
                object class_object = obj_constructor.Invoke(new object[] { });

                string name_method = "IniciarConsulta";
                MethodInfo method = type.GetMethod(name_method);
                object[] arguments = null;

                if (method != null)
                {
                    string list_obj = "[{\"CON_ID\":44,\"CON_DESCRICAO\":\"FILA_PRODUCAO\",\"CON_GRUPO\":\"\",\"CON_COMAND\":\"SELECT * FROM V_FILA_PRODUCAO\",\"CON_CONEXAO\":\"\",\"CON_TITULO\":\"\",\"CON_TIPO\":\"\",\"PlayAction\":\"\",\"PlayMsgErroValidacao\":\"\",\"IndexClone\":null}]";
                    object list_objects = UtilPlay.ConvertJsonToListObjects(list_obj, class_name);
                    List<object> objects = ((IEnumerable)list_objects).Cast<object>().ToList();
                    UtilPlay.InjetarUsuarioLogado(ref objects, ObterUsuarioLogado());

                    arguments = new object[] { objects, log };
                    method.Invoke(class_object, arguments);
                    return Json(new { log });
                }
                else
                {
                    throw new Exception($"O método especificado não existe na classe: {class_name}");
                }
            }
            catch (Exception ex)
            {
                string strException = UtilPlay.getErro(ex);
                log = new List<LogPlay>();
                log.Add(new LogPlay() { MsgErro = strException, Status = "ERRO" });
                return Json(new { log });
            }

        }

        // Link => https://localhost:44369/PlugAndPlay/Consultas/IniciarConsulta?id=11
        public IActionResult IniciarConsulta(int? id)
        {
            string connection = _configuration.GetConnectionString("PlayConect");
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                Consultas consulta = db.Consultas.AsNoTracking()
                                        .Where(x => x.CON_ID == id)
                                        .Select(x => new Consultas
                                        {
                                            CON_ID = x.CON_ID,
                                            CON_TITULO = x.CON_TITULO,
                                            CON_DESCRICAO = x.CON_DESCRICAO,
                                            CON_COMAND = x.CON_COMAND
                                        }).FirstOrDefault();
                if (consulta != null)
                {
                    ViewBag.Id = consulta.CON_ID;
                    ViewBag.Titulo = consulta.CON_TITULO;
                    ViewBag.Descricao = consulta.CON_DESCRICAO;

                    List<string> parametros = SepararParametros(consulta.CON_COMAND);

                    List<string> descParametros = new List<string>();
                    List<string> nameParametros = new List<string>();
                    List<string> typeParametros = new List<string>();
                    foreach (var parametro in parametros)
                    {
                        if (parametro.StartsWith("@") && parametro.EndsWith("@") && parametro.Contains("#"))
                        {
                            string[] auxParam = parametro.Split("#");
                            descParametros.Add(auxParam[0].Replace("@", "").Trim());
                            nameParametros.Add(auxParam[1].Replace("@", "").Trim());
                            typeParametros.Add(auxParam[2].Replace("@", "").Trim());
                        }
                    }

                    ViewBag.qtdParametros = descParametros.Count;
                    ViewBag.descParametros = descParametros;
                    ViewBag.nameParametros = nameParametros;
                    ViewBag.typeParametros = typeParametros;
                }
                else
                {
                    ViewBag.MsgErro = "Consulta não encontrada!";
                }
            }
            return View();
        }

        private List<string> SepararParametros(string commandSql)
        {
            List<string> paramList = new List<string>();
            int inicio = 0, fim = 0;
            string strParam;
            for (int i = 0; i < commandSql.Length; i++)
            {
                if (commandSql.ElementAt(i) == '@')
                {
                    inicio = i;

                    int j = i + 1;
                    while (j < commandSql.Length && commandSql.ElementAt(j) != '@')
                    {
                        j++;
                    }

                    if (j < commandSql.Length)
                    {
                        fim = j;

                        strParam = commandSql.Substring(inicio, (fim - inicio) + 1);
                        paramList.Add(strParam);

                        i = fim;
                    }
                }
            }

            return paramList;
        }

        public IActionResult ExecutarConsulta(string idConsulta, string parametrosJSON)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                string msg = "";
                List<List<object>> resultadoQuery = ObterDadosDaQuery(db, ref msg, idConsulta, parametrosJSON);
                return Json(new { resultadoQuery, msg });
            }
        }

        private List<List<object>> ObterDadosDaQuery(JSgi db, ref string msg, string idConsulta, string parametrosJSON)
        {
            int id = int.Parse(idConsulta);
            List<List<object>> resultadoQuery = new List<List<object>>();

            Consultas consulta = db.Consultas.AsNoTracking()
                                    .Where(x => x.CON_ID == id)
                                    .Select(x => new Consultas
                                    {
                                        CON_ID = x.CON_ID,
                                        CON_TITULO = x.CON_TITULO,
                                        CON_DESCRICAO = x.CON_DESCRICAO,
                                        CON_COMAND = x.CON_COMAND
                                    }).FirstOrDefault();

            if (consulta != null && !String.IsNullOrEmpty(consulta.CON_COMAND))
            {
                string sql = consulta.CON_COMAND;
                bool deuProblema = false;

                if (!String.IsNullOrEmpty(parametrosJSON))
                {
                    var definicao = new[] { new { name = "", value = "" } };
                    var parametros = JsonConvert.DeserializeAnonymousType(parametrosJSON, definicao);

                    List<string> listParametros = SepararParametros(consulta.CON_COMAND);

                    if (listParametros.Count == parametros.Length)
                    {
                        for (int i = 0; i < listParametros.Count; i++)
                        {
                            if (listParametros[i].StartsWith("@") && listParametros[i].EndsWith("@") && listParametros[i].Contains("#"))
                            {
                                string nomeColuna = listParametros[i].Split("#")[1].Replace("@", "").Trim();
                                string typeColuna = listParametros[i].Split("#")[2].Replace("@", "").Trim();
                                if (nomeColuna == parametros[i].name)
                                {
                                    string value = parametros[i].value;
                                    if (!String.IsNullOrEmpty(value) && typeColuna.StartsWith("DATE"))
                                    {
                                        value = DateTime.Parse(value).ToString("yyyy-MM-dd HH:mm:ss.fff");

                                    }
                                    sql = sql.Replace(listParametros[i], value);
                                }
                                else
                                {
                                    i = listParametros.Count;
                                    deuProblema = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        deuProblema = true;
                        msg = "Parametros inválidos";
                    }
                }

                if (!deuProblema && !sql.Contains("@"))
                {// realizar consulta na base de dados
                    resultadoQuery = ExecutarQuery(db, sql, ref msg);
                }
                else if (sql.Contains("@"))
                {
                    msg = "A consulta sql cadastrada é inválida, pois os parâmetros não foram definidos corretamente";
                }
            }
            else
            {
                msg = "Consulta SQL não encontrada";
            }

            return resultadoQuery;
        }

        public List<List<object>> ExecutarQuery(JSgi db, string sql, ref string msg)
        {
            List<List<object>> resultadoQuery = new List<List<object>>();
            using (DbCommand command = db.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;
                    db.Database.OpenConnection();

                    using (DbDataReader result = command.ExecuteReader())
                    {

                        List<object> colunasQuery = new List<object>();
                        List<object> linhaQuery;
                        object valor;

                        int qtdColunas = result.FieldCount;

                        for (int i = 0; i < qtdColunas; i++)
                        {
                            colunasQuery.Add(result.GetName(i));
                        }
                        resultadoQuery.Add(colunasQuery);

                        while (result.Read())
                        {
                            linhaQuery = new List<object>();
                            for (int i = 0; i < qtdColunas; i++)
                            {
                                valor = result.GetValue(i);
                                if (valor.GetType().Name == nameof(DateTime))
                                {
                                    DateTime dateTime = (DateTime)valor;
                                    valor = dateTime.ToString("dd/MM/yyyy HH:mm:ss");
                                }
                                linhaQuery.Add(valor);
                            }

                            if (linhaQuery.Count > 0)
                            {
                                resultadoQuery.Add(linhaQuery);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    msg = $"Erro ao executar a consulta: {sql}";
                }
            }

            return resultadoQuery;
        }

        private string CriarArquivo(List<List<object>> resultadoQuery, ref string msg)
        {
            string downloadUrl = "";

            //https://www.c-sharpcorner.com/article/import-and-export-data-using-epplus-core/
            try
            {
                string rootFolder = _hostingEnvironment.WebRootPath;
                string fileName = $"ExportQuery-{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
                downloadUrl = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, fileName);

                FileInfo file = new FileInfo(Path.Combine(rootFolder, fileName));
                if (file.Exists)
                {
                    file.Delete();
                    file = new FileInfo(Path.Combine(rootFolder, fileName));
                }

                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                    for (int i = 0; i < resultadoQuery.Count(); i++)
                    {
                        List<object> linha = resultadoQuery[i];
                        for (int j = 0; j < linha.Count(); j++)
                        {
                            object coluna = linha[j];
                            worksheet.Cells[i + 1, j + 1].Value = coluna;
                        }
                    }
                    package.Save();
                }
            }
            catch (Exception ex)
            {
                msg = "Erro ao exportar os dados.";
            }

            return downloadUrl;
        }

        public IActionResult Exportar(string idConsulta, string parametrosJSON)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                string msg = "";
                string downloadUrl = "";
                List<List<object>> resultadoQuery = ObterDadosDaQuery(db, ref msg, idConsulta, parametrosJSON);
                if (msg == "")
                {
                    downloadUrl = CriarArquivo(resultadoQuery, ref msg);
                }

                return Json(new { msg, downloadUrl });
            }
        }

        public IActionResult ExecutaConsultaGrafico(int indId, string dimensao, string periodo, string subDim, string dataIni, string dataFim)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                Resultquery queryResult = new Resultquery();
                List<Parametro> aiparam = new List<Parametro>();
                string sql = "";
                string msg = "";

                using (DbCommand command = db.Database.GetDbConnection().CreateCommand())
                {
                    try
                    {
                        command.CommandText = $"select DIM_SQL from T_INDICADORES_DIMENCOES WHERE IND_ID = @IndId AND DIM_ID = @DimId";
                        command.CommandType = CommandType.Text;

                        DbParameter param = command.CreateParameter();
                        param.ParameterName = "@IndId";
                        param.Value = indId;
                        command.Parameters.Add(param);

                        param = command.CreateParameter();
                        param.ParameterName = "@DimId";
                        param.Value = dimensao;
                        command.Parameters.Add(param);

                        db.Database.OpenConnection();

                        using (DbDataReader result = command.ExecuteReader())
                        {
                            while (result.Read())
                            {
                                sql = result.GetValue(result.GetOrdinal("DIM_SQL")).ToString().ToUpper();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        msg = $"Erro ao executar a consulta: {sql}";
                    }
                }

                sql = sql.Replace("SELECT", "");
                int ifrom = 0;
                int iwhere = 0;
                for (int i = 0; i < sql.Length - 5; i++)
                {

                    if (sql.Substring(i, 4).ToUpper() == "FROM")
                    {
                        ifrom = i;
                        iwhere = sql.Length + 1;
                    }
                    if (sql.Substring(i, 5).ToUpper() == "WHERE")
                    {
                        iwhere = i;
                    }
                }
                string campos = sql.Substring(0, ifrom - 1);
                string tabela = sql.Substring(ifrom + 5, iwhere - 5 - ifrom);
                string[] sql_split = campos.Split(',');
                sql = "SELECT * FROM " + tabela;
                string where = "";
                //indId, dimensao, subDimensao, dataIni, dataFim, periodo
                if (!String.IsNullOrEmpty(subDim))
                    where += " WHERE " + sql_split[0] + "'" + subDim + "'";
                if (periodo.Trim() == "D")
                {
                    if (where.Length > 0)
                        where += " AND ";
                    else
                        where += " WHERE ";
                    where += sql_split[2] + " BETWEEN '" + dataIni.Substring(6, 4) + dataIni.Substring(3, 2) + dataIni.Substring(0, 2) + "' AND '" + dataFim.Substring(6, 4) + dataFim.Substring(3, 2) + dataFim.Substring(0, 2) + "'";
                }

                // montagem da query por parametro 
                sql += where;


                string downloadUrl = "";
                List<List<object>> resultadoQuery = ExecutarQuery(db, sql, ref msg);
                if (msg == "")
                {
                    downloadUrl = CriarArquivo(resultadoQuery, ref msg);
                }

                return Json(new { msg, downloadUrl });
            }
        }
    }
}

using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Models;
using DynamicForms.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Areas.PlugAndPlay.Controllers
{
    [Authorize]
    [Area("plugandplay")]
    public class DicionarioController : BaseController
    {
        Dictionary Dicionario;
        public DateTime DataInicial;
        public T_Usuario usuario_logado;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;


        public DicionarioController(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            Dicionario = Dictionary.GetInstance();
        }

        public ActionResult Index()
        {
            usuario_logado = ObterUsuarioLogado(); //Obtem somente informações básicas
            ViewBag.UserName = usuario_logado.USE_NOME;

            if (!ValidacoesUsuario.ValidarAcessoTela(usuario_logado, typeof(DicionarioController).FullName))
                return RedirectToAction("SemAcesso", "Acesso", new { area = "" });

            return View();
        }

        public IActionResult SepararChaveTabela(string tabelaNome, string sql, string entidadesJSON) // pega a chave primaria da tabela selecionada para poder fazer a consulta das tabelas com relação
        {
            string sqlNovo = "";
            List<string> entidades = JsonConvert.DeserializeObject<List<string>>(entidadesJSON);
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                string msg = "";
                string chavePK = "";
                using (DbCommand cmd = db.Database.GetDbConnection().CreateCommand())
                {
                    try
                    {  
                        //comando sql que pega chave primaria de uma tabela
                        cmd.CommandText = $"SELECT KU.table_name as TABLENAME ,column_name as PRIMARYKEYCOLUMN " +
                            $"FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC " +
                            $"INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KU " +
                            $"ON TC.CONSTRAINT_TYPE = 'PRIMARY KEY' " +
                            $"AND TC.CONSTRAINT_NAME = KU.CONSTRAINT_NAME " +
                            $"AND KU.table_name = @tabelaNOME " +
                            $"ORDER BY  KU.TABLE_NAME ,KU.ORDINAL_POSITION;";
                        cmd.CommandType = CommandType.Text;

                        DbParameter param = cmd.CreateParameter();
                        param.ParameterName = "@tabelaNOME";
                        param.Value = tabelaNome;
                        cmd.Parameters.Add(param);


                        db.Database.OpenConnection();
                        using (DbDataReader result = cmd.ExecuteReader())
                        {
                            while(result.Read())
                            {
                                chavePK = result.GetValue(result.GetOrdinal("PRIMARYKEYCOLUMN")).ToString().ToUpper();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                         msg = $"Erro ao executar a sql";
                    }
                }
                if(msg=="")
                {              
                   
                    List<string> sqlWhere = new List<string>();
                    for (int i = 1; i < entidades.Count; i++)
                    {
                        sqlWhere.Add( entidades[i] + "." + chavePK + " = " + entidades[0] + "." + chavePK);
                    }

                    sqlNovo = SepararParametros(sql, sqlWhere);

                }

                

                return Json(new { msg, sqlNovo });

            }
           
        }


        private string SepararParametros(string commandSql,List<string> sqlWhere)// separa parametros do where
        {
  
            List<string> paramList = new List<string>();
            int inicio = 0, fim = 0, x = 0;
            string novoSql = "";
            for(int i = 0; i < commandSql.Length; i++)
            {
                if(commandSql.ElementAt(i) == '@')
                {
                    inicio = i;
                    int j = i + 1;
                    while (j < commandSql.Length && commandSql.ElementAt(j) != '@')
                    {
                        j++;
                    }
                    if(j < commandSql.Length)
                    {
                       if(x != sqlWhere.Count)
                            novoSql += sqlWhere[x];

                        x++;
                        /*        
                            
                            strParam = commandSql.Substring(inicio, (fim - inicio) + 1);
                            paramList.Add(strParam);
                            
                        */
                        fim = j;
                        i = fim;
                    }
                }
                else
                {
                    novoSql += commandSql[i];
                }
            }

            return novoSql;
        }

        public IActionResult ConsultaDicionario()
        {
            usuario_logado = ObterUsuarioLogado(); //Obtem somente informações básicas
            ViewBag.UserName = usuario_logado.USE_NOME;

            if (!ValidacoesUsuario.ValidarAcessoTela(usuario_logado, typeof(DicionarioController).FullName))
                return RedirectToAction("SemAcesso", "Acesso", new { area = "" });

            return View();
        }

        public IActionResult ExecutarConsulta(string sql)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                string msg = "";
                List<List<object>> resultadoQuery = ObterDadosDaQuery(db, ref msg, sql);
                return Json(new { resultadoQuery, msg });
            }
        }

        private List<List<object>> ObterDadosDaQuery(JSgi db, ref string msg, string sql)
        {
            
            List<List<object>> resultadoQuery = new List<List<object>>();


            if (sql != null)
            {
                string _sql = sql;
                if (!_sql.Contains("@"))
                {// realizar consulta na base de dados
                    resultadoQuery = ExecutarQuery(db, _sql, ref msg);
                }
            }
            else
            {
                msg = "Consulta SQL não encontrada";
            }

            return resultadoQuery;
        }

        public List<List<object>> ExecutarQuery(JSgi db, string _sql, ref string msg)
        {
            List<List<object>> resultadoQuery = new List<List<object>>();
            using (DbCommand command = db.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    command.CommandText = _sql;
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
                    msg = $"Erro ao executar a consulta: {_sql}";
                }
            }

            return resultadoQuery;
        }

            public IActionResult listarColunas(string nomeTabela)
        {
            Entity EntidadeSelecionada = Dicionario.GetActualEntityByName(nomeTabela);
            
            return Json(EntidadeSelecionada.ActualColumns);
        }

        private string CriarArquivo(List<List<object>> resultadoQuery, ref string msg)
        {
            string downloadUrl = "";
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

        public IActionResult Exportar(string sql)
        {
            using(JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                string msg = "";
                string downloadUrl = "";
                 List<List<object>> resultadoQuery = ObterDadosDaQuery(db, ref msg, sql);
                if(msg == "")
                {
                    downloadUrl = CriarArquivo(resultadoQuery, ref msg);
                }

                return Json(new { msg, downloadUrl });
            }
        }


        public IActionResult listarRelacoes(string nomeTabela)
        {
            Entity EntidadeSelecionada = Dicionario.GetActualEntityByName(nomeTabela);

            if (EntidadeSelecionada.ActualRelations.Count > 0)
            {
                return Json(EntidadeSelecionada.ActualRelations);
            }
            else
                return Json("");

        }

        public IActionResult listarEntidadesCompleto()
        {
            return Json(Dicionario.ActualEntities);
        }
    }
}

using DynamicForms.Areas.SGI.Model;
using DynamicForms.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace DynamicForms.Areas.SGI.Utils
{
    public static class QueryAnaliser
    {
        public static List<ViewsCampos> GetCamposProcedure(string nome)
        {

            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                List<ViewsCampos> lstNomes = new List<ViewsCampos>();
                using (DbCommand command = db.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = @"EXEC " + nome;
                    command.CommandType = CommandType.Text;

                    db.Database.OpenConnection();
                    command.CommandTimeout = 99999;
                    try
                    {
                        DbDataReader result = command.ExecuteReader();
                        if (result.Read())
                        {
                            for (int i = 0; i < result.FieldCount; i++)
                            {
                                lstNomes.Add(new ViewsCampos() { Nome = result.GetName(i) });
                            }
                        }
                    }
                    catch (Exception erro)
                    {
                        throw new Exception("Erro ao executar procedure: " + erro.Message);
                    }
                }
                return lstNomes;
            }
        }

        public static string[,] GetValores(string tipo, string viewNome, string data1, string data2)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                data1 = DateTime.Parse(data1).ToString("yyyyMMdd");
                data2 = DateTime.Parse(data2).ToString("yyyyMMdd");
                string[,] dados = new string[0, 0];

                using (DbCommand command = db.Database.GetDbConnection().CreateCommand())
                {
                    if (tipo == "view")
                    {
                        command.CommandText = @"select * from " + viewNome;
                        command.CommandText += " where (DATA between @data1 and @data2) or (DATA = '') ";

                        DbParameter param = command.CreateParameter();
                        param.ParameterName = "@data1";
                        param.Value = data1;
                        command.Parameters.Add(param);

                        param = command.CreateParameter();
                        param.ParameterName = "@data2";
                        param.Value = data2;
                        command.Parameters.Add(param);
                    }
                    else
                    {
                        command.CommandText = @"EXEC " + viewNome;
                    }
                    command.CommandType = CommandType.Text;
                    db.Database.OpenConnection();
                    try
                    {
                        DbDataReader reader = command.ExecuteReader();

                        List<List<object>> resultadoQuery = new List<List<object>>();
                        List<object> linhaQuery;
                        while (reader.Read())
                        {
                            linhaQuery = new List<object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                linhaQuery.Add(reader[i].ToString());
                            }
                            if (linhaQuery.Count > 0)
                            {
                                resultadoQuery.Add(linhaQuery);
                            }
                        }

                        dados = new string[resultadoQuery.Count, reader.FieldCount];
                        for (int i = 0; i < resultadoQuery.Count; i++)
                        {
                            for (int j = 0; j < resultadoQuery[i].Count; j++)
                            {
                                dados[i, j] = resultadoQuery[i][j].ToString();
                            }
                        }
                    }
                    catch (Exception erro)
                    {
                        throw new Exception("Erro ao listar tipo embalagem: " + erro.Message);
                    }
                }
                return dados;
            }
        }

        public static int GetLinhas(string tipo, string viewNome, string data1, string data2)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                int linhas = 0;
                using (DbCommand command = db.Database.GetDbConnection().CreateCommand())
                {
                    if (tipo == "view")
                    {
                        command.CommandText = @"select COUNT(*)TOTAL from " + viewNome;
                        command.CommandText += " where (DATA between @data1 and @data2) or (DATA = '') ";

                        DbParameter param = command.CreateParameter();
                        param.ParameterName = "@data1";
                        param.Value = data1;
                        command.Parameters.Add(param);

                        param = command.CreateParameter();
                        param.ParameterName = "@data2";
                        param.Value = data2;
                        command.Parameters.Add(param);

                    }
                    else
                    {
                        command.CommandText = @"EXEC " + viewNome;
                    }
                    command.CommandType = CommandType.Text;

                    db.Database.OpenConnection();
                    try
                    {
                        DbDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if (tipo == "view")
                                linhas = Int32.Parse(reader["TOTAL"].ToString());
                            else
                                linhas++;
                        }
                    }
                    catch (Exception erro)
                    {
                        throw new Exception("Conta linhas views: " + erro.Message);
                    }


                }
                return linhas;
            }
        }

        /// <summary>
        /// Rotina para retornar valores referente ao ano Anterior
        /// </summary>
        /// <param name="ano">Ano atual</param>
        /// <param name="idMeta">Id Meta</param>
        /// <returns>Lista de dados do tipo vw_SGI_PARAMETRO_RELMEDICOES</returns>
        public static List<vw_SGI_PARAMETRO_RELMEDICOES> AnoAnterior(string ano, int idIndicador)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                var anoAtual = DateTime.Parse("01/01/" + ano);
                var valores = new List<vw_SGI_PARAMETRO_RELMEDICOES>();
                string query = "select * from vw_SGI_PARAMETRO_RELMEDICOES WHERE IND_ID = '" + idIndicador.ToString() + "' ";
                //Filtra ano atual
                if (ano != "" && ano != null)
                    query += "AND LEFT(MES,4) = '" + anoAtual.AddYears(-1).Year.ToString() + "' ";
                query += "order by IND_ID,Mes";
                valores = db.VW_SGI_PARAMETRO_RELMEDICOES.FromSql(query).ToList();
                return valores;
            }
        }

        /// <summary>
        /// Rotina para retornar valores referente ao ano Anterior
        /// </summary>
        /// <param name="ano">Ano atual</param>
        /// <returns>Lista de dados do tipo vw_SGI_PARAMETRO_RELMEDICOES</returns>
        public static List<vw_SGI_PARAMETRO_RELMEDICOES> AnoAnterior(string ano)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                var anoAtual = DateTime.Parse("01/01/" + ano);
                var valores = new List<vw_SGI_PARAMETRO_RELMEDICOES>();
                string query = "select * from vw_SGI_PARAMETRO_RELMEDICOES ";
                //Filtra ano atual
                query += "where LEFT(MES,4) = '" + anoAtual.AddYears(-1).Year.ToString() + "' ";
                query += "order by IND_ID,Mes";
                valores = db.VW_SGI_PARAMETRO_RELMEDICOES.FromSql(query).ToList();
                return valores;
            }
        }

    }
}

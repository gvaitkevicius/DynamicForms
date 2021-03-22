using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Context;
using DynamicForms.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DynamicForms.Interfaces
{
    public class MunicipiosI
    {
        public void ImportarMunicipios(ref List<LogPlay> log, int forceInsert, JSgi db)
        {
            try
            {
                Console.WriteLine("\n----------------------");
                Console.WriteLine("Importando municipios...");
                var stopwatch = new Stopwatch();
                Console.WriteLine($"Atualizando municipios na base dadados...");
                stopwatch.Start();
                var linhasAlteradas = db.Database.ExecuteSqlCommand("UPDATE T_MUNICIPIOS SET T_MUNICIPIOS.MUN_ID_INTEGRACAO_ERP = V_INPUT_T_MUNICIPIOS.MUN_ID_INTEGRACAO_ERP  " +
                " FROM T_MUNICIPIOS INNER JOIN V_INPUT_T_MUNICIPIOS ON UPPER(T_MUNICIPIOS.UF_COD) = UPPER(V_INPUT_T_MUNICIPIOS.UF_COD) COLLATE Latin1_General_CI_AS " +
                " AND T_MUNICIPIOS.MUN_NOME COLLATE Latin1_General_CI_AS = UPPER(V_INPUT_T_MUNICIPIOS.MUN_NOME COLLATE Latin1_General_CI_AS) " +
                " WHERE T_MUNICIPIOS.MUN_ID_INTEGRACAO_ERP IS NULL OR T_MUNICIPIOS.MUN_ID_INTEGRACAO_ERP = ''");
                stopwatch.Stop();
                Console.WriteLine($"Fim da Atualizacao dos municipios: {stopwatch.Elapsed}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Falha na importacao de municipios: ");
                Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                log.Add(new LogPlay(new Order(), "ERRO UPDATE T_MUNICIPIOS", UtilPlay.getErro(ex)));
                return;
            }

        }
    }
    public class V_INPUT_T_MUNICIPIOS
    {

        public string MUN_ID { get; set; }
        public string MUN_ID_INTEGRACAO_ERP { get; set; }
        public string MUN_NOME { get; set; }
        public string UF_COD { get; set; }
        public string MUN_CODIGO_IBGE { get; set; }
        public double MUN_LATITUDE { get; set; }
        public double MUN_LONGITUDE { get; set; }

        public Municipio ToMunicipio()
        {
            Municipio o = new Municipio
            {
                MUN_ID = this.MUN_ID,
                MUN_ID_INTEGRACAO_ERP = this.MUN_ID_INTEGRACAO_ERP,
                MUN_NOME = this.MUN_NOME,
                UF_COD = this.UF_COD,
                MUN_CODIGO_IBGE = this.MUN_CODIGO_IBGE,
                MUN_LATITUDE = this.MUN_LATITUDE,
                MUN_LONGITUDE = this.MUN_LONGITUDE,
            };
            return o;
        }
        public V_INPUT_T_MUNICIPIOS()
        {

        }
    }
}

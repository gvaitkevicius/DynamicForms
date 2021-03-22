using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Context;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DynamicForms.Controllers
{
    public static class TestesDesempenho
    {
        public static string TesteInsertMaquina()
        {
            MasterController mc = new MasterController();
            var stopwatch = new Stopwatch();
            Maquina maquina;
            using (JSgi _context = new ContextFactory().CreateDbContext(new string[] { }))
            {
                maquina = _context.Maquina.AsNoTracking().FirstOrDefault();
            }

            List<Maquina> maquinas = new List<Maquina>();
            for (int i = 0; i < 10; i++)
            {
                //maquinas.Add(new Maquina()
                //{
                //    Id = "TesteInsert-" + i,
                //    Descricao = "DescricaoMaquina",
                //    ControlIp = maquina.ControlIp,
                //    CalendarioId = maquina.CalendarioId,
                //    Sirene = maquina.Sirene,
                //    CorSemafaro = maquina.CorSemafaro,
                //    TipoContador = maquina.TipoContador,
                //    GrupoMaquinaId = maquina.GrupoMaquinaId,
                //    MaqIdMaqPai = maquina.MaqIdMaqPai,
                //    QtdCores = maquina.QtdCores,
                //    TempoMinimoDeParada = maquina.TempoMinimoDeParada,
                //    CongelaFila = maquina.CongelaFila,
                //    FilaId = maquina.FilaId,
                //    MAQ_ULTIMA_ATUALIZACAO = DateTime.Now,
                //    PlayAction = "insert"
                //});
            }

            string json = JsonConvert.SerializeObject(maquinas);
            string[] vet_json = new string[] { json };

            string[] vet_classe = new string[] { "DynamicForms.Areas.PlugAndPlay.Models.Maquina" };
            List<string[]> list_classes = new List<string[]>() { vet_classe };

            stopwatch.Start();
            // List<LogPlay> logs = mc.UpdateData(vet_json, list_classes, 0, false);
            stopwatch.Stop();
            // List<LogPlay> logs_erros = new LogPlay().GetLogsErro(logs);

            string time = $"Tempo passado: {stopwatch.Elapsed}";
            return time;
        }

        public static string TesteInsertGrupoMaquina()
        {
            MasterController mc = new MasterController();
            var stopwatch = new Stopwatch();

            List<GrupoMaquina> grupoMaquinas = new List<GrupoMaquina>();
            for (int i = 0; i < 100; i++)
            {
                grupoMaquinas.Add(
                    new GrupoMaquina()
                    {
                        GMA_ID = "GrupoMaquina-" + i,
                        GMA_DESCRICAO = "DescricaoGrupoMaquina",
                        PlayAction = "insert"
                    });
            }

            string json = JsonConvert.SerializeObject(grupoMaquinas);
            string[] vet_json = new string[] { json };

            string[] vet_classe = new string[] { "DynamicForms.Areas.PlugAndPlay.Models.GrupoMaquina" };
            List<string[]> list_classes = new List<string[]>() { vet_classe };

            stopwatch.Start();
            //List<LogPlay> logs = mc.UpdateData(vet_json, list_classes, 0, true);
            stopwatch.Stop();

            string time = $"Tempo passado: {stopwatch.Elapsed}";
            return time;
        }

        public static string TesteUpdateGrupoMaquina(JSgi _context)
        {
            MasterController mc = new MasterController();
            var stopwatch = new Stopwatch();

            Maquina maquina = _context.Maquina.AsNoTracking().FirstOrDefault();
            List<GrupoMaquina> grupo_maquinas = new List<GrupoMaquina>();
            grupo_maquinas = _context.GrupoMaquina
                .Where(gp => gp.GMA_DESCRICAO == "DescricaoGrupoMaquina")
                .Include(gp => gp.Maquinas)
                .ToList();

            for (int i = 0; i < grupo_maquinas.Count; i++)
            {
                GrupoMaquina grupo_maquina = grupo_maquinas[i];
                List<Maquina> maquinas = null; //grupo_maquina.Maquinas;

                // Teste Insert Maquina ***************
                //maquinas.Add(new Maquina()
                //{
                //    Id = "MaquinaI-" + i,
                //    Descricao = "MaquinaI",
                //    ControlIp = maquina.ControlIp,
                //    CalendarioId = maquina.CalendarioId,
                //    Sirene = maquina.Sirene,
                //    CorSemafaro = maquina.CorSemafaro,
                //    TipoContador = maquina.TipoContador,
                //    MaqIdMaqPai = maquina.MaqIdMaqPai,
                //    QtdCores = maquina.QtdCores,
                //    TempoMinimoDeParada = maquina.TempoMinimoDeParada,
                //    CongelaFila = maquina.CongelaFila,
                //    FilaId = maquina.FilaId,
                //    PlayAction = "insert"
                //});


                // Teste Update Maquina * **************
                for (int j = 0; j < maquinas.Count; j++)
                {
                    if (j < 2)
                    {
                        //maquinas[j].Descricao = "MaquinaUpdate";
                    }
                }

                // Teste Delete Maquina  ***************
                maquinas.RemoveAt(3);

                // Teste Update GrupoMaquina
                if (i < 50)
                {
                    grupo_maquina.GMA_DESCRICAO = "GrupoMaquinaUpdate";
                }

                grupo_maquina.PlayAction = "update";
            }

            string json = JsonConvert.SerializeObject(grupo_maquinas, Formatting.Indented,
                new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });

            string[] vet_json = new string[] { json };

            string[] vet_classe = new string[] { "GrupoMaquina", "Maquina" };
            List<string[]> list_classes = new List<string[]>() { vet_classe };

            stopwatch.Start();
            //mc.UpdateData(vet_json, list_classes, 0, true);
            stopwatch.Stop();

            string time = $"Tempo passado: {stopwatch.Elapsed}";
            return time;
        }

        public static string TesteDeleteGrupoMaquina(JSgi _context)
        {
            MasterController mc = new MasterController();
            var stopwatch = new Stopwatch();

            List<GrupoMaquina> grupo_maquinas = _context.GrupoMaquina
                .Where(gp => gp.GMA_DESCRICAO == "DescricaoGrupoMaquina")
                .AsNoTracking()
                .Include(gp => gp.Maquinas)
                .ToList();

            for (int i = 0; i < 20; i++)
            {
                grupo_maquinas[i].PlayAction = "delete";
            }

            string json = JsonConvert.SerializeObject(grupo_maquinas, Formatting.Indented,
                new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });

            string[] vet_json = new string[] { json };

            string[] vet_classe = new string[] { "GrupoMaquina" };
            List<string[]> list_classes = new List<string[]>() { vet_classe };

            stopwatch.Start();
            //mc.UpdateData(vet_json, list_classes, 0, true);
            stopwatch.Stop();

            string time = $"Tempo passado: {stopwatch.Elapsed}";
            return time;
        }
    }
}

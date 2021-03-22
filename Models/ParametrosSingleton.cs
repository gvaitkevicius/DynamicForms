using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Context;
using Microsoft.EntityFrameworkCore;
using OptMiddleware;
using OptShered;
using OptTransport;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicForms.Models
{
    public sealed class ParametrosSingleton
    {
        private static ParametrosSingleton _instance = null;

        public T_Usuario Usuario { get; set; }
        public DateTime DataBase { get; set; } // inicializar com hora 0000000000
        public double tempoAdicionalDiaTurma { get; set; }
        public int TimOut { get; set; }
        public bool cancelar_interface { get; set; }
        public string Armazem { get; set; }
        public string semaforoInterface { get; set; }
        public string semaforoOtimizador { get; set; }
        public string PercentualOpt { get; set; }
        public string FasePercentualOpt { get; set; }
        public DateTime proxima_exec_interface { get; set; }
        public DateTime proxima_exec_otimizador { get; set; }
        public DateTime ultima_exec_interface { get; set; }
        public DateTime ultima_exec_otimizador { get; set; }

        public Impressora ImpressoraPadrao { get; set; }
        public List<Mensagem> Menssagens { get; set; }

        private ParametrosSingleton()
        {
            this.Armazem = "01";
            this.tempoAdicionalDiaTurma = 2;
            this.DataBase = new DateTime(1970, 01, 01);// Data inicial minima para o sistema
            this.TimOut = 10000;
            this.semaforoInterface = "";
            this.semaforoOtimizador = "";
            this.ImpressoraPadrao = new Impressora() { IMP_ID = 3 };
            this.Menssagens = new List<Mensagem>();
        }
        public static bool MsgSingleton(string tipo, string msg)
        {
            if (OPTParametrosSingleton.Instance.Cancelar)
                return true;

            ParametrosSingleton.Instance.PercentualOpt = msg;
            ParametrosSingleton.Instance.FasePercentualOpt = tipo;
            var msgInterface = new DynamicForms.Areas.PlugAndPlay.Models.Mensagem
            {
                MEN_TYPE = tipo,
                MEN_SEND = msg,
                MEN_EMISSION = DateTime.Now
            };

            ParametrosSingleton.Instance.Menssagens.Add(msgInterface);

            return false;
        }

        public static ParametrosSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (typeof(ParametrosSingleton))
                        _instance = new ParametrosSingleton();
                }
                return _instance;
            }
        }

        public static string DiaTurmaS()
        {
            DateTime data = DateTime.Now;//ParametrosSingleton.Instance.DataBase;
            //double tempoAdicional = db.Parametros.Find("HORA_DIA_TURMA").PAR_VALOR_N;
            data = data.AddHours(ParametrosSingleton.Instance.tempoAdicionalDiaTurma);
            return data.ToString("yyyyMMdd");
        }

        public static void LoadOPTSingleton()
        {

            if (OPTParametrosSingleton.Instance.Parametro_sheOptQueueTransport == null ||
                    OPTParametrosSingleton.Instance.Parametro_sheOptQueueTransport.Parametros == null ||
                    OPTParametrosSingleton.Instance.Parametro_sheOptQueueTransport.Parametros.Count() == 0)
            {
                using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
                {
                    List<OrderOpt> listaPedidos = new List<OrderOpt>();
                    OrderOpt tempOrder = db.OrderOpt.AsNoTracking().FirstOrDefault();

                    listaPedidos.Add(tempOrder);
                    sheOptQueueBox b = null;
                    OptQueueTransport t_Otimizado = null;

                    OPTMiddleware run = new OPTMiddleware(DateTime.Now, 1, "", 2, b, OPTParametrosSingleton.Instance.Parametro_sheOptQueueTransport, listaPedidos, db, ref t_Otimizado);
                }
            }

        }

    }
}

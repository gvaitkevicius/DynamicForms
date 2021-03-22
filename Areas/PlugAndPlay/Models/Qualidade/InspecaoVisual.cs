using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Models;
using DynamicForms.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class InspecaoVisual
    {
        [READ] [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo IPV_ID requirido.")] public int IPV_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VALOR")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo IPV_VALOR")] public string IPV_VALOR { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID_OPERADOR")] public int? IPV_ID_OPERADOR { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID_LIBERACAO")] public int? IPV_ID_LIBERACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OBS")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo IPV_OBS")] public string IPV_OBS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_COLETA")] public DateTime IPV_DATA_COLETA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_AVAL")] public DateTime IPV_DATA_AVAL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VALOR_MEDIDA")] public double? IPV_VALOR_MEDIDA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] public int? TIV_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "_ID")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo TURN_ID")] public string TURN_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "_ID")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo TURM_ID")] public string TURM_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRO_ID")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo ROT_PRO_ID")] public string ROT_PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "MAQ_ID")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo ROT_MAQ_ID")] public string ROT_MAQ_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ_TRANSFORMACAO")] public int? ROT_SEQ_TRANSFORMACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ_REPETICAO")] public int? FPR_SEQ_REPETICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "STATUS_LIBERACAO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo IPV_STATUS_LIBERACAO")] public string IPV_STATUS_LIBERACAO { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        public virtual TipoInspecaoVisual TipoInspecaoVisual { get; set; }

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) { return true; }
        public void SalvarInspeçãoVisual(string idOrd, string idPro, string idMaq, string seqRep, string seqTrans, String Turno, String Turma, int User, string[] qtd, String[] idVisual)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                int idAux = 0;
                List<int> quantidade = new List<int>();
                List<int> id = new List<int>();
                DateTime _dataAux = DateTime.Now;
                int cont = 0;

                foreach (var item in qtd) //Covertendo Lista de Ids dos Tipos em Int
                {
                    int.TryParse(item, out idAux);
                    if (idAux != 0)
                        quantidade.Add(idAux);
                }

                foreach (var item in idVisual) //Covertendo Lista de Ids dos Tipos em Int
                {
                    int.TryParse(item, out idAux);
                    if (idAux != 0)
                        id.Add(idAux);
                }

                MasterController mc = new MasterController();
                List<object> listaItem = new List<object>();

                InspecaoVisual insp = new InspecaoVisual();

                var tipo = db.TipoInspecaoVisual.AsNoTracking().Where(x => id.Contains(x.TIV_ID)).Select(x => new { x.TIV_ID }).ToList();//Obtendo todos os Tipos de testes Visuais 
                foreach (var item in tipo)
                {
                    for (int i = 0; i < quantidade[cont]; i++)
                    {

                        insp.IPV_ID_OPERADOR = User;
                        insp.IPV_ID_LIBERACAO = 0;
                        insp.IPV_OBS = "";
                        insp.IPV_DATA_COLETA = _dataAux;
                        insp.TIV_ID = id[cont]; // ID do tipo da inspecao
                        insp.TURN_ID = Turno;
                        insp.TURM_ID = Turma;
                        insp.ORD_ID = idOrd;
                        insp.ROT_PRO_ID = idPro;
                        insp.ROT_MAQ_ID = idMaq;
                        insp.ROT_SEQ_TRANSFORMACAO = Convert.ToInt32(seqTrans);
                        insp.FPR_SEQ_REPETICAO = Convert.ToInt32(seqRep);
                        insp.IPV_STATUS_LIBERACAO = "AMOSTRA_COLETADA";
                        insp.IPV_VALOR_MEDIDA = 0;
                        insp.PlayAction = "insert";


                        listaItem.Add(insp);
                        insp = new InspecaoVisual();
                    }
                    cont++;
                }
                List<List<object>> ListOfListObjects = new List<List<object>>() { listaItem };
                List<LogPlay> logs = mc.UpdateData(ListOfListObjects, 0, true);//Persintindo Objetos

            }
        }

        public int AvaliarSequenciaInspecao(string[] testeid, string[] testevalor)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(Array.Empty<string>()))
            {
                string status = "";
                int idAux = 0;
                List<int> TestesIds = new List<int>();
                List<double> ValoresTestes = new List<double>();
                double resultado = 0, auxItem = 0;

                foreach (var item in testeid)
                {
                    int.TryParse(item, out idAux);
                    if (idAux != 0)
                        TestesIds.Add(idAux);
                }

                foreach (var item in testevalor)
                {
                    if (!String.IsNullOrEmpty(item))
                    {
                        double.TryParse(item, out auxItem);
                        ValoresTestes.Add(auxItem);
                    }
                }

                var Db_Insp = db.InspecaoVisual
                   .Include(t => t.TipoInspecaoVisual)
                   .AsNoTracking()
                   .Where(x => TestesIds.Contains(x.IPV_ID))
                   .FirstOrDefault();

                if (Db_Insp != null && ValoresTestes.Count > 0 && Db_Insp.TipoInspecaoVisual.TIV_MEDIDA.Equals("S"))
                {
                    resultado = ValoresTestes.Average();

                    status = resultado >= (Db_Insp.TipoInspecaoVisual.TIV_ESPECIFICACAO - Db_Insp.TipoInspecaoVisual.TIV_TOL_MENOS) && resultado <= (Db_Insp.TipoInspecaoVisual.TIV_ESPECIFICACAO + Db_Insp.TipoInspecaoVisual.TIV_TOL_MAIS) ? "APROVADO" : "REPROVADO";
                }
                else
                {
                    foreach (var item_v in testevalor)
                    {
                        if (item_v != null && item_v.Equals("OK"))
                            status = "APROVADO";
                        else
                            status = "REPROVADO";
                    }
                }

                return (status.Equals("APROVADO")) ? 1 : 0;
            }
        }
    }
}

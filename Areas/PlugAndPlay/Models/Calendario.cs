using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Models;
using DynamicForms.Util;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "CALENDÁRIOS")]
    public class Calendario
    {
        public Calendario()
        {
            IndisponibilidadeCliche = new HashSet<V_DISPONIBILIDADE_CLICHE>();
            IndisponibilidadeFaca = new HashSet<V_DISPONIBILIDADE_FACA>();
            IntensCalendario = new HashSet<ItensCalendario>();
            Maquinas = new HashSet<Maquina>();
        }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD CALENDÁRIO")] [Required(ErrorMessage = "Campo CAL_ID requirido.")] [Range(1, 99999)] public int CAL_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO")] [Required(ErrorMessage = "Campo CAL_DESCRICAO requirido.")] public string CAL_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DIVISÃO DO DIA EM")] public int? CAL_DIVIDE_DIA_EM { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  } 

        public virtual ICollection<ItensCalendario> IntensCalendario { get; set; }
        public virtual ICollection<V_DISPONIBILIDADE_FACA> IndisponibilidadeFaca { get; set; }
        public virtual ICollection<V_DISPONIBILIDADE_CLICHE> IndisponibilidadeCliche { get; set; }
        public virtual ICollection<Maquina> Maquinas { get; set; }
        //Metodos de Classe
        [Display(Name ="Inserir Itens Calendário")]
        public bool Inserir_Itens_Calendario(List<object> objects, ref List<LogPlay> Logs)
        {
            List<object> ObjetosProcessados = new List<object>();
            List<List<object>> ListObjectsToUpdate = new List<List<object>>();
            MasterController mc = new MasterController();
            //Criando um objeto para a nova carga
            int CalId = -1;
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                //Para cada item da lista
                foreach (var item in objects)
                {
                    Calendario _Calendario = (Calendario)item;
                    CalId = _Calendario.CAL_ID;
                    _Calendario.PlayAction = "OK";
                    ObjetosProcessados.Add(_Calendario);
                }
            }
            ListObjectsToUpdate.Add(ObjetosProcessados);
            //Concatenando Logs por se tratar de um objeto de interface
            Logs.Add(new LogPlay(this.ToString(), "PROTOCOLO", "LINK", "/PlugAndPlay/ItensCalendario/CadastrarItensCalendario?CalendarioId=", "" + CalId + ""));
            Logs.AddRange(mc.UpdateData(ListObjectsToUpdate, 4, true));

            return true;
        }
        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            List<object> Calendarios = new List<object>();
            List<List<object>> ListOfListObjects = new List<List<object>>();

            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (var item in objects)
                {
                    //Objeto retornado da View
                    Calendario _Calendario = (Calendario)item;
                    //Validaçoes --
                    if (_Calendario.PlayAction == "delete")
                    {
                        _Calendario.PlayAction = "OK";
                        Calendario Db_calendario = db.Calendario.Where(x => x.CAL_ID == _Calendario.CAL_ID).FirstOrDefault();
                        Db_calendario.PlayAction = "delete";
                        var Db_Itens_Calendario = db.ItensCalendario.Where(ic => ic.CAL_ID == _Calendario.CAL_ID).ToList();
                        var Db_Maquinas = db.Maquina.Where(m => m.CAL_ID == _Calendario.CAL_ID).ToList();

                        foreach (var itemM in Db_Maquinas)
                        {
                            itemM.CAL_ID = null;
                            itemM.PlayAction = "update";
                        }
                        foreach (var itemC in Db_Itens_Calendario)
                        {
                            itemC.PlayAction = "delete";
                        }
                        Calendarios.AddRange(Db_Maquinas);
                        Calendarios.AddRange(Db_Itens_Calendario);
                        Calendarios.Add(Db_calendario);
                    }
                }

            }
            objects.AddRange(Calendarios);
            return true;
        }
    }
}

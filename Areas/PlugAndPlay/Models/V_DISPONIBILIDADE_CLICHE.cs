using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Models;
using DynamicForms.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "DISPONIBILIDADE DE CLICHE")]
    public class V_DISPONIBILIDADE_CLICHE
    {
        [READ] [TAB(Value = "PRINCIPAL")] [Display(Name = "COD ITENS CALEND")] public int ICA_ID { get; set; }
        
        [SEARCH] [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA INICIO")] [Required(ErrorMessage = "Campo ICA_DATA_DE requirido.")] public DateTime ICA_DATA_DE { get; set; }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA FIM")] [Required(ErrorMessage = "Campo ICA_DATA_ATE requirido.")] public DateTime ICA_DATA_ATE { get; set; }
        
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OBSERVACAO")] [MaxLength(200, ErrorMessage = "")] public string ICA_OBSERVACAO { get; set; }

        [Combobox(Description = "Expediente Normal", Value = "1")]
        [Combobox(Description = "Sem Expediente de trabalho", Value = "2")]
        [Combobox(Description = "Folga", Value = "3")]
        [Combobox(Description = "Feriado", Value = "4")]
        [Combobox(Description = "Troca de Feriado", Value = "5")]
        [Combobox(Description = "Ferias Coletivas", Value = "6")]
        [Combobox(Description = "O", Value = "7")]
        [Combobox(Description = "Indisponível", Value = "8")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO")] [Required(ErrorMessage = "Campo ICA_TIPO requirido.")] public int ICA_TIPO { get; set; }

        [READ] [TAB(Value = "PRINCIPAL")] [Display(Name = "COD CALENDÁRIO")] [Range(1, 99999)] public int CAL_ID { get; set; }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PRODUTO")] public string PRO_ID { get; set; }
        
        [NotMapped] public string PlayAction { get; set; }
        
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        
        [NotMapped] public int? IndexClone { get; set; }
        
        public virtual ProdutoCliches ProdutoCliches { get; set; }
        public virtual Calendario Calendario{ get; set; }

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            bool check = true;

            foreach (var item in objects)
            {
                V_DISPONIBILIDADE_CLICHE disp_cliche = (V_DISPONIBILIDADE_CLICHE)item;
                if(disp_cliche.PlayAction.ToUpper() == "UPDATE" || disp_cliche.PlayAction.ToUpper() == "INSERT")
                {
                    CriarNovoCalendarioDisponibilidade(ref check);

                    //se estiver tudo certo, define o CAL_ID
                    if (check) { 
                        disp_cliche.CAL_ID = 100;
                    }
                }

                //se por algum motivo nao conseguiu definir o CAL_ID, retorna falso
                if (disp_cliche.CAL_ID != 100)
                    check = false;
            }

            return check;
        }

        [HIDDEN]
        private void CriarNovoCalendarioDisponibilidade(ref bool check)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                //se o calendário 100 nao existir, cria ele
                if(!db.Calendario.Where(x => x.CAL_ID == 100).Any())
                {
                    //cria um novo calendário
                    Calendario novo_calendario = new Calendario()
                    {
                        CAL_ID = 100,
                        CAL_DESCRICAO = "DISPONIBILIDADE",
                        CAL_DIVIDE_DIA_EM = 0,
                        PlayAction = "INSERT"
                    };

                    MasterController mc = new MasterController();
                    List<object> list = new List<object>() { novo_calendario };
                    List<List<object>> list_of_list = new List<List<object>>() { list };

                    //tenta adicionar o calendario no banco em um try catch
                    try
                    {
                        mc.UpdateData(list_of_list, 0, true);
                    }
                    catch (Exception ex)
                    {
                        check = false;
                    }
                }
            }
        }
    }
}

using DynamicForms.Models;
using DynamicForms.Util;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class Consultas
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] public int? CON_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo CON_DESCRICAO")] public string CON_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "GRUPO")] [MaxLength(20, ErrorMessage = "Maximode 20 caracteres, campo CON_GRUPO")] public string CON_GRUPO { get; set; }
        [TextArea] [TAB(Value = "PRINCIPAL")] [Display(Name = "COMAND")] public string CON_COMAND { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CONEXAO")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo CON_CONEXAO")] public string CON_CONEXAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TITULO")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo CON_TITULO")] public string CON_TITULO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo CON_TIPO")] public string CON_TIPO { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        // public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) { return true; }

        public bool IniciarConsulta(List<object> objects, ref List<LogPlay> Logs)
        {
            string parametros = "";
            if (objects.Count > 1)
            {
                string[] obj = (string[])objects[1];
                parametros = "&";

                foreach (var item in obj)
                {
                    parametros += item + "&";
                }

                if (parametros.Length > 1)
                {
                    parametros = parametros.Substring(0, parametros.Length - 1);
                }
            }


            foreach (var item in objects)
            {
                if (item.GetType().Name == "Consultas")
                {
                    Consultas consulta = (Consultas)item;
                    Logs.Add(new LogPlay(this.ToString(), "PROTOCOLO", "LINK", "/PlugAndPlay/Consultas/IniciarConsulta?id=", "" + consulta.CON_ID + parametros));
                }
            }
            return true;
        }

    }
}

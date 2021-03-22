using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class T_AGENDA_SCHEDULE
    {
        public int AGE_ID { get; set; }
        public DateTime AGE_DATA_ESPECIFICA { get; set; }
        public TimeSpan AGE_HORARIO_INICIO { get; set; }
        public TimeSpan AGE_HORARIO_FIM { get; set; }
        public string AGE_SEGUNDA { get; set; }
        public string AGE_TERCA { get; set; }
        public string AGE_QUARTA { get; set; }
        public string AGE_QUINTA { get; set; }
        public string AGE_SEXTA { get; set; }
        public string AGE_SABADO { get; set; }
        public string AGE_DOMINGO { get; set; }
        public double AGE_INTERVALO { get; set; }
        public string AGE_ORDEM_EXECUCAO { get; set; }
        public string AGE_PARAMETROS { get; set; }
        public string AGE_EXCECAO { get; set; }
        public string AGE_DESCRICAO { get; set; }

        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string IndexClone { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs, ref int modo_insert) {  } 
    }
}

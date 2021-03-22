using DynamicForms.Models;
using DynamicForms.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class ResultLote
    {
        public ResultLote()
        {
            TesteFisico = new HashSet<TesteFisico>();
        }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo RL_ID requirido.")] public int RL_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TD_DEF_GRAVE")] [Required(ErrorMessage = "Campo RL_QTD_DEF_GRAVE requirido.")] public int RL_QTD_DEF_GRAVE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TD_DEF_CRITICO")] [Required(ErrorMessage = "Campo RL_QTD_DEF_CRITICO requirido.")] public int RL_QTD_DEF_CRITICO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "STATUS")] [Required(ErrorMessage = "Campo RL_STATUS requirido.")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo RL_STATUS")] public string RL_STATUS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VALOR_ENCONTRADO")] [Required(ErrorMessage = "Campo RL_VALOR_ENCONTRADO requirido.")] public double RL_VALOR_ENCONTRADO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_LIBERACAO")] public DateTime RL_DATA_LIBERACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NOME_LIBERACAO")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo RL_NOME_LIBERACAO")] public string RL_NOME_LIBERACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OBS")] [MaxLength(255, ErrorMessage = "Maximode 255 caracteres, campo RL_OBS")] public string RL_OBS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LIBERADO")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo RL_LIBERADO")] public string RL_LIBERADO { get; set; }

        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        public ICollection<TesteFisico> TesteFisico { get; set; }
        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) { return true; }
    }
}




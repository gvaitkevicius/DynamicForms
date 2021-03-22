using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DynamicForms.Context;
using DynamicForms.Models;
using Microsoft.EntityFrameworkCore;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "PARÂMETROS")]
    public class Param
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PARAMETROS")] [Required(ErrorMessage = "Campo PAR_ID requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PAR_ID")] public string PAR_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRIÇÃO")] [Required(ErrorMessage = "Campo PAR_DESCRICAO requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PAR_DESCRICAO")] public string PAR_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VALOR_S")] [Required(ErrorMessage = "Campo PAR_VALOR_S requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PAR_VALOR_S")] public string PAR_VALOR_S { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VALOR_N")] [Required(ErrorMessage = "Campo PAR_VALOR_N requirido.")] public double PAR_VALOR_N { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VALOR_D")] [Required(ErrorMessage = "Campo PAR_VALOR_D requirido.")] public DateTime PAR_VALOR_D { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  } 
        public bool AddParametro(JSgi db, Param p)
        {
            Param Par = db.Param.Find(p.PAR_ID);
            if (Par == null)
            {
                Par.PAR_ID = p.PAR_ID;
                Par.PAR_DESCRICAO = p.PAR_DESCRICAO;
                Par.PAR_VALOR_S = p.PAR_VALOR_S;
                Par.PAR_VALOR_N = p.PAR_VALOR_N;
                db.Param.Add(Par);
            }
            else
            {
                db.Entry(Par).State = EntityState.Modified;
                Par.PAR_ID = p.PAR_ID;
                Par.PAR_DESCRICAO = p.PAR_DESCRICAO;
                Par.PAR_VALOR_S = p.PAR_VALOR_S;
                Par.PAR_VALOR_N = p.PAR_VALOR_N;
            }
            db.SaveChanges();
            return true;
        }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DynamicForms.Context;
using DynamicForms.Models;
using Microsoft.EntityFrameworkCore;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class Mensagem
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD MENSAGEM")] [Required(ErrorMessage = "Campo MEN_ID requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MEN_ID")] public string MEN_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ENVIAR")] [MaxLength(8000, ErrorMessage = "Maximode * caracteres, campo MEN_SEND")] public string MEN_SEND { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "EMISSÃO")] public DateTime MEN_EMISSION { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "STATUS")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MEN_STATUS")] public string MEN_STATUS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "MENSAGEM RECIBIDA")] [MaxLength(8000, ErrorMessage = "Maximode * caracteres, campo MEN_RECEIVE")] public string MEN_RECEIVE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO MENSAGEM")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MEN_TYPE")] public string MEN_TYPE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE ENVIAR")] public double? MEN_QTD_TRY_SEND { get; set; }
        public DateTime MEN_DATE_TRY_SEND { get; set; }

        public bool AddMensagem(JSgi db, Mensagem m)
        {
            Mensagem Men = null;
            Men = db.Mensagem.Find(m.MEN_ID);
            if (Men == null)
            {
                m.MEN_EMISSION = DateTime.Now;
                db.Mensagem.Add(m);
            }
            else
            {
                db.Entry(Men).State = EntityState.Modified;
                Men.MEN_EMISSION = DateTime.Now;
                Men.MEN_SEND = m.MEN_SEND;
                Men.MEN_STATUS = m.MEN_STATUS;
                Men.MEN_RECEIVE = m.MEN_RECEIVE;
                Men.MEN_TYPE = m.MEN_TYPE;
                Men.MEN_DATE_TRY_SEND = DateTime.Now;
                Men.MEN_QTD_TRY_SEND = 0;

                db.SaveChanges();
            }
            db.SaveChanges();
            m = null;
            Men = null;
            return true;
        }

        /*        internal void AddMensagem(Mensagem m)
                {
                    throw new NotImplementedException();
                }*/

        /// <summary>
        /// Esta propriedade foi criada para representar o estado do objeto
        /// insert, update, delete ou unchanged 
        /// </summary>
        [NotMapped]
        public string PlayAction { get; set; }

        /// <summary>
        /// Deve seguir a seguinte convecão: NameProperty:MsgErro;NameProperty:MsgErro; ...
        /// Representa os erros de validacao deste objeto
        /// </summary>
        [NotMapped]
        public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
    }
}
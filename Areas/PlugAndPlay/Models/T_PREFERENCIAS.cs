using DynamicForms.Context;
using DynamicForms.Models;
using DynamicForms.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class T_PREFERENCIAS
    {
        public T_PREFERENCIAS()
        {

        }
        [READ] [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo PRE_ID requirido.")] public int PRE_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO")] [MaxLength(140, ErrorMessage = "Maximode 140 caracteres, campo PRE_DESCRICAO")] public string PRE_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NAMESPACE")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PRE_NAMESPACE")] public string PRE_NAMESPACE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO")] [MaxLength(50, ErrorMessage = "Maximode 50 caracteres, campo PRE_TIPO")] public string PRE_TIPO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VALOR")] [MaxLength(1000, ErrorMessage = "Maximode 1000 caracteres, campo PRE_VALOR")] public string PRE_VALOR { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "USE_ID")] public int? USE_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PER_ID")] public int? PER_ID { get; set; }
        [NotMapped] public T_Usuario UsuarioLogado { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        public virtual T_Usuario T_Usuario { get; set; }
        public virtual T_Perfil T_Perfil { get; set; }
        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            //buffer é uma linha na tabela de preferencias que sempre carregará a ultima pesquisa feita
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (object obj in objects)
                {
                    if (obj.GetType().Name == "T_PREFERENCIAS")
                    {
                        T_PREFERENCIAS pre = (T_PREFERENCIAS)obj;
                        pre.USE_ID = pre.UsuarioLogado.USE_ID;

                        if (pre.PlayAction.Equals("insert", StringComparison.OrdinalIgnoreCase) || pre.PlayAction.Equals("update", StringComparison.OrdinalIgnoreCase))
                        {
                            if (pre.PRE_TIPO == "BUFFER")
                            {
                                pre.PRE_DESCRICAO = "ÚLTIMA PESQUISA REALIZADA.";
                            }
                        }
                    }
                }

                return true;
            }
        }

        public bool AfterChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert, JSgi db = null)
        {
            var preferencias = objects.Where(r => r.GetType().Name == nameof(T_PREFERENCIAS)).Cast<T_PREFERENCIAS>();
            foreach (var preferencia in preferencias)
            {
                if (preferencia.PlayAction.Equals("insert", StringComparison.OrdinalIgnoreCase) || preferencia.PlayAction.Equals("update", StringComparison.OrdinalIgnoreCase))
                {
                    var usuario = UsuarioSingleton.Instance.ObterUsuario(preferencia.UsuarioLogado.USE_ID);

                    //Atualizando a preferencia do usuário no singleton
                    var prefUsuario = usuario.T_PREFERENCIAS.Where(p => p.PRE_ID == preferencia.PRE_ID).FirstOrDefault();
                    if (prefUsuario != null)
                        usuario.T_PREFERENCIAS.Remove(prefUsuario);
                    usuario.T_PREFERENCIAS.Add(preferencia);
                }
            }
            return true;
        }

    }
}

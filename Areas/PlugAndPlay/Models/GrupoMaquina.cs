using DynamicForms.Models;
using DynamicForms.Util;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "GRUPOS DE MÁQUINAS")]
    public class GrupoMaquina
    {
        public GrupoMaquina()
        {
            Maquinas = new HashSet<Maquina>();
            Roteiros = new HashSet<Roteiro>();
            V_ROTEIROS_CHAPAS = new HashSet<V_ROTEIROS_CHAPAS>();
            Ocorrencia = new List<Ocorrencia>();
        }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD GRUPO MÁQUINA")] [Required(ErrorMessage = "Campo GMA_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo GMA_ID")] public string GMA_ID { get; set; }
        [SEARCH] [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRIÇÃO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo GMA_DESCRICAO")] public string GMA_DESCRICAO { get; set; }

        [Combobox(Description = "1-COMPRAS", Value = "1")]
        [Combobox(Description = "1.1 PREPARAÇÃO MATERIA PRIMA", Value = "1.1")]
        [Combobox(Description = "2 ONDULADEIRA", Value = "2")]
        [Combobox(Description = "2.1 CARTONAGEM", Value = "2.1")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO PLANEJAMENTO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo GMA_TIPO_PLANEJAMENTO")] public string GMA_TIPO_PLANEJAMENTO { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            foreach (var item in objects)
            {
                if (item.GetType().Name == "GrupoMaquina")
                {
                    GrupoMaquina grupoMaquina = (GrupoMaquina)item;

                    if (grupoMaquina.PlayAction == "insert")
                    {
                        if (grupoMaquina.GMA_ID.Length < 3)
                        {
                            grupoMaquina.PlayMsgErroValidacao = "O código do Grupo de Máquina precisa ter mais que 3 caracteres";
                            return false;
                        }
                    }
                }

            }

            return true;
        }

        public virtual ICollection<Maquina> Maquinas { get; set; }
        public virtual ICollection<Roteiro> Roteiros { get; set; }
        public virtual ICollection<V_ROTEIROS_CHAPAS> V_ROTEIROS_CHAPAS { get; set; }
        public virtual ICollection<Ocorrencia> Ocorrencia { get; set; }

        /// <summary>
        /// Estas propriedades foram criadas para representar os estados dos objetos
        /// vindos da interface (importacao)
        /// </summary>
        [NotMapped]
        public string T_MAQUINAS { get; set; }

    }
}
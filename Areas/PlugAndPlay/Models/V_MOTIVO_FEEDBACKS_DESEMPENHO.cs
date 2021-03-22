using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class V_MOTIVO_FEEDBACKS_DESEMPENHO
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(28)]
        public string TIPO { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(8)]
        public string TAR_DIA_TURMA { get; set; }

        [StringLength(30)]
        public string OCO_ID_PERFORMANCE { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string OCO_DESCRICAO { get; set; }

        [StringLength(200)]
        public string TAR_OBS_PERFORMANCE { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(80)]
        public string NOME { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(30)]
        public string MAQ_ID { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(30)]
        public string ORD_ID { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(30)]
        public string PRO_ID { get; set; }

        public int? FPR_SEQ_REPETICAO { get; set; }

        public int? ROT_SEQ_TRANFORMACAO { get; set; }

        [StringLength(10)]
        public string TURM_ID { get; set; }

        [StringLength(10)]
        public string TURN_ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TAR_DIA_TURMA_D { get; set; }

        public string TIPO_ID { get; set; }

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
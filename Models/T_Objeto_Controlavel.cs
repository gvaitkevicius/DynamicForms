using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Models
{
    public class T_Objeto_Controlavel
    {
        public T_Objeto_Controlavel()
        {
            this.T_PERFIL_OBJETO_CONTROLAVEL = new HashSet<T_Perfil_Objeto_Controlavel>();
            this.T_USUARIO_OBJETO_CONTROLAVEL = new HashSet<T_USUARIO_OBJETO_CONTROLAVEL>();
        }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo OBJ_ID requirido.")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo OBJ_ID")] public string OBJ_ID { get; set; }
        [SEARCH] [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo OBJ_DESCRICAO")] public string OBJ_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo OBJ_TIPO")] public string OBJ_TIPO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "GRUPO")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo OBJ_GRUPO")] public string OBJ_GRUPO { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs) {  } 

        public virtual ICollection<T_Perfil_Objeto_Controlavel> T_PERFIL_OBJETO_CONTROLAVEL { get; set; }
        public virtual ICollection<T_USUARIO_OBJETO_CONTROLAVEL> T_USUARIO_OBJETO_CONTROLAVEL { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is T_Objeto_Controlavel)
            {
                T_Objeto_Controlavel objetoControlavel = (T_Objeto_Controlavel)obj;
                if (this.OBJ_ID == objetoControlavel.OBJ_ID && this.OBJ_DESCRICAO == objetoControlavel.OBJ_DESCRICAO &&
                    this.OBJ_TIPO == objetoControlavel.OBJ_TIPO && this.OBJ_GRUPO == objetoControlavel.OBJ_GRUPO)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

    }

    public class T_Objeto_Controlavel_ResultConfiguration : IEntityTypeConfiguration<T_Objeto_Controlavel>
    {
        public void Configure(EntityTypeBuilder<T_Objeto_Controlavel> builder)
        {
            builder.HasKey(oc => oc.OBJ_ID);
            builder.ToTable("T_OBJETO_CONTROLAVEL");
            builder.Property(oc => oc.OBJ_ID).HasColumnName("OBJ_ID").IsRequired().HasMaxLength(100);
            builder.Property(oc => oc.OBJ_GRUPO).HasColumnName("OBJ_GRUPO").IsRequired().HasMaxLength(100);
            builder.Property(oc => oc.OBJ_DESCRICAO).HasColumnName("OBJ_DESCRICAO").HasMaxLength(10);
            builder.Property(oc => oc.OBJ_TIPO).HasColumnName("OBJ_TIPO").HasMaxLength(10);

        }
    }
}

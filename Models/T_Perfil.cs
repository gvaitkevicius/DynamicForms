using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Models
{
    public class T_Perfil
    {
        public T_Perfil()
        {
            this.T_Usuario_Perfil = new HashSet<T_Usuario_Perfil>();
            this.T_PERFIL_OBJETO_CONTROLAVEL = new HashSet<T_Perfil_Objeto_Controlavel>();
            this.T_PREFERENCIAS = new HashSet<T_PREFERENCIAS>();
        }

        [READ] [TAB(Value = "PRINCIPAL")] [Display(Name = "CÓDIGO")] [Required(ErrorMessage = "Campo PER_ID requirido.")] public int PER_ID { get; set; }
        [SEARCH] [TAB(Value = "PRINCIPAL")] [Display(Name = "NOME")] [Required(ErrorMessage = "Campo PER_NOME requirido.")] [MaxLength(50, ErrorMessage = "Maximode 50 caracteres, campo PER_NOME")] public string PER_NOME { get; set; }

        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs) {  } 

        public virtual ICollection<T_Usuario_Perfil> T_Usuario_Perfil { get; set; }
        public virtual ICollection<T_Perfil_Objeto_Controlavel> T_PERFIL_OBJETO_CONTROLAVEL { get; set; }
        public virtual ICollection<T_PREFERENCIAS> T_PREFERENCIAS { get; set; }

    }

    public class T_Perfil_ResultConfiguration : IEntityTypeConfiguration<T_Perfil>
    {
        public void Configure(EntityTypeBuilder<T_Perfil> builder)
        {
            builder.HasKey(p => p.PER_ID);
            builder.ToTable("T_PERFIL");
            builder.Property(p => p.PER_ID).HasColumnName("PER_ID").IsRequired();
            builder.Property(p => p.PER_NOME).HasColumnName("PER_NOME").IsRequired().HasMaxLength(50);
        }
    }
}

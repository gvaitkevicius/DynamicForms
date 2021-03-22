using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Models
{
    public class T_Perfil_Objeto_Controlavel
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo PER_ID requirido.")] public int PER_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo OBJ_ID requirido.")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo OBJ_ID")] public string OBJ_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OBJ_ACAO")] [MaxLength(50, ErrorMessage = "Maximode 50 caracteres, campo PEO_ACAO")] public string PEO_ACAO { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs, ref int modo_insert) {  } 

        public virtual T_Perfil T_Perfil { get; set; }
        public virtual T_Objeto_Controlavel T_Objeto_Controlavel { get; set; }
    }

    public class T_PERFIL_OBJETO_CONTROLAVEL_MAP : IEntityTypeConfiguration<T_Perfil_Objeto_Controlavel>
    {
        public void Configure(EntityTypeBuilder<T_Perfil_Objeto_Controlavel> builder)
        {
            builder.ToTable("T_PERFIL_OBJETO_CONTROLAVEL");
            builder.HasKey(x => new { x.PER_ID, x.OBJ_ID });
            builder.Property(x => x.PER_ID).HasColumnName("PER_ID").IsRequired();
            builder.Property(x => x.OBJ_ID).HasColumnName("OBJ_ID").HasMaxLength(200).IsRequired();
            builder.Property(x => x.PEO_ACAO).HasColumnName("PEO_ACAO").HasMaxLength(50);

            builder.HasOne(u => u.T_Perfil).WithMany(t => t.T_PERFIL_OBJETO_CONTROLAVEL).HasForeignKey(u => u.PER_ID);
            builder.HasOne(u => u.T_Objeto_Controlavel).WithMany(t => t.T_PERFIL_OBJETO_CONTROLAVEL).HasForeignKey(u => u.OBJ_ID);
        }
    }
}

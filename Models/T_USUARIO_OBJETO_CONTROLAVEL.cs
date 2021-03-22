using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Models
{
    public class T_USUARIO_OBJETO_CONTROLAVEL
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD USUÁRIO")] [Required(ErrorMessage = "Campo USE_ID requirido.")] public int USE_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD OBJ CONTROLAVEL")] [Required(ErrorMessage = "Campo OBJ_ID requirido.")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo OBJ_ID")] public string OBJ_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OBJETO_ACAO")] [MaxLength(50, ErrorMessage = "Maximode 50 caracteres, campo USO_ACAO")] public string USO_ACAO { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs, ref int modo_insert) {  } 
        public virtual T_Usuario T_Usuario { get; set; }
        public virtual T_Objeto_Controlavel T_Objeto_Controlavel { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
    }

    public class T_USUARIO_OBJETO_CONTROLAVEL_MAP : IEntityTypeConfiguration<T_USUARIO_OBJETO_CONTROLAVEL>
    {
        public void Configure(EntityTypeBuilder<T_USUARIO_OBJETO_CONTROLAVEL> builder)
        {
            builder.ToTable("T_USUARIO_OBJETO_CONTROLAVEL");
            builder.HasKey(x => new { x.USE_ID, x.OBJ_ID });
            builder.Property(x => x.USE_ID).HasColumnName("USE_ID").IsRequired();
            builder.Property(x => x.OBJ_ID).HasColumnName("OBJ_ID").HasMaxLength(200).IsRequired();
            builder.Property(x => x.USO_ACAO).HasColumnName("USO_ACAO").HasMaxLength(50);

            builder.HasOne(u => u.T_Usuario).WithMany(t => t.T_USUARIO_OBJETO_CONTROLAVEL).HasForeignKey(u => u.USE_ID);
            builder.HasOne(u => u.T_Objeto_Controlavel).WithMany(t => t.T_USUARIO_OBJETO_CONTROLAVEL).HasForeignKey(u => u.OBJ_ID);
        }
    }
}

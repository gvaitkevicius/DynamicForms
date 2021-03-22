using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class EquipeMap : IEntityTypeConfiguration<Equipe>
    {
        public void Configure(EntityTypeBuilder<Equipe> builder)
        {
            builder.ToTable("T_EQUIPE");
            builder.HasKey(x => x.EQU_ID);
            builder.Property(x => x.EQU_ID).HasColumnName("EQU_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.EQU_HIERARQUIA_SEQ_TRANSFORMACAO).HasColumnName("EQU_HIERARQUIA_SEQ_TRANSFORMACAO");
        }
    }
}

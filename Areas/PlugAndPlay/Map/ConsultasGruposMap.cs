using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class ConsultasGruposMap : IEntityTypeConfiguration<ConsultasGrupos>
    {
        public void Configure(EntityTypeBuilder<ConsultasGrupos> builder)
        {
            builder.ToTable("T_CONSULTAS_GRUPOS");
            builder.HasKey(x => new { x.CON_ID, x.GRU_ID });
            builder.Property(x => x.CON_ID).HasColumnName("CON_ID");
            builder.Property(x => x.GRU_ID).HasColumnName("GRU_ID");
        }
    }
}

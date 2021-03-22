using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map
{
    public class ConsultasIndicadoresMap : IEntityTypeConfiguration<ConsultasIndicadores>
    {
        public void Configure(EntityTypeBuilder<ConsultasIndicadores> builder)
        {
            builder.ToTable("T_CONSULTAS_INDICADORES");
            builder.HasKey(x => new { x.CON_ID, x.IND_ID });
            builder.Property(x => x.CON_ID).HasColumnName("CON_ID");
            builder.Property(x => x.IND_ID).HasColumnName("IND_ID");
        }
    }
}

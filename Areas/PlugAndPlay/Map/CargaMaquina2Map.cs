using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map
{
    public class CargaMaquina2Map : IEntityTypeConfiguration<CargaMaquina2>
    {
        public void Configure(EntityTypeBuilder<CargaMaquina2> builder)
        {
            builder.ToTable("V_CARGA_MAQUINA_2");
            builder.HasKey(x => new { x.MED_DATA, x.DIM_ID });
            builder.Property(x => x.MED_DATA).HasColumnName("MED_DATA").IsRequired();
            builder.Property(x => x.GMA_ID).HasColumnName("GMA_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.DIM_ID).HasColumnName("DIM_ID").HasMaxLength(200);
            builder.Property(x => x.MAQ_DESCRICAO).HasColumnName("MAQ_DESCRICAO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.ATRASO).HasColumnName("ATRASO");
            builder.Property(x => x.NA_DATA).HasColumnName("NA_DATA");
            builder.Property(x => x.ADIANTADO).HasColumnName("ADIANTADO");
            builder.Property(x => x.OCIOSO).HasColumnName("OCIOSO");
            builder.Property(x => x.TOTAL).HasColumnName("TOTAL");
        }
    }
}

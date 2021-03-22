using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class CargaMaquinaMap : IEntityTypeConfiguration<CargaMaquina>
    {
        public void Configure(EntityTypeBuilder<CargaMaquina> builder)
        {
            builder.ToTable("V_CARGA_MAQUINA");
            builder.HasKey(x => new { x.MED_DATA, x.DIM_ID });
            builder.Property(x => x.TIPO).HasColumnName("TIPO").HasMaxLength(1).IsRequired();
            builder.Property(x => x.MED_DATA).HasColumnName("MED_DATA").IsRequired();
            builder.Property(x => x.GMA_ID).HasColumnName("GMA_ID").HasMaxLength(30);
            builder.Property(x => x.DIM_ID).HasColumnName("DIM_ID").HasMaxLength(200);
            builder.Property(x => x.MAQ_DESCRICAO).HasColumnName("MAQ_DESCRICAO").HasMaxLength(100);
            builder.Property(x => x.ATRASO).HasColumnName("ATRASO");
            builder.Property(x => x.NA_DATA).HasColumnName("NA_DATA");
            builder.Property(x => x.ADIANTADO).HasColumnName("ADIANTADO");
            builder.Property(x => x.OCIOSO).HasColumnName("OCIOSO");
            builder.Property(x => x.DISPONIVEL).HasColumnName("DISPONIVEL");
            builder.Property(x => x.TOTAL).HasColumnName("TOTAL");
            builder.Property(x => x.M2_MAQUINA_DIA).HasColumnName("M2_MAQUINA_DIA");
            builder.Property(x => x.M2_ACABADO_DIA).HasColumnName("M2_ACABADO_DIA");
            builder.Property(x => x.PESO_MAQUINA_DIA).HasColumnName("PESO_MAQUINA_DIA");
            builder.Property(x => x.PESO_ACABADO_DIA).HasColumnName("PESO_ACABADO_DIA");
            builder.Property(x => x.MAQ_HIERARQUIA_SEQ_TRANSFORMACAO).HasColumnName("MAQ_HIERARQUIA_SEQ_TRANSFORMACAO");
            builder.Property(x => x.MAQ_ID).HasColumnName("MAQ_ID").HasMaxLength(30);
        }
    }
}

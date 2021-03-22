using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map.Qualidade
{
    public class InspecaoVisualMap : IEntityTypeConfiguration<InspecaoVisual>
    {
        public void Configure(EntityTypeBuilder<InspecaoVisual> builder)
        {
            builder.ToTable("T_INSPECAO_VISUAL");
            builder.HasKey(x => x.IPV_ID);
            builder.Property(x => x.IPV_ID).HasColumnName("IPV_ID").IsRequired();
            builder.Property(x => x.IPV_VALOR).HasColumnName("IPV_VALOR").HasMaxLength(10);
            builder.Property(x => x.IPV_ID_OPERADOR).HasColumnName("IPV_ID_OPERADOR");
            builder.Property(x => x.IPV_ID_LIBERACAO).HasColumnName("IPV_ID_LIBERACAO");
            builder.Property(x => x.IPV_OBS).HasColumnName("IPV_OBS").HasMaxLength(100);
            builder.Property(x => x.IPV_DATA_COLETA).HasColumnName("IPV_DATA_COLETA");
            builder.Property(x => x.IPV_DATA_AVAL).HasColumnName("IPV_DATA_AVAL");
            builder.Property(x => x.TIV_ID).HasColumnName("TIV_ID");
            builder.Property(x => x.TURN_ID).HasColumnName("TURN_ID").HasMaxLength(10);
            builder.Property(x => x.TURM_ID).HasColumnName("TURM_ID").HasMaxLength(10);
            builder.Property(x => x.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(60);
            builder.Property(x => x.ROT_PRO_ID).HasColumnName("ROT_PRO_ID").HasMaxLength(30);
            builder.Property(x => x.ROT_MAQ_ID).HasColumnName("ROT_MAQ_ID").HasMaxLength(30);
            builder.Property(x => x.ROT_SEQ_TRANSFORMACAO).HasColumnName("ROT_SEQ_TRANSFORMACAO");
            builder.Property(x => x.FPR_SEQ_REPETICAO).HasColumnName("FPR_SEQ_REPETICAO");
            builder.Property(x => x.IPV_STATUS_LIBERACAO).HasColumnName("IPV_STATUS_LIBERACAO").HasMaxLength(30);
            builder.Property(x => x.IPV_VALOR_MEDIDA).HasColumnName("IPV_VALOR_MEDIDA");
            //builder.HasOne(x => x.TipoInspecaoVisual).WithMany(u => u.InspecaoVisual).HasForeignKey(x => x.TIV_ID);
        }
    }
}

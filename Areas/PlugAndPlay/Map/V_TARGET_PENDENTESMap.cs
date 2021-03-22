using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class V_TARGET_PENDENTESMap : IEntityTypeConfiguration<V_TARGET_PENDENTES>
    {
        public void Configure(EntityTypeBuilder<V_TARGET_PENDENTES> builder)
        {
            builder.ToTable("V_TARGET_PENDENTES");
            builder.HasKey(x => new { x.MOV_ID, x.MAQ_ID, x.ORD_ID, x.PRO_ID });
            builder.Property(x => x.MOV_ID).HasColumnName("MOV_ID").IsRequired();
            builder.Property(x => x.MAQ_ID).HasColumnName("MAQ_ID").HasMaxLength(30);
            builder.Property(x => x.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(60);
            builder.Property(x => x.PRO_ID).HasColumnName("PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.FPR_SEQ_REPETICAO).HasColumnName("FPR_SEQ_REPETICAO");
            builder.Property(x => x.FPR_SEQ_TRANFORMACAO).HasColumnName("FPR_SEQ_TRANFORMACAO");
            builder.Property(x => x.MOV_QUANTIDADE).HasColumnName("MOV_QUANTIDADE").IsRequired();
            builder.Property(x => x.TURM_ID).HasColumnName("TURM_ID").HasMaxLength(10);
            builder.Property(x => x.TURN_ID).HasColumnName("TURN_ID").HasMaxLength(10);
            builder.Property(x => x.MOV_DATA_HORA_CRIACAO).HasColumnName("MOV_DATA_HORA_CRIACAO");
            builder.Property(x => x.USE_NOME).HasColumnName("USE_NOME").HasMaxLength(80).IsRequired();
            builder.Property(x => x.PRO_DESCRICAO).HasColumnName("PRO_DESCRICAO").HasMaxLength(100).IsRequired();
        }
    }
}

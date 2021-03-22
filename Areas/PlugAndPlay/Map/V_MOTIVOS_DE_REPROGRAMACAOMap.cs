using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map
{
    public class V_MOTIVOS_DE_REPROGRAMACAOMap : IEntityTypeConfiguration<V_MOTIVOS_DE_REPROGRAMACAO>
    {
        public void Configure(EntityTypeBuilder<V_MOTIVOS_DE_REPROGRAMACAO> builder)
        {
            builder.ToTable("V_MOTIVOS_DE_REPROGRAMACAO");
            builder.HasKey(x => x.ORD_ID);
            builder.Property(x => x.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(60);
            builder.Property(x => x.PRO_ID).HasColumnName("PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.MAQ_ID).HasColumnName("MAQ_ID").HasMaxLength(30);
            builder.Property(x => x.FPR_SEQ_REPETICAO).HasColumnName("FPR_SEQ_REPETICAO");
            builder.Property(x => x.FPR_SEQ_TRANFORMACAO).HasColumnName("FPR_SEQ_TRANFORMACAO");
            builder.Property(x => x.MOV_OCO_ID_OP_PARCIAL).HasColumnName("MOV_OCO_ID_OP_PARCIAL").HasMaxLength(30).IsRequired();
            builder.Property(x => x.MOV_OBS_OP_PARCIAL).HasColumnName("MOV_OBS_OP_PARCIAL").HasMaxLength(200).IsRequired();
            builder.Property(x => x.OCO_DESCRICAO).HasColumnName("OCO_DESCRICAO").HasMaxLength(100).IsRequired();
        }
    }
}

using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map
{
    public class OperacoesMap : IEntityTypeConfiguration<Operacoes>
    {
        public void Configure(EntityTypeBuilder<Operacoes> builder)
        {
            builder.ToTable("T_OPERACOES");
            builder.HasKey(x => new { x.OPE_TIPO_REGISTRO, x.OPE_ID, x.MAQ_ID, x.PRO_ID, x.ROT_SEQ_TRANFORMACAO });
            builder.Property(x => x.OPE_TIPO_REGISTRO).HasColumnName("OPE_TIPO_REGISTRO").HasMaxLength(2).IsRequired();
            builder.Property(x => x.OPE_ID).HasColumnName("OPE_ID").HasMaxLength(60).IsRequired();
            builder.Property(x => x.GMA_ID).HasColumnName("GMA_ID").HasMaxLength(10);
            builder.Property(x => x.MAQ_ID).HasColumnName("MAQ_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.PRO_ID).HasColumnName("PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.OPE_EXCECAO).HasColumnName("OPE_EXCECAO").HasMaxLength(10);
            builder.Property(x => x.ROT_SEQ_TRANFORMACAO).HasColumnName("ROT_SEQ_TRANSFORMACAO").IsRequired();
            builder.Property(x => x.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(60);
            builder.Property(x => x.FPR_SEQ_REPETICAO).HasColumnName("FPR_SEQ_REPETICAO");

            builder.HasOne(x => x.Roteiro).WithMany(c => c.Operacoes).HasForeignKey(x => new { x.MAQ_ID, x.PRO_ID, x.ROT_SEQ_TRANFORMACAO });

        }
    }
}

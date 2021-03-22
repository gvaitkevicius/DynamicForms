
using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map
{
    public class V_ROTEIROS_CHAPASMap : IEntityTypeConfiguration<V_ROTEIROS_CHAPAS>
    {
        public void Configure(EntityTypeBuilder<V_ROTEIROS_CHAPAS> builder)
        {
            builder.ToTable("V_ROTEIROS_CHAPAS");
            builder.HasKey(x => new { x.MAQ_ID, x.PRO_ID, x.ROT_SEQ_TRANFORMACAO });
            builder.Property(x => x.GMA_ID).HasColumnName("GMA_ID").HasMaxLength(30);
            builder.Property(x => x.MAQ_ID).HasColumnName("MAQ_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.PRO_ID).HasColumnName("PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.ROT_SEQ_TRANFORMACAO).HasColumnName("ROT_SEQ_TRANFORMACAO").IsRequired();
            builder.Property(x => x.ROT_PECAS_POR_PULSO).HasColumnName("ROT_PECAS_POR_PULSO");
            builder.Property(x => x.ROT_PRIORIDADE_INFORMADA).HasColumnName("ROT_PRIORIDADE_INFORMADA");
            builder.Property(x => x.ROT_ACAO).HasColumnName("ROT_ACAO").HasMaxLength(2);
            builder.Property(x => x.ROT_PERFORMANCE).HasColumnName("ROT_PERFORMANCE");
            builder.Property(x => x.ROT_TEMPO_SETUP).HasColumnName("ROT_TEMPO_SETUP");
            builder.Property(x => x.ROT_TEMPO_SETUP_AJUSTE).HasColumnName("ROT_TEMPO_SETUP_AJUSTE");
            builder.Property(x => x.ROT_VA_PARA_SEQ_TRANSFORMACAO).HasColumnName("ROT_VA_PARA_SEQ_TRANSFORMACAO");
            builder.Property(x => x.ROT_STATUS).HasColumnName("ROT_STATUS").HasMaxLength(2);
            builder.Property(x => x.ROT_HIERARQUIA_SEQ_TRANSFORMACAO).HasColumnName("ROT_HIERARQUIA_SEQ_TRANSFORMACAO");
            builder.Property(x => x.ROT_AVALIA_CUSTO).HasColumnName("ROT_AVALIA_CUSTO");

            //forengn key
            builder.HasOne(x => x.ProdutoChapaIntermediaria).WithMany(ch => ch.V_ROTEIROS_CHAPAS).HasForeignKey(x => x.PRO_ID);
            builder.HasOne(x => x.Maquina).WithMany(m => m.V_ROTEIROS_CHAPAS).HasForeignKey(x => x.MAQ_ID);
            builder.HasOne(x => x.GrupoMaquina).WithMany(gm => gm.V_ROTEIROS_CHAPAS).HasForeignKey(x => x.GMA_ID);
        }
    }
}

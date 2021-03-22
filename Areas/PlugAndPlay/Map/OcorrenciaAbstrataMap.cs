using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class OcorrenciaAbstrataMap : IEntityTypeConfiguration<OcorrenciaAbstrata>
    {

        public void Configure(EntityTypeBuilder<OcorrenciaAbstrata> builder)
        {
            builder.ToTable("V_OCORRENCIAS");
            builder.HasKey(x => x.OCO_ID);
            builder.Property(x => x.OCO_ID).HasColumnName("OCO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.OCO_DESCRICAO).HasColumnName("OCO_DESCRICAO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.OCO_SUB_TIPO).HasColumnName("OCO_SUB_TIPO").IsRequired();
            builder.Property(x => x.TIP_ID).HasColumnName("TIP_ID").IsRequired();
            builder.Property(x => x.GMA_ID).HasColumnName("GMA_ID");
            builder.Property(x => x.MAQ_ID).HasColumnName("MAQ_ID").HasMaxLength(30);
            builder.Property(x => x.SPR).HasColumnName("SPR");
            builder.HasDiscriminator<int>("TIP_ID").HasValue<OcorrenciaMotivosDeParadas>(1).
                HasValue<OcorrenciaMotivosDasPerdas>(2).
                HasValue<OcorrenciaProducao>(5).
                HasValue<OcorrenciaProducaoParciais>(6).
                HasValue<OcorrenciaVendas>(11).
                HasValue<OcorrenciaTransporte>(7).
                HasValue<OcorrenciaSaidaInventario>(8).
                HasValue<OcorrenciaEntradaInventario>(9).
                HasValue<OcorrenciaConsumoMateriaPrima>(10).
                HasValue<OcorrenciaPularOrdemFila>(12).
                HasValue<OcorrenciaRetencaoLotes>(101);

        }
    }
}
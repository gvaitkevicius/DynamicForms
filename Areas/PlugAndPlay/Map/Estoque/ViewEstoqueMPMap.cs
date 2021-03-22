using DynamicForms.Areas.PlugAndPlay.Models.Estoque;
using Microsoft.EntityFrameworkCore;

namespace DynamicForms.Areas.PlugAndPlay.Map.Estoque
{
    public class ViewEstoqueMPMap : IEntityTypeConfiguration<ViewEstoqueMP>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ViewEstoqueMP> builder)
        {
            builder.ToTable("V_ESTOQUE_MP");
            builder.HasKey(x => x.PRO_ID_COMPONENTE);
            builder.Property(x => x.GRP_ID).HasColumnName("GRP_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.PRO_ID_COMPONENTE).HasColumnName("PRO_ID_COMPONENTE").HasMaxLength(30).IsRequired();
            builder.Property(x => x.PRO_DESCRICAO).HasColumnName("PRO_DESCRICAO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.CONSUMO_DO_PERIODO).HasColumnName("CONSUMO_DO_PERIODO");
            builder.Property(x => x.CONSUMO_HOJE).HasColumnName("CONSUMO_HOJE");
            builder.Property(x => x.CONSUMO_AMANHA).HasColumnName("CONSUMO_AMANHA");
            builder.Property(x => x.CONSUMO_CINCO).HasColumnName("CONSUMO_CINCO");
            builder.Property(x => x.CONSUMO_DEZ).HasColumnName("CONSUMO_DEZ");
            builder.Property(x => x.CONSUMO_QUINZE).HasColumnName("CONSUMO_QUINZE");
            builder.Property(x => x.CONSUMO_PREVISTO).HasColumnName("CONSUMO_PREVISTO");
        }
    }
}

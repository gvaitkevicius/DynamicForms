using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map
{
    public class ProdutoWMSExpedicaoMap : IEntityTypeConfiguration<ProdutoWMSExpedicao>
    {
        public void Configure(EntityTypeBuilder<ProdutoWMSExpedicao> builder)
        {
            builder.Property(x => x.UNI_ID).HasColumnName("UNI_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.GRP_ID).HasColumnName("GRP_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.PRO_PECAS_POR_FARDO).HasColumnName("PRO_PECAS_POR_FARDO");
            builder.Property(x => x.PRO_FARDOS_POR_CAMADA).HasColumnName("PRO_FARDOS_POR_CAMADA");
            builder.Property(x => x.PRO_CAMADAS_POR_PALETE).HasColumnName("PRO_CAMADAS_POR_PALETE");

            builder.HasOne(x => x.GrupoProdutoWMSExpedicao).WithMany(gp => gp.ProdutoWMSExpedicao).HasForeignKey(x => x.GRP_ID);
            builder.HasOne(x => x.UnidadeMedida).WithMany(um => um.ProdutoWMSExpedicao).HasForeignKey(x => x.UNI_ID);

        }
    }
}

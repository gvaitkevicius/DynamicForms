using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class ProdutoAbstratoMap : IEntityTypeConfiguration<ProdutoAbstrato>
    {
        public void Configure(EntityTypeBuilder<ProdutoAbstrato> builder)
        {
            builder.ToTable("T_PRODUTOS");
            builder.HasKey(x => x.PRO_ID);
            builder.Property(x => x.PRO_ID).HasColumnName("PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.PRO_DESCRICAO).HasColumnName("PRO_DESCRICAO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.PRO_ID_INTEGRACAO).HasColumnName("PRO_ID_INTEGRACAO").HasMaxLength(100);
            builder.Property(x => x.PRO_ID_INTEGRACAO_ERP).HasColumnName("PRO_ID_INTEGRACAO_ERP").HasMaxLength(100);
            builder.Property(x => x.PRO_STATUS).HasColumnName("PRO_STATUS").HasMaxLength(2);
            builder.HasDiscriminator<double>("PRO_TYPE")
                .HasValue<ProdutoAbstrato>(1000)
                .HasValue<ProdutoChapaIntermediaria>(2)
                .HasValue<ProdutoConjunto>(3)
                .HasValue<ProdutoCaixa>(4)
                .HasValue<ProdutoChapaVenda>(5)
                .HasValue<ProdutoPalete>(6)
                .HasValue<ProdutoTinta>(7)
                .HasValue<ProdutoCliches>(8)
                .HasValue<ProdutoFaca>(8.1)
                .HasValue<ProdutoWMSExpedicao>(9)
                .HasValue<ProdutoPapel>(11);
        }
    }
}

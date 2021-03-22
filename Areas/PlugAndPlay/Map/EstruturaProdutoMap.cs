using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class EstruturaProdutoMap : IEntityTypeConfiguration<EstruturaProduto>
    {
        public void Configure(EntityTypeBuilder<EstruturaProduto> builder)
        {
            builder.ToTable("T_ESTRUTURA_PRODUTO");
            builder.HasKey(x => new { x.PRO_ID_PRODUTO, x.PRO_ID_COMPONENTE });
            builder.Property(x => x.EST_DATA_VALIDADE).HasColumnName("EST_DATA_VALIDADE").IsRequired();
            builder.Property(x => x.PRO_ID_PRODUTO).HasColumnName("PRO_ID_PRODUTO").HasMaxLength(30).IsRequired();
            builder.Property(x => x.PRO_ID_COMPONENTE).HasColumnName("PRO_ID_COMPONENTE").HasMaxLength(30).IsRequired();
            builder.Property(x => x.EST_QUANT).HasColumnName("EST_QUANT").IsRequired();
            builder.Property(x => x.EST_DATA_INCLUSAO).HasColumnName("EST_DATA_INCLUSAO").IsRequired();
            builder.Property(x => x.EST_BASE_PRODUCAO).HasColumnName("EST_BASE_PRODUCAO").IsRequired();
            builder.Property(x => x.EST_TIPO_REQUISICAO).HasColumnName("EST_TIPO_REQUISICAO").HasMaxLength(2).IsRequired();
            builder.Property(x => x.EST_CODIGO_DE_EXCECAO).HasColumnName("EST_CODIGO_DE_EXCECAO").HasMaxLength(3000).IsRequired();

            builder.HasOne(x => x.Produto).WithMany(x => x.EstruturasProdutoPai).HasForeignKey(x => x.PRO_ID_PRODUTO);
            builder.HasOne(x => x.ProdutoComponente).WithMany(p => p.EstruturasProdutoFilho).HasForeignKey(x => x.PRO_ID_COMPONENTE);
        }
    }
}
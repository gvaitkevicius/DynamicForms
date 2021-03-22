using DynamicForms.Areas.PlugAndPlay.Models.Estoque;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map.Estoque
{
    public class ViewEstoqueIntermediarioMap : IEntityTypeConfiguration<ViewEstoqueIntermediario>
    {
        public void Configure(EntityTypeBuilder<ViewEstoqueIntermediario> builder)
        {
            builder.ToTable("V_ESTOQUE_INTERMEDIARIO");
            builder.HasKey(x => x.ORD_ID);
            builder.Property(x => x.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(60).IsRequired();
            builder.Property(x => x.PRO_ID).HasColumnName("PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.PRO_DESCRICAO).HasColumnName("PRO_DESCRICAO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.GRP_ID).HasColumnName("GRP_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.GRP_DESCRICAO).HasColumnName("GRP_DESCRICAO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.GRP_TIPO).HasColumnName("GRP_TIPO").IsRequired();
            builder.Property(x => x.ORD_QUANTIDADE).HasColumnName("ORD_QUANTIDADE");
            builder.Property(x => x.ORD_TOLERANCIA_MENOS).HasColumnName("ORD_TOLERANCIA_MENOS");
            builder.Property(x => x.ORD_TOLERANCIA_MAIS).HasColumnName("ORD_TOLERANCIA_MAIS");
            builder.Property(x => x.SALDO).HasColumnName("SALDO");
            builder.Property(x => x.SALDO_RETIDO).HasColumnName("SALDO_RETIDO");
            builder.Property(x => x.QTD_PALETES).HasColumnName("QTD_PALETES");
            builder.Property(x => x.COMPROMISSADO).HasColumnName("COMPROMISSADO");
            builder.Property(x => x.DISPONIVEL).HasColumnName("DISPONIVEL");
            builder.Property(x => x.SOBRA_PRODUCAO).HasColumnName("SOBRA_PRODUCAO");
            builder.Property(x => x.SOBRA_EXPEDICAO).HasColumnName("SOBRA_EXPEDICAO");
            builder.Property(x => x.DEVOLUCAO).HasColumnName("DEVOLUCAO");
            builder.Property(x => x.QTD_VENDA).HasColumnName("QTD_VENDA");
            builder.Property(x => x.PEDIDOS_FUTUROS).HasColumnName("PEDIDOS_FUTUROS");
        }
    }
}

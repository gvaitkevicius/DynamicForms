using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map
{
    public class ViewPedidosFuturosMap : IEntityTypeConfiguration<ViewPedidosFuturos>
    {
        public void Configure(EntityTypeBuilder<ViewPedidosFuturos> builder)
        {
            builder.ToTable("V_PEDIDOS_FUTUROS");
            builder.HasKey(x => x.ORD_ID);
            builder.Property(x => x.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(60).IsRequired();
            builder.Property(x => x.ORD_TIPO).HasColumnName("ORD_TIPO");
            builder.Property(x => x.ORD_TIPO_DESCRICAO).HasColumnName("ORD_TIPO_DESCRICAO").HasMaxLength(22);
            builder.Property(x => x.PRO_ID).HasColumnName("PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.PRO_DESCRICAO).HasColumnName("PRO_DESCRICAO").HasMaxLength(100);
            builder.Property(x => x.SALDO_ESTOQUE).HasColumnName("SALDO_ESTOQUE").IsRequired();
            builder.Property(x => x.ORD_QUANTIDADE).HasColumnName("ORD_QUANTIDADE").IsRequired();
            builder.Property(x => x.CLI_NOME).HasColumnName("CLI_NOME").HasMaxLength(100);
            builder.Property(x => x.ORD_DATA_ENTREGA_DE).HasColumnName("ORD_DATA_ENTREGA_DE").IsRequired();
            builder.Property(x => x.ORD_DATA_ENTREGA_ATE).HasColumnName("ORD_DATA_ENTREGA_ATE").IsRequired();
            builder.Property(x => x.ORD_TOLERANCIA_MENOS).HasColumnName("ORD_TOLERANCIA_MENOS");
            builder.Property(x => x.ORD_TOLERANCIA_MAIS).HasColumnName("ORD_TOLERANCIA_MAIS");
            builder.Property(x => x.GRP_TIPO).HasColumnName("GRP_TIPO");
        }
    }
}

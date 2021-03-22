using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map
{
    public class SaldoPedidoMap : IEntityTypeConfiguration<SaldoPedido>
    {
        public void Configure(EntityTypeBuilder<SaldoPedido> builder)
        {
            builder.ToTable("V_SALDO_PEDIDO");
            builder.HasKey(x => x.ORD_ID);
            builder.Property(x => x.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(60).IsRequired();
            builder.Property(x => x.CLI_ID).HasColumnName("CLI_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.ORD_QUANTIDADE).HasColumnName("ORD_QUANTIDADE").IsRequired();
            builder.Property(x => x.ORD_TOLERANCIA_MAIS).HasColumnName("ORD_TOLERANCIA_MAIS");
            builder.Property(x => x.ORD_TOLERANCIA_MENOS).HasColumnName("ORD_TOLERANCIA_MENOS");
            builder.Property(x => x.ORD_DATA_ENTREGA_DE).HasColumnName("ORD_DATA_ENTREGA_DE").IsRequired();
            builder.Property(x => x.ORD_DATA_ENTREGA_ATE).HasColumnName("ORD_DATA_ENTREGA_ATE").IsRequired();
            builder.Property(x => x.SALDO).HasColumnName("SALDO");
        }
    }
}

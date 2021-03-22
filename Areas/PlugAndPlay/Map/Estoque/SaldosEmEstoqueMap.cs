using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class SaldosEmEstoquePorLoteMap : IEntityTypeConfiguration<SaldosEmEstoquePorLote>
    {
        public void Configure(EntityTypeBuilder<SaldosEmEstoquePorLote> builder)
        {
            builder.ToTable("V_SALDO_ESTOQUE_POR_LOTE");
            builder.HasKey(me => new { me.PRO_ID, me.MOV_LOTE, me.MOV_SUB_LOTE });
            builder.Property(x => x.PRO_ID).HasColumnName("PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.MOV_LOTE).HasColumnName("MOV_LOTE").HasMaxLength(100);
            builder.Property(x => x.MOV_SUB_LOTE).HasColumnName("MOV_SUB_LOTE").HasMaxLength(30);
            builder.Property(x => x.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(60).IsRequired();
            builder.Property(x => x.MOV_ENDERECO).HasColumnName("MOV_ENDERECO").HasMaxLength(30);
            builder.Property(x => x.MOV_APROVEITAMENTO).HasColumnName("MOV_APROVEITAMENTO").HasMaxLength(1).IsRequired();
            builder.Property(x => x.SALDO).HasColumnName("SALDO");
            builder.Property(x => x.VENDAS).HasColumnName("VENDAS");

            builder.Property(x => x.QTD_RESERVA).HasColumnName("QTD_RESERVA");
            builder.Property(x => x.CAR_ID).HasColumnName("CAR_ID");
            builder.Property(x => x.PESO).HasColumnName("PESO");
            builder.Property(x => x.PESO_EMBALAGENS).HasColumnName("PESO_EMBALAGENS");

            builder.HasOne(x => x.ConsultaPedido).WithMany(x => x.SaldosEmEstoquePorLote).HasForeignKey(x => x.ORD_ID);

        }
    }
    public class SaldoEmEstoquePorPedidoMap : IEntityTypeConfiguration<SaldoEmEstoquePorPedido>
    {
        public void Configure(EntityTypeBuilder<SaldoEmEstoquePorPedido> builder)
        {
            builder.ToTable("V_SALDO_ALOCADO_PEDIDO");
            builder.HasKey(me => new { me.PRO_ID, me.ORD_ID });
            builder.Property(x => x.PRO_ID).HasColumnName("PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(60);
            builder.Property(x => x.FPR_SEQ_TRANFORMACAO).HasColumnName("FPR_SEQ_TRANFORMACAO");
            builder.Property(x => x.SALDO_ALOCADO).HasColumnName("SALDO_ALOCADO");
        }
    }
}

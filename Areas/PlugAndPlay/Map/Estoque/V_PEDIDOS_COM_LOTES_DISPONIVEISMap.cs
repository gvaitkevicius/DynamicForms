using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Areas.PlugAndPlay.Map.Estoque
{
    public class V_PEDIDOS_COM_LOTES_DISPONIVEISMap : IEntityTypeConfiguration<V_PEDIDOS_COM_LOTES_DISPONIVEIS>
    {
        public void Configure(EntityTypeBuilder<V_PEDIDOS_COM_LOTES_DISPONIVEIS> builder)
        {
            builder.ToTable("V_PEDIDOS_COM_LOTES_DISPONIVEIS");
            builder.HasKey(me => new { me.PRO_ID, me.MOV_LOTE, me.MOV_SUB_LOTE });
            builder.Property(x => x.PRO_ID).HasColumnName("PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.MOV_LOTE).HasColumnName("MOV_LOTE").HasMaxLength(100);
            builder.Property(x => x.MOV_SUB_LOTE).HasColumnName("MOV_SUB_LOTE").HasMaxLength(30);
            builder.Property(x => x.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(60).IsRequired();
            builder.Property(x => x.MOV_ENDERECO).HasColumnName("MOV_ENDERECO").HasMaxLength(30);
            builder.Property(x => x.MOV_APROVEITAMENTO).HasColumnName("MOV_APROVEITAMENTO").HasMaxLength(1).IsRequired();
            builder.Property(x => x.SALDO).HasColumnName("SALDO");
            builder.Property(x => x.QTD_RESERVA).HasColumnName("QTD_RESERVA");
            builder.Property(x => x.CAR_ID).HasColumnName("CAR_ID");
            builder.Property(x => x.DISPONIVEL).HasColumnName("DISPONIVEL");
        }
    }
}

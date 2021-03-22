using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Areas.PlugAndPlay.Map
{
    public class V_INPUT_INTEGRACAO_BALANCA_FATURAMENTO_MAP : IEntityTypeConfiguration<V_INPUT_INTEGRACAO_BALANCA_FATURAMENTO>
    {
        public void Configure(EntityTypeBuilder<V_INPUT_INTEGRACAO_BALANCA_FATURAMENTO> builder)
        {
            builder.ToTable("V_INPUT_INTEGRACAO_BALANCA_FATURAMENTO");
            builder.HasKey(x => x.CAR_ID_INTEGRACAO_BALANCA);
            builder.Property(x => x.CAR_ID_INTEGRACAO_BALANCA).HasColumnName("CAR_ID_INTEGRACAO_BALANCA").IsRequired();
            builder.Property(x => x.VEI_PLACA).HasColumnName("VEI_PLACA").HasMaxLength(8).IsRequired();
            builder.Property(x => x.CAR_PESO_ENTRADA).HasColumnName("CAR_PESO_ENTRADA").IsRequired();
            builder.Property(x => x.CAR_PESO_SAIDA).HasColumnName("CAR_PESO_SAIDA").IsRequired();
        }
    }
}

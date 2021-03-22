using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Areas.PlugAndPlay.Map
{
    public class V_DISPONIBILIDADE_CLICHE_MAP : IEntityTypeConfiguration<V_DISPONIBILIDADE_CLICHE>
    {
        public void Configure(EntityTypeBuilder<V_DISPONIBILIDADE_CLICHE> builder)
        {
            builder.ToTable("V_DISPONIBILIDADE_CLICHE");
            builder.HasKey(x => x.ICA_ID);
            builder.Property(x => x.ICA_ID).HasColumnName("ICA_ID").IsRequired();
            builder.Property(x => x.ICA_DATA_DE).HasColumnName("ICA_DATA_DE").IsRequired();
            builder.Property(x => x.ICA_DATA_ATE).HasColumnName("ICA_DATA_ATE").IsRequired();
            builder.Property(x => x.ICA_OBSERVACAO).HasColumnName("ICA_OBSERVACAO").HasMaxLength(200);
            builder.Property(x => x.ICA_TIPO).HasColumnName("ICA_TIPO").IsRequired();
            builder.Property(x => x.CAL_ID).HasColumnName("CAL_ID").IsRequired();
            builder.Property(x => x.PRO_ID).HasColumnName("PRO_ID");

            builder.HasOne(x => x.ProdutoCliches).WithMany(x => x.Indisponibilidade).HasForeignKey(x => x.PRO_ID);
            builder.HasOne(x => x.Calendario).WithMany(x => x.IndisponibilidadeCliche).HasForeignKey(x => x.CAL_ID);

        }

    }
}

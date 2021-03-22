using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Areas.PlugAndPlay.Map
{
    public class V_IMPRESSAO_ETIQUETAS_OND_MAP : IEntityTypeConfiguration<V_IMPRESSAO_ETIQUETAS_OND>
    {
        public void Configure(EntityTypeBuilder<V_IMPRESSAO_ETIQUETAS_OND> builder)
        {
            builder.ToTable("V_IMPRESSAO_ETIQUETAS_OND");
            builder.HasKey(x => x.ORD_ID);
            builder.Property(x => x.BOL_ID).HasColumnName("BOL_ID").IsRequired();
            builder.Property(x => x.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(60).IsRequired();
            builder.Property(x => x.FPR_QUANTIDADE_PREVISTA).HasColumnName("FPR_QUANTIDADE_PREVISTA").IsRequired();
            builder.Property(x => x.PC_PRO_ID).HasColumnName("PC_PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.FPR_DATA_INICIO_PREVISTA).HasColumnName("FPR_DATA_INICIO_PREVISTA").IsRequired();
            builder.Property(x => x.FPR_DATA_FIM_PREVISTA).HasColumnName("FPR_DATA_FIM_PREVISTA").IsRequired();
            builder.Property(x => x.FPR_PREVISAO_MATERIA_PRIMA).HasColumnName("FPR_PREVISAO_MATERIA_PRIMA").IsRequired();

            builder.HasOne(x => x.Order).WithMany(x => x.ImpressaoEtiquetasOnd).HasForeignKey(x => x.ORD_ID);
            builder.HasOne(x => x.Produto).WithMany(x => x.ImpressaoEtiquetasOnd).HasForeignKey(x => x.PC_PRO_ID);

        }
    }
}

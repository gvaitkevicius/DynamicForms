using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Areas.PlugAndPlay.Map
{
    public class EstruturaEtiquetaMap : IEntityTypeConfiguration<EstruturaEtiqueta>
    {
        public void Configure(EntityTypeBuilder<EstruturaEtiqueta> builder)
        {
            builder.ToTable("T_ESTRUTURA_ETIQUETA");
            builder.HasKey(x => x.EST_ID);
            builder.Property(x => x.EST_ID).HasColumnName("EST_ID").IsRequired();
            builder.Property(x => x.HTML_ESTRUTURA).HasColumnName("HTML_ESTRUTURA").HasMaxLength(8000);
            builder.Property(x => x.CLI_ID).HasColumnName("CLI_ID").HasMaxLength(30);
            builder.Property(x => x.EST_DESCRICAO).HasColumnName("EST_DESCRICAO").HasMaxLength(30);

            builder.HasOne(x => x.Cliente).WithMany(x => x.EstruturaEtiqueta).HasForeignKey(x => x.CLI_ID);
        }
    }
}

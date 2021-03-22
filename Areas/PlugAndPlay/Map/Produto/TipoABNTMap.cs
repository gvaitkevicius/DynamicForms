using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Areas.PlugAndPlay.Map
{
    public class TipoABNTMap : IEntityTypeConfiguration<TipoABNT>
    {
        public void Configure(EntityTypeBuilder<TipoABNT> builder)
        {
            builder.ToTable("T_TIPO_ABNT");
            builder.HasKey(x => x.ABN_ID);
            builder.Property(x => x.ABN_ID).HasColumnName("ABN_ID").HasMaxLength(50).IsRequired();
            builder.Property(x => x.ABN_DESCRICAO).HasColumnName("ABN_DESCRICAO").HasMaxLength(50);
        }
    }
}

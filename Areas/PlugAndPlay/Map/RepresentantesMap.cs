using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Areas.PlugAndPlay.Map
{
    public class RepresentantesMap : IEntityTypeConfiguration<Representantes>
    {
        public void Configure(EntityTypeBuilder<Representantes> builder)
        {
            builder.ToTable("T_REPRESENTANTES");
            builder.HasKey(x => x.REP_ID);
            builder.Property(x => x.REP_ID).HasColumnName("REP_ID").IsRequired();
            builder.Property(x => x.REP_NOME).HasColumnName("REP_NOME").HasMaxLength(80);
        }
    }
}

using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Areas.PlugAndPlay.Map
{
    public class CotasMap : IEntityTypeConfiguration<Cotas>
    {
        public void Configure(EntityTypeBuilder<Cotas> builder)
        {
            builder.ToTable("T_COTAS");
            builder.HasKey(x => x.COT_ID);
            builder.Property(x => x.COT_ID).HasColumnName("COT_ID").IsRequired();
            builder.Property(x => x.COT_DATA_DE).HasColumnName("COT_DATA_DE");
            builder.Property(x => x.COT_DATA_ATE).HasColumnName("COT_DATA_ATE");
            builder.Property(x => x.COT_VALOR).HasColumnName("COT_VALOR");
            builder.Property(x => x.COT_OCUPADO).HasColumnName("COT_OCUPADO");
            builder.Property(x => x.REP_ID).HasColumnName("REP_ID").IsRequired();

            builder.HasOne(x => x.Representantes).WithMany(x => x.Cotas).HasForeignKey(x => x.REP_ID);
        }
    }
}

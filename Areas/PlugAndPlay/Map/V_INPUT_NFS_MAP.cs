using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Areas.PlugAndPlay.Map
{
    public class V_INPUT_NFS_MAP : IEntityTypeConfiguration<V_INPUT_NFS>
    {
        public void Configure(EntityTypeBuilder<V_INPUT_NFS> builder)
        {
            builder.ToTable("V_INPUT_NFS");
            builder.HasKey(x => x.ORD_ID);
            builder.Property(x => x.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(60).IsRequired();
            builder.Property(x => x.PRO_ID).HasColumnName("PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.CAR_ID).HasColumnName("CAR_ID").HasMaxLength(1).IsRequired();
            builder.Property(x => x.NF_NUMERO).HasColumnName("NF_NUMERO").HasMaxLength(1).IsRequired();
            builder.Property(x => x.NF_SERIE).HasColumnName("NF_SERIE").HasMaxLength(1).IsRequired();
            builder.Property(x => x.NF_EMISSAO).HasColumnName("NF_EMISSAO").IsRequired();
            builder.Property(x => x.NF_QTD).HasColumnName("NF_QTD").IsRequired();

            builder.HasOne(x => x.ConsultaPedido).WithMany(x => x.NFS).HasForeignKey(x => x.ORD_ID);
        }
    }
}

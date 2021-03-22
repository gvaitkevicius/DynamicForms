using DynamicForms.Areas.SGI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.SGI.Maps
{
    public class T_MetasMap : IEntityTypeConfiguration<T_Metas>
    {
        public void Configure(EntityTypeBuilder<T_Metas> builder)
        {
            builder.ToTable("T_METAS");
            builder.HasKey(x => x.MET_ID);
            builder.Property(x => x.MET_ID).HasColumnName("MET_ID").IsRequired();
            builder.Property(x => x.MET_DTINICIO).HasColumnName("MET_DTINICIO").HasMaxLength(8).IsRequired();
            builder.Property(x => x.MET_DTFIM).HasColumnName("MET_DTFIM").HasMaxLength(8).IsRequired();
            builder.Property(x => x.MET_ALVO).HasColumnName("MET_ALVO").HasMaxLength(50).IsRequired();
            builder.Property(x => x.MET_TIPOALVO).HasColumnName("MET_TIPOALVO").IsRequired();
            builder.Property(x => x.IND_ID).HasColumnName("IND_ID").IsRequired();
            builder.Property(x => x.MET_RANGE01).HasColumnName("MET_RANGE01");
            builder.Property(x => x.MET_RANGE02).HasColumnName("MET_RANGE02");
            builder.Property(x => x.MET_RANGE03).HasColumnName("MET_RANGE03");
            builder.Property(x => x.DIM_ID).HasColumnName("DIM_ID");
            builder.Property(x => x.FAT_ID).HasColumnName("FAT_ID").HasMaxLength(100);
            builder.Property(x => x.DIM_SUBDIMENSAO_ID).HasColumnName("DIM_SUBDIMENSAO_ID").HasMaxLength(100);
            builder.Property(x => x.PER_ID).HasColumnName("PER_ID").HasMaxLength(3);
            builder.Property(x => x.DOM_EMPRESA).HasColumnName("DOM_EMPRESA").HasMaxLength(30);
            builder.Property(x => x.DOM_FILIAL).HasColumnName("DOM_FILIAL").HasMaxLength(30);

            builder.HasOne(c => c.T_Indicadores)
                .WithMany(c => c.T_Metas)
                .HasForeignKey(c => c.IND_ID);
        }
    }
}

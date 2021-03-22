using DynamicForms.Areas.SGI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.SGI.Maps
{
    public class vw_SGI_PARAMETRO_RELMEDICOES_MAP : IEntityTypeConfiguration<vw_SGI_PARAMETRO_RELMEDICOES>
    {
        public void Configure(EntityTypeBuilder<vw_SGI_PARAMETRO_RELMEDICOES> builder)
        {
            builder.ToTable("vw_SGI_PARAMETRO_RELMEDICOES");
            builder.HasKey(x => x.MedId);
            builder.Property(x => x.MED_DATA_MEDICAO).HasColumnName("MED_DATA_MEDICAO").HasMaxLength(8);
            builder.Property(x => x.MED_DATA).HasColumnName("MED_DATA");
            builder.Property(x => x.MET_ID).HasColumnName("MET_ID");
            builder.Property(x => x.NEG_ID).HasColumnName("NEG_ID").IsRequired();
            builder.Property(x => x.IND_ID).HasColumnName("IND_ID").IsRequired();
            builder.Property(x => x.UNID).HasColumnName("UNID");
            builder.Property(x => x.UN).HasColumnName("UN").HasMaxLength(20);
            builder.Property(x => x.IND_DESCRICAO).HasColumnName("IND_DESCRICAO").HasMaxLength(200).IsRequired();
            builder.Property(x => x.Mes).HasColumnName("Mes").HasMaxLength(6);
            builder.Property(x => x.Valor).HasColumnName("Valor");
            builder.Property(x => x.META).HasColumnName("META");
            builder.Property(x => x.ValorAc).HasColumnName("ValorAc").IsRequired();
            builder.Property(x => x.METAMES).HasColumnName("METAMES");
            builder.Property(x => x.METACU).HasColumnName("METACU");
            builder.Property(x => x.MED_PONDERACAO).HasColumnName("MED_PONDERACAO").IsRequired();
            builder.Property(x => x.DimId).HasColumnName("DimId").HasMaxLength(200);
            builder.Property(x => x.DimDescricao).HasColumnName("DimDescricao").HasMaxLength(200);
            builder.Property(x => x.FatId).HasColumnName("FatId").HasMaxLength(200);
            builder.Property(x => x.FatDescricao).HasColumnName("FatDescricao").HasMaxLength(200);
            builder.Property(x => x.PerId).HasColumnName("PerId").HasMaxLength(3);
            builder.Property(x => x.PerDescricao).HasColumnName("PerDescricao").HasMaxLength(30);
            builder.Property(x => x.MedId).HasColumnName("MedId").IsRequired();
            builder.Property(x => x.DimSubId).HasColumnName("DimSubId").HasMaxLength(200);
            builder.Property(x => x.DimSubDescricao).HasColumnName("DimSubDescricao").HasMaxLength(200);
        }
    }
}

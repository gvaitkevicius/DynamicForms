using DynamicForms.Areas.SGI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.SGI.Maps
{
    public class T_MedicoesMap : IEntityTypeConfiguration<T_Medicoes>
    {
        public void Configure(EntityTypeBuilder<T_Medicoes> builder)
        {
            builder.ToTable("T_MEDICOES");
            builder.HasKey(x => x.MED_ID);
            builder.Property(x => x.MED_ID).HasColumnName("MED_ID").IsRequired();
            builder.Property(x => x.IND_ID).HasColumnName("IND_ID");
            builder.Property(x => x.MET_ID).HasColumnName("MET_ID");
            builder.Property(x => x.UNI_ID).HasColumnName("UNI_ID");
            builder.Property(x => x.MED_DATA).HasColumnName("MED_DATA").IsRequired();
            builder.Property(x => x.MED_VALOR).HasColumnName("MED_VALOR").HasMaxLength(50);
            builder.Property(x => x.MED_AC_ANO).HasColumnName("MED_AC_ANO").HasMaxLength(70);
            builder.Property(x => x.MED_DATAMEDICAO).HasColumnName("MED_DATAMEDICAO").HasMaxLength(8);
            builder.Property(x => x.MED_PONDERACAO).HasColumnName("MED_PONDERACAO");
            builder.Property(x => x.DIM_ID).HasColumnName("DIM_ID").HasMaxLength(200);
            builder.Property(x => x.DIM_DESCRICAO).HasColumnName("DIM_DESCRICAO").HasMaxLength(200);
            builder.Property(x => x.DIM_SUBDIMENSAO_ID).HasColumnName("DIM_SUBDIMENSAO_ID").HasMaxLength(200);
            builder.Property(x => x.DIM_SUB_DESCRICAO).HasColumnName("DIM_SUB_DESCRICAO").HasMaxLength(200);
            builder.Property(x => x.PER_ID).HasColumnName("PER_ID").HasMaxLength(3);
            builder.Property(x => x.PER_DESCRICAO).HasColumnName("PER_DESCRICAO").HasMaxLength(30);
            builder.Property(x => x.FAT_ID).HasColumnName("FAT_ID").HasMaxLength(200);
            builder.Property(x => x.FAT_DESCRICAO).HasColumnName("FAT_DESCRICAO").HasMaxLength(200);
            builder.Property(x => x.MED_SQL).HasColumnName("MED_SQL").HasMaxLength(8000);
            builder.Property(x => x.DOM_EMPRESA).HasColumnName("DOM_EMPRESA").HasMaxLength(30);
            builder.Property(x => x.DOM_FILIAL).HasColumnName("DOM_FILIAL").HasMaxLength(30);
        }
    }
}

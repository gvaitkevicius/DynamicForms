using DynamicForms.Areas.SGI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.SGI.Maps
{
    public class T_IndicadoresMap : IEntityTypeConfiguration<T_Indicadores>
    {
        public void Configure(EntityTypeBuilder<T_Indicadores> builder)
        {
            builder.ToTable("T_INDICADORES");
            builder.HasKey(x => x.IND_ID);
            builder.Property(x => x.IND_ID).HasColumnName("IND_ID").IsRequired();
            builder.Property(x => x.IND_DESCRICAO).HasColumnName("IND_DESCRICAO").HasMaxLength(200).IsRequired();
            builder.Property(x => x.NEG_ID).HasColumnName("NEG_ID").IsRequired();
            builder.Property(x => x.DESC_CALCULO).HasColumnName("DESC_CALCULO").HasMaxLength(3000);
            builder.Property(x => x.IND_TIPOCOMPARADOR).HasColumnName("IND_TIPOCOMPARADOR").IsRequired();
            builder.Property(x => x.IND_GRAFICO).HasColumnName("IND_GRAFICO");
            builder.Property(x => x.IND_CONEXAO).HasColumnName("IND_CONEXAO").HasMaxLength(100);
            builder.Property(x => x.IND_DTCRIACAO).HasColumnName("IND_DTCRIACAO");
            builder.Property(x => x.RESPOSAVELIND).HasColumnName("RESPOSAVELIND").HasMaxLength(100);
            builder.Property(x => x.RESPOSAVELCARGA).HasColumnName("RESPOSAVELCARGA").HasMaxLength(100);
            builder.Property(x => x.PROCEXTRACAO).HasColumnName("PROCEXTRACAO").HasMaxLength(100);
            builder.Property(x => x.PER_ID).HasColumnName("PER_ID").HasMaxLength(3);
            builder.Property(x => x.DIM_ID).HasColumnName("DIM_ID");
            builder.Property(x => x.DOM_EMPRESA).HasColumnName("DOM_EMPRESA").HasMaxLength(30);
            builder.Property(x => x.DOM_FILIAL).HasColumnName("DOM_FILIAL").HasMaxLength(30);
        }
    }
}

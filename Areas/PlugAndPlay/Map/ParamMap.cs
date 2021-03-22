using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class ParamMap : IEntityTypeConfiguration<Param>
    {
        public void Configure(EntityTypeBuilder<Param> builder)
        {
            builder.ToTable("T_PARAMETROS");
            builder.HasKey(x => x.PAR_ID);
            builder.Property(x => x.PAR_ID).HasColumnName("PAR_ID").HasMaxLength(100).IsRequired();
            builder.Property(x => x.PAR_DESCRICAO).HasColumnName("PAR_DESCRICAO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.PAR_VALOR_S).HasColumnName("PAR_VALOR_S").HasMaxLength(100).IsRequired();
            builder.Property(x => x.PAR_VALOR_N).HasColumnName("PAR_VALOR_N").IsRequired();
            builder.Property(x => x.PAR_VALOR_D).HasColumnName("PAR_VALOR_D").IsRequired();
        }
    }
}
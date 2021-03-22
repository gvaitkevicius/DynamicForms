using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class CalendarioMap : IEntityTypeConfiguration<Calendario>
    {
        public void Configure(EntityTypeBuilder<Calendario> builder)
        {
            builder.ToTable("T_CALENDARIO");
            builder.HasKey(x => x.CAL_ID);
            builder.Property(x => x.CAL_ID).HasColumnName("CAL_ID").IsRequired();
            builder.Property(x => x.CAL_DESCRICAO).HasColumnName("CAL_DESCRICAO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.CAL_DIVIDE_DIA_EM).HasColumnName("CAL_DIVIDE_DIA_EM");
        }
    }

}
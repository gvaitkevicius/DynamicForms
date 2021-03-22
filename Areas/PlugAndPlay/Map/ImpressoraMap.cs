using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class ImpressoraMap : IEntityTypeConfiguration<Impressora>
    {
        public void Configure(EntityTypeBuilder<Impressora> builder)
        {
            builder.ToTable("T_IMPRESSORAS");
            builder.HasKey(x => x.IMP_ID);
            builder.Property(x => x.IMP_ID).HasColumnName("IMP_ID").IsRequired();
            builder.Property(x => x.IMP_IP).HasColumnName("IMP_IP").HasMaxLength(20);
            builder.Property(x => x.IMP_NOME).HasColumnName("IMP_NOME").HasMaxLength(100);
        }
    }
}

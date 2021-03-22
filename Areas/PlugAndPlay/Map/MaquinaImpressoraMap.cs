using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class MaquinaImpressoraMap : IEntityTypeConfiguration<MaquinaImpressora>
    {
        public void Configure(EntityTypeBuilder<MaquinaImpressora> builder)
        {
            builder.ToTable("T_MAQUINA_IMPRESSORA");
            builder.HasKey(x => new { x.MAQ_ID, x.IMP_ID });
            builder.Property(x => x.MAQ_ID).HasColumnName("MAQ_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.IMP_ID).HasColumnName("IMP_ID").IsRequired();
            builder.HasOne(x => x.Maquina).WithMany(m => m.MaquinaImpressora).HasForeignKey(x => x.MAQ_ID);
            builder.HasOne(x => x.Impressora).WithMany(i => i.MaquinaImpressora).HasForeignKey(x => x.IMP_ID);
        }
    }
}

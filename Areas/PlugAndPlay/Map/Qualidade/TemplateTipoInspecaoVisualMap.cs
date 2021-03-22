using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map.Qualidade
{
    public class TemplateTipoInspecaoVisualMap : IEntityTypeConfiguration<TemplateTipoInspecaoVisual>
    {
        public void Configure(EntityTypeBuilder<TemplateTipoInspecaoVisual> builder)
        {
            builder.ToTable("T_TEMPLATE_TIPO_INSPECAO_VISUAL");
            builder.HasKey(x => x.TTI_ID);
            builder.Property(x => x.TTI_ID).HasColumnName("TTI_ID").IsRequired();
            builder.Property(x => x.TIV_ID).HasColumnName("TIV_ID");
            builder.Property(x => x.TEM_ID).HasColumnName("TEM_ID");

            builder.HasOne(x => x.TipoInspecaoVisual).WithMany(u => u.TemplateTipoInspecaoVisual).HasForeignKey(x => x.TIV_ID);
            builder.HasOne(x => x.TemplateDeTestes).WithMany(u => u.TemplateTipoInspecaoVisual).HasForeignKey(x => x.TEM_ID);
        }
    }
}

using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map.Qualidade
{
    public class TemplateTipoTesteMap : IEntityTypeConfiguration<TemplateTipoTeste>
    {
        public void Configure(EntityTypeBuilder<TemplateTipoTeste> builder)
        {
            builder.ToTable("T_TEMPLATE_TIPO_TESTE");
            builder.HasKey(x => x.TTT_ID);
            builder.Property(x => x.TTT_ID).HasColumnName("TTT_ID").IsRequired();
            builder.Property(x => x.TT_ID).HasColumnName("TT_ID").IsRequired();
            builder.Property(x => x.TEM_ID).HasColumnName("TEM_ID").IsRequired();
            builder.HasOne(x => x.TemplateDeTestes).WithMany(y => y.TemplateTipoTeste).HasForeignKey(x => x.TEM_ID);
            builder.HasOne(x => x.TipoTeste).WithMany(y => y.TemplateTipoTeste).HasForeignKey(x => x.TT_ID);
        }
    }
}

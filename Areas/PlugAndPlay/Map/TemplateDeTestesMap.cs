using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class TemplateDeTestesMap : IEntityTypeConfiguration<TemplateDeTestes>
    {
        public void Configure(EntityTypeBuilder<TemplateDeTestes> builder)
        {
            builder.ToTable("T_TEMPLATE_DE_TESTES");
            builder.HasKey(x => x.TEM_ID);
            builder.Property(x => x.TEM_ID).HasColumnName("TEM_ID").IsRequired();
            builder.Property(x => x.TEM_DESCRICAO).HasColumnName("TEM_DESCRICAO").HasMaxLength(200);
            builder.Property(x => x.Observacao).HasColumnName("TEM_OBS").HasMaxLength(4000);
        }
    }
}
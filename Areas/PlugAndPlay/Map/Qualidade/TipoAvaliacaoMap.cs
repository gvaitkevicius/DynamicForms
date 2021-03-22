using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map.Qualidade
{
    public class TipoAvaliacaoMap : IEntityTypeConfiguration<TipoAvaliacao>
    {
        public void Configure(EntityTypeBuilder<TipoAvaliacao> builder)
        {
            builder.ToTable("T_TIPO_AVALIACAO");
            builder.HasKey(x => x.TA_ID);
            builder.Property(x => x.TA_ID).HasColumnName("TA_ID").IsRequired();
            builder.Property(x => x.TA_DESC).HasColumnName("TA_DESC").HasMaxLength(200);
        }
    }
}

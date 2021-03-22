using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;

namespace DynamicForms.Areas.PlugAndPlay.Map
{
    public class RelatoriosMap : IEntityTypeConfiguration<Relatorios>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Relatorios> builder)
        {
            builder.ToTable("T_RELATORIOS");
            builder.HasKey(x => x.REL_ID);
            builder.Property(x => x.REL_ID).HasColumnName("REL_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.REL_NOME_RELATORIO).HasColumnName("REL_NOME_RELATORIO").HasMaxLength(30);
            builder.Property(x => x.REL_NOME_CAMPO).HasColumnName("REL_NOME_CAMPO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.REL_TIPO_CAMPO).HasColumnName("REL_TIPO_CAMPO").HasMaxLength(50).IsRequired();
            builder.Property(x => x.REL_POS_X).HasColumnName("REL_POS_X");
            builder.Property(x => x.REL_POS_Y).HasColumnName("REL_POS_Y");
            builder.Property(x => x.REL_TAMANHO_FONTE).HasColumnName("REL_TAMANHO_FONTE");
        }
    }
}

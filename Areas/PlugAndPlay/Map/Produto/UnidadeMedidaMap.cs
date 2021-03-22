using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DynamicForms.Areas.PlugAndPlay.Models
{

    public class UnidadeMedidaMap : IEntityTypeConfiguration<UnidadeMedida>
    {
        public void Configure(EntityTypeBuilder<UnidadeMedida> builder)
        {
            builder.ToTable("T_UNIDADE_MEDIDA");
            builder.HasKey(x => x.UNI_ID);
            builder.Property(x => x.UNI_ID).HasColumnName("UNI_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.UNI_DESCRICAO).HasColumnName("UNI_DESCRICAO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.UNI_ESCALA_TEMPO).HasColumnName("UNI_ESCALA_TEMPO").HasMaxLength(1);
        }
    }
}
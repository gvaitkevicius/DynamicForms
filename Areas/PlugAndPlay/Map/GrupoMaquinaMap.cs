using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class GrupoMaquinaMap : IEntityTypeConfiguration<GrupoMaquina>
    {
        public void Configure(EntityTypeBuilder<GrupoMaquina> builder)
        {
            builder.ToTable("T_GRUPO_MAQUINA");
            builder.HasKey(x => x.GMA_ID);
            builder.Property(x => x.GMA_ID).HasColumnName("GMA_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.GMA_DESCRICAO).HasColumnName("GMA_DESCRICAO").HasMaxLength(100);
            builder.Property(x => x.GMA_TIPO_PLANEJAMENTO).HasColumnName("GMA_TIPO_PLANEJAMENTO").HasMaxLength(30);
        }
    }
}
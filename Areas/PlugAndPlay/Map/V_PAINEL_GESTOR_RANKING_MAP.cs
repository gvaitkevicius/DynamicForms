using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map
{
    public class V_PAINEL_GESTOR_RANKING_MAP : IEntityTypeConfiguration<V_PAINEL_GESTOR_RANKING>
    {
        public void Configure(EntityTypeBuilder<V_PAINEL_GESTOR_RANKING> builder)
        {
            builder.ToTable("V_PAINEL_GESTOR_RANKING");
            builder.HasKey(x => x.USE_NOME);
            builder.Property(x => x.ORDEM).HasColumnName("ORDEM").HasMaxLength(1).IsRequired();
            builder.Property(x => x.DT).HasColumnName("DT").HasMaxLength(3).IsRequired();
            builder.Property(x => x.MED_VALOR).HasColumnName("MED_VALOR");
            builder.Property(x => x.USE_NOME).HasColumnName("USE_NOME").HasMaxLength(80).IsRequired();
        }
    }
}

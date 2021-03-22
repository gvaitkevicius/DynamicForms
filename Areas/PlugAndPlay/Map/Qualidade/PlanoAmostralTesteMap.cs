using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map.Qualidade
{
    public class PlanoAmostralTesteMap : IEntityTypeConfiguration<PlanoAmostralTeste>
    {
        public void Configure(EntityTypeBuilder<PlanoAmostralTeste> builder)
        {
            builder.ToTable("T_PLANO_AMOSTRAL_TESTE");
            builder.HasKey(x => x.PAT_ID);
            builder.Property(x => x.PAT_ID).HasColumnName("PAT_ID").IsRequired();
            builder.Property(x => x.PAT_QTD_CAIXAS_DE).HasColumnName("PAT_QTD_CAIXAS_DE");
            builder.Property(x => x.PAT_QTD_CAIXAS_ATE).HasColumnName("PAT_QTD_CAIXAS_ATE");
            builder.Property(x => x.PAT_N_AMOSTRAGEM).HasColumnName("PAT_N_AMOSTRAGEM");
            builder.Property(x => x.PAT_PERCENT_ESPECIF).HasColumnName("PAT_PERCENT_ESPECIF");
        }
    }
}

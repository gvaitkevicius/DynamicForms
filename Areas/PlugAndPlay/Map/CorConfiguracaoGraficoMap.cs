
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class CorConfiguracaoGraficoMap : IEntityTypeConfiguration<CorConfiguracaoGrafico>
    {
        public CorConfiguracaoGraficoMap()
        {
            /* Migração do EntityFramework para o EntityCore, o construtor foi substituido pelo método
             * public void Configure(EntityTypeBuilder<> builder), que é a implementação da interface
             * IEntityTypeConfiguration<>
            ToTable("T_CORES_CONFIGURACAO_GRAFICO");
            Property(x => x.Id).HasColumnName("COR_ID").HasMaxLength(2);
            Property(x => x.PercentualIni).HasColumnName("COR_PERCENTUAL_INI");
            Property(x => x.PercentualFim).HasColumnName("COR_PERCENTUAL_FIM");
            Property(x => x.Descricao).HasColumnName("COR_DESCRICAO").HasMaxLength(30);

            HasKey(x => x.Id);
            */
        }

        public void Configure(EntityTypeBuilder<CorConfiguracaoGrafico> builder)
        {
            builder.ToTable("T_CORES_CONFIGURACAO_GRAFICO");
            builder.HasKey(x => x.COR_ID);
            builder.Property(x => x.COR_ID).HasColumnName("COR_ID").HasMaxLength(2).IsRequired();
            builder.Property(x => x.COR_PERCENTUAL_INI).HasColumnName("COR_PERCENTUAL_INI").IsRequired();
            builder.Property(x => x.COR_PERCENTUAL_FIM).HasColumnName("COR_PERCENTUAL_FIM").IsRequired();
            builder.Property(x => x.COR_DESCRICAO).HasColumnName("COR_DESCRICAO").HasMaxLength(30).IsRequired();
        }
    }
}
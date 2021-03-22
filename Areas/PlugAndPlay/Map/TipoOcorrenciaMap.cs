using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class TipoOcorrenciaMap : IEntityTypeConfiguration<TipoOcorrencia>
    {
        public TipoOcorrenciaMap()
        {
            /* Migração do EntityFramework para o EntityCore, o construtor foi substituido pelo método
             * public void Configure(EntityTypeBuilder<> builder), que é a implementação da interface
             * IEntityTypeConfiguration<>
            ToTable("T_TIPO_OCORRENCIA");
            Property(x => x.Id).HasColumnName("TIP_ID");
            Property(x => x.Descricao).HasColumnName("TIP_DESCRICAO").IsRequired().HasMaxLength(100);
            Property(x => x.Spr).HasColumnName("SPR").IsOptional();
            HasKey(x => x.Id);
            */
        }

        public void Configure(EntityTypeBuilder<TipoOcorrencia> builder)
        {
            builder.ToTable("T_TIPO_OCORRENCIA");
            builder.Property(x => x.Id).HasColumnName("TIP_ID");
            builder.Property(x => x.Descricao).HasColumnName("TIP_DESCRICAO").IsRequired().HasMaxLength(100);
            builder.Property(x => x.Spr).HasColumnName("SPR");
            builder.HasKey(x => x.Id);
        }
    }
}
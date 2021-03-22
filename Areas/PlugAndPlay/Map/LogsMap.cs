using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class LogsMap : IEntityTypeConfiguration<Logs>
    {
        public void Configure(EntityTypeBuilder<Logs> builder)
        {
            builder.ToTable("T_LOGS");
            builder.HasKey(x => x.LOG_CHAVE);
            builder.Property(x => x.LOG_CHAVE).HasColumnName("LOG_CHAVE").HasMaxLength(100);
            builder.Property(x => x.LOG_CONTEXTO).HasColumnName("LOG_CONTEXTO").HasMaxLength(100);
            builder.Property(x => x.LOG_CONTEUDO).HasColumnName("LOG_CONTEUDO").HasMaxLength(3500);
            builder.Property(x => x.LOG_ID).HasColumnName("LOG_ID").IsRequired();
            builder.Property(x => x.LOG_EMISSAO).HasColumnName("LOG_EMISSAO");
        }
    }
}

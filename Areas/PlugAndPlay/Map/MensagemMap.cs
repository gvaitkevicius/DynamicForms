using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class MensagemMap : IEntityTypeConfiguration<Mensagem>
    {
        public MensagemMap()
        {
            /* Migração do EntityFramework para o EntityCore, o construtor foi substituido pelo método
             * public void Configure(EntityTypeBuilder<> builder), que é a implementação da interface
             * IEntityTypeConfiguration<>
            ToTable("T_MENSAGENS");
            Property(x => x.MEN_ID).HasColumnName("MEN_ID").HasMaxLength(100);
            Property(x => x.MEN_SEND).HasColumnName("MEN_SEND").IsRequired().HasMaxLength(8000);
            Property(x => x.MEN_EMISSION).HasColumnName("MEN_EMISSION").IsRequired();
            Property(x => x.MEN_STATUS).HasColumnName("MEN_STATUS").IsRequired().HasMaxLength(30);
            Property(x => x.MEN_RECEIVE).HasColumnName("MEN_RECEIVE").IsRequired().HasMaxLength(8000);
            Property(x => x.MEN_TYPE).HasColumnName("MEN_TYPE").IsRequired().HasMaxLength(30);
            Property(x => x.MEN_QTD_TRY_SEND).HasColumnName("MEN_QTD_TRY_SEND");
            Property(x => x.MEN_DATE_TRY_SEND).HasColumnName("MEN_DATE_TRY_SEND").IsRequired();
            HasKey(x => x.MEN_ID);
            */
        }

        public void Configure(EntityTypeBuilder<Mensagem> builder)
        {
            builder.ToTable("T_MENSAGENS");
            builder.Property(x => x.MEN_ID).HasColumnName("MEN_ID").HasMaxLength(100);
            builder.Property(x => x.MEN_SEND).HasColumnName("MEN_SEND").IsRequired().HasMaxLength(8000);
            builder.Property(x => x.MEN_EMISSION).HasColumnName("MEN_EMISSION").IsRequired();
            builder.Property(x => x.MEN_STATUS).HasColumnName("MEN_STATUS").IsRequired().HasMaxLength(30);
            builder.Property(x => x.MEN_RECEIVE).HasColumnName("MEN_RECEIVE").IsRequired().HasMaxLength(8000);
            builder.Property(x => x.MEN_TYPE).HasColumnName("MEN_TYPE").IsRequired().HasMaxLength(30);
            builder.Property(x => x.MEN_QTD_TRY_SEND).HasColumnName("MEN_QTD_TRY_SEND");
            builder.Property(x => x.MEN_DATE_TRY_SEND).HasColumnName("MEN_DATE_TRY_SEND").IsRequired();
            builder.HasKey(x => x.MEN_ID);
        }
    }
}
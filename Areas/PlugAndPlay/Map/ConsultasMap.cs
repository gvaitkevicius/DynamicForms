using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class ConsultasMap : IEntityTypeConfiguration<Consultas>
    {

        public void Configure(EntityTypeBuilder<Consultas> builder)
        {
            builder.ToTable("T_CONSULTAS");
            builder.HasKey(x => x.CON_ID);
            builder.Property(x => x.CON_ID).HasColumnName("CON_ID");
            builder.Property(x => x.CON_DESCRICAO).HasColumnName("CON_DESCRICAO").HasMaxLength(100);
            builder.Property(x => x.CON_GRUPO).HasColumnName("CON_GRUPO").HasMaxLength(20);
            builder.Property(x => x.CON_COMAND).HasColumnName("CON_COMAND");
            builder.Property(x => x.CON_CONEXAO).HasColumnName("CON_CONEXAO").HasMaxLength(60);
            builder.Property(x => x.CON_TITULO).HasColumnName("CON_TITULO").HasMaxLength(60);
            builder.Property(x => x.CON_TIPO).HasColumnName("CON_TIPO").HasMaxLength(10);
        }
    }
}

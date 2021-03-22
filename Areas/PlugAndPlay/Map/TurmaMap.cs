using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class TurmaMap : IEntityTypeConfiguration<Turma>
    {
        /*
        * Migração do EntityFramework para o EntityCore, o construtor foi substituido pelo método
        * public void Configure(EntityTypeBuilder<> builder), que é a implementação da interface
        * IEntityTypeConfiguration<>
        * 
        public TurmaMap()
        {
            ToTable("T_TURMA");
            Property(x => x.Id).HasColumnName("TURM_ID").HasMaxLength(10);
            Property(x => x.Descricao).HasColumnName("TURM_DESCRICAO").IsRequired().HasMaxLength(100);

            HasKey(x => x.Id);
        }
        */
        public void Configure(EntityTypeBuilder<Turma> builder)
        {
            builder.ToTable("T_TURMA");
            builder.Property(x => x.Id).HasColumnName("TURM_ID").HasMaxLength(10);
            builder.Property(x => x.Descricao).HasColumnName("TURM_DESCRICAO").IsRequired().HasMaxLength(100);

            builder.HasKey(x => x.Id);
        }
    }
}
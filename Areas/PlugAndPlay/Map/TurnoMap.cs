using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class TurnoMap : IEntityTypeConfiguration<Turno>
    {
        /*
        * Migração do EntityFramework para o EntityCore, o construtor foi substituido pelo método
        * public void Configure(EntityTypeBuilder<> builder), que é a implementação da interface
        * IEntityTypeConfiguration<>
        * 
        public TurnoMap()
        {
            ToTable("T_TURNO");
            Property(x => x.Id).HasColumnName("TURN_ID").HasMaxLength(10);
            Property(x => x.Descricao).HasColumnName("TURN_DESCRICAO").IsRequired().HasMaxLength(100);

            HasKey(x => x.Id);
        }
        */
        public void Configure(EntityTypeBuilder<Turno> builder)
        {
            builder.ToTable("T_TURNO");
            builder.Property(x => x.Id).HasColumnName("TURN_ID").HasMaxLength(10);
            builder.Property(x => x.Descricao).HasColumnName("TURN_DESCRICAO").IsRequired().HasMaxLength(100);

            builder.HasKey(x => x.Id);
        }
    }
}
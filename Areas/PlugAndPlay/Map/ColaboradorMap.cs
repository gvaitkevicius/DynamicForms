using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class ColaboradorMap : IEntityTypeConfiguration<Colaborador>
    {
        public ColaboradorMap()
        {
            /* Migração do EntityFramework para o EntityCore, o construtor foi substituido pelo método
             * public void Configure(EntityTypeBuilder<> builder), que é a implementação da interface
             * IEntityTypeConfiguration<>
            ToTable("T_COLABORADOR");
            Property(x => x.Cpf).HasColumnName("COL_CPF").HasMaxLength(14);
            Property(x => x.Nome).HasColumnName("COL_NOME").HasMaxLength(100).IsRequired();
            Property(x => x.DataNascimento).HasColumnName("COL_NASCIMENTO").IsRequired();
            Property(x => x.Email).HasColumnName("COL_EMAIL").HasMaxLength(150).IsRequired();
            Property(x => x.Matricula).HasColumnName("COL_MATRICULA").HasMaxLength(10).IsRequired();
            Property(x => x.TurmaId).HasColumnName("TURM_ID").HasMaxLength(10).IsRequired();

            HasKey(x => x.Cpf);
            HasRequired(x => x.Turma).WithMany(x => x.Colaboradores).HasForeignKey(x => x.TurmaId);
            */
        }

        public void Configure(EntityTypeBuilder<Colaborador> builder)
        {
            builder.ToTable("T_COLABORADOR");
            builder.HasKey(x => x.COL_CPF);
            builder.Property(x => x.COL_CPF).HasColumnName("COL_CPF").HasMaxLength(14).IsRequired();
            builder.Property(x => x.COL_NOME).HasColumnName("COL_NOME").HasMaxLength(100).IsRequired();
            builder.Property(x => x.COL_NASCIMENTO).HasColumnName("COL_NASCIMENTO").IsRequired();
            builder.Property(x => x.COL_EMAIL).HasColumnName("COL_EMAIL").HasMaxLength(150).IsRequired();
            builder.Property(x => x.COL_MATRICULA).HasColumnName("COL_MATRICULA").HasMaxLength(10).IsRequired();
            builder.Property(x => x.TURM_id).HasColumnName("TURM_id").HasMaxLength(10).IsRequired();
            builder.HasOne(x => x.Turma).WithMany(x => x.Colaboradores).HasForeignKey(x => x.TURM_id);
        }
    }
}
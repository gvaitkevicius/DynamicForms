//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.SGI.Model
{

    public class T_MESES
    {
        public string MES { get; set; }
        public int fator { get; set; }
    }

    class T_MESES_ResultConfiguration : IEntityTypeConfiguration<T_MESES>
    {
        /*
        * Migra??o do EntityFramework para o EntityCore, o construtor foi substituido pelo m?todo
        * public void Configure(EntityTypeBuilder<> builder), que ? a implementa??o da interface
        * IEntityTypeConfiguration<>
        * 
        public T_MESES_ResultConfiguration()
        {
            this.HasKey(c => c.MES);

            this.Property(c => c.MES)
                .HasMaxLength(2)
                .HasColumnName("MES")
                .IsRequired();

            this.Property(c => c.fator)
                .HasColumnName("fator")
                .IsRequired();

            // Configurando a Tabela
            this.ToTable("T_MESES");
        }
        */

        public void Configure(EntityTypeBuilder<T_MESES> builder)
        {
            builder.HasKey(c => c.MES);

            builder.Property(c => c.MES)
                .HasMaxLength(2)
                .HasColumnName("MES")
                .IsRequired();

            builder.Property(c => c.fator)
                .HasColumnName("fator")
                .IsRequired();

            // Configurando a Tabela
            builder.ToTable("T_MESES");
        }
    }
}

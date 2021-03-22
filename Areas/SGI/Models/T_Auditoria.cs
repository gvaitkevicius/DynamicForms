using DynamicForms.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace DynamicForms.Areas.SGI.Model
{
    public class T_Auditoria
    {
        public int ID { get; set; }
        public DateTime DATA { get; set; }
        public int ID_USUARIO { get; set; }
        public virtual T_Usuario T_Usuario { get; set; }
        public string ROTINA { get; set; }
        public string HISTORICO { get; set; }
        public string CHAVE { get; set; }
    }

    class T_Auditoria_ResultConfiguration : IEntityTypeConfiguration<T_Auditoria>
    {
        /*
        * Migração do EntityFramework para o EntityCore, o construtor foi substituido pelo método
        * public void Configure(EntityTypeBuilder<> builder), que é a implementação da interface
        * IEntityTypeConfiguration<>
        * 
        public T_Auditoria_ResultConfiguration()
        {
            // Configurando propriedades e chaves
            this.HasKey(c => c.ID);

            this.Property(c => c.DATA)
                .HasColumnName("DATA")
                .IsRequired();

            this.Property(c => c.ID_USUARIO)
                .HasColumnName("ID_USUARIO");

            this.HasRequired(e => e.T_Usuario)
                .WithMany(t => t.T_Auditoria)
                .HasForeignKey(c => c.ID_USUARIO);

            this.Property(c => c.ROTINA)
                .HasColumnName("ROTINA")
                .HasMaxLength(100)
                .IsRequired();

            this.Property(c => c.HISTORICO)
                .HasColumnName("HISTORICO")
                .HasMaxLength(3000)
                .IsOptional();

            this.Property(c => c.CHAVE)
                .HasColumnName("CHAVE")
                .HasMaxLength(100)
                .IsOptional();

            // Configurando a Tabela
            this.ToTable("T_AUDITORIA");
        }
        */

        public void Configure(EntityTypeBuilder<T_Auditoria> builder)
        {
            // Configurando propriedades e chaves
            builder.HasKey(c => c.ID);

            builder.Property(c => c.DATA)
                .HasColumnName("DATA")
                .IsRequired();

            builder.Property(c => c.ID_USUARIO)
                .HasColumnName("ID_USUARIO");

            builder.HasOne(e => e.T_Usuario)
                .WithMany(t => t.T_Auditoria)
                .HasForeignKey(c => c.ID_USUARIO);

            builder.Property(c => c.ROTINA)
                .HasColumnName("ROTINA")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.HISTORICO)
                .HasColumnName("HISTORICO")
                .HasMaxLength(3000);

            builder.Property(c => c.CHAVE)
                .HasColumnName("CHAVE")
                .HasMaxLength(100);

            // Configurando a Tabela
            builder.ToTable("T_AUDITORIA");
        }
    }
}
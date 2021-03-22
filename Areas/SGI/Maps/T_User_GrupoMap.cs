using DynamicForms.Areas.SGI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.SGI.Maps
{
    public class T_User_GrupoMap : IEntityTypeConfiguration<T_USER_GRUPO>
    {
        public void Configure(EntityTypeBuilder<T_USER_GRUPO> builder)
        {
            // Configurando propriedades e chaves
            builder.HasKey(c => new { c.GRU_ID, c.ID_USUARIO });

            builder.Property(c => c.GRU_ID)
                .HasColumnName("GRU_ID")
                .IsRequired();

            builder.HasOne(c => c.T_Grupo)
                .WithMany(c => c.T_USER_GRUPO)
                .HasForeignKey(c => c.GRU_ID);

            builder.Property(c => c.ID_USUARIO)
                .HasColumnName("USE_ID")
                .IsRequired();

            builder.HasOne(c => c.T_Usuario)
                .WithMany(c => c.T_USER_GRUPO)
                .HasForeignKey(c => c.ID_USUARIO);

            // Configurando a Tabela
            builder.ToTable("T_USER_GRUPO");
        }
    }
}

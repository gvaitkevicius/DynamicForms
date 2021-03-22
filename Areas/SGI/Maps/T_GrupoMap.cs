using DynamicForms.Areas.SGI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.SGI.Maps
{
    public class T_GrupoMap : IEntityTypeConfiguration<T_Grupo>
    {
        public void Configure(EntityTypeBuilder<T_Grupo> builder)
        {
            builder.ToTable("T_GRUPO");
            builder.HasKey(x => x.GRU_ID);
            builder.Property(x => x.GRU_ID).HasColumnName("GRU_ID").IsRequired();
            builder.Property(x => x.NOME).HasColumnName("GRU_NOME").HasMaxLength(80).IsRequired();
            builder.Property(x => x.EXIBELISTA).HasColumnName("GRU_EXIBELISTA").IsRequired();
            builder.Property(x => x.GRU_DESCRICAO).HasColumnName("GRU_DESCRICAO").HasMaxLength(2000).IsRequired();
        }

    }
}

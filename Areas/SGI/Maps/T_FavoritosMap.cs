using DynamicForms.Areas.SGI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.SGI.Maps
{
    public class T_FavoritosMap : IEntityTypeConfiguration<T_Favoritos>
    {
        public void Configure(EntityTypeBuilder<T_Favoritos> builder)
        {
            builder.ToTable("T_FAVORITOS");
            builder.HasKey(x => x.IDFAVORITO);
            builder.Property(x => x.IDFAVORITO).HasColumnName("IDFAVORITO").IsRequired();
            builder.Property(x => x.USE_ID).HasColumnName("USE_ID").IsRequired();
            builder.Property(x => x.ID_INDICADOR).HasColumnName("ID_INDICADOR").IsRequired();
            builder.HasOne(x => x.t_usuario).WithMany(u => u.T_Favoritos).HasForeignKey(x => x.USE_ID);
            builder.HasOne(x => x.t_indicadores).WithMany(u => u.T_Favoritos).HasForeignKey(x => x.ID_INDICADOR);
        }
    }
}

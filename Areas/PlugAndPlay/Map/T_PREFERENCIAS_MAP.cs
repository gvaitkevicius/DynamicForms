using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map
{
    public class T_PREFERENCIAS_MAP : IEntityTypeConfiguration<T_PREFERENCIAS>
    {
        public void Configure(EntityTypeBuilder<T_PREFERENCIAS> builder)
        {
            builder.ToTable("T_PREFERENCIAS");
            builder.HasKey(x => x.PRE_ID);
            builder.Property(x => x.PRE_ID).HasColumnName("PRE_ID").IsRequired();
            builder.Property(x => x.PRE_DESCRICAO).HasColumnName("PRE_DESCRICAO").HasMaxLength(140);
            builder.Property(x => x.PRE_NAMESPACE).HasColumnName("PRE_NAMESPACE").HasMaxLength(100);
            builder.Property(x => x.PRE_TIPO).HasColumnName("PRE_TIPO").HasMaxLength(50);
            builder.Property(x => x.PRE_VALOR).HasColumnName("PRE_VALOR").HasMaxLength(50);
            builder.Property(x => x.USE_ID).HasColumnName("USE_ID");
            builder.Property(x => x.PER_ID).HasColumnName("PER_ID");

            builder.HasOne(x => x.T_Usuario).WithMany(x => x.T_PREFERENCIAS).HasForeignKey(x => x.USE_ID);
            builder.HasOne(x => x.T_Perfil).WithMany(x => x.T_PREFERENCIAS).HasForeignKey(x => x.PER_ID);
        }
    }
}

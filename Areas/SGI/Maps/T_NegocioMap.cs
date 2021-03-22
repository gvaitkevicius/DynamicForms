using DynamicForms.Areas.SGI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.SGI.Maps
{
    public class T_NegocioMap : IEntityTypeConfiguration<T_Negocio>
    {
        public void Configure(EntityTypeBuilder<T_Negocio> builder)
        {
            builder.ToTable("T_NEGOCIO");
            builder.HasKey(x => x.NEG_ID);
            builder.Property(x => x.NEG_ID).HasColumnName("NEG_ID").IsRequired();
            builder.Property(x => x.NEG_DESCRICAO).HasColumnName("NEG_DESCRICAO").HasMaxLength(80).IsRequired();
        }
    }
}

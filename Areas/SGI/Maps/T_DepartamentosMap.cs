using DynamicForms.Areas.SGI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.SGI.Maps
{
    public class T_DepartamentosMap : IEntityTypeConfiguration<T_Departamentos>
    {
        public void Configure(EntityTypeBuilder<T_Departamentos> builder)
        {
            builder.ToTable("T_DEPARTAMENTOS");
            builder.HasKey(x => x.DEP_ID);
            builder.Property(x => x.DEP_ID).HasColumnName("DEP_ID").IsRequired();
            builder.Property(x => x.DEP_NOME).HasColumnName("DEP_NOME").HasMaxLength(80).IsRequired();
        }
    }
}

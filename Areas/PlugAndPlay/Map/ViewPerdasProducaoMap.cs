using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map
{
    public class ViewPerdasProducaoMap : IEntityTypeConfiguration<ViewPerdasProducao>
    {
        public void Configure(EntityTypeBuilder<ViewPerdasProducao> builder)
        {
            builder.ToTable("V_PERDAS_PRODUCAO");
            builder.HasKey(x => x.ORD_ID);
            builder.Property(x => x.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(60);
            builder.Property(x => x.MAQ_ID).HasColumnName("MAQ_ID").HasMaxLength(60);
            builder.Property(x => x.MOV_QUANTIDADE).HasColumnName("MOV_QUANTIDADE").IsRequired();
        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class ProdutoTintaMap : IEntityTypeConfiguration<ProdutoTinta>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ProdutoTinta> builder)
        {
            builder.Property(x => x.PRO_SUB_ESCALA_COR).HasColumnName("PRO_SUB_ESCALA_COR");
            builder.Property(x => x.PRO_ESCALA_COR).HasColumnName("PRO_ESCALA_COR");
            builder.Property(x => x.PRO_CUSTO_SUBIDA_ESCALA_COR).HasColumnName("PRO_CUSTO_SUBIDA_ESCALA_COR");
            builder.Property(x => x.PRO_CUSTO_DECIDA_ESCALA_COR).HasColumnName("PRO_CUSTO_DECIDA_ESCALA_COR");
            builder.Property(x => x.TEM_ID).HasColumnName("TEM_ID");
            builder.Property(x => x.UNI_ID).HasColumnName("UNI_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.GRP_ID).HasColumnName("GRP_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.PRO_COLOR_HEXA).HasColumnName("PRO_COLOR_HEXA").HasMaxLength(6);
            builder.Property(x => x.PRO_GRUPO_PALETIZACAO).HasColumnName("PRO_GRUPO_PALETIZACAO");
            

            builder.HasOne(me => me.TemplateDeTestes).WithMany(u => u.ProdutoTinta).HasForeignKey(me => me.TEM_ID);
            builder.HasOne(x => x.UnidadeMedida).WithMany(um => um.ProdutoTinta).HasForeignKey(x => x.UNI_ID);
            builder.HasOne(x => x.GrupoProdutoOutros).WithMany(um => um.ProdutoTinta).HasForeignKey(x => x.GRP_ID);
            builder.HasOne(x => x.GrupoPaletizacao).WithMany(um => um.ProdutoTinta).HasForeignKey(x => x.PRO_GRUPO_PALETIZACAO);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class ProdutoFacaMap : IEntityTypeConfiguration<ProdutoFaca>
    {
        public void Configure(EntityTypeBuilder<ProdutoFaca> builder)
        {
            builder.Property(x => x.TEM_ID).HasColumnName("TEM_ID");
            builder.Property(x => x.UNI_ID).HasColumnName("UNI_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.GRP_ID).HasColumnName("GRP_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.PRO_GRUPO_PALETIZACAO).HasColumnName("PRO_GRUPO_PALETIZACAO");

            builder.HasOne(me => me.TemplateDeTestes).WithMany(u => u.ProdutoFacas).HasForeignKey(me => me.TEM_ID);
            builder.HasOne(x => x.UnidadeMedida).WithMany(um => um.ProdutoFacas).HasForeignKey(x => x.UNI_ID);
            builder.HasOne(x => x.GrupoProdutoOutros).WithMany(gp => gp.ProdutoFacas).HasForeignKey(x => x.GRP_ID);
            builder.HasOne(x => x.GrupoPaletizacao).WithMany(um => um.ProdutoFaca).HasForeignKey(x => x.PRO_GRUPO_PALETIZACAO);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class ProdutoPaleteMap : IEntityTypeConfiguration<ProdutoPalete>
    {
        public void Configure(EntityTypeBuilder<ProdutoPalete> builder)
        {
            builder.Property(x => x.TEM_ID).HasColumnName("TEM_ID");
            builder.Property(x => x.UNI_ID).HasColumnName("UNI_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.GRP_ID).HasColumnName("GRP_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.PRO_LARGURA_PECA).HasColumnName("PRO_LARGURA_PECA");
            builder.Property(x => x.PRO_COMPRIMENTO_PECA).HasColumnName("PRO_COMPRIMENTO_PECA");
            builder.Property(x => x.PRO_ALTURA_PECA).HasColumnName("PRO_ALTURA_PECA");
            builder.Property(x => x.PRO_GRUPO_PALETIZACAO).HasColumnName("PRO_GRUPO_PALETIZACAO");
            builder.Property(x => x.PRO_ESTOQUE_ATUAL).HasColumnName("PRO_ESTOQUE_ATUAL");
            builder.Property(x => x.PRO_PESO).HasColumnName("PRO_PESO");

            builder.HasOne(me => me.TemplateDeTestes).WithMany(u => u.ProdutoPalete).HasForeignKey(me => me.TEM_ID);
            builder.HasOne(x => x.UnidadeMedida).WithMany(um => um.ProdutoPalete).HasForeignKey(x => x.UNI_ID);
            builder.HasOne(x => x.GrupoProdutoPalete).WithMany(um => um.ProdutoPalete).HasForeignKey(x => x.GRP_ID);
            builder.HasOne(x => x.GrupoPaletizacao).WithMany(um => um.ProdutoPalete).HasForeignKey(x => x.PRO_GRUPO_PALETIZACAO);
        }
    }
}

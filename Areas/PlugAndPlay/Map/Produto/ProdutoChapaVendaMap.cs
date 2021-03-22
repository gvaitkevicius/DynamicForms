using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class ProdutoChapaVendaMap : IEntityTypeConfiguration<ProdutoChapaVenda>
    {
        public void Configure(EntityTypeBuilder<ProdutoChapaVenda> builder)
        {
            builder.Property(x => x.PRO_FARDOS_POR_CAMADA).HasColumnName("PRO_FARDOS_POR_CAMADA");
            builder.Property(x => x.PRO_CAMADAS_POR_PALETE).HasColumnName("PRO_CAMADAS_POR_PALETE");
            builder.Property(x => x.PRO_GRUPO_PALETIZACAO).HasColumnName("PRO_GRUPO_PALETIZACAO");
            builder.Property(x => x.PRO_PECAS_POR_FARDO).HasColumnName("PRO_PECAS_POR_FARDO");
            builder.Property(x => x.UNI_ID).HasColumnName("UNI_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.GRP_ID).HasColumnName("GRP_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.TEM_ID).HasColumnName("TEM_ID");
            builder.Property(x => x.PRO_LARGURA_PECA).HasColumnName("PRO_LARGURA_PECA");
            builder.Property(x => x.PRO_COMPRIMENTO_PECA).HasColumnName("PRO_COMPRIMENTO_PECA");
            builder.Property(x => x.PRO_ALTURA_PECA).HasColumnName("PRO_ALTURA_PECA");
            builder.Property(x => x.PRO_LARGURA_EMBALADA).HasColumnName("PRO_LARGURA_EMBALADA");
            builder.Property(x => x.PRO_COMPRIMENTO_EMBALADA).HasColumnName("PRO_COMPRIMENTO_EMBALADA");
            builder.Property(x => x.PRO_ALTURA_EMBALADA).HasColumnName("PRO_ALTURA_EMBALADA");
            builder.Property(x => x.PRO_FRENTE).HasColumnName("PRO_FRENTE").HasMaxLength(1);
            builder.Property(x => x.PRO_ROTACIONA_COMPRIMENTO).HasColumnName("PRO_ROTACIONA_COMPRIMENTO").HasMaxLength(1);
            builder.Property(x => x.PRO_ROTACIONA_LARGURA).HasColumnName("PRO_ROTACIONA_LARGURA").HasMaxLength(1);
            builder.Property(x => x.PRO_ROTACIONA_ALTURA).HasColumnName("PRO_ROTACIONA_ALTURA").HasMaxLength(1);

            builder.Property(x => x.TMP_TIPO_CARGA).HasColumnName("TMP_TIPO_CARGA").HasMaxLength(50);
            builder.Property(x => x.PRO_TEMPO_CARREGAMENTO_UNITARIO).HasColumnName("PRO_TEMPO_CARREGAMENTO_UNITARIO");
            builder.Property(x => x.PRO_TEMPO_DESCARREGAMENTO_UNITARIO).HasColumnName("PRO_TEMPO_DESCARREGAMENTO_UNITARIO");
            builder.Property(x => x.PRO_PERCENTUAL_JANELA_EMBARQUE).HasColumnName("PRO_PERCENTUAL_JANELA_EMBARQUE");
            // PENDENCIA builder.Property(x => x.PRO_TEMPO_PRODUCAO_CONJUNTO).HasColumnName("PRO_TEMPO_PRODUCAO_CONJUNTO");
            builder.Property(x => x.PRO_PECAS_DA_PECA).HasColumnName("PRO_PECAS_DA_PECA");

            builder.HasOne(me => me.TemplateDeTestes).WithMany(u => u.ProdutoChapaVenda).HasForeignKey(me => me.TEM_ID);
            builder.HasOne(x => x.UnidadeMedida).WithMany(um => um.ProdutoChapaVenda).HasForeignKey(x => x.UNI_ID);
            builder.HasOne(x => x.GrupoProdutoOutros).WithMany(um => um.ProdutoChapaVenda).HasForeignKey(x => x.GRP_ID);
            builder.HasOne(x => x.GrupoPaletizacao).WithMany(um => um.ProdutoChapaVenda).HasForeignKey(x => x.PRO_GRUPO_PALETIZACAO);
        }
    }
}

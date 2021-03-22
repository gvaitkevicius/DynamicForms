using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class ProdutoCaixaMap : IEntityTypeConfiguration<ProdutoCaixa>
    {

        public void Configure(EntityTypeBuilder<ProdutoCaixa> builder)
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
            builder.Property(x => x.PRO_LARGURA_INTERNA).HasColumnName("PRO_LARGURA_INTERNA");
            builder.Property(x => x.PRO_COMPRIMENTO_INTERNA).HasColumnName("PRO_COMPRIMENTO_INTERNA");
            builder.Property(x => x.PRO_ALTURA_INTERNA).HasColumnName("PRO_ALTURA_INTERNA");
            builder.Property(x => x.PRO_AREA_LIQUIDA).HasColumnName("PRO_AREA_LIQUIDA");
            builder.Property(x => x.PRO_PESO).HasColumnName("PRO_PESO");
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
            builder.Property(x => x.PRO_COD_DESENHO).HasColumnName("PRO_COD_DESENHO").HasMaxLength(100);
            builder.Property(x => x.PRO_FECHAMENTO).HasColumnName("PRO_FECHAMENTO");
            builder.Property(x => x.PRO_TIPO_LAP).HasColumnName("PRO_TIPO_LAP").HasMaxLength(1);
            builder.Property(x => x.PRO_TAMANHO_LAP).HasColumnName("PRO_TAMANHO_LAP");
            builder.Property(x => x.PRO_LAP_PROLONGADO).HasColumnName("PRO_LAP_PROLONGADO").HasMaxLength(1);
            builder.Property(x => x.PRO_TAMANHO_LAP_PROLONGADO).HasColumnName("PRO_TAMANHO_LAP_PROLONGADO");
            builder.Property(x => x.PRO_ARRANJO_LARGURA).HasColumnName("PRO_ARRANJO_LARGURA");
            builder.Property(x => x.PRO_ARRANJO_COMPRIMENTO).HasColumnName("PRO_ARRANJO_COMPRIMENTO");
            builder.Property(x => x.PRO_FITILHOS_FARDO_LARG).HasColumnName("PRO_FITILHOS_FARDO_LARG");
            builder.Property(x => x.PRO_FITILHOS_FARDO_COMP).HasColumnName("PRO_FITILHOS_FARDO_COMP");
            builder.Property(x => x.PRO_FILME_PALETE).HasColumnName("PRO_FILME_PALETE");
            builder.Property(x => x.PRO_CANTONEIRA).HasColumnName("PRO_CANTONEIRA").HasMaxLength(1);
            builder.Property(x => x.PRO_VINCOS_LARGURA).HasColumnName("PRO_VINCOS_LARGURA");
            builder.Property(x => x.PRO_VINCOS_COMPRIMENTO).HasColumnName("PRO_VINCOS_COMPRIMENTO").HasMaxLength(100);
            builder.Property(x => x.PRO_FITILHOS_PALETE_LARG).HasColumnName("PRO_FITILHOS_PALETE_LARG");
            builder.Property(x => x.PRO_FITILHOS_PALETE_COMP).HasColumnName("PRO_FITILHOS_PALETE_COMP");

            builder.HasOne(me => me.TemplateDeTestes).WithMany(u => u.ProdutosCaixa).HasForeignKey(me => me.TEM_ID);
            builder.HasOne(x => x.TipoABNT).WithMany(um => um.ProdutoCaixa).HasForeignKey(x => x.ABN_ID);
            builder.HasOne(x => x.UnidadeMedida).WithMany(um => um.ProdutoCaixa).HasForeignKey(x => x.UNI_ID);
            builder.HasOne(x => x.GrupoProdutoConjunto).WithMany(um => um.ProdutoCaixa).HasForeignKey(x => x.GRP_ID);
            builder.HasOne(x => x.GrupoProdutoOutros).WithMany(um => um.ProdutoCaixa).HasForeignKey(x => x.GRP_ID);
            builder.HasOne(x => x.GrupoPaletizacao).WithMany(um => um.ProdutoCaixas).HasForeignKey(x => x.PRO_GRUPO_PALETIZACAO);

        }
    }
}

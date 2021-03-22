using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map
{
    public class ProdutoPapelMap : IEntityTypeConfiguration<ProdutoPapel>
    {
        public void Configure(EntityTypeBuilder<ProdutoPapel> builder)
        {
            builder.Property(x => x.PRO_ESTOQUE_ATUAL).HasColumnName("PRO_ESTOQUE_ATUAL");
            builder.Property(x => x.UNI_ID).HasColumnName("UNI_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.PRO_FARDOS_POR_CAMADA).HasColumnName("PRO_FARDOS_POR_CAMADA");
            builder.Property(x => x.PRO_TIPO_IDENTIFICACAO).HasColumnName("PRO_TIPO_IDENTIFICACAO");
            builder.Property(x => x.PRO_PECAS_POR_FARDO).HasColumnName("PRO_PECAS_POR_FARDO");
            builder.Property(x => x.GRP_ID).HasColumnName("GRP_ID").HasMaxLength(30).IsRequired();
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
            builder.Property(x => x.PRO_ESCALA_COR).HasColumnName("PRO_ESCALA_COR");
            builder.Property(x => x.PRO_CUSTO_SUBIDA_ESCALA_COR).HasColumnName("PRO_CUSTO_SUBIDA_ESCALA_COR");
            builder.Property(x => x.PRO_CUSTO_DECIDA_ESCALA_COR).HasColumnName("PRO_CUSTO_DECIDA_ESCALA_COR");
            builder.Property(x => x.TMP_TIPO_CARGA).HasColumnName("TMP_TIPO_CARGA").HasMaxLength(50);
            builder.Property(x => x.PRO_TEMPO_CARREGAMENTO_UNITARIO).HasColumnName("PRO_TEMPO_CARREGAMENTO_UNITARIO");
            builder.Property(x => x.PRO_TEMPO_DESCARREGAMENTO_UNITARIO).HasColumnName("PRO_TEMPO_DESCARREGAMENTO_UNITARIO");
            builder.Property(x => x.PRO_PERCENTUAL_JANELA_EMBARQUE).HasColumnName("PRO_PERCENTUAL_JANELA_EMBARQUE");
            builder.Property(x => x.PRO_GRUPO_PALETIZACAO).HasColumnName("PRO_GRUPO_PALETIZACAO");

            builder.HasOne(x => x.UnidadeMedida).WithMany(um => um.ProdutoPapel).HasForeignKey(x => x.UNI_ID);
            builder.HasOne(x => x.GrupoProduto).WithMany(gp => gp.ProdutoPapel).HasForeignKey(x => x.GRP_ID);
            builder.HasOne(x => x.GrupoPaletizacao).WithMany(um => um.ProdutoPapel).HasForeignKey(x => x.PRO_GRUPO_PALETIZACAO);
        }
    }
}

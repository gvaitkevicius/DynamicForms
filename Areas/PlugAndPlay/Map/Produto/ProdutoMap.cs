using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models

{
    public class ProdutoMap : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("V_PRODUTOS");
            builder.HasKey(x => x.PRO_ID);
            builder.Property(x => x.PRO_ID).HasColumnName("PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.PRO_DESCRICAO).HasColumnName("PRO_DESCRICAO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.PRO_ESTOQUE_ATUAL).HasColumnName("PRO_ESTOQUE_ATUAL");
            builder.Property(x => x.UNI_ID).HasColumnName("UNI_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.PRO_FARDOS_POR_CAMADA).HasColumnName("PRO_FARDOS_POR_CAMADA");
            builder.Property(x => x.PRO_CAMADAS_POR_PALETE).HasColumnName("PRO_CAMADAS_POR_PALETE");
            builder.Property(x => x.PRO_TIPO_IDENTIFICACAO).HasColumnName("PRO_TIPO_IDENTIFICACAO");
            builder.Property(x => x.PRO_GRUPO_PALETIZACAO).HasColumnName("PRO_GRUPO_PALETIZACAO").HasMaxLength(30);
            builder.Property(x => x.PRO_PECAS_POR_FARDO).HasColumnName("PRO_PECAS_POR_FARDO");
            builder.Property(x => x.PRO_ID_INTEGRACAO).HasColumnName("PRO_ID_INTEGRACAO").HasMaxLength(100);
            builder.Property(x => x.PRO_ID_INTEGRACAO_ERP).HasColumnName("PRO_ID_INTEGRACAO_ERP").HasMaxLength(100);
            builder.Property(x => x.GRP_ID).HasColumnName("GRP_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.TEM_ID).HasColumnName("TEM_ID");
            builder.Property(x => x.PRO_LARGURA_PECA).HasColumnName("PRO_LARGURA_PECA");
            builder.Property(x => x.PRO_COMPRIMENTO_PECA).HasColumnName("PRO_COMPRIMENTO_PECA");
            builder.Property(x => x.PRO_ALTURA_PECA).HasColumnName("PRO_ALTURA_PECA");
            builder.Property(x => x.PRO_LARGURA_INTERNA).HasColumnName("PRO_LARGURA_INTERNA");
            builder.Property(x => x.PRO_COMPRIMENTO_INTERNA).HasColumnName("PRO_COMPRIMENTO_INTERNA");
            builder.Property(x => x.PRO_ALTURA_INTERNA).HasColumnName("PRO_ALTURA_INTERNA");
            builder.Property(x => x.PRO_AREA_LIQUIDA).HasColumnName("PRO_AREA_LIQUIDA");
            builder.Property(x => x.PRO_AREA_LIQUIDA_CHAPA).HasColumnName("PRO_AREA_LIQUIDA_CHAPA");
            builder.Property(x => x.PRO_PESO).HasColumnName("PRO_PESO");
            builder.Property(x => x.PRO_PESO_CHAPA).HasColumnName("PRO_PESO_CHAPA");
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
            builder.Property(x => x.PRO_TEMPO_PRODUCAO_CONJUNTO).HasColumnName("PRO_TEMPO_PRODUCAO_CONJUNTO");
            builder.Property(x => x.PRO_PECAS_DA_PECA).HasColumnName("PRO_PECAS_DA_PECA");
            builder.Property(x => x.PRO_TYPE).HasColumnName("PRO_TYPE");
            builder.Property(x => x.PRO_STATUS).HasColumnName("PRO_STATUS").HasMaxLength(2);
            builder.Property(x => x.PRO_COLOR_HEXA).HasColumnName("PRO_COLOR_HEXA").HasMaxLength(6);
            builder.Property(x => x.PRO_ID_C).HasColumnName("PRO_ID_C").HasMaxLength(30);
            builder.Property(x => x.PRO_ID_CHAPA).HasColumnName("PRO_ID_CHAPA").HasMaxLength(30);
            builder.Property(x => x.QTD_CHAPA).HasColumnName("QTD_CHAPA");
            builder.Property(x => x.BASE_PRODUCAO_CHAPA).HasColumnName("BASE_PRODUCAO_CHAPA");
            builder.Property(x => x.PRO_LARGURA_PECA_CHAPA).HasColumnName("PRO_LARGURA_PECA_CHAPA");
            builder.Property(x => x.PRO_COMPRIMENTO_PECA_CHAPA).HasColumnName("PRO_COMPRIMENTO_PECA_CHAPA");
            builder.Property(x => x.PRO_DESCRICAO_CHAPA).HasColumnName("PRO_DESCRICAO_CHAPA").HasMaxLength(100);
            builder.Property(x => x.ALTURA_CHAPA).HasColumnName("ALTURA_CHAPA").HasMaxLength(100);
            builder.Property(x => x.PRO_VINCOS_LARGURA).HasColumnName("PRO_VINCOS_LARGURA").HasMaxLength(100);
            builder.Property(x => x.PRO_VINCOS_COMPRIMENTO).HasColumnName("PRO_VINCOS_COMPRIMENTO").HasMaxLength(100);
            builder.Property(x => x.PRO_VINCOS_ONDULADEIRA).HasColumnName("PRO_VINCOS_ONDULADEIRA").HasMaxLength(100);
            builder.Property(x => x.PRO_ID_CL).HasColumnName("PRO_ID_CL").HasMaxLength(30);
            builder.Property(x => x.PRO_ID_CLICHE).HasColumnName("PRO_ID_CLICHE").HasMaxLength(30);
            builder.Property(x => x.PRO_ID_F).HasColumnName("PRO_ID_F").HasMaxLength(30);
            builder.Property(x => x.PRO_ID_FACA).HasColumnName("PRO_ID_FACA").HasMaxLength(30);
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
            builder.Property(x => x.PRO_FITILHOS_PALETE_LARG).HasColumnName("PRO_FITILHOS_PALETE_LARG");
            builder.Property(x => x.PRO_FITILHOS_PALETE_COMP).HasColumnName("PRO_FITILHOS_PALETE_COMP");
            builder.Property(x => x.PRO_VINCOS_ONDULADEIRA).HasColumnName("PRO_VINCOS_ONDULADEIRA");
            builder.Property(x => x.PRO_ID_P).HasColumnName("PRO_ID_P").HasMaxLength(30);
            builder.Property(x => x.PRO_ID_PALETE).HasColumnName("PRO_ID_PALETE").HasMaxLength(30);
            builder.Property(x => x.QTD_PALETE).HasColumnName("QTD_PALETE");
            builder.Property(x => x.PRO_FILME_PALETE).HasColumnName("PRO_FILME_PALETE");
            builder.Property(x => x.PRO_CANTONEIRA).HasColumnName("PRO_CANTONEIRA").HasMaxLength(1);
            builder.Property(x => x.PRO_QTD_ESPELHO).HasColumnName("PRO_QTD_ESPELHO");
            builder.Property(x => x.PRO_ID_TP).HasColumnName("PRO_ID_TP").HasMaxLength(30);
            builder.Property(x => x.QTD_TP_PALETE).HasColumnName("QTD_TP_PALETE");
            builder.Property(x => x.PRO_ID_TP_PALETE).HasColumnName("PRO_ID_TP_PALETE").HasMaxLength(30);
            builder.Property(x => x.PRO_DESCRICAO_PALETE).HasColumnName("PRO_DESCRICAO_PALETE").HasMaxLength(100);
            builder.Property(x => x.PRO_DESCRICAO_TP).HasColumnName("PRO_DESCRICAO_TP").HasMaxLength(100);
            builder.Property(x => x.PRO_LARGURA_PALETE).HasColumnName("PRO_LARGURA_PALETE");
            builder.Property(x => x.PRO_COMPRIMENTO_PALETE).HasColumnName("PRO_COMPRIMENTO_PALETE");
            builder.Property(x => x.PRO_LARGURA_TP_PALETE).HasColumnName("PRO_LARGURA_TP_PALETE");
            builder.Property(x => x.PRO_COMPRIMENTO_TP_PALETE).HasColumnName("PRO_COMPRIMENTO_TP_PALETE");
            builder.Property(x => x.ABN_ID).HasColumnName("ABN_ID").HasMaxLength(50);
            builder.Property(x => x.GRP_ID_C).HasColumnName("GRP_ID_C").HasMaxLength(50);
            builder.Property(x => x.GRP_DESCRICAO_C).HasColumnName("GRP_DESCRICAO_C").HasMaxLength(50);
            builder.Property(x => x.GRP_DESCRICAO).HasColumnName("GRP_DESCRICAO").HasMaxLength(50);
            builder.Property(x => x.GRP_TIPO).HasColumnName("GRP_TIPO").HasMaxLength(50);



            builder.HasOne(x => x.UnidadeMedida).WithMany(um => um.Produtos).HasForeignKey(x => x.UNI_ID);
            builder.HasOne(x => x.GrupoProduto).WithMany(gp => gp.Produtos).HasForeignKey(x => x.GRP_ID);
            builder.HasOne(x => x.TemplateDeTestes).WithMany(t => t.Produtos).HasForeignKey(x => x.TEM_ID);
        }

    }
}
using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Areas.PlugAndPlay.Map
{
    public class V_CONSULTA_PEDIDO_MAP : IEntityTypeConfiguration<V_CONSULTA_PEDIDO>
    {
        public void Configure(EntityTypeBuilder<V_CONSULTA_PEDIDO> builder)
        {
            builder.ToTable("V_CONSULTA_PEDIDO");
            builder.HasKey(x => x.ORD_ID);
            builder.Property(x => x.CLI_NOME).HasColumnName("CLI_NOME").HasMaxLength(100).IsRequired();
            builder.Property(x => x.PRO_DESCRICAO).HasColumnName("PRO_DESCRICAO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(60).IsRequired();
            builder.Property(x => x.ORD_ID_CONJUNTO).HasColumnName("ORD_ID_CONJUNTO").HasMaxLength(30).IsRequired();
            builder.Property(x => x.PRO_ID).HasColumnName("PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.PRO_ID_CONJUNTO).HasColumnName("PRO_ID_CONJUNTO").HasMaxLength(30).IsRequired();
            builder.Property(x => x.CLI_ID).HasColumnName("CLI_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.ORD_PRECO_UNITARIO).HasColumnName("ORD_PRECO_UNITARIO");
            builder.Property(x => x.ORD_QUANTIDADE).HasColumnName("ORD_QUANTIDADE").IsRequired();
            builder.Property(x => x.ORD_DATA_ENTREGA_DE).HasColumnName("ORD_DATA_ENTREGA_DE").IsRequired();
            builder.Property(x => x.ORD_DATA_ENTREGA_ATE).HasColumnName("ORD_DATA_ENTREGA_ATE").IsRequired();
            builder.Property(x => x.ORD_TIPO).HasColumnName("ORD_TIPO");
            builder.Property(x => x.ORD_TOLERANCIA_MAIS).HasColumnName("ORD_TOLERANCIA_MAIS");
            builder.Property(x => x.ORD_TOLERANCIA_MENOS).HasColumnName("ORD_TOLERANCIA_MENOS");
            builder.Property(x => x.HASH_KEY).HasColumnName("HASH_KEY").HasMaxLength(100);
            builder.Property(x => x.ORD_INICIO_JANELA_EMBARQUE).HasColumnName("ORD_INICIO_JANELA_EMBARQUE");
            builder.Property(x => x.ORD_FIM_JANELA_EMBARQUE).HasColumnName("ORD_FIM_JANELA_EMBARQUE");
            builder.Property(x => x.ORD_EMBARQUE_ALVO).HasColumnName("ORD_EMBARQUE_ALVO");
            builder.Property(x => x.ORD_INICIO_GRUPO_PRODUTIVO).HasColumnName("ORD_INICIO_GRUPO_PRODUTIVO");
            builder.Property(x => x.ORD_FIM_GRUPO_PRODUTIVO).HasColumnName("ORD_FIM_GRUPO_PRODUTIVO");
            builder.Property(x => x.ORD_PESO_UNITARIO).HasColumnName("ORD_PESO_UNITARIO");
            builder.Property(x => x.ORD_M2_UNITARIO).HasColumnName("ORD_M2_UNITARIO");
            builder.Property(x => x.ORD_MIT).HasColumnName("ORD_MIT").HasMaxLength(100);
            builder.Property(x => x.CAR_TIPO_CARREGAMENTO).HasColumnName("CAR_TIPO_CARREGAMENTO").HasMaxLength(50);
            builder.Property(x => x.ORD_STATUS).HasColumnName("ORD_STATUS").HasMaxLength(10);
            builder.Property(x => x.ORD_TIPO_FRETE).HasColumnName("ORD_TIPO_FRETE").HasMaxLength(3);
            builder.Property(x => x.ORD_ENDERECO_ENTREGA).HasColumnName("ORD_ENDERECO_ENTREGA").HasMaxLength(200);
            builder.Property(x => x.ORD_BAIRRO_ENTREGA).HasColumnName("ORD_BAIRRO_ENTREGA").HasMaxLength(100);
            builder.Property(x => x.UF_ID_ENTREGA).HasColumnName("UF_ID_ENTREGA").HasMaxLength(2);
            builder.Property(x => x.ORD_CEP_ENTREGA).HasColumnName("ORD_CEP_ENTREGA").HasMaxLength(10);
            builder.Property(x => x.MUN_ID_ENTREGA).HasColumnName("MUN_ID_ENTREGA").HasMaxLength(50);
            builder.Property(x => x.ORD_REGIAO_ENTREGA).HasColumnName("ORD_REGIAO_ENTREGA").HasMaxLength(100);
            builder.Property(x => x.ORD_LARGURA).HasColumnName("ORD_LARGURA");
            builder.Property(x => x.ORD_COMPRIMENTO).HasColumnName("ORD_COMPRIMENTO");
            builder.Property(x => x.ORD_GRAMATURA).HasColumnName("ORD_GRAMATURA");
            builder.Property(x => x.GRP_ID).HasColumnName("GRP_ID").HasMaxLength(100);
            builder.Property(x => x.ORD_ID_INTEGRACAO).HasColumnName("ORD_ID_INTEGRACAO").HasMaxLength(100);
            builder.Property(x => x.ORD_OBSERVACAO_OTIMIZADOR).HasColumnName("ORD_OBSERVACAO_OTIMIZADOR").HasMaxLength(8000);
            builder.Property(x => x.ORD_COR_FILA).HasColumnName("ORD_COR_FILA").HasMaxLength(30);
            builder.Property(x => x.ORD_OP_INTEGRACAO).HasColumnName("ORD_OP_INTEGRACAO").HasMaxLength(100);
            builder.Property(x => x.ORD_PED_CLI).HasColumnName("ORD_PED_CLI").HasMaxLength(100);
            builder.Property(x => x.ORD_PRIORIDADE).HasColumnName("ORD_PRIORIDADE");
            builder.Property(x => x.ORD_LOTE_PILOTO).HasColumnName("ORD_LOTE_PILOTO");
            builder.Property(x => x.ORD_EMISSAO).HasColumnName("ORD_EMISSAO");
            builder.Property(x => x.ORD_ID_RESERVA).HasColumnName("ORD_ID_RESERVA").HasMaxLength(60);
            builder.Property(x => x.REP_ID).HasColumnName("REP_ID");
            builder.Property(x => x.ORD_PESO_UNITARIO_BRUTO).HasColumnName("ORD_PESO_UNITARIO_BRUTO");

        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class OrderMap : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("T_ORDENS");
            builder.HasKey(x => x.ORD_ID);
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
            builder.Property(x => x.ORD_PESO_UNITARIO_BRUTO).HasColumnName("ORD_PESO_UNITARIO_BRUTO");
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
            builder.Property(x => x.ORD_OBSERVACAO_OTIMIZADOR).HasColumnName("ORD_OBSERVACAO_OTIMIZADOR").HasMaxLength(1000);
            //builder.Property(x => x.ORD_COR_FILA).HasColumnName("ORD_COR_FILA").HasMaxLength(30);
            builder.Property(x => x.ORD_OP_INTEGRACAO).HasColumnName("ORD_OP_INTEGRACAO").HasMaxLength(100);
            builder.Property(x => x.ORD_PED_CLI).HasColumnName("ORD_PED_CLI").HasMaxLength(100);
            builder.Property(x => x.ORD_PRIORIDADE).HasColumnName("ORD_PRIORIDADE");
            builder.Property(x => x.ORD_EMISSAO).HasColumnName("ORD_EMISSAO");
            builder.Property(x => x.REP_ID).HasColumnName("REP_ID");
            builder.Property(x => x.PRO_ID_INTEGRACAO_ERP).HasColumnName("PRO_ID_INTEGRACAO_ERP");
            builder.Property(x => x.ORD_VINCOS_ONDULADEIRA).HasColumnName("ORD_VINCOS_ONDULADEIRA");
            

            builder.HasOne(x => x.Produto).WithMany(x => x.Ordens).HasForeignKey(x => x.PRO_ID);
            builder.HasOne(x => x.Cliente).WithMany(x => x.Ordens).HasForeignKey(x => x.CLI_ID);
            builder.HasOne(x => x.Municipio).WithMany(x => x.Order).HasForeignKey(x => x.MUN_ID_ENTREGA);
            builder.HasOne(x => x.PontosMapa).WithMany(x => x.Order).HasForeignKey(x => x.ORD_REGIAO_ENTREGA);
            builder.HasOne(x => x.Representantes).WithMany(x => x.Order).HasForeignKey(x => x.REP_ID);
        }
    }


    public class OrderOptMap : IEntityTypeConfiguration<OrderOpt>
    {
        public void Configure(EntityTypeBuilder<OrderOpt> builder)
        {
            builder.ToTable("V_PEDIDOS_A_PLANEJAR_EXPEDICAO");
            builder.HasKey(x => new { x.CAR_ID, x.Id});
            builder.Property(x => x.TIP_ID).HasColumnName("TIP_ID").IsRequired();
            builder.Property(x => x.CAR_ID).HasColumnName("CAR_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.ITC_QTD_PLANEJADA).HasColumnName("ITC_QTD_PLANEJADA").IsRequired();
            builder.Property(x => x.ITC_QTD_REALIZADA).HasColumnName("ITC_QTD_REALIZADA").IsRequired();
            builder.Property(x => x.QTD_TOTAL_PLANEJADO_E_EXPEDIDO).HasColumnName("QTD_TOTAL_PLANEJADO_E_EXPEDIDO").IsRequired();
            builder.Property(x => x.CAR_STATUS).HasColumnName("CAR_STATUS").IsRequired();
            builder.Property(x => x.CAR_EMBARQUE_ALVO).HasColumnName("CAR_EMBARQUE_ALVO").IsRequired();
            builder.Property(x => x.FPR_COR_FILA).HasColumnName("FPR_COR_FILA").HasMaxLength(30).IsRequired();
            builder.Property(x => x.VIR_COR_OTIF).HasColumnName("VIR_COR_OTIF").HasMaxLength(8);
            builder.Property(x => x.CLI_REGIAO_ENTREGA).HasColumnName("CLI_REGIAO_ENTREGA").HasMaxLength(100);
            builder.Property(x => x.CLI_MUN_ID_ENTREGA).HasColumnName("CLI_MUN_ID_ENTREGA").HasMaxLength(50);
            builder.Property(x => x.UF_COD).HasColumnName("UF_COD").HasMaxLength(2);
            builder.Property(x => x.MUN_NOME).HasColumnName("MUN_NOME").HasMaxLength(100);
            builder.Property(x => x.MUN_ID).HasColumnName("MUN_ID").HasMaxLength(50);
            builder.Property(x => x.BairroEntrega).HasColumnName("BairroEntrega").HasMaxLength(100);
            builder.Property(x => x.EnderecoEntrega).HasColumnName("EnderecoEntrega").HasMaxLength(200);
            builder.Property(x => x.CEPEntrega).HasColumnName("CEPEntrega").HasMaxLength(10);
            builder.Property(x => x.PON_ID_MUN).HasColumnName("PON_ID_MUN").HasMaxLength(100);
            builder.Property(x => x.PON_ID_REG).HasColumnName("PON_ID_REG").HasMaxLength(100);
            builder.Property(x => x.PON_DESCRICAO_MUN).HasColumnName("PON_DESCRICAO_MUN").HasMaxLength(100);
            builder.Property(x => x.PON_DESCRICAO_REG).HasColumnName("PON_DESCRICAO_REG").HasMaxLength(100);
            builder.Property(x => x.PON_DISTANCIA_KM_MUN).HasColumnName("PON_DISTANCIA_KM_MUN").IsRequired();
            builder.Property(x => x.PON_DISTANCIA_KM_REG).HasColumnName("PON_DISTANCIA_KM_REG").IsRequired();
            builder.Property(x => x.Id).HasColumnName("Id").HasMaxLength(60).IsRequired();
            builder.Property(x => x.MIT).HasColumnName("MIT").HasMaxLength(100);
            builder.Property(x => x.M2_Unitario).HasColumnName("M2_Unitario");
            builder.Property(x => x.Peso_Unitario).HasColumnName("Peso_Unitario");
            builder.Property(x => x.FimJanelaEmbarque).HasColumnName("FimJanelaEmbarque");
            builder.Property(x => x.InicioJanelaEmbarque).HasColumnName("InicioJanelaEmbarque");
            builder.Property(x => x.EmbarqueAlvo).HasColumnName("EmbarqueAlvo");
            builder.Property(x => x.ProdutoId).HasColumnName("ProdutoId").HasMaxLength(30).IsRequired();
            builder.Property(x => x.PRO_DESCRICAO).HasColumnName("PRO_DESCRICAO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.ClienteId).HasColumnName("ClienteId").HasMaxLength(30).IsRequired();
            builder.Property(x => x.CLI_NOME).HasColumnName("CLI_NOME").HasMaxLength(100).IsRequired();
            builder.Property(x => x.ToleranciaMenos).HasColumnName("ToleranciaMenos");
            builder.Property(x => x.ToleranciaMais).HasColumnName("ToleranciaMais");
            builder.Property(x => x.Tipo).HasColumnName("Tipo");
            builder.Property(x => x.PrecoUnitario).HasColumnName("PrecoUnitario");
            builder.Property(x => x.Quantidade).HasColumnName("Quantidade").IsRequired();
            builder.Property(x => x.DataEntregaDe).HasColumnName("DataEntregaDe");
            builder.Property(x => x.DataEntregaAte).HasColumnName("DataEntregaAte");
            builder.Property(x => x.VIR_QTD_SALDO_A_EXPEDIR).HasColumnName("VIR_QTD_SALDO_A_EXPEDIR");
            builder.Property(x => x.VIR_QTD_SALDO_UE).HasColumnName("VIR_QTD_SALDO_UE");
            builder.Property(x => x.PRO_GRUPO_PALETIZACAO).HasColumnName("PRO_GRUPO_PALETIZACAO").HasMaxLength(30).IsRequired();
            builder.Property(x => x.VIR_METODO_CUBAGEM).HasColumnName("VIR_METODO_CUBAGEM").IsRequired();
            builder.Property(x => x.VIR_M3_UE).HasColumnName("VIR_M3_UE");
            builder.Property(x => x.VIR_M3_UNITARIO).HasColumnName("VIR_M3_UNITARIO");
            builder.Property(x => x.PRO_CAMADAS_POR_PALETE).HasColumnName("PRO_CAMADAS_POR_PALETE");
            builder.Property(x => x.PRO_FARDOS_POR_CAMADA).HasColumnName("PRO_FARDOS_POR_CAMADA");
            builder.Property(x => x.PRO_PECAS_POR_FARDO).HasColumnName("PRO_PECAS_POR_FARDO");
            builder.Property(x => x.PRO_LARGURA_EMBALADA).HasColumnName("PRO_LARGURA_EMBALADA");
            builder.Property(x => x.PRO_COMPRIMENTO_EMBALADA).HasColumnName("PRO_COMPRIMENTO_EMBALADA");
            builder.Property(x => x.PRO_ALTURA_EMBALADA).HasColumnName("PRO_ALTURA_EMBALADA");
            builder.Property(x => x.VIR_PECAS_POR_UE).HasColumnName("VIR_PECAS_POR_UE");
            builder.Property(x => x.Translado).HasColumnName("Translado").IsRequired();
            builder.Property(x => x.PRO_FRENTE).HasColumnName("PRO_FRENTE").HasMaxLength(1);
            builder.Property(x => x.PRO_ROTACIONA_COMPRIMENTO).HasColumnName("PRO_ROTACIONA_COMPRIMENTO").HasMaxLength(1);
            builder.Property(x => x.PRO_ROTACIONA_LARGURA).HasColumnName("PRO_ROTACIONA_LARGURA").HasMaxLength(1);
            builder.Property(x => x.PRO_ROTACIONA_ALTURA).HasColumnName("PRO_ROTACIONA_ALTURA").HasMaxLength(1);
            builder.Property(x => x.PRO_TEMPO_DESCARREGAMENTO_UNITARIO).HasColumnName("PRO_TEMPO_DESCARREGAMENTO_UNITARIO");
            builder.Property(x => x.PRO_TEMPO_CARREGAMENTO_UNITARIO).HasColumnName("PRO_TEMPO_CARREGAMENTO_UNITARIO");
            builder.Property(x => x.CLI_TEMPO_MEDIO_ESPERA_DE_DESCARREGAMENTO).HasColumnName("CLI_TEMPO_MEDIO_ESPERA_DE_DESCARREGAMENTO");
            builder.Property(x => x.PRO_PERCENTUAL_JANELA_EMBARQUE).HasColumnName("PRO_PERCENTUAL_JANELA_EMBARQUE");
            builder.Property(x => x.GrupoEmbarque).HasColumnName("GrupoEmbarque");
            builder.Property(x => x.TMP_TIPO_CARGA).HasColumnName("TMP_TIPO_CARGA").HasMaxLength(50);
            builder.Property(x => x.TEMPO_CARREGAMENTO).HasColumnName("TEMPO_CARREGAMENTO");
            builder.Property(x => x.TEMPO_DESCARREGAMENTO).HasColumnName("TEMPO_DESCARREGAMENTO");
            builder.Property(x => x.HashKey).HasColumnName("HashKey").HasMaxLength(200);
            builder.Property(x => x.IDsCargasPotenciaisParaAntecipacoes).HasColumnName("IDsCargasPotenciaisParaAntecipacoes").HasMaxLength(1).IsRequired();
            builder.Property(x => x.SALDO_ESTOQUE).HasColumnName("SALDO_ESTOQUE").IsRequired();
            builder.Property(x => x.SALDO_ESTOQUE_UE).HasColumnName("SALDO_ESTOQUE_UE").IsRequired();
            builder.Property(x => x.PECENT_ESTOQUE_PRONTO).HasColumnName("PECENT_ESTOQUE_PRONTO").IsRequired();


        }
    }

}
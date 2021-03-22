using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class CargaMap : IEntityTypeConfiguration<Carga>
    {
        public void Configure(EntityTypeBuilder<Carga> builder)
        {
            builder.ToTable("T_CARGA");
            builder.HasKey(x => x.CAR_ID);
            builder.Property(x => x.CAR_ID).HasColumnName("CAR_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.CAR_PREVISAO_MATERIA_PRIMA).HasColumnName("CAR_PREVISAO_MATERIA_PRIMA");
            builder.Property(x => x.CAR_DATA_INICIO_PREVISTO).HasColumnName("CAR_DATA_INICIO_PREVISTO");
            builder.Property(x => x.CAR_DATA_INICIO_REALIZADO).HasColumnName("CAR_DATA_INICIO_REALIZADO");
            builder.Property(x => x.CAR_DATA_FIM_PREVISTO).HasColumnName("CAR_DATA_FIM_PREVISTO");
            builder.Property(x => x.CAR_DATA_FIM_REALIZADO).HasColumnName("CAR_DATA_FIM_REALIZADO");
            builder.Property(x => x.CAR_INICIO_JANELA_EMBARQUE).HasColumnName("CAR_INICIO_JANELA_EMBARQUE");
            builder.Property(x => x.CAR_FIM_JANELA_EMBARQUE).HasColumnName("CAR_FIM_JANELA_EMBARQUE");
            builder.Property(x => x.CAR_EMBARQUE_ALVO).HasColumnName("CAR_EMBARQUE_ALVO");
            builder.Property(x => x.CAR_STATUS).HasColumnName("CAR_STATUS");
            builder.Property(x => x.CAR_PESO_TEORICO).HasColumnName("CAR_PESO_TEORICO");
            builder.Property(x => x.CAR_VOLUME_TEORICO).HasColumnName("CAR_VOLUME_TEORICO");
            builder.Property(x => x.CAR_PESO_REAL).HasColumnName("CAR_PESO_REAL");
            builder.Property(x => x.CAR_VOLUME_REAL).HasColumnName("CAR_VOLUME_REAL");
            builder.Property(x => x.CAR_PESO_EMBALAGEM).HasColumnName("CAR_PESO_EMBALAGEM");
            builder.Property(x => x.CAR_PESO_ENTRADA).HasColumnName("CAR_PESO_ENTRADA");
            builder.Property(x => x.CAR_PESO_SAIDA).HasColumnName("CAR_PESO_SAIDA");
            builder.Property(x => x.CAR_ID_DOCA).HasColumnName("CAR_ID_DOCA").HasMaxLength(30);
            builder.Property(x => x.VEI_PLACA).HasColumnName("VEI_PLACA").HasMaxLength(8);
            builder.Property(x => x.TIP_ID).HasColumnName("TIP_ID");
            builder.Property(x => x.TRA_ID).HasColumnName("TRA_ID").HasMaxLength(30);
            builder.Property(x => x.CAR_GRUPO_PRODUTIVO).HasColumnName("CAR_GRUPO_PRODUTIVO");
            builder.Property(x => x.ROT_ID).HasColumnName("ROT_ID").HasMaxLength(100);
            builder.Property(x => x.OCO_ID).HasColumnName("OCO_ID").HasMaxLength(30);
            builder.Property(x => x.CAR_ID_JUNTADA).HasColumnName("CAR_ID_JUNTADA").HasMaxLength(30);
            builder.Property(x => x.CAR_OBSERVACAO_DE_TRANSPORTE).HasColumnName("CAR_OBSERVACAO_DE_TRANSPORTE").HasMaxLength(4000);
            builder.Property(x => x.CAR_JUSTIFICATIVA_DE_CARREGAMENTO).HasColumnName("CAR_JUSTIFICATIVA_DE_CARREGAMENTO").HasMaxLength(4000);
            builder.Property(x => x.OCO_ID_LIBERACAO).HasColumnName("OCO_ID_LIBERACAO");
            builder.Property(x => x.CAR_OBS_LIBERACAO).HasColumnName("CAR_OBS_LIBERACAO");
            builder.Property(x => x.CAR_PESAGEM_LIBERADA).HasColumnName("CAR_PESAGEM_LIBERADA");
            builder.Property(x => x.CAR_ID_INTEGRACAO_BALANCA).HasColumnName("CAR_ID_INTEGRACAO_BALANCA");

            builder.Property(x => x.CAR_DATA_ENTRADA_VEICULO).HasColumnName("CAR_DATA_ENTRADA_VEICULO");
            builder.Property(x => x.CAR_DATA_SAIDA_VEICULO).HasColumnName("CAR_DATA_SAIDA_VEICULO");

            builder.HasOne(x => x.Veiculo).WithMany(v => v.Cargas).HasForeignKey(x => x.VEI_PLACA);
            builder.HasOne(x => x.TipoVeiculo).WithMany(tp => tp.Cargas).HasForeignKey(x => x.TIP_ID);
            builder.HasOne(x => x.Transportadora).WithMany(t => t.Cargas).HasForeignKey(x => x.TRA_ID);
            builder.HasOne(x => x.OcorrenciaTransporte).WithMany(o => o.Carga).HasForeignKey(x => x.OCO_ID);
            builder.HasOne(x => x.OcorrenciaTransportePesagem).WithMany(o => o.CargaPesagem).HasForeignKey(x => x.OCO_ID_LIBERACAO);

        }
    }


    public class ItenCargaMap : IEntityTypeConfiguration<ItenCarga>
    {
        public void Configure(EntityTypeBuilder<ItenCarga> builder)
        {
            //public Veiculo VEI_PLACA { get; set; }
            //public TipoVeiculo TipoVeiculo { get; set; }
            //public List<ItensCarga> ItensCargas { get; set; }

            builder.ToTable("T_ITENS_CARGA");
            builder.HasKey(x => new { x.CAR_ID, x.ORD_ID });
            builder.Property(x => x.CAR_ID).HasColumnName("CAR_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(60).IsRequired();
            builder.Property(x => x.ITC_ENTREGA_PLANEJADA).HasColumnName("ITC_ENTREGA_PLANEJADA").IsRequired();
            builder.Property(x => x.ITC_ENTREGA_REALIZADA).HasColumnName("ITC_ENTREGA_REALIZADA").IsRequired();
            builder.Property(x => x.ITC_ORDEM_ENTREGA).HasColumnName("ITC_ORDEM_ENTREGA").IsRequired();
            builder.Property(x => x.ITC_QTD_PLANEJADA).HasColumnName("ITC_QTD_PLANEJADA").IsRequired();
            builder.Property(x => x.ITC_QTD_REALIZADA).HasColumnName("ITC_QTD_REALIZADA").IsRequired();
            builder.Property(x => x.ORD_HASH_KEY).HasColumnName("ORD_HASH_KEY").HasMaxLength(300);

            builder.HasOne(x => x.Carga).WithMany(x => x.ItensCarga).HasForeignKey(x => x.CAR_ID);
            builder.HasOne(x => x.Oredr).WithMany(x => x.ItensCarga).HasForeignKey(x => x.ORD_ID);
        }
    }
    public class V_ITENS_ROMANEADOSMap : IEntityTypeConfiguration<V_ITENS_ROMANEADOS>
    {
        public void Configure(EntityTypeBuilder<V_ITENS_ROMANEADOS> builder)
        {
            builder.ToTable("V_ITENS_ROMANEADOS");
            builder.HasKey(x => new { x.CAR_ID, x.ORD_ID });
            builder.Property(x => x.CAR_ID).HasColumnName("CAR_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(60).IsRequired();
            builder.Property(x => x.ITC_ENTREGA_PLANEJADA).HasColumnName("ITC_ENTREGA_PLANEJADA").IsRequired();
            builder.Property(x => x.ITC_ORDEM_ENTREGA).HasColumnName("ITC_ORDEM_ENTREGA").IsRequired();
            builder.Property(x => x.ITC_QTD_PLANEJADA).HasColumnName("ITC_QTD_PLANEJADA").IsRequired();
            builder.Property(x => x.QTD_PALETES).HasColumnName("QTD_PALETES").IsRequired();
            builder.Property(x => x.ORD_HASH_KEY).HasColumnName("ORD_HASH_KEY").HasMaxLength(300).IsRequired();
            builder.Property(x => x.PRO_ID).HasColumnName("PRO_ID").HasMaxLength(30);
            builder.Property(x => x.ORD_TOLERANCIA_MAIS).HasColumnName("ORD_TOLERANCIA_MAIS");
            builder.Property(x => x.ORD_TOLERANCIA_MENOS).HasColumnName("ORD_TOLERANCIA_MENOS");
            builder.Property(x => x.QTD_ROMANEADA).HasColumnName("QTD_ROMANEADA");
            builder.Property(x => x.QTD_CONSOLIDADA).HasColumnName("QTD_CONSOLIDADA");
            builder.Property(x => x.PERCENTUAL_EMBARCADO).HasColumnName("PERCENTUAL_EMBARCADO");

            builder.Property(x => x.PRO_ID_CONJUNTO).HasColumnName("PRO_ID_CONJUNTO");
            builder.Property(x => x.ORD_ID_CONJUNTO).HasColumnName("ORD_ID_CONJUNTO");
            builder.Property(x => x.EST_QUANT).HasColumnName("EST_QUANT");
            builder.Property(x => x.QTD_CONJUNTOS).HasColumnName("QTD_CONJUNTOS");
            builder.Property(x => x.ORD_QUANTIDADE).HasColumnName("ORD_QUANTIDADE");

            builder.HasOne(x => x.Carga).WithMany(x => x.V_ITENS_ROMANEADOS).HasForeignKey(x => x.CAR_ID);
        }
    }

    public class VeiculoMap : IEntityTypeConfiguration<Veiculo>
    {
        public void Configure(EntityTypeBuilder<Veiculo> builder)
        {
            builder.ToTable("T_VEICULOS");
            builder.HasKey(x => x.VEI_PLACA);
            builder.Property(x => x.VEI_PLACA).HasColumnName("VEI_PLACA").HasMaxLength(8).IsRequired();
            builder.Property(x => x.TIP_ID).HasColumnName("TIP_ID").IsRequired();
            builder.Property(x => x.VEI_CAPACIDADE_M3).HasColumnName("VEI_CAPACIDADE_M3");
            builder.Property(x => x.VEI_CAPACIDADE_LARGURA).HasColumnName("VEI_CAPACIDADE_LARGURA");
            builder.Property(x => x.VEI_CAPACIDADE_COMPRIMENTO).HasColumnName("VEI_CAPACIDADE_COMPRIMENTO");
            builder.Property(x => x.VEI_CAPACIDADE_ALTURA).HasColumnName("VEI_CAPACIDADE_ALTURA");
            builder.Property(x => x.VEI_MODELO).HasColumnName("VEI_MODELO");
            builder.Property(x => x.VEI_NOME_MOTORISTA).HasColumnName("VEI_NOME_MOTORISTA");
            builder.Property(x => x.VEI_DADOS_CONTATO).HasColumnName("VEI_DADOS_CONTATO");
            builder.HasOne(x => x.TipoVeiculo).WithMany(x => x.Veiculos).HasForeignKey(x => x.TIP_ID);
        }
    }

    public class TipoVeiculoMap : IEntityTypeConfiguration<TipoVeiculo>
    {
        public void Configure(EntityTypeBuilder<TipoVeiculo> builder)
        {
            builder.ToTable("T_TIPO_VEICULO");
            builder.HasKey(x => x.TIP_ID);
            builder.Property(x => x.TIP_ID).HasColumnName("TIP_ID").IsRequired();
            builder.Property(x => x.TIP_DESCRICAO).HasColumnName("TIP_DESCRICAO").HasMaxLength(100);
            builder.Property(x => x.TIP_QTD_DISPONIVEL).HasColumnName("TIP_QTD_DISPONIVEL");
            builder.Property(x => x.TIP_VALOR_KM).HasColumnName("TIP_VALOR_KM");
            builder.Property(x => x.TIP_VALOR_DIARIA).HasColumnName("TIP_VALOR_DIARIA");
            builder.Property(x => x.TIP_VALOR_AJUDANTE).HasColumnName("TIP_VALOR_AJUDANTE");
            builder.Property(x => x.TIP_QTD_EIXOS).HasColumnName("TIP_QTD_EIXOS");
            builder.Property(x => x.TIP_VELOCIDADE_MEDIA).HasColumnName("TIP_VELOCIDADE_MEDIA");
            builder.Property(x => x.TIP_CAPACIDADE_ALTURA).HasColumnName("TIP_CAPACIDADE_ALTURA");
            builder.Property(x => x.TIP_CAPACIDADE_COMPRIMENTO).HasColumnName("TIP_CAPACIDADE_COMPRIMENTO");
            builder.Property(x => x.TIP_CAPACIDADE_LARGURA).HasColumnName("TIP_CAPACIDADE_LARGURA");
            builder.Property(x => x.TIP_CAPACIDADE_ALTURA_PESCOCO_E).HasColumnName("TIP_CAPACIDADE_ALTURA_PESCOCO_E");
            builder.Property(x => x.TIP_CAPACIDADE_COMPRIMENTO_PESCOCO_E).HasColumnName("TIP_CAPACIDADE_COMPRIMENTO_PESCOCO_E");
            builder.Property(x => x.TIP_CAPACIDADE_LARGURA_PESCOCO_E).HasColumnName("TIP_CAPACIDADE_LARGURA_PESCOCO_E");
            builder.Property(x => x.TIP_CAPACIDADE_ALTURA_PESCOCO_D).HasColumnName("TIP_CAPACIDADE_ALTURA_PESCOCO_D");
            builder.Property(x => x.TIP_CAPACIDADE_COMPRIMENTO_PESCOCO_D).HasColumnName("TIP_CAPACIDADE_COMPRIMENTO_PESCOCO_D");
            builder.Property(x => x.TIP_CAPACIDADE_LARGURA_PESCOCO_D).HasColumnName("TIP_CAPACIDADE_LARGURA_PESCOCO_D");
            builder.Property(x => x.TIP_CAPACIDADE_M3).HasColumnName("TIP_CAPACIDADE_M3");
        }
    }

    public class RotasFactiveisMap : IEntityTypeConfiguration<RotaPontosMapa>
    {
        public void Configure(EntityTypeBuilder<RotaPontosMapa> builder)
        {
            builder.ToTable("T_ROTAS_FACTIVEIS");
            builder.HasKey(x => new { x.ROT_ID, x.PON_ID_DESTINO, x.PON_ID_ORIGEM, x.PON_ID_ROTEIRO });
            builder.Property(x => x.ROT_ID).HasColumnName("ROT_ID").HasMaxLength(100).IsRequired();
            builder.Property(x => x.PON_ID_DESTINO).HasColumnName("PON_ID_DESTINO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.PON_ID_ORIGEM).HasColumnName("PON_ID_ORIGEM").HasMaxLength(100).IsRequired();
            builder.Property(x => x.ROT_CUSTO_TOTAL).HasColumnName("ROT_CUSTO_TOTAL");
            builder.Property(x => x.PON_ID_ROTEIRO).HasColumnName("PON_ID_ROTEIRO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.ROT_ORDEM_ROTEIRO).HasColumnName("ROT_ORDEM_ROTEIRO");
            builder.Property(x => x.ROT_TIPO).HasColumnName("ROT_TIPO").HasMaxLength(2);
            builder.Property(x => x.ROT_DISTANCIA).HasColumnName("ROT_DISTANCIA");

            builder.HasOne(x => x.PontoMapaDestino).WithMany(pm => pm.RotaPontosMapaDestino).HasForeignKey(x => x.PON_ID_DESTINO);
            builder.HasOne(x => x.PontoMapaOrigem).WithMany(pm => pm.RotaPontosMapaOrigem).HasForeignKey(x => x.PON_ID_ORIGEM);
            builder.HasOne(x => x.PontoMapaRoteiro).WithMany(pm => pm.RotaPontosMapaRoteiro).HasForeignKey(x => x.PON_ID_ROTEIRO);
        }
    }

    public class CargasWebMap : IEntityTypeConfiguration<CargasWeb>
    {
        public void Configure(EntityTypeBuilder<CargasWeb> builder)
        {
            builder.ToTable("V_CARGASWEB");
            builder.HasKey(x => x.CAR_ID);
            builder.Property(x => x.TRIANGULAR5).HasColumnName("TRIANGULAR5").HasMaxLength(2).IsRequired();
            builder.Property(x => x.UF).HasColumnName("UF").HasMaxLength(2);
            builder.Property(x => x.MUN).HasColumnName("MUN").HasMaxLength(100);
            builder.Property(x => x.PON_ID).HasColumnName("PON_ID").HasMaxLength(100);
            builder.Property(x => x.PON_DESCRICAO).HasColumnName("PON_DESCRICAO").HasMaxLength(100);
            builder.Property(x => x.CLI_NOME).HasColumnName("CLI_NOME").HasMaxLength(100);
            builder.Property(x => x.CLI_ID).HasColumnName("CLI_ID").HasMaxLength(30);
            builder.Property(x => x.EMBARQUE).HasColumnName("EMBARQUE").HasMaxLength(30);
            builder.Property(x => x.DTEMBARQUE).HasColumnName("DTEMBARQUE");
            builder.Property(x => x.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(60);
            builder.Property(x => x.PRO_ID).HasColumnName("PRO_ID").HasMaxLength(30);
            builder.Property(x => x.ORD_QUANTIDADE).HasColumnName("ORD_QUANTIDADE");
            builder.Property(x => x.ORD_PESO_UNITARIO).HasColumnName("ORD_PESO_UNITARIO");
            builder.Property(x => x.QTD_ENTREGUE).HasColumnName("QTD_ENTREGUE").IsRequired();
            builder.Property(x => x.ITC_QTD_PLANEJADA).HasColumnName("ITC_QTD_PLANEJADA").IsRequired();
            builder.Property(x => x.QTD_UE).HasColumnName("QTD_UE");
            builder.Property(x => x.M3_PLANEJADO).HasColumnName("M3_PLANEJADO");
            builder.Property(x => x.M3_UE).HasColumnName("M3_UE");
            builder.Property(x => x.M3_PEDIDO).HasColumnName("M3_PEDIDO");
            builder.Property(x => x.PRO_LARGURA_EMBALADA).HasColumnName("PRO_LARGURA_EMBALADA");
            builder.Property(x => x.PRO_COMPRIMENTO_EMBALADA).HasColumnName("PRO_COMPRIMENTO_EMBALADA");
            builder.Property(x => x.PRO_ALTURA_EMBALADA).HasColumnName("PRO_ALTURA_EMBALADA");
            builder.Property(x => x.TRIANGULAR).HasColumnName("TRIANGULAR").HasMaxLength(1).IsRequired();
            builder.Property(x => x.SALDO_ESTOQUE).HasColumnName("SALDO_ESTOQUE").IsRequired();
            builder.Property(x => x.SALDO_ESTOQUE_UE).HasColumnName("SALDO_ESTOQUE_UE");
            builder.Property(x => x.PECENT_ESTOQUE_PRONTO).HasColumnName("PECENT_ESTOQUE_PRONTO");
            builder.Property(x => x.FIMPREVISTO).HasColumnName("FIMPREVISTO").IsRequired();
            builder.Property(x => x.CAR_ID).HasColumnName("CAR_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.ITC_ORDEM_ENTREGA).HasColumnName("ITC_ORDEM_ENTREGA").IsRequired();
            builder.Property(x => x.TIP_ID).HasColumnName("TIP_ID");
            builder.Property(x => x.CAR_DATA_INICIO_PREVISTO).HasColumnName("CAR_DATA_INICIO_PREVISTO");
            builder.Property(x => x.CAR_DATA_FIM_PREVISTO).HasColumnName("CAR_DATA_FIM_PREVISTO");
            builder.Property(x => x.CAR_STATUS).HasColumnName("CAR_STATUS");
            builder.Property(x => x.FPR_COR_FILA).HasColumnName("FPR_COR_FILA").HasMaxLength(30);
            builder.Property(x => x.CAR_PESO_TEORICO).HasColumnName("CAR_PESO_TEORICO");
            builder.Property(x => x.CAR_VOLUME_TEORICO).HasColumnName("CAR_VOLUME_TEORICO");
            builder.Property(x => x.CAR_PESO_REAL).HasColumnName("CAR_PESO_REAL");
            builder.Property(x => x.CAR_VOLUME_REAL).HasColumnName("CAR_VOLUME_REAL");
            builder.Property(x => x.CAR_PESO_EMBALAGEM).HasColumnName("CAR_PESO_EMBALAGEM");
            builder.Property(x => x.CAR_PESO_ENTRADA).HasColumnName("CAR_PESO_ENTRADA");
            builder.Property(x => x.CAR_PESO_SAIDA).HasColumnName("CAR_PESO_SAIDA");
            builder.Property(x => x.CAR_ID_DOCA).HasColumnName("CAR_ID_DOCA").HasMaxLength(30);
            builder.Property(x => x.CAR_ID_JUNTADA).HasColumnName("CAR_ID_JUNTADA").HasMaxLength(30);
            builder.Property(x => x.ORD_DATA_ENTREGA_DE).HasColumnName("ORD_DATA_ENTREGA_DE");
            builder.Property(x => x.ORD_DATA_ENTREGA_ATE).HasColumnName("ORD_DATA_ENTREGA_ATE");
            builder.Property(x => x.ORD_TOLERANCIA_MAIS).HasColumnName("ORD_TOLERANCIA_MAIS");
            builder.Property(x => x.ORD_TOLERANCIA_MENOS).HasColumnName("ORD_TOLERANCIA_MENOS");
            builder.Property(x => x.ORD_REGIAO_ENTREGA).HasColumnName("ORD_REGIAO_ENTREGA").HasMaxLength(100);
            builder.Property(x => x.PRO_DESCRICAO).HasColumnName("PRO_DESCRICAO").HasMaxLength(100);
            builder.Property(x => x.ORD_TIPO).HasColumnName("ORD_TIPO");
            builder.Property(x => x.CAR_DATA_INICIO_REALIZADO).HasColumnName("CAR_DATA_INICIO_REALIZADO");
            builder.Property(x => x.ITC_QTD).HasColumnName("ITC_QTD").IsRequired();
            builder.Property(x => x.TIP_DESCRICAO).HasColumnName("TIP_DESCRICAO").HasMaxLength(100);
            builder.Property(x => x.VEI_PLACA).HasColumnName("VEI_PLACA").HasMaxLength(8).IsRequired();
            builder.Property(x => x.TRA_NOME).HasColumnName("TRA_NOME").HasMaxLength(100).IsRequired();
            builder.Property(x => x.CAPACIDADE).HasColumnName("CAPACIDADE");
            builder.Property(x => x.OCUPACAO).HasColumnName("OCUPACAO");
            builder.Property(x => x.TIP_CAPACIDADE_M3).HasColumnName("TIP_CAPACIDADE_M3");
            builder.Property(x => x.QTD_DESCARGAS).HasColumnName("QTD_DESCARGAS");
            builder.Property(x => x.ORD_TIPO_FRETE).HasColumnName("ORD_TIPO_FRETE").HasMaxLength(3);
            builder.Property(x => x.ORD_COR_FILA).HasColumnName("ORD_COR_FILA").HasMaxLength(8);
            builder.Property(x => x.FPR_DATA_INICIO_PREVISTA).HasColumnName("FPR_DATA_INICIO_PREVISTA");
            builder.Property(x => x.FFF).HasColumnName("FFF");
            builder.Property(x => x.CAR_EMBARQUE_ALVO).HasColumnName("CAR_EMBARQUE_ALVO");
            builder.Property(x => x.COR_OTIF).HasColumnName("COR_OTIF").HasMaxLength(8);
            builder.Property(x => x.GRP_TIPO).HasColumnName("GRP_TIPO");
            builder.Property(x => x.PRO_PECAS_POR_FARDO).HasColumnName("PRO_PECAS_POR_FARDO");
            builder.Property(x => x.PRO_CAMADAS_POR_PALETE).HasColumnName("PRO_CAMADAS_POR_PALETE");
            builder.Property(x => x.PRO_FARDOS_POR_CAMADA).HasColumnName("PRO_FARDOS_POR_CAMADA");
        }
    }

    public class MapaMap : IEntityTypeConfiguration<Mapa>
    {
        public void Configure(EntityTypeBuilder<Mapa> builder)
        {
            builder.ToTable("T_MAPA");
            builder.HasKey(x => x.MAP_ID);
            builder.Property(x => x.MAP_ID).HasColumnName("MAP_ID").IsRequired();
            builder.Property(x => x.PON_ID).HasColumnName("PON_ID").HasMaxLength(100).IsRequired();
            builder.Property(x => x.PON_ID_VIZINHO).HasColumnName("PON_ID_VIZINHO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.MAP_DISTANCIA).HasColumnName("MAP_DISTANCIA").IsRequired();
            builder.Property(x => x.MAP_CUSTO_PEDAGIO_POR_EIXO).HasColumnName("MAP_CUSTO_PEDAGIO_POR_EIXO");
            builder.Property(x => x.ROD_ID).HasColumnName("ROD_ID").IsRequired();
            builder.Property(x => x.MAP_ALTURA_ROD).HasColumnName("MAP_ALTURA_ROD").IsRequired();

            builder.HasOne(x => x.PontosMapa_ID).WithMany(pm => pm.Mapa_ID).HasForeignKey(x => x.PON_ID);
            builder.HasOne(x => x.PontosMapa_ID_VIZINHO).WithMany(pm => pm.Mapa_ID_VIZINHO).HasForeignKey(x => x.PON_ID_VIZINHO);
            builder.HasOne(x => x.Rodovias).WithMany(r => r.Mapas).HasForeignKey(x => x.ROD_ID);
        }
    }

    public class PontosMapaMap : IEntityTypeConfiguration<PontosMapa>
    {
        public void Configure(EntityTypeBuilder<PontosMapa> builder)
        {
            builder.ToTable("T_PONTOS_MAPA");
            builder.HasKey(x => x.PON_ID);
            builder.Property(x => x.PON_ID).HasColumnName("PON_ID").HasMaxLength(100).IsRequired();
            builder.Property(x => x.PON_DESCRICAO).HasColumnName("PON_DESCRICAO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.PON_TIPO).HasColumnName("PON_TIPO").HasMaxLength(3).IsRequired();
            builder.Property(x => x.PON_LATITUDE).HasColumnName("PON_LATITUDE");
            builder.Property(x => x.PON_LONGITUDE).HasColumnName("PON_LONGITUDE");
            builder.Property(x => x.PON_DISTANCIA_KM).HasColumnName("PON_DISTANCIA_KM");
        }
    }
    public class TransportadoraMap : IEntityTypeConfiguration<Transportadora>
    {
        public void Configure(EntityTypeBuilder<Transportadora> builder)
        {
            builder.ToTable("T_TRANSPORTADORA");
            builder.HasKey(x => x.TRA_ID);
            builder.Property(x => x.TRA_ID).HasColumnName("TRA_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.TRA_NOME).HasColumnName("TRA_NOME").HasMaxLength(100);
            builder.Property(x => x.TRA_EMAIL).HasColumnName("TRA_EMAIL").HasMaxLength(1000);
            builder.Property(x => x.TRA_RESPONSAVEL).HasColumnName("TRA_RESPONSAVEL").HasMaxLength(100);
            builder.Property(x => x.TRA_FONE).HasColumnName("TRA_FONE").HasMaxLength(15);
            builder.Property(x => x.TRA_ID_INTEGRACAO).HasColumnName("TRA_ID_INTEGRACAO").HasMaxLength(100);
            builder.Property(x => x.TRA_ID_INTEGRACAO_ERP).HasColumnName("TRA_ID_INTEGRACAO_ERP").HasMaxLength(100);
        }
    }

    public class RodoviaMap : IEntityTypeConfiguration<Rodovias>
    {

        public void Configure(EntityTypeBuilder<Rodovias> builder)
        {
            builder.ToTable("T_RODOVIAS");
            builder.HasKey(x => x.ROD_ID);
            builder.Property(x => x.ROD_ID).HasColumnName("ROD_ID").IsRequired();
            builder.Property(x => x.ROD_DESCRICAO).HasColumnName("ROD_DESCRICAO").HasMaxLength(100);
        }
    }
    public class Municipios_CoordenadasMap : IEntityTypeConfiguration<Municipios_Coordenadas>
    {
        public void Configure(EntityTypeBuilder<Municipios_Coordenadas> builder)
        {
            builder.ToTable("T_MUNICIPIOS_COORDENADAS");
            builder.Property(x => x.MUN_CODIGO_IBGE).HasColumnName("MUN_CODIGO_IBGE");
            builder.Property(x => x.MUN_NOME).HasColumnName("MUN_NOME");
            builder.Property(x => x.MUN_LATITUDE).HasColumnName("MUN_LATITUDE");
            builder.Property(x => x.MUN_LONGITUDE).HasColumnName("MUN_LONGITUDE");
            builder.Property(x => x.EST_UF).HasColumnName("EST_UF");
            builder.HasKey(x => new { x.MUN_CODIGO_IBGE });
        }
    }


    public class TemposLogisticosMap : IEntityTypeConfiguration<TemposLogisticos>
    {
        public void Configure(EntityTypeBuilder<TemposLogisticos> builder)
        {
            builder.ToTable("T_TEMPOS_LOGISTICA");
            builder.HasKey(x => new { x.TMP_TIPO_TEMPO, x.TMP_TIPO_CARGA, x.CLI_ID });
            builder.Property(x => x.TMP_TIPO_TEMPO).HasColumnName("TMP_TIPO_TEMPO").HasMaxLength(50).IsRequired();
            builder.Property(x => x.TMP_TIPO_CARGA).HasColumnName("TMP_TIPO_CARGA").HasMaxLength(50).IsRequired();
            builder.Property(x => x.TMP_TEMPO_MEDIO_UNITARIO).HasColumnName("TMP_TEMPO_MEDIO_UNITARIO").IsRequired();
            builder.Property(x => x.CLI_ID).HasColumnName("CLI_ID").HasMaxLength(30).IsRequired();
        }
    }

    public class RestricoesDeRodagemMap : IEntityTypeConfiguration<RestricoesDeRodagem>
    {
        public void Configure(EntityTypeBuilder<RestricoesDeRodagem> builder)
        {
            builder.ToTable("T_RESTRICOES_DE_RODAGEM");
            builder.HasKey(x => x.RES_ID);
            builder.Property(x => x.RES_ID).HasColumnName("RES_ID").IsRequired();
            builder.Property(x => x.RES_TIPO).HasColumnName("RES_TIPO").HasMaxLength(1).IsRequired();
            builder.Property(x => x.RES_HORA_INI).HasColumnName("RES_HORA_INI").HasMaxLength(5);
            builder.Property(x => x.RES_HORA_FIM).HasColumnName("RES_HORA_FIM").HasMaxLength(10);
            builder.Property(x => x.RES_VELOCIDADE_HORA_RUSH).HasColumnName("RES_VELOCIDADE_HORA_RUSH");
            builder.Property(x => x.TVE_ID).HasColumnName("TVE_ID");
            builder.Property(x => x.MAP_ID).HasColumnName("MAP_ID");

            builder.HasOne(x => x.Mapa).WithMany(mp => mp.RestricoesDeRodagem).HasForeignKey(x => x.MAP_ID);
        }
    }


    public class CalendarioDisponibilidadeVeiculosMap : IEntityTypeConfiguration<CalendarioDisponibilidadeVeiculos>
    {
        public void Configure(EntityTypeBuilder<CalendarioDisponibilidadeVeiculos> builder)
        {
            builder.ToTable("T_CALENDARIO_DISPONIBILIDADE_VEICULOS");
            builder.HasKey(x => x.CDV_ID);
            builder.Property(x => x.CDV_ID).HasColumnName("CDV_ID").IsRequired();
            builder.Property(x => x.CDV_DATA_DE).HasColumnName("CDV_DATA_DE");
            builder.Property(x => x.CDV_DATA_ATE).HasColumnName("CDV_DATA_ATE");
            builder.Property(x => x.CDV_SEGUNDA).HasColumnName("CDV_SEGUNDA");
            builder.Property(x => x.CDV_TERCA).HasColumnName("CDV_TERCA");
            builder.Property(x => x.CDV_QUARTA).HasColumnName("CDV_QUARTA");
            builder.Property(x => x.CDV_QUINTA).HasColumnName("CDV_QUINTA");
            builder.Property(x => x.CDV_SEXTA).HasColumnName("CDV_SEXTA");
            builder.Property(x => x.CDV_SABADO).HasColumnName("CDV_SABADO");
            builder.Property(x => x.CDV_DOMINGO).HasColumnName("CDV_DOMINGO");
        }
    }
    public class ItenCalendarioDisponibilidadeVeiculosMap : IEntityTypeConfiguration<ItenCalendarioDisponibilidadeVeiculos>
    {
        public void Configure(EntityTypeBuilder<ItenCalendarioDisponibilidadeVeiculos> builder)
        {
            builder.ToTable("T_ITENS_CALENDARIO_DISPONIBILIDADE_VEICULOS");
            builder.HasKey(up => new { up.CDV_ID, up.TIP_ID });
            builder.HasOne(x => x.TipoVeiculo).WithMany(tv => tv.ItenCalendarioDisponibilidadeVeiculos).HasForeignKey(x => x.TIP_ID);
            builder.HasOne(x => x.CalendarioDisponibilidadeVeiculos).WithMany(tv => tv.ItenCalendarioDisponibilidadeVeiculos).HasForeignKey(x => x.CDV_ID);
            builder.Property(x => x.CDV_ID).HasColumnName("CDV_ID");
            builder.Property(x => x.TIP_ID).HasColumnName("TIP_ID");
            builder.Property(x => x.IDV_QTD).HasColumnName("IDV_QTD");
        }
    }
    public class PontosEntregaMap : IEntityTypeConfiguration<PontosEntrega>
    {
        public void Configure(EntityTypeBuilder<PontosEntrega> builder)
        {
            builder.ToTable("V_PONTOS_ENTREGA");
            builder.HasKey(x => x.PON_ID);
            builder.Property(x => x.PON_ID).HasColumnName("PON_ID").HasMaxLength(100).IsRequired();
            builder.Property(x => x.PON_LATITUDE).HasColumnName("PON_LATITUDE");
            builder.Property(x => x.PON_LONGITUDE).HasColumnName("PON_LONGITUDE");
            builder.Property(x => x.PON_DESCRICAO).HasColumnName("PON_DESCRICAO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.CAR_ID).HasColumnName("CAR_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.CAR_EMBARQUE_ALVO).HasColumnName("CAR_EMBARQUE_ALVO");
        }
    }


    public class V_DISPONIBILIDADE_VEICULOMap : IEntityTypeConfiguration<V_DISPONIBILIDADE_VEICULO>
    {
        public void Configure(EntityTypeBuilder<V_DISPONIBILIDADE_VEICULO> builder)
        {
            builder.ToTable("V_DISPONIBILIDADE_VEICULO");
            builder.HasKey(x => new { x.TIP_ID, x.CDV_DATA_DE, x.CDV_DATA_ATE });
            builder.Property(x => x.SALDO).HasColumnName("SALDO");
            builder.Property(x => x.TIP_CAPACIDADE_LARGURA_PESCOCO_D).HasColumnName("TIP_CAPACIDADE_LARGURA_PESCOCO_D");
            builder.Property(x => x.TIP_CAPACIDADE_COMPRIMENTO_PESCOCO_D).HasColumnName("TIP_CAPACIDADE_COMPRIMENTO_PESCOCO_D");
            builder.Property(x => x.TIP_CAPACIDADE_ALTURA_PESCOCO_D).HasColumnName("TIP_CAPACIDADE_ALTURA_PESCOCO_D");
            builder.Property(x => x.TIP_CAPACIDADE_LARGURA_PESCOCO_E).HasColumnName("TIP_CAPACIDADE_LARGURA_PESCOCO_E");
            builder.Property(x => x.TIP_CAPACIDADE_COMPRIMENTO_PESCOCO_E).HasColumnName("TIP_CAPACIDADE_COMPRIMENTO_PESCOCO_E");
            builder.Property(x => x.TIP_CAPACIDADE_ALTURA_PESCOCO_E).HasColumnName("TIP_CAPACIDADE_ALTURA_PESCOCO_E");
            builder.Property(x => x.TIP_ID).HasColumnName("TIP_ID").IsRequired();
            builder.Property(x => x.TIP_DESCRICAO).HasColumnName("TIP_DESCRICAO").HasMaxLength(109);
            builder.Property(x => x.TIP_QTD_DISPONIVEL).HasColumnName("TIP_QTD_DISPONIVEL");
            builder.Property(x => x.TIP_VALOR_KM).HasColumnName("TIP_VALOR_KM");
            builder.Property(x => x.TIP_VALOR_DIARIA).HasColumnName("TIP_VALOR_DIARIA");
            builder.Property(x => x.TIP_VALOR_AJUDANTE).HasColumnName("TIP_VALOR_AJUDANTE");
            builder.Property(x => x.TIP_QTD_EIXOS).HasColumnName("TIP_QTD_EIXOS");
            builder.Property(x => x.TIP_VELOCIDADE_MEDIA).HasColumnName("TIP_VELOCIDADE_MEDIA");
            builder.Property(x => x.TIP_CAPACIDADE_ALTURA).HasColumnName("TIP_CAPACIDADE_ALTURA");
            builder.Property(x => x.TIP_CAPACIDADE_COMPRIMENTO).HasColumnName("TIP_CAPACIDADE_COMPRIMENTO");
            builder.Property(x => x.TIP_CAPACIDADE_LARGURA).HasColumnName("TIP_CAPACIDADE_LARGURA");
            builder.Property(x => x.TIP_CAPACIDADE_M3).HasColumnName("TIP_CAPACIDADE_M3");
            builder.Property(x => x.CDV_DATA_DE).HasColumnName("CDV_DATA_DE").IsRequired();
            builder.Property(x => x.CDV_DATA_ATE).HasColumnName("CDV_DATA_ATE").IsRequired();
        }
    }


}










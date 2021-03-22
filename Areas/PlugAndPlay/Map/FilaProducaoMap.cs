using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class FilaProducaoMap : IEntityTypeConfiguration<FilaProducao>
    {
        public void Configure(EntityTypeBuilder<FilaProducao> builder)
        {
            builder.ToTable("T_FILA_PRODUCAO");
            builder.HasKey(x => x.FPR_ID);
            builder.Property(x => x.FPR_ID).HasColumnName("FPR_ID").IsRequired();
            builder.Property(x => x.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(60).IsRequired();
            builder.Property(x => x.ROT_PRO_ID).HasColumnName("ROT_PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.FPR_QUANTIDADE_PREVISTA).HasColumnName("FPR_QUANTIDADE_PREVISTA").IsRequired();
            builder.Property(x => x.ROT_MAQ_ID).HasColumnName("ROT_MAQ_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.FPR_DATA_INICIO_PREVISTA).HasColumnName("FPR_DATA_INICIO_PREVISTA").IsRequired();
            builder.Property(x => x.FPR_DATA_FIM_PREVISTA).HasColumnName("FPR_DATA_FIM_PREVISTA").IsRequired();
            builder.Property(x => x.FPR_DATA_FIM_MAXIMA).HasColumnName("FPR_DATA_FIM_MAXIMA").IsRequired();
            builder.Property(x => x.ROT_SEQ_TRANFORMACAO).HasColumnName("ROT_SEQ_TRANFORMACAO").IsRequired();
            builder.Property(x => x.FPR_SEQ_REPETICAO).HasColumnName("FPR_SEQ_REPETICAO").IsRequired();
            builder.Property(x => x.FPR_OBS_PRODUCAO).HasColumnName("FPR_OBS_PRODUCAO").HasMaxLength(4000);
            builder.Property(x => x.FPR_STATUS).HasColumnName("FPR_STATUS").HasMaxLength(2);
            builder.Property(x => x.FPR_TEMPO_DECORRIDO_SETUP).HasColumnName("FPR_TEMPO_DECORRIDO_SETUP");
            builder.Property(x => x.FPR_TEMPO_DECORRIDO_SETUPA).HasColumnName("FPR_TEMPO_DECORRIDO_SETUPA");
            builder.Property(x => x.FPR_TEMPO_DECORRIDO_PERFORMANC).HasColumnName("FPR_TEMPO_DECORRIDO_PERFORMANC");
            builder.Property(x => x.FPR_TEMPO_DECO_PEQUENA_PARADA).HasColumnName("FPR_TEMPO_DECO_PEQUENA_PARADA");
            builder.Property(x => x.FPR_QTD_PERFORMANCE).HasColumnName("FPR_QTD_PERFORMANCE");
            builder.Property(x => x.FPR_QTD_SETUP).HasColumnName("FPR_QTD_SETUP");
            builder.Property(x => x.FPR_QTD_PRODUZIDA).HasColumnName("FPR_QTD_PRODUZIDA");
            builder.Property(x => x.FPR_TEMPO_TEORICO_PERFORMANCE).HasColumnName("FPR_TEMPO_TEORICO_PERFORMANCE");
            builder.Property(x => x.FPR_TEMPO_RESTANTE_PERFORMANC).HasColumnName("FPR_TEMPO_RESTANTE_PERFORMANC");
            builder.Property(x => x.FPR_VELOCIDADE_P_ATINGIR_META).HasColumnName("FPR_VELOCIDADE_P_ATINGIR_META");
            builder.Property(x => x.FPR_QTD_RESTANTE).HasColumnName("FPR_QTD_RESTANTE");
            builder.Property(x => x.FPR_VELO_ATU_PC_SEGUNDO).HasColumnName("FPR_VELO_ATU_PC_SEGUNDO");
            builder.Property(x => x.FPR_PERFORMANCE_PROJETADA).HasColumnName("FPR_PERFORMANCE_PROJETADA");
            builder.Property(x => x.FPR_TEMPO_RESTANTE_TOTAL).HasColumnName("FPR_TEMPO_RESTANTE_TOTAL");
            builder.Property(x => x.FPR_FIM_PREVISTO_ATUAL).HasColumnName("FPR_FIM_PREVISTO_ATUAL");
            builder.Property(x => x.FPR_PRODUZINDO).HasColumnName("FPR_PRODUZINDO");
            builder.Property(x => x.FPR_ORDEM_NA_FILA).HasColumnName("FPR_ORDEM_NA_FILA");
            builder.Property(x => x.FPR_ID_INTEGRACAO).HasColumnName("FPR_ID_INTEGRACAO").HasMaxLength(100);
            builder.Property(x => x.FPR_TRUNCADO).HasColumnName("FPR_TRUNCADO").HasMaxLength(1);
            builder.Property(x => x.FPR_DATA_TRUNC_INI).HasColumnName("FPR_DATA_TRUNC_INI");
            builder.Property(x => x.FPR_DATA_TRUNC_FIM).HasColumnName("FPR_DATA_TRUNC_FIM");
            builder.Property(x => x.FPR_COR_FILA).HasColumnName("FPR_COR_FILA").HasMaxLength(30);
            builder.Property(x => x.MAQ_ID_MANUAL).HasColumnName("MAQ_ID_MANUAL").HasMaxLength(30);
            builder.Property(x => x.FPR_PREVISAO_MATERIA_PRIMA).HasColumnName("FPR_PREVISAO_MATERIA_PRIMA").IsRequired();
            builder.Property(x => x.FPR_DATA_NECESSIDADE_INICIO_PRODUCAO).HasColumnName("FPR_DATA_NECESSIDADE_INICIO_PRODUCAO");
            builder.Property(x => x.FPR_DATA_NECESSIDADE_FIM_PRODUCAO).HasColumnName("FPR_DATA_NECESSIDADE_FIM_PRODUCAO");
            builder.Property(x => x.FPR_COR_BICO1).HasColumnName("FPR_COR_BICO1").HasMaxLength(30);
            builder.Property(x => x.FPR_COR_BICO2).HasColumnName("FPR_COR_BICO2").HasMaxLength(30);
            builder.Property(x => x.FPR_COR_BICO3).HasColumnName("FPR_COR_BICO3").HasMaxLength(30);
            builder.Property(x => x.FPR_COR_BICO4).HasColumnName("FPR_COR_BICO4").HasMaxLength(30);
            builder.Property(x => x.FPR_COR_BICO5).HasColumnName("FPR_COR_BICO5").HasMaxLength(30);
            builder.Property(x => x.FPR_META_SETUP).HasColumnName("FPR_META_SETUP");
            builder.Property(x => x.FPR_PRIORIDADE).HasColumnName("FPR_PRIORIDADE");
            builder.Property(x => x.FPR_GRUPO_PRODUTIVO).HasColumnName("FPR_GRUPO_PRODUTIVO");
            builder.Property(x => x.FPR_GRUPO_PRODUTIVO_MANUAL).HasColumnName("FPR_GRUPO_PRODUTIVO_MANUAL");
            builder.Property(x => x.FPR_ORD_ID_REPROGRAMADO).HasColumnName("FPR_ORD_ID_REPROGRAMADO").HasMaxLength(30);
            builder.Property(x => x.FPR_HIERARQUIA_SEQ_TRANSFORMACAO).HasColumnName("FPR_HIERARQUIA_SEQ_TRANSFORMACAO");
            builder.Property(x => x.FPR_DATA_ENTREGA).HasColumnName("FPR_DATA_ENTREGA");
            builder.Property(x => x.FPR_ID_ORIGEM).HasColumnName("FPR_ID_ORIGEM");
            builder.Property(x => x.EQU_ID).HasColumnName("EQU_ID").HasMaxLength(30);
            builder.Property(x => x.FPR_EMISSAO).HasColumnName("FPR_EMISSAO");
            builder.Property(x => x.FPR_MOTIVO_PULA_FILA).HasColumnName("FPR_MOTIVO_PULA_FILA").HasMaxLength(140);
            //builder.Property(x => x.PRO_IMG_LASTRO).HasColumnName("PRO_IMG_LASTRO").HasMaxLength(50);

            builder.HasOne(x => x.Order).WithMany(x => x.FilasProducao).HasForeignKey(x => x.ORD_ID);
            builder.HasOne(x => x.Maquina).WithMany(x => x.FilasProducao).HasForeignKey(x => x.ROT_MAQ_ID);
            builder.HasOne(x => x.Roteiro).WithMany(x => x.FilasProducao).HasForeignKey(x => new { x.ROT_MAQ_ID, x.ROT_PRO_ID, x.ROT_SEQ_TRANFORMACAO });
            builder.HasOne(x => x.Produto).WithMany(x => x.FilasProducao).HasForeignKey(x => x.ROT_PRO_ID);
            builder.HasOne(x => x.OcorrenciaPularOrdemFila).WithMany(o => o.FilaProducao).HasForeignKey(x => x.OCO_ID);
        }
    }

    public class V_PEDIDOS_PARCIALMENTE_PRODUZIDOS_MAP : IEntityTypeConfiguration<V_PEDIDOS_PARCIALMENTE_PRODUZIDOS>
    {
        public void Configure(EntityTypeBuilder<V_PEDIDOS_PARCIALMENTE_PRODUZIDOS> builder)
        {
            builder.ToTable("V_PEDIDOS_PARCIALMENTE_PRODUZIDOS");
            builder.HasKey(x => new { x.ORD_ID, x.ROT_SEQ_TRANFORMACAO, x.FPR_SEQ_REPETICAO, x.ROT_PRO_ID });
            builder.Property(x => x.FASE).HasColumnName("FASE").HasMaxLength(15).IsRequired();
            builder.Property(x => x.ROT_MAQ_ID).HasColumnName("ROT_MAQ_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(60).IsRequired();
            builder.Property(x => x.ROT_PRO_ID).HasColumnName("ROT_PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.ROT_SEQ_TRANFORMACAO).HasColumnName("ROT_SEQ_TRANFORMACAO").IsRequired();
            builder.Property(x => x.FPR_SEQ_REPETICAO).HasColumnName("FPR_SEQ_REPETICAO").IsRequired();
            builder.Property(x => x.FPR_QUANTIDADE_PREVISTA).HasColumnName("FPR_QUANTIDADE_PREVISTA").IsRequired();
            builder.Property(x => x.ORD_TOLERANCIA_MENOS).HasColumnName("ORD_TOLERANCIA_MENOS");
            builder.Property(x => x.DATA_PRODUCAO).HasColumnName("DATA_PRODUCAO");
            builder.Property(x => x.SALDO_A_PRODUZIR).HasColumnName("SALDO_A_PRODUZIR");
        }

    }

    public class EstruturaDoScheduleMap : IEntityTypeConfiguration<EstruturaDoSchedule>
    {
        public void Configure(EntityTypeBuilder<EstruturaDoSchedule> builder)
        {
            builder.ToTable("V_ESTRUTURA_DO_SCHEDULE");
            builder.HasKey(x => new { x.PRO_ID_PRODUTO, x.PRO_ID_COMPONENTE });
            builder.Property(x => x.EST_DATA_VALIDADE).HasColumnName("EST_DATA_VALIDADE").IsRequired();
            builder.Property(x => x.PRO_ID_PRODUTO).HasColumnName("PRO_ID_PRODUTO").HasMaxLength(30).IsRequired();
            builder.Property(x => x.PRO_ID_COMPONENTE).HasColumnName("PRO_ID_COMPONENTE").HasMaxLength(30).IsRequired();
            builder.Property(x => x.EST_QUANT).HasColumnName("EST_QUANT").IsRequired();
            builder.Property(x => x.EST_DATA_INCLUSAO).HasColumnName("EST_DATA_INCLUSAO").IsRequired();
            builder.Property(x => x.EST_BASE_PRODUCAO).HasColumnName("EST_BASE_PRODUCAO").IsRequired();
            builder.Property(x => x.EST_TIPO_REQUISICAO).HasColumnName("EST_TIPO_REQUISICAO").HasMaxLength(2).IsRequired();
            builder.Property(x => x.GRP_TIPO_P).HasColumnName("GRP_TIPO_P").IsRequired();
            builder.Property(x => x.GRP_ID_P).HasColumnName("GRP_ID_P").HasMaxLength(30).IsRequired();
            builder.Property(x => x.GRP_TIPO_C).HasColumnName("GRP_TIPO_C").IsRequired();
            builder.Property(x => x.GRP_ID_C).HasColumnName("GRP_ID_C").HasMaxLength(30).IsRequired();
            builder.Property(x => x.PRO_ESCALA_COR).HasColumnName("PRO_ESCALA_COR").HasMaxLength(30);
            builder.Property(x => x.PRO_CUSTO_SUBIDA_ESCALA_COR).HasColumnName("PRO_CUSTO_SUBIDA_ESCALA_COR");
            builder.Property(x => x.PRO_CUSTO_DECIDA_ESCALA_COR).HasColumnName("PRO_CUSTO_DECIDA_ESCALA_COR");
            builder.Property(x => x.PRO_COMPRIMENTO_PECA).HasColumnName("PRO_COMPRIMENTO_PECA");
            builder.Property(x => x.PRO_LARGURA_PECA).HasColumnName("PRO_LARGURA_PECA");
            builder.Property(x => x.PRO_ALTURA_PECA).HasColumnName("PRO_ALTURA_PECA");
            builder.Property(x => x.EST_CODIGO_DE_EXCECAO).HasColumnName("EST_CODIGO_DE_EXCECAO").HasMaxLength(3000).IsRequired();
        }

    }

    public class FilaDoScheduleMap : IEntityTypeConfiguration<FilaDoSchedule>
    {
        public void Configure(EntityTypeBuilder<FilaDoSchedule> builder)
        {
            builder.ToTable("V_OPS_A_PLANEJAR_");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.MAX_ROT_SEQ_TRANFORMACAO).HasColumnName("MAX_ROT_SEQ_TRANFORMACAO");
            builder.Property(x => x.FPR_PRIORIDADE).HasColumnName("FPR_PRIORIDADE");
            builder.Property(x => x.ORD_STATUS).HasColumnName("ORD_STATUS").HasMaxLength(10);
            builder.Property(x => x.FPR_STATUS).HasColumnName("FPR_STATUS").HasMaxLength(2);
            builder.Property(x => x.MaquinaId).HasColumnName("MaquinaId").HasMaxLength(30).IsRequired();
            builder.Property(x => x.OrderId).HasColumnName("OrderId").HasMaxLength(60).IsRequired();
            builder.Property(x => x.ORD_PRO_ID).HasColumnName("ORD_PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.ProdutoId).HasColumnName("ProdutoId").HasMaxLength(30).IsRequired();
            builder.Property(x => x.SequenciaTransformacao).HasColumnName("SequenciaTransformacao").IsRequired();
            builder.Property(x => x.SequenciaRepeticao).HasColumnName("SequenciaRepeticao").IsRequired();
            builder.Property(x => x.MaquinaIdManual).HasColumnName("MaquinaIdManual").HasMaxLength(30);
            builder.Property(x => x.DataInicioPrevista).HasColumnName("DataInicioPrevista").IsRequired();
            builder.Property(x => x.DataFimPrevista).HasColumnName("DataFimPrevista").IsRequired();
            builder.Property(x => x.DataFimMaxima).HasColumnName("DataFimMaxima").IsRequired();
            builder.Property(x => x.PrevisaoMateriaPrima).HasColumnName("PrevisaoMateriaPrima").IsRequired();
            builder.Property(x => x.ObservacaoProducao).HasColumnName("ObservacaoProducao").HasMaxLength(1).IsRequired();
            builder.Property(x => x.QuantidadePrevista).HasColumnName("QuantidadePrevista").IsRequired();
            builder.Property(x => x.Status).HasColumnName("Status").HasMaxLength(1).IsRequired();
            builder.Property(x => x.Produzindo).HasColumnName("Produzindo").IsRequired();
            builder.Property(x => x.IdIntegracao).HasColumnName("IdIntegracao").HasMaxLength(1).IsRequired();
            builder.Property(x => x.QuantidadeProduzida).HasColumnName("QuantidadeProduzida");
            builder.Property(x => x.QuantidadeRestante).HasColumnName("QuantidadeRestante");
            builder.Property(x => x.TempoRestanteTotal).HasColumnName("TempoRestanteTotal");
            builder.Property(x => x.ORD_DATA_ENTREGA_DE).HasColumnName("ORD_DATA_ENTREGA_DE");
            builder.Property(x => x.ORD_DATA_ENTREGA_ATE).HasColumnName("ORD_DATA_ENTREGA_ATE");
            builder.Property(x => x.CLI_TRANSLADO).HasColumnName("CLI_TRANSLADO").IsRequired();
            builder.Property(x => x.TempoProducao).HasColumnName("TempoProducao");
            builder.Property(x => x.Performance).HasColumnName("Performance").IsRequired();
            builder.Property(x => x.TempoSetup).HasColumnName("TempoSetup").IsRequired();
            builder.Property(x => x.TempoSetupAjuste).HasColumnName("TempoSetupAjuste").IsRequired();
            builder.Property(x => x.PecasPorPulso).HasColumnName("PecasPorPulso").IsRequired();
            builder.Property(x => x.HIERARQUIA_SEQ_TRANSFORMACAO).HasColumnName("HIERARQUIA_SEQ_TRANSFORMACAO").IsRequired();
            builder.Property(x => x.AVALIA_CUSTO).HasColumnName("AVALIA_CUSTO").IsRequired();
            builder.Property(x => x.Truncado).HasColumnName("Truncado").HasMaxLength(1);
            builder.Property(x => x.DataInicioTrunc).HasColumnName("DataInicioTrunc");
            builder.Property(x => x.DataFimTrunc).HasColumnName("DataFimTrunc");
            builder.Property(x => x.OrdemDaFila).HasColumnName("OrdemDaFila");
            builder.Property(x => x.Id).HasColumnName("Id").IsRequired();
            builder.Property(x => x.ORD_LOTE_PILOTO).HasColumnName("ORD_LOTE_PILOTO");
            builder.Property(x => x.FPR_INICIO_GRUPO_PRODUTIVO).HasColumnName("FPR_INICIO_GRUPO_PRODUTIVO");
            builder.Property(x => x.FPR_FIM_GRUPO_PRODUTIVO).HasColumnName("FPR_FIM_GRUPO_PRODUTIVO");
            builder.Property(x => x.DataHoraNecessidadeInicioProducao).HasColumnName("DataHoraNecessidadeInicioProducao");
            builder.Property(x => x.DataHoraNecessidadeFimProducao).HasColumnName("DataHoraNecessidadeFimProducao");
            builder.Property(x => x.TempoDecorridoSetup).HasColumnName("TempoDecorridoSetup");
            builder.Property(x => x.TempoDecorridoSetupAjuste).HasColumnName("TempoDecorridoSetupAjuste");
            builder.Property(x => x.TempoDecorridoPerformacace).HasColumnName("TempoDecorridoPerformacace");
            builder.Property(x => x.QuantidadePerformace).HasColumnName("QuantidadePerformace");
            builder.Property(x => x.QuantidadeSetup).HasColumnName("QuantidadeSetup");
            builder.Property(x => x.TempoTeoricoPerformace).HasColumnName("TempoTeoricoPerformace");
            builder.Property(x => x.TempoRestantePerformace).HasColumnName("TempoRestantePerformace");
            builder.Property(x => x.VelocidadeAtingirMeta).HasColumnName("VelocidadeAtingirMeta");
            builder.Property(x => x.VeloAtuPcSegundo).HasColumnName("VeloAtuPcSegundo");
            builder.Property(x => x.PerformaceProjetada).HasColumnName("PerformaceProjetada");
            builder.Property(x => x.TempoDecorridoPequenasParadas).HasColumnName("TempoDecorridoPequenasParadas");
            builder.Property(x => x.AlocadaEmMaquina).HasColumnName("AlocadaEmMaquina");
            builder.Property(x => x.FPR_GRUPO_PRODUTIVO).HasColumnName("FPR_GRUPO_PRODUTIVO");
            builder.Property(x => x.CAR_INICIO_JANELA_EMBARQUE).HasColumnName("CAR_INICIO_JANELA_EMBARQUE").IsRequired();
            builder.Property(x => x.CAR_FIM_JANELA_EMBARQUE).HasColumnName("CAR_FIM_JANELA_EMBARQUE").IsRequired();
            builder.Property(x => x.EMBARQUE_ALVO).HasColumnName("EMBARQUE_ALVO").IsRequired();
            builder.Property(x => x.CLI_EXIGENTE_NA_IMPRESSAO).HasColumnName("CLI_EXIGENTE_NA_IMPRESSAO");
            builder.Property(x => x.FPR_COR_BICO1).HasColumnName("FPR_COR_BICO1").HasMaxLength(30);
            builder.Property(x => x.FPR_COR_BICO2).HasColumnName("FPR_COR_BICO2").HasMaxLength(30);
            builder.Property(x => x.FPR_COR_BICO3).HasColumnName("FPR_COR_BICO3").HasMaxLength(30);
            builder.Property(x => x.FPR_COR_BICO4).HasColumnName("FPR_COR_BICO4").HasMaxLength(30);
            builder.Property(x => x.FPR_COR_BICO5).HasColumnName("FPR_COR_BICO5").HasMaxLength(30);
            builder.Property(x => x.GRP_ID).HasColumnName("GRP_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.GRP_PAP_ONDA).HasColumnName("GRP_PAP_ONDA").HasMaxLength(10);
            builder.Property(x => x.ORD_TIPO).HasColumnName("ORD_TIPO");
            builder.Property(x => x.FPR_PREVISAO_MATERIA_PRIMA).HasColumnName("FPR_PREVISAO_MATERIA_PRIMA").IsRequired();
            builder.Property(x => x.GRP_TIPO).HasColumnName("GRP_TIPO");
            builder.Property(x => x.FPR_ID_ORIGEM).HasColumnName("FPR_ID_ORIGEM");
            builder.Property(x => x.FPR_DATA_ENTREGA).HasColumnName("FPR_DATA_ENTREGA");
            builder.Property(x => x.EQU_ID).HasColumnName("EQU_ID").HasMaxLength(30);
            builder.Property(x => x.PRO_COMPRIMENTO_PECA).HasColumnName("PRO_COMPRIMENTO_PECA");
            builder.Property(x => x.PRO_LARGURA_PECA).HasColumnName("PRO_LARGURA_PECA");
            builder.Property(x => x.ATRAZADO_UTILIZA_EMBARQUE).HasColumnName("ATRAZADO_UTILIZA_EMBARQUE");
            builder.Property(x => x.ORD_M2_UNITARIO).HasColumnName("ORD_M2_UNITARIO");
            builder.Property(x => x.ORD_PESO_UNITARIO).HasColumnName("ORD_PESO_UNITARIO");
            builder.Property(x => x.ORD_PESO_UNITARIO_BRUTO).HasColumnName("ORD_PESO_UNITARIO_BRUTO");
            builder.Property(x => x.GRP_PERFORMANCE_METRO_LINEAR_POR_SEGUNDO).HasColumnName("GRP_PERFORMANCE_METRO_LINEAR_POR_SEGUNDO");
        }
    }
}
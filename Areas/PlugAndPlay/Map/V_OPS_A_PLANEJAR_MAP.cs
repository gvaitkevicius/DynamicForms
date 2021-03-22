using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map
{
    public class V_OPS_A_PLANEJAR_MAP : IEntityTypeConfiguration<V_OPS_A_PLANEJAR>
    {
        public void Configure(EntityTypeBuilder<V_OPS_A_PLANEJAR> builder)
        {
            builder.ToTable("V_OPS_A_PLANEJAR");
            builder.HasKey(x => x.OrderId);
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
            builder.Property(x => x.GRP_PERFORMANCE_METRO_LINEAR_POR_SEGUNDO).HasColumnName("GRP_PERFORMANCE_METRO_LINEAR_POR_SEGUNDO").IsRequired();
        }
    }
}

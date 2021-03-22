using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class ViewFilaProducaoMap : IEntityTypeConfiguration<ViewFilaProducao>
    {

        public void Configure(EntityTypeBuilder<ViewFilaProducao> builder)
        {
            builder.ToTable("V_FILA_PRODUCAO");
            //cliente
            builder.Property(x => x.CliId).HasColumnName("CLI_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.CliNome).HasColumnName("CLI_NOME").HasMaxLength(100).IsRequired();
            builder.Property(x => x.CliFone).HasColumnName("CLI_FONE").HasMaxLength(100).IsRequired();
            builder.Property(x => x.CliObs).HasColumnName("CLI_OBS");
            //produto
            //            Property(x => x.RotPcProId).HasColumnName("ROT_PC_PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.PcProId).HasColumnName("PC_PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.PcProDescricao).HasColumnName("PC_PRO_DESCRICAO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.PcUniId).HasColumnName("PC_UNI_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.GRP_DESCRICAO).HasColumnName("GRP_DESCRICAO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.GRP_TIPO).HasColumnName("GRP_TIPO");
            //maquina
            builder.Property(x => x.RotMaqId).HasColumnName("ROT_MAQ_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.MaqDescricao).HasColumnName("MAQ_DESCRICAO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.GMA_ID).HasColumnName("GMA_ID").HasMaxLength(30).IsRequired();
            //order
            builder.Property(x => x.ORD_OP_INTEGRACAO).HasColumnName("ORD_OP_INTEGRACAO").HasMaxLength(30).IsRequired();
            builder.Property(x => x.OrdId).HasColumnName("ORD_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.OrdTipoCarregamento).HasColumnName("CAR_TIPO_CARREGAMENTO");
            builder.Property(x => x.PaProId).HasColumnName("PA_PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.PaProDescricao).HasColumnName("PA_PRO_DESCRICAO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.PA_PRO_INTEGRACAO).HasColumnName("PA_PRO_INTEGRACAO").HasMaxLength(100);
            builder.Property(x => x.PRO_IMG_LASTRO).HasColumnName("PRO_IMG_LASTRO").HasMaxLength(50);
            builder.Property(x => x.PaUniId).HasColumnName("PA_UNI_ID").HasMaxLength(30);
            builder.Property(x => x.OrdPrecoUnitario).HasColumnName("ORD_PRECO_UNITARIO");
            builder.Property(x => x.OrdQuantidade).HasColumnName("ORD_QUANTIDADE").IsRequired();
            builder.Property(x => x.OrdDataEntregaDe).HasColumnName("ORD_DATA_ENTREGA_DE").IsRequired();
            builder.Property(x => x.OrdDataEntregaAte).HasColumnName("ORD_DATA_ENTREGA_ATE").IsRequired();
            builder.Property(x => x.OrdTipo).HasColumnName("ORD_TIPO");
            builder.Property(x => x.OrdToleranciaMais).HasColumnName("ORD_TOLERANCIA_MAIS");
            builder.Property(x => x.OrdToleranciaMenos).HasColumnName("ORD_TOLERANCIA_MENOS");
            builder.Property(x => x.OrdInicioJanelaEmbarque).HasColumnName("ORD_INICIO_JANELA_EMBARQUE");
            builder.Property(x => x.OrdFimJanelaEmbarque).HasColumnName("ORD_FIM_JANELA_EMBARQUE");
            builder.Property(x => x.OrdEmbarqueAlvo).HasColumnName("ORD_EMBARQUE_ALVO");
            builder.Property(x => x.OrdInicioGrupoProdutivo).HasColumnName("ORD_INICIO_GRUPO_PRODUTIVO");
            builder.Property(x => x.OrdFimGrupoProdutivo).HasColumnName("ORD_FIM_GRUPO_PRODUTIVO");
            builder.Property(x => x.ORD_STATUS).HasColumnName("ORD_STATUS");
            builder.Property(x => x.ORD_M2_UNITARIO).HasColumnName("ORD_M2_UNITARIO");
            builder.Property(x => x.ORD_M2_TOTAL).HasColumnName("ORD_M2_TOTAL");
            builder.Property(x => x.ORD_LOTE_PILOTO).HasColumnName("ORD_LOTE_PILOTO");
            //fila
            builder.Property(x => x.FprDataInicioPrevista).HasColumnName("FPR_DATA_INICIO_PREVISTA").IsRequired();
            builder.Property(x => x.FprDataFimPrevista).HasColumnName("FPR_DATA_FIM_PREVISTA").IsRequired();
            builder.Property(x => x.FprDataFimMaxima).HasColumnName("FPR_DATA_FIM_MAXIMA").IsRequired();
            builder.Property(x => x.FprQuantidadePrevista).HasColumnName("FPR_QUANTIDADE_PREVISTA").IsRequired();
            builder.Property(x => x.RotSeqTransformacao).HasColumnName("ROT_SEQ_TRANFORMACAO").IsRequired();
            builder.Property(x => x.FprSeqRepeticao).HasColumnName("FPR_SEQ_REPETICAO").IsRequired();
            builder.Property(x => x.FprObsProducao).HasColumnName("FPR_OBS_PRODUCAO");
            builder.Property(x => x.FprStatus).HasColumnName("FPR_STATUS").HasMaxLength(2);
            builder.Property(x => x.Produzindo).HasColumnName("FPR_PRODUZINDO");
            builder.Property(x => x.CorFila).HasColumnName("FPR_COR_FILA");
            builder.Property(x => x.CorOrd).HasColumnName("ORD_COR_FILA");
            builder.Property(x => x.MaquinaIdManual).HasColumnName("MAQ_ID_MANUAL");
            builder.Property(x => x.PrevisaoMateriaPrima).HasColumnName("FPR_PREVISAO_MATERIA_PRIMA");
            builder.Property(x => x.EQU_ID).HasColumnName("EQU_ID").HasMaxLength(30);
            builder.Property(x => x.CorBico1).HasColumnName("FPR_COR_BICO1");
            builder.Property(x => x.CorBico2).HasColumnName("FPR_COR_BICO2");
            builder.Property(x => x.CorBico3).HasColumnName("FPR_COR_BICO3");
            builder.Property(x => x.CorBico4).HasColumnName("FPR_COR_BICO4");
            builder.Property(x => x.CorBico5).HasColumnName("FPR_COR_BICO5");
            builder.Property(x => x.FPR_META_SETUP).HasColumnName("FPR_META_SETUP");
            builder.Property(x => x.FPR_GRUPO_PRODUTIVO).HasColumnName("FPR_GRUPO_PRODUTIVO");
            builder.Property(x => x.MAQ_TIPO_PLANEJAMENTO).HasColumnName("MAQ_TIPO_PLANEJAMENTO");
            builder.Property(x => x.FPR_HIERARQUIA_SEQ_TRANSFORMACAO).HasColumnName("FPR_HIERARQUIA_SEQ_TRANSFORMACAO");
            //--NOVOS CAMPOS CONTROLE TRUNCAGEM
            builder.Property(x => x.Truncado).HasColumnName("FPR_TRUNCADO");
            builder.Property(x => x.DataInicioTrunc).HasColumnName("FPR_DATA_TRUNC_INI");
            builder.Property(x => x.DataFimTrunc).HasColumnName("FPR_DATA_TRUNC_FIM");
            builder.Property(x => x.DataHoraNecessidadeInicioProducao).HasColumnName("FPR_DATA_NECESSIDADE_INICIO_PRODUCAO");
            builder.Property(x => x.DataHoraNecessidadeFimProducao).HasColumnName("FPR_DATA_NECESSIDADE_FIM_PRODUCAO");

            //roteiro
            builder.Property(x => x.RotQuantPecasPulso).HasColumnName("ROT_PECAS_POR_PULSO");
            builder.Property(x => x.ROT_PERFORMANCE).HasColumnName("ROT_PERFORMANCE");
            //medicoes variaveis
            builder.Property(x => x.FprTempoDecorridoSetup).HasColumnName("FPR_TEMPO_DECORRIDO_SETUP");
            builder.Property(x => x.FprTempoDecorridoSetupAjuste).HasColumnName("FPR_TEMPO_DECORRIDO_SETUPA");
            builder.Property(x => x.FprTempoDecorridoPerformacace).HasColumnName("FPR_TEMPO_DECORRIDO_PERFORMANC");
            builder.Property(x => x.FprQuantidadePerformace).HasColumnName("FPR_QTD_PERFORMANCE");
            builder.Property(x => x.FprQuantidadeSetup).HasColumnName("FPR_QTD_SETUP");
            builder.Property(x => x.FprQuantidadeProduzida).HasColumnName("FPR_QTD_PRODUZIDA");
            builder.Property(x => x.FprTempoTeoricoPerformace).HasColumnName("FPR_TEMPO_TEORICO_PERFORMANCE");
            builder.Property(x => x.FprTempoRestantePerformace).HasColumnName("FPR_TEMPO_RESTANTE_PERFORMANC");
            builder.Property(x => x.FprVelocidadeAtingirMeta).HasColumnName("FPR_VELOCIDADE_P_ATINGIR_META");
            builder.Property(x => x.FprQuantidadeRestante).HasColumnName("FPR_QTD_RESTANTE");
            builder.Property(x => x.FprVeloAtuPcSegundo).HasColumnName("FPR_VELO_ATU_PC_SEGUNDO");
            builder.Property(x => x.FprPerformaceProjetada).HasColumnName("FPR_PERFORMANCE_PROJETADA");
            builder.Property(x => x.TempoDecorridoPequenasParadas).HasColumnName("FPR_TEMPO_DECO_PEQUENA_PARADA");
            builder.Property(x => x.OrdemDaFila).HasColumnName("FPR_ORDEM_NA_FILA");
            builder.Property(x => x.CongelaFila).HasColumnName("MAQ_CONGELA_FILA");
            builder.Property(x => x.Id).HasColumnName("FPR_ID");
            builder.Property(x => x.FPR_PRIORIDADE).HasColumnName("FPR_PRIORIDADE");

            builder.Property(x => x.ETI_QUANTIDADE_PALETE).HasColumnName("ETI_QUANTIDADE_PALETE");
            builder.Property(x => x.ETI_IMPRIMIR_DE).HasColumnName("ETI_IMPRIMIR_DE").IsRequired();
            builder.Property(x => x.ETI_IMPRIMIR_ATE).HasColumnName("ETI_IMPRIMIR_ATE");
            builder.Property(x => x.ETI_NUMERO_COPIAS).HasColumnName("ETI_NUMERO_COPIAS").IsRequired();
            builder.Property(x => x.TEMPLATE_TESTES).HasColumnName("TEMPLATE_TESTES").IsRequired();
            //primary key
            builder.HasKey(x => new { x.RotMaqId, x.OrdId, x.PaProId, x.FprSeqRepeticao, x.RotSeqTransformacao });
            builder.HasOne(x => x.Order).WithMany(x => x.ViewFilaProducao).HasForeignKey(x => x.OrdId);
            
        }
    }
}
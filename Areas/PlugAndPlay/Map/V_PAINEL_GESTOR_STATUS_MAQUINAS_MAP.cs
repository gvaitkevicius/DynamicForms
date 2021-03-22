﻿using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map
{
    public class V_PAINEL_GESTOR_STATUS_MAQUINAS_MAP : IEntityTypeConfiguration<V_PAINEL_GESTOR_STATUS_MAQUINAS>
    {
        public void Configure(EntityTypeBuilder<V_PAINEL_GESTOR_STATUS_MAQUINAS> builder)
        {
            builder.ToTable("V_PAINEL_GESTOR_STATUS_MAQUINAS");
            builder.HasKey(x => x.ROT_MAQ_ID);
            builder.Property(x => x.ROT_MAQ_ID).HasColumnName("ROT_MAQ_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.MAQ_DESCRICAO).HasColumnName("MAQ_DESCRICAO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.MAQ_STATUS).HasColumnName("MAQ_STATUS").HasMaxLength(30);
            builder.Property(x => x.STATUS_COR_MAQUINA).HasColumnName("STATUS_COR_MAQUINA").HasMaxLength(30);
            builder.Property(x => x.ULTIMA_ATUALIZACAO).HasColumnName("ULTIMA_ATUALIZACAO").HasMaxLength(30);
            builder.Property(x => x.OCO_ID).HasColumnName("OCO_ID").HasMaxLength(131).IsRequired();
            builder.Property(x => x.OCO_DESCRICAO).HasColumnName("OCO_DESCRICAO").HasMaxLength(100);
            builder.Property(x => x.FEE_OBSERVACOES).HasColumnName("FEE_OBSERVACOES").HasMaxLength(100);
            builder.Property(x => x.FEEDBACKS_PENDENTES).HasColumnName("FEEDBACKS_PENDENTES");
            builder.Property(x => x.TEMPO_SEM_FEEDBACK).HasColumnName("TEMPO_SEM_FEEDBACK").HasMaxLength(30);
            builder.Property(x => x.OPS_PARCIAIS).HasColumnName("OPS_PARCIAIS").HasMaxLength(1).IsRequired();
            builder.Property(x => x.CLI_ID).HasColumnName("CLI_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.CLI_NOME).HasColumnName("CLI_NOME").HasMaxLength(100).IsRequired();
            builder.Property(x => x.CLI_FONE).HasColumnName("CLI_FONE").HasMaxLength(68);
            builder.Property(x => x.CLI_OBS).HasColumnName("CLI_OBS").HasMaxLength(3000);
            builder.Property(x => x.PC_PRO_ID).HasColumnName("PC_PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.PC_PRO_DESCRICAO).HasColumnName("PC_PRO_DESCRICAO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.PC_UNI_ID).HasColumnName("PC_UNI_ID").HasMaxLength(30).IsRequired();
            //builder.Property(x => x.DS_DIA_TURMA).HasColumnName("DS_DIA_TURMA").HasMaxLength(8);
            builder.Property(x => x.PA_PRO_ID).HasColumnName("PA_PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.PA_PRO_DESCRICAO).HasColumnName("PA_PRO_DESCRICAO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.PA_UNI_ID).HasColumnName("PA_UNI_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.ORD_PRECO_UNITARIO).HasColumnName("ORD_PRECO_UNITARIO");
            builder.Property(x => x.ORD_QUANTIDADE).HasColumnName("ORD_QUANTIDADE").IsRequired();
            builder.Property(x => x.ORD_DATA_ENTREGA_DE).HasColumnName("ORD_DATA_ENTREGA_DE").IsRequired();
            builder.Property(x => x.ORD_DATA_ENTREGA_ATE).HasColumnName("ORD_DATA_ENTREGA_ATE").IsRequired();
            builder.Property(x => x.ORD_TIPO).HasColumnName("ORD_TIPO");
            builder.Property(x => x.ORD_TOLERANCIA_MAIS).HasColumnName("ORD_TOLERANCIA_MAIS");
            builder.Property(x => x.ORD_TOLERANCIA_MENOS).HasColumnName("ORD_TOLERANCIA_MENOS");
            builder.Property(x => x.FPR_DATA_INICIO_PREVISTA).HasColumnName("FPR_DATA_INICIO_PREVISTA").IsRequired();
            builder.Property(x => x.FPR_DATA_FIM_PREVISTA).HasColumnName("FPR_DATA_FIM_PREVISTA").IsRequired();
            builder.Property(x => x.FPR_DATA_FIM_MAXIMA).HasColumnName("FPR_DATA_FIM_MAXIMA").IsRequired();
            builder.Property(x => x.FPR_QUANTIDADE_PREVISTA).HasColumnName("FPR_QUANTIDADE_PREVISTA").IsRequired();
            //builder.Property(x => x.FPR_OBS_PRODUCAO).HasColumnName("FPR_OBS_PRODUCAO").HasMaxLength(4000);
            builder.Property(x => x.FPR_STATUS).HasColumnName("FPR_STATUS").HasMaxLength(2);
            builder.Property(x => x.FPR_TEMPO_DECORRIDO_SETUP).HasColumnName("FPR_TEMPO_DECORRIDO_SETUP");
            builder.Property(x => x.FPR_TEMPO_DECORRIDO_SETUPA).HasColumnName("FPR_TEMPO_DECORRIDO_SETUPA");
            builder.Property(x => x.FPR_TEMPO_DECORRIDO_PERFORMANC).HasColumnName("FPR_TEMPO_DECORRIDO_PERFORMANC");
            builder.Property(x => x.FPR_QTD_PERFORMANCE).HasColumnName("FPR_QTD_PERFORMANCE");
            builder.Property(x => x.FPR_QTD_SETUP).HasColumnName("FPR_QTD_SETUP");
            builder.Property(x => x.FPR_QTD_PRODUZIDA).HasColumnName("FPR_QTD_PRODUZIDA");
            builder.Property(x => x.FPR_TEMPO_TEORICO_PERFORMANCE).HasColumnName("FPR_TEMPO_TEORICO_PERFORMANCE");
            builder.Property(x => x.FPR_TEMPO_RESTANTE_PERFORMANC).HasColumnName("FPR_TEMPO_RESTANTE_PERFORMANC");
            builder.Property(x => x.FPR_VELOCIDADE_P_ATINGIR_META).HasColumnName("FPR_VELOCIDADE_P_ATINGIR_META");
            builder.Property(x => x.FPR_QTD_RESTANTE).HasColumnName("FPR_QTD_RESTANTE");
            builder.Property(x => x.FPR_VELO_ATU_PC_SEGUNDO).HasColumnName("FPR_VELO_ATU_PC_SEGUNDO");
            builder.Property(x => x.FPR_TEMPO_DECO_PEQUENA_PARADA).HasColumnName("FPR_TEMPO_DECO_PEQUENA_PARADA");
            builder.Property(x => x.FPR_PERFORMANCE_PROJETADA).HasColumnName("FPR_PERFORMANCE_PROJETADA");
            builder.Property(x => x.STATUS_FILA).HasColumnName("STATUS_FILA").HasMaxLength(30);
            builder.Property(x => x.ROT_PECAS_POR_PULSO).HasColumnName("ROT_PECAS_POR_PULSO");
            builder.Property(x => x.TAR_PROXIMA_META_PERFORMANCE).HasColumnName("TAR_PROXIMA_META_PERFORMANCE");
            builder.Property(x => x.PERCENTUAL_PERFORMANCE).HasColumnName("PERCENTUAL_PERFORMANCE");
            builder.Property(x => x.STATUS_COR_PERFORMANCE).HasColumnName("STATUS_COR_PERFORMANCE").HasMaxLength(8);
            builder.Property(x => x.TAR_PROXIMA_META_TEMPO_SETUP).HasColumnName("TAR_PROXIMA_META_TEMPO_SETUP");
            builder.Property(x => x.PERCENTUAL_SETUP).HasColumnName("PERCENTUAL_SETUP");
            builder.Property(x => x.STATUS_COR_SETUP).HasColumnName("STATUS_COR_SETUP").HasMaxLength(8);
            builder.Property(x => x.TAR_PROXIMA_META_TEMPO_SETUP_AJUSTE).HasColumnName("TAR_PROXIMA_META_TEMPO_SETUP_AJUSTE");
            builder.Property(x => x.PERCENTUAL_SETUP_AJUSTE).HasColumnName("PERCENTUAL_SETUP_AJUSTE");
            builder.Property(x => x.STATUS_COR_SETUP_AJUSTE).HasColumnName("STATUS_COR_SETUP_AJUSTE").HasMaxLength(8);
            builder.Property(x => x.PERCENTUAL_SETUP_GERAL).HasColumnName("PERCENTUAL_SETUP_GERAL");
            builder.Property(x => x.STATUS_COR_SETUP_GERAL).HasColumnName("STATUS_COR_SETUP_GERAL").HasMaxLength(8);
            builder.Property(x => x.MAQ_ULTIMA_ATUALIZACAO).HasColumnName("MAQ_ULTIMA_ATUALIZACAO");
            builder.Property(x => x.PERCENTUAL_CONCLUIDO_SETUP).HasColumnName("PERCENTUAL_CONCLUIDO_SETUP");
            builder.Property(x => x.PERCENTUAL_CONCLUIDO_SETUP_AJUSTE).HasColumnName("PERCENTUAL_CONCLUIDO_SETUP_AJUSTE");
            builder.Property(x => x.PERCENTUAL_CONCLUIDO_PERFORMANCE).HasColumnName("PERCENTUAL_CONCLUIDO_PERFORMANCE");
            builder.Property(x => x.VELO_ATU_PC_SEGUNDO_ROUD_1).HasColumnName("VELO_ATU_PC_SEGUNDO_ROUD_1");
            builder.Property(x => x.TAR_PERCENTUAL_REALIZADO_PERFORMANCE).HasColumnName("TAR_PERCENTUAL_REALIZADO_PERFORMANCE");
            builder.Property(x => x.OCO_ID_PERFORMANCE).HasColumnName("OCO_ID_PERFORMANCE").HasMaxLength(30);
            builder.Property(x => x.TAR_OBS_PERFORMANCE).HasColumnName("TAR_OBS_PERFORMANCE").HasMaxLength(200);
            builder.Property(x => x.OCO_ID_SETUP).HasColumnName("OCO_ID_SETUP").HasMaxLength(30);
            builder.Property(x => x.TAR_OBS_SETUP).HasColumnName("TAR_OBS_SETUP").HasMaxLength(200);
            builder.Property(x => x.OCO_ID_SETUPA).HasColumnName("OCO_ID_SETUPA").HasMaxLength(30);
            builder.Property(x => x.TAR_OBS_SETUPA).HasColumnName("TAR_OBS_SETUPA").HasMaxLength(200);
            builder.Property(x => x.TAR_TIPO_FEEDBACK_PERFORMANCE).HasColumnName("TAR_TIPO_FEEDBACK_PERFORMANCE").HasMaxLength(1);
            builder.Property(x => x.TAR_TIPO_FEEDBACK_SETUP).HasColumnName("TAR_TIPO_FEEDBACK_SETUP").HasMaxLength(1);
            builder.Property(x => x.TAR_TIPO_FEEDBACK_SETUP_AJUSTE).HasColumnName("TAR_TIPO_FEEDBACK_SETUP_AJUSTE").HasMaxLength(1);
            builder.Property(x => x.TAR_QTD_SETUP_AJUSTE).HasColumnName("TAR_QTD_SETUP_AJUSTE");
            builder.Property(x => x.TAR_QTD).HasColumnName("TAR_QTD");
            builder.Property(x => x.TAR_PARAMETRO_TIME_WORK_STOP_MACHINE).HasColumnName("TAR_PARAMETRO_TIME_WORK_STOP_MACHINE");
            builder.Property(x => x.TAR_PARAMETRO_TEMPO_QUEBRA_DE_LOTE).HasColumnName("TAR_PARAMETRO_TEMPO_QUEBRA_DE_LOTE");
            builder.Property(x => x.TAR_PERFORMANCE_MAX_VERDE).HasColumnName("TAR_PERFORMANCE_MAX_VERDE");
            builder.Property(x => x.TAR_PERFORMANCE_MIN_VERDE).HasColumnName("TAR_PERFORMANCE_MIN_VERDE");
            builder.Property(x => x.TAR_SETUP_MAX_VERDE).HasColumnName("TAR_SETUP_MAX_VERDE");
            builder.Property(x => x.TAR_SETUP_MIN_VERDE).HasColumnName("TAR_SETUP_MIN_VERDE");
            builder.Property(x => x.TAR_SETUPA_MAX_VERDE).HasColumnName("TAR_SETUPA_MAX_VERDE");
            builder.Property(x => x.TAR_SETUPA_MIN_VERDE).HasColumnName("TAR_SETUPA_MIN_VERDE");
            builder.Property(x => x.TAR_PERFORMANCE_MIN_AMARELO).HasColumnName("TAR_PERFORMANCE_MIN_AMARELO");
            builder.Property(x => x.TAR_SETUP_MAX_AMARELO).HasColumnName("TAR_SETUP_MAX_AMARELO");
            builder.Property(x => x.TAR_SETUPA_MAX_AMARELO).HasColumnName("TAR_SETUPA_MAX_AMARELO");
            builder.Property(x => x.TAR_ID).HasColumnName("TAR_ID").IsRequired();
            builder.Property(x => x.SETUP_GERAL_AZUL).HasColumnName("SETUP_GERAL_AZUL").IsRequired();
            builder.Property(x => x.SETUP_GERAL_VERDE).HasColumnName("SETUP_GERAL_VERDE").IsRequired();
            builder.Property(x => x.SETUP_GERAL_AMARELO).HasColumnName("SETUP_GERAL_AMARELO").IsRequired();
            builder.Property(x => x.SETUP_GERAL_VERMELHO).HasColumnName("SETUP_GERAL_VERMELHO").IsRequired();
            builder.Property(x => x.PERFORMANCE_AZUL).HasColumnName("PERFORMANCE_AZUL").IsRequired();
            builder.Property(x => x.PERFORMANCE_VERDE).HasColumnName("PERFORMANCE_VERDE").IsRequired();
            builder.Property(x => x.PERFORMANCE_AMARELO).HasColumnName("PERFORMANCE_AMARELO").IsRequired();
            builder.Property(x => x.PERFORMANCE_VERMELHO).HasColumnName("PERFORMANCE_VERMELHO").IsRequired();
            builder.Property(x => x.TEMPO_PARADAS_NAO_PROGRAMADAS).HasColumnName("TEMPO_PARADAS_NAO_PROGRAMADAS").HasMaxLength(30);
            builder.Property(x => x.QTD_PARADAS_NAO_PROGRAMADAS).HasColumnName("QTD_PARADAS_NAO_PROGRAMADAS").IsRequired();
            builder.Property(x => x.TEMPO_PARADAS_NAO_PROGRAMADAS_EXETO_SETUP).HasColumnName("TEMPO_PARADAS_NAO_PROGRAMADAS_EXETO_SETUP").HasMaxLength(30);
            builder.Property(x => x.QTD_PARADAS_NAO_PROGRAMADAS_EXETO_SETUP).HasColumnName("QTD_PARADAS_NAO_PROGRAMADAS_EXETO_SETUP").IsRequired();
            builder.Property(x => x.TEMPO_PEQUENAS_PARADAS).HasColumnName("TEMPO_PEQUENAS_PARADAS").HasMaxLength(30);
            builder.Property(x => x.QTD_PEQUENAS_PARADAS).HasColumnName("QTD_PEQUENAS_PARADAS").IsRequired();
        }
    }
}
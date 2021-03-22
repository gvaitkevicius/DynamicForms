using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class TargetProdutoMap : IEntityTypeConfiguration<TargetProduto>
    {
        public void Configure(EntityTypeBuilder<TargetProduto> builder)
        {
            builder.ToTable("T_TARGET_PRODUTO");
            builder.HasKey(x => x.TAR_ID);
            builder.Property(x => x.TAR_ID).HasColumnName("TAR_ID").IsRequired();
            builder.Property(x => x.MOV_ID).HasColumnName("MOV_ID");
            builder.Property(x => x.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(60);
            builder.Property(x => x.PRO_ID).HasColumnName("PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.MAQ_ID).HasColumnName("MAQ_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.UNI_ID).HasColumnName("UNI_ID").HasMaxLength(30);
            builder.Property(x => x.TURM_ID).HasColumnName("TURM_ID").HasMaxLength(10);
            builder.Property(x => x.TURN_ID).HasColumnName("TURN_ID").HasMaxLength(10);
            builder.Property(x => x.USE_ID).HasColumnName("USE_ID");
            builder.Property(x => x.TAR_DIA_TURMA).HasColumnName("TAR_DIA_TURMA").HasMaxLength(8).IsRequired();
            builder.Property(x => x.TAR_META_PERFORMANCE).HasColumnName("TAR_META_PERFORMANCE").IsRequired();
            builder.Property(x => x.TAR_REALIZADO_PERFORMANCE).HasColumnName("TAR_REALIZADO_PERFORMANCE");
            builder.Property(x => x.TAR_PERCENTUAL_REALIZADO_PERFORMANCE).HasColumnName("TAR_PERCENTUAL_REALIZADO_PERFORMANCE");
            builder.Property(x => x.TAR_PROXIMA_META_PERFORMANCE).HasColumnName("TAR_PROXIMA_META_PERFORMANCE");
            builder.Property(x => x.TAR_META_TEMPO_SETUP).HasColumnName("TAR_META_TEMPO_SETUP").IsRequired();
            builder.Property(x => x.TAR_REALIZADO_TEMPO_SETUP).HasColumnName("TAR_REALIZADO_TEMPO_SETUP");
            builder.Property(x => x.TAR_PROXIMA_META_TEMPO_SETUP).HasColumnName("TAR_PROXIMA_META_TEMPO_SETUP");
            builder.Property(x => x.TAR_META_TEMPO_SETUP_AJUSTE).HasColumnName("TAR_META_TEMPO_SETUP_AJUSTE").IsRequired();
            builder.Property(x => x.TAR_REALIZADO_TEMPO_SETUP_AJUSTE).HasColumnName("TAR_REALIZADO_TEMPO_SETUP_AJUSTE");
            builder.Property(x => x.TAR_PROXIMA_META_TEMPO_SETUP_AJUSTE).HasColumnName("TAR_PROXIMA_META_TEMPO_SETUP_AJUSTE");
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
            builder.Property(x => x.ROT_SEQ_TRANFORMACAO).HasColumnName("ROT_SEQ_TRANFORMACAO");
            builder.Property(x => x.FPR_SEQ_REPETICAO).HasColumnName("FPR_SEQ_REPETICAO");
            builder.Property(x => x.TAR_PERFORMANCE_MAX_VERDE).HasColumnName("TAR_PERFORMANCE_MAX_VERDE");
            builder.Property(x => x.TAR_PERFORMANCE_MIN_VERDE).HasColumnName("TAR_PERFORMANCE_MIN_VERDE");
            builder.Property(x => x.TAR_SETUP_MAX_VERDE).HasColumnName("TAR_SETUP_MAX_VERDE");
            builder.Property(x => x.TAR_SETUP_MIN_VERDE).HasColumnName("TAR_SETUP_MIN_VERDE");
            builder.Property(x => x.TAR_SETUPA_MAX_VERDE).HasColumnName("TAR_SETUPA_MAX_VERDE");
            builder.Property(x => x.TAR_SETUPA_MIN_VERDE).HasColumnName("TAR_SETUPA_MIN_VERDE");
            builder.Property(x => x.TAR_PERFORMANCE_MIN_AMARELO).HasColumnName("TAR_PERFORMANCE_MIN_AMARELO");
            builder.Property(x => x.TAR_SETUP_MAX_AMARELO).HasColumnName("TAR_SETUP_MAX_AMARELO");
            builder.Property(x => x.TAR_SETUPA_MAX_AMARELO).HasColumnName("TAR_SETUPA_MAX_AMARELO");
            builder.Property(x => x.TAR_OBS_OP_PARCIAL).HasColumnName("TAR_OBS_OP_PARCIAL").HasMaxLength(200);
            builder.Property(x => x.TAR_OCO_ID_OP_PARCIAL).HasColumnName("TAR_OCO_ID_OP_PARCIAL").HasMaxLength(30);
            builder.Property(x => x.TAR_COR_PERFORMANCE).HasColumnName("TAR_COR_PERFORMANCE").HasMaxLength(10);
            builder.Property(x => x.TAR_COR_SETUP_GERAL).HasColumnName("TAR_COR_SETUP_GERAL").HasMaxLength(10);
            builder.Property(x => x.TAR_COR_SETUP).HasColumnName("TAR_COR_SETUP").HasMaxLength(10);
            builder.Property(x => x.TAR_COR_SETUPA).HasColumnName("TAR_COR_SETUPA").HasMaxLength(10);
            builder.Property(x => x.TAR_DIA_TURMA_D).HasColumnName("TAR_DIA_TURMA_D");
            builder.Property(x => x.FEE_QTD_PECAS_POR_PULSO).HasColumnName("FEE_QTD_PECAS_POR_PULSO");
            builder.Property(x => x.TAR_QTD_PERDAS).HasColumnName("TAR_QTD_PERDAS");
            builder.Property(x => x.TAR_DATA_INICIAL).HasColumnName("TAR_DATA_INICIAL");
            builder.Property(x => x.TAR_DATA_FINAL).HasColumnName("TAR_DATA_FINAL");
            builder.Property(x => x.TAR_APROVADO).HasColumnName("TAR_APROVADO").HasMaxLength(2);

            builder.HasOne(x => x.MovimentoEstoque).WithMany(me => me.TargetsProduto).HasForeignKey(x => x.MOV_ID);
            builder.HasOne(x => x.Order).WithMany(o => o.TargetsProduto).HasForeignKey(x => x.ORD_ID);
            builder.HasOne(x => x.Produto).WithMany(p => p.TargetsProduto).HasForeignKey(x => x.PRO_ID);
            builder.HasOne(x => x.Maquina).WithMany(m => m.TargetsProduto).HasForeignKey(x => x.MAQ_ID);
            builder.HasOne(x => x.UnidadeMedida).WithMany(um => um.TargetsProduto).HasForeignKey(x => x.UNI_ID);
            builder.HasOne(x => x.Turma).WithMany(t => t.TargetsProduto).HasForeignKey(x => x.TURM_ID);
            builder.HasOne(x => x.Turno).WithMany(t => t.TargetsProduto).HasForeignKey(x => x.TURN_ID);
            builder.HasOne(x => x.Usuario).WithMany(t => t.TargetsProduto).HasForeignKey(x => x.USE_ID);
            builder.HasOne(x => x.OcorrenciaPerformace).WithMany(oc => oc.TarProdOcoPers).HasForeignKey(x => x.OCO_ID_PERFORMANCE);
            builder.HasOne(x => x.OcorrenciaSetup).WithMany(oc => oc.TarProdOcoSetups).HasForeignKey(x => x.OCO_ID_SETUP);
            builder.HasOne(x => x.OcorrenciaSetupAjuste).WithMany(oc => oc.TarProdOcoSetupAs).HasForeignKey(x => x.OCO_ID_SETUPA);
        }
    }
}
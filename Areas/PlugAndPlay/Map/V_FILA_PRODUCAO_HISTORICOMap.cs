using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class V_FILA_PRODUCAO_HISTORICOMap : IEntityTypeConfiguration<V_FILA_PRODUCAO_HISTORICO>
    {
        public void Configure(EntityTypeBuilder<V_FILA_PRODUCAO_HISTORICO> builder)
        {
            builder.ToTable("V_FILA_PRODUCAO_HISTORICO");
            builder.HasKey(x => new { x.ROT_PRO_ID, x.ROT_MAQ_ID, x.ORD_ID, x.FPR_SEQ_REPETICAO, x.ROT_SEQ_TRANFORMACAO });
            builder.Property(x => x.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(60).IsRequired();
            builder.Property(x => x.ROT_MAQ_ID).HasColumnName("ROT_MAQ_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.FPR_QTD_PRODUZIDA).HasColumnName("FPR_QTD_PRODUZIDA");
            builder.Property(x => x.ROT_PRO_ID).HasColumnName("ROT_PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.PRO_DESCRICAO).HasColumnName("PRO_DESCRICAO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.FPR_DATA_INICIO_PREVISTA).HasColumnName("FPR_DATA_INICIO_PREVISTA").IsRequired();
            builder.Property(x => x.FPR_DATA_FIM_PREVISTA).HasColumnName("FPR_DATA_FIM_PREVISTA").IsRequired();
            builder.Property(x => x.FPR_DATA_FIM_MAXIMA).HasColumnName("FPR_DATA_FIM_MAXIMA").IsRequired();
            builder.Property(x => x.ROT_SEQ_TRANFORMACAO).HasColumnName("ROT_SEQ_TRANFORMACAO").IsRequired();
            builder.Property(x => x.FPR_SEQ_REPETICAO).HasColumnName("FPR_SEQ_REPETICAO").IsRequired();
            builder.Property(x => x.FPR_OBS_PRODUCAO).HasColumnName("FPR_OBS_PRODUCAO").HasMaxLength(4000);
            builder.Property(x => x.FPR_QUANTIDADE_PREVISTA).HasColumnName("FPR_QUANTIDADE_PREVISTA").IsRequired();
            builder.Property(x => x.TIP_ID).HasColumnName("TIP_ID").HasMaxLength(3).IsRequired();
            builder.Property(x => x.MOV_QUANTIDADE).HasColumnName("MOV_QUANTIDADE").IsRequired();
            builder.Property(x => x.QTD_PERDA).HasColumnName("QTD_PERDA").IsRequired();
            builder.Property(x => x.MOV_ID).HasColumnName("MOV_ID").IsRequired();
            builder.Property(x => x.MOV_DATA_HORA_CRIACAO).HasColumnName("MOV_DATA_HORA_CRIACAO");
            builder.Property(x => x.USE_NOME).HasColumnName("USE_NOME").HasMaxLength(80);
            builder.Property(x => x.TURN_ID).HasColumnName("TURN_ID").HasMaxLength(10);
            builder.Property(x => x.EQU_ID).HasColumnName("EQU_ID").HasMaxLength(30);
        }
    }
}

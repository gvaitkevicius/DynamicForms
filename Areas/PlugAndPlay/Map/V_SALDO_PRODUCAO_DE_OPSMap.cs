using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map
{
    public class V_SALDO_PRODUCAO_DE_OPSMap : IEntityTypeConfiguration<V_SALDO_PRODUCAO_DE_OPS>
    {
        public void Configure(EntityTypeBuilder<V_SALDO_PRODUCAO_DE_OPS> builder)
        {
            builder.HasKey(x => new { x.ORD_ID, x.ROT_SEQ_TRANFORMACAO, x.FPR_SEQ_REPETICAO, x.ROT_PRO_ID });

            builder.Property(x => x.ENCERRADO_NORMAL).HasColumnName("ENCERRADO_NORMAL").HasMaxLength(1).IsRequired();
            builder.Property(x => x.RESIDUO_ELIMINADO).HasColumnName("RESIDUO_ELIMINADO").HasMaxLength(1).IsRequired();
            builder.Property(x => x.FASE).HasColumnName("FASE").HasMaxLength(15).IsRequired();
            builder.Property(x => x.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(60).IsRequired();
            builder.Property(x => x.PRO_ID).HasColumnName("PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.PRO_DESCRICAO).HasColumnName("PRO_DESCRICAO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.ROT_PRO_ID).HasColumnName("ROT_PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.ROT_SEQ_TRANFORMACAO).HasColumnName("ROT_SEQ_TRANFORMACAO").IsRequired();
            builder.Property(x => x.FPR_DATA_INICIO_PREVISTA).HasColumnName("FPR_DATA_INICIO_PREVISTA");
            builder.Property(x => x.FPR_DATA_FIM_PREVISTA).HasColumnName("FPR_DATA_FIM_PREVISTA");
            builder.Property(x => x.FPR_SEQ_REPETICAO).HasColumnName("FPR_SEQ_REPETICAO");
            builder.Property(x => x.ORD_QUANTIDADE).HasColumnName("ORD_QUANTIDADE").IsRequired();
            builder.Property(x => x.ORD_TOLERANCIA_MENOS).HasColumnName("ORD_TOLERANCIA_MENOS");
            builder.Property(x => x.FPR_PREVISAO_MATERIA_PRIMA).HasColumnName("FPR_PREVISAO_MATERIA_PRIMA");
            builder.Property(x => x.ORD_OP_INTEGRACAO).HasColumnName("ORD_OP_INTEGRACAO").HasMaxLength(100);
            builder.Property(x => x.CLI_NOME).HasColumnName("CLI_NOME").HasMaxLength(100).IsRequired();
            builder.Property(x => x.SALDO_A_PRODUZIR).HasColumnName("SALDO_A_PRODUZIR");
            builder.Property(x => x.DATA_PRODUCAO).HasColumnName("DATA_PRODUCAO").IsRequired();
            builder.Property(x => x.QTD_PECAS_BOAS).HasColumnName("QTD_PECAS_BOAS");
            builder.Property(x => x.QTD_PERDAS).HasColumnName("QTD_PERDAS");
            builder.Property(x => x.MAQ_ID).HasColumnName("MAQ_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.MAQ_DESCRICAO).HasColumnName("MAQ_DESCRICAO").HasMaxLength(100).IsRequired();
        }
    }
}

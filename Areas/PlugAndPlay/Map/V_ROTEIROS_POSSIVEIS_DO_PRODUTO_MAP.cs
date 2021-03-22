using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class V_ROTEIROS_POSSIVEIS_DO_PRODUTO_MAP : IEntityTypeConfiguration<V_ROTEIROS_POSSIVEIS_DO_PRODUTO>
    {
        public void Configure(EntityTypeBuilder<V_ROTEIROS_POSSIVEIS_DO_PRODUTO> builder)
        {
            builder.ToTable("V_ROTEIROS_POSSIVEIS_DO_PRODUTO");
            builder.HasKey(x => new { x.MAQ_ID, x.PRO_ID, x.ROT_SEQ_TRANFORMACAO });
            builder.Property(x => x.STATUS_CADASTRO).HasColumnName("STATUS_CADASTRO").HasMaxLength(8).IsRequired();
            builder.Property(x => x.MAQ_TIPO_PLANEJAMENTO).HasColumnName("MAQ_TIPO_PLANEJAMENTO").HasMaxLength(60);
            builder.Property(x => x.AVALIA_CUSTO).HasColumnName("AVALIA_CUSTO");
            builder.Property(x => x.PERCENTUAL_INICIO_PASSO_ANTERIOR).HasColumnName("PERCENTUAL_INICIO_PASSO_ANTERIOR");
            builder.Property(x => x.CAL_ID).HasColumnName("CAL_ID").IsRequired();
            builder.Property(x => x.PRO_ID).HasColumnName("PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.MAQ_ID).HasColumnName("MAQ_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.ROT_SEQ_TRANFORMACAO).HasColumnName("ROT_SEQ_TRANFORMACAO").IsRequired();
            builder.Property(x => x.HIERARQUIA_SEQ_TRANSFORMACAO).HasColumnName("HIERARQUIA_SEQ_TRANSFORMACAO");
            builder.Property(x => x.ROT_VA_PARA_SEQ_TRANSFORMACAO).HasColumnName("ROT_VA_PARA_SEQ_TRANSFORMACAO");
            builder.Property(x => x.ROT_PERFORMANCE).HasColumnName("ROT_PERFORMANCE");
            builder.Property(x => x.ROT_TEMPO_SETUP).HasColumnName("ROT_TEMPO_SETUP");
            builder.Property(x => x.ROT_TEMPO_SETUP_AJUSTE).HasColumnName("ROT_TEMPO_SETUP_AJUSTE");
            builder.Property(x => x.ROT_PECAS_POR_PULSO).HasColumnName("ROT_PECAS_POR_PULSO");
            builder.Property(x => x.ROT_PRIORIDADE_INFORMADA).HasColumnName("ROT_PRIORIDADE_INFORMADA");
            builder.Property(x => x.ROT_STATUS).HasColumnName("ROT_STATUS").HasMaxLength(2);
            builder.Property(x => x.ROT_OPERACOES).HasColumnName("ROT_OPERACOES").HasMaxLength(100).IsRequired();
            builder.Property(x => x.ROT_EXCECAO_OPERACOES).HasColumnName("ROT_EXCECAO_OPERACOES").HasMaxLength(100).IsRequired();
            builder.Property(x => x.ROT_LINHA_DIRETA).HasColumnName("ROT_LINHA_DIRETA").HasMaxLength(1).IsRequired();
            builder.Property(x => x.MAQ_LARGURA_UTIL).HasColumnName("MAQ_LARGURA_UTIL");
            builder.Property(x => x.GRP_PERFORMANCE_METRO_LINEAR_POR_SEGUNDO).HasColumnName("GRP_PERFORMANCE_METRO_LINEAR_POR_SEGUNDO").IsRequired();
            builder.Property(x => x.GRP_TIPO).HasColumnName("GRP_TIPO");


            builder.HasOne(x => x.Maquina).WithMany(x => x.V_ROTEIROS_POSSIVEIS_DO_PRODUTO).HasForeignKey(x => x.MAQ_ID);
        }
    }
}

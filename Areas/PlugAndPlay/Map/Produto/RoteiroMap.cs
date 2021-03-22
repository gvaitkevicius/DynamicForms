using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class RoteiroMap : IEntityTypeConfiguration<Roteiro>
    {


        public void Configure(EntityTypeBuilder<Roteiro> builder)
        {
            builder.ToTable("T_ROTEIROS");
            builder.HasKey(x => new { x.MAQ_ID, x.PRO_ID, x.ROT_SEQ_TRANFORMACAO });
            builder.Property(x => x.GMA_ID).HasColumnName("GMA_ID").HasMaxLength(30);
            builder.Property(x => x.MAQ_ID).HasColumnName("MAQ_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.PRO_ID).HasColumnName("PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.ROT_SEQ_TRANFORMACAO).HasColumnName("ROT_SEQ_TRANFORMACAO").IsRequired();
            builder.Property(x => x.ROT_PECAS_POR_PULSO).HasColumnName("ROT_PECAS_POR_PULSO");
            builder.Property(x => x.ROT_PRIORIDADE_INFORMADA).HasColumnName("ROT_PRIORIDADE_INFORMADA");
            builder.Property(x => x.ROT_ACAO).HasColumnName("ROT_ACAO").HasMaxLength(2);
            builder.Property(x => x.ROT_PERFORMANCE).HasColumnName("ROT_PERFORMANCE");
            builder.Property(x => x.ROT_TEMPO_SETUP).HasColumnName("ROT_TEMPO_SETUP");
            builder.Property(x => x.ROT_TEMPO_SETUP_AJUSTE).HasColumnName("ROT_TEMPO_SETUP_AJUSTE");
            builder.Property(x => x.ROT_VA_PARA_SEQ_TRANSFORMACAO).HasColumnName("ROT_VA_PARA_SEQ_TRANSFORMACAO");
            builder.Property(x => x.ROT_STATUS).HasColumnName("ROT_STATUS").HasMaxLength(2);
            builder.Property(x => x.ROT_HIERARQUIA_SEQ_TRANSFORMACAO).HasColumnName("ROT_HIERARQUIA_SEQ_TRANSFORMACAO");
            builder.Property(x => x.ROT_AVALIA_CUSTO).HasColumnName("ROT_AVALIA_CUSTO");
            builder.Property(x => x.ROT_OPERACOES).HasColumnName("ROT_OPERACOES").HasMaxLength(100);
            builder.Property(x => x.ROT_EXCECAO_OPERACOES).HasColumnName("ROT_EXCECAO_OPERACOES").HasMaxLength(100);
            builder.Property(x => x.ROT_PERCENTUAL_INICIO_PASSO_ANTERIOR).HasColumnName("ROT_PERCENTUAL_INICIO_PASSO_ANTERIOR");
            builder.Property(x => x.ROT_LINHA_DIRETA).HasColumnName("ROT_LINHA_DIRETA");
            builder.Property(x => x.TEM_ID).HasColumnName("TEM_ID");

            //forengn key
            builder.HasOne(x => x.TemplateDeTestes).WithMany(x => x.Roteiro).HasForeignKey(x => x.TEM_ID);
            builder.HasOne(x => x.Produto).WithMany(x => x.Roteiros).HasForeignKey(x => x.PRO_ID);
            builder.HasOne(x => x.Maquina).WithMany(x => x.Roteiros).HasForeignKey(x => x.MAQ_ID);
            builder.HasOne(x => x.GrupoMaquina).WithMany(x => x.Roteiros).HasForeignKey(x => x.GMA_ID);
        }
    }
}
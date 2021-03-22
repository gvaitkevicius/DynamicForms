using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map.Qualidade
{
    public class TesteFisicoMap : IEntityTypeConfiguration<TesteFisico>
    {
        public void Configure(EntityTypeBuilder<TesteFisico> builder)
        {
            builder.ToTable("T_TESTE_FISICO");
            builder.HasKey(x => x.TES_ID);
            builder.Property(x => x.TES_ID).HasColumnName("TES_ID").IsRequired();
            builder.Property(x => x.USE_ID).HasColumnName("USE_ID");
            builder.Property(x => x.TES_NOME_TECNICO).HasColumnName("TES_NOME_TECNICO").HasMaxLength(200);
            builder.Property(x => x.TES_VALOR_NUMERICO).HasColumnName("TES_VALOR_NUMERICO");
            builder.Property(x => x.TES_VALOR_DATA).HasColumnName("TES_VALOR_DATA");
            builder.Property(x => x.TES_VALOR_TEXTO).HasColumnName("TES_VALOR_TEXTO").HasMaxLength(200);
            builder.Property(x => x.TES_EMISSAO).HasColumnName("TES_EMISSAO");
            builder.Property(x => x.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(60);
            builder.Property(x => x.FPR_SEQ_REPETICAO).HasColumnName("FPR_SEQ_REPETICAO");
            builder.Property(x => x.TES_STATUS_LIBERACAO).HasColumnName("TES_STATUS_LIBERACAO").HasMaxLength(30);
            builder.Property(x => x.ROT_SEQ_TRANSFORMACAO).HasColumnName("ROT_SEQ_TRANSFORMACAO");
            builder.Property(x => x.ROT_PRO_ID).HasColumnName("ROT_PRO_ID").HasMaxLength(30);
            builder.Property(x => x.ROT_MAQ_ID).HasColumnName("ROT_MAQ_ID").HasMaxLength(30);
            builder.Property(x => x.TT_ID).HasColumnName("TT_ID");

            builder.Property(x => x.TURN_ID).HasColumnName("TURN_ID").HasMaxLength(10);
            builder.Property(x => x.TES_OBS).HasColumnName("TES_OBS").HasMaxLength(100);
            builder.Property(x => x.TURM_ID).HasColumnName("TURM_ID").HasMaxLength(10);
            builder.Property(x => x.RL_ID).HasColumnName("RL_ID");
            builder.Property(x => x.TA_ID).HasColumnName("TA_ID");
            builder.Property(x => x.DATA_ULTIMA_ALTERACAO).HasColumnName("DATA_ULTIMA_ALTERACAO");
            //--
            builder.HasOne(x => x.Usuario).WithMany(u => u.TesteFisico).HasForeignKey(x => x.USE_ID);
            builder.HasOne(x => x.TipoTeste).WithMany(u => u.TesteFisico).HasForeignKey(x => x.TT_ID);
            builder.HasOne(x => x.Turno).WithMany(u => u.TesteFisico).HasForeignKey(x => x.TURN_ID);
            builder.HasOne(x => x.Turma).WithMany(u => u.TesteFisico).HasForeignKey(x => x.TURM_ID);
            builder.HasOne(x => x.ResultLote).WithMany(u => u.TesteFisico).HasForeignKey(x => x.RL_ID);
            builder.HasOne(x => x.TipoAvaliacao).WithMany(u => u.TesteFisico).HasForeignKey(x => x.TA_ID);
        }
    }
}

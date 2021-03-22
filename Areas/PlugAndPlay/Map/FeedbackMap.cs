using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class FeedbackMap : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.ToTable("T_FEEDBACK");
            builder.HasKey(f => f.Id);
            //colunas comuns
            builder.Property(f => f.Id).HasColumnName("FEE_ID").IsRequired();
            builder.Property(f => f.DataInicial).HasColumnName("FEE_DATA_INICIAL").IsRequired();
            builder.Property(f => f.Datafinal).HasColumnName("FEE_DATA_FINAL").IsRequired();
            builder.Property(f => f.Observacoes).HasColumnName("FEE_OBSERVACOES").HasMaxLength(100);
            builder.Property(f => f.Grupo).HasColumnName("FEE_GRUPO").IsRequired();
            builder.Property(f => f.DiaTurma).HasColumnName("FEE_DIA_TURMA");
            builder.Property(f => f.SequenciaTransformacao).HasColumnName("ROT_SEQ_TRANFORMACAO");
            builder.Property(f => f.SequenciaRepeticao).HasColumnName("FPR_SEQ_REPETICAO");
            builder.Property(f => f.QuantidadePulsos).HasColumnName("FEE_QTD_PULSOS");
            builder.Property(f => f.QuantidadePecasPorPulso).HasColumnName("FEE_QTD_PECAS_POR_PULSO");
            //chaves estrangeiras
            builder.Property(f => f.MaquinaId).HasColumnName("MAQ_ID").HasMaxLength(30).IsRequired();
            builder.Property(f => f.TurnoId).HasColumnName("TURN_ID").HasMaxLength(10).IsRequired();
            builder.Property(f => f.TurmaId).HasColumnName("TURM_ID").HasMaxLength(10).IsRequired();
            builder.Property(f => f.OrderId).HasColumnName("ORD_ID").HasMaxLength(30);
            builder.Property(f => f.OcorrenciaId).HasColumnName("OCO_ID").HasMaxLength(10);
            builder.Property(f => f.UsuarioId).HasColumnName("USE_ID").IsRequired();
            builder.Property(f => f.ProdutoId).HasColumnName("PRO_ID");

            builder.HasOne(f => f.Maquina).WithMany(m => m.Feedbacks).HasForeignKey(f => f.MaquinaId);
            builder.HasOne(f => f.Turno).WithMany(t => t.Medicoes).HasForeignKey(f => f.TurnoId);
            builder.HasOne(f => f.Turma).WithMany(tur => tur.Feedbacks).HasForeignKey(f => f.TurmaId);
            builder.HasOne(f => f.Usuario).WithMany(u => u.T_Feedbacks).HasForeignKey(f => f.UsuarioId);
            builder.HasOne(f => f.Produto).WithMany(p => p.Feedbacks).HasForeignKey(f => f.ProdutoId);
            builder.HasOne(f => f.Order).WithMany(od => od.Medicoes).HasForeignKey(f => f.OrderId);
            builder.HasOne(f => f.Ocorrencia).WithMany(oc => oc.Medicoes).HasForeignKey(f => f.OcorrenciaId);
        }
    }
}
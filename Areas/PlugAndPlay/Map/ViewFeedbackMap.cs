using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class ViewFeedbackMap : IEntityTypeConfiguration<ViewFeedback>
    {
        public void Configure(EntityTypeBuilder<ViewFeedback> builder)
        {
            builder.ToTable("V_FEEDBACKS");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.DiaTurma).HasColumnName("DiaTurma").HasMaxLength(8);
            builder.Property(x => x.Id).HasColumnName("Id").IsRequired();
            builder.Property(x => x.QuantidadePulsos).HasColumnName("QuantidadePulsos").IsRequired();
            builder.Property(x => x.QuantidadePecasPorPulso).HasColumnName("QuantidadePecasPorPulso");
            builder.Property(x => x.DataInicial).HasColumnName("DataInicial").IsRequired();
            builder.Property(x => x.Datafinal).HasColumnName("Datafinal").IsRequired();
            builder.Property(x => x.MaquinaId).HasColumnName("MaquinaId").HasMaxLength(30).IsRequired();
            builder.Property(x => x.OcorrenciaId).HasColumnName("OcorrenciaId").HasMaxLength(30);
            builder.Property(x => x.TurnoId).HasColumnName("TurnoId").HasMaxLength(10);
            builder.Property(x => x.TurmaId).HasColumnName("TurmaId").HasMaxLength(10);
            builder.Property(x => x.UsuarioId).HasColumnName("UsuarioId").IsRequired();
            builder.Property(x => x.OrderId).HasColumnName("OrderId").HasMaxLength(60);
            builder.Property(x => x.SequenciaTransformacao).HasColumnName("SequenciaTransformacao");
            builder.Property(x => x.SequenciaRepeticao).HasColumnName("SequenciaRepeticao");
            builder.Property(x => x.Observacoes).HasColumnName("Observacoes").HasMaxLength(100);
            builder.Property(x => x.Grupo).HasColumnName("Grupo").IsRequired();
            builder.Property(x => x.ProdutoId).HasColumnName("ProdutoId").HasMaxLength(30).IsRequired();
            builder.Property(x => x.ProdutoDescricao).HasColumnName("ProdutoDescricao").HasMaxLength(100).IsRequired();
            builder.Property(x => x.FeeIdMovEstoque).HasColumnName("FeeIdMovEstoque");
        }
    }
}
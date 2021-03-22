using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class ViewClpMedicoesMap : IEntityTypeConfiguration<ViewClpMedicoes>
    {
        public void Configure(EntityTypeBuilder<ViewClpMedicoes> builder)
        {
            builder.ToTable("V_CLP_MEDICOES");
            builder.Property(x => x.MaquinaId).HasColumnName("MAQUINA_ID");
            builder.Property(x => x.DataIni).HasColumnName("DATA_INI");
            builder.Property(x => x.DataFim).HasColumnName("DATA_FIM");
            builder.Property(x => x.Quantidade).HasColumnName("QTD");
            builder.Property(x => x.Grupo).HasColumnName("GRUPO");
            builder.Property(x => x.TurnoId).HasColumnName("URN_ID");
            builder.Property(x => x.TurmaId).HasColumnName("URM_ID");
            builder.Property(x => x.FeedBackId).HasColumnName("FEE_ID");
            builder.Property(x => x.FeedbackObs).HasColumnName("FEE_OBSERVACOES");
            builder.Property(x => x.OcoId).HasColumnName("OCO_ID");
            builder.Property(x => x.FeedBackIdMov).HasColumnName("FEE_ID_MOV");
            builder.Property(x => x.Clp_Origem).HasColumnName("CLP_ORIGEM");
            builder.HasKey(x => new { x.MaquinaId, x.Grupo });
        }
    }
}
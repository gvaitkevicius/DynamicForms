using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class ClpMedicoesMap : IEntityTypeConfiguration<ClpMedicoes>
    {


        public void Configure(EntityTypeBuilder<ClpMedicoes> builder)
        {
            builder.ToTable("T_CLP_MEDICOES");
            builder.Property(x => x.Id).HasColumnName("ID");
            builder.Property(x => x.MaquinaId).HasColumnName("MAQUINA_ID").HasMaxLength(10);
            builder.Property(x => x.DataInicio).HasColumnName("DATA_INI");
            builder.Property(x => x.DataFim).HasColumnName("DATA_FIM").IsRequired();
            builder.Property(x => x.Quantidade).HasColumnName("QTD");
            builder.Property(x => x.Grupo).HasColumnName("GRUPO");
            builder.Property(x => x.Status).HasColumnName("STATUS");
            builder.Property(x => x.TurmaId).HasColumnName("URM_ID").HasMaxLength(1);
            builder.Property(x => x.TurnoId).HasColumnName("URN_ID").HasMaxLength(1);
            //builder.Property(x => x.OrdemProducaoId).HasColumnName("ORD_ID").HasMaxLength(30);
            builder.Property(x => x.OcorrenciaId).HasColumnName("OCO_ID").HasMaxLength(30);
            builder.Property(x => x.IdLoteClp).HasColumnName("ID_LOTE_CLP");
            builder.Property(x => x.Fase).HasColumnName("FASE");
            builder.Property(x => x.Emissao).HasColumnName("CLP_EMISSAO");
            //builder.Property(x => x.ProdutoId).HasColumnName("PRO_ID").HasMaxLength(30).IsOptional();
            //builder.Property(x => x.SequenciaTransformacaoId).HasColumnName("ROT_SEQ_TRANFORMACAO").IsOptional();
            //builder.Property(x => x.SequenciaRepeticaoId).HasColumnName("FPR_SEQ_REPETICAO").IsOptional();
            //builder.Property(x => x.FilaProducaoId).HasColumnName("FPR_ID").IsOptional();
            builder.Property(x => x.ClpOrigem).HasColumnName("CLP_ORIGEM").HasMaxLength(1);

            builder.HasKey(x => x.Id);
        }
    }
}
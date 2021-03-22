using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map.Qualidade
{
    public class ResultLoteMap : IEntityTypeConfiguration<ResultLote>
    {
        public void Configure(EntityTypeBuilder<ResultLote> builder)
        {
            builder.ToTable("T_RESULT_LOTE");
            builder.HasKey(x => x.RL_ID);
            builder.Property(x => x.RL_ID).HasColumnName("RL_ID").IsRequired();
            builder.Property(x => x.RL_QTD_DEF_GRAVE).HasColumnName("RL_QTD_DEF_GRAVE").IsRequired();
            builder.Property(x => x.RL_QTD_DEF_CRITICO).HasColumnName("RL_QTD_DEF_CRITICO").IsRequired();
            builder.Property(x => x.RL_STATUS).HasColumnName("RL_STATUS").HasMaxLength(200).IsRequired();
            builder.Property(x => x.RL_VALOR_ENCONTRADO).HasColumnName("RL_VALOR_ENCONTRADO").IsRequired();
            builder.Property(x => x.RL_DATA_LIBERACAO).HasColumnName("RL_DATA_LIBERACAO");
            builder.Property(x => x.RL_NOME_LIBERACAO).HasColumnName("RL_NOME_LIBERACAO").HasMaxLength(200);
            builder.Property(x => x.RL_OBS).HasColumnName("RL_OBS").HasMaxLength(255);
            builder.Property(x => x.RL_LIBERADO).HasColumnName("RL_LIBERADO").HasMaxLength(1);
        }
    }
}

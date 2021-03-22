using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map
{
    public class CorridasOnduladeiraMap : IEntityTypeConfiguration<CorridasOnduladeira>
    {
        public void Configure(EntityTypeBuilder<CorridasOnduladeira> builder)
        {
            builder.ToTable("V_CORRIDA_ONDULADEIRA");
            builder.HasKey(x => x.COR_ID);
            builder.Property(x => x.COR_ID).HasColumnName("COR_ID").IsRequired();
            builder.Property(x => x.MAQ_ID).HasColumnName("MAQ_ID").HasMaxLength(30);
            builder.Property(x => x.BOL_ID).HasColumnName("BOL_ID");
            builder.Property(x => x.COR_SEQUENCIA).HasColumnName("COR_SEQUENCIA");
            builder.Property(x => x.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(60);
            builder.Property(x => x.FPR_SEQ_REPETICAO).HasColumnName("FPR_SEQ_REPETICAO");
            builder.Property(x => x.COR_FACAO).HasColumnName("COR_FACAO");
            builder.Property(x => x.COR_FORMATO_BOBINA).HasColumnName("COR_FORMATO_BOBINA");
            builder.Property(x => x.COR_INICIO_PREVISTO).HasColumnName("COR_INICIO_PREVISTO");
            builder.Property(x => x.COR_FIM_PREVISTO).HasColumnName("COR_FIM_PREVISTO");
            builder.Property(x => x.PRO_ID).HasColumnName("PRO_ID").HasMaxLength(30);
            builder.Property(x => x.PRO_QTD_PACAS).HasColumnName("PRO_QTD_PACAS");
            builder.Property(x => x.COR_PECAS_LARGURA).HasColumnName("COR_PECAS_LARGURA");
            builder.Property(x => x.ROT_SEQ_TRANFORMACAO).HasColumnName("ROT_SEQ_TRANFORMACAO");
            builder.Property(x => x.ORD_COMPRIMENTO).HasColumnName("ORD_COMPRIMENTO");
            builder.Property(x => x.ORD_COR_FILA).HasColumnName("ORD_COR_FILA").HasMaxLength(30);
            builder.Property(x => x.FPR_COR_FILA).HasColumnName("FPR_COR_FILA").HasMaxLength(7).IsRequired();
        }
    }
}

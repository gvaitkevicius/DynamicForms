using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map.Qualidade
{
    public class FechamentoTesteMap : IEntityTypeConfiguration<FechamentoTeste>
    {
        public void Configure(EntityTypeBuilder<FechamentoTeste> builder)
        {
            builder.ToTable("T_FECHAMENTO_TESTE");
            builder.HasKey(x => x.FEC_ID);
            builder.Property(x => x.FEC_ID).HasColumnName("FEC_ID").IsRequired();
            builder.Property(x => x.FEC_QTD).HasColumnName("FEC_QTD");
            builder.Property(x => x.GRP_ID).HasColumnName("GRP_ID").HasMaxLength(30);

            builder.HasOne(x => x.GrupoProduto).WithMany(u => u.FechamentoTeste).HasForeignKey(x => x.GRP_ID);
        }
    }
}

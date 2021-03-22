using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map.Qualidade
{
    public class ViewControleQABobinasMap : IEntityTypeConfiguration<ViewControleQABobinas>
    {
        public void Configure(EntityTypeBuilder<ViewControleQABobinas> builder)
        {
            builder.ToTable("V_CONTROLE_QA_BOBINAS");
            builder.HasKey(x => x.ORD_ID);
            builder.Property(x => x.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(60).IsRequired();
            builder.Property(x => x.ORD_DATA_ENTREGA_DE).HasColumnName("ORD_DATA_ENTREGA_DE").IsRequired();
            builder.Property(x => x.ORD_DATA_ENTREGA_ATE).HasColumnName("ORD_DATA_ENTREGA_ATE").IsRequired();
            builder.Property(x => x.PRO_ID).HasColumnName("PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.PRO_DESCRICAO).HasColumnName("PRO_DESCRICAO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.CLI_ID).HasColumnName("CLI_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.CLI_NOME).HasColumnName("CLI_NOME").HasMaxLength(100).IsRequired();
            builder.Property(x => x.LTF_ID).HasColumnName("LTF_ID");
            builder.Property(x => x.LTF_DATA_ULTIMA_ALTERACAO).HasColumnName("LTF_DATA_ULTIMA_ALTERACAO");
            builder.Property(x => x.STATUS_LAUDO).HasColumnName("STATUS_LAUDO").HasMaxLength(60);
            builder.Property(x => x.STATUS_TESTAGEM).HasColumnName("STATUS_TESTAGEM").HasMaxLength(15).IsRequired();
        }
    }
}

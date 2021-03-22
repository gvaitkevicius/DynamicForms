using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map.Qualidade
{
    public class LaudoTesteFisicoMap : IEntityTypeConfiguration<LaudoTesteFisico>
    {
        public void Configure(EntityTypeBuilder<LaudoTesteFisico> builder)
        {
            builder.ToTable("T_LAUDO_TESTE_FISICO");
            builder.HasKey(x => x.LTF_ID);
            builder.Property(x => x.LTF_ID).HasColumnName("LTF_ID").IsRequired();
            builder.Property(x => x.LTF_EMISSAO).HasColumnName("LTF_EMISSAO");
            builder.Property(x => x.LTF_VALOR).HasColumnName("LTF_VALOR");
            builder.Property(x => x.LTF_OBS).HasColumnName("LTF_OBS").HasMaxLength(120);
            builder.Property(x => x.LTF_STATUS).HasColumnName("LTF_STATUS").HasMaxLength(60);
            builder.Property(x => x.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(60);
            builder.Property(x => x.ROT_PRO_ID).HasColumnName("ROT_PRO_ID").HasMaxLength(30);
            builder.Property(x => x.FPR_SEQ_REPETICAO).HasColumnName("FPR_SEQ_REPETICAO");
            builder.Property(x => x.USE_ID).HasColumnName("USE_ID");
            builder.Property(x => x.LTF_DATA_ULTIMA_ALTERACAO).HasColumnName("LTF_DATA_ULTIMA_ALTERACAO");
            builder.Property(x => x.LTF_DATA_EMISSAO_TESTES).HasColumnName("LTF_DATA_EMISSAO_TESTES");
            builder.Property(x => x.LTF_DATA_EMISSAO_INSPECOES).HasColumnName("LTF_DATA_EMISSAO_INSPECOES");

            builder.HasOne(x => x.Usuario).WithMany(u => u.LaudoTesteFisico).HasForeignKey(x => x.USE_ID);
        }
    }
}

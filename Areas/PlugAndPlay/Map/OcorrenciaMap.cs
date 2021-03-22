using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class OcorrenciaMap : IEntityTypeConfiguration<Ocorrencia>
    {

        public void Configure(EntityTypeBuilder<Ocorrencia> builder)
        {
            builder.ToTable("T_OCORRENCIAS");
            builder.HasKey(x => x.OCO_ID);
            builder.Property(x => x.OCO_ID).HasColumnName("OCO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.OCO_DESCRICAO).HasColumnName("OCO_DESCRICAO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.OCO_SUB_TIPO).HasColumnName("OCO_SUB_TIPO").IsRequired();
            builder.Property(x => x.TIP_ID).HasColumnName("TIP_ID").IsRequired();
            builder.Property(x => x.GMA_ID).HasColumnName("GMA_ID");
            builder.Property(x => x.MAQ_ID).HasColumnName("MAQ_ID").HasMaxLength(30);
            builder.Property(x => x.SPR).HasColumnName("SPR");
            builder.HasOne(x => x.TipoOcorrencia).WithMany(to => to.Ocorrencia).HasForeignKey(x => x.TIP_ID);
            builder.HasOne(x => x.GrupoMaquina).WithMany(gp => gp.Ocorrencia).HasForeignKey(x => x.GMA_ID);
            builder.HasOne(x => x.Maquina).WithMany(m => m.Ocorrencia).HasForeignKey(x => x.MAQ_ID);
        }
    }
}

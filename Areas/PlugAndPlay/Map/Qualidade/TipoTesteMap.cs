using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map.Qualidade
{
    public class TipoTesteMap : IEntityTypeConfiguration<TipoTeste>
    {
        public void Configure(EntityTypeBuilder<TipoTeste> builder)
        {
            builder.ToTable("T_TIPO_TESTE");
            builder.HasKey(x => x.TT_ID);
            builder.Property(x => x.TT_ID).HasColumnName("TT_ID").IsRequired();
            builder.Property(x => x.TT_NOME).HasColumnName("TT_NOME").HasMaxLength(50).IsRequired();
            builder.Property(x => x.TT_DESC).HasColumnName("TT_DESC").HasMaxLength(200).IsRequired();
            builder.Property(x => x.TT_TOL_MAIS).HasColumnName("TT_TOL_MAIS");
            builder.Property(x => x.TT_TOL_MENOS).HasColumnName("TT_TOL_MENOS");
            builder.Property(x => x.TT_NORMA).HasColumnName("TT_NORMA").HasMaxLength(50);
            builder.Property(x => x.TT_INICIO_PROCESSO).HasColumnName("TT_INICIO_PROCESSO").HasMaxLength(1).IsRequired();
            builder.Property(x => x.TA_ID).HasColumnName("TA_ID").IsRequired();
            builder.Property(x => x.UNI_ID).HasColumnName("UNI_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.TT_N_AMOSTRAS_P_TESTE).HasColumnName("TT_N_AMOSTRAS_P_TESTE");
            builder.Property(x => x.TT_MAX_DEF_CRITICO).HasColumnName("TT_MAX_DEF_CRITICO");
            builder.Property(x => x.TT_MAX_DEF_GRAVE).HasColumnName("TT_MAX_DEF_GRAVE");
            builder.Property(x => x.TT_ESPECIFICACAO).HasColumnName("TT_ESPECIFICACAO");
            builder.HasOne(x => x.UnidadeMedida).WithMany(y => y.TipoTeste).HasForeignKey(x => new { x.UNI_ID });
            builder.HasOne(x => x.TipoAvaliacao).WithMany(u => u.TipoTeste).HasForeignKey(x => x.TA_ID);
        }
    }
}

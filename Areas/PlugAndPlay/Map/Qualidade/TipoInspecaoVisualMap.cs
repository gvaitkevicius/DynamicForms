using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map.Qualidade
{
    public class TipoInspecaoVisualMap : IEntityTypeConfiguration<TipoInspecaoVisual>
    {
        public void Configure(EntityTypeBuilder<TipoInspecaoVisual> builder)
        {
            builder.ToTable("T_TIPO_INSPECAO_VISUAL");
            builder.HasKey(x => x.TIV_ID);
            builder.Property(x => x.TIV_ID).HasColumnName("TIV_ID").IsRequired();
            builder.Property(x => x.TIV_NOME).HasColumnName("TIV_NOME").HasMaxLength(60);
            builder.Property(x => x.TIV_DESCRICAO).HasColumnName("TIV_DESCRICAO").HasMaxLength(120);
            builder.Property(x => x.TIV_FECHAMENTO).HasColumnName("TIV_FECHAMENTO").HasMaxLength(1);
            builder.Property(x => x.TIV_AMOSTRA_ALEATORIA).HasColumnName("TIV_AMOSTRA_ALEATORIA").HasMaxLength(1);
            builder.Property(x => x.TIV_N_AMOSTRAS).HasColumnName("TIV_N_AMOSTRAS");
            builder.Property(x => x.TIV_MEDIDA).HasColumnName("TIV_MEDIDA");
            builder.Property(x => x.TIV_ESPECIFICACAO).HasColumnName("TIV_ESPECIFICACAO");
            builder.Property(x => x.TIV_TOL_MAIS).HasColumnName("TIV_TOL_MAIS");
            builder.Property(x => x.TIV_TOL_MENOS).HasColumnName("TIV_TOL_MENOS");
        }
    }
}

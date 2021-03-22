using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map
{
    public class V_PAINEL_GESTOR_DESEMPENHO_TURNOS_MAP : IEntityTypeConfiguration<V_PAINEL_GESTOR_DESEMPENHO_TURNOS>
    {
        public void Configure(EntityTypeBuilder<V_PAINEL_GESTOR_DESEMPENHO_TURNOS> builder)
        {
            builder.ToTable("V_PAINEL_GESTOR_DESEMPENHO_TURNOS");
            builder.HasKey(x => x.FEE_DIA_TURMA);
            builder.Property(x => x.FEE_DIA_TURMA).HasColumnName("FEE_DIA_TURMA").HasMaxLength(8);
            builder.Property(x => x.TURN_ID).HasColumnName("TURN_ID").HasMaxLength(10);
            builder.Property(x => x.TURM_ID).HasColumnName("TURM_ID").HasMaxLength(10);
            builder.Property(x => x.TEMPO_PARADAS_NAO_PROGRAMADAS).HasColumnName("TEMPO_PARADAS_NAO_PROGRAMADAS").HasMaxLength(30).IsRequired();
            builder.Property(x => x.QTD_PARADAS_NAO_PROGRAMADAS).HasColumnName("QTD_PARADAS_NAO_PROGRAMADAS").IsRequired();
            builder.Property(x => x.TEMPO_PARADAS_NAO_PROGRAMADAS_EXETO_SETUP).HasColumnName("TEMPO_PARADAS_NAO_PROGRAMADAS_EXETO_SETUP").HasMaxLength(30).IsRequired();
            builder.Property(x => x.QTD_PARADAS_NAO_PROGRAMADAS_EXETO_SETUP).HasColumnName("QTD_PARADAS_NAO_PROGRAMADAS_EXETO_SETUP").IsRequired();
            builder.Property(x => x.TEMPO_PEQUENAS_PARADAS).HasColumnName("TEMPO_PEQUENAS_PARADAS").HasMaxLength(30).IsRequired();
            builder.Property(x => x.QTD_PEQUENAS_PARADAS).HasColumnName("QTD_PEQUENAS_PARADAS").IsRequired();
            builder.Property(x => x.TEMPO_PLANEJADO).HasColumnName("TEMPO_PLANEJADO").IsRequired();
            builder.Property(x => x.TEMPO_PRODUZINDO).HasColumnName("TEMPO_PRODUZINDO").IsRequired();
            builder.Property(x => x.DS_DIA_TURMA).HasColumnName("DS_DIA_TURMA").HasMaxLength(8);
            builder.Property(x => x.DS_TURM_ID).HasColumnName("DS_TURM_ID").HasMaxLength(10);
            builder.Property(x => x.DS_TURN_ID).HasColumnName("DS_TURN_ID").HasMaxLength(10);
            builder.Property(x => x.SETUP_GERAL_AZUL).HasColumnName("SETUP_GERAL_AZUL").IsRequired();
            builder.Property(x => x.SETUP_GERAL_VERDE).HasColumnName("SETUP_GERAL_VERDE").IsRequired();
            builder.Property(x => x.SETUP_GERAL_AMARELO).HasColumnName("SETUP_GERAL_AMARELO").IsRequired();
            builder.Property(x => x.SETUP_GERAL_VERMELHO).HasColumnName("SETUP_GERAL_VERMELHO").IsRequired();
            builder.Property(x => x.SETUP_AZUL).HasColumnName("SETUP_AZUL").IsRequired();
            builder.Property(x => x.SETUP_VERDE).HasColumnName("SETUP_VERDE").IsRequired();
            builder.Property(x => x.SETUP_AMARELO).HasColumnName("SETUP_AMARELO").IsRequired();
            builder.Property(x => x.SETUP_VERMELHO).HasColumnName("SETUP_VERMELHO").IsRequired();
            builder.Property(x => x.SETUPA_AZUL).HasColumnName("SETUPA_AZUL").IsRequired();
            builder.Property(x => x.SETUPA_VERDE).HasColumnName("SETUPA_VERDE").IsRequired();
            builder.Property(x => x.SETUPA_AMARELO).HasColumnName("SETUPA_AMARELO").IsRequired();
            builder.Property(x => x.SETUPA_VERMELHO).HasColumnName("SETUPA_VERMELHO").IsRequired();
            builder.Property(x => x.PERFORMANCE_AZUL).HasColumnName("PERFORMANCE_AZUL").IsRequired();
            builder.Property(x => x.PERFORMANCE_VERDE).HasColumnName("PERFORMANCE_VERDE").IsRequired();
            builder.Property(x => x.PERFORMANCE_AMARELO).HasColumnName("PERFORMANCE_AMARELO").IsRequired();
            builder.Property(x => x.PERFORMANCE_VERMELHO).HasColumnName("PERFORMANCE_VERMELHO").IsRequired();
            builder.Property(x => x.QTD_SETUPS).HasColumnName("QTD_SETUPS").IsRequired();
        }
    }
}

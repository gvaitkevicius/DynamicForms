using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map
{
    public class V_PAINEL_GESTOR_DESEMPENHO_TURNOS_MAQUINA_MAP : IEntityTypeConfiguration<V_PAINEL_GESTOR_DESEMPENHO_TURNOS_MAQUINA>
    {
        public void Configure(EntityTypeBuilder<V_PAINEL_GESTOR_DESEMPENHO_TURNOS_MAQUINA> builder)
        {
            builder.ToTable("V_PAINEL_GESTOR_DESEMPENHO_TURNOS_MAQUINA");
            builder.HasKey(x => x.MAQ_ID);
            builder.Property(x => x.FEE_DIA_TURMA).HasColumnName("FEE_DIA_TURMA").HasMaxLength(8);
            builder.Property(x => x.MAQ_ID).HasColumnName("MAQ_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.TURN_ID).HasColumnName("TURN_ID").HasMaxLength(10);
            builder.Property(x => x.TURM_ID).HasColumnName("TURM_ID").HasMaxLength(10);
            builder.Property(x => x.TEMPO_PARADAS_NAO_PROGRAMADAS).HasColumnName("TEMPO_PARADAS_NAO_PROGRAMADAS").HasMaxLength(30);
            builder.Property(x => x.QTD_PARADAS_NAO_PROGRAMADAS).HasColumnName("QTD_PARADAS_NAO_PROGRAMADAS");
            builder.Property(x => x.TEMPO_PARADAS_NAO_PROGRAMADAS_EXETO_SETUP).HasColumnName("TEMPO_PARADAS_NAO_PROGRAMADAS_EXETO_SETUP").HasMaxLength(30);
            builder.Property(x => x.QTD_PARADAS_NAO_PROGRAMADAS_EXETO_SETUP).HasColumnName("QTD_PARADAS_NAO_PROGRAMADAS_EXETO_SETUP");
            builder.Property(x => x.TEMPO_PEQUENAS_PARADAS).HasColumnName("TEMPO_PEQUENAS_PARADAS").HasMaxLength(30);
            builder.Property(x => x.QTD_PEQUENAS_PARADAS).HasColumnName("QTD_PEQUENAS_PARADAS");
            builder.Property(x => x.TEMPO_PLANEJADO).HasColumnName("TEMPO_PLANEJADO");
            builder.Property(x => x.TEMPO_PRODUZINDO).HasColumnName("TEMPO_PRODUZINDO");
            builder.Property(x => x.DS_DIA_TURMA).HasColumnName("DS_DIA_TURMA").HasMaxLength(8);
            builder.Property(x => x.DS_TURM_ID).HasColumnName("DS_TURM_ID").HasMaxLength(10);
            builder.Property(x => x.DS_TURN_ID).HasColumnName("DS_TURN_ID").HasMaxLength(10);
            builder.Property(x => x.DS_MAQ_ID).HasColumnName("DS_MAQ_ID").HasMaxLength(30);
            builder.Property(x => x.SETUP_GERAL_AZUL).HasColumnName("SETUP_GERAL_AZUL");
            builder.Property(x => x.SETUP_GERAL_VERDE).HasColumnName("SETUP_GERAL_VERDE");
            builder.Property(x => x.SETUP_GERAL_AMARELO).HasColumnName("SETUP_GERAL_AMARELO");
            builder.Property(x => x.SETUP_GERAL_VERMELHO).HasColumnName("SETUP_GERAL_VERMELHO");
            builder.Property(x => x.SETUP_AZUL).HasColumnName("SETUP_AZUL");
            builder.Property(x => x.SETUP_VERDE).HasColumnName("SETUP_VERDE");
            builder.Property(x => x.SETUP_AMARELO).HasColumnName("SETUP_AMARELO");
            builder.Property(x => x.SETUP_VERMELHO).HasColumnName("SETUP_VERMELHO");
            builder.Property(x => x.SETUPA_AZUL).HasColumnName("SETUPA_AZUL");
            builder.Property(x => x.SETUPA_VERDE).HasColumnName("SETUPA_VERDE");
            builder.Property(x => x.SETUPA_AMARELO).HasColumnName("SETUPA_AMARELO");
            builder.Property(x => x.SETUPA_VERMELHO).HasColumnName("SETUPA_VERMELHO");
            builder.Property(x => x.PERFORMANCE_AZUL).HasColumnName("PERFORMANCE_AZUL");
            builder.Property(x => x.PERFORMANCE_VERDE).HasColumnName("PERFORMANCE_VERDE");
            builder.Property(x => x.PERFORMANCE_AMARELO).HasColumnName("PERFORMANCE_AMARELO");
            builder.Property(x => x.PERFORMANCE_VERMELHO).HasColumnName("PERFORMANCE_VERMELHO");
            builder.Property(x => x.QTD_SETUPS).HasColumnName("QTD_SETUPS");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class MaquinaMap : IEntityTypeConfiguration<Maquina>
    {

        public void Configure(EntityTypeBuilder<Maquina> builder)
        {
            builder.ToTable("T_MAQUINA");
            builder.HasKey(x => x.MAQ_ID);
            builder.Property(x => x.MAQ_ID).HasColumnName("MAQ_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.MAQ_DESCRICAO).HasColumnName("MAQ_DESCRICAO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.CAL_ID).HasColumnName("CAL_ID");
            builder.Property(x => x.MAQ_CONTROL_IP).HasColumnName("MAQ_CONTROL_IP").HasMaxLength(30);
            builder.Property(x => x.GMA_ID).HasColumnName("GMA_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.MAQ_STATUS).HasColumnName("MAQ_STATUS").HasMaxLength(30);
            builder.Property(x => x.MAQ_ULTIMA_ATUALIZACAO).HasColumnName("MAQ_ULTIMA_ATUALIZACAO");
            builder.Property(x => x.MAQ_SIRENE_SEMAFORO).HasColumnName("MAQ_SIRENE_SEMAFORO");
            builder.Property(x => x.MAQ_COR_SEMAFORO).HasColumnName("MAQ_COR_SEMAFORO").HasMaxLength(30);
            builder.Property(x => x.MAQ_ID_MAQ_PAI).HasColumnName("MAQ_ID_MAQ_PAI").HasMaxLength(30);
            builder.Property(x => x.MAQ_TIPO_CONTADOR).HasColumnName("MAQ_TIPO_CONTADOR");
            builder.Property(x => x.MAQ_TIPO_PLANEJAMENTO).HasColumnName("MAQ_TIPO_PLANEJAMENTO").HasMaxLength(60);
            builder.Property(x => x.MAQ_AVALIA_CUSTO).HasColumnName("MAQ_AVALIA_CUSTO");
            builder.Property(x => x.FPR_ID_OP_PRODUZINDO).HasColumnName("FPR_ID_OP_PRODUZINDO");
            builder.Property(x => x.MAQ_CONGELA_FILA).HasColumnName("MAQ_CONGELA_FILA");
            builder.Property(x => x.MAQ_TEMPO_MIN_PARADA).HasColumnName("MAQ_TEMPO_MIN_PARADA");
            builder.Property(x => x.MAQ_QTD_CORES).HasColumnName("MAQ_QTD_CORES");
            builder.Property(x => x.MAQ_ID_INTEGRACAO).HasColumnName("MAQ_ID_INTEGRACAO").HasMaxLength(100);
            builder.Property(x => x.MAQ_ID_INTEGRACAO_ERP).HasColumnName("MAQ_ID_INTEGRACAO_ERP").HasMaxLength(100);
            builder.Property(x => x.MAQ_HIERARQUIA_SEQ_TRANSFORMACAO).HasColumnName("MAQ_HIERARQUIA_SEQ_TRANSFORMACAO");
            builder.Property(x => x.EQU_ID).HasColumnName("EQU_ID").HasMaxLength(30);
            builder.Property(x => x.MAQ_PERCENTUAL_INICIO_PASSO_ANTERIOR).HasColumnName("MAQ_PERCENTUAL_INICIO_PASSO_ANTERIOR");
            builder.Property(x => x.MAQ_ACOMPANHA_LOTE_PILOTO).HasColumnName("MAQ_ACOMPANHA_LOTE_PILOTO").HasMaxLength(60);
            builder.Property(x => x.MAQ_ID_SENSOR).HasColumnName("MAQ_ID_SENSOR");
            builder.Property(x => x.MAQ_DEBOUNCING_LOW).HasColumnName("MAQ_DEBOUNCING_LOW");
            builder.Property(x => x.MAQ_DEBOUNCING_HIGHT).HasColumnName("MAQ_DEBOUNCING_HIGHT");
            builder.Property(x => x.MAQ_TIPO_SINAL).HasColumnName("MAQ_TIPO_SINAL");
            builder.Property(x => x.TEM_ID).HasColumnName("TEM_ID");
            builder.Property(x => x.MAQ_LARGURA_UTIL).HasColumnName("MAQ_LARGURA_UTIL");

            builder.HasOne(x => x.Calendario).WithMany(x => x.Maquinas).HasForeignKey(x => x.CAL_ID);
            builder.HasOne(x => x.GrupoMaquina).WithMany(x => x.Maquinas).HasForeignKey(x => x.GMA_ID);
            builder.HasOne(x => x.TemplateDeTestes).WithMany(x => x.Maquina).HasForeignKey(x => x.TEM_ID);

        }
    }
}
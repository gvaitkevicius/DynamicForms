using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map
{
    public class ViewCargaMaquinasPedidosMap : IEntityTypeConfiguration<ViewCargaMaquinasPedidos>
    {
        public void Configure(EntityTypeBuilder<ViewCargaMaquinasPedidos> builder)
        {
            builder.ToTable("V_CARGA_MAQUINA_PEDIDOS");
            builder.HasKey(x => x.ORD_ID);
            builder.Property(x => x.TEMPO_OP).HasColumnName("TEMPO_OP");
            builder.Property(x => x.FPR_DATA_INICIO_PREVISTA).HasColumnName("FPR_DATA_INICIO_PREVISTA").IsRequired();
            builder.Property(x => x.FPR_DATA_FIM_PREVISTA).HasColumnName("FPR_DATA_FIM_PREVISTA").IsRequired();
            builder.Property(x => x.ROT_MAQ_ID).HasColumnName("ROT_MAQ_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(60).IsRequired();
            builder.Property(x => x.FPR_COR_FILA).HasColumnName("FPR_COR_FILA").HasMaxLength(30);
            builder.Property(x => x.ORD_COR_FILA).HasColumnName("ORD_COR_FILA").HasMaxLength(30);
            builder.Property(x => x.ORD_PRO_ID).HasColumnName("ORD_PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.ORD_PRO_DESCRICAO).HasColumnName("ORD_PRO_DESCRICAO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.ORD_QUANTIDADE).HasColumnName("ORD_QUANTIDADE").IsRequired();
            builder.Property(x => x.ORD_DATA_ENTREGA_DE).HasColumnName("ORD_DATA_ENTREGA_DE").IsRequired();
            builder.Property(x => x.EQU_ID).HasColumnName("EQU_ID").HasMaxLength(30);
            builder.Property(x => x.ORD_M2_TOTAL).HasColumnName("ORD_M2_TOTAL");
            builder.Property(x => x.ORD_PESO_TOTAL).HasColumnName("ORD_PESO_TOTAL");
            builder.Property(x => x.CLI_NOME).HasColumnName("CLI_NOME").HasMaxLength(100).IsRequired();
        }
    }
}

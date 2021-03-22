using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class EtiquetaMap : IEntityTypeConfiguration<Etiqueta>
    {
        public void Configure(EntityTypeBuilder<Etiqueta> builder)
        {
            builder.ToTable("T_ETIQUETAS");
            builder.HasKey(x => x.ETI_ID);
            builder.Property(x => x.ETI_ID).HasColumnName("ETI_ID").HasMaxLength(30);
            builder.Property(x => x.ETI_EMISSAO).HasColumnName("ETI_EMISSAO");
            builder.Property(x => x.ETI_CODIGO_BARRAS).HasColumnName("ETI_CODIGO_BARRAS").HasMaxLength(100);
            builder.Property(x => x.ETI_SEQUENCIA).HasColumnName("ETI_SEQUENCIA").HasMaxLength(3);
            builder.Property(x => x.ETI_NUMERO_COPIAS).HasColumnName("ETI_NUMERO_COPIAS");
            builder.Property(x => x.ETI_STATUS).HasColumnName("ETI_STATUS").HasMaxLength(1);
            builder.Property(x => x.ETI_DATA_FABRICACAO).HasColumnName("ETI_DATA_FABRICACAO");
            builder.Property(x => x.ETI_COD_BARRAS_ORIGINAL).HasColumnName("ETI_COD_BARRAS_ORIGINAL");
            builder.Property(x => x.ETI_OP_ORIGINAL).HasColumnName("ETI_OP_ORIGINAL").HasMaxLength(30);
            builder.Property(x => x.MAQ_ID).HasColumnName("MAQ_ID").HasMaxLength(30);
            builder.Property(x => x.IMP_ID).HasColumnName("IMP_ID");
            builder.Property(x => x.USE_ID).HasColumnName("USE_ID");
            builder.Property(x => x.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(30);
            builder.Property(x => x.ROT_PRO_ID).HasColumnName("ROT_PRO_ID").HasMaxLength(30);
            builder.Property(x => x.ETI_QUANTIDADE_PALETE).HasColumnName("ETI_QUANTIDADE_PALETE");
            builder.Property(x => x.FPR_SEQ_REPETICAO).HasColumnName("FPR_SEQ_REPETICAO");
            builder.Property(x => x.ETI_LOTE).HasColumnName("ETI_LOTE");
            builder.Property(x => x.ETI_SUB_LOTE).HasColumnName("ETI_SUB_LOTE");
            builder.Property(x => x.ETI_IMPRIMIR_DE).HasColumnName("ETI_IMPRIMIR_DE");
            builder.Property(x => x.ETI_IMPRIMIR_ATE).HasColumnName("ETI_IMPRIMIR_ATE");
            builder.HasOne(me => me.Usuario).WithMany(u => u.Etiquetas).HasForeignKey(me => me.USE_ID);
            builder.HasOne(me => me.Produto).WithMany(u => u.Etiquetas).HasForeignKey(me => me.ROT_PRO_ID);
            builder.HasOne(me => me.Order).WithMany(u => u.Etiquetas).HasForeignKey(me => me.ORD_ID);



        }
    }
}

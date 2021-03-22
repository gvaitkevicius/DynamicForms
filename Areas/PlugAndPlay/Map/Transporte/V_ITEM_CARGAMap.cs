using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class V_ITEM_CARGAMap : IEntityTypeConfiguration<V_ITEM_CARGA>
    {
        public void Configure(EntityTypeBuilder<V_ITEM_CARGA> builder)
        {
            builder.ToTable("V_ITEM_CARGA");
            builder.HasKey(x => new { x.CAR_ID, x.ORD_ID });
            builder.Property(x => x.CAR_ID).HasColumnName("CAR_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(60).IsRequired();
            builder.Property(x => x.ITC_ENTREGA_PLANEJADA).HasColumnName("ITC_ENTREGA_PLANEJADA").IsRequired();
            builder.Property(x => x.ITC_ENTREGA_REALIZADA).HasColumnName("ITC_ENTREGA_REALIZADA").IsRequired();
            builder.Property(x => x.ITC_ORDEM_ENTREGA).HasColumnName("ITC_ORDEM_ENTREGA").IsRequired();
            builder.Property(x => x.ITC_QTD_PLANEJADA).HasColumnName("ITC_QTD_PLANEJADA").IsRequired();
            builder.Property(x => x.ITC_QTD_REALIZADA).HasColumnName("ITC_QTD_REALIZADA").IsRequired();
            builder.Property(x => x.ORD_HASH_KEY).HasColumnName("ORD_HASH_KEY").HasMaxLength(300).IsRequired();
            builder.Property(x => x.CLI_ID).HasColumnName("CLI_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.CLI_NOME).HasColumnName("CLI_NOME").HasMaxLength(100).IsRequired();
            builder.Property(x => x.UF_COD).HasColumnName("UF_COD").HasMaxLength(2);
            builder.Property(x => x.MUN_NOME).HasColumnName("MUN_NOME").HasMaxLength(100);
            builder.Property(x => x.PON_DESCRICAO).HasColumnName("PON_DESCRICAO").HasMaxLength(100);
            builder.Property(x => x.PRO_LARGURA_EMBALADA).HasColumnName("PRO_LARGURA_EMBALADA");
            builder.Property(x => x.PRO_COMPRIMENTO_EMBALADA).HasColumnName("PRO_COMPRIMENTO_EMBALADA");
            builder.Property(x => x.PRO_ALTURA_EMBALADA).HasColumnName("PRO_ALTURA_EMBALADA");

            builder.Property(x => x.ORD_DATA_ENTREGA_DE).HasColumnName("ORD_DATA_ENTREGA_DE");
            builder.Property(x => x.ORD_DATA_ENTREGA_ATE).HasColumnName("ORD_DATA_ENTREGA_ATE");


            builder.HasOne(x => x.Carga).WithMany(c => c.V_ITEM_CARGA).HasForeignKey(x => x.CAR_ID);
            builder.HasOne(x => x.Order).WithMany(o => o.V_ITEM_CARGA).HasForeignKey(x => x.ORD_ID);
        }
    }
}

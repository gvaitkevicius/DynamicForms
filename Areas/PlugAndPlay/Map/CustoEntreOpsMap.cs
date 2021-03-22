using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class CustoEntreOpsMap : IEntityTypeConfiguration<CustoEntreOps>
    {
        public void Configure(EntityTypeBuilder<CustoEntreOps> builder)
        {
            builder.ToTable("T_CUSTO_ENTRE_OPS");
            builder.HasKey(x => x.CUS_UNIC_ID);
            builder.Property(x => x.CUS_UNIC_ID).HasColumnName("CUS_UNIC_ID").IsRequired();
            builder.Property(x => x.CUS_ID).HasColumnName("CUS_ID").HasMaxLength(150).IsRequired();
            builder.Property(x => x.PRO_ID).HasColumnName("PRO_ID").HasMaxLength(30);
            builder.Property(x => x.GRP_ID).HasColumnName("GRP_ID").HasMaxLength(30);
            builder.Property(x => x.MAQ_ID).HasColumnName("MAQ_ID").HasMaxLength(30);
            builder.Property(x => x.CUS_DESCRICAO).HasColumnName("CUS_DESCRICAO").HasMaxLength(3000);
            builder.Property(x => x.CUS_PESO_CASO_VERDADEIRO).HasColumnName("CUS_PESO_CASO_VERDADEIRO");
            builder.Property(x => x.CUS_PESO_CASO_FALSO).HasColumnName("CUS_PESO_CASO_FALSO");
            builder.Property(x => x.CUS_TIPO_AVALIACAO).HasColumnName("CUS_TIPO_AVALIACAO").HasMaxLength(10).IsRequired();
            builder.Property(x => x.CUS_GRUPO_TEMPO_SETUP).HasColumnName("CUS_GRUPO_TEMPO_SETUP").HasMaxLength(50);
            builder.Property(x => x.CUS_TEMPO_CASO_VERDADEIRO).HasColumnName("CUS_TEMPO_CASO_VERDADEIRO");
            builder.Property(x => x.CUS_TEMPO_CASO_FALSO).HasColumnName("CUS_TEMPO_CASO_FALSO");
            builder.Property(x => x.CUS_OPERACOES).HasColumnName("CUS_OPERACOES").HasMaxLength(100);


            builder.HasOne(x => x.Produto).WithMany(p => p.CustoEntreOps).HasForeignKey(x => x.PRO_ID);
            builder.HasOne(x => x.GrupoProdutoOutros).WithMany(gp => gp.CustoEntreOps).HasForeignKey(x => x.GRP_ID);
            builder.HasOne(x => x.Maquina).WithMany(m => m.CustoEntreOps).HasForeignKey(x => x.MAQ_ID);
        }
    }
}

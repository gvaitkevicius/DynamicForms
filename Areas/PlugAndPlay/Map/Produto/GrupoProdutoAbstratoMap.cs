using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class GrupoProdutoAbstratoMap : IEntityTypeConfiguration<GrupoProdutoAbstrato>
    {
        public void Configure(EntityTypeBuilder<GrupoProdutoAbstrato> builder)
        {
            builder.ToTable("T_GRUPO_PRODUTO");
            builder.HasKey(x => x.GRP_ID);
            builder.Property(x => x.GRP_ID).HasColumnName("GRP_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.GRP_DESCRICAO).HasColumnName("GRP_DESCRICAO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.GRP_ATIVO).HasColumnName("GRP_ATIVO").HasMaxLength(1);
            builder.Property(x => x.GRP_DT_CRIACAO).HasColumnName("GRP_DT_CRIACAO");
            builder.Property(x => x.GRP_ID_INTEGRACAO).HasColumnName("GRP_ID_INTEGRACAO").HasMaxLength(100);
            builder.Property(x => x.GRP_ID_INTEGRACAO_ERP).HasColumnName("GRP_ID_INTEGRACAO_ERP").HasMaxLength(100);
            builder.HasDiscriminator<int>("GRP_TYPE")
                .HasValue<GrupoProdutoAbstrato>(999)
                .HasValue<GrupoProdutoComposicao>(2)
                .HasValue<GrupoProdutoConjunto>(3)
                .HasValue<GrupoProdutoPalete>(6)
                .HasValue<GrupoProdutoWMSExpedicao>(9)
                .HasValue<GrupoProdutoOutros>(1000);
        }
    }

    public class GrupoProdutoWMSExpedicaoMap : IEntityTypeConfiguration<GrupoProdutoWMSExpedicao>
    {
        public void Configure(EntityTypeBuilder<GrupoProdutoWMSExpedicao> builder)
        {
            builder.Property(x => x.GRP_TIPO).HasColumnName("GRP_TIPO");
        }
    }

    public class GrupoProdutoOutrosMap : IEntityTypeConfiguration<GrupoProdutoOutros>
    {
        public void Configure(EntityTypeBuilder<GrupoProdutoOutros> builder)
        {
            builder.Property(x => x.GRP_TIPO).HasColumnName("GRP_TIPO");
            builder.Property(x => x.TEM_ID).HasColumnName("TEM_ID");

            builder.HasOne(me => me.TemplateDeTestes).WithMany(u => u.GrupoProdutoOutros).HasForeignKey(me => me.TEM_ID);
        }
    }

    public class GrupoProdutoPaleteMap : IEntityTypeConfiguration<GrupoProdutoPalete>
    {
        public void Configure(EntityTypeBuilder<GrupoProdutoPalete> builder)
        {
            builder.Property(x => x.GRP_TIPO).HasColumnName("GRP_TIPO");
            builder.Property(x => x.TEM_ID).HasColumnName("TEM_ID");

            builder.HasOne(gp => gp.TemplateDeTestes).WithMany(t => t.GrupoProdutoPalete).HasForeignKey(gp => gp.TEM_ID);
        }
    }

    public class GrupoProdutoConjuntoMap : IEntityTypeConfiguration<GrupoProdutoConjunto>
    {
        public void Configure(EntityTypeBuilder<GrupoProdutoConjunto> builder)
        {
            builder.Property(x => x.GRP_TIPO).HasColumnName("GRP_TIPO");
            builder.Property(x => x.TEM_ID).HasColumnName("TEM_ID");

            builder.HasOne(me => me.TemplateDeTestes).WithMany(u => u.GrupoProdutoConjunto).HasForeignKey(me => me.TEM_ID);
        }
    }

    public class GrupoProdutoComposicaoMap : IEntityTypeConfiguration<GrupoProdutoComposicao>
    {
        public void Configure(EntityTypeBuilder<GrupoProdutoComposicao> builder)
        {
            builder.Property(x => x.TEM_ID).HasColumnName("TEM_ID");
            builder.Property(x => x.GRP_TIPO).HasColumnName("GRP_TIPO");
            builder.Property(x => x.GRP_PAP_ONDA).HasColumnName("GRP_PAP_ONDA").HasMaxLength(10);
            builder.Property(x => x.GRP_PAP_GRAMATURA).HasColumnName("GRP_PAP_GRAMATURA");
            builder.Property(x => x.GRP_PAP_ALTURA).HasColumnName("GRP_PAP_ALTURA");
            builder.Property(x => x.GRP_PAP_NOME_COMERCIAL).HasColumnName("GRP_PAP_NOME_COMERCIAL").HasMaxLength(100);
            builder.Property(x => x.GRP_PAPEL1).HasColumnName("GRP_PAPEL1").HasMaxLength(30);
            builder.Property(x => x.GRP_PAPEL2).HasColumnName("GRP_PAPEL2").HasMaxLength(30);
            builder.Property(x => x.GRP_PAPEL3).HasColumnName("GRP_PAPEL3").HasMaxLength(30);
            builder.Property(x => x.GRP_PAPEL4).HasColumnName("GRP_PAPEL4").HasMaxLength(30);
            builder.Property(x => x.GRP_PAPEL5).HasColumnName("GRP_PAPEL5").HasMaxLength(30);
            builder.Property(x => x.GRP_RESINA_INTERNA).HasColumnName("GRP_RESINA_INTERNA").HasMaxLength(1);
            builder.Property(x => x.GRP_RESINA_EXTERNA).HasColumnName("GRP_RESINA_EXTERNA").HasMaxLength(1);
            builder.Property(x => x.GRP_PERFORMANCE_METRO_LINEAR_POR_SEGUNDO).HasColumnName("GRP_PERFORMANCE_METRO_LINEAR_POR_SEGUNDO");

            builder.HasOne(me => me.TemplateDeTestes).WithMany(t => t.GrupoProdutoComposicao).HasForeignKey(me => me.TEM_ID);
        }
    }

    public class GrupoProdutoMap : IEntityTypeConfiguration<GrupoProduto>
    {
        public void Configure(EntityTypeBuilder<GrupoProduto> builder)
        {
            builder.ToTable("V_GRUPO_PRODUTO");
            builder.HasKey(x => x.GRP_ID);
            builder.Property(x => x.GRP_ID).HasColumnName("GRP_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.GRP_DESCRICAO).HasColumnName("GRP_DESCRICAO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.TEM_ID).HasColumnName("TEM_ID");
            builder.Property(x => x.GRP_TIPO).HasColumnName("GRP_TIPO");
            builder.Property(x => x.GRP_PAP_ONDA).HasColumnName("GRP_PAP_ONDA").HasMaxLength(10);
            builder.Property(x => x.GRP_PAP_GRAMATURA).HasColumnName("GRP_PAP_GRAMATURA");
            builder.Property(x => x.GRP_PAP_ALTURA).HasColumnName("GRP_PAP_ALTURA");
            builder.Property(x => x.GRP_PAP_NOME_COMERCIAL).HasColumnName("GRP_PAP_NOME_COMERCIAL").HasMaxLength(100);
            builder.Property(x => x.GRP_ATIVO).HasColumnName("GRP_ATIVO").HasMaxLength(1);
            builder.Property(x => x.GRP_DT_CRIACAO).HasColumnName("GRP_DT_CRIACAO");
            builder.Property(x => x.GRP_PAPEL1).HasColumnName("GRP_PAPEL1").HasMaxLength(30);
            builder.Property(x => x.GRP_PAPEL2).HasColumnName("GRP_PAPEL2").HasMaxLength(30);
            builder.Property(x => x.GRP_PAPEL3).HasColumnName("GRP_PAPEL3").HasMaxLength(30);
            builder.Property(x => x.GRP_PAPEL4).HasColumnName("GRP_PAPEL4").HasMaxLength(30);
            builder.Property(x => x.GRP_PAPEL5).HasColumnName("GRP_PAPEL5").HasMaxLength(30);
            builder.Property(x => x.GRP_ID_INTEGRACAO).HasColumnName("GRP_ID_INTEGRACAO").HasMaxLength(100);
            builder.Property(x => x.GRP_ID_INTEGRACAO_ERP).HasColumnName("GRP_ID_INTEGRACAO_ERP").HasMaxLength(100);
            builder.Property(x => x.GRP_TYPE).HasColumnName("GRP_TYPE");

            builder.HasOne(gp => gp.TemplateDeTestes).WithMany(t => t.GrupoProduto).HasForeignKey(gp => gp.TEM_ID);
        }
    }
}
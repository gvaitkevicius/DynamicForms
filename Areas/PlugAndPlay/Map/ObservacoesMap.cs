using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class ObservacoesMap : IEntityTypeConfiguration<Observacoes>
    {
        public void Configure(EntityTypeBuilder<Observacoes> builder)
        {
            builder.ToTable("T_OBSERVACOES");
            builder.HasKey(x => x.OBS_ID);
            builder.Property(x => x.OBS_ID).HasColumnName("OBS_ID").IsRequired();
            builder.Property(x => x.OBS_TIPO).HasColumnName("OBS_TIPO").HasMaxLength(30);
            builder.Property(x => x.OBS_DESCRICAO).HasColumnName("OBS_DESCRICAO").HasMaxLength(300);
            builder.Property(x => x.CLI_ID).HasColumnName("CLI_ID").HasMaxLength(30);
            builder.Property(x => x.MAQ_ID).HasColumnName("MAQ_ID").HasMaxLength(30);
            builder.Property(x => x.PRO_ID).HasColumnName("PRO_ID").HasMaxLength(30);
            builder.Property(x => x.ROT_SEQ_TRANFORMACAO).HasColumnName("ROT_SEQ_TRANFORMACAO");
            builder.Property(x => x.OBS_INTEGRACAO).HasColumnName("OBS_INTEGRACAO").HasMaxLength(50);

            builder.HasOne(x => x.Cliente).WithMany(c => c.Observacoes).HasForeignKey(x => x.CLI_ID);
            builder.HasOne(x => x.Roteiro).WithMany(c => c.Observacoes).HasForeignKey(x => new { x.MAQ_ID, x.PRO_ID, x.ROT_SEQ_TRANFORMACAO });
            builder.HasOne(x => x.Produto).WithMany(c => c.Observacoes).HasForeignKey(x => x.PRO_ID);


            // tipos    F-FATURAMENTO,PG-OBSERVAÇÕES GERAIS DE PRODUCAO-  PO-PRODUÇÃO INDULADEIRA,PC-PRODUÇÃO CONVERSAO,PC-PRODUÇÃO ACABAMENTO, E-ENGENHARIA, E-EXPEDICAO,
            /*
             DADOS GERAIS DO CLIENTE 
             DADOS PEDIDO = OBS DO CLIENTE    SELECT * FROM T_OBSERVACOES WHERE OBS_TIPO= 'PG' AND  ((CLI_ID = 'XXX' AND PRO_ID IS NULL) OU (CLI_ID = 'XXX' AND PRO_ID = 'CHICARA'))
             DADOS ONDULADEIRA = OBS DO CLIENTE    SELECT * FROM T_OBSERVACOES WHERE   OBS_TIPO= 'PO' AND CLI_ID = 'XXX'
             DADOS CONVERSAO = OBS DO CLIENTE    SELECT * FROM T_OBSERVACOES WHERE   OBS_TIPO= 'PC' AND CLI_ID = 'XXX'
             DADOS ACABAMENTO/PALETIZAÇÃO = OBS DO CLIENTE    SELECT * FROM T_OBSERVACOES WHERE   OBS_TIPO= 'PA' AND CLI_ID = 'XXX'
             DADOS EXPEDICAO = OBS DO CLIENTE    SELECT * FROM T_OBSERVACOES WHERE   OBS_TIPO= 'EP' AND CLI_ID = 'XXX'

            

             
             
             */


        }
    }
}

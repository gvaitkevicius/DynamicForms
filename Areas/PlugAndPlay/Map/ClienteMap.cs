using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class ClienteMap : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("T_CLIENTES");
            builder.HasKey(x => x.CLI_ID);
            builder.Property(x => x.CLI_ID).HasColumnName("CLI_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.CLI_NOME).HasColumnName("CLI_NOME").HasMaxLength(100).IsRequired();
            builder.Property(x => x.CLI_FONE).HasColumnName("CLI_FONE").HasMaxLength(68);
            builder.Property(x => x.CLI_OBS).HasColumnName("CLI_OBS").HasMaxLength(3000);
            builder.Property(x => x.CLI_ENDERECO_ENTREGA).HasColumnName("CLI_ENDERECO_ENTREGA").HasMaxLength(200);
            builder.Property(x => x.CLI_CPF_CNPJ).HasColumnName("CLI_CPF_CNPJ").HasMaxLength(18);
            builder.Property(x => x.CLI_BAIRRO_ENTREGA).HasColumnName("CLI_BAIRRO_ENTREGA").HasMaxLength(100);
            builder.Property(x => x.CLI_CEP_ENTREGA).HasColumnName("CLI_CEP_ENTREGA").HasMaxLength(10);
            builder.Property(x => x.CLI_EMAIL).HasColumnName("CLI_EMAIL").HasMaxLength(3000);
            builder.Property(x => x.CLI_INTEGRACAO).HasColumnName("CLI_INTEGRACAO").HasMaxLength(100);
            builder.Property(x => x.MUN_ID).HasColumnName("MUN_ID_ENTREGA").HasMaxLength(50);
            builder.Property(x => x.CLI_TRANSLADO).HasColumnName("CLI_TRANSLADO");
            builder.Property(x => x.CLI_REGIAO_ENTREGA).HasColumnName("CLI_REGIAO_ENTREGA").HasMaxLength(100);
            builder.Property(x => x.CLI_EXIGENTE_NA_IMPRESSAO).HasColumnName("CLI_EXIGENTE_NA_IMPRESSAO");
            builder.Property(x => x.CLI_TEMPO_MEDIO_ESPERA_DE_DESCARREGAMENTO).HasColumnName("CLI_TEMPO_MEDIO_ESPERA_DE_DESCARREGAMENTO");
            builder.Property(x => x.CLI_TEMPO_DESCARREGAMENTO_UNITARIO).HasColumnName("CLI_TEMPO_DESCARREGAMENTO_UNITARIO");
            builder.Property(x => x.CLI_PERCENTUAL_JANELA_EMBARQUE).HasColumnName("CLI_PERCENTUAL_JANELA_EMBARQUE");
            builder.Property(x => x.REP_ID).HasColumnName("REP_ID");

            builder.HasOne(x => x.Municipio).WithMany(m => m.Clientes).HasForeignKey(x => x.MUN_ID);
            builder.HasOne(x => x.Representantes).WithMany(x => x.Cliente).HasForeignKey(x => x.REP_ID);

        }
    }
    public class T_HORARIO_RECEBIMENTO_MAP : IEntityTypeConfiguration<T_HORARIO_RECEBIMENTO>
    {
        public void Configure(EntityTypeBuilder<T_HORARIO_RECEBIMENTO> builder)
        {
            builder.ToTable("T_HORARIO_RECEBIMENTO");
            builder.HasKey(x => x.HRE_ID);
            builder.Property(x => x.HRE_DIA_DA_SEMANA).HasColumnName("HRE_DIA_DA_SEMANA").IsRequired();
            builder.Property(x => x.HRE_HORA_INICIAL).HasColumnName("HRE_HORA_INICIAL").IsRequired();
            builder.Property(x => x.HRE_HORA_FINAL).HasColumnName("HRE_HORA_FINAL").IsRequired();
            builder.Property(x => x.CLI_ID).HasColumnName("CLI_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.HRE_ID).HasColumnName("HRE_ID").IsRequired();

            builder.HasOne(x => x.Cliente).WithMany(c => c.HorariosRecebimentos).HasForeignKey(x => x.CLI_ID);
        }
    }
}
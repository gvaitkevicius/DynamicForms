using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class MunicipioMap : IEntityTypeConfiguration<Municipio>
    {
        public void Configure(EntityTypeBuilder<Municipio> builder)
        {
            builder.ToTable("T_MUNICIPIOS");
            builder.HasKey(x => x.MUN_ID);
            builder.Property(x => x.MUN_ID).HasColumnName("MUN_ID").HasMaxLength(50).IsRequired();
            builder.Property(x => x.MUN_NOME).HasColumnName("MUN_NOME").HasMaxLength(100);
            builder.Property(x => x.UF_COD).HasColumnName("UF_COD").HasMaxLength(2);
            builder.Property(x => x.MUN_CODIGO_IBGE).HasColumnName("MUN_CODIGO_IBGE").HasMaxLength(50);
            builder.Property(x => x.MUN_LATITUDE).HasColumnName("MUN_LATITUDE");
            builder.Property(x => x.MUN_LONGITUDE).HasColumnName("MUN_LONGITUDE");
            builder.Property(x => x.MUN_ID_INTEGRACAO_ERP).HasColumnName("MUN_ID_INTEGRACAO_ERP").HasMaxLength(100);
            builder.Property(x => x.MUN_CODIGO_SIAFI).HasColumnName("MUN_CODIGO_SIAFI").HasMaxLength(100);
            builder.Property(x => x.MUN_CODIGO_CNPJ).HasColumnName("MUN_CODIGO_CNPJ").HasMaxLength(100);
            builder.Property(x => x.MUN_DISTANCIA_KM).HasColumnName("MUN_DISTANCIA_KM");
        }
    }
}

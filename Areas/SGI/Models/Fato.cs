using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace DynamicForms.Areas.SGI.Model
{
    public class Fato
    {
        public string Id { get; set; }
        public string Descricao { get; set; }
        public int IndicadorId { get; set; }
        public int DimensaoId { get; set; }
        public virtual T_Indicadores Indicador { get; set; }
        public virtual ICollection<Periodo> Periodos { get; set; }
    }
    public class IndicadorFatoDimensaoMap : IEntityTypeConfiguration<Fato>
    {
        /*
        * Migração do EntityFramework para o EntityCore, o construtor foi substituido pelo método
        * public void Configure(EntityTypeBuilder<> builder), que é a implementação da interface
        * IEntityTypeConfiguration<>
        * 
        public IndicadorFatoDimensaoMap()
        {
            ToTable("T_INDICADORES_FATOS_DIMENCOES");
            Property(x => x.Id).HasColumnName("FAT_ID").HasMaxLength(10).IsRequired();
            Property(x => x.Descricao).HasColumnName("FAT_DESCRICAO").HasMaxLength(100).IsRequired();
            Property(x => x.IndicadorId).HasColumnName("IND_ID").IsRequired();
            Property(x => x.DimensaoId).HasColumnName("DIM_ID").IsRequired();

            HasKey(x => new { x.Id, x.IndicadorId, x.DimensaoId });
        }
        */

        public void Configure(EntityTypeBuilder<Fato> builder)
        {
            builder.ToTable("T_INDICADORES_FATOS_DIMENCOES");
            builder.Property(x => x.Id).HasColumnName("FAT_ID").HasMaxLength(10).IsRequired();
            builder.Property(x => x.Descricao).HasColumnName("FAT_DESCRICAO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.IndicadorId).HasColumnName("IND_ID").IsRequired();
            builder.Property(x => x.DimensaoId).HasColumnName("DIM_ID").IsRequired();

            builder.HasKey(x => new { x.Id, x.IndicadorId, x.DimensaoId });
        }
    }
}
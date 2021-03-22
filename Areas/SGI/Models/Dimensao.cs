using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace DynamicForms.Areas.SGI.Model
{
    public class Dimensao
    {
        public string Id { get; set; }
        public int IndicadorId { get; set; }
        public string Descricao { get; set; }
        public string Sql { get; set; }
        public string Conexao { get; set; }
        public virtual T_Indicadores Indicador { get; set; }
        public virtual ICollection<Fato> Fatos { get; set; }
        public virtual ICollection<Periodo> Periodos { get; set; }
        public virtual ICollection<SubDimensao> SubDimensao { get; set; }
    }

    public class DimensaoMap : IEntityTypeConfiguration<Dimensao>
    {
        public void Configure(EntityTypeBuilder<Dimensao> builder)
        {
            builder.ToTable("T_INDICADORES_DIMENCOES");
            builder.HasKey(x => new { x.Id, x.IndicadorId });
            builder.Property(x => x.Id).HasColumnName("DIM_ID").IsRequired();
            builder.Property(x => x.IndicadorId).HasColumnName("IND_ID").IsRequired();
            builder.Property(x => x.Descricao).HasColumnName("DIM_DESCRICAO").HasMaxLength(200).IsRequired();
            builder.Property(x => x.Sql).HasColumnName("DIM_SQL").HasMaxLength(8000);
            builder.Property(x => x.Conexao).HasColumnName("DIM_CONEXAO").HasMaxLength(200);

            builder.HasOne(x => x.Indicador).WithMany(x => x.Dimensoes).HasForeignKey(x => x.IndicadorId);
        }
    }

    public class SubDimensao
    {
        public string Id { get; set; }
        public string Descricao { get; set; }
        public int IndicadorId { get; set; }
        public virtual T_Indicadores Indicador { get; set; }
    }

    public class IndicadorDimencao
    {
        public int DIM_ID { get; set; }
        public int IND_ID { get; set; }
        public string DIM_DESCRICAO { get; set; }
        public string DIM_SQL { get; set; }
        public string DIM_CONEXAO { get; set; }
    }


}
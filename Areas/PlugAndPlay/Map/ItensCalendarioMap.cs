
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class ItensCalendarioMap : IEntityTypeConfiguration<ItensCalendario>
    {


        public void Configure(EntityTypeBuilder<ItensCalendario> builder)
        {
            builder.ToTable("T_ITENS_CALENDARIO");
            builder.HasKey(x => x.ICA_ID);
            builder.Property(x => x.ICA_ID).HasColumnName("ICA_ID").IsRequired();
            builder.Property(x => x.ICA_DATA_DE).HasColumnName("ICA_DATA_DE").IsRequired();
            builder.Property(x => x.ICA_DATA_ATE).HasColumnName("ICA_DATA_ATE").IsRequired();
            builder.Property(x => x.ICA_OBSERVACAO).HasColumnName("ICA_OBSERVACAO").HasMaxLength(200);
            builder.Property(x => x.ICA_TIPO).HasColumnName("ICA_TIPO").IsRequired();
            builder.Property(x => x.URM_ID).HasColumnName("URM_ID").HasMaxLength(10);
            builder.Property(x => x.URN_ID).HasColumnName("URN_ID").HasMaxLength(10);
            builder.Property(x => x.CAL_ID).HasColumnName("CAL_ID").IsRequired();
            builder.Property(x => x.ICA_LIMPESA_MAQUINA).HasColumnName("ICA_LIMPESA_MAQUINA");
            builder.Property(x => x.MAQ_ID).HasColumnName("MAQ_ID");
            builder.Property(x => x.PRO_ID).HasColumnName("PRO_ID");

            builder.HasOne(x => x.Turno).WithMany(x => x.Calendarios).HasForeignKey(x => x.URN_ID);
            builder.HasOne(x => x.Turma).WithMany(x => x.Calendarios).HasForeignKey(x => x.URM_ID);
            builder.HasOne(x => x.Calendario).WithMany(x => x.IntensCalendario).HasForeignKey(x => x.CAL_ID);
            builder.HasOne(x => x.Maquina).WithMany(x => x.Calendarios).HasForeignKey(x => x.MAQ_ID);
            builder.HasOne(x => x.Produto).WithMany(x => x.Calendarios).HasForeignKey(x => x.PRO_ID);
        }
    }
}
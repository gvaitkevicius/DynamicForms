
using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map
{
    public class T_MAQUINAS_EQUIPESMap : IEntityTypeConfiguration<T_MAQUINAS_EQUIPES>
    {
        public void Configure(EntityTypeBuilder<T_MAQUINAS_EQUIPES> builder)
        {
            builder.ToTable("T_MAQUINAS_EQUIPES");
            builder.HasKey(x => new { x.MAQ_ID, x.EQU_ID });
            builder.Property(x => x.MAQ_ID).HasColumnName("MAQ_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.EQU_ID).HasColumnName("EQU_ID").HasMaxLength(30).IsRequired();
            builder.Property(x => x.CAL_ID).HasColumnName("CAL_ID");

            builder.HasOne(x => x.Maquina).WithMany(tv => tv.MaquinasEquipes).HasForeignKey(x => x.MAQ_ID);
            builder.HasOne(x => x.Equipe).WithMany(tv => tv.MaquinasEquipes).HasForeignKey(x => x.EQU_ID);
        }
    }
}

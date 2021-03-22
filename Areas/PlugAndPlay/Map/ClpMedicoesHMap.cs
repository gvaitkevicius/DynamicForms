using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;

namespace DynamicForms.Areas.PlugAndPlay.Map
{
    public class ClpMedicoesHMap : IEntityTypeConfiguration<ClpMedicoesH>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ClpMedicoesH> builder)
        {
            builder.ToTable("T_CLP_MEDICOES_H");
            builder.HasKey(x => new { x.MAQUINA_ID, x.DATA_INI, x.DATA_FIM });
            builder.Property(x => x.QTD).HasColumnName("QTD").IsRequired();
            builder.Property(x => x.DATA_INI).HasColumnName("DATA_INI").IsRequired();
            builder.Property(x => x.DATA_FIM).HasColumnName("DATA_FIM").IsRequired();
            builder.Property(x => x.MAQUINA_ID).HasColumnName("MAQUINA_ID").HasMaxLength(10).IsRequired();
            builder.Property(x => x.GRUPO).HasColumnName("GRUPO");
            builder.Property(x => x.QTD_REGS).HasColumnName("QTD_REGS");
        }
    }
}

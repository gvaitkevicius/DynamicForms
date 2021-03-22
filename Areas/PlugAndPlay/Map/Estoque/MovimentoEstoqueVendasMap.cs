using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class MovimentoEstoqueVendasMap : IEntityTypeConfiguration<MovimentoEstoqueVendas>
    {
        public void Configure(EntityTypeBuilder<MovimentoEstoqueVendas> builder)
        {
            builder.Property(me => me.TIP_ID).HasColumnName("TIP_ID").HasMaxLength(3).IsRequired();
            builder.Property(me => me.PRO_ID).HasColumnName("PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(me => me.MOV_QUANTIDADE).HasColumnName("MOV_QUANTIDADE").IsRequired();
            //builder.Property(me => me.MOV_DATA_HORA_EMISSAO).HasColumnName("MOV_DATA_HORA_EMISSAO").IsRequired();
            builder.Property(me => me.MOV_DOC).HasColumnName("MOV_DOC").HasMaxLength(30).IsRequired();
            builder.Property(me => me.MOV_LOTE).HasColumnName("MOV_LOTE").HasMaxLength(30).IsRequired();
            builder.Property(me => me.MOV_SUB_LOTE).HasColumnName("MOV_SUB_LOTE").HasMaxLength(30).IsRequired();
            builder.Property(me => me.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(30).IsRequired();
            builder.Property(me => me.FPR_SEQ_TRANFORMACAO).HasColumnName("FPR_SEQ_TRANFORMACAO").IsRequired();
            builder.Property(me => me.FPR_SEQ_REPETICAO).HasColumnName("FPR_SEQ_REPETICAO").IsRequired();
            builder.Property(me => me.OCO_ID).HasColumnName("OCO_ID").HasMaxLength(30).IsRequired();
            builder.Property(me => me.MOV_OBS).HasColumnName("MOV_OBS").HasMaxLength(400).IsRequired();
            builder.Property(me => me.MOV_ARMAZEM).HasColumnName("MOV_ARMAZEM").HasMaxLength(30).IsRequired();
            builder.Property(me => me.MOV_ENDERECO).HasColumnName("MOV_ENDERECO").HasMaxLength(30).IsRequired();
            builder.Property(me => me.MOV_OBS_OP_PARCIAL).HasColumnName("MOV_OBS_OP_PARCIAL").HasMaxLength(400).IsRequired();
            builder.Property(me => me.MOV_OCO_ID_OP_PARCIAL).HasColumnName("MOV_OCO_ID_OP_PARCIAL").HasMaxLength(30).IsRequired();
            builder.Property(me => me.USE_ID).HasColumnName("USE_ID").IsRequired();
            //DEFINE CHAVE ESTRANGEIRA 
            builder.HasOne(me => me.Produto).WithMany(u => u.MovimentoEstoqueVendas).HasForeignKey(me => me.PRO_ID);
            builder.HasOne(me => me.Order).WithMany(u => u.MovimentoEstoqueVendas).HasForeignKey(me => me.ORD_ID);
            builder.HasOne(me => me.OcorrenciaVendas).WithMany(u => u.MovimentoEstoqueVendas).HasForeignKey(me => me.OCO_ID);
            builder.HasOne(me => me.OcorrenciaProducaoParciais).WithMany(u => u.MovimentoEstoqueVendas).HasForeignKey(me => me.MOV_OCO_ID_OP_PARCIAL);
            builder.HasOne(me => me.Carga).WithMany(u => u.MovimentoEstoqueVendas).HasForeignKey(me => me.CAR_ID);
            builder.HasOne(me => me.Turno).WithMany(u => u.MovimentoEstoqueVendas).HasForeignKey(me => me.TURN_ID);
            builder.HasOne(me => me.Turma).WithMany(u => u.MovimentoEstoqueVendas).HasForeignKey(me => me.TURM_ID);
            builder.HasOne(me => me.Maquina).WithMany(u => u.MovimentoEstoqueVendas).HasForeignKey(me => me.MAQ_ID);
            builder.HasOne(me => me.TipoMovSaidaVendas).WithMany(u => u.MovimentoEstoqueVendas).HasForeignKey(me => me.TIP_ID);
            builder.HasOne(me => me.Usuario).WithMany(u => u.MovimentoEstoqueVendas).HasForeignKey(me => me.USE_ID);
        }
    }
}

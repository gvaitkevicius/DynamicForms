using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class MovimentoEstoqueTransferenciaSimplesMap : IEntityTypeConfiguration<MovimentoEstoqueTransferenciaSimples>
    {
        public void Configure(EntityTypeBuilder<MovimentoEstoqueTransferenciaSimples> builder)
        {
            builder.Property(me => me.TIP_ID).HasColumnName("TIP_ID").HasMaxLength(3).IsRequired();
            builder.Property(me => me.PRO_ID).HasColumnName("PRO_ID").HasMaxLength(30).IsRequired();
            builder.Property(me => me.MOV_QUANTIDADE).HasColumnName("MOV_QUANTIDADE").IsRequired();
            builder.Property(me => me.MOV_DATA_HORA_EMISSAO).HasColumnName("MOV_DATA_HORA_EMISSAO").IsRequired();
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
            builder.HasOne(me => me.TipoMovTransferenciaInterna).WithMany(u => u.MovimentoEstoqueTransferenciaSimples).HasForeignKey(me => me.TIP_ID);
            builder.HasOne(me => me.Turno).WithMany(u => u.MovimentoEstoqueTransferenciaSimples).HasForeignKey(me => me.TURN_ID);
            builder.HasOne(me => me.Turma).WithMany(u => u.MovimentoEstoqueTransferenciaSimples).HasForeignKey(me => me.TURM_ID);
            builder.HasOne(me => me.Carga).WithMany(u => u.MovimentoEstoqueTransferenciaSimples).HasForeignKey(me => me.CAR_ID);
            builder.HasOne(me => me.OcorrenciaProducao).WithMany(u => u.MovimentoEstoqueTransferenciaSimples).HasForeignKey(me => me.OCO_ID);
            builder.HasOne(me => me.OcorrenciaProducaoParciais).WithMany(u => u.MovimentoEstoqueTransferenciaSimples).HasForeignKey(me => me.MOV_OCO_ID_OP_PARCIAL);
            builder.HasOne(me => me.Usuario).WithMany(u => u.MovimentoEstoqueTransferenciaSimples).HasForeignKey(me => me.USE_ID);
            builder.HasOne(me => me.Order).WithMany(u => u.MovimentoEstoqueTransferenciaSimples).HasForeignKey(me => me.ORD_ID);
        }
    }

}

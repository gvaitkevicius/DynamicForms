
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class MovimentoEstoqueMap : IEntityTypeConfiguration<MovimentoEstoque>
    {


        public void Configure(EntityTypeBuilder<MovimentoEstoque> builder)
        {
            builder.ToTable("V_MOVIMENTOS_ESTOQUE");

            builder.HasKey(me => me.MOV_ID).HasName("MOV_ID");
            builder.Property(me => me.TIP_ID).HasColumnName("TIP_ID").HasMaxLength(3);
            builder.Property(me => me.PRO_ID).HasColumnName("PRO_ID").HasMaxLength(30);
            builder.Property(me => me.MOV_QUANTIDADE).HasColumnName("MOV_QUANTIDADE");
            builder.Property(me => me.MOV_DATA_HORA_EMISSAO).HasColumnName("MOV_DATA_HORA_EMISSAO");
            builder.Property(me => me.MOV_DOC).HasColumnName("MOV_DOC").HasMaxLength(30);
            builder.Property(me => me.MOV_LOTE).HasColumnName("MOV_LOTE").HasMaxLength(30);
            builder.Property(me => me.MOV_SUB_LOTE).HasColumnName("MOV_SUB_LOTE").HasMaxLength(30);
            builder.Property(me => me.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(30);
            builder.Property(me => me.FPR_SEQ_TRANFORMACAO).HasColumnName("FPR_SEQ_TRANFORMACAO");
            builder.Property(me => me.FPR_SEQ_REPETICAO).HasColumnName("FPR_SEQ_REPETICAO");
            builder.Property(me => me.OCO_ID).HasColumnName("OCO_ID").HasMaxLength(30);
            builder.Property(me => me.MOV_OBS).HasColumnName("MOV_OBS").HasMaxLength(400);
            builder.Property(me => me.MOV_ARMAZEM).HasColumnName("MOV_ARMAZEM").HasMaxLength(30);
            builder.Property(me => me.MOV_ENDERECO).HasColumnName("MOV_ENDERECO").HasMaxLength(30);
            builder.Property(me => me.MOV_OBS_OP_PARCIAL).HasColumnName("MOV_OBS_OP_PARCIAL").HasMaxLength(400);
            builder.Property(me => me.MOV_OCO_ID_OP_PARCIAL).HasColumnName("MOV_OCO_ID_OP_PARCIAL").HasMaxLength(30);

            builder.Property(me => me.MAQ_ID).HasColumnName("MAQ_ID").HasMaxLength(30);
            builder.Property(me => me.TURN_ID).HasColumnName("TURN_ID").HasMaxLength(10);
            builder.Property(me => me.TURM_ID).HasColumnName("TURM_ID").HasMaxLength(10);
            builder.Property(me => me.MOV_DIA_TURMA).HasColumnName("MOV_DIA_TURMA").HasMaxLength(8);
            builder.Property(me => me.MOV_DATA_HORA_CRIACAO).HasColumnName("MOV_DATA_HORA_CRIACAO");
            builder.Property(me => me.MOV_ESTORNO).HasColumnName("MOV_ESTORNO").HasMaxLength(1);
            builder.Property(me => me.USE_ID).HasColumnName("USE_ID");
            builder.Property(me => me.MOV_ID_INTEGRACAO).HasColumnName("MOV_ID_INTEGRACAO").HasMaxLength(100);
            builder.Property(me => me.MOV_ID_INTEGRACAO_ERP).HasColumnName("MOV_ID_INTEGRACAO_ERP").HasMaxLength(100);
            builder.Property(me => me.MOV_TYPE).HasColumnName("MOV_TYPE");
            builder.Property(me => me.CAR_ID).HasColumnName("CAR_ID").HasMaxLength(30);
            builder.Property(me => me.MOV_ID_DESTINO).HasColumnName("MOV_ID_DESTINO");
            builder.Property(me => me.PRO_ID_DESTINO).HasColumnName("PRO_ID_DESTINO").HasMaxLength(30);
            builder.Property(me => me.MOV_LOTE_DESTINO).HasColumnName("MOV_LOTE_DESTINO").HasMaxLength(30);
            builder.Property(me => me.MOV_SUB_LOTE_DESTINO).HasColumnName("MOV_SUB_LOTE_DESTINO").HasMaxLength(30);
            builder.Property(me => me.MOV_ID_ORIGEM).HasColumnName("MOV_ID_ORIGEM");
            builder.Property(me => me.PRO_ID_ORIGEM).HasColumnName("PRO_ID_ORIGEM").HasMaxLength(30);
            builder.Property(me => me.MOV_LOTE_ORIGEM).HasColumnName("MOV_LOTE_ORIGEM").HasMaxLength(30);
            builder.Property(me => me.MOV_SUB_LOTE_ORIGEM).HasColumnName("MOV_SUB_LOTE_ORIGEM").HasMaxLength(30);

            builder.HasOne(me => me.Produto).WithMany(p => p.MovimentoEstoque).HasForeignKey(me => me.PRO_ID);
            builder.HasOne(me => me.Maquina).WithMany(m => m.MovimentoEstoque).HasForeignKey(me => me.MAQ_ID);
            builder.HasOne(me => me.OcorrenciaOpParcial).WithMany(ocp => ocp.MovimentoEstoque).HasForeignKey(me => me.MOV_OCO_ID_OP_PARCIAL);
            builder.HasOne(me => me.Turma).WithMany(me => me.MovimentoEstoque).HasForeignKey(me => me.TURM_ID);
            builder.HasOne(me => me.Turno).WithMany(ocp => ocp.MovimentoEstoque).HasForeignKey(me => me.TURN_ID);
        }
    }
}
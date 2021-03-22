using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class MovimentoEstoqueAbstrataMap : IEntityTypeConfiguration<MovimentoEstoqueAbstrata>
    {
        public void Configure(EntityTypeBuilder<MovimentoEstoqueAbstrata> builder)
        {
            builder.ToTable("T_MOVIMENTOS_ESTOQUE");
            builder.HasKey(me => me.MOV_ID);
            builder.Property(me => me.MOV_ID).HasColumnName("MOV_ID").IsRequired();
            //builder.Property(me => me.TIP_ID).HasColumnName("TIP_ID").HasMaxLength(3).IsRequired();
            //builder.Property(me => me.PRO_ID).HasColumnName("PRO_ID").HasMaxLength(30).IsRequired();
            //builder.Property(me => me.MOV_QUANTIDADE).HasColumnName("MOV_QUANTIDADE").IsRequired();
            builder.Property(me => me.MOV_DATA_HORA_EMISSAO).HasColumnName("MOV_DATA_HORA_EMISSAO").IsRequired();
            builder.Property(me => me.MOV_DOC).HasColumnName("MOV_DOC").HasMaxLength(30);
            //builder.Property(me => me.MOV_LOTE).HasColumnName("MOV_LOTE").HasMaxLength(30).IsRequired();
            //builder.Property(me => me.MOV_SUB_LOTE).HasColumnName("MOV_SUB_LOTE").HasMaxLength(30).IsRequired();
            //builder.Property(me => me.ORD_ID).HasColumnName("ORD_ID").HasMaxLength(30).IsRequired();
            //builder.Property(me => me.FPR_SEQ_TRANFORMACAO).HasColumnName("FPR_SEQ_TRANFORMACAO").IsRequired();
            //builder.Property(me => me.FPR_SEQ_REPETICAO).HasColumnName("FPR_SEQ_REPETICAO").IsRequired();
            //builder.Property(me => me.OCO_ID).HasColumnName("OCO_ID").HasMaxLength(30).IsRequired();
            //builder.Property(me => me.MOV_OBS).HasColumnName("MOV_OBS").HasMaxLength(400).IsRequired();
            //builder.Property(me => me.MOV_ARMAZEM).HasColumnName("MOV_ARMAZEM").HasMaxLength(30).IsRequired();
            //builder.Property(me => me.MOV_ENDERECO).HasColumnName("MOV_ENDERECO").HasMaxLength(30).IsRequired();
            //builder.Property(me => me.MOV_OBS_OP_PARCIAL).HasColumnName("MOV_OBS_OP_PARCIAL").HasMaxLength(400).IsRequired();
            //builder.Property(me => me.MOV_OCO_ID_OP_PARCIAL).HasColumnName("MOV_OCO_ID_OP_PARCIAL").HasMaxLength(30).IsRequired();

            builder.Property(me => me.MAQ_ID).HasColumnName("MAQ_ID").HasMaxLength(30).IsRequired();
            builder.Property(me => me.TURN_ID).HasColumnName("TURN_ID").HasMaxLength(10).IsRequired();
            builder.Property(me => me.TURM_ID).HasColumnName("TURM_ID").HasMaxLength(10).IsRequired();
            builder.Property(me => me.MOV_DIA_TURMA).HasColumnName("MOV_DIA_TURMA").HasMaxLength(8).IsRequired();
            builder.Property(me => me.MOV_DATA_HORA_CRIACAO).HasColumnName("MOV_DATA_HORA_CRIACAO").IsRequired();
            builder.Property(me => me.MOV_ESTORNO).HasColumnName("MOV_ESTORNO").HasMaxLength(1).IsRequired();

            builder.Property(me => me.MOV_ID_INTEGRACAO).HasColumnName("MOV_ID_INTEGRACAO").HasMaxLength(100).IsRequired();
            builder.Property(me => me.MOV_ID_INTEGRACAO_ERP).HasColumnName("MOV_ID_INTEGRACAO_ERP").HasMaxLength(100).IsRequired();
            builder.Property(me => me.CAR_ID).HasColumnName("CAR_ID").HasMaxLength(30);
            builder.Property(me => me.MOV_ID_DESTINO).HasColumnName("MOV_ID_DESTINO").IsRequired();
            builder.Property(me => me.PRO_ID_DESTINO).HasColumnName("PRO_ID_DESTINO").HasMaxLength(30).IsRequired();
            builder.Property(me => me.MOV_LOTE_DESTINO).HasColumnName("MOV_LOTE_DESTINO").HasMaxLength(30).IsRequired();
            builder.Property(me => me.MOV_SUB_LOTE_DESTINO).HasColumnName("MOV_SUB_LOTE_DESTINO").HasMaxLength(30).IsRequired();
            builder.Property(me => me.MOV_ID_ORIGEM).HasColumnName("MOV_ID_ORIGEM").IsRequired();
            builder.Property(me => me.PRO_ID_ORIGEM).HasColumnName("PRO_ID_ORIGEM").HasMaxLength(30).IsRequired();
            builder.Property(me => me.MOV_LOTE_ORIGEM).HasColumnName("MOV_LOTE_ORIGEM").HasMaxLength(30).IsRequired();
            builder.Property(me => me.MOV_SUB_LOTE_ORIGEM).HasColumnName("MOV_SUB_LOTE_ORIGEM").HasMaxLength(30).IsRequired();


            builder.HasDiscriminator<int>("MOV_TYPE").
            HasValue<MovimentoEstoqueConsumoMateriaPrima>(1).
            HasValue<MovimentoEstoqueProducao>(2).
            HasValue<MovimentoEstoquePerdas>(3).
            HasValue<MovimentoEstoqueTransferenciaSimples>(4).
            HasValue<MovimentoEstoqueEntradaInventario>(5).
            HasValue<MovimentoEstoqueSaidaInventario>(6).
            HasValue<MovimentoEstoqueVendas>(7).
            HasValue<MovimentoEstoqueDevolucao>(8).
            HasValue<MovimentoEstoqueReservaDeEstoque>(1001);


            //builder.HasOne(me => me.Carga).WithMany(od => od.MovimentoEstoqueAbstrata).HasForeignKey(me => me.CAR_ID);
            // builder.HasOne(me => me.Produto).WithMany(p => p.MovimentoEstoqueAbstrata).HasForeignKey(me => me.PRO_ID);
            //builder.HasOne(me => me.Order).WithMany(od => od.MovimentoEstoqueAbstrata).HasForeignKey(me => me.ORD_ID);
            //builder.HasOne(me => me.Maquina).WithMany(m => m.MovimentoEstoqueAbstrata).HasForeignKey(me => me.MAQ_ID);
            ////builder.HasOne(me => me.Ocorrencia).WithMany(oc => oc.MovimentoEstoqueAbstrata).HasForeignKey(me => me.OCO_ID);
            ////builder.HasOne(me => me.OcorrenciaOpParcial).WithMany(ocp => ocp.MovimentoEstoqueAbstrata).HasForeignKey(me => me.MOV_OCO_ID_OP_PARCIAL);
            //builder.HasOne(me => me.Turma).WithMany(ocp => ocp.MovimentoEstoqueAbstrata).HasForeignKey(me => me.TURM_ID);
            //builder.HasOne(me => me.Turno).WithMany(ocp => ocp.MovimentoEstoqueAbstrata).HasForeignKey(me => me.TURN_ID);
        }
    }
}

using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Map.Estoque
{
    //public class MovimentoEstoqueMap : IEntityTypeConfiguration<MovimentoEstoque>
    //{
    //    /*public MovimentoEstoqueMap()
    //    {
    //        * Migração do EntityFramework para o EntityCore, o construtor foi substituido pelo método
    //         * public void Configure(EntityTypeBuilder<> builder), que é a implementação da interface
    //         * IEntityTypeConfiguration<>
    //        ToTable("T_MOVIMENTOS_ESTOQUE");
    //        Property(x => x.Id).HasColumnName("MOV_ID");
    //        Property(x => x.Quantidade).HasColumnName("MOV_QUANTIDADE").IsRequired();
    //        Property(x => x.Tipo).HasColumnName("TIP_ID").HasMaxLength(3).IsRequired();
    //        Property(x => x.DataHoraCriacao).HasColumnName("MOV_DATA_HORA_EMISSAO").IsRequired();
    //        Property(x => x.DataHoraEmissao).HasColumnName("MOV_DATA_HORA_CRIACAO").IsRequired();
    //        Property(x => x.DiaTurma).HasColumnName("MOV_DIA_TURMA").HasMaxLength(8).IsOptional();
    //        Property(x => x.Lote).HasColumnName("MOV_LOTE").IsOptional();
    //        Property(x => x.SubLote).HasColumnName("MOV_SUB_LOTE").IsOptional();
    //        Property(x => x.Observacao).HasColumnName("MOV_OBS").HasMaxLength(200).IsOptional();
    //        Property(x => x.ProdutoId).HasColumnName("PRO_ID").HasMaxLength(30).IsRequired();
    //        Property(x => x.OrderId).HasColumnName("ORD_ID").HasMaxLength(30).IsRequired();
    //        Property(x => x.MaquinaId).HasColumnName("MAQ_ID").HasMaxLength(30).IsRequired();
    //        Property(x => x.UsuarioId).HasColumnName("USU_ID").IsRequired();
    //        Property(x => x.OcorrenciaId).HasColumnName("OCO_ID").IsOptional();
    //        Property(x => x.Armazem).HasColumnName("MOV_ARMAZEM").HasMaxLength(30).IsOptional();
    //        Property(x => x.Endereco).HasColumnName("MOV_ENDERECO").HasMaxLength(30).IsOptional();
    //        Property(x => x.Estorno).HasColumnName("MOV_ESTORNO").HasMaxLength(2).IsOptional();
    //        Property(x => x.SequenciaTransformacao).HasColumnName("FPR_SEQ_TRANFORMACAO").IsOptional();
    //        Property(x => x.SequenciaRepeticao).HasColumnName("FPR_SEQ_REPETICAO").IsOptional();
    //        Property(x => x.TurnoId).HasColumnName("TURN_ID").IsOptional();
    //        Property(x => x.TurmaId).HasColumnName("TURM_ID").IsOptional();
    //        Property(x => x.ObsOpParcial).HasColumnName("MOV_OBS_OP_PARCIAL").HasMaxLength(200).IsOptional();
    //        Property(x => x.OcoIdOpParcial).HasColumnName("MOV_OCO_ID_OP_PARCIAL").IsOptional();

    //        HasKey(x => x.Id);

    //        HasRequired(x => x.Produto).WithMany(x => x.MovimentosEstoque).HasForeignKey(x => x.ProdutoId);
    //        HasRequired(x => x.Order).WithMany(x => x.MovimentosEstoque).HasForeignKey(x => x.OrderId);
    //        HasRequired(x => x.Maquina).WithMany(x => x.MovimentosEstoque).HasForeignKey(x => x.MaquinaId);
    //        HasRequired(x => x.Usuario).WithMany(x => x.MovimentosEstoque).HasForeignKey(x => x.UsuarioId);
    //        HasOptional(x => x.Ocorrencia).WithMany(x => x.MovimentosEstoque).HasForeignKey(x => x.OcorrenciaId);
    //        HasOptional(x => x.OcorrenciaOpParcial).WithMany(x => x.MovimentosEstoqueOpParcial).HasForeignKey(x => x.OcoIdOpParcial);

    //        HasMany(x => x.Feedbacks).WithMany(x => x.MovimentosEstoque).Map(x =>
    //        {
    //            x.MapLeftKey("MOV_ID");
    //            x.MapRightKey("FEE_ID");
    //            x.ToTable("T_FEEDBACK_MOV_ESTOQUE");
    //        });
            
    //    } */

    //    public void Configure(EntityTypeBuilder<MovimentoEstoque> builder)
    //    {
    //        builder.ToTable("T_MOVIMENTOS_ESTOQUE");
    //        builder.Property(me => me.Id).HasColumnName("MOV_ID");
    //        builder.Property(me => me.Quantidade).HasColumnName("MOV_QUANTIDADE").IsRequired();
    //        builder.Property(me => me.Tipo).HasColumnName("TIP_ID").HasMaxLength(3).IsRequired();
    //        builder.Property(me => me.DataHoraCriacao).HasColumnName("MOV_DATA_HORA_EMISSAO").IsRequired();
    //        builder.Property(me => me.DataHoraEmissao).HasColumnName("MOV_DATA_HORA_CRIACAO").IsRequired();
    //        builder.Property(me => me.DiaTurma).HasColumnName("MOV_DIA_TURMA").HasMaxLength(8);
    //        builder.Property(me => me.Lote).HasColumnName("MOV_LOTE");
    //        builder.Property(me => me.SubLote).HasColumnName("MOV_SUB_LOTE");
    //        builder.Property(me => me.Observacao).HasColumnName("MOV_OBS").HasMaxLength(200);
    //        builder.Property(me => me.ProdutoId).HasColumnName("PRO_ID").HasMaxLength(30).IsRequired();
    //        builder.Property(me => me.OrderId).HasColumnName("ORD_ID").HasMaxLength(30).IsRequired();
    //        builder.Property(me => me.MaquinaId).HasColumnName("MAQ_ID").HasMaxLength(30).IsRequired();
    //        builder.Property(me => me.T_UsuarioId).HasColumnName("USU_ID").IsRequired();
    //        builder.Property(me => me.OcorrenciaId).HasColumnName("OCO_ID");
    //        builder.Property(me => me.Armazem).HasColumnName("MOV_ARMAZEM").HasMaxLength(30);
    //        builder.Property(me => me.Endereco).HasColumnName("MOV_ENDERECO").HasMaxLength(30);
    //        builder.Property(me => me.Estorno).HasColumnName("MOV_ESTORNO").HasMaxLength(2);
    //        builder.Property(me => me.SequenciaTransformacao).HasColumnName("FPR_SEQ_TRANFORMACAO");
    //        builder.Property(me => me.SequenciaRepeticao).HasColumnName("FPR_SEQ_REPETICAO");
    //        builder.Property(me => me.TurnoId).HasColumnName("TURN_ID");
    //        builder.Property(me => me.TurmaId).HasColumnName("TURM_ID");
    //        builder.Property(me => me.ObsOpParcial).HasColumnName("MOV_OBS_OP_PARCIAL").HasMaxLength(200);
    //        builder.Property(me => me.OcoIdOpParcial).HasColumnName("MOV_OCO_ID_OP_PARCIAL");

    //        builder.HasKey(me => me.Id);

    //        builder.HasOne(me => me.Produto).WithMany(p => p.MovimentosEstoque).HasForeignKey(me => me.ProdutoId);
    //        builder.HasOne(me => me.Order).WithMany(od => od.MovimentosEstoque).HasForeignKey(me => me.OrderId);
    //        builder.HasOne(me => me.Maquina).WithMany(m => m.MovimentosEstoque).HasForeignKey(me => me.MaquinaId);
    //        builder.HasOne(me => me.Usuario).WithMany(u => u.MovimentosEstoque).HasForeignKey(me => me.T_UsuarioId);
    //        //builder.HasOne(me => me.Ocorrencia).WithMany(oc => oc.MovimentosEstoque).HasForeignKey(me => me.OcorrenciaId);
    //        //builder.HasOne(me => me.OcorrenciaOpParcial).WithMany(ocp => ocp.MovimentosEstoqueOpParcial).HasForeignKey(me => me.OcoIdOpParcial);
            
    //    }
    //}
}
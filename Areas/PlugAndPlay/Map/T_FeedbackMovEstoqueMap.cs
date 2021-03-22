using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    /*
     * Esta entidade foi criada para representar a relacao many-to-many entre as classes Feedback e MovimentoEstoque 
     * 
     */
    public class T_FeedbackMovEstoqueMap : IEntityTypeConfiguration<T_FeedbackMovEstoque>
    {
        public void Configure(EntityTypeBuilder<T_FeedbackMovEstoque> builder)
        {
            builder.ToTable("T_FEEDBACK_MOV_ESTOQUE");
            builder.HasKey(fme => new { fme.FeedbackId, fme.MovimentoEstoqueId });
            builder.HasOne(fme => fme.Feedback).WithMany(f => f.T_FeedbackMovEstoque).HasForeignKey(fme => fme.FeedbackId);
            builder.HasOne(fme => fme.MovimentoEstoque).WithMany(me => me.T_FeedbackMovEstoque).HasForeignKey(fme => fme.MovimentoEstoqueId);
            builder.Property(fme => fme.FeedbackId).HasColumnName("FEE_ID");
            builder.Property(fme => fme.MovimentoEstoqueId).HasColumnName("MOV_ID");
        }
    }
}


using DynamicForms.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.SGI.Model
{
    public class T_Favoritos
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VORITO")] [Required(ErrorMessage = "Campo IDFAVORITO requirido.")] public int IDFAVORITO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo USE_ID requirido.")] public int USE_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NDICADOR")] [Required(ErrorMessage = "Campo ID_INDICADOR requirido.")] public int ID_INDICADOR { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        public virtual T_Usuario t_usuario { get; set; }
        public virtual T_Indicadores t_indicadores { get; set; }
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  } 
    }

    /*
    class T_Favoritos_ResultConfiguration : IEntityTypeConfiguration<T_Favoritos>
    {
        * Migração do EntityFramework para o EntityCore, o construtor foi substituido pelo método
        * public void Configure(EntityTypeBuilder<> builder), que é a implementação da interface
        * IEntityTypeConfiguration<>
        * 
        public T_Favoritos_ResultConfiguration()
        {
            // Configurando propriedades e chaves
            this.HasKey(c => c.IDFAVORITO);

            this.Property(c => c.ID_USUARIO)
                .HasColumnName("ID_USUARIO")
                .IsRequired();

            this.HasRequired(e => e.T_Usuario)
                .WithMany(t => t.T_Favoritos)
                .HasForeignKey(c => c.ID_USUARIO);

            this.Property(c => c.ID_INDICADOR)
                .HasColumnName("ID_INDICADOR")
                .IsRequired();

            this.HasRequired(e => e.T_Indicadores)
                .WithMany(t => t.T_Favoritos)
                .HasForeignKey(c => c.ID_INDICADOR);

            // Configurando a Tabela
            this.ToTable("T_FAVORITOS");
        }
    }
    */
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Models
{
    public class T_Usuario_Perfil
    {
        public T_Usuario_Perfil()
        {

        }

        public int USE_ID { get; set; }
        public T_Usuario Usuario { get; set; }

        public int PER_ID { get; set; }
        public T_Perfil Perfil { get; set; }

        /// <summary>
        /// Esta propriedade foi criada para representar o estado do objeto
        /// insert, update, delete ou unchanged 
        /// </summary>
        [NotMapped]
        public string PlayAction { get; set; }

        /// <summary>
        /// Deve seguir a seguinte convecão: NameProperty:MsgErro;NameProperty:MsgErro; ...
        /// Representa os erros de validacao deste objeto
        /// </summary>
        [NotMapped]
        public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }


    }

    public class T_Usuario_Perfil_ResultConfiguration : IEntityTypeConfiguration<T_Usuario_Perfil>
    {
        public void Configure(EntityTypeBuilder<T_Usuario_Perfil> builder)
        {
            builder.ToTable("T_USUARIO_PERFIL");
            builder.HasKey(up => new { up.USE_ID, up.PER_ID });
            builder.HasOne(up => up.Perfil).WithMany(p => p.T_Usuario_Perfil).HasForeignKey(up => up.PER_ID);
            builder.HasOne(up => up.Usuario).WithMany(u => u.T_Usuario_Perfil).HasForeignKey(u => u.USE_ID);
            builder.Property(up => up.USE_ID).HasColumnName("USE_ID");
            builder.Property(up => up.PER_ID).HasColumnName("PER_ID");
        }
    }
}

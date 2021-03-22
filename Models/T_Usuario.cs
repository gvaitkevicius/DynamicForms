using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Areas.SGI.Model;
using DynamicForms.Context;
using DynamicForms.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DynamicForms.Models
{
    public class T_Usuario
    {
        public T_Usuario()
        {
            this.T_PlanoAcao = new HashSet<T_PlanoAcao>();
            this.T_UNIUSER = new HashSet<T_UNIUSER>();
            this.T_USER_GRUPO = new HashSet<T_USER_GRUPO>();
            this.T_Auditoria = new HashSet<T_Auditoria>();
            this.T_Favoritos = new HashSet<T_Favoritos>();
            this.TargetsProduto = new HashSet<TargetProduto>();
            this.T_Usuario_Perfil = new HashSet<T_Usuario_Perfil>();
            this.TesteFisico = new HashSet<TesteFisico>();
            this.LaudoTesteFisico = new HashSet<LaudoTesteFisico>();
            this.MovimentoEstoqueProducao = new HashSet<MovimentoEstoqueProducao>();
            this.MovimentoEstoqueVendas = new HashSet<MovimentoEstoqueVendas>();
            this.MovimentoEstoqueReservaDeEstoque = new HashSet<MovimentoEstoqueReservaDeEstoque>();
            this.MovimentoEstoqueTransferenciaSimples = new HashSet<MovimentoEstoqueTransferenciaSimples>();
            this.MovimentoEstoquePerdas = new HashSet<MovimentoEstoquePerdas>();
            this.MovimentoEstoqueEntradaInventario = new HashSet<MovimentoEstoqueEntradaInventario>();
            this.MovimentoEstoqueSaidaInventario = new HashSet<MovimentoEstoqueSaidaInventario>();
            this.MovimentoEstoqueConsumoMateriaPrima = new HashSet<MovimentoEstoqueConsumoMateriaPrima>();
            this.Etiquetas = new HashSet<Etiqueta>();
            this.T_PREFERENCIAS = new HashSet<T_PREFERENCIAS>();
            this.T_LOGS_DATABASE = new HashSet<T_LOGS_DATABASE>();
            this.T_USUARIO_OBJETO_CONTROLAVEL = new HashSet<T_USUARIO_OBJETO_CONTROLAVEL>();
        }

        [READ] [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo USE_ID requirido.")] public int USE_ID { get; set; }
        [SEARCH] [TAB(Value = "PRINCIPAL")] [Display(Name = "NOME")] [Required(ErrorMessage = "Campo USE_NOME requirido.")] [MaxLength(80, ErrorMessage = "Maximode 80 caracteres, campo USE_NOME")] public string USE_NOME { get; set; }
        [Email] [TAB(Value = "PRINCIPAL")] [Display(Name = "LOGIN/EMAIL")] [Required(ErrorMessage = "Campo LOGIN/EMAIL requirido.")]  [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo USE_EMAIL")] public string USE_EMAIL { get; set; }
        [Password] [TAB(Value = "PRINCIPAL")] [Display(Name = "SENHA")] [Required(ErrorMessage = "Campo USE_SENHA requirido.")] [MaxLength(50, ErrorMessage = "Maximode 50 caracteres, campo USE_SENHA")] public string USE_SENHA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD TURMA")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo TURM_ID")] public string TURM_ID { get; set; }
        [Checkbox(Description = "Ativo", TargetValue = "1")] [TAB(Value = "PRINCIPAL")] [Display(Name = "ATIVO")] [Required(ErrorMessage = "Campo USE_ATIVO requirido.")] public int USE_ATIVO { get; set; }
        [HIDDEN] [TAB(Value = "PRINCIPAL")] [Display(Name = "CODERP")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo USE_CODERP")] public string USE_CODERP { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        public Turma T_Turma { get; set; }

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (object obj in objects)
                {
                    if (obj.ToString() != typeof(T_Usuario).FullName)
                        continue;

                    T_Usuario usuario = (T_Usuario)obj;
                    string playActionUpper = usuario.PlayAction.ToUpper();

                    if (playActionUpper == "INSERT")
                    {
                        string hash_senha = UtilPlay.GetSha1(usuario.USE_SENHA);
                        usuario.USE_SENHA = hash_senha;
                    }
                    else if (playActionUpper == "UPDATE")
                    {
                        var cloneUsuario = cloneObjeto.GetClone(usuario);
                        var changedProperties = cloneObjeto.getChangedPoperties(usuario, cloneUsuario);

                        //se alterar a senha, gera uma nova hash, senao, mantem a senha do banco
                        if (changedProperties.Contains(nameof(USE_SENHA)))
                        {
                            string hash_senha = UtilPlay.GetSha1(usuario.USE_SENHA);
                            usuario.USE_SENHA = hash_senha;
                        }
                        else
                        {
                            usuario.USE_SENHA = ((T_Usuario)cloneUsuario).USE_SENHA;
                        }

                    }
                }

                return true;
            }

        }


        [HIDDENINTERFACE] public virtual ICollection<T_Auditoria> T_Auditoria { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<T_PlanoAcao> T_PlanoAcao { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<T_UNIUSER> T_UNIUSER { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<T_USER_GRUPO> T_USER_GRUPO { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<T_Favoritos> T_Favoritos { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<Feedback> T_Feedbacks { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<MovimentoEstoqueProducao> MovimentoEstoqueProducao { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<MovimentoEstoqueVendas> MovimentoEstoqueVendas { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<MovimentoEstoqueReservaDeEstoque> MovimentoEstoqueReservaDeEstoque { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<MovimentoEstoqueTransferenciaSimples> MovimentoEstoqueTransferenciaSimples { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<MovimentoEstoquePerdas> MovimentoEstoquePerdas { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<MovimentoEstoqueSaidaInventario> MovimentoEstoqueSaidaInventario { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<MovimentoEstoqueEntradaInventario> MovimentoEstoqueEntradaInventario { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<MovimentoEstoqueConsumoMateriaPrima> MovimentoEstoqueConsumoMateriaPrima { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<TargetProduto> TargetsProduto { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<Etiqueta> Etiquetas { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<TesteFisico> TesteFisico { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<LaudoTesteFisico> LaudoTesteFisico { get; set; }
        public virtual ICollection<T_Usuario_Perfil> T_Usuario_Perfil { get; set; }
        public virtual ICollection<T_USUARIO_OBJETO_CONTROLAVEL> T_USUARIO_OBJETO_CONTROLAVEL { get; set; }
        public virtual ICollection<T_PREFERENCIAS> T_PREFERENCIAS { get; set; }
        public virtual ICollection<T_LOGS_DATABASE> T_LOGS_DATABASE { get; set; }
    }

    public class T_Usuario_ResultConfiguration : IEntityTypeConfiguration<T_Usuario>
    {
        public void Configure(EntityTypeBuilder<T_Usuario> builder)
        {
            builder.ToTable("T_USUARIO");
            builder.HasKey(x => x.USE_ID);
            builder.Property(x => x.USE_ID).HasColumnName("USE_ID").IsRequired();
            builder.Property(x => x.USE_NOME).HasColumnName("USE_NOME").HasMaxLength(80).IsRequired();
            builder.Property(x => x.USE_EMAIL).HasColumnName("USE_EMAIL").HasMaxLength(100).IsRequired();
            builder.Property(x => x.USE_SENHA).HasColumnName("USE_SENHA").HasMaxLength(50).IsRequired();
            builder.Property(x => x.TURM_ID).HasColumnName("TURM_ID").HasMaxLength(10);
            builder.Property(x => x.USE_ATIVO).HasColumnName("USE_ATIVO").IsRequired();
            builder.Property(x => x.USE_CODERP).HasColumnName("USE_CODERP").HasMaxLength(200);
            builder.HasOne(u => u.T_Turma).WithMany(t => t.Usuarios).HasForeignKey(u => u.TURM_ID);
        }
    }
}

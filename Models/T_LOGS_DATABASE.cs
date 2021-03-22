
using DynamicForms.Context;
using DynamicForms.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DynamicForms.Models
{
    public class T_LOGS_DATABASE
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [READ] public int LOGS_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LOGS_TABLE")] [Required(ErrorMessage = "Campo LOGS_TABLE requirido.")] [MaxLength(50, ErrorMessage = "Maximo de 50 caracteres, campo LOGS_TABLE")] public string LOGS_TABLE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "KEY")] [Required(ErrorMessage = "Campo LOGS_KEY requirido.")] [MaxLength(100, ErrorMessage = "Maximo de 100 caracteres, campo LOGS_KEY")] public string LOGS_KEY { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COLUMN")] [Required(ErrorMessage = "Campo LOGS_COLUMN requirido.")] [MaxLength(100, ErrorMessage = "Maximo de 100 caracteres, campo LOGS_COLUMN")] public string LOGS_COLUMN { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "BEFORE")] [MaxLength(3500, ErrorMessage = "Maximo de * caracteres, campo LOGS_BEFORE")] public string LOGS_BEFORE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "AFTER")] [MaxLength(3500, ErrorMessage = "Maximo de * caracteres, campo LOGS_AFTER")] public string LOGS_AFTER { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ACTION")] [Required(ErrorMessage = "Campo LOGS_ACTION requirido.")] [MaxLength(30, ErrorMessage = "Maximo de 30 caracteres, campo LOGS_ACTION")] public string LOGS_ACTION { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATE")] [Required(ErrorMessage = "Campo LOGS_DATE requirido.")] public DateTime LOGS_DATE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORIGEM")] public string LOGS_ORIGEM { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID DO USUÁRIO")] [Required(ErrorMessage = "Campo USE_ID requirido.")] public int USE_ID { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        public T_Usuario T_Usuario { get; set; }

        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  } 

        [HIDDEN]
        public List<T_LOGS_DATABASE> GerarLogsAlteracoes(List<object> objects, ref CloneObjeto cloneObjeto, T_Usuario usuario)
        {
            List<T_LOGS_DATABASE> logsDatabase = new List<T_LOGS_DATABASE>();
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                int? indexClone;
                object obj2;
                List<string> camposAlterados;
                string valorAntes = "";
                string valorDepois = "";
                string valor = "";
                string conlumnName = "";
                string nameProperty = "";
                string playAction = ""; ;
                T_LOGS_DATABASE logDatabase;
                IEntityType entityType;
                StringBuilder stringBuilder;
                foreach (object objeto in objects)
                {
                    dynamic obj1 = objeto;
                    indexClone = obj1.IndexClone;
                    playAction = obj1.PlayAction.ToUpper();
                    obj2 = cloneObjeto.GetClone(obj1);

                    entityType = db.Model.FindEntityType(objeto.GetType());

                    string[] primaryKey = UtilPlay.GetPrimaryKey(objeto.GetType().FullName, db);
                    stringBuilder = new StringBuilder();
                    for (int i = 0; i < primaryKey.Length; i++)
                    {
                        nameProperty = primaryKey[i].Split(".")[1];
                        conlumnName = entityType.FindProperty(nameProperty).Relational().ColumnName;
                        valor = objeto.GetType().GetProperty(nameProperty).GetValue(objeto).ToString();
                        if (i == primaryKey.Length - 1)
                        {
                            stringBuilder.Append($"{conlumnName}: {valor}");
                        }
                        else
                        {
                            stringBuilder.Append($"{conlumnName}: {valor}, ");
                        }
                    }

                    if (playAction == "UPDATE")
                    {

                        camposAlterados = cloneObjeto.getChangedPoperties(obj1, obj2);
                        foreach (string prop in camposAlterados)
                        {
                            logDatabase = new T_LOGS_DATABASE();
                            logDatabase.LOGS_TABLE = entityType.Relational().TableName;
                            logDatabase.LOGS_KEY = stringBuilder.ToString();

                            conlumnName = entityType.FindProperty(prop).Relational().ColumnName;
                            valorAntes = (obj2.GetType().GetProperty(prop).GetValue(obj2) != null) ?
                                            obj2.GetType().GetProperty(prop).GetValue(obj2).ToString() : null;
                            valorDepois = (obj1.GetType().GetProperty(prop).GetValue(obj1) != null) ?
                                            obj1.GetType().GetProperty(prop).GetValue(obj1).ToString() : null;

                            logDatabase.LOGS_COLUMN = conlumnName;
                            logDatabase.LOGS_BEFORE = valorAntes;
                            logDatabase.LOGS_AFTER = valorDepois;
                            logDatabase.LOGS_ACTION = obj1.PlayAction.ToUpper();
                            logDatabase.USE_ID = (usuario != null) ? usuario.USE_ID : 27;
                            logDatabase.LOGS_DATE = DateTime.Now;
                            logDatabase.LOGS_ORIGEM = entityType.Name;
                            logDatabase.PlayAction = "insert";

                            logsDatabase.Add(logDatabase);
                        }
                    }
                    else if (playAction == "DELETE")
                    {
                        logDatabase = new T_LOGS_DATABASE();
                        logDatabase.LOGS_TABLE = entityType.Relational().TableName;
                        logDatabase.LOGS_KEY = stringBuilder.ToString();

                        logDatabase.LOGS_COLUMN = null;
                        logDatabase.LOGS_BEFORE = null;
                        logDatabase.LOGS_AFTER = null;
                        logDatabase.LOGS_ACTION = obj1.PlayAction.ToUpper();
                        logDatabase.USE_ID = (usuario != null) ? usuario.USE_ID : 27;
                        logDatabase.LOGS_DATE = DateTime.Now;
                        logDatabase.PlayAction = "insert";

                        logsDatabase.Add(logDatabase);
                    }

                }
            }
            return logsDatabase;
        }
    }

    public class T_LOGS_DATABASEMap : IEntityTypeConfiguration<T_LOGS_DATABASE>
    {
        public void Configure(EntityTypeBuilder<T_LOGS_DATABASE> builder)
        {
            builder.ToTable("T_LOGS_DATABASE");
            builder.HasKey(x => x.LOGS_ID);
            builder.Property(x => x.LOGS_ID).HasColumnName("LOGS_ID").IsRequired();
            builder.Property(x => x.LOGS_TABLE).HasColumnName("LOGS_TABLE").HasMaxLength(50).IsRequired();
            builder.Property(x => x.LOGS_KEY).HasColumnName("LOGS_KEY").HasMaxLength(100).IsRequired();
            builder.Property(x => x.LOGS_COLUMN).HasColumnName("LOGS_COLUMN").HasMaxLength(100).IsRequired();
            builder.Property(x => x.LOGS_BEFORE).HasColumnName("LOGS_BEFORE").HasMaxLength(3500).IsRequired();
            builder.Property(x => x.LOGS_AFTER).HasColumnName("LOGS_AFTER").HasMaxLength(3500).IsRequired();
            builder.Property(x => x.LOGS_ACTION).HasColumnName("LOGS_ACTION").HasMaxLength(30).IsRequired();
            builder.Property(x => x.LOGS_DATE).HasColumnName("LOGS_DATE").IsRequired();
            builder.Property(x => x.LOGS_ORIGEM).HasColumnName("LOGS_ORIGEM").IsRequired();
            builder.Property(x => x.USE_ID).HasColumnName("USE_ID").IsRequired();
            builder.HasOne(x => x.T_Usuario).WithMany(u => u.T_LOGS_DATABASE).HasForeignKey(x => x.USE_ID);
        }
    }
}

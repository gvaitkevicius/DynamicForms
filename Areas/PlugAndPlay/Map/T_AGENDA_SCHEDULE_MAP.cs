using DynamicForms.Areas.PlugAndPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Areas.PlugAndPlay.Map.Schedule
{
    public class T_AGENDA_SCHEDULE_MAP : IEntityTypeConfiguration<T_AGENDA_SCHEDULE>
    {

        public void Configure(EntityTypeBuilder<T_AGENDA_SCHEDULE> builder)
        {
            builder.ToTable("T_AGENDA_SCHEDULE");
            builder.HasKey(x => x.AGE_ID);
            builder.Property(x => x.AGE_ID).HasColumnName("AGE_ID").IsRequired();
            builder.Property(x => x.AGE_DATA_ESPECIFICA).HasColumnName("AGE_DATA_ESPECIFICA");
            builder.Property(x => x.AGE_HORARIO_INICIO).HasColumnName("AGE_HORARIO_INICIO");
            builder.Property(x => x.AGE_HORARIO_FIM).HasColumnName("AGE_HORARIO_FIM");
            builder.Property(x => x.AGE_SEGUNDA).HasColumnName("AGE_SEGUNDA").HasMaxLength(10);
            builder.Property(x => x.AGE_TERCA).HasColumnName("AGE_TERCA").HasMaxLength(10);
            builder.Property(x => x.AGE_QUARTA).HasColumnName("AGE_QUARTA").HasMaxLength(10);
            builder.Property(x => x.AGE_QUINTA).HasColumnName("AGE_QUINTA").HasMaxLength(10);
            builder.Property(x => x.AGE_SEXTA).HasColumnName("AGE_SEXTA").HasMaxLength(10);
            builder.Property(x => x.AGE_SABADO).HasColumnName("AGE_SABADO").HasMaxLength(10);
            builder.Property(x => x.AGE_DOMINGO).HasColumnName("AGE_DOMINGO").HasMaxLength(10);
            builder.Property(x => x.AGE_INTERVALO).HasColumnName("AGE_INTERVALO");
            builder.Property(x => x.AGE_ORDEM_EXECUCAO).HasColumnName("AGE_ORDEM_EXECUCAO").HasMaxLength(50);
            builder.Property(x => x.AGE_PARAMETROS).HasColumnName("AGE_PARAMETROS").HasMaxLength(500);
            builder.Property(x => x.AGE_EXCECAO).HasColumnName("AGE_EXCECAO").HasMaxLength(10);
            builder.Property(x => x.AGE_DESCRICAO).HasColumnName("AGE_DESCRICAO").HasMaxLength(100);
        }
    }
}

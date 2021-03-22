using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Models;
using DynamicForms.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class MovimentoEstoqueVendas : MovimentoEstoqueAbstrata
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PRODUTO")] [Required(ErrorMessage = "Campo PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE")] [Required(ErrorMessage = "Campo MOV_QUANTIDADE requirido.")] public double MOV_QUANTIDADE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NÚM DOCUMENTO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MOV_DOC")] public string MOV_DOC { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LOTE")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MOV_LOTE")] public string MOV_LOTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SUB LOTE")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_SUB_LOTE")] public string MOV_SUB_LOTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PEDIDO")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ TRANFORMAÇÃO")] public int? FPR_SEQ_TRANFORMACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ REPETIÇÃO")] public int? FPR_SEQ_REPETICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD OCORÊNCIA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo OCO_ID")] public string OCO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OBSERVAÇÃO")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo MOV_OBS")] public string MOV_OBS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ARMAZÉM")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_ARMAZEM")] public string MOV_ARMAZEM { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ENDEREÇO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_ENDERECO")] public string MOV_ENDERECO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OBSERVAÇÃO OP PARCIAL")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo MOV_OBS_OP_PARCIAL")] public string MOV_OBS_OP_PARCIAL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD OCORRÊNCIA OP PARCIAL")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_OCO_ID_OP_PARCIAL")] public string MOV_OCO_ID_OP_PARCIAL { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "USUÁRIO")] public int? USE_ID { get; set; }
        [NotMapped] public T_Usuario UsuarioLogado { get; set; }
        public virtual TipoMovSaidaVendas TipoMovSaidaVendas { get; set; }
        public virtual OcorrenciaVendas OcorrenciaVendas { get; set; }
        public OcorrenciaProducaoParciais OcorrenciaProducaoParciais { get; set; }
        public virtual Produto Produto { get; set; }
        public virtual Order Order { get; set; }
        public virtual Carga Carga { get; set; }
        public virtual Maquina Maquina { get; set; }
        public virtual Turno Turno { get; set; }
        public virtual Turma Turma { get; set; }
        public virtual T_Usuario Usuario { get; set; }

        [HIDDEN]
        public bool ValidarSeEhEstornoRomaneio(List<object> objects, string carId)
        {
            IEnumerable<MovimentoEstoqueVendas> movimentosEstornoRomaneio = objects.Where(mv => mv.GetType().Name == nameof(MovimentoEstoqueVendas)).Cast<MovimentoEstoqueVendas>();

            bool existeMovEstornoRomaneio = movimentosEstornoRomaneio.Any(mv => mv.CAR_ID == carId && mv.MOV_ESTORNO == "E");

            return existeMovEstornoRomaneio;
        }

        public override bool ImprimirEtiqueta(List<object> objects, ref List<LogPlay> Logs)
        {
            bool check = true;
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (var item in objects)
                {
                    MovimentoEstoqueVendas mov = (MovimentoEstoqueVendas)item;
                    var Db_Etiqueta = db.Etiqueta.AsNoTracking().Where(x => x.ETI_LOTE == mov.MOV_LOTE && x.ETI_SUB_LOTE == mov.MOV_SUB_LOTE).Select(x => x.ETI_ID).FirstOrDefault();
                    if (Db_Etiqueta == 0)
                    {
                        Logs.Add(new LogPlay(this.ToString(), "ERRO", "Não existe uma etiqueta associada a este movimento de vendas"));
                    }
                    else
                    {
                        InterfaceTelaImpressaoEtiquetas et = new InterfaceTelaImpressaoEtiquetas();
                        Logs.Add(et.ImprimirEt($"{Db_Etiqueta}"));
                    }
                }
            }
            return check;
        }
    }
}

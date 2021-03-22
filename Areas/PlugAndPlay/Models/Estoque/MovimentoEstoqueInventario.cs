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
    [Display(Name = "SAÍDAS NO INVENTÁRIO")]
    public class MovimentoEstoqueSaidaInventario : MovimentoEstoqueAbstrata
    {
        [SEARCH_NOT_FK(namespaceOfForeignClass = "DynamicForms.Areas.PlugAndPlay.Models.Produto")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PRODUTO")] [Required(ErrorMessage = "Campo PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [SEARCH_NOT_FK(namespaceOfForeignClass = "DynamicForms.Areas.PlugAndPlay.Models.Order")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PEDIDO")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE")] [Required(ErrorMessage = "Campo MOV_QUANTIDADE requirido.")] public double MOV_QUANTIDADE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LOTE")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MOV_LOTE")] public string MOV_LOTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SUB LOTE")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_SUB_LOTE")] public string MOV_SUB_LOTE { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "COD OCORRÊNCIA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo OCO_ID")] public string OCO_ID { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "SEQ TRANFORMAÇÃO")] public int? FPR_SEQ_TRANFORMACAO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "SEQ REPETIÇÃO")] public int? FPR_SEQ_REPETICAO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "OBSERVAÇÕES")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo MOV_OBS")] public string MOV_OBS { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "ARMAZÉM")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_ARMAZEM")] public string MOV_ARMAZEM { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "ENDEREÇO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_ENDERECO")] public string MOV_ENDERECO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "OBSERVAÇÕES OP PARCIAL")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo MOV_OBS_OP_PARCIAL")] public string MOV_OBS_OP_PARCIAL { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "COD OCORRÊNCIA OP PARCIAL")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_OCO_ID_OP_PARCIAL")] public string MOV_OCO_ID_OP_PARCIAL { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "USUÁRIO")] public int? USE_ID { get; set; }
        [NotMapped] public T_Usuario UsuarioLogado { get; set; }
        public virtual TipoMovSaidaInventario TipoMovSaidaInventario { get; set; }
        public virtual OcorrenciaSaidaInventario OcorrenciaSaidaInventario { get; set; }
        public virtual OcorrenciaProducaoParciais OcorrenciaProducaoParciais { get; set; }
        public virtual Turno Turno { get; set; }
        public virtual Turma Turma { get; set; }
        public virtual Carga Carga { get; set; }
        public virtual Order Order { get; set; }
        public virtual Maquina Maquina { get; set; }
        public virtual T_Usuario Usuario { get; set; }
        public MovimentoEstoqueSaidaInventario()
        {

        }
        ///METODOS DE CLASSE
        ///
        public override bool ImprimirEtiqueta(List<object> objects, ref List<LogPlay> Logs)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (var item in objects)
                {
                    MovimentoEstoqueSaidaInventario movimentoEstoque = (MovimentoEstoqueSaidaInventario)item;
                    movimentoEstoque.PlayAction = "OK";
                
                    var etiquetaexistente = db.Etiqueta.AsNoTracking()
                            .Where(x => x.ETI_LOTE.Equals(movimentoEstoque.MOV_LOTE) && 
                                x.ETI_SUB_LOTE.Equals(movimentoEstoque.MOV_SUB_LOTE))
                            .OrderByDescending(x => x.ETI_EMISSAO)
                            .FirstOrDefault();

                    if (etiquetaexistente != null)
                    {
                        InterfaceTelaImpressaoEtiquetas et = new InterfaceTelaImpressaoEtiquetas();
                        Logs.Add(et.ImprimirEt($"{etiquetaexistente.ETI_ID}"));
                        etiquetaexistente.PlayAction = "OK";
                    }
                    else
                    {
                        Logs.Add(new LogPlay() { Status = "ERRO", MsgErro = "NÃO EXISTE UMA ETIQUETA GERADA PARA ESTE MOVIMENTO DE ESTOQUE." });
                        return false;
                    }
                }
            }
            return true;
        }

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            List<object> objetosParaPersistir = new List<object>();
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (var item in objects)
                {
                    if (item.GetType().Name != nameof(MovimentoEstoqueSaidaInventario))
                        continue;

                    // SAIDAS DE INVENTARIO

                    MovimentoEstoqueSaidaInventario saidaInventario = (MovimentoEstoqueSaidaInventario)item;

                    #region validações
                    if (String.IsNullOrEmpty(saidaInventario.PRO_ID?.Trim()))
                    {
                        saidaInventario.PlayMsgErroValidacao = "O produto deve ser informado.";
                        saidaInventario.PlayAction = "ERRO";
                        return false;
                    }
                    if (String.IsNullOrEmpty(saidaInventario.MOV_LOTE?.Trim()))
                    {
                        saidaInventario.PlayMsgErroValidacao = "O lote deve ser informado.";
                        saidaInventario.PlayAction = "ERRO";
                        return false;
                    }
                    if (String.IsNullOrEmpty(saidaInventario.MOV_SUB_LOTE?.Trim()))
                    {
                        saidaInventario.PlayMsgErroValidacao = "O sublote deve ser informado.";
                        saidaInventario.PlayAction = "ERRO";
                        return false;
                    }
                    if (saidaInventario.MOV_QUANTIDADE <= 0)
                    {
                        saidaInventario.PlayMsgErroValidacao = "A quantidade deve ser maior que zero.";
                        saidaInventario.PlayAction = "ERRO";
                        return false;
                    }

                    double saldo = db.SaldosEmEstoquePorLote.AsNoTracking()
                        .Where(x => x.PRO_ID == saidaInventario.PRO_ID && x.MOV_LOTE == saidaInventario.MOV_LOTE && x.MOV_SUB_LOTE == saidaInventario.MOV_SUB_LOTE)
                        .Select(x => x.SALDO)
                        .FirstOrDefault().Value;

                    if (saidaInventario.MOV_QUANTIDADE > saldo)
                    {
                        saidaInventario.PlayMsgErroValidacao = "A quantidade informada é maior que o saldo do palete.";
                        saidaInventario.PlayAction = "ERRO";
                        return false;
                    }
                    #endregion validações

                    #region Tratamento reserva de estoque
                    var movReserva = new MovimentoEstoqueReservaDeEstoque();
                    movReserva = movReserva.GetReserva(saidaInventario.MOV_LOTE, saidaInventario.MOV_SUB_LOTE, saidaInventario.PRO_ID);

                    if (movReserva.ORD_ID == null)
                    {
                        /* O ORD_ID não foi preenchido porque não foi encontrado etiqueta 
                         * para o produto, lote e sublote deste movimento de saída.
                         */

                        movReserva.ORD_ID = saidaInventario.ORD_ID;
                        movReserva.FPR_SEQ_REPETICAO = 1;
                    }

                    movReserva.MOV_QUANTIDADE = saidaInventario.MOV_QUANTIDADE;
                    movReserva.MOV_ENDERECO = saidaInventario.MOV_ENDERECO;

                    objetosParaPersistir.Add(movReserva);
                    #endregion Tratamento reserva de estoque

                    #region Tratamento movimento de saida
                    saidaInventario.MOV_DATA_HORA_CRIACAO = DateTime.Now;
                    saidaInventario.TIP_ID = "600";
                    saidaInventario.MAQ_ID = movReserva.MAQ_ID;
                    saidaInventario.ORD_ID = movReserva.ORD_ID;
                    saidaInventario.FPR_SEQ_REPETICAO = movReserva.FPR_SEQ_REPETICAO.Value;
                    saidaInventario.MOV_OBS = movReserva.MOV_OBS;
                    saidaInventario.MOV_ARMAZEM = movReserva.MOV_ARMAZEM;
                    saidaInventario.MOV_ENDERECO = movReserva.MOV_ENDERECO;
                    saidaInventario.MOV_OBS_OP_PARCIAL = movReserva.MOV_OBS_OP_PARCIAL;
                    saidaInventario.MOV_OCO_ID_OP_PARCIAL = movReserva.MOV_OCO_ID_OP_PARCIAL;
                    #endregion Tratamento movimento de saida
                }
                objects.AddRange(objetosParaPersistir);
            }
            return true;
        }

        public bool AfterChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert, JSgi db = null)
        {
            List<Etiqueta> etiquetas = new List<Etiqueta>();

            foreach (var item in objects)
            {
                if (item.GetType().Name != nameof(MovimentoEstoqueSaidaInventario))
                    continue;

                MovimentoEstoqueSaidaInventario mov = (MovimentoEstoqueSaidaInventario)item;

                // gerando etiqueta do lote
                var etiqueta = new Etiqueta()
                    .GerarEtiquetaLoteProduto(mov.PRO_ID, mov.MOV_LOTE, mov.MOV_SUB_LOTE, mov.FPR_SEQ_REPETICAO.Value, Logs, mov.UsuarioLogado.USE_ID);

                etiquetas.Add(etiqueta);
            }

            if (!Logs.Any(x => x.Status.Equals("ERRO")))
            {
                var Ids = String.Join(",", etiquetas.Select(x => x.ETI_ID).ToArray());
                Logs.Add(new LogPlay(this.ToString(), "PROTOCOLO", "LINK", "/PlugAndPlay/ReportEtiqueta/EtiquetaIndividualLoteProduto?etiqueta=", $"{Ids}"));
            }

            return true;
        }
    }

    [Display(Name = "ENTRADAS NO INVENTÁRIO")]
    public class MovimentoEstoqueEntradaInventario : MovimentoEstoqueAbstrata
    {
        [SEARCH_NOT_FK(namespaceOfForeignClass = "DynamicForms.Areas.PlugAndPlay.Models.Order")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PEDIDO")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [SEARCH_NOT_FK(namespaceOfForeignClass = "DynamicForms.Areas.PlugAndPlay.Models.Produto")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PRODUTO")] [Required(ErrorMessage = "Campo PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE")] [Required(ErrorMessage = "Campo MOV_QUANTIDADE requirido.")] public double MOV_QUANTIDADE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LOTE")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MOV_LOTE")] public string MOV_LOTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SUB LOTE")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_SUB_LOTE")] public string MOV_SUB_LOTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ENDEREÇO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_ENDERECO")] public string MOV_ENDERECO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OBSERVAÇÕES")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo MOV_OBS")] public string MOV_OBS { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "SEQ TRANFORMAÇÃO")] public int? FPR_SEQ_TRANFORMACAO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "SEQ REPETIÇÃO")] public int? FPR_SEQ_REPETICAO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "ARMAZÉM")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_ARMAZEM")] public string MOV_ARMAZEM { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "COD OCORRÊNCIA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo OCO_ID")] public string OCO_ID { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "OBSERVAÇÕES OP PARCIAL")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo MOV_OBS_OP_PARCIAL")] public string MOV_OBS_OP_PARCIAL { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "COD OCORRÊNCIA OP PARCIAL")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_OCO_ID_OP_PARCIAL")] public string MOV_OCO_ID_OP_PARCIAL { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "USUÁRIO")] public int? USE_ID { get; set; }
        [NotMapped] public T_Usuario UsuarioLogado { get; set; }
        public virtual TipoMovEntradaInventario TipoMovEntradaInventario { get; set; }
        public virtual OcorrenciaEntradaInventario OcorrenciaEntradaInventario { get; set; }
        public virtual OcorrenciaProducaoParciais OcorrenciaProducaoParciais { get; set; }
        public virtual Turno Turno { get; set; }
        public virtual Turma Turma { get; set; }
        public virtual Order Order { get; set; }
        public virtual Carga Carga { get; set; }
        public virtual Maquina Maquina { get; set; }
        public virtual T_Usuario Usuario { get; set; }


        public MovimentoEstoqueEntradaInventario()
        {
        }
        ///MÉTODOS DE CLASSE
        public override bool ImprimirEtiqueta(List<object> objects, ref List<LogPlay> Logs)
        {
            foreach (var item in objects)
            {
                MovimentoEstoqueEntradaInventario movimentoEstoque = (MovimentoEstoqueEntradaInventario)item;
                movimentoEstoque.PlayAction = "OK";
                using (var db = new ContextFactory().CreateDbContext(new string[] { }))
                {
                    var etiquetaexistente = db.Etiqueta.AsNoTracking()
                        .Where(x => x.ETI_LOTE.Equals(movimentoEstoque.MOV_LOTE) && x.ETI_SUB_LOTE.Equals(movimentoEstoque.MOV_SUB_LOTE))
                        .OrderByDescending(x => x.ETI_EMISSAO)
                        .FirstOrDefault();

                    if (etiquetaexistente != null)
                    {
                        InterfaceTelaImpressaoEtiquetas et = new InterfaceTelaImpressaoEtiquetas();
                        Logs.Add(et.ImprimirEt($"{etiquetaexistente.ETI_ID}"));
                        etiquetaexistente.PlayAction = "OK";
                    }
                    else
                    {
                        Logs.Add(new LogPlay() { Status = "ERRO", MsgErro = "NÃO EXISTE UMA ETIQUETA GERADA PARA ESTE MOVIMENTO DE ESTOQUE." });
                        return false;
                    }
                }
            }
            return true;
        }
        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            List<object> objetosParaPersistir = new List<object>();
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (var item in objects)
                {
                    if (item.GetType().Name == nameof(MovimentoEstoqueEntradaInventario))
                        continue;

                    // ENTRADAS DE INVENTARIO

                    MovimentoEstoqueEntradaInventario entradaInventario = (MovimentoEstoqueEntradaInventario)item;
                   
                    #region Validacoes
                    if (String.IsNullOrEmpty(entradaInventario.PRO_ID?.Trim()))
                    {
                        entradaInventario.PlayMsgErroValidacao = "O produto deve ser informado.";
                        entradaInventario.PlayAction = "ERRO";
                        return false;
                    }
                    if (String.IsNullOrEmpty(entradaInventario.MOV_LOTE?.Trim()))
                    {
                        entradaInventario.PlayMsgErroValidacao = "O lote deve ser informado.";
                        entradaInventario.PlayAction = "ERRO";
                        return false;
                    }
                    if (String.IsNullOrEmpty(entradaInventario.MOV_SUB_LOTE?.Trim()))
                    {
                        entradaInventario.PlayMsgErroValidacao = "O sublote deve ser informado.";
                        entradaInventario.PlayAction = "ERRO";
                        return false;
                    }
                    if (entradaInventario.MOV_QUANTIDADE <= 0)
                    {
                        entradaInventario.PlayMsgErroValidacao = "A quantidade informada deve ser maior que zero.";
                        entradaInventario.PlayAction = "ERRO";
                        return false;
                    }
                    #endregion Validacoes

                    #region Tratamento reserva de estoque
                    var movReserva = new MovimentoEstoqueReservaDeEstoque();
                    movReserva = movReserva.GetReserva(entradaInventario.MOV_LOTE, entradaInventario.MOV_SUB_LOTE, entradaInventario.PRO_ID);
                    if (movReserva.ORD_ID == null || movReserva.ORD_ID.Trim() == "")
                    {
                        /* O ORD_ID não foi preenchido porque não foi encontrado etiqueta 
                         * para o produto, lote e sublote deste movimento de entrada.
                         */
                        movReserva.ORD_ID = entradaInventario.ORD_ID;
                        movReserva.FPR_SEQ_REPETICAO = 1;
                    }

                    movReserva.MOV_QUANTIDADE = entradaInventario.MOV_QUANTIDADE;
                    movReserva.MOV_ENDERECO = entradaInventario.MOV_ENDERECO; 

                    objetosParaPersistir.Add(movReserva);
                    #endregion Tratamento reserva de estoque

                    #region Tratamento movimento de entrada
                    entradaInventario.TIP_ID = "300";
                    entradaInventario.MOV_DATA_HORA_CRIACAO = DateTime.Now;
                    entradaInventario.FPR_SEQ_REPETICAO = movReserva.FPR_SEQ_REPETICAO;
                    entradaInventario.MOV_OBS = movReserva.MOV_OBS;
                    entradaInventario.MOV_ARMAZEM = movReserva.MOV_ARMAZEM ?? ParametrosSingleton.Instance.Armazem;
                    entradaInventario.MOV_ENDERECO = movReserva.MOV_ENDERECO;
                    entradaInventario.MOV_OBS_OP_PARCIAL = movReserva.MOV_OBS_OP_PARCIAL;
                    entradaInventario.MOV_OCO_ID_OP_PARCIAL = movReserva.MOV_OCO_ID_OP_PARCIAL;
                    #endregion Tratamento movimento de entrada
                }
                objects.AddRange(objetosParaPersistir);
            }
            return true;
        }
        public bool AfterChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert, JSgi db = null)
        {
            List<Etiqueta> etiquetas = new List<Etiqueta>();

            foreach (var item in objects)
            {
                if (item.GetType().Name == "MovimentoEstoqueEntradaInventario")
                {
                    MovimentoEstoqueEntradaInventario mov = (MovimentoEstoqueEntradaInventario)item;

                    // gerando etiqueta do lote
                    var etiqueta = new Etiqueta()
                        .GerarEtiquetaLoteProduto(mov.PRO_ID, mov.MOV_LOTE, mov.MOV_SUB_LOTE, mov.FPR_SEQ_REPETICAO.Value, Logs, mov.UsuarioLogado.USE_ID);

                    etiquetas.Add(etiqueta);
                }
            }

            if (!Logs.Any(x => x.Status.Equals("ERRO")))
            {
                var Ids = String.Join(",", etiquetas.Select(x => x.ETI_ID).ToArray());
                Logs.Add(new LogPlay(this.ToString(), "PROTOCOLO", "LINK", "/PlugAndPlay/ReportEtiqueta/EtiquetaIndividualLoteProduto?etiqueta=", $"{Ids}"));
            }

            return true;
        }

    }

}

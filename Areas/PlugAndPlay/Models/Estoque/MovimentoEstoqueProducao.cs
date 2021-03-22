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
    [Display(Name = "PRODUÇÃO")]
    public class MovimentoEstoqueProducao : MovimentoEstoqueAbstrata
    {
        public MovimentoEstoqueProducao()
        {

        }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PRODUTO")] [Required(ErrorMessage = "Campo PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE")] [Required(ErrorMessage = "Campo MOV_QUANTIDADE requirido.")] public double MOV_QUANTIDADE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PESO UNT.")] [Required(ErrorMessage = "Campo MOV_QUANTIDADE requirido.")] public double MOV_PESO_UNITARIO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LOTE")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MOV_LOTE")] public string MOV_LOTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SUB LOTE")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_SUB_LOTE")] public string MOV_SUB_LOTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PEDIDO")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ TRANFORMAÇÃO")] public int? FPR_SEQ_TRANFORMACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ REPETIÇÃO")] public int? FPR_SEQ_REPETICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD OCORRÊNCIA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo OCO_ID")] public string OCO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OBSERVAÇÕES")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo MOV_OBS")] public string MOV_OBS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ARMAZÉM")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_ARMAZEM")] public string MOV_ARMAZEM { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ENDEREÇO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_ENDERECO")] public string MOV_ENDERECO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OBSERVAÇÕES OP PARCIAL")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo MOV_OBS_OP_PARCIAL")] public string MOV_OBS_OP_PARCIAL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD OCORRÊNCIA OP PARCIAL")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_OCO_ID_OP_PARCIAL")] public string MOV_OCO_ID_OP_PARCIAL { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "USUÁRIO")] public int? USE_ID { get; set; }
        [NotMapped] public T_Usuario UsuarioLogado { get; set; }
        public virtual T_Usuario Usuario { get; set; }
        public virtual TipoMovEntradaProducao TipoMovEntradaProducao { get; set; }
        public virtual Produto Produto { get; set; }
        public virtual Order Order { get; set; }
        public virtual Carga Carga { get; set; }
        public virtual Turno Turno { get; set; }
        public virtual Turma Turma { get; set; }
        public virtual OcorrenciaProducao OcorrenciaProducao { get; set; }
        public virtual OcorrenciaProducaoParciais OcorrenciaProducaoParciais { get; set; }

        public override bool ImprimirEtiqueta(List<object> objects, ref List<LogPlay> Logs)
        {
            bool check = true;
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (var item in objects)
                {
                    MovimentoEstoqueProducao producao = (MovimentoEstoqueProducao)item;
                    var Db_Etiqueta = db.Etiqueta.AsNoTracking().Where(x => x.ETI_LOTE == producao.MOV_LOTE && x.ETI_SUB_LOTE == producao.MOV_SUB_LOTE).Select(x => x.ETI_ID).FirstOrDefault();
                    if (Db_Etiqueta == 0)
                    {
                        Logs.Add(new LogPlay(this.ToString(), "ERRO", "Não existe uma etiqueta associada a este movimento de produção"));
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
        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            List<object> listAux = new List<object>();
            using (var db = new ContextFactory().CreateDbContext(Array.Empty<string>()))
            {
                foreach (var item in objects)
                {
                    if (item.GetType().Name != nameof(MovimentoEstoqueProducao))
                        continue;

                    // APONTAMENTO DE PRODUÇÃO - PARTE 02

                    MovimentoEstoqueProducao movApProducao = (MovimentoEstoqueProducao)item;

                    #region validações movimento de ap. produção
                    if (movApProducao.MOV_QUANTIDADE <= 0)
                    {
                        movApProducao.PlayMsgErroValidacao = "Quantidade não pode ser menor ou igual a zero";
                        return false;
                    }
                    
                    bool existeMovApProd = db.MovimentoEstoqueProducao.Any(x => x.MOV_LOTE == movApProducao.MOV_LOTE &&
                        x.MOV_SUB_LOTE == movApProducao.MOV_SUB_LOTE && x.PRO_ID == movApProducao.PRO_ID && x.MOV_ESTORNO != "E");
                    
                    if (existeMovApProd)
                    {
                        movApProducao.PlayMsgErroValidacao = "Codigo de barras duplicado.";
                        return false;
                    }

                    var pedido = db.Order.AsNoTracking().Where(o => o.ORD_ID == movApProducao.ORD_ID)
                        .Select(o => new Order 
                        {
                            ORD_TIPO = o.ORD_TIPO,
                            ORD_PESO_UNITARIO = o.ORD_PESO_UNITARIO,
                            ORD_PESO_UNITARIO_BRUTO = o.ORD_PESO_UNITARIO_BRUTO,
                            PRO_ID = o.PRO_ID

                        }).FirstOrDefault();

                    if (pedido.ORD_TIPO >= 3)
                    {
                        movApProducao.PlayAction = $"Você não pode apontar uma produção para pedidos do tipo {pedido.ORD_TIPO}";
                        return false;
                    }

                    #endregion validações movimento de ap. produção

                    Etiqueta etiqueta = null;
                    
                    #region Verificando se existe a etique na lista de objetos
                    var listEtiquetas = objects.Where(obj => obj.GetType().Name == nameof(Etiqueta)).Cast<Etiqueta>();
                    
                    etiqueta = listEtiquetas.Where(e => e.ETI_LOTE == movApProducao.MOV_LOTE &&
                        e.ETI_SUB_LOTE == movApProducao.MOV_SUB_LOTE && e.ROT_PRO_ID == movApProducao.PRO_ID)
                        .FirstOrDefault();
                    #endregion Verificando se existe a etique na lista de objetos

                    if (etiqueta == null)
                    {// A etiqueta não foi encontrada na lista de objetos recebida, portanto será consultada na base de dados
                        etiqueta = db.Etiqueta.AsNoTracking().Where(e => e.ETI_LOTE == movApProducao.MOV_LOTE && 
                            e.ETI_SUB_LOTE == movApProducao.MOV_SUB_LOTE && e.ROT_PRO_ID == movApProducao.PRO_ID)
                            .FirstOrDefault();
                    }

                    if (etiqueta != null)
                    {
                        var movReserva = new MovimentoEstoqueReservaDeEstoque();
                        movReserva = movReserva.GetReserva(movApProducao.MOV_LOTE, movApProducao.MOV_SUB_LOTE, movApProducao.PRO_ID, etiqueta);
                        movReserva.USE_ID = movApProducao.UsuarioLogado.USE_ID;
                        movReserva.MOV_APROVEITAMENTO = "N";
                        movReserva.MOV_ENDERECO = "PRO";
                        movReserva.MOV_QUANTIDADE = movApProducao.MOV_QUANTIDADE;
                        movReserva.MOV_PESO_UNITARIO = movApProducao.PRO_ID == pedido.PRO_ID ? 
                            pedido.ORD_PESO_UNITARIO ?? 0 : pedido.ORD_PESO_UNITARIO_BRUTO ?? 0;
                        
                        listAux.Add(movReserva);
                    }
                    else
                    {
                        movApProducao.PlayMsgErroValidacao = "Etiqueta não encontrada.";
                        return false;
                    }

                }
                objects.AddRange(listAux);
            }
            return true;
        }
    }

    [Display(Name = "APONTAMENTO DE CAIXAS E ACESSÓRIOS")]
    public class ProducaoCodigoBarras : InterfaceDeTelas
    {

        [TAB(Value = "PRINCIPAL")]
        [SEARCH] public string CodigoDeBarras { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        [NotMapped] public T_Usuario UsuarioLogado { get; set; }
        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                List<object> objetosParaPersistir = new List<object>();

                foreach (var item in objects)
                {
                    if (item.GetType().Name != nameof(ProducaoCodigoBarras))
                        continue;

                    // APONTAMENTO DE PRODUÇÃO - PARTE 01

                    ProducaoCodigoBarras CodigosBarras = (ProducaoCodigoBarras)item;
                    if (String.IsNullOrEmpty(CodigosBarras.CodigoDeBarras?.Trim()))
                    {
                        CodigosBarras.PlayAction = "ERRO";
                        CodigosBarras.PlayMsgErroValidacao = "Você deve informar um código de barras válido";
                        return false;
                    }

                    Etiqueta etiqueta = db.Etiqueta.AsNoTracking()
                        .Where(x => x.ETI_CODIGO_BARRAS.Trim() == CodigosBarras.CodigoDeBarras.Trim()).FirstOrDefault();

                    if (etiqueta != null)
                    {
                        //verifica se o palete foi apontado
                        var palete_apontado = db.SaldosEmEstoquePorLote.Where(x =>
                            x.MOV_LOTE == etiqueta.ETI_LOTE &&
                            x.MOV_SUB_LOTE == etiqueta.ETI_SUB_LOTE &&
                            x.PRO_ID == etiqueta.ROT_PRO_ID
                        ).FirstOrDefault();

                        if (palete_apontado != null)
                        {
                            CodigosBarras.PlayAction = "ERRO";
                            CodigosBarras.PlayMsgErroValidacao = "O palete dessa caixa/acessório já foi apontado.";
                            return false;
                        }
                    }
                    else //etiqueta não existe
                    {
                        CodigosBarras.PlayAction = "ERRO";
                        CodigosBarras.PlayMsgErroValidacao = "Etiqueta não Existe Verifique o código informado";
                        return false;
                    }

                    var saldoEmEstoque = db.SaldosEmEstoquePorLote.AsNoTracking()
                        .Where(x => x.ORD_ID == etiqueta.ORD_ID && x.PRO_ID == etiqueta.ROT_PRO_ID)
                        .GroupBy(x => new { x.ORD_ID, x.MOV_ENDERECO, x.PRO_ID })
                        .Select(c => new
                        {
                            ORD_ID = c.Key.ORD_ID,
                            MOV_ENDERECO = c.Key.MOV_ENDERECO,
                            PRO_ID = c.Key.PRO_ID,
                            QTD = c.Count(),
                            SALDO = c.Sum(a => a.SALDO)
                        })
                        .ToList();

                    Order pedido = db.Order.AsNoTracking().Where(x => x.ORD_ID == etiqueta.ORD_ID).FirstOrDefault();
                    double qtdMaxima = pedido.ORD_QUANTIDADE + (pedido.ORD_QUANTIDADE * ((pedido.ORD_TOLERANCIA_MAIS ?? 0) / 100));

                    double somatorio = saldoEmEstoque.Sum(x => x.SALDO).Value + etiqueta.ETI_QUANTIDADE_PALETE.Value;
                    string mensagem = "  SALDO: " + somatorio + "/" + qtdMaxima;

                    mensagem += "   ENDEREÇO(S): ";
                    foreach (var o in saldoEmEstoque)
                    {
                        mensagem += o.QTD + " " + o.MOV_ENDERECO + ",   ";
                    }

                    CodigosBarras.PlayMsgErroValidacao = mensagem;
                    CodigosBarras.PlayAction = "ALERT";

                    etiqueta.ETI_STATUS = "P";
                    etiqueta.PlayAction = "update";
                    objetosParaPersistir.Add(etiqueta);

                    objetosParaPersistir.Add(new MovimentoEstoqueProducao()
                    {
                        ORD_ID = etiqueta.ORD_ID,
                        MOV_QUANTIDADE = (double)etiqueta.ETI_QUANTIDADE_PALETE,
                        MOV_DATA_HORA_EMISSAO = DateTime.Now,//ParametrosSingleton.Instance.DataBase,
                        MOV_DATA_HORA_CRIACAO = DateTime.Now,
                        MOV_DIA_TURMA = ParametrosSingleton.DiaTurmaS(),
                        MOV_LOTE = etiqueta.ETI_LOTE,
                        MOV_SUB_LOTE = etiqueta.ETI_SUB_LOTE,
                        MAQ_ID = etiqueta.MAQ_ID,
                        USE_ID = CodigosBarras.UsuarioLogado.USE_ID,
                        PRO_ID = etiqueta.ROT_PRO_ID,
                        FPR_SEQ_REPETICAO = (int)etiqueta.FPR_SEQ_REPETICAO,
                        TIP_ID = "001",
                        MOV_ARMAZEM = ParametrosSingleton.Instance.Armazem,
                        MOV_ENDERECO = "PRO",
                        TURM_ID = CodigosBarras.UsuarioLogado.TURM_ID,
                        TURN_ID = null,//-- pendencia
                        CAR_ID = null,//-- pendencia
                        PlayMsgErroValidacao = "",
                        PlayAction = "insert",
                        UsuarioLogado = CodigosBarras.UsuarioLogado
                    });
                }

                objects.AddRange(objetosParaPersistir);
            }
            return true;
        }
    }

    [Display(Name = "APONTAMENTO DE CHAPAS")]
    public class ProducaoCodBar_Quantidade : InterfaceDeTelas
    {
        [TAB(Value = "PRINCIPAL")]
        [SEARCH]
        [Display(Name = "CÓDIGO DE BARRAS")]
        public string CodigoDeBarras { get; set; }

        [TAB(Value = "PRINCIPAL")]
        public double Quantidade { get; set; }
        [NotMapped] public T_Usuario UsuarioLogado { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            // monta movimento de apontamento a partir do codigo de barras lido 
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                List<object> objetosParaPersistir = new List<object>();
                foreach (var item in objects)
                {
                    ProducaoCodBar_Quantidade CodigosBarras = (ProducaoCodBar_Quantidade)item;
                    if (String.IsNullOrEmpty(CodigosBarras.CodigoDeBarras?.Trim()))
                    {
                        CodigosBarras.PlayAction = "ERRO";
                        CodigosBarras.PlayMsgErroValidacao = "Informe um código de barras válido.";
                        return false;
                    }
                    
                    if (CodigosBarras.Quantidade <= 0)
                    {
                        CodigosBarras.PlayAction = "ERRO";
                        CodigosBarras.PlayMsgErroValidacao = "A quantidade do palete deve ser maior que zero.";
                        return false;
                    }

                    T_Usuario usuarioDb = CodigosBarras.UsuarioLogado;
                    Etiqueta etiqueta = db.Etiqueta.Where(x => x.ETI_CODIGO_BARRAS == CodigosBarras.CodigoDeBarras).FirstOrDefault();

                    if (etiqueta != null)
                    {
                        //verifica se o palete foi apontado
                        var palete_apontado = db.SaldosEmEstoquePorLote.Where(x =>
                            x.MOV_LOTE == etiqueta.ETI_LOTE &&
                            x.MOV_SUB_LOTE == etiqueta.ETI_SUB_LOTE &&
                            x.PRO_ID == etiqueta.ROT_PRO_ID
                        ).FirstOrDefault();

                        if (palete_apontado != null)
                        {
                            CodigosBarras.PlayAction = "ERRO";
                            CodigosBarras.PlayMsgErroValidacao = "O palete de chapa já foi apontado.";
                            return false;
                        }
                    }
                    else //etiqueta não existe
                    {
                        CodigosBarras.PlayAction = "ERRO";
                        CodigosBarras.PlayMsgErroValidacao = "Etiqueta não Existe Verifique o código informado";
                        return false;
                    }

                    var saldoEmEstoque = db.SaldosEmEstoquePorLote.AsNoTracking()
                        .Where(x => x.ORD_ID == etiqueta.ORD_ID && x.PRO_ID == etiqueta.ROT_PRO_ID)
                        .GroupBy(x => new { x.ORD_ID, x.MOV_ENDERECO, x.PRO_ID })
                        .Select(c => new
                        {
                            ORD_ID = c.Key.ORD_ID,
                            MOV_ENDERECO = c.Key.MOV_ENDERECO,
                            PRO_ID = c.Key.PRO_ID,
                            QTD = c.Count(),
                            SALDO = c.Sum(a => a.SALDO)
                        })
                        .ToList();

                    Order pedido = db.Order.AsNoTracking().Where(x => x.ORD_ID == etiqueta.ORD_ID).FirstOrDefault();
                    double qtdMaxima = pedido.ORD_QUANTIDADE + (pedido.ORD_QUANTIDADE * ((pedido.ORD_TOLERANCIA_MAIS ?? 0) / 100));

                    double somatorio = saldoEmEstoque.Sum(x => x.SALDO).Value + etiqueta.ETI_QUANTIDADE_PALETE.Value;
                    string mensagem = "  SALDO: " + somatorio + "/" + qtdMaxima;

                    mensagem += "   ENDEREÇO(S): ";
                    foreach (var o in saldoEmEstoque)
                    {
                        mensagem += o.QTD + " " + o.MOV_ENDERECO + ",   ";
                    }

                    CodigosBarras.PlayMsgErroValidacao = mensagem;
                    CodigosBarras.PlayAction = "ALERT";

                    etiqueta.ETI_STATUS = "P";
                    etiqueta.PlayAction = "update";
                    objetosParaPersistir.Add(etiqueta);

                    objetosParaPersistir.Add(new MovimentoEstoqueProducao()
                    {
                        MOV_QUANTIDADE = CodigosBarras.Quantidade,
                        MOV_DATA_HORA_EMISSAO = DateTime.Now,//ParametrosSingleton.Instance.DataBase,
                        MOV_DATA_HORA_CRIACAO = DateTime.Now,
                        MOV_DIA_TURMA = ParametrosSingleton.DiaTurmaS(),
                        MOV_LOTE = etiqueta.ETI_LOTE,
                        MOV_SUB_LOTE = etiqueta.ETI_SUB_LOTE,
                        MAQ_ID = etiqueta.MAQ_ID,
                        USE_ID = usuarioDb.USE_ID,
                        PRO_ID = etiqueta.ROT_PRO_ID,
                        ORD_ID = etiqueta.ORD_ID,
                        FPR_SEQ_REPETICAO = (int)etiqueta.FPR_SEQ_REPETICAO,
                        TIP_ID = "001",
                        MOV_ARMAZEM = ParametrosSingleton.Instance.Armazem,
                        MOV_ENDERECO = "PRO",
                        TURM_ID = usuarioDb.TURM_ID,
                        //TURN_ID = ParametrosSingleton.Instance.Usuario.TURN_ID,
                        TURN_ID = null,//-- pendencia
                        CAR_ID = null,//pendencia
                        PlayMsgErroValidacao = "",
                        PlayAction = "insert"
                    });

                }

                objects.AddRange(objetosParaPersistir);
            }
            return true;
        }
    }

}

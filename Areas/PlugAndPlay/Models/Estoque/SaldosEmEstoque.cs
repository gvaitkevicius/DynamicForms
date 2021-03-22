using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Models;
using DynamicForms.Util;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "SALDOS EM ESTOQUE POR LOTE")]
    public class SaldosEmEstoquePorLote
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRODUTO")] [Required(ErrorMessage = "Campo PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LOTE")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MOV_LOTE")] public string MOV_LOTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SUB_LOTE")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_SUB_LOTE")] public string MOV_SUB_LOTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PEDIDO")] [Required(ErrorMessage = "Campo ORD_ID requirido.")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ENDERECO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_ENDERECO")] public string MOV_ENDERECO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "APROVEITAMENTO")] [Required(ErrorMessage = "Campo MOV_APROVEITAMENTO requirido.")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo MOV_APROVEITAMENTO")] public string MOV_APROVEITAMENTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SALDO")] public double? SALDO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VENDAS")] public double? VENDAS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "RESERVA")] public double? QTD_RESERVA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PESO")] public double? PESO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PESO DAS EMBALAGENS")] public double? PESO_EMBALAGENS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID CARGA")]  public string CAR_ID { get; set; }
        [NotMapped] public T_Usuario UsuarioLogado { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        public virtual V_CONSULTA_PEDIDO ConsultaPedido { get; set; }

        public SaldosEmEstoquePorLote()
        {

        }
        public bool ImprimirEtiqueta(List<object> objects, ref List<LogPlay> Logs)
        {
            foreach (var item in objects)
            {
                SaldosEmEstoquePorLote saldoLote = (SaldosEmEstoquePorLote)item;
                saldoLote.PlayAction = "OK";
                using (var db = new ContextFactory().CreateDbContext(new string[] { }))
                {
                    var etiquetaexistente = db.Etiqueta.AsNoTracking().Where(x => x.ETI_LOTE.Equals(saldoLote.MOV_LOTE) && x.ETI_SUB_LOTE.Equals(saldoLote.MOV_SUB_LOTE)).OrderByDescending(x => x.ETI_EMISSAO).FirstOrDefault();
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

        /// <summary>
        /// Cria um objeto de movimento estoque reserva com o MOV_RETIDO = "S"
        /// OBS: O before changes da classe utiliza o SetReserva.
        /// </summary>
        /// <param name="objects"></param>
        /// <param name="logs"></param>
        /// <returns></returns>
        public bool ReterLote(List<object> objects, ref List<LogPlay> logs)
        {
            foreach(var item in objects)
            {
                if (item.GetType().Name != nameof(SaldosEmEstoquePorLote))
                    continue;

                // RETER LOTE

                SaldosEmEstoquePorLote saldoLote = (SaldosEmEstoquePorLote)item;

                var movReserva = new MovimentoEstoqueReservaDeEstoque();
                movReserva = movReserva.GetReserva(saldoLote.MOV_LOTE, saldoLote.MOV_SUB_LOTE, saldoLote.PRO_ID);
                movReserva.MOV_RETIDO = "S";
                movReserva.USE_ID = saldoLote.UsuarioLogado.USE_ID;

                MasterController mc = new MasterController();
                mc.UsuarioLogado = saldoLote.UsuarioLogado;

                List<object> list_of_object = new List<object>() { movReserva };
                List<List<object>> list_of_list_of_object = new List<List<object>>() { list_of_object };

                List<LogPlay> logs_update_data = mc.UpdateData(list_of_list_of_object, 0, true);
                logs.AddRange(logs_update_data);
            }

            return true;
        }

        /// <summary>
        /// Cria um objeto de movimento estoque reserva com o MOV_RETIDO = "N"
        /// OBS: O before changes da classe utiliza o SetReserva.
        /// </summary>
        /// <param name="objects"></param>
        /// <param name="logs"></param>
        /// <returns></returns>
        public bool LiberarLote(List<object> objects, ref List<LogPlay> logs)
        {
            foreach (var item in objects)
            {
                if (item.GetType().Name != nameof(SaldosEmEstoquePorLote))
                    continue;

                // LIBERAR LOTE

                SaldosEmEstoquePorLote saldoLote = (SaldosEmEstoquePorLote)item;

                var movReserva = new MovimentoEstoqueReservaDeEstoque();
                movReserva = movReserva.GetReserva(saldoLote.MOV_LOTE, saldoLote.MOV_SUB_LOTE, saldoLote.PRO_ID);
                movReserva.MOV_RETIDO = "N";
                movReserva.USE_ID = saldoLote.UsuarioLogado.USE_ID;

                MasterController mc = new MasterController();
                mc.UsuarioLogado = saldoLote.UsuarioLogado;

                List<object> list_of_object = new List<object>() { movReserva };
                List<List<object>> list_of_list_of_object = new List<List<object>>() { list_of_object };

                List<LogPlay> logs_update_data = mc.UpdateData(list_of_list_of_object, 0, true);
                logs.AddRange(logs_update_data);
            }

            return true;
        }

        /// <summary>
        /// Cria um objeto InterfaceTelaTransferenciaSimples, serializa ele em uma string JSON, e o coloca dentro de um objeto LogPlay do tipo PROTOCOLO - LINK.
        /// O protocolo será executado pela função executarProtocolos do javascript e irá abrir a tela do namespace especificado contendo os campos do objetos serializado já preenchidos.
        /// </summary>
        /// <param name="objects"></param>
        /// <param name="Logs"></param>
        /// <returns></returns>
        public bool DesmontarLote(List<object> objects, ref List<LogPlay> Logs)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (var item in objects)
                {
                    if (item.GetType().Name != nameof(SaldosEmEstoquePorLote))
                        continue;

                    SaldosEmEstoquePorLote saldoLote = (SaldosEmEstoquePorLote)item;

                    var max_sub_lote = db.MovimentoEstoque.Where(x => x.MOV_LOTE == saldoLote.MOV_LOTE && x.PRO_ID == saldoLote.PRO_ID).Max(x => x.MOV_SUB_LOTE) + 1;

                    string name_space = "DynamicForms.Areas.PlugAndPlay.Models.InterfaceTelaTransferenciaSimples";

                    InterfaceTelaTransferenciaSimples parametros = new InterfaceTelaTransferenciaSimples() { MOV_LOTE_ORIGEM = saldoLote.MOV_LOTE, MOV_SUB_LOTE_ORIGEM = saldoLote.MOV_SUB_LOTE, MOV_LOTE_DESTINO = saldoLote.MOV_LOTE, MOV_SUB_LOTE_DESTINO = max_sub_lote, PRO_ID_DESTINO = saldoLote.PRO_ID, ORD_ID = saldoLote.ORD_ID, MOV_ENDERECO = saldoLote.MOV_ENDERECO };

                    //transforma o objeto em uma string JSON para enviar por GET quando o protocolo for executado
                    string string_parametros = JsonConvert.SerializeObject(parametros);

                    LogPlay protocolo = new LogPlay() { Status = "LINK", MsgErro = $"/DynamicWeb/LinkSGI?str_namespace={name_space}&ArrayDeValoresDefault={string_parametros}", NomeAtributo = "PROTOCOLO" };
                    Logs.Add(protocolo);
                }
            }

            return true;
        }

        [HIDDEN]
        [Obsolete]
        public bool UnSetReserva(string lote, string sub_lote, double quantidade, string CAR_ID, string ORD_ID, string MOV_ENDERECO, ref MovimentoEstoqueReservaDeEstoque reserva)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                if (reserva != null)
                {
                    //Recuperando tipo do pedido
                    /* 
                    int Db_pedidoRomaneado = db.Order.Where(o => o.ORD_ID == ORD_ID).Select(x => x.ORD_TIPO).FirstOrDefault().Value;
                    if (Db_pedidoRomaneado == 1) //Se for um pedido normal(produçao), anular pedido e carga da reserva
                    {
                        reserva.ORD_ID = null;
                    }*/
                    reserva.CAR_ID = null;
                    reserva.PlayAction = "update";
                }
            }
            return true;
        }

        /// <summary>
        /// Cria um novo movimento de reserva para um lote epecificado e o retorna por referência
        /// Codigos de Origem 1: Reserva de Producao, 2- Reserva Romaneio - 3 Reserva manual
        /// Se a quantidade for igual a 0 significa que esta tentando reservar 100% do lote.
        /// Caso a reserva não exista, ele irá pegar todos os dados passados por parãmetro e irá criar uma nova reserva com os parâmetros especificados.
        /// Caso a reserva exista, apesar de ser possível passar vários dados por parâmetros, só serão alterado as propriedades especificadas pela origem, por exemplo, se você passar um valor de MOV_RETIDO e passar o parâmetro origem como 4, ele não irá alterar o MOV_RETIDO do produto (para isso, é preciso passar origem como 8)
        /// </summary>
        /// <param name="origem">1-producao|2-Romaneio|3-Manual|4-Desmontagem|5-Enderecamento|6-Reaproveitamento|7-E/S Inventario|8-Reter Lote</param>
        /// <param name="lote"></param>
        /// <param name="sub_lote"></param>
        /// <param name="quantidade"></param>
        /// <param name="CAR_ID"></param>
        /// <param name="ORD_ID"></param>
        /// <param name="MOV_ENDERECO"></param>
        /// <param name="SEQ_REP"></param>
        /// <param name="MOV_RETIDO">Campo para reter o lote. Deve ser passado em conjunto com origem = 8 caso seja uma reserva existente. Caso a reserva não exista, ele irá inserir uma nova reserva com o MOV_RETIDO = mov_retido especificado no parâmetro. </param>
        /// <param name="reserva">referência ao objeto que será persistido</param>
        /// <returns></returns>
        [HIDDEN]
        [Obsolete]
        public bool SetReserva(int origem, int MOV_ID, string aproveitamento, string lote, string sub_lote, double quantidade, string CAR_ID, string ORD_ID, string PRO_ID, int USE_ID, string MOV_ENDERECO, int? SEQ_REP, string mov_retido, ref MovimentoEstoqueReservaDeEstoque reserva)
        {
            if (reserva is null)
            {
                throw new ArgumentNullException(nameof(reserva));
            }
            bool check = true;
            MovimentoEstoqueReservaDeEstoque reservaPadrao = new MovimentoEstoqueReservaDeEstoque()
            {
                MOV_ID = MOV_ID,
                TIP_ID = "998",
                ORD_ID = ORD_ID,
                PRO_ID = PRO_ID,
                FPR_SEQ_REPETICAO = SEQ_REP,
                CAR_ID = CAR_ID,
                MOV_LOTE = lote,
                MOV_SUB_LOTE = sub_lote,
                MOV_QUANTIDADE = quantidade,
                MOV_RETIDO = mov_retido,
                MOV_DATA_HORA_CRIACAO = DateTime.Now,
                MOV_DATA_HORA_EMISSAO = DateTime.Now,//ParametrosSingleton.Instance.DataBase,
                MOV_DIA_TURMA = ParametrosSingleton.DiaTurmaS(),
                MOV_ARMAZEM = ParametrosSingleton.Instance.Armazem,
                MOV_ENDERECO = MOV_ENDERECO,
                MOV_APROVEITAMENTO = aproveitamento,
                USE_ID = USE_ID,
                TURM_ID = null,
                MAQ_ID = null,
                PlayMsgErroValidacao = ""
            };
            using (var db = new ContextFactory().CreateDbContext(Array.Empty<string>()))
            {
                var reservaExistente=db.MovimentoEstoqueReservaDeEstoque.Where(x=> x.PRO_ID.Equals(PRO_ID) && x.MOV_LOTE.Equals(lote) && x.MOV_SUB_LOTE.Equals(sub_lote) && !x.MOV_ESTORNO.Equals("E")).FirstOrDefault();
                if (reservaExistente != null) 
                {
                    reservaPadrao = reservaExistente;
                }


                switch (origem)
                {
                    case 1://Reserva de Producao => ok
                        var pedidoTipo = db.Order.AsNoTracking().Where(x => x.ORD_ID.Equals(ORD_ID)).Select(x => new { x.ORD_TIPO, x.ORD_ID_INTEGRACAO }).FirstOrDefault();
                        //O Pedido é do Tipo Kanban e sua reserva sera Criada sem o ORD_ID
                        if (pedidoTipo.ORD_TIPO == 2)
                        {
                            reservaPadrao.ORD_ID = "";
                            reservaPadrao.FPR_SEQ_TRANFORMACAO = 0;
                            reservaPadrao.FPR_SEQ_REPETICAO = 0;
                        }
                        reservaPadrao.PlayAction = (reservaExistente != null) ? "update" : "insert";
                        break;
                    case 2://Reserva por Romaneio
                        reservaPadrao.CAR_ID = CAR_ID;
                        reservaPadrao.ORD_ID = ORD_ID;
                        reservaPadrao.PlayAction = "update";
                        reserva = reservaPadrao;
                        break;

                    case 3://rESERVA MANUAL
                        reservaPadrao.PlayAction = (reservaExistente != null) ? "update" : "insert";
                        reservaPadrao.FPR_SEQ_REPETICAO = (reservaPadrao.FPR_SEQ_REPETICAO == null) ? 0 : reservaPadrao.FPR_SEQ_REPETICAO;
                        break;
                    case 4://Desmontagem de produto => ok
                        reservaPadrao.PlayAction = (reservaExistente != null) ? "update" : "insert";
                        break;
                    case 5://Enderecamento => ok
                        reservaPadrao.MOV_ENDERECO = MOV_ENDERECO;
                        reservaPadrao.MOV_DATA_HORA_EMISSAO = reserva.MOV_DATA_HORA_EMISSAO;
                        reservaPadrao.MOV_DATA_HORA_CRIACAO = reserva.MOV_DATA_HORA_CRIACAO;
                        reservaPadrao.MOV_DIA_TURMA = reserva.MOV_DIA_TURMA;
                        reservaPadrao.PlayAction = (reservaExistente != null) ? "update" : "insert";
                        break;
                    case 6: // Reaproveitamento
                        reservaPadrao.ORD_ID = ORD_ID;
                        reservaPadrao.PlayAction = (reservaExistente != null) ? "update" : "insert";
                        reservaPadrao.MOV_APROVEITAMENTO = "S";
                        break;
                    case 7: // E/S INVENTARIO => ok
                        reservaPadrao.PlayAction = (reservaExistente != null) ? "update" : "insert";
                        reservaPadrao.MOV_APROVEITAMENTO = (String.IsNullOrEmpty(reservaPadrao.ORD_ID)) ? "N" : "S";
                        break;
                    case 8: // RETER LOTE => ok
                        reservaPadrao.PlayAction = (reservaExistente != null) ? "update" : "insert";
                        reservaPadrao.MOV_RETIDO = mov_retido;
                        break;

                }
                if (origem != 2)
                    reserva = reservaPadrao;
            }

            return check;
        }

        
    }
    
    [Display(Name = "SALDO EM ESTOQUE POR PEDIDO")]
    public class SaldoEmEstoquePorPedido
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CÓDIGO PRODUTO")] [Required(ErrorMessage = "Campo PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CÓDIGO DO PEDIDO")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ_TRANFORMACAO")] public int? FPR_SEQ_TRANFORMACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SALDO ALOCADO")] public double? SALDO_ALOCADO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SALDO RETIDO")] public double? SALDO_RETIDO { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
    }
}
//using Opt;

using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Models;
using DynamicForms.Util;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Remotion.Linq.Clauses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "CARGAS")]
    public class Carga
    {
        public Carga()
        {
            this.ItensCarga = new HashSet<ItenCarga>();
            this.V_ITEM_CARGA = new HashSet<V_ITEM_CARGA>();
            this.MovimentoEstoqueProducao = new HashSet<MovimentoEstoqueProducao>();
            this.MovimentoEstoqueReservaDeEstoque = new HashSet<MovimentoEstoqueReservaDeEstoque>();
            this.MovimentoEstoqueVendas = new HashSet<MovimentoEstoqueVendas>();
            this.MovimentoEstoqueTransferenciaSimples = new HashSet<MovimentoEstoqueTransferenciaSimples>();
            this.MovimentoEstoqueSaidaInventario = new HashSet<MovimentoEstoqueSaidaInventario>();
            this.MovimentoEstoqueEntradaInventario = new HashSet<MovimentoEstoqueEntradaInventario>();
            this.MovimentoEstoquePerdas = new HashSet<MovimentoEstoquePerdas>();
            this.MovimentoEstoqueConsumoMateriaPrima = new HashSet<MovimentoEstoqueConsumoMateriaPrima>();
            this.V_ITENS_ROMANEADOS = new HashSet<V_ITENS_ROMANEADOS>();
        }

        public Carga(string CAR_ID, int SecCargaId)
        {
            this.CAR_ID = CAR_ID;
            Rota = new Rota();
            ItensCarga = new List<ItenCarga>();
            PedidosParaExpedicao = new List<PedidosParaExpedicao>();
            // pendencia this.VolumeDisponivel = TipoVeiculo.TIP_CAPACIDADE_M3;
            CAR_DATA_INICIO_PREVISTO = DateTime.Now;
            CAR_DATA_INICIO_REALIZADO = DateTime.Now;
            CAR_DATA_FIM_PREVISTO = DateTime.Now;
            CAR_DATA_FIM_REALIZADO = DateTime.Now;
            Opt = "";
            this.SecCargaId = SecCargaId;


            CAR_STATUS = 0;
            CAR_PESO_TEORICO = 0;
            CAR_VOLUME_TEORICO = 0;
            CAR_PESO_REAL = 0;
            CAR_VOLUME_REAL = 0;
            CAR_PESO_EMBALAGEM = 0;
            CAR_PESO_ENTRADA = 0;
            CAR_PESO_SAIDA = 0;
            CAR_ID_DOCA = "";
            VEI_PLACA = "";
            TIP_ID = 0;
            CAR_PREVISAO_MATERIA_PRIMA = DateTime.Now;
            TRA_ID = "";
            CAR_GRUPO_PRODUTIVO = 0;
        }

        [TAB(Value = "PRINCIPAL")] [READ] [Display(Name = "COD CARGA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo CAR_ID")] public string CAR_ID { get; set; }
        //[Combobox(Description = "DELETADA", Value = "-1")]
        [Combobox(Description = "1.0 ABERTA", Value = "1")]
        [Combobox(Description = "1.1 CONGELADA", Value = "1.1")]
        [Combobox(Description = "1.2 ANTECIPAR", Value = "1.2")]
        [Combobox(Description = "2.0 APROVADA", Value = "2")]
        [Combobox(Description = "3.0 AGENCIADA", Value = "3")]
        [Combobox(Description = "4.0 PICKING", Value = "4")]
        [Combobox(Description = "5.0 CARREGANDO", Value = "5")]
        [Combobox(Description = "6.0 DESPACHADA (CONSOLIDADA)", Value = "6", Disabled = true)]
        [Combobox(Description = "7.0 FATURADA", Value = "7")]
        [Combobox(Description = "8.0 ENTREGUE PARCIAL", Value = "8")]
        [Combobox(Description = "9.0 ENTREGUE", Value = "9")]
        //[Combobox(Description = "ESTORNADA", Value = "99")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "STATUS CARGA")] public double? CAR_STATUS { get; set; }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "EMBARQUE ALVO")] public DateTime CAR_EMBARQUE_ALVO { get; set; }
        [TAB(Value = "PRINCIPAL")] [READ] [Display(Name = "INICIO PREVISTO")] public DateTime CAR_DATA_INICIO_PREVISTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [READ] [Display(Name = "FIM PREVISTO")] public DateTime CAR_DATA_FIM_PREVISTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [READ] [Display(Name = "INICIO JANELA EMB")] public DateTime CAR_INICIO_JANELA_EMBARQUE { get; set; }
        [TAB(Value = "PRINCIPAL")] [READ] [Display(Name = "FIM JANELA EMB")] public DateTime CAR_FIM_JANELA_EMBARQUE { get; set; }


        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD TIPO VEÍCULO")] public int? TIP_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PLACA VEÍCULO")] [MaxLength(8, ErrorMessage = "Maximode 8 caracteres, campo VEI_PLACA")] public string VEI_PLACA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD TRANSPORTADORA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo TRA_ID")] public string TRA_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OBSERVACAO_DE_TRANSPORTE")] [MaxLength(4000, ErrorMessage = "Maximode 4000 caracteres, campo CAR_OBSERVACAO_DE_TRANSPORTE")] public string CAR_OBSERVACAO_DE_TRANSPORTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD DOCA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo CAR_ID_DOCA")] public string CAR_ID_DOCA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID DE OCORRENCIA DE PESAGEM")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo OCO_ID_LIBERACAO")] public string OCO_ID_LIBERACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OBSERVAÇÃO DA LIBERAÇÃO POR PESAGEM")] [MaxLength(500, ErrorMessage = "Maximode 500 caracteres, campo CAR_OBS_LIBERACAO")] public string CAR_OBS_LIBERACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LIBERAÇÃO DA CARGA POR PESAGEM")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo CAR_PESAGEM_LIBERADA")] [READ] public string CAR_PESAGEM_LIBERADA { get; set; }


        [TAB(Value = "PESOS")][READ][Display(Name = "Pesagem Entrada Veiculo")] public DateTime CAR_DATA_ENTRADA_VEICULO { get; set; }
        [TAB(Value = "PESOS")][READ][Display(Name = "Peso Entrada(tara)")] public double? CAR_PESO_ENTRADA { get; set; }
        [TAB(Value = "PESOS")][READ][Display(Name = "Pesagem Saida Veiculo")] public DateTime CAR_DATA_SAIDA_VEICULO { get; set; }
        [TAB(Value = "PESOS")][READ][Display(Name = "Peso Saida(carregado)")] public double? CAR_PESO_SAIDA { get; set; }
        [TAB(Value = "PESOS")][READ][Display(Name = "Peso Embalagem")] public double? CAR_PESO_EMBALAGEM { get; set; }
        [TAB(Value = "PESOS")][READ][Display(Name = "Peso Real (Balança)")] public double? CAR_PESO_REAL { get; set; }
        [TAB(Value = "PESOS")][READ][Display(Name = "Peso teorico")] public double? CAR_PESO_TEORICO { get; set; }
                              
        [TAB(Value = "PESOS")][READ][Display(Name = "Volume Teorico")] public double? CAR_VOLUME_TEORICO { get; set; }
        [TAB(Value = "PESOS")][READ][Display(Name = "Volume Real")] public double? CAR_VOLUME_REAL { get; set; }

        [TAB(Value = "OUTROS")] [Display(Name = "PREVISÃO MP")] public DateTime CAR_PREVISAO_MATERIA_PRIMA { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "INICIO REALIZADO")] public DateTime CAR_DATA_INICIO_REALIZADO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "FIM REALIZADO")] public DateTime CAR_DATA_FIM_REALIZADO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "OCORRENCIA")] [MaxLength(30, ErrorMessage = "")] public string OCO_ID { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "GRUPO PRODUTIVO")] public double? CAR_GRUPO_PRODUTIVO { get; set; }

        
        [TAB(Value = "OUTROS")] [Display(Name = "COD ROTAS FACTÍVEIS")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo ROT_ID")] public string ROT_ID { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "JUSTIFICATIVA_DE_CARREGAMENTO")] [MaxLength(4000, ErrorMessage = "Maximode 4000 caracteres, campo CAR_JUSTIFICATIVA_DE_CARREGAMENTO")] public string CAR_JUSTIFICATIVA_DE_CARREGAMENTO { get; set; }
        [TAB(Value = "OUTROS")] [READ] [Display(Name = "ID_JUNTADA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo CAR_ID_JUNTADA")] public string CAR_ID_JUNTADA { get; set; }
        [TAB(Value = "OUTROS")] [READ] [Display(Name = "ID_INTEGRACAO_BALANCA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo CAR_ID_INTEGRACAO_BALANCA")] public string CAR_ID_INTEGRACAO_BALANCA { get; set; }

        [NotMapped] public T_Usuario UsuarioLogado { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public string PlayOrigem { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  } 
        public virtual OcorrenciaTransporte OcorrenciaTransporte { get; set; }
        public virtual OcorrenciaTransporte OcorrenciaTransportePesagem { get; set; }
        [Display(Name = "TIPO DO VEICULO")] public virtual TipoVeiculo TipoVeiculo { get; set; }
        [Display(Name = "VEICULO")] public virtual Veiculo Veiculo { get; set; }
        [Display(Name = "TRANSPORTADORA")] public virtual Transportadora Transportadora { get; set; }

        [NotMapped]
        public DateTime InicioGrupoProdutivo { get; set; }
        [NotMapped]
        public DateTime FimGrupoProdutivo { get; set; }
        [NotMapped]
        public int SecCargaId { get; set; } // campo auxiliar para algoritimo CLP
        [NotMapped]
        public List<PedidosParaExpedicao> PedidosParaExpedicao { get; set; }
        [NotMapped]
        public Rota Rota { get; set; }
        [NotMapped]
        //public List<Item> ItensPacked { get; set; }
        //[NotMapped]
        public int IndexList { get; set; }
        [NotMapped]
        public string Opt { get; set; } // utilizada para controle nos algoritimos atualmente utilizado para descartar cargas da lista de cargas factiveis 

        [HIDDENINTERFACE] public virtual ICollection<MovimentoEstoqueProducao> MovimentoEstoqueProducao { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<MovimentoEstoqueReservaDeEstoque> MovimentoEstoqueReservaDeEstoque { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<MovimentoEstoqueVendas> MovimentoEstoqueVendas { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<MovimentoEstoqueSaidaInventario> MovimentoEstoqueSaidaInventario { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<MovimentoEstoqueEntradaInventario> MovimentoEstoqueEntradaInventario { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<MovimentoEstoqueTransferenciaSimples> MovimentoEstoqueTransferenciaSimples { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<MovimentoEstoqueConsumoMateriaPrima> MovimentoEstoqueConsumoMateriaPrima { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<MovimentoEstoquePerdas> MovimentoEstoquePerdas { get; set; }
        [HIDDENINTERFACE] public virtual ICollection<ItenCarga> ItensCarga { get; set; }
        [Display(Name = "ITENS PLANEJADOS")] public virtual ICollection<V_ITEM_CARGA> V_ITEM_CARGA { get; set; }
        [Display(Name = "ITENS ROMANEADOS")] public virtual ICollection<V_ITENS_ROMANEADOS> V_ITENS_ROMANEADOS { get; set; }

        /// <summary>
        /// Métodos de classe
        /// </summary>
        /// 
        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            List<object> Cargas = new List<object>();
            List<List<object>> ListOfListObjects = new List<List<object>>();
            int IdIntAux = 0;
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (var item in objects)
                {
                    //Objeto retornado da View
                    if (item.GetType().Name == "Carga")
                    {
                        Carga _NovaCarga = (Carga)item;
                        //Validaçoes --
                        if (_NovaCarga.CAR_STATUS == 0)
                        {
                            _NovaCarga.PlayMsgErroValidacao = "Você deve atribuir um Status para a carga.";
                            return false;
                        }

                        if (_NovaCarga.PlayAction.Equals("update"))
                        {
                            if (_NovaCarga.PlayOrigem != "DesconsolidarRomaneio" && _NovaCarga.PlayOrigem != "ConsolidarRomaneio")
                            {
                                // valida alteração de status 
                                var itensRomaneados = db.V_ITENS_ROMANEADOS.AsNoTracking().Where(x => x.CAR_ID.Equals(_NovaCarga.CAR_ID)).ToList();
                                bool isRomaneado = itensRomaneados.Any(x => x.QTD_ROMANEADA > 0);
                                bool isConsolidado = itensRomaneados.Any(x => x.QTD_CONSOLIDADA > 0);

                                var clone = (Carga)cloneObjeto.GetClone(item);
                                //if (clone.CAR_STATUS != _NovaCarga.CAR_STATUS)
                                if (true)
                                {// pendencia deve ser revisto de maneira mais ampla
                                    if (isRomaneado == false && isConsolidado == false)
                                    {
                                        if (_NovaCarga.CAR_STATUS > 5)
                                        {
                                            _NovaCarga.PlayMsgErroValidacao = $"Alteração ilegal de status.";
                                            return false;
                                        }
                                    }

                                    if (isRomaneado == true && isConsolidado == false)
                                    {
                                        if (_NovaCarga.CAR_STATUS != 5)
                                        {
                                            _NovaCarga.PlayMsgErroValidacao = $"Alteração ilegal de status. O status deve ser 5.";
                                            return false;
                                        }
                                    }

                                    if (isRomaneado == true && isConsolidado == true)
                                    {
                                        if (_NovaCarga.CAR_STATUS < 6) // pendencia tratar a origem do movimento 
                                        {
                                            _NovaCarga.PlayMsgErroValidacao = $"Alteração ilegal de status. O status deve ser maior que 5.";
                                            return false;
                                        }
                                    }

                                }
                            }
                            //if (_NovaCarga.CAR_EMBARQUE_ALVO.CompareTo(DateTime.Now.AddDays(20)) >= 0)
                            //{
                            //    _NovaCarga.PlayMsgErroValidacao = $"Você não pode alterar uma carga com uma data maior que [{ DateTime.Now.AddDays(20)}].";
                            //    return false;
                            //}
                        }
                        if (!_NovaCarga.PlayAction.Equals("delete") && _NovaCarga.CAR_STATUS > 1)
                        {// Se a carga for diferente de estudo, então o Tipo de Veículo é obrigatório

                            if (_NovaCarga.TIP_ID == null || _NovaCarga.TIP_ID <= 0)
                            {
                                _NovaCarga.PlayMsgErroValidacao = "Você deve informar o Tipo de Veículo para esta carga.";
                                return false;
                            }
                        }

                        if (_NovaCarga.PlayAction == "insert")
                        {
                            _NovaCarga.CAR_ID = GetNextId(db, 2);
                        }
                        if (_NovaCarga.PlayAction == "CARGA_APS")
                        {
                            _NovaCarga.PlayAction = "insert";
                            _NovaCarga.CAR_ID = GetNextId(db, 2);
                        }
                        if(_NovaCarga.PlayAction == "CARGA_ANTECIPADA")
                        {
                            _NovaCarga.PlayAction = "insert";

                        }

                        if (_NovaCarga.PlayAction == "CARGA_UNIAO")
                        {
                            _NovaCarga.PlayAction = "insert";
                            if (_NovaCarga.CAR_ID.Substring(0, 1).Equals("M"))
                            {
                                _NovaCarga.CAR_ID = _NovaCarga.CAR_ID.Replace("M", "J");
                            }
                            else if (_NovaCarga.CAR_ID.Substring(0, 1).Equals("A"))
                            {
                                _NovaCarga.CAR_ID = _NovaCarga.CAR_ID.Replace("A", "J");
                            }
                            _NovaCarga.CAR_ID = GetNextId(db, 1);
                        }

                        //se o usuário preencher os dois campos, ele irá forçar a liberação da carga
                        if(!String.IsNullOrEmpty(_NovaCarga.OCO_ID_LIBERACAO) && !String.IsNullOrEmpty(_NovaCarga.CAR_OBS_LIBERACAO))
                        {
                            _NovaCarga.CAR_PESAGEM_LIBERADA = "S";
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Altera o status da lista de cargas que vêm como parâmetro, á função é ativada somente no menu de expedição na tela do APS.
        [HIDDEN]
        public bool AlterarStatusCarga(List<string> listaCarId, int status, ref List<LogPlay> Logs)
        {
            string msg = "";
            List<object> ObjetosProcessados = new List<object>();
            List<List<object>> ListObjectsToUpdate = new List<List<object>>();
            MasterController mc = new MasterController();

            //Adicionando carga atual para processamento posterior no UpdateData
            List<Carga> listaCargas = new List<Carga>();

            //Concatenando Logs por se tratar de um objeto de interface
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                listaCargas = db.Carga.AsNoTracking().Where(x => listaCarId.Contains(x.CAR_ID)).ToList();
            }
            if (listaCargas.Count > 0)
            {
                foreach (var carga in listaCargas)
                {
                    carga.CAR_STATUS = status;
                    carga.PlayAction = "update";

                    ObjetosProcessados.Add(carga);
                }

                ListObjectsToUpdate.Add(ObjetosProcessados);
                Logs.AddRange(mc.UpdateData(ListObjectsToUpdate, 3, true));


                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Gera uma nova carga produto da união de duas ou mais cargas ou reverte o processo para uma unica carga
        /// fornecida como parametro na listaCargas 
        /// </summary>
        /// <param name="listaCargas"></param>
        /// <param name="Logs"></param>
        /// <returns></returns>
        [HIDDEN]
        public string UnirCargas(List<Carga> listaCargas, string CaragaSelecionada, ref List<LogPlay> Logs, List<ItenCarga> listaItensCarga)
        {
            string msg = "";
            List<object> ObjetosProcessados = new List<object>();
            List<List<object>> ListObjectsToUpdate = new List<List<object>>();
            MasterController mc = new MasterController();
            List<ItenCarga> listaItensCargaOld = new List<ItenCarga>();
            bool flag = false;
            //Criando um objeto para a nova carga
            Carga _NovaCarga = new Carga() { ItensCarga = new List<ItenCarga>(), PlayAction = "CARGA_UNIAO", CAR_STATUS = 1 };
            List<ItenCarga> Db_ItensCargaAtual = new List<ItenCarga>();
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                //Determinando Id da nova carga
                _NovaCarga.CAR_ID = GetNextId(db, 1);
                var datas = listaCargas.Where(x => x.CAR_ID.Equals(CaragaSelecionada)).Select(x => new { x.CAR_DATA_INICIO_PREVISTO, x.CAR_DATA_FIM_PREVISTO, x.CAR_EMBARQUE_ALVO, CAR_INICIO_JANELA_EMBARQUE, x.CAR_FIM_JANELA_EMBARQUE, x.TRA_ID, x.CAR_VOLUME_TEORICO, x.TIP_ID }).FirstOrDefault();

                _NovaCarga.CAR_FIM_JANELA_EMBARQUE = datas.CAR_FIM_JANELA_EMBARQUE;
                _NovaCarga.CAR_INICIO_JANELA_EMBARQUE = datas.CAR_INICIO_JANELA_EMBARQUE;
                _NovaCarga.CAR_EMBARQUE_ALVO = datas.CAR_EMBARQUE_ALVO;
                _NovaCarga.CAR_DATA_FIM_PREVISTO = datas.CAR_DATA_FIM_PREVISTO;
                _NovaCarga.CAR_DATA_INICIO_PREVISTO = datas.CAR_DATA_INICIO_PREVISTO;
                _NovaCarga.CAR_DATA_FIM_PREVISTO = datas.CAR_DATA_FIM_PREVISTO;
                _NovaCarga.TRA_ID = datas.TRA_ID;
                _NovaCarga.TIP_ID = datas.TIP_ID;
                _NovaCarga.CAR_VOLUME_TEORICO = datas.CAR_VOLUME_TEORICO;
                //Para cada carga da lista
                if (listaItensCarga != null && listaItensCarga.Count > 0)
                {

                    flag = true;
                    foreach (var carga in listaCargas)
                    {
                        //Informando o destino desta carga para o sistema
                        //carga.CAR_ID_JUNTADA = _NovaCarga.CAR_ID;
                        //Definindo STATUS da antiga carga como ESTUDO
                        carga.CAR_STATUS = 1;
                        carga.PlayAction = "update";

                        //Adicionando carga atual para processamento posterior no UpdateData
                        ObjetosProcessados.Add(carga);
                    }
                    foreach (var itemCarga in listaItensCarga)
                    {
                        itemCarga.PlayAction = "delete";

                        listaItensCargaOld.Add(new ItenCarga(itemCarga));

                        itemCarga.CAR_ID = _NovaCarga.CAR_ID;
                        itemCarga.ITC_ENTREGA_PLANEJADA = _NovaCarga.CAR_EMBARQUE_ALVO;
                        itemCarga.ITC_ENTREGA_REALIZADA = _NovaCarga.CAR_EMBARQUE_ALVO;
                        itemCarga.PlayAction = "insert";
                        itemCarga.Oredr = null;
                        _NovaCarga.ItensCarga.Add(itemCarga);

                    }

                }
                else
                {
                    flag = false;
                    foreach (var carga in listaCargas)
                    {
                        //Consultando os itens da carga
                        Db_ItensCargaAtual = db.ItenCarga.AsNoTracking().Where(ic => ic.CAR_ID == carga.CAR_ID).ToList();
                        //Se a craga nao é produto de uma uniao 
                        if (Db_ItensCargaAtual != null && listaCargas.Count() > 1)
                        {
                            foreach (var itemCarga in Db_ItensCargaAtual)
                            {
                                //Adicionando os itens da antiga carga para a nova carga
                                itemCarga.CAR_ID = _NovaCarga.CAR_ID;
                                itemCarga.ITC_ENTREGA_PLANEJADA = _NovaCarga.CAR_EMBARQUE_ALVO;
                                itemCarga.ITC_ENTREGA_REALIZADA = _NovaCarga.CAR_EMBARQUE_ALVO;
                                _NovaCarga.ItensCarga.Add(itemCarga);
                            }
                        }

                        //Informando o destino desta carga para o sistema
                        carga.CAR_ID_JUNTADA = _NovaCarga.CAR_ID;
                        //Definindo STATUS da antiga carga como ESTUDO
                        carga.CAR_STATUS = 1;
                        carga.PlayAction = "update";

                        //Adicionando carga atual para processamento posterior no UpdateData
                        ObjetosProcessados.Add(carga);
                    }

                }
            }
            ObjetosProcessados.Add(_NovaCarga);
            ListObjectsToUpdate.Add(ObjetosProcessados);
            //Concatenando Logs por se tratar de um objeto de interface
            Logs.AddRange(mc.UpdateData(ListObjectsToUpdate, 3, true));
            if (flag)
            {
                ObjetosProcessados = new List<object>();
                ListObjectsToUpdate = new List<List<object>>();
                foreach (var item in listaItensCargaOld)
                {
                    ObjetosProcessados.Add(item);
                }
                ListObjectsToUpdate.Add(ObjetosProcessados);
                mc = new MasterController();

                Logs.AddRange(mc.UpdateData(ListObjectsToUpdate, 4, true));
            }


            var resp = new LogPlay().GetLogsErro(Logs);
            if (resp.Count > 0)
            {
                foreach (var item in Logs)
                {
                    msg += item.MsgErro;
                }
            }
            else
            {
                msg = _NovaCarga.CAR_ID;
            }

            return msg;
        }



        /// <summary>
        /// Realiza a consolidação de uma carga  previamente romaneada
        /// </summary>
        /// <param name="objects"></param>
        /// <param name="Logs"></param>
        /// <returns></returns>
        public bool ConsolidarRomaneio(List<object> objects, ref List<LogPlay> Logs)
        {
            bool check = true;
            int cont = 0;
            bool flag = false;
            List<object> RomaneioConsolidado = new List<object>();
            MasterController mc = new MasterController();
            List<Order> dbPedidosNosLotes = new List<Order>();
            List<MovimentoEstoqueReservaDeEstoque> dbReservaLotesRomaneados = new List<MovimentoEstoqueReservaDeEstoque>();
            List<SaldosEmEstoquePorLote> dbSaldosEmEstoquePorLote = new List<SaldosEmEstoquePorLote>();
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                double quantidadeEstoque;
                string actionAux = "";
                string msgAux = "";
                List<V_ITENS_ROMANEADOS> dbItensRomaneados = null;

                foreach (var item in objects)
                {
                    Carga cargaRomaneada = (Carga)item;
                    cargaRomaneada.PlayOrigem = "ConsolidarRomaneio";

                    //Modificando action do objeto da view para que a funcao UpdateData não tente a persistencia. 
                    cargaRomaneada.PlayAction = "OK";
                    cargaRomaneada.PlayMsgErroValidacao = "";
                    //Validando existencia da carga.
                    Carga dbCargaInformada = db.Carga.AsNoTracking().Where(ca => ca.CAR_ID == cargaRomaneada.CAR_ID).FirstOrDefault();

                    #region Validações e consultas para inicio do processo(CARAGA INFORMADA E RESERVA DOS LOTES

                    if (dbCargaInformada == null)
                    {
                        cargaRomaneada.PlayMsgErroValidacao = "A carga informada não existe, verifique o Id informado.";
                        check = false;
                    }

                    if (check)
                    {
                        if (String.IsNullOrEmpty(dbCargaInformada.TRA_ID))
                        {
                            cargaRomaneada.PlayMsgErroValidacao = "Informe a transportadora.";
                            check = false;
                        }
                    }


                    if (check)
                    {
                        //Recuperando lotes reservados
                        dbReservaLotesRomaneados = db.MovimentoEstoqueReservaDeEstoque.AsNoTracking().Where(re => re.CAR_ID == dbCargaInformada.CAR_ID && String.IsNullOrEmpty(re.MOV_ESTORNO)).ToList();

                        // PENDENCIA revisar fonte eliminando  a consulta a cima db.MovimentoEstoqueReservaDeEstoque e usando esta de baixo 
                        dbItensRomaneados = db.V_ITENS_ROMANEADOS.AsNoTracking().Where(x => x.CAR_ID == dbCargaInformada.CAR_ID).ToList();
                        dbSaldosEmEstoquePorLote = db.SaldosEmEstoquePorLote.Where(x => x.CAR_ID == dbCargaInformada.CAR_ID).ToList();

                        if (dbItensRomaneados.Where(x=>x.QTD_ROMANEADA <= 0).Count() > 0 )
                        {
                            cargaRomaneada.PlayMsgErroValidacao = $"Há [{ dbItensRomaneados.Where(x => x.QTD_ROMANEADA <= 0).Count() }] Pedidos para esta carga que ainda não foram Romaneados\n Pedidos:[{dbItensRomaneados.Select(i => i.ORD_ID).Aggregate((i, j) => i + "," + j)}]";
                            check = false;
                        }
                    }


                    #region Integração peso de balança
                    if (check)
                    {
                        V_INPUT_INTEGRACAO_BALANCA_FATURAMENTO pesos = db.V_INPUT_INTEGRACAO_BALANCA_FATURAMENTO.Where(x => x.VEI_PLACA == cargaRomaneada.VEI_PLACA).OrderByDescending(x => x.CAR_DATA_ENTRADA_VEICULO).FirstOrDefault();
                        double? param_percentual_pesagem = db.Param.Where(x => x.PAR_ID == "EXPEDICAO_PERCENTUAL_PESAGEM").Select(x => x.PAR_VALOR_N).FirstOrDefault();
                        if (pesos != null)
                        {
                            // valida se ticket ja foi utilizado por outra carga 
                            foreach (var c in db.Carga.Where(x =>x.CAR_ID != cargaRomaneada.CAR_ID && x.CAR_ID_INTEGRACAO_BALANCA == pesos.CAR_ID_INTEGRACAO_BALANCA).ToList())
                            {
                                cargaRomaneada.PlayMsgErroValidacao = $" O ticket de pasegem {pesos.CAR_ID_INTEGRACAO_BALANCA} ja foi utilizado na carga [{c.CAR_ID}]";
                                check = false;
                            }
                            
                            //importa pesos de balança e coloca na carga
                            cargaRomaneada.CAR_PESO_ENTRADA = (double?)pesos.CAR_PESO_ENTRADA;
                            cargaRomaneada.CAR_PESO_SAIDA = (double?)pesos.CAR_PESO_SAIDA;
                            cargaRomaneada.CAR_DATA_ENTRADA_VEICULO = CAR_DATA_ENTRADA_VEICULO;
                            cargaRomaneada.CAR_DATA_SAIDA_VEICULO = CAR_DATA_SAIDA_VEICULO;
                            cargaRomaneada.CAR_PESO_EMBALAGEM = dbSaldosEmEstoquePorLote.Sum(x => x.PESO_EMBALAGENS);

                            //verifica se o percentual de diferença esta dentro do range do parametro EXPEDICAO_PERCENTUAL_PESAGEM
                            if (VerificarPeso(cargaRomaneada, param_percentual_pesagem, dbSaldosEmEstoquePorLote))
                            {
                                cargaRomaneada.CAR_PESAGEM_LIBERADA = "S";
                            }
                            else //se estiver fora do range, bloqueia a carga
                            {
                                LogPlay log = new LogPlay(this.ToString(), "ALERT", "Carga bloqueada por peso.");
                                Logs.Add(log);

                                cargaRomaneada.CAR_PESAGEM_LIBERADA = "N";
                            }
                        }
                    }
                    #endregion



                    #endregion

                    #region Tratamento para KITs(CONJUNTOS)

                    if (check)
                    {
                        ///TRATAMENTO EXCLUSIVO PARA KITS(CONJUNTO)
                        if (dbItensRomaneados.Where(x=>x.ORD_ID_CONJUNTO !="").Count() > 0)
                        {
                            string idconjunto = "";
                            double QtdConjunto = 0;
                            foreach (var cj in dbItensRomaneados.Where(x=>x.ORD_ID_CONJUNTO!= "").OrderBy(x=>x.ORD_ID_CONJUNTO))
                            {
                                if (idconjunto != cj.ORD_ID_CONJUNTO)
                                {
                                    idconjunto = cj.ORD_ID_CONJUNTO;
                                    QtdConjunto = (double)cj.QTD_CONJUNTOS;
                                }
                                if (QtdConjunto != (double)cj.QTD_CONJUNTOS)
                                {
                                    cargaRomaneada.PlayMsgErroValidacao = $"Os itens dos conjuntos [{cj.ORD_ID_CONJUNTO}], diferem entre si.";
                                    check = false;

                                }
                            }

                            if (check)
                            {
                                var conjuntosDaCarga = dbItensRomaneados.Where(cj => cj.ORD_ID_CONJUNTO != "").
                                GroupBy(cc => new { cc.ORD_ID_CONJUNTO, cc.PRO_ID_CONJUNTO }).Select(c => 
                                new { PRO_ID_CONJUNTO = c.Key.PRO_ID_CONJUNTO,   ORD_ID_CONJUNTO = c.Key.ORD_ID_CONJUNTO, QTD = c.Count()}).ToList();

                                foreach (var cj in conjuntosDaCarga)
                                {
                                    int qtdcjRomaneio = db.EstruturaProduto.AsNoTracking().Where(x => x.PRO_ID_PRODUTO == cj.PRO_ID_CONJUNTO).Count();
                                    if (qtdcjRomaneio != cj.QTD)
                                    {
                                        cargaRomaneada.PlayMsgErroValidacao = $"Divergencia nos itens do conjunto [{cj.ORD_ID_CONJUNTO}] Qtd itens do conjunto [{cj.QTD}], Qtd itens do romaneio [{qtdcjRomaneio}]";
                                        check = false;
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    #region Validação Carregamento total ou parcial

                    //Pega todos os pedidos romaneados na carga
                    //dbPedidosNosLotes = db.MovimentoEstoque.Where(x => x.TIP_ID == "998" && x.CAR_ID == cargaRomaneada.CAR_ID).GroupBy(x => new { x.ORD_ID }).ToList();
                    if (check)
                    {
                        //Validando se todos os pedidos estão carregados /e ou carregados integralmente
                        while (!flag && cont < dbItensRomaneados.Count)
                        {
                            double tolMenos = db.Order.AsNoTracking().Where(x => x.ORD_ID.Equals(dbItensRomaneados[cont].ORD_ID)).Select(x => x.ORD_TOLERANCIA_MENOS.Value).FirstOrDefault();
                            double qtdCarregado = dbReservaLotesRomaneados.Where(x => x.ORD_ID == dbItensRomaneados[cont].ORD_ID).Sum(xx => xx.MOV_QUANTIDADE);
                            double tolerancia_minima = dbItensRomaneados[cont].ITC_QTD_PLANEJADA - ((tolMenos / 100.0) * dbItensRomaneados[cont].ITC_QTD_PLANEJADA);

                            //se estiver abaixo da tolerancia minima (romaneou 80 de um minimo de 90 por exemplo), e não tiver uma justificativa, solicita que o usuário preencha uma justificativa
                            if (qtdCarregado <= tolerancia_minima && String.IsNullOrWhiteSpace(cargaRomaneada.CAR_JUSTIFICATIVA_DE_CARREGAMENTO))
                            {
                                cargaRomaneada.PlayMsgErroValidacao = $"A quantidade ({qtdCarregado}) romaneada no pedido[{dbItensRomaneados[cont].ORD_ID}] é menor que a tolerância mínima ({tolerancia_minima}) para esse pedido \n e nenhuma justificativa foi apontada, preencha a justificativa.";
                                cargaRomaneada.PlayAction = "OK";
                                check = false;                          
                            }

                            cont++;
                        }
                    }

                    #endregion

                    #region Criando movimentos de saída para baixa do estoque

                    if (check)
                    {
                        cont = 0;
                        flag = false;

                        while (cont < dbReservaLotesRomaneados.Count)
                        {
                            actionAux = "insert";
                            msgAux = "";
                            quantidadeEstoque = dbSaldosEmEstoquePorLote.Where(x => x.MOV_LOTE == dbReservaLotesRomaneados[cont].MOV_LOTE && x.MOV_SUB_LOTE == dbReservaLotesRomaneados[cont].MOV_SUB_LOTE).Select(x => x.SALDO.Value).FirstOrDefault();
                            flag = dbReservaLotesRomaneados[cont].MOV_QUANTIDADE > quantidadeEstoque;

                            if (!flag)
                            {
                                MovimentoEstoqueVendas mvSaida = new MovimentoEstoqueVendas()
                                {
                                    MOV_QUANTIDADE = dbReservaLotesRomaneados[cont].MOV_QUANTIDADE,
                                    MOV_DATA_HORA_EMISSAO = DateTime.Now,//ParametrosSingleton.Instance.DataBase,
                                    MOV_DATA_HORA_CRIACAO = DateTime.Now,
                                    MOV_DIA_TURMA = ParametrosSingleton.DiaTurmaS(),
                                    MOV_LOTE = dbReservaLotesRomaneados[cont].MOV_LOTE,
                                    MOV_SUB_LOTE = dbReservaLotesRomaneados[cont].MOV_SUB_LOTE,
                                    MAQ_ID = dbReservaLotesRomaneados[cont].MAQ_ID,
                                    USE_ID = cargaRomaneada.UsuarioLogado.USE_ID,
                                    PRO_ID = dbReservaLotesRomaneados[cont].PRO_ID,
                                    ORD_ID = dbReservaLotesRomaneados[cont].ORD_ID,
                                    FPR_SEQ_TRANFORMACAO = dbReservaLotesRomaneados[cont].FPR_SEQ_TRANFORMACAO.Value,
                                    FPR_SEQ_REPETICAO = dbReservaLotesRomaneados[cont].FPR_SEQ_REPETICAO.Value,
                                    TIP_ID = "700",
                                    MOV_ARMAZEM = ParametrosSingleton.Instance.Armazem,
                                    MOV_ENDERECO = dbReservaLotesRomaneados[cont].MOV_ENDERECO,
                                    TURM_ID = cargaRomaneada.UsuarioLogado.TURM_ID,
                                    TURN_ID = dbReservaLotesRomaneados[cont].TURN_ID,
                                    CAR_ID = dbReservaLotesRomaneados[cont].CAR_ID,
                                    PlayMsgErroValidacao = msgAux,
                                    PlayAction = actionAux,
                                };

                                RomaneioConsolidado.Add(mvSaida);
                            }
                            cont++;
                            
                            //Alterando status da caraga para 6 (Consolidada)
                            if (!RomaneioConsolidado.Contains(dbCargaInformada))
                            {
                                dbCargaInformada.PlayAction = "update";
                                dbCargaInformada.PlayOrigem = "ConsolidarRomaneio";
                                dbCargaInformada.CAR_STATUS = 6.0;
                                RomaneioConsolidado.Add(dbCargaInformada);
                            }
                        }
                    }

                    #endregion

                    #region Alterar FPR_STATUS da fila de producao
                    if (check)
                    {
                        if (dbCargaInformada.PlayOrigem.Equals("ConsolidarRomaneio") && dbCargaInformada.PlayAction.Equals("update"))
                        {
                            // altera o status da ordem
                            var ordem = db.Order.AsNoTracking().Where(x => x.ORD_ID.Equals(dbCargaInformada.CAR_ID)).ToList();
                            foreach (var od in ordem)
                            {
                                od.ORD_STATUS = "E";
                                od.PlayAction = "update";
                                RomaneioConsolidado.Add(od);
                            }


                            var filaProducao = db.FilaProducao.AsNoTracking().Where(x => x.ORD_ID.Equals(dbCargaInformada.CAR_ID)).ToList();
                            foreach (var fila in filaProducao)
                            {
                                fila.FPR_STATUS = "E";
                                fila.PlayAction = "update";
                                RomaneioConsolidado.Add(fila);
                            }
                        }
                    }
                       
                    #endregion
                    
                    RomaneioConsolidado.Add(cargaRomaneada);                 
                }            
            }
            objects.AddRange(RomaneioConsolidado);

            Logs.AddRange(mc.UpdateData(new List<List<object>> { RomaneioConsolidado }, 0, true));
            if (!Logs.Any(x => x.Status.Equals("ERRO")))
            {
                foreach (var item in Logs)
                {
                    if(item.Status != "ALERT")
                        item.MsgErro = "Carga  consolidada com sucesso!";
                }
            };


            return check;
        }
        /// <summary>
        /// Realiza o estorno de uma carga previamente romaneada e consolidada assim como 
        /// de todos os seus movimentos de estoque associados
        /// </summary>
        /// <param name="objects">Carga a ser desconsolidada</param>
        /// <param name="Logs">Referência para os Logs da operação</param>
        /// <returns></returns>True para estorno com sucesso dos itens carregados
        public bool DesconsolidarRomaneio(List<object> objects, List<LogPlay> Logs)
        {
            List<object> listaAuxiliar = new List<object>();
            MasterController mc = new MasterController();
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (var item in objects)
                {
                    //Objeto  retornado da View
                    Carga carga = (Carga)item;
                    carga = db.Carga.AsNoTracking().Where(c => c.CAR_ID == carga.CAR_ID).FirstOrDefault();
                    //Recuperando Movimentos a estornar
                    var itensRomaneados = db.V_ITENS_ROMANEADOS.AsNoTracking().Where(x => x.CAR_ID.Equals(carga.CAR_ID)).Select(x => x.ORD_ID).ToList();
                    var movimentosEstornar = db.MovimentoEstoqueVendas.AsNoTracking().Where(m => m.CAR_ID.Equals(carga.CAR_ID) && !m.MOV_ESTORNO.Equals("E") && itensRomaneados.Contains(m.ORD_ID)).OrderByDescending(m => m.MOV_ID).ToList();
                    if (movimentosEstornar != null && movimentosEstornar.Count > 0)
                    {
                        carga.PlayAction = "update";
                        carga.PlayOrigem = "DesconsolidarRomaneio";
                        //Alterando o STATUS da carga para Estornada
                        carga.CAR_STATUS = 5;
                        listaAuxiliar.Add(carga);
                        foreach (var movEstornar in movimentosEstornar)
                        {
                            movEstornar.PlayAction = "update";
                            //Alterando STATUS da movimentação como estornada.
                            movEstornar.MOV_ESTORNO = "E";
                            listaAuxiliar.Add(movEstornar);
                        }
               
                        if (carga.PlayOrigem.Equals("ConsolidarRomaneio") && carga.PlayAction.Equals("update"))
                        {
                            //altera o status da ordem
                            var ordem = db.Order.AsNoTracking().Where(x => x.ORD_ID.Equals(carga.CAR_ID)).ToList();
                            foreach (var od in ordem)
                            {
                                od.ORD_STATUS = "";
                                od.PlayAction = "update";
                                listaAuxiliar.Add(od);
                            }
                            var filaProducao = db.FilaProducao.AsNoTracking().Where(x => x.ORD_ID.Equals(carga.CAR_ID)).ToList();
                            foreach (var fila in filaProducao)
                            {
                                fila.FPR_STATUS = "";
                                fila.PlayAction = "update";
                                listaAuxiliar.Add(fila);
                            }
                        }                      
                    }
                    else
                    {
                        Logs.Add(new LogPlay("ERRO", "Não foram encontrados movimentos a estornar para esta carga."));
                        return false;
                    }
                }

               
            }
            //Adicionando objetos para o update data.
            //Concatenando Logs por se tratar de um objeto de interface
            Logs.AddRange(mc.UpdateData(new List<List<object>>() { listaAuxiliar }, 3, true));
            if (!Logs.Any(x => x.Status.Equals("ERRO")))
            {
                Logs.ForEach(x => x.MsgErro = "Carga  estornada com sucesso!");
            }
            else
            {
                return false;
            }
            return true;
        }
        [HIDDEN]
        public bool MapaPontosDeEntrega(List<object> objects, ref List<LogPlay> Logs)
        {

            //Para cada item da lista
            foreach (var item in objects)
            {
                Carga _PontoMapa = (Carga)item;
                Logs.Add(new LogPlay(this.ToString(), "PROTOCOLO", "LINK", "/PlugAndPlay/PontoMapa/MapUI?id=", $"{CAR_ID}"));
            }
            return true;

        }
        [HIDDEN]
        public double GetVolumeDisponivel()
        {
            double M3Ped = 0;
            foreach (var item in ItensCarga)
            {
                M3Ped += (double)item.M3_Unitario * item.ITC_QTD_PLANEJADA;
            }
            return (double)TipoVeiculo.TIP_CAPACIDADE_M3 - M3Ped;
        }
        [HIDDEN]
        public double GetVolumeOcupado()
        {
            // PENDENCIA GRAVE TRATAR GRUPO DE PALETIZACAO 
            double Vol = 0;
            foreach (var item in ItensCarga)
            {
                Vol += (double)item.M3_Unitario * item.ITC_QTD_PLANEJADA;
            }
            return Vol;
        }
        [HIDDEN]
        public void AddPedidosParaExpedicao(List<PedidosParaExpedicao> p)
        {

        }

        //public int AddItensCargas(PedidosParaExpedicao pe)
        //{
        //    //double VolumePedidos = 0;
        //    //foreach (var item in pe.Ordens)
        //    //{
        //    //    VolumePedidos += item.QTD_SALDO_A_EXPEDIR * (double)item.M3_Unitario;
        //    //}
        //    //OptFunctions.Logs(" Capac Vei " + TipoVeiculo.TIP_CAPACIDADE_M3+ " Vol ped " + VolumePedidos + " Vol Disp vei " + this.GetVolumeDisponivel());

        //    //public static List<ContainerPackingResult> Pack(List<Container> containers, List<Item> itemsToPack, List<int> algorithmTypeIDs)
        //    //return PackingService.Pack(request.Containers, request.ItemsToPack, request.AlgorithmTypeIDs);

        ////    if (GetVolumeDisponivel() >= VolumePedidos)
        ////    {
        ////        foreach (var item in pe.Ordens)
        ////        {
        ////            //OptFunctions.Logs("try Add ->1->1 " + item.Id + " qtd " + item.QTD_SALDO_A_EXPEDIR);
        ////            ItensCarga.Add(new ItenCarga(CAR_ID, item.Id, -1, item.QTD_SALDO_A_EXPEDIR, (double)item.M3_Unitario, DateTime.Now, DateTime.Now, ""));
        ////            item.QTD_SALDO_A_EXPEDIR = 0;
        ////            //OptFunctions.Logs(" -> Sucesso. Saldo ped " + item.QTD_SALDO_A_EXPEDIR);
        ////            //OptFunctions.Logs("");
        ////        }
        ////        PedidosParaExpedicao.Add(pe);// 0 =adicionados; 1 = adicionados alterando tipo caminhao , 2 = não cabe na carga, 3 não cabe em apenas um caminhao   
        ////    }
        ////    else
        ////    {
        ////        //if (VolumePedidos > TipoVeiculo.TIP_CAPACIDADE_M3) // aqui tem que calcular para mais veiculos 
        ////        //{
        ////        //    // converte volume disponivel em caixas 
        ////        //    // se for mit a divisão de carga precisa ser proporcional 
        ////        //    foreach (var item in pe.Ordens)
        ////        //    {

        ////        //        if (item.QTD_SALDO_A_EXPEDIR * item.M3_Unitario < GetVolumeDisponivel())
        ////        //        {

        ////        //            //OptFunctions.Logs("try Add ->3->1 " + item.Id + " qtd " + item.QTD_SALDO_A_EXPEDIR);
        ////        //            ItensCarga.Add(new ItenCarga(CAR_ID, item.Id, -1, item.QTD_SALDO_A_EXPEDIR, (double)item.M3_Unitario, DateTime.Now, DateTime.Now, ""));
        ////        //            item.QTD_SALDO_A_EXPEDIR = 0;
        ////        //            //OptFunctions.Logs(" -> Sucesso. Saldo ped " + item.QTD_SALDO_A_EXPEDIR);
        ////        //            //OptFunctions.Logs("");
        ////        //        }
        ////        //        else
        ////        //        {
        ////        //            // incluiu itens de forma parcial 
        ////        //            //OptFunctions.Logs("try Add ->3->2 " + item.Id + " qtd " + item.QTD_SALDO_A_EXPEDIR);
        ////        //            int qtdItem = Convert.ToInt32(Math.Floor(GetVolumeDisponivel() / (double)item.M3_Unitario));
        ////        //            ItensCarga.Add(new ItenCarga(CAR_ID, item.Id, -1, Convert.ToDouble(qtdItem), (double)item.M3_Unitario, DateTime.Now, DateTime.Now, ""));
        ////        //            item.QTD_SALDO_A_EXPEDIR = item.QTD_SALDO_A_EXPEDIR - Convert.ToDouble(qtdItem);
        ////        //            //OptFunctions.Logs(" -> Sucesso. Saldo ped " + item.QTD_SALDO_A_EXPEDIR);
        ////        //            //OptFunctions.Logs("");
        ////        //        }
        ////        //    }
        ////            PedidosParaExpedicao.Add(pe);// 0 =adicionados; 1 = adicionados alterando tipo caminhao , 2 = não cabe na carga, 3 não cabe em apenas um caminhao   
        ////            //OptFunctions.Logs(" -> Tot itens " +this.ItensCarga.Count());
        ////            return 3;
        ////        }
        ////        //OptFunctions.Logs(" -> Tot itens " + this.ItensCarga.Count());
        ////        return 2;
        ////    }
        ////    //OptFunctions.Logs(" -> Tot itens " + this.ItensCarga.Count());
        ////    return 1;// 1 = adicionados, 2 = não cabe na carga, 3 não cabe em apenas um caminhao   
        ////}

        /// <summary>
        /// Retorna o proximo ID para insercao de uma nova carga
        /// db: Contexto do banco de dados 
        /// modo 1 Uniao de duas ou mais cargas, 2 Carga gerada manualmente pelo usuário 3- Carga gerada automaticamente pelo sistema
        /// </summary>
        /// <param name="db"></param>
        /// <param name="modo"></param>
        /// <returns></returns>
        [HIDDEN]
        private string GetNextId(JSgi db, int modo)
        {
            int IdIntAux = 0;
            string CAR_ID = "";
            switch (modo)
            {
                case 1:
                    CAR_ID = db.Carga.AsNoTracking().Where(c => c.CAR_ID.Substring(0, 1).Equals("J")).Max(c => c.CAR_ID);
                    if (!String.IsNullOrEmpty(CAR_ID))
                        IdIntAux = Convert.ToInt32(CAR_ID.Substring(1, CAR_ID.Length - 1));
                    if (IdIntAux < 1)
                    {
                        CAR_ID = "J00001";
                    }
                    else
                    {
                        IdIntAux++;
                        CAR_ID = "J";
                        CAR_ID += $"{IdIntAux:D5}";
                    }


                    break;
                case 2:
                    CAR_ID = db.Carga.AsNoTracking().Where(c => c.CAR_ID.Substring(0, 1).Equals("M")).Max(c => c.CAR_ID);
                    IdIntAux = (String.IsNullOrEmpty(CAR_ID)) ? 1 : Convert.ToInt32(CAR_ID.Substring(1, CAR_ID.Length - 1));
                    if (IdIntAux < 1)
                    {
                        CAR_ID = "M00001";
                    }
                    else
                    {
                        //Comment
                        IdIntAux++;
                        CAR_ID = "M";
                        CAR_ID += $"{IdIntAux:D5}";
                    }
                    break;
                case 3:
                    CAR_ID = db.Carga.AsNoTracking().Where(c => c.CAR_ID.Substring(0, 1).Equals("A")).Max(c => c.CAR_ID);
                    IdIntAux = (String.IsNullOrEmpty(CAR_ID)) ? 1 : Convert.ToInt32(CAR_ID.Substring(1, CAR_ID.Length - 1));
                    if (IdIntAux < 1)
                    {
                        CAR_ID = "A00001";
                    }
                    else
                    {
                        IdIntAux++;
                        CAR_ID = "A";
                        CAR_ID += $"{IdIntAux:D5}";
                    }
                    break;

            }
            return CAR_ID;
        }

        public bool RelatorioCargaConsolidada(List<object> objects, ref List<LogPlay> Logs)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                string param_bloqueio_carga = db.Param.Where(x => x.PAR_ID == "EXPEDICAO_BLOQUEIO_DE_CARGAS").Select(x => x.PAR_VALOR_S).FirstOrDefault();

                foreach (var item in objects)
                {
                    Carga _Carga = (Carga)item;

                    //não permitir impressão do relatório de romaneio se carga estiver bloqueada e EXPEDICAO_BLOQUEIO_DE_CARGAS == "AO GERAR ROMANEIO"
                    if (param_bloqueio_carga == "AO GERAR ROMANEIO" && _Carga.CAR_PESAGEM_LIBERADA != "S")
                    {
                        Logs.Add(new LogPlay(this.ToString(), "ERRO", "Romaneio gerado com bloqueio de peso balança."));
                            //"Não foi possível gerar o relatório da carga pois a carga foi bloqueada por complicações na pesagem e o parâmetro EXPEDICAO_BLOQUEIO_DE_CARGAS estava igual a 'AO GERAR ROMANEIO'"
                        continue;
                    }

                    Logs.Add(new LogPlay(this.ToString(), "PROTOCOLO", "LINK", "/PlugAndPlay/ReportCargaConsolidada/GerarPDF?CargaId=", "" + _Carga.CAR_ID + ""));
                }

                return true;
            }
        }

        public bool EmitirLaudosPedidosNaCarga(List<object> objects, ref List<LogPlay> Logs)
        {
            foreach (var item in objects)
            {
                Carga _Carga = (Carga)item;
                var db = new ContextFactory().CreateDbContext(new string[] { });
                var LaudoDaCarga = (from ltf in db.LaudoTesteFisico
                                    join itc in db.ItenCarga
                       on ltf.ORD_ID equals itc.ORD_ID
                                    where itc.CAR_ID.Equals(CAR_ID)
                                    select new
                                    {
                                        ltf.LTF_ID
                                    }).ToList();
                if (LaudoDaCarga.Count > 0)
                {
                    Logs.Add(new LogPlay(this.ToString(), "PROTOCOLO", "LINK", "/PlugAndPlay/ReportLaudoTesteFisico/LaudoTestesFisicosPorCarga?CAR_ID=", "" + _Carga.CAR_ID + ""));
                }
                else
                {
                    Logs.Add(new LogPlay(this.ToString(), "ERRO", "Não existem laudos de qualidade emitidos para esta carga."));
                    return false;
                }

            }
            return true;
        }


        /// <summary>
        /// Verifica se o peso da carga está dentro do range permitido pelo valor param_percentual_pesagem
        /// </summary>
        /// <param name="carga">Carga, será utilizada para recuperar o peso de entrada e saida</param>
        /// <param name="param_percentual_pesagem">Percentual de discrepância que pode haver entre o peso teórico e o peso real. Se o parâmetro valer 5, o percentual de diferença pode ser entre 95% e 105% do peso esperado</param>
        /// <param name="saldosEmEstoquePorLote">Uma lista de todos os lotes da carga</param>
        /// <returns>Retorna true se o percentual de diferença estiver dentro do range.</returns>
        [HIDDEN]
        private bool VerificarPeso(Carga carga, double? param_percentual_pesagem, List<SaldosEmEstoquePorLote> saldosEmEstoquePorLote)
        {
            //soma o peso teorico e peso embalagens de todos os lotes da carga
            double? peso_todas_embalagens = saldosEmEstoquePorLote.Sum(x => x.PESO_EMBALAGENS);
            double? peso_teorico_todos_lotes = saldosEmEstoquePorLote.Sum(x => x.PESO);

            //calcula o peso real e o percentual de diferença entre o peso real e o peso teórico
            double? peso_real = carga.CAR_PESO_SAIDA - carga.CAR_PESO_ENTRADA - peso_todas_embalagens;
            double? percentual_diferenca = peso_teorico_todos_lotes * 100 / peso_real;

            return percentual_diferenca > (100 - param_percentual_pesagem) && percentual_diferenca < (100 + param_percentual_pesagem);
        }

    }

    [Display(Name = "VEÍCULOS")]
    public class Veiculo
    {
        public Veiculo()
        {
            Cargas = new HashSet<Carga>();
        }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PLACA")] [Required(ErrorMessage = "Campo VEI_PLACA requirido.")] [MaxLength(8, ErrorMessage = "Maximode 8 caracteres, campo VEI_PLACA")] public string VEI_PLACA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIP_ID")] [Required(ErrorMessage = "Campo TIP_ID requirido.")] public int TIP_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CAPACIDADE_M3")] public double? VEI_CAPACIDADE_M3 { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CAPACIDADE_LARGURA")] public double? VEI_CAPACIDADE_LARGURA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CAPACIDADE_COMPRIMENTO")] public double? VEI_CAPACIDADE_COMPRIMENTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CAPACIDADE_ALTURA")] public double? VEI_CAPACIDADE_ALTURA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "MODELO")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo VEI_MODELO")] public string VEI_MODELO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NOME_MOTORISTA")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo VEI_NOME_MOTORISTA")] public string VEI_NOME_MOTORISTA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DADOS_CONTATO")] [MaxLength(4000, ErrorMessage = "Maximode * caracteres, campo VEI_DADOS_CONTATO")] public string VEI_DADOS_CONTATO { get; set; }
        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs, ref int modo_insert) {  } 


        public virtual TipoVeiculo TipoVeiculo { get; set; }
        public virtual ICollection<Carga> Cargas { get; set; }

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

    [Display(Name = "TIPOS DE VEÍCULOS")]
    public class TipoVeiculo
    {
        public TipoVeiculo()
        {
            Cargas = new HashSet<Carga>();
            Veiculos = new HashSet<Veiculo>();
            ItenCalendarioDisponibilidadeVeiculos = new HashSet<ItenCalendarioDisponibilidadeVeiculos>();

        }

        [READ] [TAB(Value = "PRINCIPAL")] [Display(Name = "COD VEÍCULO")] [Required(ErrorMessage = "Campo TIP_ID requirido.")] public int TIP_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRIÇÃO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo TIP_DESCRICAO")] public string TIP_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VEÍCULOS DISPONÍVEIS")] public int? TIP_QTD_DISPONIVEL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VALOR/KM")] public double? TIP_VALOR_KM { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VALOR/DIÁRIA")] public double? TIP_VALOR_DIARIA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VALOR/AJUDANTE")] public double? TIP_VALOR_AJUDANTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANT EIXOS")] public double? TIP_QTD_EIXOS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VELOCIDADE MÉDIA")] public double? TIP_VELOCIDADE_MEDIA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ALTURA")] public double? TIP_CAPACIDADE_ALTURA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COMPRIMENTO")] public double? TIP_CAPACIDADE_COMPRIMENTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LARGURA")] public double? TIP_CAPACIDADE_LARGURA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ALT PESCOCO DIANT")] public double? TIP_CAPACIDADE_ALTURA_PESCOCO_E { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COMP PESCOCO DIANT")] public double? TIP_CAPACIDADE_COMPRIMENTO_PESCOCO_E { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LARG PESCOCO DIANT")] public double? TIP_CAPACIDADE_LARGURA_PESCOCO_E { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ALT PESCOCO TRAS")] public double? TIP_CAPACIDADE_ALTURA_PESCOCO_D { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COMP PESCOCO TRAS")] public double? TIP_CAPACIDADE_COMPRIMENTO_PESCOCO_D { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LARG PESCOCO TRAS")] public double? TIP_CAPACIDADE_LARGURA_PESCOCO_D { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CAPACIDADE TOTAL (M³)")] public double? TIP_CAPACIDADE_M3 { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  }  


        public virtual ICollection<Carga> Cargas { get; set; }
        public virtual ICollection<Veiculo> Veiculos { get; set; }
        public virtual ICollection<ItenCalendarioDisponibilidadeVeiculos> ItenCalendarioDisponibilidadeVeiculos { get; set; }

    }

    public class ItenCarga
    {
        /// <summary>
        /// Construtores
        /// </summary>
        public ItenCarga()
        {

        }
        public ItenCarga(string carga, string ped, int ordemEntrega, double Qtd, double M3_Unit, DateTime Entrega, DateTime EntregaRealisada, string ORD_HASH_KEY)
        {
            ORD_ID = ped;
            CAR_ID = carga;
            M3_Unitario = M3_Unit;
            ITC_ORDEM_ENTREGA = ordemEntrega;
            ITC_QTD_PLANEJADA = Qtd;
            ITC_QTD_REALIZADA = 0;
            ITC_ENTREGA_PLANEJADA = Entrega;
            ITC_ENTREGA_REALIZADA = EntregaRealisada;
            this.ORD_HASH_KEY = ORD_HASH_KEY;
        }
        public ItenCarga(string carga, string ped, int ordemEntrega, double qtd, double m3_Unit, DateTime entrega)
        {
            this.carga = carga;
            this.ped = ped;
            this.ordemEntrega = ordemEntrega;
            this.qtd = qtd;
            this.m3_Unit = m3_Unit;
            this.entrega = entrega;
        }

        public ItenCarga(ItenCarga item)
        {
            this.ORD_ID = item.ORD_ID;
            this.CAR_ID = item.CAR_ID;

            this.ITC_QTD_PLANEJADA = item.ITC_QTD_PLANEJADA;
            this.ITC_QTD_REALIZADA = item.ITC_QTD_REALIZADA;
            this.ITC_ORDEM_ENTREGA = item.ITC_ORDEM_ENTREGA;
            this.ITC_ENTREGA_PLANEJADA = item.ITC_ENTREGA_PLANEJADA;
            this.ITC_ENTREGA_REALIZADA = item.ITC_ENTREGA_REALIZADA;
            this.ORD_HASH_KEY = item.ORD_HASH_KEY;
            this.PlayAction = item.PlayAction;
            this.PlayMsgErroValidacao = item.PlayMsgErroValidacao;
        }
        //--
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CARGA")] [Required(ErrorMessage = "Campo CAR_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo CAR_ID")] public string CAR_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PEDIDO")] [Required(ErrorMessage = "Campo ORD_ID requirido.")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ENTREGA_PLANEJADA")] [Required(ErrorMessage = "Campo ITC_ENTREGA_PLANEJADA requirido.")] public DateTime ITC_ENTREGA_PLANEJADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ENTREGA_REALIZADA")] [Required(ErrorMessage = "Campo ITC_ENTREGA_REALIZADA requirido.")] public DateTime ITC_ENTREGA_REALIZADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORDEM_ENTREGA")] [Required(ErrorMessage = "Campo ITC_ORDEM_ENTREGA requirido.")] public int ITC_ORDEM_ENTREGA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD_PLANEJADA")] [Required(ErrorMessage = "Campo ITC_QTD_PLANEJADA requirido.")] public double ITC_QTD_PLANEJADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD_REALIZADA")] [Required(ErrorMessage = "Campo ITC_QTD_REALIZADA requirido.")] public double ITC_QTD_REALIZADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "HASH_KEY")] [MaxLength(300, ErrorMessage = "Maximode 300 caracteres, campo ORD_HASH_KEY")] public string ORD_HASH_KEY { get; set; }
        //--- Uso no CLP
        private DateTime now;
        private string v;
        private string carga;
        private string ped;
        private int ordemEntrega;
        private double qtd;
        private double m3_Unit;
        private DateTime entrega;
        //--
        public virtual Carga Carga { get; set; }
        public virtual Order Oredr { get; set; }
        [NotMapped] public double? M3_Unitario { get; set; }
        [NotMapped] public T_Usuario UsuarioLogado { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {

            bool check = true;
            List<object> listaAux = new List<object>();
            MasterController mc = new MasterController();
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                List<object> apontamentoProducao = new List<object>();
                ItenCarga itemRomanear = null;
                bool romaneioSimplificado = db.Param.Any(x => x.PAR_ID.Equals("ROMANEIO_SIMPLIFICADO") && x.PAR_VALOR_S.Equals("S"));
                foreach (var item in objects)
                {
                    if (item.GetType().Name == nameof(ItenCarga))
                    {
                        itemRomanear = (ItenCarga)item;
                        var clone = (ItenCarga)cloneObjeto.GetClone(item);
                        bool isRomaneado = db.MovimentoEstoqueReservaDeEstoque.Any(x => x.ORD_ID.Equals(itemRomanear.ORD_ID) && x.CAR_ID.Equals(itemRomanear.CAR_ID) && !x.MOV_ESTORNO.Equals("E"));
                        switch (itemRomanear.PlayAction)
                        {
                            case "insert":

                                break;
                            case "update":
                                if (!isRomaneado)
                                {
                                    if (itemRomanear.ITC_QTD_REALIZADA > 0)
                                    {
                                        if (romaneioSimplificado && itemRomanear.ITC_QTD_REALIZADA != clone.ITC_QTD_REALIZADA)
                                        {
                                            //itemRomanear.PlayAction = "OK";
                                            #region Gerar etiqueta
                                            string pedidoEtiqueta = "", produtoetiqueta = "", maquinaEtiqeta = "";
                                            int seqRepPedidoetiqueta = 1;
                                            int qtdEtiquetas = 1;
                                            var infoPed = (from o in db.Order where o.ORD_ID.Equals(itemRomanear.ORD_ID) select new { o.ORD_ID, o.ORD_TIPO, o.PRO_ID }).FirstOrDefault(); ;

                                            //Recuperando informaçoes do OP para gerear etiqueta.
                                            if (infoPed.ORD_TIPO == 3)
                                            {
                                                // Encontrar pedido com mesmo produto mas com tipo produção. 
                                                var ped = (from fila in db.FilaProducao
                                                           join pedido in db.Order on fila.ORD_ID equals pedido.ORD_ID
                                                           where (pedido.ORD_TIPO == 2 || pedido.ORD_TIPO == 4) && fila.ROT_PRO_ID == infoPed.PRO_ID
                                                           orderby fila.ROT_SEQ_TRANFORMACAO descending
                                                           select new { fila.ORD_ID, fila.ROT_MAQ_ID, fila.ROT_PRO_ID, fila.FPR_SEQ_REPETICAO, pedido.ORD_TIPO })
                                                         .Take(1)
                                                         .FirstOrDefault();
                                                if (ped != null)
                                                {
                                                    pedidoEtiqueta = ped.ORD_ID;
                                                    produtoetiqueta = ped.ROT_PRO_ID;
                                                    seqRepPedidoetiqueta = ped.FPR_SEQ_REPETICAO;
                                                    maquinaEtiqeta = ped.ROT_MAQ_ID;
                                                    qtdEtiquetas = 1;

                                                }
                                                else
                                                {
                                                    itemRomanear.PlayMsgErroValidacao = $"Não foi encontrado pedido de plrodução para o pedido de faturamento [{itemRomanear.ORD_ID}].";
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                var info = (from fila in db.FilaProducao
                                                            where fila.ORD_ID.Equals(itemRomanear.ORD_ID)
                                                            orderby fila.ROT_SEQ_TRANFORMACAO descending
                                                            select new { fila.ORD_ID, fila.ROT_MAQ_ID, fila.ROT_PRO_ID, fila.FPR_SEQ_REPETICAO })
                                                                 .Take(1)
                                                                 .FirstOrDefault();

                                                qtdEtiquetas = 1;//PENDENCIA PEGAR O MAX ETIQUETA CASO SEJA N ROMANEIO COM MESMO PEDIDO 
                                                seqRepPedidoetiqueta = info.FPR_SEQ_REPETICAO;
                                                pedidoEtiqueta = info.ORD_ID;
                                                produtoetiqueta = info.ROT_PRO_ID;
                                                maquinaEtiqeta = info.ROT_MAQ_ID;
                                            }

                                            //Recuperando id da impressora da maquina.
                                            int impId = db.MaquinaImpressora.Where(x => x.MAQ_ID.Equals(maquinaEtiqeta)).Select(x => x.IMP_ID).FirstOrDefault();
                                            int Sequencia = db.Etiqueta.Where(x => x.ORD_ID== pedidoEtiqueta).Max(x=>x.ETI_SEQUENCIA).GetValueOrDefault(0);
                                            Sequencia++;

                                            mc.UsuarioLogado = itemRomanear.UsuarioLogado;
                                            InterfaceTelaImpressaoEtiquetas dadosEtiqueta = new InterfaceTelaImpressaoEtiquetas
                                            {
                                                ORD_ID = pedidoEtiqueta,
                                                ROT_PRO_ID = produtoetiqueta,
                                                FPR_SEQ_REPETICAO = seqRepPedidoetiqueta,
                                                MAQ_ID = maquinaEtiqeta,
                                                ETI_NUMERO_COPIAS = 2,
                                                ETI_IMPRIMIR_DE = Sequencia,
                                                ETI_IMPRIMIR_ATE = Sequencia,
                                                ETI_QUANTIDADE_PALETE = itemRomanear.ITC_QTD_REALIZADA,
                                                IMP_ID = impId,
                                                //IMPRIMIR_AGORA = "N",
                                                PlayAction = "insert",
                                            };

                                            listaAux.Add(dadosEtiqueta);
                                            Logs.AddRange(mc.UpdateData(new List<List<object>>() { listaAux }, 0, true));
                                            check = !Logs.Any(x => x.Status.Equals("ERRO"));
                                            #endregion
                                            if (check)
                                            {
                                                #region Apontar produção dos lotes
                                                //Etiquetas Geradas com Sucesso, Iniciando processo de apontamento de produção
                                                //Criando Movimentos de estoque 001 998 e modificando o status da etiquta para P
                                                //Através do BeforeChanges de ProducaoCodigoBarras
                                                List<Etiqueta> etiquetasGeradas = new List<Etiqueta>();
                                                foreach (var e in listaAux)
                                                {
                                                    if (e.GetType().Name == nameof(Etiqueta))
                                                    {
                                                        Etiqueta etiqueta = (Etiqueta)e;
                                                        if (etiqueta.PlayAction != "ERRO")
                                                        {
                                                            etiquetasGeradas.Add(etiqueta);
                                                        }
                                                    }
                                                }
                                                //var etiquetasGeradas = db.Etiqueta.AsNoTracking()
                                                //                                  .Where(e => e.ORD_ID == pedidoEtiqueta &&
                                                //                                              e.ROT_PRO_ID == produtoetiqueta &&
                                                //                                              e.FPR_SEQ_REPETICAO == seqRepPedidoetiqueta &&
                                                //                                              e.ETI_SEQUENCIA == Sequencia )
                                                //                                  .Select(e => new { e.ETI_CODIGO_BARRAS }).ToList();
                                                listaAux.Clear();

                                                foreach (var etiqueta in etiquetasGeradas)
                                                {
                                                    listaAux.Add(new ProducaoCodigoBarras { CodigoDeBarras = etiqueta.ETI_CODIGO_BARRAS, PlayAction = "OK" });
                                                }
                                                // aponta produção
                                                Logs.AddRange(mc.UpdateData(new List<List<object>>() { listaAux }, 0, true));
                                                check = !Logs.Any(x => x.Status.Equals("ERRO"));
                                                #endregion
                                                if (check)
                                                {
                                                    #region Romaneio dos lotes
                                                    //Produção apontada com sucesso,iniciando processo de romaneio dos lotes apontados
                                                    //Alterando registro das cargas e pedidos,  movimento de reserva (998) e status da carga para 5 (carregando)
                                                    //Através do BeforeChanges de Romaneio
                                                    listaAux.Clear();

                                                    foreach (var etiqueta in etiquetasGeradas)
                                                    {
                                                        //listaAux.Add(new Romaneio { CodigoDeBarras = etiqueta.ETI_CODIGO_BARRAS, CargaId = itemRomanear.CAR_ID, INCLUIR = "S", TIPO = infoPed.ORD_TIPO.Value, PedidoOriginal = infoPed.ORD_ID, PlayAction = "OK" });
                                                        listaAux.Add(new Romaneio { CodigoDeBarras = etiqueta.ETI_CODIGO_BARRAS, CargaId = itemRomanear.CAR_ID, INCLUIR = "S", TIPO = infoPed.ORD_TIPO.Value, PlayAction = "OK" });
                                                    }
                                                    Logs.AddRange(mc.UpdateData(new List<List<object>>() { listaAux }, 0, true));
                                                    check = !Logs.Any(x => x.Status.Equals("ERRO"));
                                                    #endregion
                                                }
                                            }
                                            if (!check)
                                            {
                                                #region Estorno do processo em caso de erro
                                                string msg = "";
                                                try
                                                {
                                                    var sql = @"EXEC SP_ESTORNAR_ROMANEIO @CAR_ID,@ORD_ID";
                                                    int res = db.Database.ExecuteSqlCommand(sql, new SqlParameter("@CAR_ID", itemRomanear.CAR_ID),
                                                                                               new SqlParameter("@ORD_ID", itemRomanear.ORD_ID));
                                                }
                                                catch (Exception ex)
                                                {
                                                    msg = (ex.InnerException != null) ? $"{ex.Message} {ex.InnerException.Message}" : ex.Message;
                                                }
                                                msg += String.Join("\n", Logs.Where(x => x.Status.Equals("ERRO")).Select(x => x.MsgErro).ToList());
                                                Logs.Clear();
                                                Logs.Add(new LogPlay(this.ToString(), "ERRO", msg));
                                                return false;
                                                #endregion
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    itemRomanear.PlayMsgErroValidacao = "Este Pedido/Lote já se encontra Romaneado. Retire o pedido/Lote do Romaneio antes de alterar.";
                                }
                                break;
                            case "delete":
                                if (isRomaneado)
                                    itemRomanear.PlayMsgErroValidacao = "Este Pedido/Lote já se encontra Romaneado, você deve excluir este item do romaneio antes.";
                                break;
                        }
                    }
                }
                //objects.Add(itemRomanear);
            }
            return check;
        }
    }
    //--
    public class V_ITENS_ROMANEADOS
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CODIGO DA CARGA")] [Required(ErrorMessage = "Informe o código do pedido. (CAR_ID)")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo CAR_ID")] public string CAR_ID { get; set; }
        [SEARCH_NOT_FK(namespaceOfForeignClass = "DynamicForms.Areas.PlugAndPlay.Models.Order")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CODIGO DO PEDIDO")] [Required(ErrorMessage = "Informe o código do pedido.(ORD_ID)")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [SEARCH_NOT_FK(namespaceOfForeignClass = "DynamicForms.Areas.PlugAndPlay.Models.Produto")]

        [TAB(Value = "OUTROS")] [Display(Name = "PRO ID CONJUNTO")] [MaxLength(30, ErrorMessage = "PRO_ID_CONJUNTO")] public string PRO_ID_CONJUNTO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "ORD ID CONJUNTO")] [MaxLength(30, ErrorMessage = "ORD_ID_CONJUNTO")] public string ORD_ID_CONJUNTO { get; set; }

        [TAB(Value = "OUTROS")] [Display(Name = "CODIGO DO PRODUTO")] [MaxLength(30, ErrorMessage = "Informe o código do produto com no máximo 30 car. (PRO_ID)")] public string PRO_ID { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "ENTREGA_PLANEJADA")] [Required(ErrorMessage = "Campo ITC_ENTREGA_PLANEJADA requirido.")] public DateTime ITC_ENTREGA_PLANEJADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD_PLANEJADA")] [Required(ErrorMessage = "Campo ITC_QTD_PLANEJADA requirido.")] public double ITC_QTD_PLANEJADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD_ROMANEADA")] [Required(ErrorMessage = "Você deve informar a quantidade realizada da carga. (QTD_ROMANEADA)")] public double? QTD_ROMANEADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD_CONSOLIDADA")] [Required(ErrorMessage = "Você deve informar a quantidade realizada da carga. (QTD_CONSOLIDADA)")] public double? QTD_CONSOLIDADA { get; set; }

        [TAB(Value = "OUTROS")] [Display(Name = "EST_QUANT")] [Required(ErrorMessage = "Qtd estrutura. (EST_QUANT)")] public double? EST_QUANT { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "QTD_CONJUNTOS")] [Required(ErrorMessage = "Qtd Cunjuntos (QTD_CONJUNTOS)")] public double? QTD_CONJUNTOS { get; set; }

        [TAB(Value = "OUTROS")] [Display(Name = "PERCENTUAL_EMBARCADO")] public double? PERCENTUAL_EMBARCADO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "ORDEM_ENTREGA")] public int ITC_ORDEM_ENTREGA { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "QTD_PALETES")] public int QTD_PALETES { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "TOLERANCIA_MAIS")] public double? ORD_TOLERANCIA_MAIS { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "TOLERANCIA_MENOS")] public double? ORD_TOLERANCIA_MENOS { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "HASH_KEY")] public string ORD_HASH_KEY { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE DO PEDIDO")] public double ORD_QUANTIDADE { get; set; }


        public virtual Carga Carga { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        [NotMapped] public T_Usuario UsuarioLogado { get; set; }

        public V_ITENS_ROMANEADOS()
        {

        }
        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            bool check = true;
            List<object> listaAux = new List<object>();
            MasterController mc = new MasterController();
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                List<object> apontamentoProducao = new List<object>();
                V_ITENS_ROMANEADOS itemRomanear = new V_ITENS_ROMANEADOS();

                bool romaneioSimplificado = db.Param.Any(x => x.PAR_ID.Equals("ROMANEIO_SIMPLIFICADO") && x.PAR_VALOR_S.Equals("S"));

                foreach (var item in objects)
                {
                    itemRomanear = (V_ITENS_ROMANEADOS)item;
                    bool isRomaneado = db.MovimentoEstoqueReservaDeEstoque.Any(x => x.ORD_ID.Equals(itemRomanear.ORD_ID) && x.CAR_ID.Equals(itemRomanear.CAR_ID) && !x.MOV_ESTORNO.Equals("E"));
                    switch (itemRomanear.PlayAction)
                    {
                        case "insert":
                            itemRomanear.PlayMsgErroValidacao = "Operação Inválida, você não pode incluir um item no romaneio por esta interface.";
                            break;
                        case "update":
                            itemRomanear.PlayMsgErroValidacao = "Operação Inválida, você não pode alterar um item no romaneio por esta interface.";
                            break;
                        case "delete":
                            if (romaneioSimplificado)
                            {
                                var itensRomaneados = db.V_ITENS_ROMANEADOS.AsNoTracking().Where(x => x.CAR_ID.Equals(itemRomanear.CAR_ID)).Select(x => x.ORD_ID).ToList();
                                bool isConsolidado = db.MovimentoEstoqueVendas.AsNoTracking().Any(x => x.TIP_ID == "700" && !x.MOV_ESTORNO.Equals("E") && itensRomaneados.Contains(x.ORD_ID));
                                if (!isConsolidado)
                                {
                                    #region Estorno do processo quando excluido um item do romaneio
                                    string msg = "";
                                    try
                                    {
                                        itemRomanear.PlayAction = "OK";
                                        var sql = @"EXEC SP_ESTORNAR_ROMANEIO @CAR_ID,@ORD_ID";
                                        int res = db.Database.ExecuteSqlCommand(sql, new SqlParameter("@CAR_ID", itemRomanear.CAR_ID),
                                                                                   new SqlParameter("@ORD_ID", itemRomanear.ORD_ID));

                                    }
                                    catch (Exception ex)
                                    {
                                        msg = (ex.InnerException != null) ? $"{ex.Message} {ex.InnerException.Message}" : ex.Message;
                                    }
                                    msg += String.Join("\n", Logs.Where(x => x.Status.Equals("ERRO")).Select(x => x.MsgErro).ToList());
                                    itemRomanear.PlayMsgErroValidacao = msg;
                                    #endregion
                                }
                                else
                                {
                                    itemRomanear.PlayMsgErroValidacao = "Operação inválida, você não pode excluir um item de uma carga já CONSOLIDADA.";
                                }

                            }
                            else
                            {
                                itemRomanear.PlayMsgErroValidacao = "Operação Inválida, você não pode excluir um item do romaneio por esta interface.";
                            }
                            break;
                    }
                }
                objects.Add(itemRomanear);
            }
            return check;
        }

    }


    //--
    [Display(Name = "PONTOS DO MAPA")]
    public class PontosMapa
    {
        public PontosMapa()
        {
            RotaPontosMapaDestino = new HashSet<RotaPontosMapa>();
            RotaPontosMapaOrigem = new HashSet<RotaPontosMapa>();
            RotaPontosMapaRoteiro = new HashSet<RotaPontosMapa>();
            Mapa_ID = new HashSet<Mapa>();
            Mapa_ID_VIZINHO = new HashSet<Mapa>();
            Order = new HashSet<Order>();
        }
        public PontosMapa(string id, string descricao, string tipo, decimal latitude, decimal longitude)
        {
            this.PON_ID = id;
            this.PON_DESCRICAO = descricao;
            this.PON_TIPO = tipo;
            this.PON_LATITUDE = latitude;
            this.PON_LONGITUDE = longitude;
        }


        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PONTO")] [Required(ErrorMessage = "Campo PON_ID requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PON_ID")] public string PON_ID { get; set; }
        [SEARCH]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRIÇÃO")] [Required(ErrorMessage = "Campo PON_DESCRICAO requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PON_DESCRICAO")] public string PON_DESCRICAO { get; set; }
        [Combobox(Description = "CIDADE", Value = "CID")]
        [Combobox(Description = "RUA", Value = "RUA")]
        [Combobox(Description = "RODOVIA", Value = "ROD")]
        [Combobox(Description = "CLIENTE", Value = "CLI")]
        [Combobox(Description = "BAIRRO", Value = "BAI")]
        [Combobox(Description = "REGIAO", Value = "REG")]
        [Combobox(Description = "CEP", Value = "CEP")]
        [Combobox(Description = "LATITUDE LONGITUDE", Value = "LLE")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PONTO")] [Required(ErrorMessage = "Campo PON_TIPO requirido.")] [MaxLength(3, ErrorMessage = "Maximode 3 caracteres, campo PON_TIPO")] public string PON_TIPO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LATITUDE")] public decimal PON_LATITUDE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LONGITUDE")] public decimal PON_LONGITUDE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LONGITUDE")] public double PON_DISTANCIA_KM { get; set; }

        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  } 

        public virtual ICollection<RotaPontosMapa> RotaPontosMapaDestino { get; set; }
        public virtual ICollection<RotaPontosMapa> RotaPontosMapaOrigem { get; set; }
        public virtual ICollection<RotaPontosMapa> RotaPontosMapaRoteiro { get; set; }
        public virtual ICollection<Mapa> Mapa_ID { get; set; }
        public virtual ICollection<Mapa> Mapa_ID_VIZINHO { get; set; }
        public virtual ICollection<Order> Order { get; set; }
        //Metodos de classe
        public bool MapaVizinhos(List<object> objects, ref List<LogPlay> Logs)
        {
            //Criando um objeto para a nova carga
            string PonId = null;

            //Para cada item da lista
            foreach (var item in objects)
            {
                PontosMapa _PontoMapa = (PontosMapa)item;
                PonId = _PontoMapa.PON_ID;
                _PontoMapa.PlayAction = "OK";
            }
            //Concatenando Logs por se tratar de um objeto de interface
            Logs.Add(new LogPlay(this.ToString(), "PROTOCOLO", "LINK", "/PlugAndPlay/PontoMapa/MapUI?id=", "" + PonId + ""));
            return true;
        }
    }

    [Display(Name = "ROTAS FACTÍVEIS")]
    public class RotaPontosMapa
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD ROTA")] [Required(ErrorMessage = "Campo ROT_ID requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo ROT_ID")] public string ROT_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD DESTINO")] [Required(ErrorMessage = "Campo PON_ID_DESTINO requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PON_ID_DESTINO")] public string PON_ID_DESTINO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD ORIGEM")] [Required(ErrorMessage = "Campo PON_ID_ORIGEM requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PON_ID_ORIGEM")] public string PON_ID_ORIGEM { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CUSTO TOTAL")] public double? ROT_CUSTO_TOTAL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD ROTEIRO ENTREGA")] [Required(ErrorMessage = "Campo PON_ID_ROTEIRO requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PON_ID_ROTEIRO")] public string PON_ID_ROTEIRO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORDEM ROTEIRO")] public int? ROT_ORDEM_ROTEIRO { get; set; }
        [Combobox(Description = "A ESTRELA", Value = "AE")]
        [Combobox(Description = "MANUAL", Value = "MA")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ROTA")] [MaxLength(2, ErrorMessage = "Maximode 2 caracteres, campo ROT_TIPO")] public string ROT_TIPO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DISTÂNCIA")] public double? ROT_DISTANCIA { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  }

        public virtual PontosMapa PontoMapaDestino { get; set; }
        public virtual PontosMapa PontoMapaOrigem { get; set; }
        public virtual PontosMapa PontoMapaRoteiro { get; set; }

    }
    public class PedidosParaExpedicao
    {
        public List<OrderOpt> Ordens { get; set; } // deve ser uma lista pois existem pediddos a expedir que devem ser expedidos juntos como exemplo conjuntos 
        public List<Rota> RotasFactiveis { get; set; }
        public List<Carga> Cargas { get; set; }
        public string ClienteId { get; set; }
        public string PON_DESTINO { get; set; }


        // equivalem a poto porem para ficar pratico e mais performatico de ordenar incluiremos como propriedades diretas da classe 
        public string UF { get; set; }
        public string Municipio { get; set; }
        public string MunicipioID { get; set; }
        public string Bairro { get; set; }
        public string Rua { get; set; }
        public string CEP { get; set; }
        public string Numero { get; set; }
        public DateTime EntregaDe { get; set; }
        public DateTime EntregaAte { get; set; }
        public DateTime InicioJanelaEmbarque { get; set; }
        public DateTime FimJanelaEmbarque { get; set; }
        public DateTime EmbarqueAlvo { get; set; }
        public DateTime PrevisaoEntrega { get; set; }
        public double GrupoEmbarque { get; set; }
        public double SequenciaEntrega { get; set; }
        public double Translado { get; set; }
        public int CAR_STATUS { get; set; }   // -2.CARGA DELETADA PEDIDO HASHKEY DIFERENTE, -1.CARGA DELETADA OUTRO PEDIDO DA CARGA COM HASHKEY DIFERENTE, 0.PEDIDO SEM CARGA; 1.PEDIDO COM CARGA 


        public PedidosParaExpedicao()
        {
            this.RotasFactiveis = new List<Rota>();
            this.Ordens = new List<OrderOpt>();

            this.Cargas = new List<Carga>();
        }
        public void AddOrdens(OrderOpt o)
        {
            // TESTA SE TRATA-SE DE TRIANGULAR 
            bool triangular = false;
            this.ClienteId = o.ClienteId;


            //System.Console.WriteLine(o.Id);
            if (o.PON_ID_MUN != "" && o.PON_ID_MUN != null)
            {
                if (o.PON_ID_MUN != o.CLI_MUN_ID_ENTREGA)
                {
                    triangular = true;
                }
                this.PON_DESTINO = o.PON_ID_MUN;
            }
            if (o.PON_ID_REG != "" && o.PON_ID_REG != null)
            {
                if (triangular)
                {
                    if (o.PON_ID_REG != "" && o.PON_ID_REG != null && o.CLI_REGIAO_ENTREGA.Trim() != o.PON_ID_REG.Trim())
                    {// A REGIAO É TRIANGULAR TAMBEM. ESSA CONDIÇÃO É ASSIM POIS SE O CLIENTE POSSUIR REGIAO E FAS UM TRIANGULAR APENAS PARA OUTRA CIDADE A TRIANGULAÇÃO NAO DEVE OLHAR PARA HIERARQUIA DE REGIAO POIS SENÃO IREIA SOBREPOR.
                        this.PON_DESTINO = o.PON_ID_REG;
                    }
                }
                else
                {
                    this.PON_DESTINO = o.PON_ID_REG;
                }
            }
            this.UF = o.UF_COD;
            this.Municipio = o.MUN_NOME;
            this.MunicipioID = o.MUN_ID;
            this.Bairro = o.BairroEntrega;
            this.Rua = o.EnderecoEntrega;
            this.CEP = o.CEPEntrega;

            this.EntregaDe = o.DataEntregaDe;
            this.EntregaAte = o.DataEntregaAte;
            this.InicioJanelaEmbarque = o.InicioJanelaEmbarque;
            this.FimJanelaEmbarque = o.FimJanelaEmbarque;
            //this.GrupoEmbarque = o.GrupoEmbarque;


            //this.Numero = o.Cliente.;
            this.Ordens.Add(o);
        }


        public PedidosParaExpedicao DeepCopy()
        {
            List<OrderOpt> NewOrdens = new List<OrderOpt>();

            PedidosParaExpedicao other = (PedidosParaExpedicao)this.MemberwiseClone();
            foreach (var item in Ordens)
            {
                //NewOrdens.Add(item.DeepCopy());
            }
            other.Ordens = new List<OrderOpt>();
            other.Ordens = NewOrdens;
            return other;
        }

    }

    public class Rota
    {
        public List<RotaPontosMapa> ItensDaRota { get; set; }
        public double Distancia { get; set; }
        public double TempoTranslado { get; set; }

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
    public class TemposLogistica
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TEMPO")] [Required(ErrorMessage = "Campo TMP_TIPO_TEMPO requirido.")] [MaxLength(50, ErrorMessage = "Maximo 50 caracteres, campo TMP_TIPO_TEMPO")] public string TMP_TIPO_TEMPO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO DE CARGA")] [Required(ErrorMessage = "Campo TMP_TIPO_CARGA requirido.")] [MaxLength(50, ErrorMessage = "Maximo 50 caracteres, campo TMP_TIPO_CARGA")] public string TMP_TIPO_CARGA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TEMPO MÉDIO/UNI")] [Required(ErrorMessage = "Campo TMP_TEMPO_MEDIO_UNITARIO requirido.")] public double TMP_TEMPO_MEDIO_UNITARIO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD CLIENTE")] [MaxLength(30, ErrorMessage = "Maximo 30 caracteres, campo CLI_ID")] public string CLI_ID { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  } 
    }
    public class Mapa
    {
        public Mapa()
        {
            RestricoesDeRodagem = new HashSet<RestricoesDeRodagem>();
        }
        [READ] [TAB(Value = "PRINCIPAL")] [Display(Name = "COD MAPA")] [Required(ErrorMessage = "Campo MAP_ID requirido.")] public int MAP_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PONTO MAPA")] [Required(ErrorMessage = "Campo PON_ID requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PON_ID")] public string PON_ID { get; set; }
        [EDIT] [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PONTO MAPA VIZINHO")] [Required(ErrorMessage = "Campo PON_ID_VIZINHO requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PON_ID_VIZINHO")] public string PON_ID_VIZINHO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DISTÂNCIA")] [Required(ErrorMessage = "Campo MAP_DISTANCIA requirido.")] public double MAP_DISTANCIA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PEDÁGIO PREÇO/EIXO")] public double? MAP_CUSTO_PEDAGIO_POR_EIXO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD RODOVIA")] public int? ROD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ALTURA RODOVIA")] public double MAP_ALTURA_ROD { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        public virtual PontosMapa PontosMapa_ID { get; set; }
        public virtual PontosMapa PontosMapa_ID_VIZINHO { get; set; }
        public virtual Rodovias Rodovias { get; set; }
        public ICollection<RestricoesDeRodagem> RestricoesDeRodagem { get; set; }

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            List<object> MapasGerados = new List<object>();
            Mapa mapa = null;
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (var item in objects)
                {
                    //Objeto retornado da View
                    Mapa _Mapa = (Mapa)item;
                    //Setando action do objeto da view como OK para que seja ignorado pela  funçao UpdateData

                    //Validaçoes --
                    if (_Mapa.PON_ID.Trim() == _Mapa.PON_ID_VIZINHO)
                    {
                        _Mapa.PlayMsgErroValidacao = "Os pontos no mapa e seus vizinhos não podem ser iguais, verifique os dados.";
                        return false;
                    }

                    if (_Mapa.PlayAction.ToLower() == "insert" && db.Mapa.AsNoTracking().Where(pm => pm.PON_ID == _Mapa.PON_ID && pm.PON_ID_VIZINHO == _Mapa.PON_ID_VIZINHO).ToList().Count() > 0)
                    {
                        _Mapa.PlayMsgErroValidacao = "Este ponto no mapa e seu vizinho ja esta cadastrado.";
                        return false;
                    }

                    var Db_Mapa = db.Mapa.AsNoTracking().Where(pm => pm.PON_ID == _Mapa.PON_ID_VIZINHO && pm.PON_ID_VIZINHO == _Mapa.PON_ID).FirstOrDefault();

                    //Insert 
                    if (Db_Mapa == null && _Mapa.PlayAction.ToLower() != "delete")
                    {
                        mapa = (Mapa)_Mapa.MemberwiseClone();
                        mapa.PON_ID = _Mapa.PON_ID_VIZINHO;
                        mapa.PON_ID_VIZINHO = _Mapa.PON_ID;
                        mapa.PlayAction = "insert";
                        MapasGerados.Add(mapa);
                    }
                    else//Update e delete
                    {
                        if (Db_Mapa != null)
                        {
                            Db_Mapa.MAP_DISTANCIA = _Mapa.MAP_DISTANCIA;
                            Db_Mapa.MAP_CUSTO_PEDAGIO_POR_EIXO = _Mapa.MAP_CUSTO_PEDAGIO_POR_EIXO;
                            Db_Mapa.ROD_ID = _Mapa.ROD_ID;
                            Db_Mapa.MAP_ALTURA_ROD = _Mapa.MAP_ALTURA_ROD;
                            Db_Mapa.PlayAction = _Mapa.PlayAction;
                            MapasGerados.Add(Db_Mapa);
                        }
                    }
                }
                //Adicionando mapa a lista de objetos da funcao BeforeCheanges
                if (MapasGerados.Count != 0)
                {
                    objects.AddRange(MapasGerados);
                }
            }
            return true;
        }
    }
    public class CargasWeb
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TRIANGULAR5")] [Required(ErrorMessage = "Campo TRIANGULAR5 requirido.")] [MaxLength(2, ErrorMessage = "Maximode 2 caracteres, campo TRIANGULAR5")] public string TRIANGULAR5 { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "UF")] [MaxLength(2, ErrorMessage = "Maximode 2 caracteres, campo UF")] public string UF { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "MUN")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MUN")] public string MUN { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PON_ID")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PON_ID")] public string PON_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PON_DESCRICAO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PON_DESCRICAO")] public string PON_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CLI_NOME")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo CLI_NOME")] public string CLI_NOME { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CLI_ID")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo CLI_ID")] public string CLI_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "EMBARQUE")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo EMBARQUE")] public string EMBARQUE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA DE EMBARQUE")] public DateTime DTEMBARQUE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORD_ID")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRO_ID")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORD_QUANTIDADE")] public double? ORD_QUANTIDADE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORD_PESO_UNITARIO")] public double? ORD_PESO_UNITARIO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD_ENTREGUE")] [Required(ErrorMessage = "Campo QTD_ENTREGUE requirido.")] public double QTD_ENTREGUE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ITC_QTD_PLANEJADA")] [Required(ErrorMessage = "Campo ITC_QTD_PLANEJADA requirido.")] public double ITC_QTD_PLANEJADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE UE")] public double? QTD_UE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "M3_PLANEJADO")] public double? M3_PLANEJADO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "M3_UE")] public double? M3_UE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "M3_PEDIDO")] public double? M3_PEDIDO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRO_LARGURA_EMBALADA")] public double? PRO_LARGURA_EMBALADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRO_COMPRIMENTO_EMBALADA")] public double? PRO_COMPRIMENTO_EMBALADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRO_ALTURA_EMBALADA")] public double? PRO_ALTURA_EMBALADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TRIANGULAR")] [Required(ErrorMessage = "Campo TRIANGULAR requirido.")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo TRIANGULAR")] public string TRIANGULAR { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SALDO_ESTOQUE")] [Required(ErrorMessage = "Campo SALDO_ESTOQUE requirido.")] public double SALDO_ESTOQUE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SALDO_ESTOQUE_UE")] public double? SALDO_ESTOQUE_UE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PECENT_ESTOQUE_PRONTO")] public double? PECENT_ESTOQUE_PRONTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FIM PREVISTO PRODUÇÃO")] [Required(ErrorMessage = "Campo FIMPREVISTO requirido.")] public DateTime FIMPREVISTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CAR_ID")] [Required(ErrorMessage = "Campo CAR_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo CAR_ID")] public string CAR_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ITC_ORDEM_ENTREGA")] [Required(ErrorMessage = "Campo ITC_ORDEM_ENTREGA requirido.")] public int ITC_ORDEM_ENTREGA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIP_ID")] public int? TIP_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CAR_DATA_INICIO_PREVISTO")] public DateTime CAR_DATA_INICIO_PREVISTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FIM PREVISTO CARGA")] public DateTime CAR_DATA_FIM_PREVISTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CAR_STATUS")] public double? CAR_STATUS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COR_FILA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo FPR_COR_FILA")] public string FPR_COR_FILA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PESO_TEORICO")] public double? CAR_PESO_TEORICO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VOLUME_TEORICO")] public double? CAR_VOLUME_TEORICO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PESO_REAL")] public double? CAR_PESO_REAL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VOLUME_REAL")] public double? CAR_VOLUME_REAL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PESO_EMBALAGEM")] public double? CAR_PESO_EMBALAGEM { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PESO_ENTRADA")] public double? CAR_PESO_ENTRADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PESO_SAIDA")] public double? CAR_PESO_SAIDA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID_DOCA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo CAR_ID_DOCA")] public string CAR_ID_DOCA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID_JUNTADA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo CAR_ID_JUNTADA")] public string CAR_ID_JUNTADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORD_DATA_ENTREGA_DE")] public DateTime ORD_DATA_ENTREGA_DE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORD_DATA_ENTREGA_ATE")] public DateTime ORD_DATA_ENTREGA_ATE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TOLERANCIA_MAIS")] public double? ORD_TOLERANCIA_MAIS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TOLERANCIA_MENOS")] public double? ORD_TOLERANCIA_MENOS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "REGIAO_ENTREGA")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo ORD_REGIAO_ENTREGA")] public string ORD_REGIAO_ENTREGA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PRO_DESCRICAO")] public string PRO_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORD_TIPO")] public int? ORD_TIPO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CAR_DATA_INICIO_REALIZADO")] public DateTime CAR_DATA_INICIO_REALIZADO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD")] [Required(ErrorMessage = "Campo ITC_QTD requirido.")] public double ITC_QTD { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo TIP_DESCRICAO")] public string TIP_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PLACA")] [Required(ErrorMessage = "Campo VEI_PLACA requirido.")] [MaxLength(8, ErrorMessage = "Maximode 8 caracteres, campo VEI_PLACA")] public string VEI_PLACA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NOME")] [Required(ErrorMessage = "Campo TRA_NOME requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo TRA_NOME")] public string TRA_NOME { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CAPACIDADE")] public double? CAPACIDADE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OCUPACAO")] public double? OCUPACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CAPACIDADE_M3")] public double? TIP_CAPACIDADE_M3 { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD_DESCARGAS")] public int? QTD_DESCARGAS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO_FRETE")] [MaxLength(3, ErrorMessage = "Maximode 3 caracteres, campo ORD_TIPO_FRETE")] public string ORD_TIPO_FRETE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORD_COR_FILA")] [MaxLength(8, ErrorMessage = "Maximode 8 caracteres, campo ORD_COR_FILA")] public string ORD_COR_FILA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FPR_DATA_INICIO_PREVISTA")] public DateTime? FPR_DATA_INICIO_PREVISTA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FFF")] public DateTime FFF { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "EMBARQUE_ALVO")] public DateTime CAR_EMBARQUE_ALVO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OTIF")] [MaxLength(8, ErrorMessage = "Maximode 8 caracteres, campo COR_OTIF")] public string COR_OTIF { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "GRP_TIPO")] public double? GRP_TIPO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PECAS_POR_FARDO")] public double? PRO_PECAS_POR_FARDO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CAMADAS_POR_PALETE")] public double? PRO_CAMADAS_POR_PALETE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FARDOS_POR_CAMADA")] public double? PRO_FARDOS_POR_CAMADA { get; set; }

        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs, ref int modo_insert) {  } 
    }

    [Display(Name = "TRANSPORTADORAS")]
    public class Transportadora
    {
        public Transportadora()
        {
            Cargas = new HashSet<Carga>();
        }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD TRANSPORTADORA")] [Required(ErrorMessage = "Campo TRA_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo TRA_ID")] public string TRA_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TRANSPORTADORA")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo TRA_NOME")] public string TRA_NOME { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "E-MAIL")] [MaxLength(1000, ErrorMessage = "Maximode * caracteres, campo TRA_EMAIL")] public string TRA_EMAIL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "RESPONSÁVEL")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo TRA_RESPONSAVEL")] public string TRA_RESPONSAVEL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FONE")] [MaxLength(15, ErrorMessage = "Maximode 15 caracteres, campo TRA_FONE")] public string TRA_FONE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD INTEGRACAO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo TRA_ID_INTEGRACAO")] public string TRA_ID_INTEGRACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD INTEGRACAO ERP")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo TRA_ID_INTEGRACAO_ERP")] public string TRA_ID_INTEGRACAO_ERP { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  } 
        public virtual ICollection<Carga> Cargas { get; set; }

    }

    public class Municipios_Coordenadas
    {
        public int MUN_CODIGO_IBGE { get; set; }
        public string MUN_NOME { get; set; }
        public double MUN_LATITUDE { get; set; }
        public double MUN_LONGITUDE { get; set; }
        public string EST_UF { get; set; }
        public Municipios_Coordenadas()
        {

        }
        public Municipios_Coordenadas(int cod_ibge, string nome, double longitude, double latitude, string uf)
        {
            MUN_CODIGO_IBGE = cod_ibge;
            MUN_NOME = nome;
            MUN_LATITUDE = latitude;
            MUN_LONGITUDE = longitude;
            EST_UF = uf;
        }

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
    public class Rodovias
    {
        public Rodovias()
        {
            Mapas = new HashSet<Mapa>();
        }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD RODOVIA")] [Required(ErrorMessage = "Campo ROD_ID requirido.")] public int ROD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICÃO")] [MaxLength(100, ErrorMessage = "Maximo 100 caracteres, campo ROD_DESCRICAO")] public string ROD_DESCRICAO { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  } 
        public virtual ICollection<Mapa> Mapas { get; set; }
    }

    [Display(Name = "TEMPOS LOGÍSTICOS")]
    public class TemposLogisticos
    {
        [Combobox(Description = " ", Value = "")]
        [Combobox(Description = "Carregamento", Value = "Carregamento")]
        [Combobox(Description = "Descarregamento", Value = "Descarregamento")]
        [Combobox(Description = "Tempo Espera Cliente", Value = "Tempo_Espera_Cliente")]
        [Combobox(Description = "Faturamento", Value = "Faturamento")]
        [Combobox(Description = "km/hora", Value = "km/hora")]
        [Combobox(Description = "Percentual Janela Embarque", Value = "PERCENTUAL_JANELA_EMBARQUE")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO_TEMPO")] [Required(ErrorMessage = "Campo TMP_TIPO_TEMPO requirido.")] [MaxLength(50, ErrorMessage = "Maximode 50 caracteres, campo TMP_TIPO_TEMPO")] public string TMP_TIPO_TEMPO { get; set; }

        [Combobox(Description = " ", Value = "")]
        [Combobox(Description = "DEITADO", Value = "DEITADO")]
        [Combobox(Description = "FARDO", Value = "FARDO")]
        [Combobox(Description = "GRANEL", Value = "GRANEL")]
        [Combobox(Description = "PALETIZADO", Value = "PALETIZADO")]
        [Combobox(Description = "ROLADO", Value = "ROLADO")]
        [Combobox(Description = "EM PÉ", Value = "EM_PE")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO_CARGA")] [MaxLength(50, ErrorMessage = "Maximode 50 caracteres, campo TMP_TIPO_CARGA")] public string TMP_TIPO_CARGA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TEMPO_MEDIO_UNITARIO")] [Required(ErrorMessage = "Campo TMP_TEMPO_MEDIO_UNITARIO requirido.")] public double TMP_TEMPO_MEDIO_UNITARIO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD CLIENTE")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo CLI_ID")] public string CLI_ID { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            foreach (object item in objects)
            {
                TemposLogisticos tl = (TemposLogisticos)item;
                /*if (tl.PlayAction.ToLower() == "insert" && String.IsNullOrEmpty(tl.CLI_ID))
                {
                    tl.PlayMsgErroValidacao = "O código do cliente deve ser informado";
                    return false;
                }*/
                if (tl.PlayAction.ToLower() == "delete" && String.IsNullOrEmpty(tl.CLI_ID))
                {
                    tl.PlayMsgErroValidacao = "Não é possível exluir um registro que não possui o código do cliente";
                    return false;
                }
            }
            return true;
        }


    }

    [Display(Name = "RESTRIÇÕES DE RODAGEM")]
    public class RestricoesDeRodagem
    {
        [READ] [TAB(Value = "PRINCIPAL")] [Display(Name = "COD RESTRIÇÃO RODOVIA")] [Required(ErrorMessage = "Campo RES_ID requirido.")] public int RES_ID { get; set; }
        [Combobox(Description = "RESTRINGE O TIPO DE VEICULO", Value = "V")]
        [Combobox(Description = "RESTRINGE O HORARIO", Value = "H")]
        [Combobox(Description = "RESTRINGE VEICULO VESUS HORARIO", Value = "X")]
        [Combobox(Description = "HORA DO RACHE", Value = "R")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO RESTRIÇÃO")] [Required(ErrorMessage = "Campo RES_TIPO requirido.")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo RES_TIPO")] public string RES_TIPO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "INÍCIO RESTRIÇÃO")] [MaxLength(5, ErrorMessage = "Maximode 5 caracteres, campo RES_HORA_INI")] public string RES_HORA_INI { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FIM RESTRIÇÃO")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo RES_HORA_FIM")] public string RES_HORA_FIM { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "RESTRIÇÃO VELOCIDADE")] public double? RES_VELOCIDADE_HORA_RUSH { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD RODOVIA")] public int? TVE_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD MAPA")] public int? MAP_ID { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  }

        public virtual Mapa Mapa { get; set; }
    }

    [Display(Name = "CALENDÁRIO DISPONIBILIDADE DE VEÍCULOS")]
    public class CalendarioDisponibilidadeVeiculos
    {
        public CalendarioDisponibilidadeVeiculos()
        {
            ItenCalendarioDisponibilidadeVeiculos = new HashSet<ItenCalendarioDisponibilidadeVeiculos>();
        }
        [READ] [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo CDV_ID requirido.")] public int CDV_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_DE")] public DateTime CDV_DATA_DE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_ATE")] public DateTime CDV_DATA_ATE { get; set; }

        [Checkbox(Description = "Disponível", TargetValue = "1")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEGUNDA")] public int? CDV_SEGUNDA { get; set; }

        [Checkbox(Description = "Disponível", TargetValue = "1")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TERCA")] public int? CDV_TERCA { get; set; }

        [Checkbox(Description = "Disponível", TargetValue = "1")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUARTA")] public int? CDV_QUARTA { get; set; }

        [Checkbox(Description = "Disponível", TargetValue = "1")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUINTA")] public int? CDV_QUINTA { get; set; }

        [Checkbox(Description = "Disponível", TargetValue = "1")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEXTA")] public int? CDV_SEXTA { get; set; }

        [Checkbox(Description = "Disponível", TargetValue = "1")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SABADO")] public int? CDV_SABADO { get; set; }

        [Checkbox(Description = "Disponível", TargetValue = "1")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DOMINGO")] public int? CDV_DOMINGO { get; set; }

        public virtual ICollection<ItenCalendarioDisponibilidadeVeiculos> ItenCalendarioDisponibilidadeVeiculos { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  }
        private InterfaceTelaCalendarioDisponibilidadeVeiculos CalendarioDisponibilidadeVeiculosToInterfaceTelaCalendarioDisponibilidadeVeiculos()
        {
            return new InterfaceTelaCalendarioDisponibilidadeVeiculos()
            {
                CDV_DATA_DE = this.CDV_DATA_DE,
                CDV_DATA_ATE = this.CDV_DATA_ATE,
            };
        }
    }

    [Display(Name = "ITENS CALENDÁRIO DISPONIBILIDADE DE VEÍCULOS")]
    public class ItenCalendarioDisponibilidadeVeiculos
    {
        public ItenCalendarioDisponibilidadeVeiculos()
        {

        }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CDV_ID")] public int? CDV_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO_VEICULO")] public int? TIP_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE")] public int? IDV_QTD { get; set; }

        public TipoVeiculo TipoVeiculo { get; set; }
        public CalendarioDisponibilidadeVeiculos CalendarioDisponibilidadeVeiculos { get; set; }

        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  } 
    }

    [Display(Name = "DISPONIBILIDADE DE VEÍCULOS")]
    public class InterfaceTelaCalendarioDisponibilidadeVeiculos : InterfaceDeTelas
    {
        public InterfaceTelaCalendarioDisponibilidadeVeiculos()
        {
            IntensCalendario = new HashSet<CalendarioDisponibilidadeVeiculos>();
            NamespaceOfClassMapped = typeof(CalendarioDisponibilidadeVeiculos).FullName;
        }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_DE")] public DateTime CDV_DATA_DE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_ATE")] public DateTime CDV_DATA_ATE { get; set; }
        [NotMapped] public string NamespaceOfClassMapped { get; set; }
        public virtual ICollection<CalendarioDisponibilidadeVeiculos> IntensCalendario { get; set; }
        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            throw new NotImplementedException();
        }
        CalendarioDisponibilidadeVeiculos InterfaceTelaCalendarioDisponibilidadeVeiculosToCalendarioDisponibilidadeVeiculos()
        {
            return new CalendarioDisponibilidadeVeiculos()
            {
                CDV_DATA_DE = this.CDV_DATA_DE,
                CDV_DATA_ATE = this.CDV_DATA_ATE
            };
        }
    }
    public class PontosEntrega
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PON_ID")] [Required(ErrorMessage = "Campo PON_ID requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PON_ID")] public string PON_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LATITUDE")] public decimal PON_LATITUDE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LONGITUDE")] public decimal PON_LONGITUDE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO")] [Required(ErrorMessage = "Campo PON_DESCRICAO requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PON_DESCRICAO")] public string PON_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CAR_ID")] [Required(ErrorMessage = "Campo CAR_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo CAR_ID")] public string CAR_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "EMBARQUE_ALVO")] public DateTime CAR_EMBARQUE_ALVO { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        [NotMapped] public string Modo { get; set; }
    }

    public class V_DISPONIBILIDADE_VEICULO
    {

        [TAB(Value = "PRINCIPAL")] [Display(Name = "O")] public int? SALDO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CAPACIDADE_LARGURA_PESCOCO_D")] public double? TIP_CAPACIDADE_LARGURA_PESCOCO_D { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CAPACIDADE_COMPRIMENTO_PESCOCO_D")] public double? TIP_CAPACIDADE_COMPRIMENTO_PESCOCO_D { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CAPACIDADE_ALTURA_PESCOCO_D")] public double? TIP_CAPACIDADE_ALTURA_PESCOCO_D { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CAPACIDADE_LARGURA_PESCOCO_E")] public double? TIP_CAPACIDADE_LARGURA_PESCOCO_E { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CAPACIDADE_COMPRIMENTO_PESCOCO_E")] public double? TIP_CAPACIDADE_COMPRIMENTO_PESCOCO_E { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CAPACIDADE_ALTURA_PESCOCO_E")] public double? TIP_CAPACIDADE_ALTURA_PESCOCO_E { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo TIP_ID requirido.")] public int TIP_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO")] [MaxLength(109, ErrorMessage = "Maximode 109 caracteres, campo TIP_DESCRICAO")] public string TIP_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD_DISPONIVEL")] public int? TIP_QTD_DISPONIVEL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VALOR_KM")] public double? TIP_VALOR_KM { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VALOR_DIARIA")] public double? TIP_VALOR_DIARIA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VALOR_AJUDANTE")] public double? TIP_VALOR_AJUDANTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD_EIXOS")] public double? TIP_QTD_EIXOS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VELOCIDADE_MEDIA")] public double? TIP_VELOCIDADE_MEDIA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CAPACIDADE_ALTURA")] public double? TIP_CAPACIDADE_ALTURA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CAPACIDADE_COMPRIMENTO")] public double? TIP_CAPACIDADE_COMPRIMENTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CAPACIDADE_LARGURA")] public double? TIP_CAPACIDADE_LARGURA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CAPACIDADE_M3")] public double? TIP_CAPACIDADE_M3 { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_DE")] [Required(ErrorMessage = "Campo CDV_DATA_DE requirido.")] public DateTime CDV_DATA_DE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_ATE")] [Required(ErrorMessage = "Campo CDV_DATA_ATE requirido.")] public DateTime CDV_DATA_ATE { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs, ref int modo_insert) {  } 
        [NotMapped]
        public int listIndex { get; set; }
    }



}

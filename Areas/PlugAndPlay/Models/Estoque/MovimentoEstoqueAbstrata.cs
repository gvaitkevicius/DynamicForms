using DynamicForms.Context;
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
    public abstract class MovimentoEstoqueAbstrata
    {
        public MovimentoEstoqueAbstrata()
        {

            this.MOV_DATA_HORA_EMISSAO = DateTime.Now;//ParametrosSingleton.Instance.DataBase;
            this.MOV_DATA_HORA_CRIACAO = DateTime.Now;
            this.MOV_DIA_TURMA = ParametrosSingleton.DiaTurmaS();
        }

        [TAB(Value = "OUTROS")] [READ] [Display(Name = "COD MOVIMENTO")] public int MOV_ID { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "TIPO MOVIMENTAÇÃO")] [MaxLength(3, ErrorMessage = "Maximode 3 caracteres, campo TIP_ID")] public string TIP_ID { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "DATA MOVIMENTO")] public DateTime MOV_DATA_HORA_EMISSAO { get; set; }

        [TAB(Value = "OUTROS")] [Display(Name = "DATA CRIAÇÃO")] public DateTime MOV_DATA_HORA_CRIACAO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "COD MÁQUINA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MAQ_ID")] public string MAQ_ID { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "TURNO")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo TURN_ID")] public string TURN_ID { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "TURMA")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo TURM_ID")] public string TURM_ID { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "DIA TURMA")] [MaxLength(8, ErrorMessage = "Maximode 8 caracteres, campo MOV_DIA_TURMA")] public string MOV_DIA_TURMA { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "ESTORNO")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo MOV_ESTORNO")] public string MOV_ESTORNO { get; set; }

        [TAB(Value = "OUTROS")] [Display(Name = "COD INTEGRACAO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MOV_ID_INTEGRACAO")] public string MOV_ID_INTEGRACAO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "COD INTEGRACAO ERP")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MOV_ID_INTEGRACAO_ERP")] public string MOV_ID_INTEGRACAO_ERP { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "TIPO REGISTRO")] public int MOV_TYPE { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "COD CARGA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo CAR_ID")] public string CAR_ID { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "COD MOV DESTINO")] public int? MOV_ID_DESTINO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "COD PRODUTO DESTINO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID_DESTINO")] public string PRO_ID_DESTINO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "LOTE MOV DESTINO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MOV_LOTE_DESTINO")] public string MOV_LOTE_DESTINO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "SUB LOTE MOV DESTINO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_SUB_LOTE_DESTINO")] public string MOV_SUB_LOTE_DESTINO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "COD MOV ORIGEM")] public int? MOV_ID_ORIGEM { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "COD PRODUTO ORIGEM")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID_ORIGEM")] public string PRO_ID_ORIGEM { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "LOTE MOV ORIGEM")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MOV_LOTE_ORIGEM")] public string MOV_LOTE_ORIGEM { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "SUB LOTE MOV ORIGEM")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_SUB_LOTE_ORIGEM")] public string MOV_SUB_LOTE_ORIGEM { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "NUM DOCUMENTO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MOV_DOC")] public string MOV_DOC { get; set; }

        [NotMapped]
        public string PlayAction { get; set; }
        [NotMapped]
        public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        /// <summary>
        /// Métodos de classe
        /// </summary>
        public abstract bool ImprimirEtiqueta(List<object> objects, ref List<LogPlay> Logs);

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                List<MovimentoEstoqueVendas> Db_movVendas = new List<MovimentoEstoqueVendas>();
                List<MovimentoEstoqueProducao> Db_movProducao = new List<MovimentoEstoqueProducao>();
                List<MovimentoEstoqueReservaDeEstoque> Db_movReserva = new List<MovimentoEstoqueReservaDeEstoque>();
                foreach (var item in objects)
                {
                    //Validação de Estorno de Movimento
                    if (item.GetType().Name.Equals("MovimentoEstoqueConsumoMateriaPrima"))
                    {
                        MovimentoEstoqueConsumoMateriaPrima movimentoEstoque = (MovimentoEstoqueConsumoMateriaPrima)item;
                        if (movimentoEstoque.MOV_ESTORNO != null && movimentoEstoque.MOV_ESTORNO.Equals("E")) { }
                    }
                    else if (item.GetType().Name == "MovimentoEstoqueSaidaInventario")
                    {
                        MovimentoEstoqueSaidaInventario movimentoEstoque = (MovimentoEstoqueSaidaInventario)item;
                        if (movimentoEstoque.MOV_ESTORNO != null && movimentoEstoque.MOV_ESTORNO.Equals("E")) { }
                    }
                    else if (item.GetType().Name.Equals("MovimentoEstoquePerdas"))
                    {
                        MovimentoEstoquePerdas movimentoEstoque = (MovimentoEstoquePerdas)item;
                        if (movimentoEstoque.MOV_ESTORNO != null && movimentoEstoque.MOV_ESTORNO.Equals("E")) { }
                    }
                    else if (item.GetType().Name.Equals("MovimentoEstoqueProducao"))
                    {
                        MovimentoEstoqueProducao movimentoEstoque = (MovimentoEstoqueProducao)item;
                        if (movimentoEstoque.MOV_ESTORNO != null && movimentoEstoque.MOV_ESTORNO.Equals("E"))
                        {
                            if (Db_movProducao.Count == 0)
                                Db_movProducao = db.MovimentoEstoqueProducao.AsNoTracking().Where(m => m.ORD_ID == movimentoEstoque.ORD_ID).ToList();
                            var idUltimoMovimento = Db_movProducao.Where(m => !m.MOV_ESTORNO.Equals("E")).Select(x => x.MOV_ID).Max();
                            if (idUltimoMovimento != movimentoEstoque.MOV_ID)
                            {
                                movimentoEstoque.PlayMsgErroValidacao = $"Você deve estornar primeiro as movimentaçöes anteriores as de ID[{idUltimoMovimento}]";
                                return false;
                            }
                            Db_movProducao.Remove(Db_movProducao.Where(x => x.MOV_ID == movimentoEstoque.MOV_ID).FirstOrDefault());
                        }
                    }
                    else if (item.GetType().Name.Equals("MovimentoEstoqueReservaDeEstoque"))
                    {
                        MovimentoEstoqueReservaDeEstoque movimentoEstoque = (MovimentoEstoqueReservaDeEstoque)item;
                        //Consultando os movimentos a estornar
                        if (movimentoEstoque.MOV_ESTORNO != null && movimentoEstoque.MOV_ESTORNO.Equals("E"))
                        {
                            if (Db_movReserva.Count == 0)
                                Db_movReserva = db.MovimentoEstoqueReservaDeEstoque.AsNoTracking().Where(m => m.CAR_ID == movimentoEstoque.CAR_ID).ToList();
                            var idUltimoMovimento = Db_movReserva.Where(m => !m.MOV_ESTORNO.Equals("E")).Select(x => x.MOV_ID).Max();
                            if (idUltimoMovimento != movimentoEstoque.MOV_ID)
                            {
                                movimentoEstoque.PlayMsgErroValidacao = "Você deve estornar primeiro as movimentaçöes anteriores as de ID[" + idUltimoMovimento + "]";
                                return false;
                            }
                            Db_movReserva.Remove(Db_movReserva.Where(x => x.MOV_ID == movimentoEstoque.MOV_ID).FirstOrDefault());
                        }
                    }
                    else if (item.GetType().Name == "MovimentoEstoqueTransferenciaSimples")
                    {
                        MovimentoEstoqueTransferenciaSimples movimentoEstoque = (MovimentoEstoqueTransferenciaSimples)item;
                        if (movimentoEstoque.MOV_ESTORNO != null && movimentoEstoque.MOV_ESTORNO.Equals("E")) { }
                    }
                    else if (item.GetType().Name == "MovimentoEstoqueVendas")
                    {
                        MovimentoEstoqueVendas movimentoEstoque = (MovimentoEstoqueVendas)item;
                        if (!String.IsNullOrEmpty(movimentoEstoque.MOV_ESTORNO) && movimentoEstoque.MOV_ESTORNO.Equals("E"))
                        {
                            if (Db_movVendas.Count == 0)
                                Db_movVendas = db.MovimentoEstoqueVendas.AsNoTracking().Where(m => m.CAR_ID == movimentoEstoque.CAR_ID && m.MOV_TYPE == 7 && !m.MOV_ESTORNO.Equals("E")).ToList();
                            var idUltimoMovimento = Db_movVendas.Max(m => m.MOV_ID);
                            if (idUltimoMovimento != movimentoEstoque.MOV_ID)
                            {
                                movimentoEstoque.PlayMsgErroValidacao = "Você deve estornar primeiro as movimentaçöes anteriores as de ID[" + idUltimoMovimento + "]";
                                return false;
                            }
                            Db_movVendas.Remove(Db_movVendas.Where(x => x.MOV_ID == movimentoEstoque.MOV_ID).FirstOrDefault());
                        }
                    }
                }
            }
            return true;
        }
    }
}


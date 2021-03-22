
using DynamicForms.Context;
using DynamicForms.Models;
using DynamicForms.Util;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "OCORRÊNCIAS")]
    public class Ocorrencia
    {
        public Ocorrencia()
        {
            MovimentoEstoque = new HashSet<MovimentoEstoque>();
            Medicoes = new HashSet<Feedback>();
            TarProdOcoPers = new HashSet<TargetProduto>();
            TarProdOcoSetups = new HashSet<TargetProduto>();
            TarProdOcoSetupAs = new HashSet<TargetProduto>();
        }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo OCO_ID requirido.")] [MaxLength(30, ErrorMessage = "")] public string OCO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICÃO")] [Required(ErrorMessage = "Campo OCO_DESCRICAO requirido.")] [MaxLength(100, ErrorMessage = "")] public string OCO_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD TIPO OCORRENCIA")] [Required(ErrorMessage = "Campo TIP_ID requirido.")] public int TIP_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SUB TIPO")] public string OCO_SUB_TIPO { get; set; }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD GRUPO MAQUINA")] public string GMA_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD MAQUINA")] [MaxLength(30, ErrorMessage = "")] public string MAQ_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [READ] [Display(Name = "RESERVADO PLAYSIS")] public int? SPR { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        public TipoOcorrencia TipoOcorrencia { get; set; }
        public GrupoMaquina GrupoMaquina { get; set; }
        public Maquina Maquina { get; set; }
        public virtual ICollection<MovimentoEstoque> MovimentoEstoque { get; set; }
        public virtual ICollection<Feedback> Medicoes { get; set; }
        public virtual ICollection<TargetProduto> TarProdOcoPers { get; set; }
        public virtual ICollection<TargetProduto> TarProdOcoSetups { get; set; }
        public virtual ICollection<TargetProduto> TarProdOcoSetupAs { get; set; }
        ///MÉTODOS DE CLASSE
        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            List<object> OcorrenciasGeradas = new List<object>();
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (var item in objects)
                {
                    //Objeto retornado da View
                    Ocorrencia _Ocorrencia = (Ocorrencia)item;
                    //Setando action do objeto da view como OK para que seja ignorado pela  funçao UpdateData
                    string auxAction = _Ocorrencia.PlayAction;
                    _Ocorrencia.PlayAction = "OK";
                    if (_Ocorrencia.TIP_ID == 0)
                    {
                        _Ocorrencia.PlayMsgErroValidacao = "Você deve informar o código do tipo de ocorrencia.";
                        return false;
                    }
                    var Db_TipoOcorrencia = db.TipoOcorrencia.Where(to => to.Id == _Ocorrencia.TIP_ID).FirstOrDefault();
                    if (Db_TipoOcorrencia == null)
                    {
                        _Ocorrencia.PlayMsgErroValidacao = "O código da ocorrencia informado não foi encontrado.";
                        return false;
                    }
                    switch (Db_TipoOcorrencia.Id)
                    {
                        case 1://PARADAS NÃO PROGRAMADAS
                            OcorrenciaMotivosDeParadas ocorrenciaParadaNP = new OcorrenciaMotivosDeParadas()
                            {
                                OCO_ID = _Ocorrencia.OCO_ID,
                                OCO_DESCRICAO = _Ocorrencia.OCO_DESCRICAO,
                                GMA_ID = _Ocorrencia.GMA_ID,
                                MAQ_ID = _Ocorrencia.MAQ_ID,
                                TIP_ID = Db_TipoOcorrencia.Id,
                                OCO_SUB_TIPO = _Ocorrencia.OCO_SUB_TIPO,
                                PlayAction = auxAction
                            };
                            OcorrenciasGeradas.Add(ocorrenciaParadaNP);
                            break;
                        case 2://PARADA PROGRAMADA
                            OcorrenciaMotivosDeParadas ocorrenciaParadaP = new OcorrenciaMotivosDeParadas()
                            {
                                OCO_ID = _Ocorrencia.OCO_ID,
                                OCO_DESCRICAO = _Ocorrencia.OCO_DESCRICAO,
                                GMA_ID = _Ocorrencia.GMA_ID,
                                MAQ_ID = _Ocorrencia.MAQ_ID,
                                TIP_ID = Db_TipoOcorrencia.Id,
                                OCO_SUB_TIPO = _Ocorrencia.OCO_SUB_TIPO,
                                PlayAction = auxAction
                            };
                            OcorrenciasGeradas.Add(ocorrenciaParadaP);
                            break;
                        case 3://BAIXA PERFORMACE
                            OcorrenciaProducao ocorrenciaProducaoBP = new OcorrenciaProducao()
                            {
                                OCO_ID = _Ocorrencia.OCO_ID,
                                OCO_DESCRICAO = _Ocorrencia.OCO_DESCRICAO,
                                GMA_ID = _Ocorrencia.GMA_ID,
                                MAQ_ID = _Ocorrencia.MAQ_ID,
                                TIP_ID = Db_TipoOcorrencia.Id,
                                OCO_SUB_TIPO = _Ocorrencia.OCO_SUB_TIPO,
                                PlayAction = auxAction
                            };
                            OcorrenciasGeradas.Add(ocorrenciaProducaoBP);
                            break;
                        case 4://ALTA PERFORMACE
                            OcorrenciaProducao ocorrenciaProducaoAP = new OcorrenciaProducao()
                            {
                                OCO_ID = _Ocorrencia.OCO_ID,
                                OCO_DESCRICAO = _Ocorrencia.OCO_DESCRICAO,
                                GMA_ID = _Ocorrencia.GMA_ID,
                                MAQ_ID = _Ocorrencia.MAQ_ID,
                                TIP_ID = Db_TipoOcorrencia.Id,
                                OCO_SUB_TIPO = _Ocorrencia.OCO_SUB_TIPO,
                                PlayAction = auxAction
                            };
                            OcorrenciasGeradas.Add(ocorrenciaProducaoAP);
                            break;
                        case 5://PRODUCAO
                            OcorrenciaProducao OcorrenciaProducao = new OcorrenciaProducao()
                            {
                                OCO_ID = _Ocorrencia.OCO_ID,
                                OCO_DESCRICAO = _Ocorrencia.OCO_DESCRICAO,
                                GMA_ID = _Ocorrencia.GMA_ID,
                                MAQ_ID = _Ocorrencia.MAQ_ID,
                                TIP_ID = Db_TipoOcorrencia.Id,
                                OCO_SUB_TIPO = _Ocorrencia.OCO_SUB_TIPO,
                                PlayAction = auxAction
                            };
                            OcorrenciasGeradas.Add(OcorrenciaProducao);
                            break;
                        case 6://OP PARCIAL
                            OcorrenciaProducaoParciais OcorrenciaProducaoParciais = new OcorrenciaProducaoParciais()
                            {
                                OCO_ID = _Ocorrencia.OCO_ID,
                                OCO_DESCRICAO = _Ocorrencia.OCO_DESCRICAO,
                                GMA_ID = _Ocorrencia.GMA_ID,
                                MAQ_ID = _Ocorrencia.MAQ_ID,
                                TIP_ID = Db_TipoOcorrencia.Id,
                                OCO_SUB_TIPO = _Ocorrencia.OCO_SUB_TIPO,
                                PlayAction = auxAction
                            };
                            OcorrenciasGeradas.Add(OcorrenciaProducaoParciais);
                            break;
                        case 7://OCORRENCIAS DE EXPEDIÇÃO
                            OcorrenciaTransporte OcorrenciaTransporte = new OcorrenciaTransporte()
                            {
                                OCO_ID = _Ocorrencia.OCO_ID,
                                OCO_DESCRICAO = _Ocorrencia.OCO_DESCRICAO,
                                GMA_ID = _Ocorrencia.GMA_ID,
                                MAQ_ID = _Ocorrencia.MAQ_ID,
                                TIP_ID = Db_TipoOcorrencia.Id,
                                OCO_SUB_TIPO = _Ocorrencia.OCO_SUB_TIPO,
                                PlayAction = auxAction
                            };
                            OcorrenciasGeradas.Add(OcorrenciaTransporte);
                            break;
                        case 101://RETENÇÃO DE PALETES
                            OcorrenciaRetencaoLotes OcorrenciaReserva = new OcorrenciaRetencaoLotes()
                            {
                                OCO_ID = _Ocorrencia.OCO_ID,
                                OCO_DESCRICAO = _Ocorrencia.OCO_DESCRICAO,
                                GMA_ID = _Ocorrencia.GMA_ID,
                                MAQ_ID = _Ocorrencia.MAQ_ID,
                                TIP_ID = Db_TipoOcorrencia.Id,
                                OCO_SUB_TIPO = _Ocorrencia.OCO_SUB_TIPO,
                                PlayAction = auxAction
                            };
                            OcorrenciasGeradas.Add(OcorrenciaReserva);
                            break;
                    }
                }
            }
            objects.AddRange(OcorrenciasGeradas);

            return true;
        }
    }

}


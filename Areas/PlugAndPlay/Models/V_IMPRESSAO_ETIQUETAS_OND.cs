using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Models;
using DynamicForms.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "IMPRESSÃO DE ETIQUETAS DA ONDULADEIRA")]
    public class V_IMPRESSAO_ETIQUETAS_OND
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "BOL_ID")] [Required(ErrorMessage = "Campo BOL_ID requirido.")] public int BOL_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORD_ID")] [Required(ErrorMessage = "Campo ORD_ID requirido.")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE_PREVISTA")] [Required(ErrorMessage = "Campo FPR_QUANTIDADE_PREVISTA requirido.")] public double FPR_QUANTIDADE_PREVISTA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PC_PRO_ID")] [Required(ErrorMessage = "Campo PC_PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PC_PRO_ID")] public string PC_PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_INICIO_PREVISTA")] [Required(ErrorMessage = "Campo FPR_DATA_INICIO_PREVISTA requirido.")] public DateTime FPR_DATA_INICIO_PREVISTA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_FIM_PREVISTA")] [Required(ErrorMessage = "Campo FPR_DATA_FIM_PREVISTA requirido.")] public DateTime FPR_DATA_FIM_PREVISTA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PREVISAO_MATERIA_PRIMA")] [Required(ErrorMessage = "Campo FPR_PREVISAO_MATERIA_PRIMA requirido.")] public DateTime FPR_PREVISAO_MATERIA_PRIMA { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        public virtual Order Order { get; set; }
        public virtual Produto Produto { get; set;}

        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs, ref int modo_insert) {  } 

        public bool ImprimirEtiqueta(List<object> objects, ref List<LogPlay> Logs)
        {
            List<object> ObjetosProcessados = new List<object>();
            List<List<object>> ListObjectsToUpdate = new List<List<object>>();
            MasterController mc = new MasterController();
            //Criando um objeto para a nova carga
            string arrayDeValoresDefault = null;
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                //Para cada item da lista
                foreach (var item in objects)
                {
                    V_IMPRESSAO_ETIQUETAS_OND etiquetaOnd = (V_IMPRESSAO_ETIQUETAS_OND)item;
                    
                    var op = db.FilaProducao.AsNoTracking()
                        .Include(f => f.Order)
                            .ThenInclude(o => o.Produto)
                        .Where(f => f.ORD_ID.Equals(etiquetaOnd.ORD_ID) && f.ROT_PRO_ID.Equals(etiquetaOnd.PC_PRO_ID))
                        .FirstOrDefault();

                    //Validando e extraindo dados para Etiqueta
                    //pedido,produto,seq_rep,maquina,qtd_palete,de ate, num_copias
                    
                    var pedido = op.Order;
                    var chapa = pedido.Produto;

                    double alturaDaPilhaDeChapas = 0;

                    if (op.ROT_PRO_ID == pedido.PRO_ID)
                    {// É uma chapa PA
                        alturaDaPilhaDeChapas = db.Param.AsNoTracking().Where(p => p.PAR_ID == "OND_ALTURA_PILHA_CHAPA_PA")
                            .Select(p => p.PAR_VALOR_N).FirstOrDefault();
                    }
                    else
                    {// É uma chapa PI
                        alturaDaPilhaDeChapas = db.Param.AsNoTracking().Where(p => p.PAR_ID == "OND_ALTURA_PILHA_CHAPA_PI")
                            .Select(p => p.PAR_VALOR_N).FirstOrDefault();

                    }
                    double maxLargEstrado = db.Param.AsNoTracking().Where(p => p.PAR_ID == "OND_MAX_LARG_ESTR_PILHA_CHAPAS")
                        .Select(p => p.PAR_VALOR_N).FirstOrDefault();

                    double qtdChapasPorPilha = alturaDaPilhaDeChapas / chapa.ALTURA_CHAPA;
                    
                    double qtdPilhasDeChapas = op.FPR_QUANTIDADE_PREVISTA / qtdChapasPorPilha;
                    
                    double qtdMaximaDePilhas = maxLargEstrado / chapa.PRO_LARGURA_PECA_CHAPA.Value;

                    double qtdEstrados = Math.Ceiling(qtdPilhasDeChapas / qtdMaximaDePilhas); // Arredonda para cima

                    //Formatando dados para Etiqueta
                    arrayDeValoresDefault = "ORD_ID:" + etiquetaOnd.ORD_ID + "," +
                       "ROT_PRO_ID:" + etiquetaOnd.PC_PRO_ID + "," +
                       "FPR_SEQ_REPETICAO:" + op.FPR_SEQ_REPETICAO + "," +
                       "ETI_QUANTIDADE_PALETE:" + 0 + "," +
                       "MAQ_ID:" + op.ROT_MAQ_ID + "," +
                       "ETI_IMPRIMIR_DE:" + 1 + "," +
                       "ETI_IMPRIMIR_ATE:" + qtdEstrados + "," +
                       "ETI_NUMERO_COPIAS:" + 2 + "";
                }
            }
            ListObjectsToUpdate.Add(ObjetosProcessados);
            //Concatenando Logs por se tratar de um objeto de interface
            Logs.Add(new LogPlay(this.ToString(), "PROTOCOLO", "_ETIQUETA", "", "" + arrayDeValoresDefault + ""));
            return true;
        }

    }
}

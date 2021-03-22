
using DynamicForms.Context;
using DynamicForms.Models;
using DynamicForms.Util;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "MÁQUINAS")]
    public class Maquina
    {

        public Maquina()
        {
            Roteiros = new HashSet<Roteiro>();
            V_ROTEIROS_POSSIVEIS_DO_PRODUTO = new HashSet<V_ROTEIROS_POSSIVEIS_DO_PRODUTO>();
            V_ROTEIROS_CHAPAS = new HashSet<V_ROTEIROS_CHAPAS>();
            Feedbacks = new HashSet<Feedback>();
            TargetsProduto = new HashSet<TargetProduto>();
            FilasProducao = new List<FilaProducao>();
            //FilaDoSchedule = new List<FilaDoSchedule>();
            MovimentoEstoqueReservaDeEstoque = new HashSet<MovimentoEstoqueReservaDeEstoque>();
            this.MAQ_COR_SEMAFORO = "";
            //MovimentoEstoqueAbstrata = new HashSet<MovimentoEstoqueAbstrata>();
            Ocorrencia = new HashSet<Ocorrencia>();
            CustoEntreOps = new HashSet<CustoEntreOps>();
            MaquinaImpressora = new HashSet<MaquinaImpressora>();
            MovimentoEstoqueEntradaInventario = new HashSet<MovimentoEstoqueEntradaInventario>();
            MovimentoEstoqueSaidaInventario = new HashSet<MovimentoEstoqueSaidaInventario>();
            MovimentoEstoque = new HashSet<MovimentoEstoque>();

            MovimentoEstoqueVendas = new HashSet<MovimentoEstoqueVendas>();
            MaquinasEquipes = new HashSet<T_MAQUINAS_EQUIPES>();
            Calendarios = new HashSet<ItensCalendario>();
        }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD MÁQUINA")] [Required(ErrorMessage = "Campo MAQ_ID requirido.")] [MaxLength(30, ErrorMessage = "")] public string MAQ_ID { get; set; }
        [SEARCH] [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRIÇÃO")] [Required(ErrorMessage = "Campo MAQ_DESCRICAO requirido.")] [MaxLength(100, ErrorMessage = "")] public string MAQ_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD CALENDÁRIO")] public int? CAL_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "IP CONTROL MÁQ")] [MaxLength(30, ErrorMessage = "")] public string MAQ_CONTROL_IP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD GRUPO MÁQUINA")] [Required(ErrorMessage = "Campo GMA_ID requirido.")] [MaxLength(30, ErrorMessage = "")] public string GMA_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "STATUS")] [READ] public string MAQ_STATUS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ULTIMA ATUALIZACAO")] public DateTime MAQ_ULTIMA_ATUALIZACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SIRENE SEMAFORO")] public int? MAQ_SIRENE_SEMAFORO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COR SEMAFORO")] [MaxLength(30, ErrorMessage = "")] public string MAQ_COR_SEMAFORO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD MÁQ PAI")] public string MAQ_ID_MAQ_PAI { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD EQUIPE")] public string EQU_ID { get; set; }
        [TAB(Value = "QUALIDADE")] [Display(Name = "TEMPLATE DE TESTES")] public int? TEM_ID { get; set; }

        //[
        //    Combobox(Value = "1", Description = "1-Produção em Série"), 
        //    Combobox(Value ="2", Description = "2-Contagem para Gestão da Identificação"),
        //    Combobox(Value ="3", Description = "3-APONTAMENTO MANUAL")
        //]
        [TAB(Value = "CLPs")]
        [Display(Name = "TIPO CONTADOR")]
        [
           Combobox(Value = "3", Description = "Manual"),
           Combobox(Value = "1", Description = "Sensor Unico"),
           Combobox(Value = "8", Description = "Contador Chapas"),
           Combobox(Value = "9", Description = "Portal Apontamento codigo Barras"),
        ]
        public int? MAQ_TIPO_CONTADOR { get; set; }



        [TAB(Value = "PRINCIPAL")]
        [Display(Name = "TIPO_PLANEJAMENTO")]
        [
           Combobox(Value = "ONDULADEIRA", Description = "ONDULADEIRA"),
           Combobox(Value = "FILA_IMPRESSORAS", Description = "FILA IMPRESSORAS"),
           Combobox(Value = "FILA_IMPRESSORAS_PLANEJAMENTO_INVERSO", Description = "FILA IMPRESSORAS PLANEJAMENTO INVERSO"),
           Combobox(Value = "SERVICO_COMPRAS", Description = "SERVICO DE COMPRAS"),
           Combobox(Value = "DOCAS_EXPEDICAO", Description = "AGENDAMENTO DE DOCAS DE EXPEDICAO"),
           Combobox(Value = "SERVICO_WMS", Description = "SERVICOS DE WMS"), // OUTROS SERVIÇOS DE ARMAZEM E APOIO 
           Combobox(Value = "PROCESSO_MANUAL", Description = "PROCESSO MANUAL"),
           Combobox(Value = "ESTUDO", Description = "ESTUDO")
        ]
        [MaxLength(60, ErrorMessage = "")] [Required(ErrorMessage = "Campo MAQ_TIPO_PLANEJAMENTO requirido.")] public string MAQ_TIPO_PLANEJAMENTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD OP EM ANDAMENTO")] public int? FPR_ID_OP_PRODUZINDO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CONGELA FILA")] public int? MAQ_CONGELA_FILA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TEMPO MIN PARADA")] public int? MAQ_TEMPO_MIN_PARADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANT CORES")] public int? MAQ_QTD_CORES { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD INTEGRACAO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MAQ_ID_INTEGRACAO")] public string MAQ_ID_INTEGRACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD INTEGRACAO ERP")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MAQ_ID_INTEGRACAO_ERP")] public string MAQ_ID_INTEGRACAO_ERP { get; set; }
        [Combobox(ValueInt = 1, Description = "SIM")]
        [Combobox(ValueInt = 0, Description = "NÃO")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "Avalia Custo")] public int MAQ_AVALIA_CUSTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "HIERARQUIA SEQ TRANSFORMACAO")] public double? MAQ_HIERARQUIA_SEQ_TRANSFORMACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PERCENTUAL_INICIO_PASSO_ANTERIOR")] public double? MAQ_PERCENTUAL_INICIO_PASSO_ANTERIOR { get; set; }
        [Mask(Value = "00:00|00:00")] [TAB(Value = "PRINCIPAL")] [Display(Name = "ACOMPANHA_LOTE_PILOTO")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo MAQ_ACOMPANHA_LOTE_PILOTO")] public string MAQ_ACOMPANHA_LOTE_PILOTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DEBOUNCING LOW")] public int? MAQ_DEBOUNCING_LOW { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DEBOUNCING HIGHT")] public int? MAQ_DEBOUNCING_HIGHT { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO SINAL")] public int? MAQ_TIPO_SINAL { get; set; }
        [Combobox(ValueInt = 0, Description = "PRETO [0]")]
        [Combobox(ValueInt = 1, Description = "BRANCO [1]")]
        [Combobox(ValueInt = 2, Description = "CINZA [2]")]
        [Combobox(ValueInt = 3, Description = "ROXO [3]")]
        [Combobox(ValueInt = 4, Description = "AZUL [4]")]
        [Combobox(ValueInt = 5, Description = "VERDE [5]")]
        [Combobox(ValueInt = 6, Description = "AMARELO [6]")]
        [Combobox(ValueInt = 7, Description = "LARANJA [7]")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID SENSOR")] public int? MAQ_ID_SENSOR { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LARGURA_UTIL")] public double? MAQ_LARGURA_UTIL { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (var item in objects)
                {
                    if (item.GetType().Name == "Maquina")
                    {
                        Maquina maquina = (Maquina)item;

                        if (maquina.PlayAction == "insert")
                        {
                            if (maquina.MAQ_ID.Length < 3)
                            {
                                maquina.PlayMsgErroValidacao = "O código da máquina precisa ter mais que 3 caracteres";
                                return false;
                            }
                        }

                        if (maquina.PlayAction == "insert" || maquina.PlayAction == "update")
                        {
                            if (!string.IsNullOrEmpty(maquina.MAQ_CONTROL_IP))
                            {
                                int count = db.Maquina.AsNoTracking()
                                    .Count(m => !m.MAQ_ID.Equals(maquina.MAQ_ID) && m.MAQ_CONTROL_IP.Equals(maquina.MAQ_CONTROL_IP) && m.MAQ_ID_SENSOR == maquina.MAQ_ID_SENSOR);
                                if (count > 0)
                                {
                                    maquina.PlayMsgErroValidacao = "ERRO: Já existe uma máquina cadastra com o mesmo IP e Sensor";
                                    return false;
                                }
                            }

                            if (!string.IsNullOrEmpty(maquina.MAQ_ACOMPANHA_LOTE_PILOTO))
                            {
                                //validar o formato do campo
                                //!!    00:00||00:00     !!\\

                                //separa a string em um array contendo as duas horas
                                string[] maq_acompanha_lote_piloto = maquina.MAQ_ACOMPANHA_LOTE_PILOTO.Split('|');

                                //da um foreach  para verificar se as hoars são validas
                                foreach (string s in maq_acompanha_lote_piloto)
                                {
                                    //if(!TimeSpan.TryParse(s, out TimeSpan result)) {
                                    //    maquina.PlayMsgErroValidacao = "O campo acompanha lote piloto deve ser uma dupla de horários HH:MM separados por '|'.";
                                    //    return false;
                                    //}

                                    try
                                    {
                                        Match m = Regex.Match(s, @"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", RegexOptions.None, TimeSpan.FromMilliseconds(350));
                                        if (!m.Success)
                                        {
                                            maquina.PlayMsgErroValidacao = "O campo acompanha lote piloto deve ser uma dupla de horários HH:MM separados por '|'.";
                                            return false;
                                        }
                                    }
                                    catch (RegexMatchTimeoutException)
                                    {
                                        maquina.PlayMsgErroValidacao = "O campo acompanha lote piloto deve ser uma dupla de horários HH:MM separados por '|'.";
                                        return false;
                                    }
                                }

                            }
                        }
                    }
                }
            }
            return true;
        }

        public virtual Calendario Calendario { get; set; }
        public virtual GrupoMaquina GrupoMaquina { get; set; }

        // variaveis para schedule não persiste em banco pelo menos por enquanto 
        [NotMapped]
        public int FilaQtdOPs { get; set; }
        [NotMapped]
        public double FilaQtdHorasAlocacaoMaquina { get; set; }
        [NotMapped]
        public double FilaQtdAtrazoAcumulado { get; set; }
        [NotMapped]
        public double FilaQtdOPsAtrazadas { get; set; }
        [NotMapped]
        public double FilaQtdAntecipacaoAcumulada { get; set; }
        [NotMapped]
        public double OPTempoAtrazado { get; set; }
        [NotMapped]
        public double OPTempoAdiantado { get; set; }
        [NotMapped]
        public double OPTempoTotalProducao { get; set; }
        [NotMapped]
        public double OPPrioridade { get; set; }
        [NotMapped]
        public double MValorAvaliacao { get; set; }
        [NotMapped]
        public double TempoOciosoEntreOps { get; set; }
        [NotMapped]
        public List<int> ListIndexEspacosAEsquerda { get; set; } // lista de indices da fila de OPs com espaço a esquerda
        [NotMapped]
        public List<FilaDoSchedule> UltimasOPsEncerradas { get; set; }
        [Display(Name = "ROTEIRO")] public virtual ICollection<Roteiro> Roteiros { get; set; }
        public virtual ICollection<V_ROTEIROS_POSSIVEIS_DO_PRODUTO> V_ROTEIROS_POSSIVEIS_DO_PRODUTO { get; set; }
        public virtual ICollection<V_ROTEIROS_CHAPAS> V_ROTEIROS_CHAPAS { get; set; }
        [Display(Name = "FEEDBACKS")] public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<TargetProduto> TargetsProduto { get; set; }
        public virtual ICollection<FilaProducao> FilasProducao { get; set; }
        //[NotMapped] public List<FilaDoSchedule> FilaDoSchedule { get; set; }
        public virtual ICollection<Ocorrencia> Ocorrencia { get; set; }
        public virtual ICollection<CustoEntreOps> CustoEntreOps { get; set; }
        [Display(Name = "IMPRESSORA DA MAQUINA")] public virtual ICollection<MaquinaImpressora> MaquinaImpressora { get; set; }
        public virtual ICollection<MovimentoEstoqueEntradaInventario> MovimentoEstoqueEntradaInventario { get; set; }
        public virtual ICollection<MovimentoEstoqueSaidaInventario> MovimentoEstoqueSaidaInventario { get; set; }
        public virtual ICollection<MovimentoEstoqueVendas> MovimentoEstoqueVendas { get; set; }
        // retiramos o virtual da filas para não carregar do banco filasProdução quando este objeto for acessado 
        // quando temos a notação virtual  sempre que o objeto for acessada o entyty fas uma consulta no banco 
        // para evitar isso deveriamos colocar um include na query de maquinas 
        public virtual ICollection<MovimentoEstoqueReservaDeEstoque> MovimentoEstoqueReservaDeEstoque { get; set; }
        //public virtual ICollection<MovimentoEstoqueAbstrata> MovimentoEstoqueAbstrata { get; set; }
        public virtual ICollection<MovimentoEstoque> MovimentoEstoque { get; set; }
        public virtual ICollection<T_MAQUINAS_EQUIPES> MaquinasEquipes { get; set; }
        public virtual ICollection<ItensCalendario> Calendarios { get; set; }
        public virtual TemplateDeTestes TemplateDeTestes { get; set; }
        [NotMapped]
        public string T_ROTEIROS { get; set; }

    }


}
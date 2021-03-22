using DynamicForms.Context;
using DynamicForms.Models;
using DynamicForms.Util;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public enum Acao
    {
        Maquina_Valida = 'V',
        Excecao = 'E'
    }
    [Display(Name = "ROTEIROS")]
    public class Roteiro
    {
        public Roteiro()
        {
            FilasProducao = new HashSet<FilaProducao>();
            Observacoes = new HashSet<Observacoes>();
            Operacoes = new HashSet<Operacoes>();
        }

        [Display(Name = "COD MÁQUINA")]
        [Required(ErrorMessage = "Campo MAQ_ID requirido.")]
        [TAB(Value = "PRINCIPAL")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MAQ_ID")] public string MAQ_ID { get; set; }
        [Display(Name = "COD PRODUTO")]
        [Required(ErrorMessage = "Campo PRO_ID requirido.")]
        [TAB(Value = "PRINCIPAL")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [Required(ErrorMessage = "Sequencia de Transformação é a ordem em que o produto é produzido nas diferentes maquinas. A primeira maquina é sequencia 1 a segunda 2 e assim sucessivamente.")]
        [Display(Name = "SEQ TRANSFORMAÇÃO")]
        [TAB(Value = "PRINCIPAL")] public int ROT_SEQ_TRANFORMACAO { get; set; }
        [Required(ErrorMessage = "Performance deve ser preenchida. A performance é utilizada para definir a primera meta de performance bem como para calcular carga maquina. Este campo sera atualizado a cada produção.")]
        [Display(Name = "PERFORMANCE PULSOS/SEG")]
        [Range(double.MinValue, double.PositiveInfinity, ErrorMessage = "A performance do roteiro não pode ser igual ou menor que 0.0 ")]
        [TAB(Value = "PRINCIPAL")] public double? ROT_PERFORMANCE { get; set; }
        [Combobox(Description = "ATIVA", Value = "A")]
        [Combobox(Description = "DESATIVADA", Value = "D")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "STATUS")] [MaxLength(2, ErrorMessage = "Maximode 2 caracteres, campo ROT_STATUS")] public string ROT_STATUS { get; set; }
        [Display(Name = "GRUPO MÁQUINAS")]
        [TAB(Value = "PRINCIPAL")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo GMA_ID")] public string GMA_ID { get; set; }
        //[Range(1, 50000, ErrorMessage = "Peças por Pulso representa, a quantidade de produtos produzidos a cada contagem do censor. Seu valor deve ser maior que zero.")]
        //[Required(ErrorMessage = "Peças por Pulso representa, a quantidade de produtos produzidos a cada contagem do censor. Seu valor deve ser maior que zero.")]
        [Display(Name = "QUANT PEÇAS/PULSO")]
        [TAB(Value = "PRINCIPAL")] public double? ROT_PECAS_POR_PULSO { get; set; }
        [Range(0, 5, ErrorMessage = "A prioridade deve ser entre 0 ate 5. Este campo interfere diretamente na montagem de fila de produção, evite usalo.")]
        [Combobox(Description = "NÃO PRIORIZAR", ValueInt = 0)]
        [Combobox(Description = "ULTRAPASSA CONGELADAS", ValueInt = 1)]
        [Combobox(Description = "LOGO APÓS CONGELADAS", ValueInt = 2)]
        [Display(Name = "NÍVEL PRIORIDADE")]
        [TAB(Value = "PRINCIPAL")] public double? ROT_PRIORIDADE_INFORMADA { get; set; }
        [Display(Name = "MÁQUINA EXCEÇÃO")]
        [Combobox(Value = "", Description = "NÃO")]
        [Combobox(Value = "E", Description = "SIM")]
        [TAB(Value = "PRINCIPAL")] [MaxLength(2, ErrorMessage = "Maximode 2 caracteres, campo ROT_ACAO")] public string ROT_ACAO { get; set; }
        [Required(ErrorMessage = "Setup deve ser preenchido. O setup é utilizado para definir a primera meta de setup bem como para calcular carga maquina. Este campo sera atualizado a cada produção.")]
        [Display(Name = "Setup (tempo total em segundos)")]
        [TAB(Value = "PRINCIPAL")] public double? ROT_TEMPO_SETUP { get; set; }
        [Required(ErrorMessage = "Setup Ajuste deve ser preenchido. O setup Ajuste é utilizado para definir a primera meta de setup bem como para calcular carga maquina. Este campo sera atualizado a cada produção.")]
        [Display(Name = "TEMPO SETUP AJUSTE SEG")]
        [TAB(Value = "PRINCIPAL")] public double? ROT_TEMPO_SETUP_AJUSTE { get; set; }
        [Display(Name = "PRÓXIMA SEQ TRNSFORM")]
        [TAB(Value = "PRINCIPAL")] public int? ROT_VA_PARA_SEQ_TRANSFORMACAO { get; set; }
        [Display(Name = "HIERARQUIA CALCULO")]
        [TAB(Value = "PRINCIPAL")] public double? ROT_HIERARQUIA_SEQ_TRANSFORMACAO { get; set; }
        [Display(Name = "AVALIA CUSTO")]
        [TAB(Value = "PRINCIPAL")] public int? ROT_AVALIA_CUSTO { get; set; }

        [Combobox(Description = "NAO", Value = "N")]
        [Combobox(Description = "SIM", Value = "S")]
        [Display(Name = "LINHA DIRETA")]
        [TAB(Value = "PRINCIPAL")] public string ROT_LINHA_DIRETA { get; set; }
            
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OPERACOES")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo ROT_OPERACOES")] public string ROT_OPERACOES { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "EXCECAO_OPERACOES")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo ROT_EXCECAO_OPERACOES")] public string ROT_EXCECAO_OPERACOES { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PERCENTUAL_INICIO_PASSO_ANTERIOR")] public double? ROT_PERCENTUAL_INICIO_PASSO_ANTERIOR { get; set; }
        [TAB(Value = "QUALIDADE")] [Display(Name = "TEMPLATE DE TESTES")] public int? TEM_ID { get; set; }
        //----
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  } 
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
        public virtual ICollection<FilaProducao> FilasProducao { get; set; }
        public virtual ICollection<Observacoes> Observacoes { get; set; }
        public virtual ICollection<Operacoes> Operacoes { get; set; }


        public virtual Produto Produto { get; set; }
        public virtual Maquina Maquina { get; set; }
        public virtual GrupoMaquina GrupoMaquina { get; set; }
        public virtual TemplateDeTestes TemplateDeTestes { get; set; }

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            List<object> ObjetosProcessados = new List<object>();
            foreach (object obj in objects)
            {

                if (obj.ToString() != "DynamicForms.Areas.PlugAndPlay.Models.Roteiro")
                {
                    continue;
                }

                using (var db = new ContextFactory().CreateDbContext(new string[] { }))
                {
                    Roteiro _Roteiro = (Roteiro)obj;
                    if (string.IsNullOrEmpty(_Roteiro.MAQ_ID) || _Roteiro.MAQ_ID == "0")
                    {
                        _Roteiro.MAQ_ID = "PLAYSIS";
                    }
                    else if (_Roteiro.MAQ_ID.Length > 0 && _Roteiro.MAQ_ID != "PLAYSIS" && !string.IsNullOrEmpty(_Roteiro.GMA_ID))
                    {
                        _Roteiro.PlayMsgErroValidacao = "O grupo e a máquina não podem ser preenchidos ao mesmo tempo!";
                        return false;
                    }
                    if (_Roteiro.MAQ_ID == "PLAYSIS" && string.IsNullOrEmpty(_Roteiro.GMA_ID) && _Roteiro.PlayAction != "delete")
                    {
                        _Roteiro.PlayMsgErroValidacao = "Combinação não permitida!";
                        return false;
                    }
                    if (_Roteiro.ROT_ACAO != "E" && (_Roteiro.ROT_PECAS_POR_PULSO <= 0 || _Roteiro.ROT_PECAS_POR_PULSO == null))
                    {
                        _Roteiro.PlayMsgErroValidacao = @"Peças por Pulso representa, a quantidade de produtos produzidos a cada 
                                        contagem do sensor. Seu valor deve ser maior que zero.";
                        return false;
                    }
                    if (_Roteiro.ROT_PERFORMANCE <= 0 && _Roteiro.PlayAction != "delete")
                    {
                        _Roteiro.PlayMsgErroValidacao = @"Performance deve ter valor maior que zero. A performance é utilizada para definir a primera meta de performance bem como para calcular carga maquina.Este campo sera atualizado a cada produção.";
                        return false;
                    }
                }
            }
            objects.AddRange(ObjetosProcessados);
            return true;
        }

        // descomentar quando mudarmos a criação de OPs pracá
        //public bool AfterChangesInTransaction(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert, JSgi db = null)
        //{
        //    if (db == null)
        //    {
        //        db = new ContextFactory().CreateDbContext(new string[] { });
        //    }

        //    List<object> newObjects = new List<object>();

        //    IEnumerable<Roteiro> roteiros = objects.Where(r => r.GetType().Name == nameof(Roteiro)).Cast<Roteiro>();
        //    var roteirosAgrupadosPorProduto = roteiros.GroupBy(r => r.PRO_ID);
        //    HashSet<string> produtosComRoteiroAlterado = new HashSet<string>();

        //    foreach (IGrouping<string, Roteiro> roteiro in roteirosAgrupadosPorProduto)
        //    {
        //        // verificando se existe uma nova sequencia de transformação
        //        var seqTranInsert = roteiros.Where(r => r.PlayAction.Equals("insert", System.StringComparison.OrdinalIgnoreCase))
        //            .Select(r => r.ROT_SEQ_TRANFORMACAO).ToHashSet();
        //        foreach (var seqTran in seqTranInsert)
        //        {
        //            int countSeqTran = db.Roteiro.Count(r => r.PRO_ID.Equals(roteiro.Key) && r.ROT_SEQ_TRANFORMACAO == seqTran);
        //            if (countSeqTran == 1)
        //            {// Significa que o roteiro do produto tem uma nova sequencia de transformação
        //                produtosComRoteiroAlterado.Add(roteiro.Key);
        //            }
        //        }

        //        // verificando se não existe mais nenhuma sequência de transformação igual a foi deletada
        //        var seqTranDelete = roteiros.Where(r => r.PlayAction.Equals("delete", System.StringComparison.OrdinalIgnoreCase))
        //            .Select(r => r.ROT_SEQ_TRANFORMACAO).ToHashSet();
        //        foreach (var seqTran in seqTranDelete)
        //        {
        //            int countSeqTran = db.Roteiro.Count(r => r.PRO_ID.Equals(roteiro.Key) && r.ROT_SEQ_TRANFORMACAO == seqTran);
        //            if (countSeqTran == 0)
        //            {// Significa que não tem mais nenhuma sequência de transformação igual a que foi deletada
        //                produtosComRoteiroAlterado.Add(roteiro.Key);
        //            }
        //        }
        //    }

        //    List<Order> pedidosEmAberto = new List<Order>();
        //    Order aux = new Order();
        //    foreach (var produto in produtosComRoteiroAlterado)
        //    {
        //        pedidosEmAberto.AddRange(aux.GetPedidosEmAberto(produto));
        //    }

        //    pedidosEmAberto.ForEach(pedido => {
        //        newObjects.AddRange(aux.ExplodirOPs(pedido, ref Logs, db));
        //    });

        //    //Aqui é adicionado o prefixo AFTER- para que o método AfterChangesInTransaction do MasterController possa identificar quais são os novos objetos da lista
        //    foreach (dynamic item in newObjects)
        //    {
        //        item.PlayAction = $"AFTER-{item.PlayAction}";
        //    }
        //    objects.InsertRange(0, newObjects);
        //    return true;
        //}
    }
}
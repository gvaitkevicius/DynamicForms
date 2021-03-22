using DynamicForms.Models;
using NPOI.POIFS.EventFileSystem;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class ClpMedicoes
    {
        public ClpMedicoes()
        {

        }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] public int Id { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "MAQUINA")] public string MaquinaId { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA INICIO")] public DateTime DataInicio { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA FIM")] [Required(ErrorMessage = "Campo DataFim requerido.")] public DateTime DataFim { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE")] public double Quantidade { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "GRUPO")] public double? Grupo { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "STATUS")] public int? Status { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TURNO")] public string TurnoId { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TURMA")] public string TurmaId { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OCORRENCIA")] public string OcorrenciaId { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID LOTE (CLP)")] public int? IdLoteClp { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FASE")] public int? Fase { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA EMISSAO")] public DateTime? Emissao { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORIGEM CLP")] public string ClpOrigem { get; set; }
        [NotMapped]
        public double QtdOriginal { get; set; }

        /// <summary>
        /// Esta propriedade foi criada para representar o estado do objeto
        /// insert, update, delete ou unchanged 
        /// </summary>
        [NotMapped] public string PlayAction { get; set; }
        /// <summary>
        /// Deve seguir a seguinte convecão: NameProperty:MsgErro;NameProperty:MsgErro; ...
        /// Representa os erros de validacao deste objeto
        /// </summary>
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

    }

    public class SetFaseFinal_ClpMedicoes
    {
        // CLP Medições
        public string ClpOrigem { get; set; }
        public DateTime DataFim { get; set; }
        public DateTime DataInicio { get; set; }
        public double Quantidade { get; set; }
        public int Fase { get; set; }
        
        // Feedback
        public double? QuantidadePecasPorPulso { get; set; }
        public int UsuarioId { get; set; }
        public string TurmaId { get; set; }
        public string TurnoId { get; set; }
        public DateTime FeeDataInicial { get; set; }
        public string OcorrenciaId { get; set; }

        // Movimento Estoque
        public double? MOV_QUANTIDADE { get; set; }
        public string TIP_ID { get; set; }

    }


    public class Schedule_ClpMedicoes
    {
        public int Id { get; set; }
        public string MaquinaId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public double Quantidade { get; set; }
        public double? Grupo { get; set; }
        public int? Status { get; set; }
        public string TurnoId { get; set; }
        public string TurmaId { get; set; }
        public string StatusMenssagem { get; set; }
        //public string OrdemProducaoId { get; set; }
        public string OcorrenciaId { get; set; }
        public int? LoteClpId { get; set; }
        public int? Fase { get; set; }
        public DateTime? Emissao { get; set; }

        //public string ProdutoId { get; set; }
        //public int? SequenciaTransformacaoId { get; set; }
        //public int? SequenciaRepeticaoId { get; set; }
        //public int? FilaProducaoId { get; set; }
        public string ClpOrigem { get; set; }
        public int AuxPendenteId { get; set; }
        public int? FeeID { get; set; }
        public double? FardosPorPalet { get; set; }
        public int Lote { get; set; }

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

}
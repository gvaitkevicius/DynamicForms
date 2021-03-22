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
    [Display(Name = "PEDIDOS COM LOTES DISPONIVEIS")]
    public class V_PEDIDOS_COM_LOTES_DISPONIVEIS
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRODUTO")] [Required(ErrorMessage = "Campo PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LOTE")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MOV_LOTE")] public string MOV_LOTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SUB_LOTE")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_SUB_LOTE")] public string MOV_SUB_LOTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PEDIDO")] [Required(ErrorMessage = "Campo ORD_ID requirido.")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ENDERECO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MOV_ENDERECO")] public string MOV_ENDERECO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "APROVEITAMENTO")] [Required(ErrorMessage = "Campo MOV_APROVEITAMENTO requirido.")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo MOV_APROVEITAMENTO")] public string MOV_APROVEITAMENTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SALDO")] public double? SALDO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "RESERVA")] public double? QTD_RESERVA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID CARGA")] public string CAR_ID { get; set; }
        public double DISPONIVEL { get; set; }

        [NotMapped] public T_Usuario UsuarioLogado { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        public V_PEDIDOS_COM_LOTES_DISPONIVEIS()
        {

        }
    }
}
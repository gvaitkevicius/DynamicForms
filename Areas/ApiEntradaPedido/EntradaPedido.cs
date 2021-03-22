using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicForms.Areas.PlugAndPlay.Controllers;
using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Areas.SGI.Model;
using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DynamicForms.Areas.ApiEntradaPedido
{
    [Area("ApiEntradaPedido")]
    [Route("ApiEntradaPedido/[controller]")]
    [ApiController]
    public class EntradaPedido : Controller
    {

        [HttpGet]
        [Route("SimularIncluirPedido")]
        public JsonResult SimularIncluirPedido(string pro_id, string cli_id, string mun_id, string ord_id, int quantidade, string data_entrega, string ord_status, int rep_id)
        {
            /*
                ApiEntradaPedido/EntradaPedido/SimularIncluirPedido?pro_id=OCXM060602&cli_id=00062801&ord_id=CM0010&quantidade=4000&data_entrega=2021-03-15&ord_status=&rep_id=1
            */

            List<V_OPS_A_PLANEJAR> logs = new List<V_OPS_A_PLANEJAR>();

            string status = "ADIAR"; //"ADIAR" //"ERRO"
            string msgRetorno = "";
            bool somente_simulacao = ord_status != null && ord_status.Contains("R") ? false : true;

            DateTime tempData = UtilPlay.ConvertStringToDate(data_entrega);
            DateTime data_fim = new DateTime(tempData.Year, tempData.Month, tempData.Day, 23, 50, 50);
            DateTime terminoMinimo = DateTime.Now;

            List<object[]> embarque = new List<object[]>();
            DateTime inicioJanelaEmbarque = DateTime.Now;
            DateTime fimJanelaEmbarque = DateTime.Now;
            DateTime embarqueAlvo = DateTime.Now;

            bool flag = UtilPlay.SimularPedido(pro_id, quantidade, cli_id, ref status, ref terminoMinimo, ref msgRetorno, ref data_fim, ref logs, ref embarque);
            if (flag)
            {
                double tempoTotal = 0;
                foreach(var item in logs)
                {
                    tempoTotal += Convert.ToDouble(item.TempoProducao);
                }
                tempoTotal = Math.Round(tempoTotal);

                UtilPlay.calcularSaldoRepresentante(rep_id, tempoTotal, logs[0].DataHoraNecessidadeInicioProducao, data_fim, ord_status, ref msgRetorno);

                if (!somente_simulacao)
                    UtilPlay.IncluirPedidoFila(cli_id, mun_id, ord_status, ord_id, pro_id, quantidade, logs, terminoMinimo, ref status, ref msgRetorno);

                inicioJanelaEmbarque = Convert.ToDateTime(embarque[0][0]);
                fimJanelaEmbarque = Convert.ToDateTime(embarque[0][1]);
                embarqueAlvo = Convert.ToDateTime(embarque[0][2]);
            }
            
            return Json(new { status, terminoMinimo, msgRetorno, inicioJanelaEmbarque, fimJanelaEmbarque, embarqueAlvo});
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

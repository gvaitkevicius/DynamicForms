using DynamicForms.Areas.SGI.Model;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DynamicForms.Areas.SGI.Components
{
    public class PrintRowValor : ViewComponent
    {
        public IViewComponentResult Invoke(T_Indicadores indicador)
        {
            string formatoValor = UtilsSGI.GetFormatoValor(indicador.T_Metas.FirstOrDefault().MET_TIPOALVO);
            ViewBag.Indicador = indicador;
            ViewBag.formatoValor = formatoValor;
            return View();
        }
    }
}

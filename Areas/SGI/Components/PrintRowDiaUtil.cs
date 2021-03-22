

using DynamicForms.Areas.SGI.Model;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DynamicForms.Areas.SGI.Components
{
    public class PrintRowDiaUtil : ViewComponent
    {
        public IViewComponentResult Invoke(MedicoesInd indicador)
        {
            string formatoValor = UtilsSGI.GetFormatoValor(indicador.Indicador.T_Metas.FirstOrDefault().MET_TIPOALVO);
            ViewBag.Indicador = indicador;
            ViewBag.formatoValor = formatoValor;
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace DynamicForms.Components
{
    public class BarraCoresDesenpenhoProducao : ViewComponent
    {
        public IViewComponentResult Invoke(int tipo, bool label)
        {
            ViewBag.tipo = tipo;
            ViewBag.label = label;
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace DynamicForms.Components
{
    public class PrintJustificativa : ViewComponent
    {
        public IViewComponentResult Invoke(string txtId, string tipoFeed, string heigth)
        {
            ViewBag.txtId = txtId;
            ViewBag.tipoFeed = tipoFeed;
            ViewBag.heigth = heigth;
            return View();
        }
    }
}

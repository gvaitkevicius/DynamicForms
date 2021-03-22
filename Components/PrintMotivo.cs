using Microsoft.AspNetCore.Mvc;

namespace DynamicForms.Components
{
    public class PrintMotivo : ViewComponent
    {
        public IViewComponentResult Invoke(string sltId, string tipoFeed)
        {
            ViewBag.sltId = sltId;
            ViewBag.tipoFeed = tipoFeed;
            return View();
        }
    }
}

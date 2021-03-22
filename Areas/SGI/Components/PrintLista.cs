using Microsoft.AspNetCore.Mvc;

namespace DynamicForms.Areas.SGI.Components
{
    public class PrintLista : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}

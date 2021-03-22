
using Microsoft.AspNetCore.Mvc;

namespace DynamicForms.Areas.SGI.Components
{
    public class PrintMetas : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}

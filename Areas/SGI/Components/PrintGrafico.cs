
using Microsoft.AspNetCore.Mvc;

namespace DynamicForms.Areas.SGI.Components
{
    public class PrintGrafico : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}

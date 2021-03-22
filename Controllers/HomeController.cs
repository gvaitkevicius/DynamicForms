using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Rotativa.AspNetCore;

namespace DynamicForms.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ReportUsuario()
        {
            var user = ObterUsuarioLogado();
            return new ViewAsPdf(user);
        }
    }
}

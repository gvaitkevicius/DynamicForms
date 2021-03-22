using Microsoft.AspNetCore.Mvc;

namespace DynamicForms.Components
{
    public class ProducaoDescTopPage : ViewComponent
    {
        public IViewComponentResult Invoke(string maquina, string op, string pedido, string descricao)
        {
            ViewBag.maquina = maquina;
            ViewBag.op = op;
            ViewBag.pedido = pedido;
            ViewBag.descricao = descricao;
            return View();
        }
    }
}

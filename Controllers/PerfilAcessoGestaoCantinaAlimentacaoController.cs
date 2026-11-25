using Microsoft.AspNetCore.Mvc;

namespace UnivManagement.Controllers
{
    public class PerfilAcessoGestaoCantinaAlimentacaoController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }

        // GET: /PerfilAcessoGestaoCantinaAlimentacao/Cliente
        public IActionResult Consumidor()
        {
            return RedirectToAction("Index", "GestaoCantinaAlimentacaoConsumidor");

        }

        // GET: /PerfilAcessoGestaoCantinaAlimentacao/Gestao
        public IActionResult Gestao()
        {

            return RedirectToAction("Index", "GestaoCantinaAlimentacaoAdmin");
        }
    }
}

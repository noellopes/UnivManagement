using Microsoft.AspNetCore.Mvc;

namespace UnivManagement.Controllers
{
    public class PerfilAcessoGestaoCantinaAlimentacaoController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }

        // GET: //Cliente
        public IActionResult Cliente()
        {
            return View();
        }

        // GET: /
        public IActionResult Gestao()
        {

            return RedirectToAction("Index", "GestaoCantinaAlimentacaoAdmin");
        }
    }
}

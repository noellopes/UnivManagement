using Microsoft.AspNetCore.Mvc;

namespace UnivManagement.Controllers
{
    public class BibliotecaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

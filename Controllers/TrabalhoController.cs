using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UnivManagement.Models;

namespace UnivManagement.Controllers
{
    public class TrabalhoController : Controller
    {
        public IActionResult Interface_Docente()
        {
            return View();
        }
        public IActionResult Sub_Trab()
        {
            return View();
        }

        public IActionResult Index2()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

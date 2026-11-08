using Microsoft.AspNetCore.Mvc;
using UnivManagement.Models;

namespace UnivManagement.Controllers
{
    public class DocumentoController : Controller
    {

        [HttpGet]
        public IActionResult CriarDocumento()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CriarDocumento(Documento documento)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(CriarDocumento));
            }

            return View(documento);
        }
    }
}
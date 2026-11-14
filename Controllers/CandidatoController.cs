using Microsoft.AspNetCore.Mvc;
using UnivManagement.Models;

namespace UnivManagement.Controllers
{
    public class CandidatoController : Controller
    {

        [HttpGet]
        public IActionResult CriarCandidato()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CriarCandidato(Candidato candidato)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(CriarCandidato));
            }

            return View(candidato);
        }
    }
}
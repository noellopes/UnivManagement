using Microsoft.AspNetCore.Mvc;
using UnivManagement.Models;

namespace UnivManagement.Controllers
{
    public class RefeicoesController : Controller
    {

        // GET: /Refeicoes/Index
        public IActionResult Index()
        {
            return View(RefeicaoRepository.Refeicoes);
        }

        // GET: /Refeicoes/Criar
        [HttpGet]
        public IActionResult Criar()
        {
            return View();
        }

        // POST: /Refeicoes/Criar
        [HttpPost]
        public IActionResult Criar(Refeicao refeicao)
        {
            if (ModelState.IsValid)
            {
                RefeicaoRepository.Adicionar(refeicao);
                return RedirectToAction("Index");
            }
            return View(refeicao);
        }
    }
}
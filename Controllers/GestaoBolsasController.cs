using Microsoft.AspNetCore.Mvc;
using UnivManagement.Models;
using System.Collections.Generic;
using System.Linq;

namespace UnivManagement.Controllers
{
    public class GestaoBolsasController : Controller
    {
        private static List<Bolsa> _bolsas = new List<Bolsa>
        {
            new Bolsa { Id = 1, Titulo = "Bolsa de Mérito 2026", Descricao = "Destina-se a alunos com média superior a 16 valores.", Estado = "Ativa" },
            new Bolsa { Id = 2, Titulo = "Bolsa de Ação Social", Descricao = "Apoio para alunos com rendimento per capita inferior a 9000€.", Estado = "Encerrada" }
        };

        public IActionResult Index()
        {
            return View(_bolsas);
        }

        public IActionResult Candidatura()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmeterCandidatura(Candidatura model)
        {
            return RedirectToAction("Index");
        }

        public IActionResult Admin()
        {
            return View(_bolsas);
        }

        [HttpPost]
        public IActionResult GuardarBolsa(string Titulo, string Descricao)
        {
            int nextId = _bolsas.Any() ? _bolsas.Max(b => b.Id) + 1 : 1;
            _bolsas.Add(new Bolsa { Id = nextId, Titulo = Titulo, Descricao = Descricao, Estado = "Ativa" });
            return RedirectToAction("Admin");
        }

        [HttpPost]
        public IActionResult AtualizarBolsa(int id, string Titulo, string Descricao)
        {
            var bolsa = _bolsas.FirstOrDefault(b => b.Id == id);
            if (bolsa != null)
            {
                bolsa.Titulo = Titulo;
                bolsa.Descricao = Descricao;
            }
            return RedirectToAction("Admin");
        }

        public IActionResult ApagarBolsa(int id)
        {
            var bolsa = _bolsas.FirstOrDefault(b => b.Id == id);
            if (bolsa != null)
            {
                _bolsas.Remove(bolsa);
            }
            return RedirectToAction("Admin");
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace UnivManagement.Controllers
{
    public class AvaliacoesController : Controller
    {
        private readonly UniversityDbContext _context;

        public AvaliacoesController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: Avaliacoes
        public async Task<IActionResult> Index(string sortOrder)
        {
            var candidaturas = from c in _context.Set<CandidaturaBolsa>()
                               select c;

            // Ordenar por Classificação (Ranking) de forma decrescente
            candidaturas = candidaturas.OrderByDescending(c => c.Classificacao);

            return View(await candidaturas.ToListAsync());
        }

        // GET: Avaliacoes/Avaliar/5
        public async Task<IActionResult> Avaliar(int? id)
        {
            if (id == null) return NotFound();

            var candidatura = await _context.Set<CandidaturaBolsa>()
                .FirstOrDefaultAsync(m => m.Id_CandidaturaBolsa == id);

            if (candidatura == null) return NotFound();

            return View(new AvaliacaoBolsa { Id_CandidaturaBolsa = candidatura.Id_CandidaturaBolsa });
        }

        // POST: Avaliacoes/Avaliar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Avaliar(AvaliacaoBolsa avaliacao)
        {
            if (avaliacao.Id_CandidaturaBolsa > 0)
            {
                // Check if an evaluation already exists for this application to avoid key conflicts
                var existingAvaliacao = await _context.Set<AvaliacaoBolsa>()
                    .FirstOrDefaultAsync(a => a.Id_CandidaturaBolsa == avaliacao.Id_CandidaturaBolsa);

                if (existingAvaliacao != null)
                {
                    // Update existing evaluation record
                    existingAvaliacao.Resultado = avaliacao.Resultado;
                    existingAvaliacao.Parecer = string.IsNullOrEmpty(avaliacao.Parecer) ? "Parecer técnico atualizado." : avaliacao.Parecer;
                    existingAvaliacao.Observacoes = avaliacao.Observacoes;
                    existingAvaliacao.Data_Avaliacao = DateTime.Now;
                    _context.Update(existingAvaliacao);
                }
                else
                {
                    // Create a brand new evaluation record
                    avaliacao.Data_Avaliacao = DateTime.Now;
                    avaliacao.Id_Tecnico = 1;
                    if (string.IsNullOrEmpty(avaliacao.Parecer))
                    {
                        avaliacao.Parecer = "Parecer técnico emitido automaticamente.";
                    }
                    _context.Add(avaliacao);
                }

                // Update the corresponding Candidature's state and decision date
                var candidatura = await _context.Set<CandidaturaBolsa>().FindAsync(avaliacao.Id_CandidaturaBolsa);
                if (candidatura != null)
                {
                    candidatura.DataDecisao = DateTime.Now;
                    candidatura.Estado = string.IsNullOrEmpty(avaliacao.Resultado) ? "Aprovado" : avaliacao.Resultado;
                    _context.Update(candidatura);
                }

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"A decisão ({avaliacao.Resultado}) foi registada com sucesso!";

                return RedirectToAction(nameof(Index));
            }

            return View(avaliacao);
        }

        // GET: Avaliacoes/SeedData
        public async Task<IActionResult> SeedData()
        {
            if (!_context.CandidaturasBolsa.Any())
            {
                _context.CandidaturasBolsa.AddRange(
                    new CandidaturaBolsa { NomeEstudante = "Ana Silva", Classificacao = 18.5, DataSubmissao = DateTime.Now.AddDays(-10), Estado = "Pendente" },
                    new CandidaturaBolsa { NomeEstudante = "João Mota", Classificacao = 14.0, DataSubmissao = DateTime.Now.AddDays(-5), Estado = "Pendente" },
                    new CandidaturaBolsa { NomeEstudante = "Maria Santos", Classificacao = 19.2, DataSubmissao = DateTime.Now.AddDays(-2), Estado = "Pendente" },
                    new CandidaturaBolsa { NomeEstudante = "Tiago Costa", Classificacao = 12.8, DataSubmissao = DateTime.Now.AddDays(-1), Estado = "Pendente" }
                );

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
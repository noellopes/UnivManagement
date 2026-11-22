using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Controllers
{
    public class AvaliacaoBolsasController : Controller
    {
        private readonly UniversityDbContext _context;

        public AvaliacaoBolsasController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: AvaliacaoBolsas
        public async Task<IActionResult> Index()
        {
            var avaliacoes = _context.AvaliacaoBolsas
                .Include(a => a.Tecnico)
                .OrderByDescending(a => a.DataAvaliacao);

            return View(await avaliacoes.ToListAsync());
        }

        // GET: AvaliacaoBolsas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var avaliacaoBolsa = await _context.AvaliacaoBolsas
                .Include(a => a.Tecnico)
                .FirstOrDefaultAsync(m => m.AvaliacaoBolsaId == id);

            if (avaliacaoBolsa == null)
            {
                return NotFound();
            }

            return View(avaliacaoBolsa);
        }

        // GET: AvaliacaoBolsas/Create
        public IActionResult Create()
        {
            ViewData["TecnicoId"] = new SelectList(_context.Tecnicos, "TecnicoId", "Nome");
            return View();
        }

        // POST: AvaliacaoBolsas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AvaliacaoBolsaId,CandidaturaBolsaId,TecnicoId,DataAvaliacao,Resultado,Parecer,Observacoes")] AvaliacaoBolsa avaliacaoBolsa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(avaliacaoBolsa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["TecnicoId"] = new SelectList(_context.Tecnicos, "TecnicoId", "Nome", avaliacaoBolsa.TecnicoId);
            return View(avaliacaoBolsa);
        }

        // GET: AvaliacaoBolsas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var avaliacaoBolsa = await _context.AvaliacaoBolsas.FindAsync(id);
            if (avaliacaoBolsa == null)
            {
                return NotFound();
            }

            ViewData["TecnicoId"] = new SelectList(_context.Tecnicos, "TecnicoId", "Nome", avaliacaoBolsa.TecnicoId);
            return View(avaliacaoBolsa);
        }

        // POST: AvaliacaoBolsas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AvaliacaoBolsaId,CandidaturaBolsaId,TecnicoId,DataAvaliacao,Resultado,Parecer,Observacoes")] AvaliacaoBolsa avaliacaoBolsa)
        {
            if (id != avaliacaoBolsa.AvaliacaoBolsaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(avaliacaoBolsa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AvaliacaoBolsaExists(avaliacaoBolsa.AvaliacaoBolsaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["TecnicoId"] = new SelectList(_context.Tecnicos, "TecnicoId", "Nome", avaliacaoBolsa.TecnicoId);
            return View(avaliacaoBolsa);
        }

        // GET: AvaliacaoBolsas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var avaliacaoBolsa = await _context.AvaliacaoBolsas
                .Include(a => a.Tecnico)
                .FirstOrDefaultAsync(m => m.AvaliacaoBolsaId == id);

            if (avaliacaoBolsa == null)
            {
                return NotFound();
            }

            return View(avaliacaoBolsa);
        }

        // POST: AvaliacaoBolsas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var avaliacaoBolsa = await _context.AvaliacaoBolsas.FindAsync(id);
            if (avaliacaoBolsa != null)
            {
                _context.AvaliacaoBolsas.Remove(avaliacaoBolsa);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AvaliacaoBolsaExists(int id)
        {
            return _context.AvaliacaoBolsas.Any(e => e.AvaliacaoBolsaId == id);
        }
    }
}
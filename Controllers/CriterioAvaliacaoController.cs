using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Controllers
{
    public class CriterioAvaliacaoController : Controller
    {
        private readonly UniversityDbContext _context;

        public CriterioAvaliacaoController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: CriterioAvaliacao
        public async Task<IActionResult> Index()
        {
            var criterios = await _context.CriterioAvaliacao
                .Include(c => c.UC)
                .ToListAsync();
            return View(criterios);
        }

        // GET: CriterioAvaliacao/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var criterioAvaliacao = await _context.CriterioAvaliacao
                .Include(c => c.UC)
                .FirstOrDefaultAsync(m => m.CriterioAvaliacaoId == id);

            if (criterioAvaliacao == null)
            {
                return NotFound();
            }

            return View(criterioAvaliacao);
        }

        // GET: CriterioAvaliacao/Create
        public IActionResult Create()
        {
            ViewData["UcId"] = new SelectList(_context.UC, "UcId", "Name");
            return View();
        }

        // POST: CriterioAvaliacao/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CriterioAvaliacaoId,UcId,PesoTrab")] CriterioAvaliacao criterioAvaliacao)
        {
            bool jaExisteCriterioParaUc = await _context.CriterioAvaliacao
        .AnyAsync(c => c.UcId == criterioAvaliacao.UcId);

            if (jaExisteCriterioParaUc)
            {
                ModelState.AddModelError("UcId", "Já existe um critério de avaliação associado a esta Unidade Curricular.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(criterioAvaliacao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["UcId"] = new SelectList(_context.UC, "UcId", "Name", criterioAvaliacao.UcId);
            return View(criterioAvaliacao);
        }

        // GET: CriterioAvaliacao/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var criterioAvaliacao = await _context.CriterioAvaliacao.FindAsync(id);
            if (criterioAvaliacao == null)
            {
                return NotFound();
            }

            ViewData["UcId"] = new SelectList(_context.UC, "UcId", "Name", criterioAvaliacao.UcId);
            return View(criterioAvaliacao);
        }

        // POST: CriterioAvaliacao/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CriterioAvaliacaoId,UcId,PesoTrab")] CriterioAvaliacao criterioAvaliacao)
        {
            if (id != criterioAvaliacao.CriterioAvaliacaoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(criterioAvaliacao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CriterioAvaliacaoExists(criterioAvaliacao.CriterioAvaliacaoId))
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

            ViewData["UcId"] = new SelectList(_context.UC, "UcId", "Name", criterioAvaliacao.UcId);
            return View(criterioAvaliacao);
        }

        // GET: CriterioAvaliacao/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var criterioAvaliacao = await _context.CriterioAvaliacao
                .Include(c => c.UC)
                .FirstOrDefaultAsync(m => m.CriterioAvaliacaoId == id);

            if (criterioAvaliacao == null)
            {
                return NotFound();
            }

            return View(criterioAvaliacao);
        }

        // POST: CriterioAvaliacao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var criterioAvaliacao = await _context.CriterioAvaliacao.FindAsync(id);
            if (criterioAvaliacao != null)
            {
                _context.CriterioAvaliacao.Remove(criterioAvaliacao);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CriterioAvaliacaoExists(int id)
        {
            return _context.CriterioAvaliacao.Any(e => e.CriterioAvaliacaoId == id);
        }
    }
}
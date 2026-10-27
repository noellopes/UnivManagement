using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Controllers
{
    public class RefeicoesController : Controller
    {
        private readonly UniversityDbContext _context;

        public RefeicoesController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: Refeicoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Refeicoes.ToListAsync());
        }

        // GET: Refeicoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var refeicao = await _context.Refeicoes
                .FirstOrDefaultAsync(m => m.RefeicaoId == id);
            if (refeicao == null) return NotFound();

            return View(refeicao);
        }

        // GET: Refeicoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Refeicoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RefeicaoId,Nome,Tipo,Preco")] Refeicao refeicao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(refeicao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(refeicao);
        }

        // GET: Refeicoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var refeicao = await _context.Refeicoes.FindAsync(id);
            if (refeicao == null) return NotFound();
            return View(refeicao);
        }

        // POST: Refeicoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RefeicaoId,Nome,Tipo,Preco")] Refeicao refeicao)
        {
            if (id != refeicao.RefeicaoId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(refeicao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RefeicaoExists(refeicao.RefeicaoId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(refeicao);
        }

        // GET: Refeicoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var refeicao = await _context.Refeicoes
                .FirstOrDefaultAsync(m => m.RefeicaoId == id);
            if (refeicao == null) return NotFound();

            return View(refeicao);
        }

        // POST: Refeicoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var refeicao = await _context.Refeicoes.FindAsync(id);
            if (refeicao != null)
            {
                _context.Refeicoes.Remove(refeicao);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RefeicaoExists(int id)
        {
            return _context.Refeicoes.Any(e => e.RefeicaoId == id);
        }
    }
}
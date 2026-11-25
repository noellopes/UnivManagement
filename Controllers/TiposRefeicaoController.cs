using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Controllers
{
    public class TiposRefeicaoController : Controller
    {
        private readonly UniversityDbContext _context;

        public TiposRefeicaoController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: TiposRefeicao
        public async Task<IActionResult> Index()
        {
            return View(await _context.TiposRefeicao.ToListAsync());
        }

        // GET: TiposRefeicao/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoRefeicao = await _context.TiposRefeicao
                .FirstOrDefaultAsync(m => m.TipoRefeicaoId == id);
            if (tipoRefeicao == null)
            {
                return NotFound();
            }

            return View(tipoRefeicao);
        }

        // GET: TiposRefeicao/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TiposRefeicao/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TipoRefeicaoId,Nome")] TipoRefeicao tipoRefeicao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoRefeicao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoRefeicao);
        }

        // GET: TiposRefeicao/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoRefeicao = await _context.TiposRefeicao.FindAsync(id);
            if (tipoRefeicao == null)
            {
                return NotFound();
            }
            return View(tipoRefeicao);
        }

        // POST: TiposRefeicao/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TipoRefeicaoId,Nome")] TipoRefeicao tipoRefeicao)
        {
            if (id != tipoRefeicao.TipoRefeicaoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoRefeicao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoRefeicaoExists(tipoRefeicao.TipoRefeicaoId))
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
            return View(tipoRefeicao);
        }

        // GET: TiposRefeicao/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoRefeicao = await _context.TiposRefeicao
                .FirstOrDefaultAsync(m => m.TipoRefeicaoId == id);
            if (tipoRefeicao == null)
            {
                return NotFound();
            }

            ViewData["NumRefeicoes"] = await _context.Refeicoes.CountAsync(r => r.TipoRefeicaoId == id);
            return View(tipoRefeicao);
        }

        // POST: TiposRefeicao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoRefeicao = await _context.TiposRefeicao.FindAsync(id);
            if (tipoRefeicao == null)
            {
                return RedirectToAction(nameof(Index));
            }

            // Não apagar um tipo que ainda tem refeições (senão as refeições seriam apagadas em cascata)
            var numRefeicoes = await _context.Refeicoes.CountAsync(r => r.TipoRefeicaoId == id);
            if (numRefeicoes > 0)
            {
                ViewData["NumRefeicoes"] = numRefeicoes;
                return View(tipoRefeicao);
            }

            _context.TiposRefeicao.Remove(tipoRefeicao);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoRefeicaoExists(int id)
        {
            return _context.TiposRefeicao.Any(e => e.TipoRefeicaoId == id);
        }
    }
}
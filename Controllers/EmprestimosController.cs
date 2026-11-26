using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Controllers
{
    public class EmprestimosController : Controller
    {
        private readonly UniversityDbContext _context;

        public EmprestimosController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: Emprestimos
        public async Task<IActionResult> Index()
        {
            var emprestimos = await _context.Emprestimo
                .Include(e => e.Exemplar)
                .ToListAsync();
            return View(emprestimos);
        }

        // GET: Emprestimos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprestimo = await _context.Emprestimo
                .Include(e => e.Exemplar)
                .FirstOrDefaultAsync(m => m.EmprestimoId == id);
            if (emprestimo == null)
            {
                return NotFound();
            }

            return View(emprestimo);
        }

        // GET: Emprestimos/Create
        public IActionResult Create()
        {
            ViewData["ExemplarId"] = new SelectList(_context.Exemplares, "ExemplarId", "Codigo");
            return View();
        }

        // POST: Emprestimos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmprestimoId,dataEmprestimo,dataDevolucao,estado,ExemplarId")] Emprestimo emprestimo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(emprestimo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ExemplarId"] = new SelectList(_context.Exemplares, "ExemplarId", "Codigo", emprestimo.ExemplarId);
            return View(emprestimo);
        }

        // GET: Emprestimos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprestimo = await _context.Emprestimo.FindAsync(id);
            if (emprestimo == null)
            {
                return NotFound();
            }

            ViewData["ExemplarId"] = new SelectList(_context.Exemplares, "ExemplarId", "Codigo", emprestimo.ExemplarId);
            return View(emprestimo);
        }

        // POST: Emprestimos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmprestimoId,dataEmprestimo,dataDevolucao,estado,ExemplarId")] Emprestimo emprestimo)
        {
            if (id != emprestimo.EmprestimoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emprestimo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmprestimoExists(emprestimo.EmprestimoId))
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

            ViewData["ExemplarId"] = new SelectList(_context.Exemplares, "ExemplarId", "Codigo", emprestimo.ExemplarId);
            return View(emprestimo);
        }

        // GET: Emprestimos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprestimo = await _context.Emprestimo
                .Include(e => e.Exemplar)
                .FirstOrDefaultAsync(m => m.EmprestimoId == id);
            if (emprestimo == null)
            {
                return NotFound();
            }

            return View(emprestimo);
        }

        // POST: Emprestimos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emprestimo = await _context.Emprestimo.FindAsync(id);
            if (emprestimo != null)
            {
                _context.Emprestimo.Remove(emprestimo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmprestimoExists(int id)
        {
            return _context.Emprestimo.Any(e => e.EmprestimoId == id);
        }
    }
}
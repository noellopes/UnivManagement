using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Controllers
{
    public class ExemplarController : Controller
    {
        private readonly UniversityDbContext _context;

        public ExemplarController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: Exemplar
        public async Task<IActionResult> Index()
        {
            var exemplares = await _context.Exemplares
                .Include(e => e.Obra)
                .ToListAsync();
            return View(exemplares);
        }

        // GET: Exemplar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exemplar = await _context.Exemplares
                .Include(e => e.Obra)
                .FirstOrDefaultAsync(m => m.ExemplarId == id);
            if (exemplar == null)
            {
                return NotFound();
            }

            return View(exemplar);
        }

        // GET: Exemplar/Create
        public IActionResult Create()
        {
            ViewData["ObraId"] = new SelectList(_context.Obras, "ObraId", "Titulo");
            return View();
        }

        // POST: Exemplar/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExemplarId,Codigo,Estado,ObraId")] Exemplar exemplar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(exemplar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ObraId"] = new SelectList(_context.Obras, "ObraId", "Titulo", exemplar.ObraId);
            return View(exemplar);
        }

        // GET: Exemplar/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exemplar = await _context.Exemplares.FindAsync(id);
            if (exemplar == null)
            {
                return NotFound();
            }

            ViewData["ObraId"] = new SelectList(_context.Obras, "ObraId", "Titulo", exemplar.ObraId);
            return View(exemplar);
        }

        // POST: Exemplar/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExemplarId,Codigo,Estado,ObraId")] Exemplar exemplar)
        {
            if (id != exemplar.ExemplarId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exemplar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExemplarExists(exemplar.ExemplarId))
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

            ViewData["ObraId"] = new SelectList(_context.Obras, "ObraId", "Titulo", exemplar.ObraId);
            return View(exemplar);
        }

        // GET: Exemplar/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exemplar = await _context.Exemplares
                .Include(e => e.Obra)
                .FirstOrDefaultAsync(m => m.ExemplarId == id);
            if (exemplar == null)
            {
                return NotFound();
            }

            return View(exemplar);
        }

        // POST: Exemplar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exemplar = await _context.Exemplares.FindAsync(id);
            if (exemplar != null)
            {
                _context.Exemplares.Remove(exemplar);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExemplarExists(int id)
        {
            return _context.Exemplares.Any(e => e.ExemplarId == id);
        }
    }
}
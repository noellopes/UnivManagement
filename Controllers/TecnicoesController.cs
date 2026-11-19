using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Controllers
{
    public class TecnicoesController : Controller
    {
        private readonly UniversityDbContext _context;

        public TecnicoesController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: Tecnicoes
        public async Task<IActionResult> Index(string? successMessage = null)
        {
            if (successMessage != null)
            {
                ViewBag.SuccessMessage = successMessage;
            }

            var tecnicos = _context.Tecnicos.Include(t => t.Departamento);
            return View(await tecnicos.ToListAsync());
        }

        // GET: Tecnicoes/Details/5
        public async Task<IActionResult> Details(int? id, string? successMessage = null)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var tecnico = await _context.Tecnicos
                .Include(t => t.Departamento)
                .FirstOrDefaultAsync(m => m.TecnicoId == id);

            if (tecnico == null)
            {
                return View("NotFound");
            }

            if (successMessage != null)
            {
                ViewBag.SuccessMessage = successMessage;
            }

            return View(tecnico);
        }

        // GET: Tecnicoes/Create
        public IActionResult Create()
        {
            ViewBag.DepartamentoId = new SelectList(_context.Departamentos, "DepartamentoId", "Nome");
            return View();
        }

        // POST: Tecnicoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TecnicoId,Nome,Email,DepartamentoId")] Tecnico tecnico)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tecnico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = tecnico.TecnicoId, successMessage = "Técnico criado com sucesso." });
            }

            ViewBag.DepartamentoId = new SelectList(_context.Departamentos, "DepartamentoId", "Nome", tecnico.DepartamentoId);
            return View(tecnico);
        }

        // GET: Tecnicoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var tecnico = await _context.Tecnicos.FindAsync(id);
            if (tecnico == null)
            {
                return View("NotFound");
            }

            ViewBag.DepartamentoId = new SelectList(_context.Departamentos, "DepartamentoId", "Nome", tecnico.DepartamentoId);
            return View(tecnico);
        }

        // POST: Tecnicoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("TecnicoId,Nome,Email,DepartamentoId")] Tecnico tecnico)
        {
            if (id != tecnico.TecnicoId)
            {
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tecnico);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TecnicoExists(tecnico.TecnicoId))
                    {
                        return View("NotFound");
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Details), new { id = tecnico.TecnicoId, successMessage = "Técnico atualizado com sucesso." });
            }

            ViewBag.DepartamentoId = new SelectList(_context.Departamentos, "DepartamentoId", "Nome", tecnico.DepartamentoId);
            return View(tecnico);
        }

        // GET: Tecnicoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var tecnico = await _context.Tecnicos
                .Include(t => t.Departamento)
                .FirstOrDefaultAsync(m => m.TecnicoId == id);

            if (tecnico == null)
            {
                return View("NotFound");
            }

            return View(tecnico);
        }

        // POST: Tecnicoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var tecnico = await _context.Tecnicos.FindAsync(id);
            if (tecnico != null)
            {
                _context.Tecnicos.Remove(tecnico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { successMessage = "Técnico eliminado com sucesso." });
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TecnicoExists(int? id)
        {
            return _context.Tecnicos.Any(e => e.TecnicoId == id);
        }
    }
}
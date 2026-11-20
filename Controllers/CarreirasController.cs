
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Controllers
{
    public class CarreirasController : Controller
    {
        private readonly UniversityDbContext _context;

        public CarreirasController(UniversityDbContext context)
        {
            _context = context;
        }

        private void CarregarEstudantes(object? selectedValue = null)
        {
            ViewBag.IdAluno = new SelectList(_context.Estudantes, "IdAluno", "Nome", selectedValue);
        }

        // GET: Carreiras
        public async Task<IActionResult> Index()
        {
            var carreiras = await _context.Carreiras
                .Include(c => c.Estudante)
                .ToListAsync();

            return View(carreiras);
        }

        // GET: Carreiras/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carreira = await _context.Carreiras
                .Include(c => c.Estudante)
                .FirstOrDefaultAsync(m => m.IdCarreira == id);

            if (carreira == null)
            {
                return NotFound();
            }

            return View(carreira);
        }

        // GET: Carreiras/Create
        public IActionResult Create()
        {
            CarregarEstudantes();
            return View();
        }

        // POST: Carreiras/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCarreira,IdAluno,CargoAtual,EmpresaAtual,Competencias,Certificacoes,HistoricoProfissional")] Carreira carreira)
        {
            ModelState.Remove(nameof(Carreira.Estudante));

            if (ModelState.IsValid)
            {
                _context.Add(carreira);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Carreira criada com sucesso.";
                return RedirectToAction(nameof(Index));
            }

            CarregarEstudantes(carreira.IdAluno);
            return View(carreira);
        }

        // GET: Carreiras/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carreira = await _context.Carreiras.FindAsync(id);
            if (carreira == null)
            {
                return NotFound();
            }

            CarregarEstudantes(carreira.IdAluno);
            return View(carreira);
        }

        // POST: Carreiras/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("IdCarreira,IdAluno,CargoAtual,EmpresaAtual,Competencias,Certificacoes,HistoricoProfissional")] Carreira carreira)
        {
            if (id != carreira.IdCarreira)
            {
                return NotFound();
            }

            ModelState.Remove(nameof(Carreira.Estudante));

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carreira);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Carreira atualizada com sucesso.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarreiraExists(carreira.IdCarreira))
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

            CarregarEstudantes(carreira.IdAluno);
            return View(carreira);
        }

        // GET: Carreiras/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carreira = await _context.Carreiras
                .Include(c => c.Estudante)
                .FirstOrDefaultAsync(m => m.IdCarreira == id);

            if (carreira == null)
            {
                return NotFound();
            }

            return View(carreira);
        }

        // POST: Carreiras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var carreira = await _context.Carreiras.FindAsync(id);
            if (carreira != null)
            {
                _context.Carreiras.Remove(carreira);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Carreira eliminada com sucesso.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CarreiraExists(int id)
        {
            return _context.Carreiras.Any(e => e.IdCarreira == id);
        }
    }
}

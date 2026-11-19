
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

        // GET: Carreiras/Details/5
        public async Task<IActionResult> Details(int? idcarreira)
        {
            if (idcarreira == null)
            {
                return NotFound();
            }

            var carreira = await _context.Carreiras
                .Include(c => c.Estudante)
                .FirstOrDefaultAsync(m => m.IdCarreira == idcarreira);

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

        // GET: Carreiras/Edit/5
        public async Task<IActionResult> Edit(int? idcarreira)
        {
            if (idcarreira == null)
            {
                return NotFound();
            }

            var carreira = await _context.Carreiras.FindAsync(idcarreira);
            if (carreira == null)
            {
                return NotFound();
            }

            CarregarEstudantes(carreira.IdAluno);
            return View(carreira);
        }

        // POST: Carreiras/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? idcarreira, [Bind("IdCarreira,IdAluno,CargoAtual,EmpresaAtual,Competencias,Certificacoes,HistoricoProfissional")] Carreira carreira)
        {
            if (idcarreira != carreira.IdCarreira)
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

        // GET: Carreiras/Delete/5
        public async Task<IActionResult> Delete(int? idcarreira)
        {
            if (idcarreira == null)
            {
                return NotFound();
            }

            var carreira = await _context.Carreiras
                .Include(c => c.Estudante)
                .FirstOrDefaultAsync(m => m.IdCarreira == idcarreira);

            if (carreira == null)
            {
                return NotFound();
            }

            return View(carreira);
        }

        // POST: Carreiras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? idcarreira)
        {
            var carreira = await _context.Carreiras.FindAsync(idcarreira);
            if (carreira != null)
            {
                _context.Carreiras.Remove(carreira);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Carreira eliminada com sucesso.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CarreiraExists(int idcarreira)
        {
            return _context.Carreiras.Any(e => e.IdCarreira == idcarreira);
        }
    }
}

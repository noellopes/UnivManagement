
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

namespace UnivManagement.Controllers
{
    public class ProgramaBolsasController : Controller
    {
        private readonly UniversityDbContext _context;

        public ProgramaBolsasController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: Index
        public async Task<IActionResult> Index()
        {
            var programas = await _context.ProgramasBolsa.ToListAsync();

            ViewBag.TiposPrograma = await _context.TiposProgramaBolsa
                .ToDictionaryAsync(t => t.IdTipoPrograma, t => t.NomeTipo);

            return View(programas);
        }

        // GET: Details
        public async Task<IActionResult> Details(int? idprograma)
        {
            if (idprograma == null)
                return NotFound();

            var programa = await _context.ProgramasBolsa
                .FirstOrDefaultAsync(p => p.IdPrograma == idprograma);

            if (programa == null)
                return NotFound();

            ViewBag.NomeTipo = await _context.TiposProgramaBolsa
                .Where(t => t.IdTipoPrograma == programa.IdTipoPrograma)
                .Select(t => t.NomeTipo)
                .FirstOrDefaultAsync();

            return View(programa);
        }

        // GET: Create
        public IActionResult Create()
        {
            CarregarTiposPrograma();
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Nome,Descricao,AnoLetivo,DataInicio,ValorMaximoBolsa,IdTipoPrograma")]
            ProgramaBolsa programabolsa)
        {
            if (!await _context.TiposProgramaBolsa.AnyAsync(
                t => t.IdTipoPrograma == programabolsa.IdTipoPrograma))
            {
                ModelState.AddModelError(
                    "IdTipoPrograma",
                    "Selecione um tipo de programa válido."
                );
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.ProgramasBolsa.Add(programabolsa);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] =
                        "Programa criado com sucesso!";

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    TempData["ErrorMessage"] =
                        "Ocorreu um erro ao criar o programa.";
                }
            }

            CarregarTiposPrograma(programabolsa.IdTipoPrograma);
            return View(programabolsa);
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int? idprograma)
        {
            if (idprograma == null)
                return NotFound();

            var programa = await _context.ProgramasBolsa
                .FindAsync(idprograma);

            if (programa == null)
                return NotFound();

            CarregarTiposPrograma(programa.IdTipoPrograma);

            return View(programa);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int? idprograma,
            [Bind("IdPrograma,Nome,Descricao,AnoLetivo,DataInicio,ValorMaximoBolsa,IdTipoPrograma")]
            ProgramaBolsa programabolsa)
        {
            if (idprograma != programabolsa.IdPrograma)
                return NotFound();

            if (!await _context.TiposProgramaBolsa.AnyAsync(
                t => t.IdTipoPrograma == programabolsa.IdTipoPrograma))
            {
                ModelState.AddModelError(
                    "IdTipoPrograma",
                    "Selecione um tipo de programa válido."
                );
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(programabolsa);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] =
                        "Programa atualizado com sucesso!";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ProgramaBolsaExists(programabolsa.IdPrograma))
                        return NotFound();

                    TempData["ErrorMessage"] =
                        "Ocorreu um erro ao atualizar o programa.";
                }
                catch
                {
                    TempData["ErrorMessage"] =
                        "Ocorreu um erro ao atualizar o programa.";
                }
            }

            CarregarTiposPrograma(programabolsa.IdTipoPrograma);
            return View(programabolsa);
        }

        // GET: Delete
        public async Task<IActionResult> Delete(int? idprograma)
        {
            if (idprograma == null)
                return NotFound();

            var programa = await _context.ProgramasBolsa
                .FirstOrDefaultAsync(p => p.IdPrograma == idprograma);

            if (programa == null)
                return NotFound();

            ViewBag.NomeTipo = await _context.TiposProgramaBolsa
                .Where(t => t.IdTipoPrograma == programa.IdTipoPrograma)
                .Select(t => t.NomeTipo)
                .FirstOrDefaultAsync();

            return View(programa);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int idprograma)
        {
            var programa = await _context.ProgramasBolsa
                .FindAsync(idprograma);

            if (programa == null)
                return NotFound();

            try
            {
                _context.ProgramasBolsa.Remove(programa);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    "Programa eliminado com sucesso!";
            }
            catch
            {
                TempData["ErrorMessage"] =
                    "Não foi possível eliminar o programa.";
            }

            return RedirectToAction(nameof(Index));
        }

        // Load dropdown options
        private void CarregarTiposPrograma(int? tipoSelecionado = null)
        {
            ViewData["IdTipoPrograma"] = new SelectList(
                _context.TiposProgramaBolsa.ToList(),
                "IdTipoPrograma",
                "NomeTipo",
                tipoSelecionado
            );
        }

        // Check if program exists
        private async Task<bool> ProgramaBolsaExists(int idprograma)
        {
            return await _context.ProgramasBolsa
                .AnyAsync(p => p.IdPrograma == idprograma);
        }
    }
}

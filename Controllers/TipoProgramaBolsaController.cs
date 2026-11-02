
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

namespace UnivManagement.Controllers
{
    public class TipoProgramaBolsaController : Controller
    {
        private readonly UniversityDbContext _context;

        public TipoProgramaBolsaController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: TipoProgramaBolsa
        public async Task<IActionResult> Index()
        {
            return View(await _context.TiposProgramaBolsa.ToListAsync());
        }

     

        // GET: TipoProgramaBolsa/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoProgramaBolsa/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("IdTipoPrograma,NomeTipo,Descricao")]
            TipoProgramaBolsa tipoProgramaBolsa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoProgramaBolsa);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    "Tipo de bolsa criado com sucesso!";

                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] =
                "Não foi possível criar o tipo de bolsa. Verifique os dados.";

            return View(tipoProgramaBolsa);
        }

        // GET: TipoProgramaBolsa/Edit
        public async Task<IActionResult> Edit(int? idtipoprograma)
        {
            if (idtipoprograma == null)
            {
                return NotFound();
            }

            var tipo = await _context.TiposProgramaBolsa
                .FindAsync(idtipoprograma);

            if (tipo == null)
            {
                return NotFound();
            }

            return View(tipo);
        }

        // POST: TipoProgramaBolsa/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int? idtipoprograma,
            [Bind("IdTipoPrograma,NomeTipo,Descricao")]
            TipoProgramaBolsa tipoProgramaBolsa)
        {
            if (idtipoprograma == null ||
                idtipoprograma != tipoProgramaBolsa.IdTipoPrograma)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoProgramaBolsa);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] =
                        "Tipo de bolsa atualizado com sucesso!";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await TipoProgramaBolsaExists(
                        tipoProgramaBolsa.IdTipoPrograma))
                    {
                        return NotFound();
                    }

                    throw;
                }
            }

            TempData["ErrorMessage"] =
                "Não foi possível atualizar o tipo de bolsa. Verifique os dados.";

            return View(tipoProgramaBolsa);
        }

        // GET: TipoProgramaBolsa/Delete
        public async Task<IActionResult> Delete(int? idtipoprograma)
        {
            if (idtipoprograma == null)
            {
                return NotFound();
            }

            var tipo = await _context.TiposProgramaBolsa
                .FirstOrDefaultAsync(m =>
                    m.IdTipoPrograma == idtipoprograma);

            if (tipo == null)
            {
                return NotFound();
            }

            return View(tipo);
        }

        // POST: TipoProgramaBolsa/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(
            int? idtipoprograma)
        {
            if (idtipoprograma == null)
            {
                return NotFound();
            }

            var tipo = await _context.TiposProgramaBolsa
                .FindAsync(idtipoprograma);

            if (tipo == null)
            {
                return NotFound();
            }

            _context.TiposProgramaBolsa.Remove(tipo);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Tipo de bolsa eliminado com sucesso!";

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TipoProgramaBolsaExists(int id)
        {
            return await _context.TiposProgramaBolsa
                .AnyAsync(e => e.IdTipoPrograma == id);
        }
    }
}


using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

namespace UnivManagement.Controllers
{
    public class BuildingController : Controller
    {
        private readonly UniversityDbContext _context;

        public BuildingController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: Building
        public async Task<IActionResult> Index(
            string? successMessage = null,
            string? errorMessage = null)
        {
            ViewBag.SuccessMessage = successMessage;
            ViewBag.ErrorMessage = errorMessage;

            var buildings = await _context.Building
                .ToListAsync();

            return View(buildings);
        }

        // GET: Building/Details/5
        public async Task<IActionResult> Details(
            int? id,
            string? successMessage = null)
        {
            if (id == null)
            {
                return NotFound();
            }

            var building = await _context.Building
                .FirstOrDefaultAsync(b => b.BuildingId == id);

            if (building == null)
            {
                return NotFound();
            }

            ViewBag.SuccessMessage = successMessage;

            return View(building);
        }

        // GET: Building/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Building/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("NomeEdificio,Localizacao")] Building building)
        {
            if (ModelState.IsValid)
            {
                _context.Building.Add(building);
                await _context.SaveChangesAsync();

                return RedirectToAction(
                    nameof(Details),
                    new
                    {
                        id = building.BuildingId,
                        successMessage = "Edifício criado com sucesso!"
                    });
            }

            return View(building);
        }

        // GET: Building/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var building = await _context.Building.FindAsync(id);

            if (building == null)
            {
                return NotFound();
            }

            return View(building);
        }

        // POST: Building/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int? id,
            [Bind("BuildingId,NomeEdificio,Localizacao")] Building building)
        {
            if (building.BuildingId <= 0 ||
                (id.HasValue && id.Value != building.BuildingId))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Building.Update(building);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuildingExists(building.BuildingId))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(
                    nameof(Details),
                    new
                    {
                        id = building.BuildingId,
                        successMessage = "Edifício atualizado com sucesso!"
                    });
            }

            return View(building);
        }

        // GET: Building/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var building = await _context.Building
                .FirstOrDefaultAsync(b => b.BuildingId == id);

            if (building == null)
            {
                return RedirectToAction(
                    nameof(Index),
                    new
                    {
                        errorMessage =
                            "O edifício não existe ou já foi eliminado."
                    });
            }

            return View(building);
        }

        // POST: Building/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(
            int? id,
            int? buildingId)
        {
            if (id.HasValue &&
                buildingId.HasValue &&
                id.Value != buildingId.Value)
            {
                return BadRequest();
            }

            int? edificioId = id ?? buildingId;

            if (edificioId == null)
            {
                return NotFound();
            }

            var building = await _context.Building
                .FindAsync(edificioId.Value);

            if (building == null)
            {
                return RedirectToAction(
                    nameof(Index),
                    new
                    {
                        errorMessage =
                            "O edifício não existe ou já foi eliminado."
                    });
            }

            // Impedir eliminação de edifícios com salas associadas
            bool temSalas = await _context.SalasAula
                .AnyAsync(s => s.IdEdificio == edificioId.Value);

            if (temSalas)
            {
                return RedirectToAction(
                    nameof(Index),
                    new
                    {
                        errorMessage =
                            "Não é possível eliminar este edifício porque possui salas associadas."
                    });
            }

            try
            {
                _context.Building.Remove(building);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(
                    nameof(Index),
                    new
                    {
                        errorMessage =
                            "Não foi possível eliminar o edifício. Existem dados associados ou ocorreu um erro na base de dados."
                    });
            }

            return RedirectToAction(
                nameof(Index),
                new
                {
                    successMessage = "Edifício eliminado com sucesso!"
                });
        }

        // Verificar se o edifício existe
        private bool BuildingExists(int id)
        {
            return _context.Building
                .Any(b => b.BuildingId == id);
        }
    }
}

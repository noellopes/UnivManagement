
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

namespace UnivManagement.Controllers
{
    public class RoomTypeController : Controller
    {
        private readonly UniversityDbContext _context;

        public RoomTypeController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: RoomType
        public async Task<IActionResult> Index(
            string? successMessage = null,
            string? errorMessage = null)
        {
            ViewBag.SuccessMessage = successMessage;
            ViewBag.ErrorMessage = errorMessage;

            var roomTypes = await _context.RoomType
                .ToListAsync();

            return View(roomTypes);
        }

        // GET: RoomType/Details/5
        public async Task<IActionResult> Details(
            int? id,
            string? successMessage = null)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomType = await _context.RoomType
                .FirstOrDefaultAsync(r => r.RoomTypeId == id);

            if (roomType == null)
            {
                return NotFound();
            }

            ViewBag.SuccessMessage = successMessage;

            return View(roomType);
        }

        // GET: RoomType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RoomType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Designacao,RequerComputadores")] RoomType roomType)
        {
            if (ModelState.IsValid)
            {
                _context.RoomType.Add(roomType);
                await _context.SaveChangesAsync();

                return RedirectToAction(
                    nameof(Details),
                    new
                    {
                        id = roomType.RoomTypeId,
                        successMessage = "Tipo de sala criado com sucesso!"
                    });
            }

            return View(roomType);
        }

        // GET: RoomType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomType = await _context.RoomType.FindAsync(id);

            if (roomType == null)
            {
                return NotFound();
            }

            return View(roomType);
        }

        // POST: RoomType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int? id,
            [Bind("RoomTypeId,Designacao,RequerComputadores")]
            RoomType roomType)
        {
            if (roomType.RoomTypeId <= 0 ||
                (id.HasValue && id.Value != roomType.RoomTypeId))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.RoomType.Update(roomType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomTypeExists(roomType.RoomTypeId))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(
                    nameof(Details),
                    new
                    {
                        id = roomType.RoomTypeId,
                        successMessage = "Tipo de sala atualizado com sucesso!"
                    });
            }

            return View(roomType);
        }

        // GET: RoomType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomType = await _context.RoomType
                .FirstOrDefaultAsync(r => r.RoomTypeId == id);

            if (roomType == null)
            {
                return RedirectToAction(
                    nameof(Index),
                    new
                    {
                        errorMessage =
                            "O tipo de sala não existe ou já foi eliminado."
                    });
            }

            return View(roomType);
        }

        // POST: RoomType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(
            int? id,
            int? roomTypeId)
        {
            if (id.HasValue &&
                roomTypeId.HasValue &&
                id.Value != roomTypeId.Value)
            {
                return BadRequest();
            }

            int? tipoId = id ?? roomTypeId;

            if (tipoId == null)
            {
                return NotFound();
            }

            var roomType = await _context.RoomType
                .FindAsync(tipoId.Value);

            if (roomType == null)
            {
                return RedirectToAction(
                    nameof(Index),
                    new
                    {
                        errorMessage =
                            "O tipo de sala não existe ou já foi eliminado."
                    });
            }

            // Impedir eliminação de tipos de sala com salas associadas
            bool temSalas = await _context.SalasAula
                .AnyAsync(s => s.IdTipoSala == tipoId.Value);

            if (temSalas)
            {
                return RedirectToAction(
                    nameof(Index),
                    new
                    {
                        errorMessage =
                            "Não é possível eliminar este tipo de sala porque existem salas associadas."
                    });
            }

            try
            {
                _context.RoomType.Remove(roomType);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(
                    nameof(Index),
                    new
                    {
                        errorMessage =
                            "Não foi possível eliminar o tipo de sala. Existem dados associados ou ocorreu um erro na base de dados."
                    });
            }

            return RedirectToAction(
                nameof(Index),
                new
                {
                    successMessage = "Tipo de sala eliminado com sucesso!"
                });
        }

        // Verificar se o tipo de sala existe
        private bool RoomTypeExists(int id)
        {
            return _context.RoomType
                .Any(r => r.RoomTypeId == id);
        }
    }
}

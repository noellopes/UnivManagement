
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

public class RoomController : Controller
{
    private readonly UniversityDbContext _context;

    public RoomController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: Room
    public async Task<IActionResult> Index(
        string? successMessage = null,
        string? errorMessage = null)
    {
        ViewBag.SuccessMessage = successMessage;
        ViewBag.ErrorMessage = errorMessage;

        var rooms = await _context.SalasAula
            .Include(r => r.Edificio)
            .Include(r => r.TipoSala)
            .ToListAsync();

        return View(rooms);
    }

    // GET: Room/Details/5
    public async Task<IActionResult> Details(
        int? id,
        int? roomid,
        string? successMessage = null)
    {
        int? salaId = id ?? roomid;

        if (salaId == null)
            return NotFound();

        var room = await _context.SalasAula
            .Include(r => r.Edificio)
            .Include(r => r.TipoSala)
            .FirstOrDefaultAsync(r => r.RoomId == salaId.Value);

        if (room == null)
            return NotFound();

        ViewBag.SuccessMessage = successMessage;

        return View(room);
    }

    // GET: Room/Create
    public IActionResult Create()
    {
        CarregarDropdowns();
        return View();
    }

    // POST: Room/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("RoomId,NumeroSala,Piso,Lotacao,Estado,IdEdificio,IdTipoSala")]
        Room room)
    {
        await ValidarSalaAsync(room);

        if (ModelState.IsValid)
        {
            _context.SalasAula.Add(room);
            await _context.SaveChangesAsync();

            return RedirectToAction(
                nameof(Details),
                new
                {
                    id = room.RoomId,
                    successMessage = "Sala criada com sucesso!"
                });
        }

        CarregarDropdowns(room);
        return View(room);
    }

    // GET: Room/Edit/5
    public async Task<IActionResult> Edit(int? id, int? roomid)
    {
        int? salaId = id ?? roomid;

        if (salaId == null)
            return NotFound();

        var room = await _context.SalasAula.FindAsync(salaId.Value);

        if (room == null)
            return NotFound();

        CarregarDropdowns(room);

        return View(room);
    }

    // POST: Room/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int? id,
        [Bind("RoomId,NumeroSala,Piso,Lotacao,Estado,IdEdificio,IdTipoSala")]
        Room room)
    {
        if (room.RoomId <= 0 ||
            (id.HasValue && id.Value != room.RoomId))
        {
            return NotFound();
        }

        await ValidarSalaAsync(room);

        if (ModelState.IsValid)
        {
            try
            {
                _context.SalasAula.Update(room);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(room.RoomId))
                    return NotFound();

                throw;
            }

            return RedirectToAction(
                nameof(Details),
                new
                {
                    id = room.RoomId,
                    successMessage = "Sala atualizada com sucesso!"
                });
        }

        CarregarDropdowns(room);
        return View(room);
    }

    // GET: Room/Delete/5
    public async Task<IActionResult> Delete(int? id, int? roomid)
    {
        int? salaId = id ?? roomid;

        if (salaId == null)
            return NotFound();

        var room = await _context.SalasAula
            .Include(r => r.Edificio)
            .Include(r => r.TipoSala)
            .FirstOrDefaultAsync(r => r.RoomId == salaId.Value);

        if (room == null)
        {
            return RedirectToAction(
                nameof(Index),
                new
                {
                    errorMessage = "A sala não existe ou já foi eliminada."
                });
        }

        return View(room);
    }

    // POST: Room/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(
        int? id,
        int? roomId)
    {
        if (id.HasValue &&
            roomId.HasValue &&
            id.Value != roomId.Value)
        {
            return BadRequest();
        }

        int? salaId = id ?? roomId;

        if (salaId == null)
            return NotFound();

        var room = await _context.SalasAula.FindAsync(salaId.Value);

        if (room == null)
        {
            return RedirectToAction(
                nameof(Index),
                new
                {
                    errorMessage = "A sala não existe ou já foi eliminada."
                });
        }

        // Não eliminar salas que tenham reservas
        bool temReservas = await _context.Reserva
            .AnyAsync(r => r.SalaFk == salaId.Value);

        if (temReservas)
        {
            return RedirectToAction(
                nameof(Index),
                new
                {
                    errorMessage =
                        "Não é possível eliminar esta sala porque tem reservas associadas."
                });
        }

        // Não eliminar salas que tenham aulas
        bool temAulas = await _context.Set<Session>()
            .AnyAsync(s => s.IdSala == salaId.Value);

        if (temAulas)
        {
            return RedirectToAction(
                nameof(Index),
                new
                {
                    errorMessage =
                        "Não é possível eliminar esta sala porque tem aulas associadas."
                });
        }

        try
        {
            _context.SalasAula.Remove(room);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return RedirectToAction(
                nameof(Index),
                new
                {
                    errorMessage =
                        "Não foi possível eliminar a sala. Verifique se existem outros dados associados."
                });
        }

        return RedirectToAction(
            nameof(Index),
            new
            {
                successMessage = "Sala eliminada com sucesso!"
            });
    }

    // Carregar edifícios e tipos de sala
    private void CarregarDropdowns(Room? room = null)
    {
        ViewBag.IdEdificio = new SelectList(
            _context.Building
                .OrderBy(b => b.NomeEdificio)
                .ToList(),
            "BuildingId",
            "NomeEdificio",
            room?.IdEdificio
        );

        ViewBag.IdTipoSala = new SelectList(
            _context.RoomType
                .OrderBy(t => t.Designacao)
                .ToList(),
            "RoomTypeId",
            "Designacao",
            room?.IdTipoSala
        );
    }

    // Validar dados da sala
    private async Task ValidarSalaAsync(Room room)
    {
        bool edificioExiste = await _context.Building
            .AnyAsync(b => b.BuildingId == room.IdEdificio);

        if (!edificioExiste)
        {
            ModelState.AddModelError(
                nameof(Room.IdEdificio),
                "Selecione um edifício válido.");
        }

        bool tipoExiste = await _context.RoomType
            .AnyAsync(t => t.RoomTypeId == room.IdTipoSala);

        if (!tipoExiste)
        {
            ModelState.AddModelError(
                nameof(Room.IdTipoSala),
                "Selecione um tipo de sala válido.");
        }

        if (!string.IsNullOrWhiteSpace(room.NumeroSala))
        {
            bool duplicada = await _context.SalasAula
                .AnyAsync(r =>
                    r.RoomId != room.RoomId &&
                    r.NumeroSala == room.NumeroSala.Trim() &&
                    r.IdEdificio == room.IdEdificio &&
                    r.Piso == room.Piso);

            if (duplicada)
            {
                ModelState.AddModelError(
                    nameof(Room.NumeroSala),
                    "Já existe uma sala com este número no mesmo edifício e piso.");
            }
        }
    }

    // Verificar se a sala existe
    private bool RoomExists(int roomId)
    {
        return _context.SalasAula
            .Any(r => r.RoomId == roomId);
    }
}

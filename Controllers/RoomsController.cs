using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models.AccommodationManagement;
using UnivManagement.Data;

namespace UnivManagement.Controllers;

public class RoomsController : Controller
{
    private readonly UniversityDbContext _context;

    public RoomsController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: Rooms
    public async Task<IActionResult> Index(int? residenceFilter)
    {
        var allRooms = await _context.Room
            .Include(r => r.RoomType)
            .Include(r => r.RoomStatus)
            .OrderBy(r => r.ResidenceId)
            .ThenBy(r => r.Floor)
            .ThenBy(r => r.Number)
            .ToListAsync();

        var residences = allRooms
            .Where(r => r.ResidenceId > 0)
            .Select(r => r.ResidenceId)
            .Distinct()
            .OrderBy(id => id)
            .Select(id => new { Id = id, Name = $"Residência #{id}" })
            .ToList();

        ViewBag.Residences = new SelectList(residences, "Id", "Name", residenceFilter);

        var filteredRooms = residenceFilter.HasValue
            ? allRooms.Where(r => r.ResidenceId == residenceFilter.Value).ToList()
            : allRooms;

        ViewBag.TotalRooms = filteredRooms.Count;
        ViewBag.FreeRooms = filteredRooms.Count(r =>
            r.RoomStatus?.Name is "Available" or "Livre");
        ViewBag.OccupiedRooms = filteredRooms.Count(r =>
            r.CurrentOccupancy > 0 || r.RoomStatus?.Name is "Occupied" or "Ocupado");
        ViewBag.MaintenanceRooms = filteredRooms.Count(r =>
            r.RoomStatus?.Name is "Maintenance" or "Em Manutenção");

        return View(filteredRooms);
    }

    // GET: Rooms/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var room = await _context.Room
            .Include(r => r.RoomType)
            .Include(r => r.RoomStatus)
            .FirstOrDefaultAsync(m => m.RoomId == id);
        if (room == null)
        {
            return NotFound();
        }

        return View(room);
    }

    // GET: Rooms/Create
    public async Task<IActionResult> Create()
    {
        await PopulateDropdownsAsync();
        return View(new Room1
        {
            Number = string.Empty,
            Floor = 1,
            CurrentOccupancy = 0,
            Capacity = 1
        });
    }

    // POST: Rooms/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("RoomId,ResidenceId,Number,Floor,CurrentOccupancy,Capacity,RoomTypeId,RoomStatusId")] Room1 room)
    {
        ValidateOccupancy(room);

        if (ModelState.IsValid)
        {
            _context.Add(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        await PopulateDropdownsAsync(room);
        return View(room);
    }

    // GET: Rooms/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var room = await _context.Room.FindAsync(id);
        if (room == null)
        {
            return NotFound();
        }

        await PopulateDropdownsAsync(room);
        return View(room);
    }

    // POST: Rooms/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("RoomId,ResidenceId,Number,Floor,CurrentOccupancy,Capacity,RoomTypeId,RoomStatusId")] Room1 room)
    {
        if (id != room.RoomId)
        {
            return NotFound();
        }

        ValidateOccupancy(room);

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(room);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(room.RoomId))
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

        await PopulateDropdownsAsync(room);
        return View(room);
    }

    // GET: Rooms/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var room = await _context.Room
            .Include(r => r.RoomType)
            .Include(r => r.RoomStatus)
            .FirstOrDefaultAsync(m => m.RoomId == id);
        if (room == null)
        {
            return NotFound();
        }

        return View(room);
    }

    // POST: Rooms/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var room = await _context.Room.FindAsync(id);
        if (room != null)
        {
            _context.Room.Remove(room);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool RoomExists(int id)
    {
        return _context.Room.Any(e => e.RoomId == id);
    }

    private void ValidateOccupancy(Room1 room)
    {
        if (room.CurrentOccupancy < 0)
        {
            ModelState.AddModelError(nameof(room.CurrentOccupancy), "A ocupação atual não pode ser negativa.");
        }

        if (room.CurrentOccupancy > room.Capacity)
        {
            ModelState.AddModelError(nameof(room.CurrentOccupancy), "A ocupação atual não pode ser superior à capacidade.");
        }
    }

    private async Task PopulateDropdownsAsync(Room1? room = null)
    {
        ViewData["RoomTypeId"] = new SelectList(
            await _context.RoomType.OrderBy(t => t.RoomTypeId).ToListAsync(),
            "RoomTypeId",
            "Name",
            room?.RoomTypeId);

        ViewData["RoomStatusId"] = new SelectList(
            await _context.RoomStatus.OrderBy(s => s.RoomStatusId).ToListAsync(),
            "RoomStatusId",
            "Name",
            room?.RoomStatusId);
    }
}
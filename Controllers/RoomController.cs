
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class RoomController : Controller
{
    private readonly UniversityDbContext _context;

    public RoomController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: ROOMS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Room.ToListAsync());
    }

    // GET: ROOMS/Details/5
    [HttpGet("Room/Details/{RoomId}")]

    public async Task<IActionResult> Details(int? roomId)
    {
        if (roomId == null)
        {
            return NotFound();
        }

        var room = await _context.Room
            .FirstOrDefaultAsync(m => m.RoomId == roomId);
        if (room == null)
        {
            return NotFound();
        }

        return View(room);
    }

    // GET: ROOMS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ROOMS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("RoomId,NumeroSala,Piso,Lotacao,Estado,IdEdificio,Edificio,IdTipoSala,TipoSala")] Room room)
    {
        if (ModelState.IsValid)
        {
            _context.Add(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(room);
    }

    // GET: ROOMS/Edit/5
    [HttpGet("Room/Edit/{RoomId}")]
    public async Task<IActionResult> Edit(int? roomId)
    {
        if (roomId == null)
        {
            return NotFound();
        }

        var room = await _context.Room.FindAsync(roomId);
        if (room == null)
        {
            return NotFound();
        }
        return View(room);
    }

    // POST: ROOMS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? roomId, [Bind("RoomId,NumeroSala,Piso,Lotacao,Estado,IdEdificio,Edificio,IdTipoSala,TipoSala")] Room room)
    {
        if (roomId != room.RoomId)      
        {
            return NotFound();
        }

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
        return View(room);
    }

    // GET: ROOMS/Delete/5
    [HttpGet("Room/Delete/{RoomId}")]
    public async Task<IActionResult> Delete(int? roomId)        
    {
        if (roomId == null)
        {
            return NotFound();
        }

        var room = await _context.Room
            .FirstOrDefaultAsync(m => m.RoomId == roomId);
        if (room == null)
        {
            return NotFound();
        }

        return View(room);
    }

    // POST: ROOMS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? roomId)
    {
        var room = await _context.Room.FindAsync(roomId);
        if (room != null)
        {
            _context.Room.Remove(room);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool RoomExists(int? roomId)
    {
        return _context.Room.Any(e => e.RoomId == roomId);
    }
}

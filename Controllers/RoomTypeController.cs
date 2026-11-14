
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class RoomTypeController : Controller
{
    private readonly UniversityDbContext _context;

    public RoomTypeController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: ROOMTYPES
    public async Task<IActionResult> Index()    
    {
        return View(await _context.RoomType.ToListAsync());
    }

    // GET: ROOMTYPES/Details/5
    [HttpGet("RoomType/Details/{roomtypeid}")]
    public async Task<IActionResult> Details(int? roomtypeid)
    {
        if (roomtypeid == null)
        {
            return NotFound();
        }

        var roomtype = await _context.RoomType
            .FirstOrDefaultAsync(m => m.RoomTypeId == roomtypeid);
        if (roomtype == null)
        {
            return NotFound();
        }

        return View(roomtype);
    }

    // GET: ROOMTYPES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ROOMTYPES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("RoomTypeId,Designacao,RequerComputadores")] RoomType roomtype)
    {
        if (ModelState.IsValid)
        {
            _context.Add(roomtype);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(roomtype);
    }

    // GET: ROOMTYPES/Edit/5
    [HttpGet("RoomType/Edit/{roomtypeid}")]
    public async Task<IActionResult> Edit(int? roomtypeid)
    {
        if (roomtypeid == null)
        {
            return NotFound();
        }

        var roomtype = await _context.RoomType.FindAsync(roomtypeid);
        if (roomtype == null)
        {
            return NotFound();
        }
        return View(roomtype);
    }

    // POST: ROOMTYPES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? roomtypeid, [Bind("RoomTypeId,Designacao,RequerComputadores")] RoomType roomtype)
    {
        if (roomtypeid != roomtype.RoomTypeId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(roomtype);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomTypeExists(roomtype.RoomTypeId))
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
        return View(roomtype);
    }

    // GET: ROOMTYPES/Delete/5
    [HttpGet("RoomType/Delete/{roomtypeid}")]
    public async Task<IActionResult> Delete(int? roomtypeid)
    {
        if (roomtypeid == null)
        {
            return NotFound();
        }

        var roomtype = await _context.RoomType
            .FirstOrDefaultAsync(m => m.RoomTypeId == roomtypeid);
        if (roomtype == null)
        {
            return NotFound();
        }

        return View(roomtype);
    }

    // POST: ROOMTYPES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? roomtypeid)
    {
        var roomtype = await _context.RoomType.FindAsync(roomtypeid);
        if (roomtype != null)
        {
            _context.RoomType.Remove(roomtype);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool RoomTypeExists(int? roomtypeid)
    {
        return _context.RoomType.Any(e => e.RoomTypeId == roomtypeid);
    }
}

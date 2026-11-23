
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class RoomAllocationsController : Controller
{
    private readonly UniversityDbContext _context;

    public RoomAllocationsController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: ROOMALLOCATIONS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.RoomAllocation.ToListAsync());
    }

    // GET: ROOMALLOCATIONS/Details/5
    public async Task<IActionResult> Details(int? roomallocationid)
    {
        if (roomallocationid == null)
        {
            return NotFound();
        }

        var roomallocation = await _context.RoomAllocation
            .FirstOrDefaultAsync(m => m.RoomAllocationId == roomallocationid);
        if (roomallocation == null)
        {
            return NotFound();
        }

        return View(roomallocation);
    }

    // GET: ROOMALLOCATIONS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ROOMALLOCATIONS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("RoomAllocationId,AccommodationApplicationId,StudentId,RoomId,StartDate,EndDate,Status")] RoomAllocation roomallocation)
    {
        if (ModelState.IsValid)
        {
            _context.Add(roomallocation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(roomallocation);
    }

    // GET: ROOMALLOCATIONS/Edit/5
    public async Task<IActionResult> Edit(int? roomallocationid)
    {
        if (roomallocationid == null)
        {
            return NotFound();
        }

        var roomallocation = await _context.RoomAllocation.FindAsync(roomallocationid);
        if (roomallocation == null)
        {
            return NotFound();
        }
        return View(roomallocation);
    }

    // POST: ROOMALLOCATIONS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? roomallocationid, [Bind("RoomAllocationId,AccommodationApplicationId,StudentId,RoomId,StartDate,EndDate,Status")] RoomAllocation roomallocation)
    {
        if (roomallocationid != roomallocation.RoomAllocationId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(roomallocation);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomAllocationExists(roomallocation.RoomAllocationId))
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
        return View(roomallocation);
    }

    // GET: ROOMALLOCATIONS/Delete/5
    public async Task<IActionResult> Delete(int? roomallocationid)
    {
        if (roomallocationid == null)
        {
            return NotFound();
        }

        var roomallocation = await _context.RoomAllocation
            .FirstOrDefaultAsync(m => m.RoomAllocationId == roomallocationid);
        if (roomallocation == null)
        {
            return NotFound();
        }

        return View(roomallocation);
    }

    // POST: ROOMALLOCATIONS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? roomallocationid)
    {
        var roomallocation = await _context.RoomAllocation.FindAsync(roomallocationid);
        if (roomallocation != null)
        {
            _context.RoomAllocation.Remove(roomallocation);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool RoomAllocationExists(int? roomallocationid)
    {
        return _context.RoomAllocation.Any(e => e.RoomAllocationId == roomallocationid);
    }
}

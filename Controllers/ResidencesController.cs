using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models.AccommodationManagement;
using UnivManagement.Data;

namespace UnivManagement.Controllers;

public class ResidencesController : Controller
{
    private readonly UniversityDbContext _context;

    public ResidencesController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: RESIDENCES
    public async Task<IActionResult> Index()
    {
        return View(await _context.Residence.ToListAsync());
    }

    // GET: RESIDENCES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var residence = await _context.Residence
            .FirstOrDefaultAsync(m => m.ResidenceId == id);
        if (residence == null)
        {
            return NotFound();
        }

        return View(residence);
    }

    // GET: RESIDENCES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: RESIDENCES/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ResidenceId,Name,Address,IsActive")] Residence residence)
    {
        if (ModelState.IsValid)
        {
            _context.Add(residence);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(residence);
    }

    // GET: RESIDENCES/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var residence = await _context.Residence.FindAsync(id);
        if (residence == null)
        {
            return NotFound();
        }
        return View(residence);
    }

    // POST: RESIDENCES/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("ResidenceId,Name,Address,IsActive")] Residence residence)
    {
        if (id != residence.ResidenceId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(residence);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResidenceExists(residence.ResidenceId))
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
        return View(residence);
    }

    // GET: RESIDENCES/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var residence = await _context.Residence
            .FirstOrDefaultAsync(m => m.ResidenceId == id);
        if (residence == null)
        {
            return NotFound();
        }

        return View(residence);
    }

    // POST: RESIDENCES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var residence = await _context.Residence.FindAsync(id);
        if (residence != null)
        {
            _context.Residence.Remove(residence);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ResidenceExists(int id)
    {
        return _context.Residence.Any(e => e.ResidenceId == id);
    }
}
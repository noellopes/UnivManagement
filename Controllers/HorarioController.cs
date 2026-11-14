
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class HorarioController : Controller
{
    private readonly UniversityDbContext _context;

    public HorarioController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: HORARIOS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Horario.ToListAsync());
    }

    // GET: HORARIOS/Details/5
    [HttpGet("Horario/Details/{HorarioId}")]
    public async Task<IActionResult> Details(int? horarioid)
    {
        if (horarioid == null)
        {
            return NotFound();
        }

        var horario = await _context.Horario
            .FirstOrDefaultAsync(m => m.HorarioId == horarioid);
        if (horario == null)
        {
            return NotFound();
        }

        return View(horario);
    }

    // GET: HORARIOS/Create

    public IActionResult Create()
    {
        return View();
    }

    // POST: HORARIOS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("HorarioId,AnoLetivo")] Horario horario)
    {
        if (ModelState.IsValid)
        {
            _context.Add(horario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(horario);
    }

    // GET: HORARIOS/Edit/5
    [HttpGet("Horario/Edit/{HorarioId}")]
    public async Task<IActionResult> Edit(int? horarioid)
    {
        if (horarioid == null)
        {
            return NotFound();
        }

        var horario = await _context.Horario.FindAsync(horarioid);
        if (horario == null)
        {
            return NotFound();
        }
        return View(horario);
    }

    // POST: HORARIOS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    
    public async Task<IActionResult> Edit(int? horarioid, [Bind("HorarioId,AnoLetivo")] Horario horario)
    {
        if (horarioid != horario.HorarioId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(horario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HorarioExists(horario.HorarioId))
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
        return View(horario);
    }

    // GET: HORARIOS/Delete/5
    [HttpGet("Horario/Delete/{HorarioId}")]
    public async Task<IActionResult> Delete(int? horarioid)
    {
        if (horarioid == null)
        {
            return NotFound();
        }

        var horario = await _context.Horario
            .FirstOrDefaultAsync(m => m.HorarioId == horarioid);
        if (horario == null)
        {
            return NotFound();
        }

        return View(horario);
    }

    // POST: HORARIOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? horarioid)
    {
        var horario = await _context.Horario.FindAsync(horarioid);
        if (horario != null)
        {
            _context.Horario.Remove(horario);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool HorarioExists(int? horarioid)
    {
        return _context.Horario.Any(e => e.HorarioId == horarioid);
    }
}


using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class NotasController : Controller
{
    private readonly UniversityDbContext _context;

    public NotasController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: NOTASS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Notas.ToListAsync());
    }

    // GET: NOTASS/Details/5
    public async Task<IActionResult> Details(int? notaid)
    {
        if (notaid == null)
        {
            return NotFound();
        }

        var notas = await _context.Notas
            .FirstOrDefaultAsync(m => m.NotaID == notaid);
        if (notas == null)
        {
            return NotFound();
        }

        return View(notas);
    }

    // GET: NOTASS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: NOTASS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("NotaID,AvaliacaoID,AvaliacaoUC,InscrUcID,InscrUC,Data,Nota,Aprovado")] Notas notas)
    {
        if (ModelState.IsValid)
        {
            _context.Add(notas);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(notas);
    }

    // GET: NOTASS/Edit/5
    public async Task<IActionResult> Edit(int? notaid)
    {
        if (notaid == null)
        {
            return NotFound();
        }

        var notas = await _context.Notas.FindAsync(notaid);
        if (notas == null)
        {
            return NotFound();
        }
        return View(notas);
    }

    // POST: NOTASS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? notaid, [Bind("NotaID,AvaliacaoID,AvaliacaoUC,InscrUcID,InscrUC,Data,Nota,Aprovado")] Notas notas)
    {
        if (notaid != notas.NotaID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(notas);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotasExists(notas.NotaID))
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
        return View(notas);
    }

    // GET: NOTASS/Delete/5
    public async Task<IActionResult> Delete(int? notaid)
    {
        if (notaid == null)
        {
            return NotFound();
        }

        var notas = await _context.Notas
            .FirstOrDefaultAsync(m => m.NotaID == notaid);
        if (notas == null)
        {
            return NotFound();
        }

        return View(notas);
    }

    // POST: NOTASS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? notaid)
    {
        var notas = await _context.Notas.FindAsync(notaid);
        if (notas != null)
        {
            _context.Notas.Remove(notas);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool NotasExists(int? notaid)
    {
        return _context.Notas.Any(e => e.NotaID == notaid);
    }
}


using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class CandidaturasController : Controller
{
    private readonly UniversityDbContext _context;

    public CandidaturasController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: CANDIDATURAS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Candidaturas.ToListAsync());
    }

    // GET: CANDIDATURAS/Details/5
    public async Task<IActionResult> Details(int? idcandidatura)
    {
        if (idcandidatura == null)
        {
            return NotFound();
        }

        var candidatura = await _context.Candidaturas
            .FirstOrDefaultAsync(m => m.IdCandidatura == idcandidatura);
        if (candidatura == null)
        {
            return NotFound();
        }

        return View(candidatura);
    }

    // GET: CANDIDATURAS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: CANDIDATURAS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdCandidatura,IdAluno,Estudante,IdOferta,Oferta,DataCandidatura,Estado,Curriculo")] Candidatura candidatura)
    {
        if (ModelState.IsValid)
        {
            _context.Add(candidatura);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(candidatura);
    }

    // GET: CANDIDATURAS/Edit/5
    public async Task<IActionResult> Edit(int? idcandidatura)
    {
        if (idcandidatura == null)
        {
            return NotFound();
        }

        var candidatura = await _context.Candidaturas.FindAsync(idcandidatura);
        if (candidatura == null)
        {
            return NotFound();
        }
        return View(candidatura);
    }

    // POST: CANDIDATURAS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? idcandidatura, [Bind("IdCandidatura,IdAluno,Estudante,IdOferta,Oferta,DataCandidatura,Estado,Curriculo")] Candidatura candidatura)
    {
        if (idcandidatura != candidatura.IdCandidatura)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(candidatura);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CandidaturaExists(candidatura.IdCandidatura))
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
        return View(candidatura);
    }

    // GET: CANDIDATURAS/Delete/5
    public async Task<IActionResult> Delete(int? idcandidatura)
    {
        if (idcandidatura == null)
        {
            return NotFound();
        }

        var candidatura = await _context.Candidaturas
            .FirstOrDefaultAsync(m => m.IdCandidatura == idcandidatura);
        if (candidatura == null)
        {
            return NotFound();
        }

        return View(candidatura);
    }

    // POST: CANDIDATURAS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? idcandidatura)
    {
        var candidatura = await _context.Candidaturas.FindAsync(idcandidatura);
        if (candidatura != null)
        {
            _context.Candidaturas.Remove(candidatura);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CandidaturaExists(int? idcandidatura)
    {
        return _context.Candidaturas.Any(e => e.IdCandidatura == idcandidatura);
    }
}

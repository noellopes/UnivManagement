
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class CarreirasController : Controller
{
    private readonly UniversityDbContext _context;

    public CarreirasController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: CARREIRAS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Carreiras.ToListAsync());
    }

    // GET: CARREIRAS/Details/5
    public async Task<IActionResult> Details(int? idcarreira)
    {
        if (idcarreira == null)
        {
            return NotFound();
        }

        var carreira = await _context.Carreiras
            .FirstOrDefaultAsync(m => m.IdCarreira == idcarreira);
        if (carreira == null)
        {
            return NotFound();
        }

        return View(carreira);
    }

    // GET: CARREIRAS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: CARREIRAS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdCarreira,IdAluno,Estudante,CargoAtual,EmpresaAtual,Competencias,Certificacoes,HistoricoProfissional")] Carreira carreira)
    {
        if (ModelState.IsValid)
        {
            _context.Add(carreira);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(carreira);
    }

    // GET: CARREIRAS/Edit/5
    public async Task<IActionResult> Edit(int? idcarreira)
    {
        if (idcarreira == null)
        {
            return NotFound();
        }

        var carreira = await _context.Carreiras.FindAsync(idcarreira);
        if (carreira == null)
        {
            return NotFound();
        }
        return View(carreira);
    }

    // POST: CARREIRAS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? idcarreira, [Bind("IdCarreira,IdAluno,Estudante,CargoAtual,EmpresaAtual,Competencias,Certificacoes,HistoricoProfissional")] Carreira carreira)
    {
        if (idcarreira != carreira.IdCarreira)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(carreira);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarreiraExists(carreira.IdCarreira))
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
        return View(carreira);
    }

    // GET: CARREIRAS/Delete/5
    public async Task<IActionResult> Delete(int? idcarreira)
    {
        if (idcarreira == null)
        {
            return NotFound();
        }

        var carreira = await _context.Carreiras
            .FirstOrDefaultAsync(m => m.IdCarreira == idcarreira);
        if (carreira == null)
        {
            return NotFound();
        }

        return View(carreira);
    }

    // POST: CARREIRAS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? idcarreira)
    {
        var carreira = await _context.Carreiras.FindAsync(idcarreira);
        if (carreira != null)
        {
            _context.Carreiras.Remove(carreira);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CarreiraExists(int? idcarreira)
    {
        return _context.Carreiras.Any(e => e.IdCarreira == idcarreira);
    }
}

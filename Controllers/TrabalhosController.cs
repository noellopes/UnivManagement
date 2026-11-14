
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class TrabalhosController : Controller
{
    private readonly UniversityDbContext _context;

    public TrabalhosController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: TRABALHOS
    public async Task<IActionResult> Index()    
    {
        System.Diagnostics.Debug.WriteLine("Base de dados de Trabalhos: " + _context.Database.GetDbConnection().Database);
        return View(await _context.Trabalhos.ToListAsync());
    }

    // GET: TRABALHOS/Details/5
    public async Task<IActionResult> Details(int? trabalhoid)
    {
        if (trabalhoid == null)
        {
            return NotFound();
        }

        var trabalho = await _context.Trabalhos
            .FirstOrDefaultAsync(m => m.TrabalhoID == trabalhoid);
        if (trabalho == null)
        {
            return NotFound();
        }

        return View(trabalho);
    }

    // GET: TRABALHOS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: TRABALHOS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TrabalhoID,DocenteId,Docente,UcId,UC,E_individual,Titulo,Descricao,Data_Limite")] Trabalho trabalho)
    {
        if (ModelState.IsValid)
        {
            _context.Add(trabalho);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(trabalho);
    }

    // GET: TRABALHOS/Edit/5
    public async Task<IActionResult> Edit(int? trabalhoid)
    {
        if (trabalhoid == null)
        {
            return NotFound();
        }

        var trabalho = await _context.Trabalhos.FindAsync(trabalhoid);
        if (trabalho == null)
        {
            return NotFound();
        }
        return View(trabalho);
    }

    // POST: TRABALHOS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? trabalhoid, [Bind("TrabalhoID,DocenteId,Docente,UcId,UC,E_individual,Titulo,Descricao,Data_Limite")] Trabalho trabalho)
    {
        if (trabalhoid != trabalho.TrabalhoID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(trabalho);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrabalhoExists(trabalho.TrabalhoID))
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
        return View(trabalho);
    }

    // GET: TRABALHOS/Delete/5
    public async Task<IActionResult> Delete(int? trabalhoid)
    {
        if (trabalhoid == null)
        {
            return NotFound();
        }

        var trabalho = await _context.Trabalhos
            .FirstOrDefaultAsync(m => m.TrabalhoID == trabalhoid);
        if (trabalho == null)
        {
            return NotFound();
        }

        return View(trabalho);
    }

    // POST: TRABALHOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? trabalhoid)
    {
        var trabalho = await _context.Trabalhos.FindAsync(trabalhoid);
        if (trabalho != null)
        {
            _context.Trabalhos.Remove(trabalho);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TrabalhoExists(int? trabalhoid)
    {
        return _context.Trabalhos.Any(e => e.TrabalhoID == trabalhoid);
    }
}

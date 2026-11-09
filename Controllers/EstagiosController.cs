
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class EstagiosController : Controller
{
    private readonly UniversityDbContext _context;

    public EstagiosController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: ESTAGIOS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Estagios.ToListAsync());
    }

    // GET: ESTAGIOS/Details/5
    public async Task<IActionResult> Details(int? idestagio)
    {
        if (idestagio == null)
        {
            return NotFound();
        }

        var estagio = await _context.Estagios
            .FirstOrDefaultAsync(m => m.IdEstagio == idestagio);
        if (estagio == null)
        {
            return NotFound();
        }

        return View(estagio);
    }

    // GET: ESTAGIOS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ESTAGIOS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdEstagio,DataInicio,DataFim,Duracao,Estado,PlanoTrabalhos,IdAluno,AntigoAluno,IdEmpregador,Empregador,IdGestor,GestorEstagio,IdOrientador,OrientadorEstagio,Avaliacoes,Relatorios")] Estagio estagio)
    {
        if (ModelState.IsValid)
        {
            _context.Add(estagio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(estagio);
    }

    // GET: ESTAGIOS/Edit/5
    public async Task<IActionResult> Edit(int? idestagio)
    {
        if (idestagio == null)
        {
            return NotFound();
        }

        var estagio = await _context.Estagios.FindAsync(idestagio);
        if (estagio == null)
        {
            return NotFound();
        }
        return View(estagio);
    }

    // POST: ESTAGIOS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? idestagio, [Bind("IdEstagio,DataInicio,DataFim,Duracao,Estado,PlanoTrabalhos,IdAluno,AntigoAluno,IdEmpregador,Empregador,IdGestor,GestorEstagio,IdOrientador,OrientadorEstagio,Avaliacoes,Relatorios")] Estagio estagio)
    {
        if (idestagio != estagio.IdEstagio)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(estagio);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstagioExists(estagio.IdEstagio))
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
        return View(estagio);
    }

    // GET: ESTAGIOS/Delete/5
    public async Task<IActionResult> Delete(int? idestagio)
    {
        if (idestagio == null)
        {
            return NotFound();
        }

        var estagio = await _context.Estagios
            .FirstOrDefaultAsync(m => m.IdEstagio == idestagio);
        if (estagio == null)
        {
            return NotFound();
        }

        return View(estagio);
    }

    // POST: ESTAGIOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? idestagio)
    {
        var estagio = await _context.Estagios.FindAsync(idestagio);
        if (estagio != null)
        {
            _context.Estagios.Remove(estagio);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool EstagioExists(int? idestagio)
    {
        return _context.Estagios.Any(e => e.IdEstagio == idestagio);
    }
}

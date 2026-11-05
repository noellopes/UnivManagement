
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class EmprestimosController : Controller
{
    private readonly UniversityDbContext _context;

    public EmprestimosController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: EMPRESTIMOS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Emprestimos.ToListAsync());
    }

    // GET: EMPRESTIMOS/Details/5
    public async Task<IActionResult> Details(int? emprestimoid)
    {
        if (emprestimoid == null)
        {
            return NotFound();
        }

        var emprestimo = await _context.Emprestimos
            .FirstOrDefaultAsync(m => m.EmprestimoId == emprestimoid);
        if (emprestimo == null)
        {
            return NotFound();
        }

        return View(emprestimo);
    }

    // GET: EMPRESTIMOS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: EMPRESTIMOS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("EmprestimoId,dataEmprestimo,dataDevolucao,estado,ExemplarId,Exemplar")] Emprestimo emprestimo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(emprestimo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(emprestimo);
    }

    // GET: EMPRESTIMOS/Edit/5
    public async Task<IActionResult> Edit(int? emprestimoid)
    {
        if (emprestimoid == null)
        {
            return NotFound();
        }

        var emprestimo = await _context.Emprestimos.FindAsync(emprestimoid);
        if (emprestimo == null)
        {
            return NotFound();
        }
        return View(emprestimo);
    }

    // POST: EMPRESTIMOS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? emprestimoid, [Bind("EmprestimoId,dataEmprestimo,dataDevolucao,estado,ExemplarId,Exemplar")] Emprestimo emprestimo)
    {
        if (emprestimoid != emprestimo.EmprestimoId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(emprestimo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmprestimoExists(emprestimo.EmprestimoId))
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
        return View(emprestimo);
    }

    // GET: EMPRESTIMOS/Delete/5
    public async Task<IActionResult> Delete(int? emprestimoid)
    {
        if (emprestimoid == null)
        {
            return NotFound();
        }

        var emprestimo = await _context.Emprestimos
            .FirstOrDefaultAsync(m => m.EmprestimoId == emprestimoid);
        if (emprestimo == null)
        {
            return NotFound();
        }

        return View(emprestimo);
    }

    // POST: EMPRESTIMOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? emprestimoid)
    {
        var emprestimo = await _context.Emprestimos.FindAsync(emprestimoid);
        if (emprestimo != null)
        {
            _context.Emprestimos.Remove(emprestimo);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool EmprestimoExists(int? emprestimoid)
    {
        return _context.Emprestimos.Any(e => e.EmprestimoId == emprestimoid);
    }
}

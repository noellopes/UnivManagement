using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

    private void PopularExemplares(int? selectedId = null, bool apenasDisponiveis = false)
    {
        var query = _context.Exemplares.AsQueryable();

        if (apenasDisponiveis)
        {
            query = query.Where(e => e.Estado == "Disponível");
        }

        ViewData["ExemplarId"] = new SelectList(query, "ExemplarId", "ExemplarId", selectedId);
    }

    // GET: EMPRESTIMOS
    public async Task<IActionResult> Index()
    {
        var universityDbContext = _context.Emprestimos.Include(e => e.Exemplar);
        return View(await universityDbContext.ToListAsync());
    }

    // GET: EMPRESTIMOS/Details/5
    public async Task<IActionResult> Details(int? emprestimoid)
    {
        if (emprestimoid == null) return NotFound();

        var emprestimo = await _context.Emprestimos
            .Include(e => e.Exemplar)
            .FirstOrDefaultAsync(m => m.EmprestimoId == emprestimoid);

        return emprestimo == null ? NotFound() : View(emprestimo);
    }

    // GET: EMPRESTIMOS/Create
    public IActionResult Create()
    {
        PopularExemplares(apenasDisponiveis: true);
        return View();
    }

    // POST: EMPRESTIMOS/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("EmprestimoId,dataEmprestimo,dataDevolucao,estado,ExemplarId")] Emprestimo emprestimo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(emprestimo);

            var exemplar = await _context.Exemplares.FindAsync(emprestimo.ExemplarId);
            if (exemplar != null)
            {
                exemplar.Estado = "Emprestado";
                _context.Update(exemplar);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        PopularExemplares(emprestimo.ExemplarId, apenasDisponiveis: true);
        return View(emprestimo);
    }

    // GET: EMPRESTIMOS/Edit/5
    public async Task<IActionResult> Edit(int? emprestimoid)
    {
        if (emprestimoid == null) return NotFound();

        var emprestimo = await _context.Emprestimos.FindAsync(emprestimoid);
        if (emprestimo == null) return NotFound();

        PopularExemplares(emprestimo.ExemplarId);
        return View(emprestimo);
    }

    // POST: EMPRESTIMOS/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? emprestimoid, [Bind("EmprestimoId,dataEmprestimo,dataDevolucao,estado,ExemplarId")] Emprestimo emprestimo)
    {
        if (emprestimoid != emprestimo.EmprestimoId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(emprestimo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmprestimoExists(emprestimo.EmprestimoId)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }

        PopularExemplares(emprestimo.ExemplarId);
        return View(emprestimo);
    }

    // GET: EMPRESTIMOS/Delete/5
    public async Task<IActionResult> Delete(int? emprestimoid)
    {
        if (emprestimoid == null) return NotFound();

        var emprestimo = await _context.Emprestimos
            .Include(e => e.Exemplar)
            .FirstOrDefaultAsync(m => m.EmprestimoId == emprestimoid);

        return emprestimo == null ? NotFound() : View(emprestimo);
    }

    // POST: EMPRESTIMOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? emprestimoid)
    {
        var emprestimo = await _context.Emprestimos.FindAsync(emprestimoid);
        if (emprestimo != null)
        {
            var exemplar = await _context.Exemplares.FindAsync(emprestimo.ExemplarId);
            if (exemplar != null)
            {
                exemplar.Estado = "Disponível";
                _context.Update(exemplar);
            }

            _context.Emprestimos.Remove(emprestimo);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // POST: EMPRESTIMOS/Return/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ReturnBook(int id)
    {
        var emprestimo = await _context.Emprestimos
            .Include(e => e.Exemplar)
            .FirstOrDefaultAsync(m => m.EmprestimoId == id);

        if (emprestimo == null) return NotFound();

        emprestimo.estado = "Devolvido";
        _context.Update(emprestimo);

        if (emprestimo.Exemplar != null)
        {
            emprestimo.Exemplar.Estado = "Disponível";
            _context.Update(emprestimo.Exemplar);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool EmprestimoExists(int? emprestimoid)
    {
        return _context.Emprestimos.Any(e => e.EmprestimoId == emprestimoid);
    }
}
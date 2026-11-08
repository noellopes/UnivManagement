
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class DepartamentosController : Controller
{
    private readonly UniversityDbContext _context;

    public DepartamentosController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: DEPARTAMENTOS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Departamentos.ToListAsync());
    }

    // GET: DEPARTAMENTOS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var departamento = await _context.Departamentos
            .FirstOrDefaultAsync(m => m.DepartamentoId == id);
        if (departamento == null)
        {
            return NotFound();
        }

        return View(departamento);
    }

    // GET: DEPARTAMENTOS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: DEPARTAMENTOS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("id,Nome,Ativo,DataCriacao")] Departamento departamento)
    {
        if (ModelState.IsValid)
        {
            _context.Add(departamento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(departamento);
    }

    // GET: DEPARTAMENTOS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var departamento = await _context.Departamentos.FindAsync(id);
        if (departamento == null)
        {
            return NotFound();
        }
        return View(departamento);
    }

    // POST: DEPARTAMENTOS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("id,Nome,Ativo,DataCriacao")] Departamento departamento)
    {
        if (id != departamento .DepartamentoId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(departamento);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartamentoExists(departamento.DepartamentoId))
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
        return View(departamento);
    }

    // GET: DEPARTAMENTOS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var departamento = await _context.Departamentos
            .FirstOrDefaultAsync(m => m.DepartamentoId == id);
        if (departamento == null)
        {
            return NotFound();
        }

        return View(departamento);
    }

    // POST: DEPARTAMENTOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var departamento = await _context.Departamentos.FindAsync(id);
        if (departamento != null)
        {
            _context.Departamentos.Remove(departamento);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DepartamentoExists(int? id)
    {
        return _context.Departamentos.Any(e => e.DepartamentoId == id);
    }
}

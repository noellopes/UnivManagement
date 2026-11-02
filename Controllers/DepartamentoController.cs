
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class DepartamentoController : Controller
{
    private readonly UniversityDbContext _context;

    public DepartamentoController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: DEPARTAMENTOS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Departamento.ToListAsync());
    }

    // GET: DEPARTAMENTOS/Details/5
    [HttpGet("/DEPARTAMENTO/Details/{departamentoid}")]
    public async Task<IActionResult> Details(int? departamentoid)
    {
        if (departamentoid == null)
        {
            return NotFound();
        }

        var departamento = await _context.Departamento
            .FirstOrDefaultAsync(m => m.DepartamentoId == departamentoid);
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
    public async Task<IActionResult> Create([Bind("DepartamentoId,Nome,Ativo,DataCriacao")] Departamento departamento)
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
    [HttpGet("/DEPARTAMENTO/Edit/{departamentoid}")]
    public async Task<IActionResult> Edit(int? departamentoid)
    {
        if (departamentoid == null)
        {
            return NotFound();
        }

        var departamento = await _context.Departamento.FindAsync(departamentoid);
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
    public async Task<IActionResult> Edit(int? departamentoid, [Bind("DepartamentoId,Nome,Ativo,DataCriacao")] Departamento departamento)
    {
        if (departamentoid != departamento.DepartamentoId)
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
    [HttpGet("/DEPARTAMENTO/Delete/{departamentoid}")]
    public async Task<IActionResult> Delete(int? departamentoid)
    {
        if (departamentoid == null)
        {
            return NotFound();
        }

        var departamento = await _context.Departamento
            .FirstOrDefaultAsync(m => m.DepartamentoId == departamentoid);
        if (departamento == null)
        {
            return NotFound();
        }

        return View(departamento);
    }

    // POST: DEPARTAMENTOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? departamentoid)
    {
        var departamento = await _context.Departamento.FindAsync(departamentoid);
        if (departamento != null)
        {
            _context.Departamento.Remove(departamento);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DepartamentoExists(int? departamentoid)
    {
        return _context.Departamento.Any(e => e.DepartamentoId == departamentoid);
    }
}

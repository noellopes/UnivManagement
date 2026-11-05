using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class DepartamentoesController : Controller
{
    private readonly UniversityDbContext _context;

    public DepartamentoesController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: DEPARTAMENTOS
    public async Task<IActionResult> Index()
    {
        var lista = await _context.Departamento.ToListAsync();
        if (!lista.Any())
        {
            _context.Departamento.Add(new Departamento { Nome = "Informática", Ativo = true, DataCriacao = DateTime.Now });
            await _context.SaveChangesAsync();
            lista = await _context.Departamento.ToListAsync();
        }
        return View(lista);
    }

    // GET: DEPARTAMENTOS/Details/5
    public async Task<IActionResult> Details(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var departamento = await _context.Departamento
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
    public async Task<IActionResult> Edit(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var departamento = await _context.Departamento.FindAsync(id);
        if (departamento == null)
        {
            return NotFound();
        }
        return View(departamento);
    }

    // POST: DEPARTAMENTOS/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long? id, [Bind("DepartamentoId,Nome,Ativo,DataCriacao")] Departamento departamento)
    {
        if (id != departamento.DepartamentoId)
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
    public async Task<IActionResult> Delete(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var departamento = await _context.Departamento
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
    public async Task<IActionResult> DeleteConfirmed(long? id)
    {
        var departamento = await _context.Departamento.FindAsync(id);
        if (departamento != null)
        {
            _context.Departamento.Remove(departamento);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DepartamentoExists(long? id)
    {
        return _context.Departamento.Any(e => e.DepartamentoId == id);
    }
}


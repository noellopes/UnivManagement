
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class DocentesController : Controller
{
    private readonly ApplicationUsersDbContext _context;

    public DocentesController(ApplicationUsersDbContext context)
    {
        _context = context;
    }

    // GET: DOCENTES
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Docente.ToListAsync());
    }

    // GET: DOCENTES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var docente = await _context.Docente
            .FirstOrDefaultAsync(m => m.DocenteId == id);
        if (docente == null)
        {
            return NotFound();
        }

        return View(docente);
    }

    // GET: DOCENTES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: DOCENTES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("DocenteId,Nome,Nif,Email,Telefone,FuncaoDocenteFK,RegimeFK,Ativo,DataAdmissao,Morada,DepartamentoFk")] Docente docente)
    {
        if (ModelState.IsValid)
        {
            _context.Add(docente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(docente);
    }

    // GET: DOCENTES/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var docente = await _context.Docente.FindAsync(id);
        if (docente == null)
        {
            return NotFound();
        }
        return View(docente);
    }

    // POST: DOCENTES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? docenteid, [Bind("DocenteId,Nome,Nif,Email,Telefone,FuncaoDocenteFK,RegimeFK,Ativo,DataAdmissao,Morada,DepartamentoFk")] Docente docente)
    {
        if (docenteid != docente.DocenteId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(docente);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocenteExists(docente.DocenteId))
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
        return View(docente);
    }

    // GET: DOCENTES/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var docente = await _context.Docente
            .FirstOrDefaultAsync(m => m.DocenteId == id);
        if (docente == null)
        {
            return NotFound();
        }

        return View(docente);
    }

    // POST: DOCENTES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? docenteid)
    {
        var docente = await _context.Docente.FindAsync(docenteid);
        if (docente != null)
        {
            _context.Docente.Remove(docente);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DocenteExists(int? docenteid)
    {
        return _context.Docente.Any(e => e.DocenteId == docenteid);
    }
}


using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class FuncaoDocenteController : Controller
{
    private readonly UniversityDbContext _context;

    public FuncaoDocenteController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: FUNCAODOCENTES
    public async Task<IActionResult> Index()    
    {
        return View(await _context.FuncaoDocente.ToListAsync());
    }

    // GET: FUNCAODOCENTES/Details/5
    [HttpGet("FuncaoDocente/Details/{funcaodocenteid}")]
    public async Task<IActionResult> Details(int? funcaodocenteid, string? successMessage = null)
    {
        if (funcaodocenteid == null)
        {
            return NotFound();
        }

        var funcaodocente = await _context.FuncaoDocente
            .FirstOrDefaultAsync(m => m.FuncaoDocenteId == funcaodocenteid);
        if (funcaodocente == null)
        {
            return View("NotFound");
        }

        if (successMessage != null)
        {
            ViewBag.SuccessMessage = successMessage;
        }

        return View(funcaodocente);
    }

    // GET: FUNCAODOCENTES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: FUNCAODOCENTES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("FuncaoDocenteId,Nome,Ativo,DataCriacao")] FuncaoDocente funcaodocente)
    {
        if (ModelState.IsValid)
        {
            _context.Add(funcaodocente);
            await _context.SaveChangesAsync();
            return RedirectToAction(
                nameof(Details), 
                new { id = funcaodocente.FuncaoDocenteId, successMessage = "Fução docente criado com sucesso." });
        }
        return View(funcaodocente);
    }

    // GET: FUNCAODOCENTES/Edit/5
    [HttpGet("FuncaoDocente/Edit/{funcaodocenteid}")]
    public async Task<IActionResult> Edit(int? funcaodocenteid)
    {
        if (funcaodocenteid == null)
        {
            return View("NotFound");
        }

        var funcaodocente = await _context.FuncaoDocente.FindAsync(funcaodocenteid);
        if (funcaodocente == null)
        {
            return NotFound();
        }
        return View(funcaodocente);
    }

    // POST: FUNCAODOCENTES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? funcaodocenteid, [Bind("FuncaoDocenteId,Nome,Ativo,DataCriacao")] FuncaoDocente funcaodocente)
    {
        if (funcaodocenteid != funcaodocente.FuncaoDocenteId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(funcaodocente);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FuncaoDocenteExists(funcaodocente.FuncaoDocenteId))
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
        return View(funcaodocente);
    }

    // GET: FUNCAODOCENTES/Delete/5
    [HttpGet("FuncaoDocente/Delete/{funcaodocenteid}")]
    public async Task<IActionResult> Delete(int? funcaodocenteid)
    {
        if (funcaodocenteid == null)
        {
            return NotFound();
        }

        var funcaodocente = await _context.FuncaoDocente
            .FirstOrDefaultAsync(m => m.FuncaoDocenteId == funcaodocenteid);
        if (funcaodocente == null)
        {
            return NotFound();
        }

        return View(funcaodocente);
    }

    // POST: FUNCAODOCENTES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? funcaodocenteid )
    {
        var funcaodocente = await _context.FuncaoDocente.FindAsync(funcaodocenteid);
        if (funcaodocente != null)
        {
            _context.FuncaoDocente.Remove(funcaodocente);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool FuncaoDocenteExists(int? funcaodocenteid)
    {
        return _context.FuncaoDocente.Any(e => e.FuncaoDocenteId == funcaodocenteid);
    }
}

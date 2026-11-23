
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models.AcademicManagement;
using UnivManagement.Data;

public class UCController : Controller
{
    private readonly UniversityDbContext _context;

    public UCController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: UC
    public async Task<IActionResult> Index(string? successMessage = null)    
    {
        if (successMessage != null)
        {
            ViewBag.SuccessMessage = successMessage;
        }

        return View(await _context.UC.ToListAsync());
    }

    // GET: UCS/Details/5
    public async Task<IActionResult> Details(int? id, string? successMessage = null)
    {
        if (id == null)
        {
            return NotFound();
        }

        var uc = await _context.UC
            .FirstOrDefaultAsync(m => m.UcId == id);

        if (uc == null)
        {
            return View("NotFound");
        }

        if (successMessage != null)
        {
            ViewBag.SuccessMessage = successMessage;
        }

        return View(uc);
    }

    // GET: UCS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: UCS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("UcId,Code,Name,Acronym,Description")] UC uc)
    {
        if (ModelState.IsValid)
        {
            _context.Add(uc);
            await _context.SaveChangesAsync();
            return RedirectToAction(
                nameof(Details),
                new { id = uc.UcId, successMessage = "Unidade Curricular criada com sucesso." }
            );
        }
        return View(uc);
    }

    // GET: UCS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var uc = await _context.UC.FindAsync(id);
        if (uc == null)
        {
            return View("NotFound");
        }

        return View(uc);
    }

    // POST: UCS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("UcId,Code,Name,Acronym,Description")] UC uc)
    {
        if (id != uc.UcId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(uc);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UCExists(uc.UcId))
                {
                    return View("NotFound");
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(
                nameof(Details),
                new { id = uc.UcId, successMessage = "Dados da UC atualizados com sucesso." }
            );
        }
        return View(uc);
    }

    // GET: UCS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var uc = await _context.UC
            .FirstOrDefaultAsync(m => m.UcId == id);

        // Se registo já tiver sido eliminado noutra janela
        if (uc == null)
        {
            return RedirectToAction(nameof(Index),
                new { successMessage = "Aviso: A Unidade Curricular já tinha sido eliminada por outro utilizador." });
        }

        return View(uc);
    }

    // POST: UCS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var uc = await _context.UC.FindAsync(id);
        if (uc != null)
        {
            _context.UC.Remove(uc);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index), new { successMessage = "Unidade Curricular eliminada com sucesso." });
    }

    private bool UCExists(int? id)
    {
        return _context.UC.Any(e => e.UcId == id);
    }
}

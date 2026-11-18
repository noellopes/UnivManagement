using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class SubmissaoTrabalhoController : Controller
{
    private readonly UniversityDbContext _context;

    public SubmissaoTrabalhoController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: SUBMISSAOTRABALHOS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.SubmissoesTrabalho.ToListAsync());
    }

    // GET: SUBMISSAOTRABALHOS/Details/5
    public async Task<IActionResult> Details(int? submissaotrabalhoid)
    {
        if (submissaotrabalhoid == null)
        {
            return NotFound();
        }

        var submissaotrabalho = await _context.SubmissoesTrabalho
            .FirstOrDefaultAsync(m => m.SubmissaoTrabalhoId == submissaotrabalhoid);
        if (submissaotrabalho == null)
        {
            return NotFound();
        }

        return View(submissaotrabalho);
    }

    // GET: SUBMISSAOTRABALHOS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: SUBMISSAOTRABALHOS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("SubmissaoTrabalhoId,TrabalhoId,Trabalho,StudentId,Student,GrupoTrabalhoId,GrupoTrabalho,DataEntrega,CaminhoFicheiro")] SubmissaoTrabalho submissaotrabalho)
    {
        if (ModelState.IsValid)
        {
            _context.Add(submissaotrabalho);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(submissaotrabalho);
    }

    // GET: SUBMISSAOTRABALHOS/Edit/5
    public async Task<IActionResult> Edit(int? submissaotrabalhoid)
    {
        if (submissaotrabalhoid == null)
        {
            return NotFound();
        }

        var submissaotrabalho = await _context.SubmissoesTrabalho.FindAsync(submissaotrabalhoid);
        if (submissaotrabalho == null)
        {
            return NotFound();
        }
        return View(submissaotrabalho);
    }

    // POST: SUBMISSAOTRABALHOS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? submissaotrabalhoid, [Bind("SubmissaoTrabalhoId,TrabalhoId,Trabalho,StudentId,Student,GrupoTrabalhoId,GrupoTrabalho,DataEntrega,CaminhoFicheiro")] SubmissaoTrabalho submissaotrabalho)
    {
        if (submissaotrabalhoid != submissaotrabalho.SubmissaoTrabalhoId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(submissaotrabalho);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubmissaoTrabalhoExists(submissaotrabalho.SubmissaoTrabalhoId))
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
        return View(submissaotrabalho);
    }

    // GET: SUBMISSAOTRABALHOS/Delete/5
    public async Task<IActionResult> Delete(int? submissaotrabalhoid)
    {
        if (submissaotrabalhoid == null)
        {
            return NotFound();
        }

        var submissaotrabalho = await _context.SubmissoesTrabalho
            .FirstOrDefaultAsync(m => m.SubmissaoTrabalhoId == submissaotrabalhoid);
        if (submissaotrabalho == null)
        {
            return NotFound();
        }

        return View(submissaotrabalho);
    }

    // POST: SUBMISSAOTRABALHOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? submissaotrabalhoid)
    {
        var submissaotrabalho = await _context.SubmissoesTrabalho.FindAsync(submissaotrabalhoid);
        if (submissaotrabalho != null)
        {
            _context.SubmissoesTrabalho.Remove(submissaotrabalho);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SubmissaoTrabalhoExists(int? submissaotrabalhoid)
    {
        return _context.SubmissoesTrabalho.Any(e => e.SubmissaoTrabalhoId == submissaotrabalhoid);
    }
}

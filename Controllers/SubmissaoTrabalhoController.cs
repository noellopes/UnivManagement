using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

    // GET: SubmissaoTrabalho
    public async Task<IActionResult> Index()
    {
        var submissoes = await _context.SubmissoesTrabalho
            .Include(s => s.Trabalho)
            .Include(s => s.Student)
            .Include(s => s.GrupoTrabalho)
            .ToListAsync();

        return View(submissoes);
    }

    // GET: SubmissaoTrabalho/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var submissao = await _context.SubmissoesTrabalho
            .Include(s => s.Trabalho)
            .Include(s => s.Student)
            .Include(s => s.GrupoTrabalho)
            .FirstOrDefaultAsync(s => s.SubmissaoTrabalhoId == id);

        if (submissao == null)
        {
            return NotFound();
        }

        return View(submissao);
    }

    // GET: SubmissaoTrabalho/Create
    public IActionResult Create()
    {
        PreencherListas();
        return View();
    }

    // POST: SubmissaoTrabalho/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("SubmissaoTrabalhoId,TrabalhoId,StudentId,GrupoTrabalhoId,DataEntrega,CaminhoFicheiro")]
        SubmissaoTrabalho submissao)
    {
        if (ModelState.IsValid)
        {
            _context.SubmissoesTrabalho.Add(submissao);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        PreencherListas(
            submissao.TrabalhoId,
            submissao.StudentId,
            submissao.GrupoTrabalhoId
        );

        return View(submissao);
    }

    // GET: SubmissaoTrabalho/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var submissao = await _context.SubmissoesTrabalho.FindAsync(id);

        if (submissao == null)
        {
            return NotFound();
        }

        PreencherListas(
            submissao.TrabalhoId,
            submissao.StudentId,
            submissao.GrupoTrabalhoId
        );

        return View(submissao);
    }

    // POST: SubmissaoTrabalho/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("SubmissaoTrabalhoId,TrabalhoId,StudentId,GrupoTrabalhoId,DataEntrega,CaminhoFicheiro")]
        SubmissaoTrabalho submissao)
    {
        if (id != submissao.SubmissaoTrabalhoId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.SubmissoesTrabalho.Update(submissao);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubmissaoTrabalhoExists(submissao.SubmissaoTrabalhoId))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        PreencherListas(
            submissao.TrabalhoId,
            submissao.StudentId,
            submissao.GrupoTrabalhoId
        );

        return View(submissao);
    }

    // GET: SubmissaoTrabalho/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var submissao = await _context.SubmissoesTrabalho
            .Include(s => s.Trabalho)
            .Include(s => s.Student)
            .Include(s => s.GrupoTrabalho)
            .FirstOrDefaultAsync(s => s.SubmissaoTrabalhoId == id);

        if (submissao == null)
        {
            return NotFound();
        }

        return View(submissao);
    }

    // POST: SubmissaoTrabalho/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var submissao = await _context.SubmissoesTrabalho.FindAsync(id);

        if (submissao != null)
        {
            _context.SubmissoesTrabalho.Remove(submissao);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    // Preenche as listas dos formulários
    private void PreencherListas(
        int? trabalhoId = null,
        int? studentId = null,
        int? grupoTrabalhoId = null)
    {
        ViewBag.Trabalhos = new SelectList(
            _context.Trabalhos,
            "TrabalhoId",
            "Titulo",
            trabalhoId
        );

        ViewBag.Alunos = new SelectList(
            _context.Student,
            "StudentId",
            "StudentId",
            studentId
        );

        ViewBag.Grupos = new SelectList(
            _context.Set<GrupoTrabalho>(),
            "GrupoTrabalhoId",
            "Nome",
            grupoTrabalhoId
        );
    }

    // Verifica se a submissão existe
    private bool SubmissaoTrabalhoExists(int id)
    {
        return _context.SubmissoesTrabalho
            .Any(s => s.SubmissaoTrabalhoId == id);
    }
}

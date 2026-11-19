using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class HorarioController : Controller
{
    private readonly UniversityDbContext _context;

    public HorarioController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: Horario
    [HttpGet("Horario")]
    [HttpGet("Horario/Index")]
    public async Task<IActionResult> Index()
    {
        var horarios = _context.Horario
            .Include(h => h.Uc)
            .Include(h => h.Departamento)
            // .Include(h => h.Turma)   // repor quando a Turma entrar na main
            // .Include(h => h.Sala)    // repor quando a Sala entrar na main
            .Include(h => h.AnoLetivo);
        return View(await horarios.ToListAsync());
    }

    // GET: Horario/Details/5
    [HttpGet("Horario/Details/{horarioid}")]
    public async Task<IActionResult> Details(int? horarioid)
    {
        if (horarioid == null)
        {
            return NotFound();
        }

        var horario = await _context.Horario
            .Include(h => h.Uc)
            .Include(h => h.Departamento)
            // .Include(h => h.Turma)
            // .Include(h => h.Sala)
            .Include(h => h.AnoLetivo)
            .FirstOrDefaultAsync(m => m.HorarioId == horarioid);
        if (horario == null)
        {
            return NotFound();
        }

        return View(horario);
    }

    // GET: Horario/Create
    [HttpGet("Horario/Create")]
    public IActionResult Create()
    {
        PreencherListas(null);
        return View();
    }

    // POST: Horario/Create
    // (repor TurmaId e SalaId no Bind quando entrarem na main)
    [HttpPost("Horario/Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Semestre,DiaSemana,HoraInicio,HoraFim,TipoAula,Periodicidade,Estado,Observacoes,UcId,DepartamentoId,AcademicYearId")] Horario horario)
    {
        if (ModelState.IsValid)
        {
            _context.Add(horario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        PreencherListas(horario);
        return View(horario);
    }

    // GET: Horario/Edit/5
    [HttpGet("Horario/Edit/{horarioid}")]
    public async Task<IActionResult> Edit(int? horarioid)
    {
        if (horarioid == null)
        {
            return NotFound();
        }

        var horario = await _context.Horario.FindAsync(horarioid);
        if (horario == null)
        {
            return NotFound();
        }
        PreencherListas(horario);
        return View(horario);
    }

    // POST: Horario/Edit/5
    [HttpPost("Horario/Edit/{horarioid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? horarioid, [Bind("HorarioId,Semestre,DiaSemana,HoraInicio,HoraFim,TipoAula,Periodicidade,Estado,Observacoes,DataCriacao,UcId,DepartamentoId,AcademicYearId")] Horario horario)
    {
        if (horarioid != horario.HorarioId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(horario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HorarioExists(horario.HorarioId))
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
        PreencherListas(horario);
        return View(horario);
    }

    // GET: Horario/Delete?horarioid=5
    public async Task<IActionResult> Delete(int? horarioid)
    {
        if (horarioid == null)
        {
            return NotFound();
        }

        var horario = await _context.Horario
            .Include(h => h.Uc)
            .Include(h => h.Departamento)
            // .Include(h => h.Turma)
            // .Include(h => h.Sala)
            .Include(h => h.AnoLetivo)
            .FirstOrDefaultAsync(m => m.HorarioId == horarioid);
        if (horario == null)
        {
            return NotFound();
        }

        return View(horario);
    }

    // POST: Horario/Delete
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? horarioid)
    {
        var horario = await _context.Horario.FindAsync(horarioid);
        if (horario != null)
        {
            _context.Horario.Remove(horario);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool HorarioExists(int? horarioid)
    {
        return _context.Horario.Any(e => e.HorarioId == horarioid);
    }

    // Listas das caixas de seleção (dropdowns) das chaves estrangeiras
    private void PreencherListas(Horario? h)
    {
        ViewData["UcId"] = new SelectList(_context.UC, "UcId", "Name", h?.UcId);
        ViewData["DepartamentoId"] = new SelectList(_context.Set<Departamento>(), "DepartamentoId", "Nome", h?.DepartamentoId);
        ViewData["AcademicYearId"] = new SelectList(_context.AcademicYear, "AcademicYearId", "StartYear", h?.AcademicYearId);
        // ViewData["TurmaId"] = new SelectList(_context.Turma, "TurmaId", "NomeTurma", h?.TurmaId);
        // ViewData["SalaId"] = new SelectList(_context.Sala, "SalaId", "NumeroSala", h?.SalaId);
    }
}

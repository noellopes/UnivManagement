using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models.TestesEstagios;

public class EstagiosController : Controller
{
    private readonly EstagiosTesteDbContext _context;

    public EstagiosController(EstagiosTesteDbContext context)
    {
        _context = context;
    }

    private async Task CarregarListas()
    {
        ViewBag.Alunos = await _context.Student.ToListAsync();
        ViewBag.Empresas = await _context.Empresas.ToListAsync();
        ViewBag.Orientadores = await _context.OrientadoresEstagio.ToListAsync();
    }

    private string CalcularEstado(DateTime dataInicio, DateTime dataFim)
    {
        DateTime hoje = DateTime.Today;

        if (dataFim.Date < hoje)
        {
            return "Concluído";
        }

        if (dataInicio.Date > hoje)
        {
            return "Pendente";
        }

        return "Em Curso";
    }

    private int CalcularDuracao(DateTime dataInicio, DateTime dataFim)
    {
        return (dataFim.Date - dataInicio.Date).Days;
    }

    // GET: Estagios
    public async Task<IActionResult> Index()
    {
        var estagios = await _context.Estagios.ToListAsync();

        foreach (var estagio in estagios)
        {
            estagio.Estado = CalcularEstado(
                estagio.DataInicio, estagio.DataFim);
        }

        return View(estagios);
    }

    // GET: Estagios/Details
    public async Task<IActionResult> Details(int? idestagio)
    {
        if (idestagio == null)
        {
            return NotFound();
        }

        var estagio = await _context.Estagios
            .FirstOrDefaultAsync(e => e.IdEstagio == idestagio);

        if (estagio == null)
        {
            return NotFound();
        }

        estagio.Estado = CalcularEstado(
            estagio.DataInicio, estagio.DataFim);

        return View(estagio);
    }

    // GET: Estagios/Create
    public async Task<IActionResult> Create()
    {
        await CarregarListas();
        return View();
    }

    // POST: Estagios/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        DateTime DataInicio,
        DateTime DataFim,
        string PlanoTrabalhos,
        int StudentId,
        int IdEmpresa,
        int? IdOrientador)
    {
        var estagio = new Estagio
        {
            DataInicio = DataInicio,
            DataFim = DataFim,
            PlanoTrabalhos = PlanoTrabalhos,
            StudentId = StudentId,
            IdEmpresa = IdEmpresa,
            IdOrientador = IdOrientador
        };

        if (DataFim.Date < DataInicio.Date)
        {
            ViewBag.ErroDatas =
                "A data de fim não pode ser anterior à data de início.";
        }
        else if (ModelState.IsValid)
        {
            estagio.Duracao = CalcularDuracao(
                DataInicio, DataFim);

            estagio.Estado = CalcularEstado(
                DataInicio, DataFim);

            _context.Estagios.Add(estagio);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        await CarregarListas();
        return View(estagio);
    }

    // GET: Estagios/Edit
    public async Task<IActionResult> Edit(int? idestagio)
    {
        if (idestagio == null)
        {
            return NotFound();
        }

        var estagio = await _context.Estagios.FindAsync(idestagio);

        if (estagio == null)
        {
            return NotFound();
        }

        await CarregarListas();
        return View(estagio);
    }

    // POST: Estagios/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int? idestagio,
        [Bind("IdEstagio,DataInicio,DataFim,Duracao,Estado,PlanoTrabalhos,StudentId,IdEmpresa,IdOrientador")]
        Estagio estagio)
    {
        if (idestagio != estagio.IdEstagio)
        {
            return NotFound();
        }

        if (DataFimAnterior(estagio.DataInicio, estagio.DataFim))
        {
            ViewBag.ErroDatas =
                "A data de fim não pode ser anterior à data de início.";

            await CarregarListas();
            return View(estagio);
        }

        if (ModelState.IsValid)
        {
            estagio.Duracao = CalcularDuracao(
                estagio.DataInicio, estagio.DataFim);

            estagio.Estado = CalcularEstado(
                estagio.DataInicio, estagio.DataFim);

            try
            {
                _context.Update(estagio);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstagioExists(estagio.IdEstagio))
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

        await CarregarListas();
        return View(estagio);
    }

    private bool DataFimAnterior(DateTime dataInicio, DateTime dataFim)
    {
        return dataFim.Date < dataInicio.Date;
    }

    // GET: Estagios/Delete
    public async Task<IActionResult> Delete(int? idestagio)
    {
        if (idestagio == null)
        {
            return NotFound();
        }

        var estagio = await _context.Estagios
            .FirstOrDefaultAsync(e => e.IdEstagio == idestagio);

        if (estagio == null)
        {
            return NotFound();
        }

        return View(estagio);
    }

    // POST: Estagios/Delete
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? idestagio)
    {
        var estagio = await _context.Estagios.FindAsync(idestagio);

        if (estagio != null)
        {
            _context.Estagios.Remove(estagio);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool EstagioExists(int? idestagio)
    {
        return _context.Estagios.Any(e => e.IdEstagio == idestagio);
    }
}
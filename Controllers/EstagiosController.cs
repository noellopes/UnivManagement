using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

public class EstagiosController : Controller
{
    private readonly UniversityDbContext _context;

    public EstagiosController(UniversityDbContext context)
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

    private bool DataFimAnterior(DateTime dataInicio, DateTime dataFim)
    {
        return dataFim.Date < dataInicio.Date;
    }

    // GET: Estagios
    public async Task<IActionResult> Index()
    {
        var estagios = await _context.Estagios.ToListAsync();

        foreach (var estagio in estagios)
        {
            estagio.Estado = CalcularEstado(
                estagio.DataInicio, estagio.DataFim);

            estagio.Student = await _context.Student
                .FindAsync(estagio.StudentId);

            estagio.Empresa = await _context.Empresas
                .FindAsync(estagio.IdEmpresa);
        }

        return View(estagios);
    }

    // GET: Estagios/Details/5
    public async Task<IActionResult> Details(int? estagioId)
    {
        if (estagioId == null)
        {
            return NotFound();
        }

        var estagio = await _context.Estagios
            .FirstOrDefaultAsync(e => e.EstagioId == estagioId);

        if (estagio == null)
        {
            return NotFound();
        }

        estagio.Estado = CalcularEstado(
            estagio.DataInicio, estagio.DataFim);

        var aluno = await _context.Student
            .FirstOrDefaultAsync(a => a.StudentId == estagio.StudentId);

        var empresa = await _context.Empresas
            .FirstOrDefaultAsync(e => e.IdEmpresa == estagio.IdEmpresa);

        ViewBag.NomeAluno = aluno != null
            ? aluno.Name
            : "Aluno não encontrado";

        ViewBag.NomeEmpresa = empresa != null
            ? empresa.Nome
            : "Empresa não encontrada";

        ViewBag.NomeOrientador = "Sem orientador";

        if (estagio.OrientadorId != null)
        {
            var orientador = await _context.OrientadoresEstagio
                .FirstOrDefaultAsync(o =>
                    o.OrientadorId == estagio.OrientadorId);

            ViewBag.NomeOrientador = orientador != null
                ? orientador.Nome
                : "Orientador não encontrado";
        }

        return View(estagio);
    }

    // GET: Estagios/Create
    public async Task<IActionResult> Create()
    {
        await CarregarListas();
        return View();
    }

    // POST: Estagios/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        DateTime DataInicio,
        DateTime DataFim,
        string PlanoTrabalhos,
        int StudentId,
        int IdEmpresa,
        int? OrientadorId)
    {
        var estagio = new Estagio
        {
            DataInicio = DataInicio,
            DataFim = DataFim,
            PlanoTrabalhos = PlanoTrabalhos,
            StudentId = StudentId,
            IdEmpresa = IdEmpresa,
            OrientadorId = OrientadorId
        };

        if (DataFimAnterior(DataInicio, DataFim))
        {
            ViewBag.ErroDatas =
                "A data de fim não pode ser anterior à data de início.";
        }
        else if (!_context.Student.Any(a => a.StudentId == StudentId))
        {
            ViewBag.ErroFormulario =
                "Selecione um aluno válido.";
        }
        else if (!_context.Empresas.Any(e => e.IdEmpresa == IdEmpresa))
        {
            ViewBag.ErroFormulario =
                "Selecione uma empresa válida.";
        }
        else if (OrientadorId != null &&
                 !_context.OrientadoresEstagio.Any(
                     o => o.OrientadorId == OrientadorId))
        {
            ViewBag.ErroFormulario =
                "Selecione um orientador válido.";
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

    // GET: Estagios/Edit/5
    public async Task<IActionResult> Edit(int? estagioId)
    {
        if (estagioId == null)
        {
            return NotFound();
        }

        var estagio = await _context.Estagios.FindAsync(estagioId);

        if (estagio == null)
        {
            return NotFound();
        }

        await CarregarListas();
        return View(estagio);
    }

    // POST: Estagios/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int? estagioId,
        DateTime DataInicio,
        DateTime DataFim,
        string PlanoTrabalhos,
        int StudentId,
        int IdEmpresa,
        int? OrientadorId)
    {
        if (estagioId == null)
        {
            return NotFound();
        }

        var estagio = await _context.Estagios.FindAsync(estagioId);

        if (estagio == null)
        {
            return NotFound();
        }

        if (DataFimAnterior(DataInicio, DataFim))
        {
            ViewBag.ErroDatas =
                "A data de fim não pode ser anterior à data de início.";
        }
        else if (!_context.Student.Any(a => a.StudentId == StudentId))
        {
            ViewBag.ErroFormulario =
                "Selecione um aluno válido.";
        }
        else if (!_context.Empresas.Any(e => e.IdEmpresa == IdEmpresa))
        {
            ViewBag.ErroFormulario =
                "Selecione uma empresa válida.";
        }
        else if (OrientadorId != null &&
                 !_context.OrientadoresEstagio.Any(
                     o => o.OrientadorId == OrientadorId))
        {
            ViewBag.ErroFormulario =
                "Selecione um orientador válido.";
        }
        else if (ModelState.IsValid)
        {
            estagio.DataInicio = DataInicio;
            estagio.DataFim = DataFim;
            estagio.PlanoTrabalhos = PlanoTrabalhos;
            estagio.StudentId = StudentId;
            estagio.IdEmpresa = IdEmpresa;
            estagio.OrientadorId = OrientadorId;

            estagio.Duracao = CalcularDuracao(
                DataInicio, DataFim);

            estagio.Estado = CalcularEstado(
                DataInicio, DataFim);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstagioExists(estagio.EstagioId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Details),
                new { estagioId = estagio.EstagioId });
        }

        await CarregarListas();
        return View(estagio);
    }

    // GET: Estagios/Delete/5
    public async Task<IActionResult> Delete(int? estagioId)
    {
        if (estagioId == null)
        {
            return NotFound();
        }

        var estagio = await _context.Estagios
            .FirstOrDefaultAsync(e => e.EstagioId == estagioId);

        if (estagio == null)
        {
            return NotFound();
        }

        return View(estagio);
    }

    // POST: Estagios/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? estagioId)
    {
        var estagio = await _context.Estagios.FindAsync(estagioId);

        if (estagio != null)
        {
            _context.Estagios.Remove(estagio);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool EstagioExists(int? estagioId)
    {
        return _context.Estagios.Any(e => e.EstagioId == estagioId);
    }
}
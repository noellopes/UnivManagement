using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;
using UnivManagement.ViewModels;

public class EstagiosController : Controller
{
    private readonly UniversityDbContext _context;

    public EstagiosController(UniversityDbContext context)
    {
        _context = context;
    }

    private void CarregarListas(Estagio? estagio = null)
    {
        ViewBag.StudentId = new SelectList(
            _context.Student, "StudentId", "Name", estagio?.StudentId);

        ViewBag.IdEmpresa = new SelectList(
            _context.Empresas, "IdEmpresa", "Nome", estagio?.IdEmpresa);

        ViewBag.OrientadorId = new SelectList(
            _context.OrientadoresEstagio, "OrientadorId", "Nome", estagio?.OrientadorId);
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
    public async Task<IActionResult> Index(int page = 1, string? successMessage = null)
    {
        if (successMessage != null)
        {
            ViewBag.SuccessMessage = successMessage;
        }

        var estagios = _context.Estagios
            .Include(e => e.Student)
            .Include(e => e.Empresa)
            .OrderBy(e => e.EstagioId);

        var queryOptions =
            await QueryOptions<Estagio>.CreateAsync(estagios, page);

        foreach (var estagio in queryOptions.Items)
        {
            estagio.Estado = CalcularEstado(
                estagio.DataInicio, estagio.DataFim);
        }

        return View(queryOptions);
    }

    // GET: Estagios/Details/5
    public async Task<IActionResult> Details(int? estagioId, string? successMessage = null)
    {
        if (estagioId == null)
        {
            return NotFound();
        }

        var estagio = await _context.Estagios
            .Include(e => e.Student)
            .Include(e => e.Empresa)
            .Include(e => e.OrientadorEstagio)
            .FirstOrDefaultAsync(e => e.EstagioId == estagioId);

        if (estagio == null)
        {
            return NotFound();
        }

        estagio.Estado = CalcularEstado(
            estagio.DataInicio, estagio.DataFim);

        if (successMessage != null)
        {
            ViewBag.SuccessMessage = successMessage;
        }

        return View(estagio);
    }

    // GET: Estagios/Create
    public IActionResult Create()
    {
        CarregarListas();
        return View();
    }

    // POST: Estagios/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("DataInicio,DataFim,PlanoTrabalhos,StudentId,IdEmpresa,OrientadorId")] Estagio estagio)
    {
        if (DataFimAnterior(estagio.DataInicio, estagio.DataFim))
        {
            ViewBag.ErroDatas =
                "A data de fim não pode ser anterior à data de início.";
        }
        else if (!_context.Student.Any(a => a.StudentId == estagio.StudentId))
        {
            ViewBag.ErroFormulario =
                "Selecione um aluno válido.";
        }
        else if (!_context.Empresas.Any(e => e.IdEmpresa == estagio.IdEmpresa))
        {
            ViewBag.ErroFormulario =
                "Selecione uma empresa válida.";
        }
        else if (estagio.OrientadorId != null &&
                 !_context.OrientadoresEstagio.Any(
                     o => o.OrientadorId == estagio.OrientadorId))
        {
            ViewBag.ErroFormulario =
                "Selecione um orientador válido.";
        }
        else if (ModelState.IsValid)
        {
            estagio.Duracao = CalcularDuracao(
                estagio.DataInicio, estagio.DataFim);

            estagio.Estado = CalcularEstado(
                estagio.DataInicio, estagio.DataFim);

            _context.Add(estagio);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details),
                new
                {
                    estagioId = estagio.EstagioId,
                    successMessage = "Estágio criado com sucesso."
                });
        }

        CarregarListas(estagio);
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

        CarregarListas(estagio);
        return View(estagio);
    }

    // POST: Estagios/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int? estagioId,
        [Bind("EstagioId,DataInicio,DataFim,PlanoTrabalhos,StudentId,IdEmpresa,OrientadorId")] Estagio estagio)
    {
        if (estagioId == null ||
            estagioId <= 0 ||
            estagioId != estagio.EstagioId)
        {
            return NotFound();
        }

        if (DataFimAnterior(estagio.DataInicio, estagio.DataFim))
        {
            ViewBag.ErroDatas =
                "A data de fim não pode ser anterior à data de início.";
        }
        else if (!_context.Student.Any(a => a.StudentId == estagio.StudentId))
        {
            ViewBag.ErroFormulario =
                "Selecione um aluno válido.";
        }
        else if (!_context.Empresas.Any(e => e.IdEmpresa == estagio.IdEmpresa))
        {
            ViewBag.ErroFormulario =
                "Selecione uma empresa válida.";
        }
        else if (estagio.OrientadorId != null &&
                 !_context.OrientadoresEstagio.Any(
                     o => o.OrientadorId == estagio.OrientadorId))
        {
            ViewBag.ErroFormulario =
                "Selecione um orientador válido.";
        }
        else if (ModelState.IsValid)
        {
            var dadosAtuais = await _context.Estagios
                .Where(e => e.EstagioId == estagioId)
                .Select(e => new { e.GestorId })
                .FirstOrDefaultAsync();

            if (dadosAtuais == null)
            {
                return NotFound();
            }

            estagio.GestorId = dadosAtuais.GestorId;

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
                new
                {
                    estagioId = estagio.EstagioId,
                    successMessage = "Estágio atualizado com sucesso."
                });
        }

        CarregarListas(estagio);
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
            return RedirectToAction(nameof(Index),
                new { successMessage = "O estágio já tinha sido eliminado." });
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

            return RedirectToAction(nameof(Index),
                new { successMessage = "Estágio eliminado com sucesso." });
        }

        return RedirectToAction(nameof(Index),
            new { successMessage = "O estágio já tinha sido eliminado." });
    }

    private bool EstagioExists(int? estagioId)
    {
        return _context.Estagios.Any(e => e.EstagioId == estagioId);
    }
}
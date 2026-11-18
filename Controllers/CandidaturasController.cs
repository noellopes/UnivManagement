using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

public class CandidaturasController : Controller
{
    private readonly UniversityDbContext _context;

    public CandidaturasController(UniversityDbContext context)
    {
        _context = context;
    }

    private async Task CarregarListas()
    {
        ViewBag.Alunos = await _context.Estudantes.ToListAsync();
        ViewBag.Ofertas = await _context.Ofertas.ToListAsync();
    }

    private string CalcularEstado(DateTime dataCandidatura)
    {
        DateTime hoje = DateTime.Today;

        if (dataCandidatura.Date > hoje)
        {
            return "Pendente";
        }

        return "Submetida";
    }

    // GET: Candidaturas
    public async Task<IActionResult> Index(string? mensagem)
    {
        // Define a mensagem apenas se vier explicitamente no URL
        if (mensagem == "criado")
        {
            ViewBag.MensagemSucesso = "Candidatura criada com sucesso!";
        }
        else if (mensagem == "editado")
        {
            ViewBag.MensagemSucesso = "Candidatura atualizada com sucesso!";
        }
        else if (mensagem == "eliminado")
        {
            ViewBag.MensagemSucesso = "Candidatura eliminada com sucesso!";
        }

        var candidaturas = await _context.Candidaturas
            .Include(c => c.Estudante)
            .Include(c => c.Oferta)
            .ToListAsync();

        return View(candidaturas);
    }

    // GET: Candidaturas/Details
    public async Task<IActionResult> Details(int? idcandidatura)
    {
        if (idcandidatura == null)
        {
            return NotFound();
        }

        var candidatura = await _context.Candidaturas
            .Include(c => c.Estudante)
            .Include(c => c.Oferta)
            .FirstOrDefaultAsync(c => c.IdCandidatura == idcandidatura);

        if (candidatura == null)
        {
            return NotFound();
        }

        ViewBag.NomeAluno = candidatura.Estudante != null
            ? candidatura.Estudante.Nome
            : "Aluno não encontrado";

        ViewBag.NomeOferta = candidatura.Oferta != null
            ? candidatura.Oferta.Titulo
            : "Oferta não encontrada";

        return View(candidatura);
    }

    // GET: Candidaturas/Create
    public async Task<IActionResult> Create()
    {
        await CarregarListas();
        return View();
    }

    // POST: Candidaturas/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        int IdAluno,
        int IdOferta,
        DateTime DataCandidatura,
        string Curriculo)
    {
        var candidatura = new Candidatura
        {
            IdAluno = IdAluno,
            IdOferta = IdOferta,
            DataCandidatura = DataCandidatura,
            Curriculo = Curriculo ?? string.Empty
        };

        if (!_context.Estudantes.Any(a => a.IdAluno == IdAluno))
        {
            ViewBag.ErroFormulario = "Selecione um aluno válido.";
        }
        else if (!_context.Ofertas.Any(o => o.IdOferta == IdOferta))
        {
            ViewBag.ErroFormulario = "Selecione uma oferta válida.";
        }
        else if (ModelState.IsValid)
        {
            candidatura.Estado = CalcularEstado(DataCandidatura);

            _context.Candidaturas.Add(candidatura);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { mensagem = "criado" }); ;
        }

        await CarregarListas();
        return View(candidatura);
    }

    // GET: Candidaturas/Edit
    public async Task<IActionResult> Edit(int? idcandidatura)
    {
        if (idcandidatura == null)
        {
            return NotFound();
        }

        var candidatura = await _context.Candidaturas.FindAsync(idcandidatura);

        if (candidatura == null)
        {
            return NotFound();
        }

        await CarregarListas();
        return View(candidatura);
    }

    // POST: Candidaturas/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int? idcandidatura,
        int IdAluno,
        int IdOferta,
        DateTime DataCandidatura,
        string Curriculo)
    {
        if (idcandidatura == null)
        {
            return NotFound();
        }

        var candidatura = await _context.Candidaturas.FindAsync(idcandidatura);

        if (candidatura == null)
        {
            return NotFound();
        }

        if (!_context.Estudantes.Any(a => a.IdAluno == IdAluno))
        {
            ViewBag.ErroFormulario = "Selecione um aluno válido.";
        }
        else if (!_context.Ofertas.Any(o => o.IdOferta == IdOferta))
        {
            ViewBag.ErroFormulario = "Selecione uma oferta válida.";
        }
        else if (ModelState.IsValid)
        {
            candidatura.IdAluno = IdAluno;
            candidatura.IdOferta = IdOferta;
            candidatura.DataCandidatura = DataCandidatura;
            candidatura.Curriculo = Curriculo ?? string.Empty;
            candidatura.Estado = CalcularEstado(DataCandidatura);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CandidaturaExists(candidatura.IdCandidatura))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index), new { mensagem = "editado" });
        }

        await CarregarListas();
        return View(candidatura);
    }

    // GET: Candidaturas/Delete
    public async Task<IActionResult> Delete(int? idcandidatura)
    {
        if (idcandidatura == null)
        {
            return NotFound();
        }

        var candidatura = await _context.Candidaturas
            .Include(c => c.Estudante)
            .Include(c => c.Oferta)
            .FirstOrDefaultAsync(c => c.IdCandidatura == idcandidatura);

        if (candidatura == null)
        {
            return NotFound();
        }

        return View(candidatura);
    }

    // POST: Candidaturas/Delete
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? idcandidatura)
    {
        var candidatura = await _context.Candidaturas.FindAsync(idcandidatura);

        if (candidatura != null)
        {
            _context.Candidaturas.Remove(candidatura);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index), new { mensagem = "eliminado" });
    }

    private bool CandidaturaExists(int? idcandidatura)
    {
        return _context.Candidaturas.Any(c => c.IdCandidatura == idcandidatura);
    }
}

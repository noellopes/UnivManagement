using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class AvaliacaoUCController : Controller
{
    private readonly UniversityDbContext _context;

    public AvaliacaoUCController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: AvaliacaoUC
    public async Task<IActionResult> Index()
    {
        var avaliacoes = await _context.AvaliacoesUC
            .Include(a => a.TipoAvaliacao)
            .ToListAsync();

        return View(avaliacoes);
    }

    // GET: AvaliacaoUC/IndexDocente
    public async Task<IActionResult> IndexDocente(int? epocaId)
    {
        var avaliacoes = _context.AvaliacoesUC
            .Include(a => a.TipoAvaliacao)
            .AsQueryable();

        if (epocaId.HasValue)
        {
            avaliacoes = avaliacoes.Where(
                a => a.TipoAvaliacaoId == epocaId.Value
            );
        }

        ViewBag.Epocas = new SelectList(
            _context.TiposAvaliacao,
            "TipoAvaliacaoId",
            "TipoProva",
            epocaId
        );

        return View(await avaliacoes.ToListAsync());
    }

    // GET: AvaliacaoUC/IndexAluno
    public async Task<IActionResult> IndexAluno()
    {
        var avaliacoes = await _context.AvaliacoesUC
            .Include(a => a.TipoAvaliacao)
            .ToListAsync();

        return View(avaliacoes);
    }

    // GET: AvaliacaoUC/DetailsAluno/5
    public async Task<IActionResult> DetailsAluno(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var avaliacao = await _context.AvaliacoesUC
            .Include(a => a.TipoAvaliacao)
            .FirstOrDefaultAsync(a => a.AvaliacaoUCId == id);

        if (avaliacao == null)
        {
            return NotFound();
        }

        return View(avaliacao);
    }

    // GET: AvaliacaoUC/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var avaliacao = await _context.AvaliacoesUC
            .Include(a => a.TipoAvaliacao)
            .FirstOrDefaultAsync(a => a.AvaliacaoUCId == id);

        if (avaliacao == null)
        {
            return NotFound();
        }

        return View(avaliacao);
    }

    // GET: AvaliacaoUC/Create
    public IActionResult Create(int? epocaId)
    {
        PreencherListas(epocaId);
        return View();
    }

    // POST: AvaliacaoUC/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("AvaliacaoUCId,TipoAvaliacaoId,UcId,Tipo,Avaliacao,Data")]
        AvaliacaoUC avaliacaouc)
    {
        ValidarTipo(avaliacaouc.Tipo);

        if (ModelState.IsValid)
        {
            _context.AvaliacoesUC.Add(avaliacaouc);
            await _context.SaveChangesAsync();

            return RedirectToAction(
                nameof(IndexDocente),
                new { epocaId = avaliacaouc.TipoAvaliacaoId }
            );
        }

        PreencherListas(
            avaliacaouc.TipoAvaliacaoId,
            avaliacaouc.Tipo,
            avaliacaouc.UcId
        );

        return View(avaliacaouc);
    }

    // GET: AvaliacaoUC/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var avaliacao = await _context.AvaliacoesUC.FindAsync(id);

        if (avaliacao == null)
        {
            return NotFound();
        }

        PreencherListas(
            avaliacao.TipoAvaliacaoId,
            avaliacao.Tipo,
            avaliacao.UcId
        );

        return View(avaliacao);
    }

    // POST: AvaliacaoUC/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("AvaliacaoUCId,TipoAvaliacaoId,UcId,Tipo,Avaliacao,Data")]
        AvaliacaoUC avaliacao)
    {
        if (id != avaliacao.AvaliacaoUCId)
        {
            return NotFound();
        }

        ValidarTipo(avaliacao.Tipo);

        if (ModelState.IsValid)
        {
            try
            {
                _context.AvaliacoesUC.Update(avaliacao);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AvaliacaoUCExists(avaliacao.AvaliacaoUCId))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(
                nameof(IndexDocente),
                new { epocaId = avaliacao.TipoAvaliacaoId }
            );
        }

        PreencherListas(
            avaliacao.TipoAvaliacaoId,
            avaliacao.Tipo,
            avaliacao.UcId
        );

        return View(avaliacao);
    }

    // GET: AvaliacaoUC/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var avaliacao = await _context.AvaliacoesUC
            .Include(a => a.TipoAvaliacao)
            .FirstOrDefaultAsync(a => a.AvaliacaoUCId == id);

        if (avaliacao == null)
        {
            return NotFound();
        }

        return View(avaliacao);
    }

    // POST: AvaliacaoUC/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var avaliacao = await _context.AvaliacoesUC
            .Include(a => a.TipoAvaliacao)
            .FirstOrDefaultAsync(a => a.AvaliacaoUCId == id);

        if (avaliacao == null)
        {
            return NotFound();
        }

        // Impede eliminar avaliações que já tenham notas associadas
        if (await _context.Notas.AnyAsync(n => n.AvaliacaoUCId == id))
        {
            ViewBag.Erro =
                "Não pode eliminar esta avaliação porque existem notas associadas.";

            return View("Delete", avaliacao);
        }

        _context.AvaliacoesUC.Remove(avaliacao);
        await _context.SaveChangesAsync();

        return RedirectToAction(
            nameof(IndexDocente),
            new { epocaId = avaliacao.TipoAvaliacaoId }
        );
    }

    // Preenche as listas dos formulários
    private void PreencherListas(
        int? epocaId = null,
        string? tipoSelecionado = null,
        int? ucId = null)
    {
        // Épocas
        ViewBag.TiposAvaliacao = new SelectList(
            _context.TiposAvaliacao,
            "TipoAvaliacaoId",
            "TipoProva",
            epocaId
        );

        // Tipos de avaliação
        ViewBag.Tipos = new SelectList(
            new[] { "Frequência", "Exame", "Apresentação" }
        );


        // Unidades Curriculares
        ViewBag.UnidadesCurriculares = new SelectList(
            _context.UC,
            "UcId",
            "Name",
            ucId
        );
    }

    // Valida os tipos de avaliação permitidos
    private void ValidarTipo(string? tipo)
    {
        string[] tiposPermitidos =
        {
            "Frequência",
            "Exame",
            "Apresentação"
        };

        if (!tiposPermitidos.Contains(tipo))
        {
            ModelState.AddModelError(
                nameof(AvaliacaoUC.Tipo),
                "Selecione um tipo de avaliação válido."
            );
        }
    }

    // Verifica se uma avaliação existe
    private bool AvaliacaoUCExists(int id)
    {
        return _context.AvaliacoesUC
            .Any(a => a.AvaliacaoUCId == id);
    }
}

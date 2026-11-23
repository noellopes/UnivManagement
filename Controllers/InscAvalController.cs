using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Models.AcademicManagement;
using UnivManagement.Data;

public class InscAvalController : Controller
{
    private readonly UniversityDbContext _context;

    public InscAvalController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: InscAval
    public async Task<IActionResult> Index()
    {
        var inscricoes = await _context.InscAval
            .Include(i => i.AvaliacaoUC)
                .ThenInclude(a => a!.TipoAvaliacao)
            .Include(i => i.InscrUC)
            .ToListAsync();

        ViewBag.TextosAvaliacao = inscricoes.ToDictionary(
            i => i.InscAvalId,
            i => TextoDaAvaliacao(i.AvaliacaoUC));
        ViewBag.TextosInscricao = inscricoes.ToDictionary(
            i => i.InscAvalId,
            i => i.InscrUC != null ? TextoDaInscricao(i.InscrUC) : ("Nº " + i.InscrUcId));

        return View(inscricoes);
    }

    // GET: InscAval/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var inscAval = await _context.InscAval
            .Include(i => i.AvaliacaoUC)
                .ThenInclude(a => a!.TipoAvaliacao)
            .Include(i => i.InscrUC)
            .FirstOrDefaultAsync(i => i.InscAvalId == id);

        if (inscAval == null)
        {
            return NotFound();
        }

        PreencherTextos(inscAval);
        return View(inscAval);
    }

    // GET: InscAval/Create
    public IActionResult Create()
    {
        PreencherListas();
        return View();
    }

    // POST: InscAval/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("AvaliacaoUCId,InscrUcId")] InscAval novaInscAval)
    {
        if (ModelState.IsValid)
        {
            novaInscAval.Pago = false;
            _context.InscAval.Add(novaInscAval);
            await _context.SaveChangesAsync();
            TempData["Mensagem"] = "A inscrição será concluída depois do pagamento ser efetuado.";
            return RedirectToAction(nameof(Index));
        }

        PreencherListas(novaInscAval.AvaliacaoUCId, novaInscAval.InscrUcId);
        return View(novaInscAval);
    }

    // GET: InscAval/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var inscAval = await _context.InscAval.FindAsync(id);

        if (inscAval == null)
        {
            return NotFound();
        }

        PreencherListas(inscAval.AvaliacaoUCId, inscAval.InscrUcId);
        return View(inscAval);
    }

    // POST: InscAval/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("InscAvalId,AvaliacaoUCId,InscrUcId,Pago")] InscAval inscAvalEditada)
    {
        if (id != inscAvalEditada.InscAvalId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.InscAval.Update(inscAvalEditada);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InscAvalExiste(inscAvalEditada.InscAvalId))
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

        PreencherListas(inscAvalEditada.AvaliacaoUCId, inscAvalEditada.InscrUcId);
        return View(inscAvalEditada);
    }

    // GET: InscAval/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var inscAval = await _context.InscAval
            .Include(i => i.AvaliacaoUC)
                .ThenInclude(a => a!.TipoAvaliacao)
            .Include(i => i.InscrUC)
            .FirstOrDefaultAsync(i => i.InscAvalId == id);

        if (inscAval == null)
        {
            return NotFound();
        }

        PreencherTextos(inscAval);
        return View(inscAval);
    }

    // POST: InscAval/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var inscAval = await _context.InscAval.FindAsync(id);

        if (inscAval != null)
        {
            _context.InscAval.Remove(inscAval);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool InscAvalExiste(int id)
    {
        return _context.InscAval.Any(i => i.InscAvalId == id);
    }

    // Textos da avaliação e da inscrição para as páginas de detalhes e de apagar
    private void PreencherTextos(InscAval inscAval)
    {
        ViewBag.TextoAvaliacao = TextoDaAvaliacao(inscAval.AvaliacaoUC);
        ViewBag.TextoInscricao = inscAval.InscrUC != null ? TextoDaInscricao(inscAval.InscrUC) : ("Nº " + inscAval.InscrUcId);
    }

    // Listas para escolher a avaliação e a inscrição nos formulários
    private void PreencherListas(int? avaliacaoEscolhida = null, int? inscricaoEscolhida = null)
    {
        var avaliacoes = _context.AvaliacoesUC
            .Include(a => a.TipoAvaliacao)
            .OrderBy(a => a.Data)
            .ToList()
            .Select(a => new
            {
                a.AvaliacaoUCId,
                Texto = TextoDaAvaliacao(a)
            })
            .ToList();

        var inscricoes = _context.InscrUC
            .ToList()
            .Select(i => new
            {
                i.InscrUCId,
                Texto = TextoDaInscricao(i)
            })
            .ToList();

        ViewData["AvaliacaoUCId"] = new SelectList(avaliacoes, "AvaliacaoUCId", "Texto", avaliacaoEscolhida);
        ViewData["InscrUcId"] = new SelectList(inscricoes, "InscrUCId", "Texto", inscricaoEscolhida);
    }

    // Texto da avaliação: tipo, nome da UC e data
    private string TextoDaAvaliacao(AvaliacaoUC? avaliacao)
    {
        if (avaliacao == null)
        {
            return "";
        }

        var tipo = avaliacao.TipoAvaliacao?.TipoProva ?? ("Avaliação " + avaliacao.AvaliacaoUCId);
        var uc = _context.UC.FirstOrDefault(u => u.UcId == avaliacao.UcId);
        var nomeUc = uc != null ? uc.Name : ("UC " + avaliacao.UcId);

        return tipo + " - " + nomeUc + " - " + avaliacao.Data.ToString("dd/MM/yyyy");
    }

    // Texto da inscrição: nome do aluno e nome da UC, se existirem
    private string TextoDaInscricao(InscrUC inscricao)
    {
        var uc = _context.UC.FirstOrDefault(u => u.UcId == inscricao.UcId);
        var inscricaoCurso = _context.InscrCourse.FirstOrDefault(c => c.InscrCourseId == inscricao.InscrCourseId);
        var aluno = inscricaoCurso == null ? null : _context.Student.FirstOrDefault(s => s.StudentId == inscricaoCurso.StudentId);

        if (aluno != null && uc != null)
        {
            return aluno.Name + " - " + uc.Name;
        }

        if (uc != null)
        {
            return "Nº " + inscricao.InscrUCId + " - " + uc.Name;
        }

        return "Nº " + inscricao.InscrUCId;
    }
}

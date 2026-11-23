
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class NotasController : Controller
{
    private readonly UniversityDbContext _context;

    public NotasController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: Notas
    public async Task<IActionResult> Index()
    {
        var notas = await _context.Notas
            .Include(n => n.AvaliacaoUC)
                .ThenInclude(a => a!.TipoAvaliacao)
            .Include(n => n.InscrUC)
            .ToListAsync();

        return View(notas);
    }

    // GET: Notas/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var nota = await _context.Notas
            .Include(n => n.AvaliacaoUC)
                .ThenInclude(a => a!.TipoAvaliacao)
            .Include(n => n.InscrUC)
            .FirstOrDefaultAsync(n => n.NotaId == id);

        if (nota == null)
        {
            return NotFound();
        }

        return View(nota);
    }

    // GET: Notas/Create
    public IActionResult Create()
    {
        PreencherListas();
        return View();
    }

    // POST: Notas/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("NotaId,AvaliacaoUCId,InscrUcId,Data,Valor")] Nota novaNota)
    {
        // Aprovado quando a nota e 9,5 ou mais
        novaNota.Aprovado = novaNota.Valor >= 9.5f;

        // A data da nota nao pode ser antes do dia de hoje
        if (novaNota.Data.Date < DateTime.Today)
        {
            ModelState.AddModelError("Data", "A data da nota não pode ser anterior à data de hoje.");
        }

        // Verificar se o aluno ja tem nota nesta avaliacao
        if (_context.Notas.Any(n => n.AvaliacaoUCId == novaNota.AvaliacaoUCId && n.InscrUcId == novaNota.InscrUcId))
        {
            ModelState.AddModelError("InscrUcId", "Este aluno já tem uma nota nesta avaliação.");
        }

        if (ModelState.IsValid)
        {
            _context.Notas.Add(novaNota);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        PreencherListas(novaNota.AvaliacaoUCId, novaNota.InscrUcId);
        return View(novaNota);
    }

    // GET: Notas/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var nota = await _context.Notas.FindAsync(id);

        if (nota == null)
        {
            return NotFound();
        }

        PreencherListas(nota.AvaliacaoUCId, nota.InscrUcId);
        return View(nota);
    }

    // POST: Notas/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("NotaId,AvaliacaoUCId,InscrUcId,Data,Valor")] Nota notaEditada)
    {
        if (id != notaEditada.NotaId)
        {
            return NotFound();
        }

        // Aprovado quando a nota e 9,5 ou mais
        notaEditada.Aprovado = notaEditada.Valor >= 9.5f;

        // Verificar se o aluno ja tem outra nota nesta avaliacao
        if (_context.Notas.Any(n => n.AvaliacaoUCId == notaEditada.AvaliacaoUCId && n.InscrUcId == notaEditada.InscrUcId && n.NotaId != notaEditada.NotaId))
        {
            ModelState.AddModelError("InscrUcId", "Este aluno já tem uma nota nesta avaliação.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Notas.Update(notaEditada);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotaExiste(notaEditada.NotaId))
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

        PreencherListas(notaEditada.AvaliacaoUCId, notaEditada.InscrUcId);
        return View(notaEditada);
    }

    // GET: Notas/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var nota = await _context.Notas
            .Include(n => n.AvaliacaoUC)
                .ThenInclude(a => a!.TipoAvaliacao)
            .Include(n => n.InscrUC)
            .FirstOrDefaultAsync(n => n.NotaId == id);

        if (nota == null)
        {
            return NotFound();
        }

        return View(nota);
    }

    // POST: Notas/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var nota = await _context.Notas.FindAsync(id);

        if (nota != null)
        {
            _context.Notas.Remove(nota);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool NotaExiste(int id)
    {
        return _context.Notas.Any(n => n.NotaId == id);
    }

    // Listas para escolher a avaliação e a inscrição nos formulários
    private void PreencherListas(int? avaliacaoEscolhida = null, int? inscricaoEscolhida = null)
    {
        var avaliacoes = _context.AvaliacoesUC
            .Include(a => a.TipoAvaliacao)
            .OrderBy(a => a.Data)
            .Select(a => new
            {
                a.AvaliacaoUCId,
                Texto = a.TipoAvaliacao.TipoProva + " - UC " + a.UcId + " - " + a.Data.ToString("dd/MM/yyyy")
            })
            .ToList();

        ViewData["AvaliacaoUCId"] = new SelectList(avaliacoes, "AvaliacaoUCId", "Texto", avaliacaoEscolhida);
        ViewData["InscrUcId"] = new SelectList(_context.InscrUC, "InscrUCId", "InscrUCId", inscricaoEscolhida);
    }
}

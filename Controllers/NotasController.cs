
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
            .FirstOrDefaultAsync(n => n.NotaID == id);

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
    public async Task<IActionResult> Create([Bind("NotaID,AvaliacaoID,InscrUcID,Data,Nota")] Notas novaNota)
    {
        // Aprovado quando a nota e 9,5 ou mais
        novaNota.Aprovado = novaNota.Nota >= 9.5f;

        if (ModelState.IsValid)
        {
            _context.Notas.Add(novaNota);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        PreencherListas(novaNota.AvaliacaoID, novaNota.InscrUcID);
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

        PreencherListas(nota.AvaliacaoID, nota.InscrUcID);
        return View(nota);
    }

    // POST: Notas/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("NotaID,AvaliacaoID,InscrUcID,Data,Nota")] Notas notaEditada)
    {
        if (id != notaEditada.NotaID)
        {
            return NotFound();
        }

        // Aprovado quando a nota e 9,5 ou mais
        notaEditada.Aprovado = notaEditada.Nota >= 9.5f;

        if (ModelState.IsValid)
        {
            try
            {
                _context.Notas.Update(notaEditada);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotaExiste(notaEditada.NotaID))
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

        PreencherListas(notaEditada.AvaliacaoID, notaEditada.InscrUcID);
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
            .FirstOrDefaultAsync(n => n.NotaID == id);

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
        return _context.Notas.Any(n => n.NotaID == id);
    }

    // Listas para escolher a avaliação e a inscrição nos formulários
    private void PreencherListas(int? avaliacaoEscolhida = null, int? inscricaoEscolhida = null)
    {
        var avaliacoes = _context.AvaliacoesUC
            .Include(a => a.TipoAvaliacao)
            .OrderBy(a => a.Data)
            .Select(a => new
            {
                a.AvaliacaoID,
                Texto = a.TipoAvaliacao.Tipo_Prova + " - UC " + a.UcID + " - " + a.Data.ToString("dd/MM/yyyy")
            })
            .ToList();

        ViewData["AvaliacaoID"] = new SelectList(avaliacoes, "AvaliacaoID", "Texto", avaliacaoEscolhida);
        ViewData["InscrUcID"] = new SelectList(_context.InscrUC, "InscrUCId", "InscrUCId", inscricaoEscolhida);
    }
}

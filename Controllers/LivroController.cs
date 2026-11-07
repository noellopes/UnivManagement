
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

public class LivroController : Controller
{
    private readonly UniversityDbContext _context;

    public LivroController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: Livro
    public async Task<IActionResult> Index()
    {
        var livros = await _context.Livros
            .Include(l => l.LivrosAutores)
                .ThenInclude(la => la.Autor)
            .ToListAsync();

        return View(livros);
    }

    // GET: Livro/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var livro = await _context.Livros
            .Include(l => l.LivrosAutores)
                .ThenInclude(la => la.Autor)
            .FirstOrDefaultAsync(l => l.LivroId == id);

        if (livro == null)
            return NotFound();

        return View(livro);
    }

    // GET: Livro/Create
    public async Task<IActionResult> Create()
    {
        await CarregarAutores();
        return View();
    }

    // POST: Livro/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("ISBN,Titulo,Tipo,ValorMulta")] Livro livro,
        List<int> autoresSelecionados)
    {
        await ValidarAutores(autoresSelecionados);

        if (ModelState.IsValid)
        {
            foreach (var autorId in autoresSelecionados.Distinct())
            {
                livro.LivrosAutores.Add(new LivroAutor
                {
                    AutorId = autorId
                });
            }

            _context.Livros.Add(livro);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        await CarregarAutores(autoresSelecionados);
        return View(livro);
    }

    // GET: Livro/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var livro = await _context.Livros
            .Include(l => l.LivrosAutores)
            .FirstOrDefaultAsync(l => l.LivroId == id);

        if (livro == null)
            return NotFound();

        await CarregarAutores(
            livro.LivrosAutores.Select(la => la.AutorId));

        return View(livro);
    }

    // POST: Livro/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int? id,
        [Bind("LivroId,ISBN,Titulo,Tipo,ValorMulta")] Livro dados,
        List<int> autoresSelecionados)
    {
        if (id == null || id != dados.LivroId)
            return NotFound();

        var livro = await _context.Livros
            .Include(l => l.LivrosAutores)
            .FirstOrDefaultAsync(l => l.LivroId == id);

        if (livro == null)
            return NotFound();

        await ValidarAutores(autoresSelecionados);

        if (ModelState.IsValid)
        {
            livro.ISBN = dados.ISBN;
            livro.Titulo = dados.Titulo;
            livro.Tipo = dados.Tipo;
            livro.ValorMulta = dados.ValorMulta;

            var idsNovos = autoresSelecionados
                .Distinct()
                .ToHashSet();

            var associacoesRemover = livro.LivrosAutores
                .Where(la => !idsNovos.Contains(la.AutorId))
                .ToList();

            _context.LivrosAutores.RemoveRange(
                associacoesRemover);

            var idsAtuais = livro.LivrosAutores
                .Select(la => la.AutorId)
                .ToHashSet();

            foreach (var autorId in idsNovos.Except(idsAtuais))
            {
                livro.LivrosAutores.Add(new LivroAutor
                {
                    AutorId = autorId
                });
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        await CarregarAutores(autoresSelecionados);
        return View(dados);
    }

    // GET: Livro/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var livro = await _context.Livros
            .Include(l => l.LivrosAutores)
                .ThenInclude(la => la.Autor)
            .FirstOrDefaultAsync(l => l.LivroId == id);

        if (livro == null)
            return NotFound();

        return View(livro);
    }

    // POST: Livro/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        if (id == null)
            return NotFound();

        var livro = await _context.Livros.FindAsync(id);

        if (livro == null)
            return NotFound();

        _context.Livros.Remove(livro);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private async Task CarregarAutores(
        IEnumerable<int>? selecionados = null)
    {
        var autores = await _context.Autores
            .OrderBy(a => a.Nome)
            .ToListAsync();

        ViewBag.Autores = new MultiSelectList(
            autores,
            "AutorId",
            "Nome",
            selecionados
        );
    }

    private async Task ValidarAutores(
        List<int> autoresSelecionados)
    {
        var ids = autoresSelecionados
            .Distinct()
            .ToList();

        if (ids.Count == 0)
        {
            ModelState.AddModelError(
                "autoresSelecionados",
                "Selecione pelo menos um autor.");

            return;
        }

        var quantidadeExistente = await _context.Autores
            .CountAsync(a => ids.Contains(a.AutorId));

        if (quantidadeExistente != ids.Count)
        {
            ModelState.AddModelError(
                "autoresSelecionados",
                "Um ou mais autores selecionados são inválidos.");
        }
    }
}

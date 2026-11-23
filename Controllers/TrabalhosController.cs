using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;
using UnivManagement.Models.AcademicManagement;

public class TrabalhosController : Controller
{
    private readonly UniversityDbContext _context;

    public TrabalhosController(UniversityDbContext context)
    {
        _context = context;
    }

    // Mantém a listagem existente.
    public async Task<IActionResult> Index(string? tipo, string? pesquisa)
    {
        var trabalhos = await CarregarTrabalhosAsync(tipo, pesquisa);
        return View(trabalhos);
    }

    // Listagem da interface do docente.
    public async Task<IActionResult> IndexDocente(
        string? tipo,
        string? pesquisa)
    {
        var trabalhos = await CarregarTrabalhosAsync(tipo, pesquisa);
        return View(trabalhos);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var trabalho = await _context.Trabalhos
            .AsNoTracking()
            .Include(t => t.Docente)
            .Include(t => t.UC)
            .FirstOrDefaultAsync(t => t.TrabalhoId == id);

        if (trabalho == null)
        {
            return NotFound();
        }

        if (!trabalho.EIndividual)
        {
            ViewData["GruposTrabalho"] = await _context.GruposTrabalho
                .AsNoTracking()
                .Where(g => g.TrabalhoId == trabalho.TrabalhoId)
                .OrderBy(g => g.Nome)
                .ToListAsync();
        }

        return View(trabalho);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await CarregarListasAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("DocenteId,UcId,EIndividual,Titulo,Descricao,DataLimite")]
        Trabalho trabalho)
    {
        await ValidarReferenciasAsync(trabalho);

        if (!ModelState.IsValid)
        {
            await CarregarListasAsync();
            return View(trabalho);
        }

        _context.Trabalhos.Add(trabalho);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(IndexDocente));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var trabalho = await _context.Trabalhos.FindAsync(id.Value);

        if (trabalho == null)
        {
            return NotFound();
        }

        await CarregarListasAsync();
        return View(trabalho);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int? id,
        [Bind("TrabalhoId,DocenteId,UcId,EIndividual,Titulo,Descricao,DataLimite")]
        Trabalho trabalho)
    {
        if (id == null || id.Value != trabalho.TrabalhoId)
        {
            return NotFound();
        }

        var atual = await _context.Trabalhos.FindAsync(id.Value);

        if (atual == null)
        {
            return NotFound();
        }

        var mudouUc = atual.UcId != trabalho.UcId;
        var passouAIndividual = !atual.EIndividual && trabalho.EIndividual;

        if ((mudouUc || passouAIndividual) &&
            await _context.GruposTrabalho.AnyAsync(
                g => g.TrabalhoId == atual.TrabalhoId))
        {
            if (mudouUc)
            {
                ModelState.AddModelError(
                    nameof(Trabalho.UcId),
                    "Elimina os grupos antes de mudar a unidade curricular.");
            }

            if (passouAIndividual)
            {
                ModelState.AddModelError(
                    nameof(Trabalho.EIndividual),
                    "Elimina os grupos antes de mudar o trabalho para individual.");
            }
        }

        await ValidarReferenciasAsync(trabalho);

        if (!ModelState.IsValid)
        {
            await CarregarListasAsync();
            return View(trabalho);
        }

        atual.DocenteId = trabalho.DocenteId;
        atual.UcId = trabalho.UcId;
        atual.EIndividual = trabalho.EIndividual;
        atual.Titulo = trabalho.Titulo;
        atual.Descricao = trabalho.Descricao;
        atual.DataLimite = trabalho.DataLimite;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Trabalhos.AnyAsync(
                t => t.TrabalhoId == id.Value))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToAction(nameof(IndexDocente));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var trabalho = await _context.Trabalhos
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.TrabalhoId == id);

        if (trabalho == null)
        {
            return NotFound();
        }

        return View(trabalho);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var trabalho = await _context.Trabalhos.FindAsync(id);

        if (trabalho == null)
        {
            return NotFound();
        }

        _context.Trabalhos.Remove(trabalho);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(IndexDocente));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CriarGrupo(
        int trabalhoId,
        string? nome)
    {
        var trabalho = await _context.Trabalhos.FindAsync(trabalhoId);

        if (trabalho == null || trabalho.EIndividual)
        {
            return NotFound();
        }

        nome = nome?.Trim();

        if (string.IsNullOrEmpty(nome) || nome.Length > 100)
        {
            TempData["GrupoErro"] =
                "Indica um nome para o grupo com até 100 caracteres.";

            return RedirectToAction(
                nameof(Details),
                new { id = trabalhoId });
        }

        _context.GruposTrabalho.Add(new GrupoTrabalho
        {
            TrabalhoId = trabalhoId,
            Nome = nome
        });

        await _context.SaveChangesAsync();

        TempData["GrupoCriado"] = $"O grupo «{nome}» foi criado.";

        return RedirectToAction(
            nameof(Details),
            new { id = trabalhoId });
    }

    [HttpGet]
    public async Task<IActionResult> GerirMembros(int grupoId)
    {
        var grupo = await _context.GruposTrabalho
            .AsNoTracking()
            .Include(g => g.Trabalho)
            .FirstOrDefaultAsync(g => g.GrupoTrabalhoId == grupoId);

        if (grupo == null ||
            grupo.Trabalho == null ||
            grupo.Trabalho.EIndividual)
        {
            return NotFound();
        }

        ViewData["Membros"] = await _context.GruposTrabalhoMembros
            .AsNoTracking()
            .Include(m => m.Student)
            .Where(m => m.GrupoTrabalhoId == grupoId)
            .OrderBy(m => m.Student == null ? "" : m.Student.Name)
            .ToListAsync();

        ViewData["AlunosDisponiveis"] =
            await AlunosInscritosNaUc(grupo.Trabalho.UcId)
                .Where(a => !_context.GruposTrabalhoMembros.Any(m =>
                    m.StudentId == a.StudentId &&
                    m.GrupoTrabalho != null &&
                    m.GrupoTrabalho.TrabalhoId == grupo.TrabalhoId))
                .OrderBy(a => a.Name)
                .ToListAsync();

        return View(grupo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AdicionarMembro(
        int grupoId,
        int studentId)
    {
        var grupo = await _context.GruposTrabalho
            .Include(g => g.Trabalho)
            .FirstOrDefaultAsync(g => g.GrupoTrabalhoId == grupoId);

        if (grupo == null ||
            grupo.Trabalho == null ||
            grupo.Trabalho.EIndividual)
        {
            return NotFound();
        }

        var inscrito = await AlunosInscritosNaUc(grupo.Trabalho.UcId)
            .AnyAsync(a => a.StudentId == studentId);

        if (!inscrito)
        {
            TempData["MembroErro"] =
                "Seleciona um aluno inscrito na UC do trabalho.";

            return RedirectToAction(
                nameof(GerirMembros),
                new { grupoId });
        }

        var jaTemGrupo = await _context.GruposTrabalhoMembros
            .AnyAsync(m =>
                m.StudentId == studentId &&
                m.GrupoTrabalho != null &&
                m.GrupoTrabalho.TrabalhoId == grupo.TrabalhoId);

        if (jaTemGrupo)
        {
            TempData["MembroErro"] =
                "Este aluno já pertence a um grupo deste trabalho.";

            return RedirectToAction(
                nameof(GerirMembros),
                new { grupoId });
        }

        _context.GruposTrabalhoMembros.Add(new GrupoTrabalhoMembro
        {
            GrupoTrabalhoId = grupoId,
            StudentId = studentId
        });

        await _context.SaveChangesAsync();

        return RedirectToAction(
            nameof(GerirMembros),
            new { grupoId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoverMembro(
        int grupoId,
        int membroId)
    {
        var grupoValido = await _context.GruposTrabalho.AnyAsync(g =>
            g.GrupoTrabalhoId == grupoId &&
            g.Trabalho != null &&
            !g.Trabalho.EIndividual);

        if (!grupoValido)
        {
            return NotFound();
        }

        var membro = await _context.GruposTrabalhoMembros
            .FirstOrDefaultAsync(m =>
                m.GrupoTrabalhoId == grupoId &&
                m.GrupoTrabalhoMembroId == membroId);

        if (membro == null)
        {
            TempData["MembroErro"] =
                "O aluno já não pertence a este grupo.";

            return RedirectToAction(
                nameof(GerirMembros),
                new { grupoId });
        }

        _context.GruposTrabalhoMembros.Remove(membro);
        await _context.SaveChangesAsync();

        return RedirectToAction(
            nameof(GerirMembros),
            new { grupoId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RenomearGrupo(
        int grupoId,
        string? nome)
    {
        var grupo = await _context.GruposTrabalho
            .Include(g => g.Trabalho)
            .FirstOrDefaultAsync(g => g.GrupoTrabalhoId == grupoId);

        if (grupo == null ||
            grupo.Trabalho == null ||
            grupo.Trabalho.EIndividual)
        {
            return NotFound();
        }

        nome = nome?.Trim();

        if (string.IsNullOrEmpty(nome) || nome.Length > 100)
        {
            TempData["MembroErro"] =
                "Indica um nome para o grupo com até 100 caracteres.";

            return RedirectToAction(
                nameof(GerirMembros),
                new { grupoId });
        }

        grupo.Nome = nome;
        await _context.SaveChangesAsync();

        return RedirectToAction(
            nameof(GerirMembros),
            new { grupoId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarGrupo(int grupoId)
    {
        var grupo = await _context.GruposTrabalho
            .Include(g => g.Trabalho)
            .FirstOrDefaultAsync(g => g.GrupoTrabalhoId == grupoId);

        if (grupo == null ||
            grupo.Trabalho == null ||
            grupo.Trabalho.EIndividual)
        {
            return NotFound();
        }

        var temMembros = await _context.GruposTrabalhoMembros
            .AnyAsync(m => m.GrupoTrabalhoId == grupoId);

        if (temMembros)
        {
            TempData["MembroErro"] =
                "Remove os membros antes de eliminar o grupo.";

            return RedirectToAction(
                nameof(GerirMembros),
                new { grupoId });
        }

        var trabalhoId = grupo.TrabalhoId;
        var nome = grupo.Nome;

        _context.GruposTrabalho.Remove(grupo);
        await _context.SaveChangesAsync();

        TempData["GrupoEliminado"] =
            $"O grupo «{nome}» foi eliminado.";

        return RedirectToAction(
            nameof(Details),
            new { id = trabalhoId });
    }

    private async Task<List<Trabalho>> CarregarTrabalhosAsync(
        string? tipo,
        string? pesquisa)
    {
        var trabalhos = _context.Trabalhos
            .AsNoTracking()
            .Include(t => t.Docente)
            .Include(t => t.UC)
            .Where(t => true);

        if (tipo == "individual")
        {
            trabalhos = trabalhos.Where(t => t.EIndividual);
        }
        else if (tipo == "grupo")
        {
            trabalhos = trabalhos.Where(t => !t.EIndividual);
        }

        pesquisa = pesquisa?.Trim();

        if (!string.IsNullOrEmpty(pesquisa))
        {
            trabalhos = trabalhos.Where(
                t => t.Titulo.Contains(pesquisa));
        }

        ViewData["TipoSelecionado"] = tipo;
        ViewData["Pesquisa"] = pesquisa;

        return await trabalhos.ToListAsync();
    }

    private IQueryable<Student> AlunosInscritosNaUc(int ucId)
    {
        var idsDosAlunos =
            from inscricaoUc in _context.InscrUC
            join inscricaoCurso in _context.InscrCourse
                on inscricaoUc.InscrCourseId
                equals inscricaoCurso.InscrCourseId
            where inscricaoUc.UcId == ucId
            select inscricaoCurso.StudentId;

        return _context.Student
            .AsNoTracking()
            .Where(a => idsDosAlunos.Contains(a.StudentId));
    }

    private async Task CarregarListasAsync()
    {
        var docentes = await _context.Set<Docente>()
            .AsNoTracking()
            .OrderBy(d => d.Nome)
            .ToListAsync();

        var ucs = await _context.UC
            .AsNoTracking()
            .OrderBy(uc => uc.Name)
            .ToListAsync();

        ViewBag.Docentes = new SelectList(
            docentes,
            "DocenteId",
            "Nome");

        ViewBag.Ucs = new SelectList(
            ucs,
            "UcId",
            "Name");
    }

    private async Task ValidarReferenciasAsync(Trabalho trabalho)
    {
        if (!await _context.Set<Docente>().AnyAsync(
            d => d.DocenteId == trabalho.DocenteId))
        {
            ModelState.AddModelError(
                nameof(Trabalho.DocenteId),
                "Indica um docente existente.");
        }

        if (!await _context.UC.AnyAsync(
            uc => uc.UcId == trabalho.UcId))
        {
            ModelState.AddModelError(
                nameof(Trabalho.UcId),
                "Indica uma unidade curricular existente.");
        }
    }
}
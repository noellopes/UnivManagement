using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

public class FuncionarioController : Controller
{
    private readonly UniversityDbContext _context;

    public FuncionarioController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: FUNCIONARIOS
    public async Task<IActionResult> Index()
    {
        // O .Include(f => f.Servico) garante que os dados do ServicoFuncionario vêm preenchidos da BD
        var funcionarios = await _context.Funcionario
            .Include(f => f.Servico)
            .ToListAsync();

        return View(funcionarios);
    }

    // GET: FUNCIONARIOS/Details/5
    [HttpGet("/Funcionario/Details/{funcionarioid}")]
    public async Task<IActionResult> Details(int? funcionarioid)
    {
        if (funcionarioid == null)
        {
            return NotFound();
        }

        var funcionario = await _context.Funcionario
            .Include(f => f.Servico) // Carrega os dados do serviço
            .FirstOrDefaultAsync(m => m.FuncionarioId == funcionarioid);

        if (funcionario == null)
        {
            return NotFound();
        }

        return View(funcionario);
    }

    // GET: Funcionarios/Create
    public IActionResult Create()
    {
        // Carrega todos os serviços disponíveis sem filtrar por 'Ativo' para evitar a dropdown vazia
        var servicos = _context.ServicoFuncionario.ToList();
        ViewData["ServicoFuncionarioId"] = new SelectList(servicos, "ServicoFuncionarioId", "Nome");

        return View();
    }

    // POST: Funcionarios/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("FuncionarioId,Nome,Nif,Email,Telefone,Ativo,DataAdmissao,Morada,ServicoFuncionarioId")] Funcionario funcionario)
    {
        if (ModelState.IsValid)
        {
            _context.Add(funcionario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Se o formulário tiver erros, recarrega a lista para a dropdown não ficar vazia
        var servicos = _context.ServicoFuncionario.ToList();
        ViewData["ServicoFuncionarioId"] = new SelectList(servicos, "ServicoFuncionarioId", "Nome", funcionario.ServicoFuncionarioId);

        return View(funcionario);
    }

    // GET: Funcionarios/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var funcionario = await _context.Funcionario.FindAsync(id);
        if (funcionario == null)
        {
            return NotFound();
        }

        // Preenche o SelectList e pré-seleciona o serviço atual do funcionário
        var servicos = _context.ServicoFuncionario.ToList();
        ViewData["ServicoFuncionarioId"] = new SelectList(servicos, "ServicoFuncionarioId", "Nome", funcionario.ServicoFuncionarioId);

        return View(funcionario);
    }

    // POST: Funcionarios/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("FuncionarioId,Nome,Nif,Email,Telefone,Ativo,DataAdmissao,Morada,ServicoFuncionarioId")] Funcionario funcionario)
    {
        if (id != funcionario.FuncionarioId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(funcionario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FuncionarioExists(funcionario.FuncionarioId))
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

        // Se falhar a validação, recarrega a dropdown mantendo o valor selecionado
        var servicos = _context.ServicoFuncionario.ToList();
        ViewData["ServicoFuncionarioId"] = new SelectList(servicos, "ServicoFuncionarioId", "Nome", funcionario.ServicoFuncionarioId);

        return View(funcionario);
    }

    // GET: FUNCIONARIOS/Delete/5
    [HttpGet("/Funcionario/Delete/{funcionarioid}")]
    public async Task<IActionResult> Delete(int? funcionarioid)
    {
        if (funcionarioid == null)
        {
            return NotFound();
        }

        var funcionario = await _context.Funcionario
            .Include(f => f.Servico) // Carrega os dados do serviço
            .FirstOrDefaultAsync(m => m.FuncionarioId == funcionarioid);

        if (funcionario == null)
        {
            return NotFound();
        }

        return View(funcionario);
    }

    // POST: FUNCIONARIOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? funcionarioid)
    {
        var funcionario = await _context.Funcionario.FindAsync(funcionarioid);
        if (funcionario != null)
        {
            _context.Funcionario.Remove(funcionario);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool FuncionarioExists(int? funcionarioid)
    {
        return _context.Funcionario.Any(e => e.FuncionarioId == funcionarioid);
    }
}
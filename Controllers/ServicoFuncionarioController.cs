
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class ServicoFuncionarioController : Controller
{
    private readonly UniversityDbContext _context;

    public ServicoFuncionarioController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: SERVICOFUNCIONARIOS  
    public async Task<IActionResult> Index(string? successMessage = null)
    {
        if (successMessage != null)
        {
            ViewBag.SuccessMessage = successMessage;
        }

        return View(await _context.ServicoFuncionario.ToListAsync());
    }

    // GET: SERVICOFUNCIONARIOS/Details/5
    [HttpGet("/ServicoFuncionario/Details/{servicofuncionarioid}")]
    public async Task<IActionResult> Details(int? servicofuncionarioid, string? successMessage = null)
    {
        if (servicofuncionarioid == null)
        {
            return NotFound();
        }

        var servicofuncionario = await _context.ServicoFuncionario
            .FirstOrDefaultAsync(m => m.ServicoFuncionarioId == servicofuncionarioid);

        if (servicofuncionario == null)
        {
            return View("NotFound");
        }

        if (successMessage != null)
        {
            ViewBag.SuccessMessage = successMessage;
        }

        return View(servicofuncionario);
    }

    // GET: SERVICOFUNCIONARIOS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: SERVICOFUNCIONARIOS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ServicoFuncionarioId,Nome,Ativo,DataCriacao")] ServicoFuncionario servicofuncionario)
    {
        if (ModelState.IsValid)
        {
            _context.Add(servicofuncionario);
            await _context.SaveChangesAsync();
            // Redireciona diretamente para os detalhes do serviço recém-criado
            return RedirectToAction(
                  nameof(Details),
                  new { servicofuncionarioid = servicofuncionario.ServicoFuncionarioId, successMessage = "Serviço criado com sucesso." }
            );
        }
        return View(servicofuncionario);
    }

    // GET: SERVICOFUNCIONARIOS/Edit/5
    [HttpGet("/ServicoFuncionario/Edit/{ServicoFuncionarioId}")]
    public async Task<IActionResult> Edit(int? servicofuncionarioid)
    {
        if (servicofuncionarioid == null)
        {
            return View("NotFound");
        }

        var servicofuncionario = await _context.ServicoFuncionario.FindAsync(servicofuncionarioid);
        if (servicofuncionario == null)
        {
            return View("NotFound");
        }
        return View(servicofuncionario);
    }

    // POST: SERVICOFUNCIONARIOS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? servicofuncionarioid, [Bind("ServicoFuncionarioId,Nome,Ativo,DataCriacao")] ServicoFuncionario servicofuncionario)
    {
        if (servicofuncionarioid != servicofuncionario.ServicoFuncionarioId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(servicofuncionario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServicoFuncionarioExists(servicofuncionario.ServicoFuncionarioId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(
               nameof(Details),
               new { servicofuncionarioid = servicofuncionario.ServicoFuncionarioId, successMessage = "Serviço de funcionário atualizado com sucesso." }
            );
        }
        return View(servicofuncionario);
    }


    // GET: SERVICOFUNCIONARIOS/Delete/5
    [HttpGet("/ServicoFuncionario/Delete/{servicofuncionarioid}")]
    public async Task<IActionResult> Delete(int? servicofuncionarioid)
    {
        if (servicofuncionarioid == null)
        {
            return NotFound();
        }

        var servicofuncionario = await _context.ServicoFuncionario
            .FirstOrDefaultAsync(m => m.ServicoFuncionarioId == servicofuncionarioid);

        if (servicofuncionario == null)
        {
            return RedirectToAction(nameof(Index),
                new { successMessage = "Sucesso: O serviço de funcionário já tinha sido eliminado por outro utilizador." });
        }

        return View(servicofuncionario);
    }

    // POST: SERVICOFUNCIONARIOS/Delete/5
    [HttpPost("/ServicoFuncionario/Delete/{servicofuncionarioid}"), ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? servicofuncionarioid)
    {
        if (servicofuncionarioid == null)
        {
            return NotFound();
        }

        var servicofuncionario = await _context.ServicoFuncionario.FindAsync(servicofuncionarioid);
        if (servicofuncionario != null)
        {
            _context.ServicoFuncionario.Remove(servicofuncionario);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index), new { successMessage = "Serviço de funcionário eliminado com sucesso." });
    }

    private bool ServicoFuncionarioExists(int? servicofuncionarioid)
    {
        return _context.ServicoFuncionario.Any(e => e.ServicoFuncionarioId == servicofuncionarioid);
    }
}


using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class GroupController : Controller
{
    private readonly UniversityDbContext _context;

    public GroupController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: GROUPS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.ClassGroup.ToListAsync());
    }

    // GET: GROUPS/Details/5
    [HttpGet("Group/Details/{GroupId}")]
    public async Task<IActionResult> Details(int? groupid)
    {
        if (groupid == null)
        {
            return NotFound();
        }

        var group = await _context.ClassGroup
            .FirstOrDefaultAsync(m => m.GroupId == groupid);
        if (group == null)
        {
            return NotFound();
        }

        return View(group);
    }

    // GET: GROUPS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: GROUPS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("GroupId,NomeTurma,NumeroAlunos,Ativo")] Group group)
    {
        if (ModelState.IsValid)
        {
            _context.Add(group);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(group);
    }


    // GET: GROUPS/Edit/5
    [HttpGet("Group/Edit/{GroupId}")]
    public async Task<IActionResult> Edit(int? groupid)
    {
        if (groupid == null)
        {
            return NotFound();
        }

        var group = await _context.ClassGroup.FindAsync(groupid);
        if (group == null)
        {
            return NotFound();
        }
        return View(group);
    }

    // POST: GROUPS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? groupid, [Bind("GroupId,NomeTurma,NumeroAlunos,Ativo")] Group group)
    {
        if (groupid != group.GroupId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(group);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(group.GroupId))
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
        return View(group);
    }

    // GET: GROUPS/Delete/5
    [HttpGet("Group/Delete/{GroupId}")]
    public async Task<IActionResult> Delete(int? groupid)
    {
        if (groupid == null)
        {
            return NotFound();
        }

        var group = await _context.ClassGroup
            .FirstOrDefaultAsync(m => m.GroupId == groupid);
        if (group == null)
        {
            return NotFound();
        }

        return View(group);
    }

    // POST: GROUPS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? groupid)
    {
        var group = await _context.ClassGroup.FindAsync(groupid);
        if (group != null)
        {
            _context.ClassGroup.Remove(group);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool GroupExists(int? groupid)
    {
        return _context.ClassGroup.Any(e => e.GroupId == groupid);
    }
}

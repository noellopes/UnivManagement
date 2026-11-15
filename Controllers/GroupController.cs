using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Controllers
{
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
            if (groupid == null || groupid <= 0)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            var group = await _context.ClassGroup
                .FirstOrDefaultAsync(m => m.GroupId == groupid);

            if (group == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            return View(group);
        }

        // GET: GROUPS/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GROUPS/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupId,NomeTurma,NumeroAlunos,Ativo")] Group group)
        {
            if (ModelState.IsValid)
            {
                _context.Add(group);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Turma registada com sucesso!";
                return RedirectToAction(nameof(Details), new { GroupId = group.GroupId });
            }
            return View(group);
        }

        // GET: GROUPS/Edit/5
        [HttpGet("Group/Edit/{GroupId}")]
        public async Task<IActionResult> Edit(int? groupid)
        {
            if (groupid == null || groupid <= 0)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            var group = await _context.ClassGroup.FindAsync(groupid);
            if (group == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }
            return View(group);
        }

        // POST: GROUPS/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? groupid, [Bind("GroupId,NomeTurma,NumeroAlunos,Ativo")] Group group)
        {
            if (groupid != group.GroupId)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(group);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Turma atualizada com sucesso!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(group.GroupId))
                    {
                        Response.StatusCode = 404;
                        return View("NotFound");
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
            if (groupid == null || groupid <= 0)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            var group = await _context.ClassGroup
                .FirstOrDefaultAsync(m => m.GroupId == groupid);

            if (group == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            return View(group);
        }

        // POST: GROUPS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? groupid)
        {
            var group = await _context.ClassGroup.FindAsync(groupid);
            if (group == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            _context.ClassGroup.Remove(group);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Turma eliminada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private bool GroupExists(int? groupid)
        {
            return _context.ClassGroup.Any(e => e.GroupId == groupid);
        }
    }
}
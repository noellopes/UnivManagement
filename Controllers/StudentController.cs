
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models.AcademicManagement;
using UnivManagement.Data;

public class StudentController : Controller
{
    private readonly UniversityDbContext _context;

    public StudentController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: STUDENT
    public async Task<IActionResult> Index(string? successMessage = null)    
    {
        if (successMessage != null)
        {
            ViewBag.SuccessMessage = successMessage;
        }

        return View(await _context.Student.ToListAsync());
    }

    // GET: STUDENT/Details/5
    public async Task<IActionResult> Details(int? id, string? successMessage = null)
    {
        if (id == null)
        {
            return NotFound();
        }

        var student = await _context.Student
            .FirstOrDefaultAsync(m => m.StudentId == id);

        if (student == null)
        {
            return View("NotFound");
        }

        if (successMessage != null)
        {
            ViewBag.SuccessMessage = successMessage;
        }

        return View(student);
    }

    // GET: STUDENT/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: STUDENTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("StudentId,NumStudent,Name,Gender,Nationality,BirthDate,Address,NIF,Email,CountryPhoneCode,Telephone")] Student student)
    {

        /* ---------- Verificação de unicidade ---------- */
        if (await _context.Student.AnyAsync(s => s.NumStudent == student.NumStudent))
            ModelState.AddModelError(nameof(Student.NumStudent), "Já existe um aluno com este Número de Aluno.");

        if (await _context.Student.AnyAsync(s => s.NIF == student.NIF))
            ModelState.AddModelError(nameof(Student.NIF), "Já existe um aluno com este NIF.");

        if (await _context.Student.AnyAsync(s => s.Email == student.Email))
            ModelState.AddModelError(nameof(Student.Email), "Já existe um aluno com este Email.");

        if (await _context.Student.AnyAsync(s => s.Telephone == student.Telephone))
            ModelState.AddModelError(nameof(Student.Telephone), "Já existe um aluno com este Telefone.");
        // --------------------- Verificação de unicidade -------------------- */

        if (ModelState.IsValid)
        {
            try
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(
                    nameof(Details),
                    new { id = student.StudentId, successMessage = "Aluno criado com sucesso." }
                );
            }
            catch (DbUpdateException ex) when (IsUniqueViolation(ex))
            {
                ModelState.AddModelError("",
                    "Não foi possível guardar: já existe um registo com um destes valores únicos (Número de Aluno, NIF, Email ou Telefone).");
            }
        }

        return View(student);
    
}

    // GET: STUDENTS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var student = await _context.Student.FindAsync(id);

        if (student == null)
        {
            return View("NotFound");
        }
        return View(student);
    }

    // POST: STUDENTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("StudentId,NumStudent,Name,Gender,Nationality,BirthDate,Address,NIF,Email,CountryPhoneCode,Telephone")] Student student)
    {
        if (id != student.StudentId)
        {
            return NotFound();
        }


        /* ---------- Verificação de unicidade (excluindo o próprio aluno) ---------- */
        if (await _context.Student.AnyAsync(s =>
                s.NumStudent == student.NumStudent && s.StudentId != student.StudentId))
            ModelState.AddModelError(nameof(Student.NumStudent), "Já existe outro aluno com este Número de Aluno.");

        if (await _context.Student.AnyAsync(s =>
                s.NIF == student.NIF && s.StudentId != student.StudentId))
            ModelState.AddModelError(nameof(Student.NIF), "Já existe outro aluno com este NIF.");

        if (await _context.Student.AnyAsync(s =>
                s.Email == student.Email && s.StudentId != student.StudentId))
            ModelState.AddModelError(nameof(Student.Email), "Já existe outro aluno com este Email.");

        if (await _context.Student.AnyAsync(s =>
                s.Telephone == student.Telephone && s.StudentId != student.StudentId))
            ModelState.AddModelError(nameof(Student.Telephone), "Já existe outro aluno com este Telefone.");

        // --------------------- Verificação de unicidade (excluindo o próprio aluno) -------------------- */


        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(student);
                await _context.SaveChangesAsync();

                return RedirectToAction(
                    nameof(Details),
                    new { id = student.StudentId,
                    successMessage = "Dados do aluno atualizados com sucesso." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(student.StudentId))
                {
                    return View("NotFound");
                }
                    throw;
                }
                catch (DbUpdateException ex) when (IsUniqueViolation(ex))
                {
                    ModelState.AddModelError("",
                    "Não foi possível guardar: já existe um registo com um destes valores únicos (Número de Aluno, NIF, Email ou Telefone).");

            }
        }
        return View(student);
    }

    // GET: STUDENT/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var student = await _context.Student
            .FirstOrDefaultAsync(m => m.StudentId == id);

        // caso registo tenha sido já eliminado.
        if (student == null)
        {
            return RedirectToAction(nameof(Index), 
                new { successMessage = "Aviso: O Aluno já tinha sido eliminado por outro utilizador." });
        }

        return View(student);
    }

    // POST: STUDENT/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var student = await _context.Student.FindAsync(id);

        try
        {
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            // FK violation: o aluno está a ser usado noutro registo
            return RedirectToAction(nameof(Index),
                new { successMessage = "Aviso: Não foi possível eliminar o aluno. Pode estar a ser utilizado noutro registo." });
        }

        return RedirectToAction(nameof(Index),
            new { successMessage = "Aluno eliminado com sucesso." });
    }

    private bool StudentExists(int? id)
    {
        return _context.Student.Any(e => e.StudentId == id);
    }

    private static bool IsUniqueViolation(DbUpdateException ex)
    {
        return ex.InnerException is Microsoft.Data.SqlClient.SqlException sql
            && (sql.Number == 2601 || sql.Number == 2627);
    }
}

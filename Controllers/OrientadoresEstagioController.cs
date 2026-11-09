using Microsoft.AspNetCore.Mvc;
using UnivManagement.Data;

namespace UnivManagement.Controllers
{
    public class OrientadoresEstagioController : Controller
    {
        private UniversityDbContext _context;

        public OrientadoresEstagioController(UniversityDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.OrientadoresEstagio);
        }
    }
}
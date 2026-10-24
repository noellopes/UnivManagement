using Microsoft.AspNetCore.Mvc;

namespace UnivManagement.Controllers
{
    public class AcademicManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

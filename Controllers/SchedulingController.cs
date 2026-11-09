using Microsoft.AspNetCore.Mvc;

namespace UnivManagement.Controllers
{
    public class SchedulingController : Controller
    {
        public IActionResult Scheduling()
        {
            return View();
        }
    }
}
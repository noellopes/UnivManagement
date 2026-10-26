using Microsoft.AspNetCore.Mvc;

namespace UnivManagement.Controllers.Scheduling_Group_2
{
    public class SchedulingController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Scheduling_Group_2/Index.cshtml");
        }
    }
}
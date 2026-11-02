using Microsoft.AspNetCore.Mvc;

namespace UnivManagement.Controllers
{
    public class AccommodationController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Accommodation_Group_5/Index.cshtml");
        }
    }
}

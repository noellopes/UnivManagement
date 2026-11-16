using Microsoft.AspNetCore.Mvc;

namespace UnivManagement.Controllers {
    public class RequestsController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}

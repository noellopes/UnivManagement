using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UnivManagement.Models;

namespace UnivManagement.Controllers
{
    public class AgendarAulaController : Controller
    {
        public IActionResult AgendarAula()
        {
            return View();
        }
    }
}
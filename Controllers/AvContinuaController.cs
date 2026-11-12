using Microsoft.AspNetCore.Mvc;
using UnivManagement.Models;
using System.Collections.Generic;
using System;

namespace UnivManagement.Controllers
{
    public class AvContinuaController : Controller
    {
        public IActionResult Index()
        {
            // Dados de exemplo para testar a interface
            var avaliacoes = new List<AvContinua>
            {
                new AvContinua
                {
                    Id_Cont = 1,
                    Data_Registo = new DateTime(2026, 11, 11)
                },
                new AvContinua
                {
                    Id_Cont = 2,
                    Data_Registo = new DateTime(2026, 11, 12)
                }
            };

            return View(avaliacoes);
        }
    }
}

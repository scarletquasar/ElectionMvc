using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjetoHubCount.Models;
using ProjetoHubCount.Managers;

namespace ProjetoHubCount.Controllers
{
    public class ApiController : Controller
    {
        private readonly ILogger<ApiController> _logger;

        public ApiController(ILogger<ApiController> logger)
        {
            _logger = logger;
        }
        [HttpPost]
        public IActionResult candidate(int leg, string nom, string vic)
        {
            CandidatoManager.RegistrarCandidato(leg, nom, vic, DateTime.Now);
            return RedirectToAction("registro", "Home");
        }
        public IActionResult cdel(int leg)
        {
            CandidatoManager.ApagarCandidato(leg);
            return RedirectToAction("registro", "Home");
        }
        public IActionResult vote(int leg)
        {
            VotoManager.Votar(leg);
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult candidate()
        {
            return Content(CandidatoManager.ListaDeCandidatos(), "application/json");
        }
        public IActionResult votes()
        {
            return Content(VotoManager.VisualizarVotos(), "application/json");
        }
    }
}

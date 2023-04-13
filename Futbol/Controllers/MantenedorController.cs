using Futbol.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Futbol.Controllers
{
    public class MantenedorController : Controller
    {
        
        public async Task<IActionResult> Listar()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync("http://localhost:5079/api/equipos");
            var equipos = await response.Content.ReadFromJsonAsync<List<Equipo>>();
            return View(equipos);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Equipo equipo)
        {
            using var client = new HttpClient();
            var liga = equipo.Ligas;
            string[] li;
            {
                if (liga[0] == null)
                {
                    ModelState.AddModelError(string.Empty, "No puede haber ligas vacías");
                    return View(equipo);
                }
                else
                {
                    li = liga[0].Split(new char[] { ',' }, StringSplitOptions.TrimEntries);
                }
            }
            equipo.Ligas = li;

            var response = await client.PostAsJsonAsync("http://localhost:5079/api/equipos", equipo );
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, errorMessage);
            }
            return View(equipo);
        }

        public IActionResult Actualizar()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

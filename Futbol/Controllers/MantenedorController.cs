using Futbol.Models;
using Futbol.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Futbol.Controllers
{
    public class MantenedorController : Controller
    {

        private readonly IEquipoService _equipoService;
        public MantenedorController(IEquipoService equipoService)
        {
            _equipoService = equipoService;
        }
        public async Task<IActionResult> Listar()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync("http://localhost:5079/api/equipos");
            var equipos = await response.Content.ReadFromJsonAsync<List<Equipo>>();
            return View(equipos);
        }

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
            equipo.Puntos = 0;

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

        [HttpPost]
        public async Task<IActionResult> ActualizarPuntos([FromForm] string winnerId, [FromForm] string equipoIds)
        {
            var ids = equipoIds.Split(',');
            var equipos = new List<Equipo>();
            foreach (var id in ids)
            {
                var equipo = await _equipoService.Get(id);
                if (equipo != null)
                {
                    equipos.Add(equipo);
                }
            }   
            var winner = await _equipoService.Get(winnerId);
            if (winner == null)
            {
                return BadRequest("Invalid winner or loser id");
            }
            winner.Puntos += 3;
            await _equipoService.Update(winnerId, winner);
            var filteredEquipos = equipos.Where(e => e.Id != winnerId).ToList();
            foreach(var equip in filteredEquipos)
            {
                if(equip.Puntos > 0)
                {
                    equip.Puntos -= 1;
                    await _equipoService.Update(equip.Id, equip);
                }
            }

            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> Torneo()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync("http://localhost:5079/api/equipos");
            var equipos = await response.Content.ReadFromJsonAsync<List<Equipo>>();
            if (equipos.Count() >= 8)
            {
                Random rnd = new Random();
                var equiposRandom = equipos.OrderBy(x => rnd.Next()).Take(8).ToList();
                return View(equiposRandom);
            }
            
            TempData["ErrorMessage"] = "Necesitas tener por lo menos 8 equipos para entrar a torneos.";
            return RedirectToAction("Error", "Home");
            
            
        }

        public async Task<IActionResult> Editar(string id)
        {
            var equipo = await _equipoService.Get(id);
            if (equipo == null)
            {
                return NotFound();
            }

            return View(equipo);
        }


        [HttpPost]
        public async Task<IActionResult> Editar(string id, Equipo equipo)
        {
            if (id != equipo.Id)
            {
                return BadRequest();
            }
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
            if (ModelState.IsValid)
            {
                await _equipoService.Update(id, equipo);
                return RedirectToAction("Index", "Home");
            }
            return View(equipo);
        }

        public async Task<IActionResult> Administrar()
        {
            var response = await _equipoService.GetAll();
            var equipos =  response.ToList();
            return View(equipos);
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var equipo = await _equipoService.Get(id);

            if (equipo == null)
            {
                return NotFound();
            }

            await _equipoService.Remove(equipo.Id);

            return RedirectToAction("Index", "Home");
        }
    }
}

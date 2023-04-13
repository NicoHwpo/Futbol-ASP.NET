using Futbol.Models;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Crear()
        {
            return View();
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

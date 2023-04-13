using Futbol.Models;
using Futbol.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Futbol.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquiposController : ControllerBase
    {
        private readonly IEquipoService _equipoService;
        public EquiposController(IEquipoService equipoService)
        {
            _equipoService = equipoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Equipo>>> GetAll()
        {
            var prueba = await _equipoService.GetAll();
            return Ok(prueba);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Equipo>> Get(string id)
        {
            var equipo = await _equipoService.Get(id);
            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Equipo equipo)
        {
            await _equipoService.Create(equipo);
            return Ok(equipo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Equipo equipoIn)
        {
            var equipo = await _equipoService.Get(id);
            if (equipo == null)
            {
                return NotFound();
            }
            await _equipoService.Update(id, equipoIn);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var equipo = await _equipoService.Get(id);
            if (equipo == null)
            {
                return NotFound();
            }
            await _equipoService.Remove(equipo.Id);
            return NoContent();
        }
    }
}

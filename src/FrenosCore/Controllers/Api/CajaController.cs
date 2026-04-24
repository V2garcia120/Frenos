using FrenosCore.Modelos.Dtos.TurnoCaja;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FrenosCore.Servicios;

namespace FrenosCore.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CajaController : ControllerBase
    {
        private readonly ITurnoCajaService _turnoCajaService;

        public CajaController(ITurnoCajaService turnoCajaService)
        {
            _turnoCajaService = turnoCajaService;
        }

        [HttpPost("turno/abrir")]
        public async Task<IActionResult> AbrirTurno([FromBody] AbrirTurnoRequest request)
        {
            var response = await _turnoCajaService.AbrirTurnoAsync(request);
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }

        [HttpPost("turno/cerrar")]
        public async Task<IActionResult> CerrarTurno([FromBody] CerrarTurnoRequest request)
        {
            var response = await _turnoCajaService.CerrarTurnoAsync(request);
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }

        [HttpPost("moviento")]
        public async Task<IActionResult> RegistrarMovimiento([FromBody] object request)
        {
            // Aquí deberías implementar la lógica para registrar un movimiento de caja
            // Por ahora, solo devuelve un OK genérico
            return Ok(new { message = "Movimiento registrado (implementación pendiente)" });
        }
    }
}

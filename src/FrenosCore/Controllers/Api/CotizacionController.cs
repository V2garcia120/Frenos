using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FrenosCore.Helpers;
using FrenosCore.Servicios;

namespace FrenosCore.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CotizacionController : ControllerBase
    {
        private readonly ICotizacionService _cotizacionService;
        public CotizacionController(ICotizacionService cotizacionService) => _cotizacionService = cotizacionService;

        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] int pagina = 1, [FromQuery] int tam = 20)
        {
            var cotizaciones = await _cotizacionService.ListarAsync(pagina, tam);
            return Ok(ApiResponse<object>.Ok(cotizaciones));
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] FrenosCore.Modelos.Dtos.Cotizacion.CrearCotizacionRequest request)
        {
            var cotizacion = await _cotizacionService.CrearAsync(request);
            return Ok(ApiResponse<object>.Ok(cotizacion));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] FrenosCore.Modelos.Dtos.Cotizacion.ActualizarCotizacionRequest request)
        {
            var cotizacion = await _cotizacionService.ActualizarAsync(id, request);
            return Ok(ApiResponse<object>.Ok(cotizacion));
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var cotizacion = await _cotizacionService.ObtenerPorIdAsync(id);
            return Ok(ApiResponse<object>.Ok(cotizacion));
        }
        [HttpPost("{id:int}/aprobar")]
        public async Task<IActionResult> Aprobar(int id)
        {
            await _cotizacionService.AprobarAsync(id);
            return Ok(ApiResponse<object>.Ok($"Cotización con ID {id} aprobada exitosamente."));
        }
        [HttpPost("{id:int}/rechazar")]
        public async Task<IActionResult> Rechazar(int id)
        {
            await _cotizacionService.RechazarAsync(id);
            return Ok(ApiResponse<object>.Ok($"Cotización con ID {id} rechazada exitosamente."));
        }
    }
}

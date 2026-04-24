using FrenosCore.Servicios;
using FrenosCore.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FrenosCore.Modelos.Dtos.Servicio;

namespace FrenosCore.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiciosController : ControllerBase
    {
        private readonly IServiciciosService _serviciciosService;

        public ServiciosController(IServiciciosService serviciciosService)
        {
            _serviciciosService = serviciciosService;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var servicios = await _serviciciosService.ListarAsync();
            IEnumerable<ServicioDto> serviciosDto = servicios.Select(s => new ServicioDto 
            (
                s.Id,
                s.Nombre,
                s.Precio,
                s.DuracionMinutos,
                s.Categoria,
                s.Activo
            ));
            return Ok(ApiResponse<object>.Ok(serviciosDto));
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar([FromQuery] string termino)
        {
            if (string.IsNullOrWhiteSpace(termino))
                return BadRequest(ApiResponse<object>.Fail(
                    "VALIDATION_ERROR", "El parámetro termino es requerido."));
            var servicios = await _serviciciosService.BuscarAsync(termino);
            return Ok(ApiResponse<object>.Ok(servicios));
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
                var servicio = await _serviciciosService.ObtenerPorIdAsync(id);
                return Ok(ApiResponse<object>.Ok(servicio));
          
        }
        
    }
}
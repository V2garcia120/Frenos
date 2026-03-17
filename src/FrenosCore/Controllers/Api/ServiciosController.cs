using FrenosCore.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            return Ok(servicios);
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar([FromQuery] string termino)
        {
            if (string.IsNullOrWhiteSpace(termino))
                return BadRequest("El parámetro termino es requerido.");
            var servicios = await _serviciciosService.BuscarAsync(termino);
            return Ok(servicios);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
                var servicio = await _serviciciosService.ObtenerPorIdAsync(id);
                return Ok(servicio);
          
        }
        
    }
}
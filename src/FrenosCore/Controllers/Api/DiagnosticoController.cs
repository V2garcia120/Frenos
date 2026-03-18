using FrenosCore.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FrenosCore.Helpers;

namespace FrenosCore.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosticoController : ControllerBase
    {
        private readonly IDiagnosticoService _diagnosticoService;
        public DiagnosticoController(IDiagnosticoService diagnosticoService) => _diagnosticoService = diagnosticoService;

        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] int ordenId)
        {
            var diagnosticos = await _diagnosticoService.ListarPorOrdenAsync(ordenId);
            return Ok(ApiResponse<object>.Ok(diagnosticos));
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var diagnostico = await _diagnosticoService.ObtenerPorIdAsync(id);
            return Ok(ApiResponse<object>.Ok(diagnostico));
        }
    }
}

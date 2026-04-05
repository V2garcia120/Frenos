using FrenosCore.Helpers;
using FrenosCore.Modelos.Dtos.Orden;
using FrenosCore.Modelos.Entidades;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FrenosCore.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenController(IOrdenService ordenes) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Listar(
        [FromQuery] int pagina = 1,
        [FromQuery] int tam = 20,
        [FromQuery] string? estado = null,
        [FromQuery] string? prioridad = null,
        [FromQuery] int? tecnicoId = null,
        [FromQuery] DateTime? fecha = null)
        {
            var resultado = await ordenes.ListarAsync(pagina, tam, estado, prioridad, tecnicoId, fecha);
            return Ok(ApiResponse<object>.Ok(resultado));
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var orden = await ordenes.ObtenerPorIdAsync(id);
            return Ok(ApiResponse<object>.Ok(orden));
        }

   
        [HttpGet("{id:int}/diagnostico")]
        public async Task<IActionResult> ObtenerDiagnostico(
            int id,
            [FromServices] IDiagnosticoService diagnosticos)
        {
            var diagnostico = await diagnosticos.ListarPorOrdenAsync(id);
            return Ok(ApiResponse<object>.Ok(diagnostico));
        }

        [HttpPost]
        [Authorize(Policy = "Mantenimiento")]
        public async Task<IActionResult> Crear([FromBody] CrearOrdenRequest request)
        {
            var nueva = await ordenes.CrearAsync(request);
            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = nueva.Id },
                ApiResponse<object>.Ok(nueva));
        }

        // ─────────────────────────────────────────────────────────
        // PATCH api/ordenes/5/estado
        // Cambia el estado siguiendo el flujo permitido:
        // Recibido → EnDiagnostico → Aprobado → EnReparacion → Lista → Entregada
        // ─────────────────────────────────────────────────────────
        [HttpPatch("{id:int}/estado")]
        [Authorize(Policy = "Mantenimiento")]
        public async Task<IActionResult> CambiarEstado(
            int id, [FromBody] CambiarEstadoOrdenRequest request)
        {
            var actualizada = await ordenes.CambiarEstadoAsync(id, request);
            return Ok(ApiResponse<object>.Ok(actualizada));
        }

        // ─────────────────────────────────────────────────────────
        // POST api/ordenes/5/cerrar
        // Operación atómica: crea historial + genera factura + cambia estado a Lista
        // Solo cuando la orden está en EnReparacion
        // ─────────────────────────────────────────────────────────
        [HttpPost("{id:int}/cerrar")]
        [Authorize(Policy = "Mantenimiento")]
        public async Task<IActionResult> Cerrar(
            int id, [FromBody] CerrarOrdenRequest request)
        {
            var resultado = await ordenes.CerrarAsync(id, request);
            return Ok(ApiResponse<object>.Ok(resultado));
        }
    }
}


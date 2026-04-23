using FrenosCore.Helpers;
using FrenosCore.Modelos.Dtos.Vehiculo;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace FrenosCore.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiculosController : ControllerBase
    {
        private readonly IVehiculoService _vehiculoService;

        public VehiculosController(IVehiculoService vehiculoService)
        {
            _vehiculoService = vehiculoService;
        }

        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] RegistrarVehiculoRequest request)
        {
            var vehiculo = await _vehiculoService.RegistrarAsync(request);
            return Ok(ApiResponse<object>.Ok(vehiculo));
        }

        [HttpGet("cliente/{clienteId:int}")]
        public async Task<IActionResult> ListarPorCliente(int clienteId)
        {
            var vehiculos = await _vehiculoService.ListarPorClienteAsync(clienteId);
            return Ok(ApiResponse<object>.Ok(vehiculos));
        }

        [HttpGet("{vehiculoId:int}/historial-reparaciones")]
        public async Task<IActionResult> ListarHistorialReparaciones(int vehiculoId)
        {
            var historial = await _vehiculoService.ListarHistorialReparacionesAsync(vehiculoId);
            return Ok(ApiResponse<object>.Ok(historial));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarVehiculoRequest request)
        {
            var vehiculo = await _vehiculoService.ActualizarAsync(id, request);
            return Ok(ApiResponse<object>.Ok(vehiculo));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _vehiculoService.DesactivarAsync(id);
            return Ok(ApiResponse<object>.Ok($"Vehículo con ID {id} desactivado exitosamente."));
        }
    }
}

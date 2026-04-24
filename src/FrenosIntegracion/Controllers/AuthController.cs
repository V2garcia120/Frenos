using FrenosIntegracion.DTOs;
using FrenosIntegracion.Helpers;
using FrenosIntegracion.Models.DTOs;
using FrenosIntegracion.Services.Cache;
using FrenosIntegracion.Services.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrenosIntegracion.Controllers
{
    [ApiController]
    [Route("int/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ICoreService _core;
        private readonly ICacheService _cache;

        // 1. Usamos constructor tradicional (SIN paréntesis en la clase) para matar el CS8863
        public AuthController(ICoreService core, ICacheService cache)
        {
            _core = core;
            _cache = cache;
        }

        // 2. Health Check
        [HttpGet("health")]
        public async Task<IActionResult> Health()
        {
            // Si aquí te sigue dando ambigüedad, el problema es el archivo de la interfaz duplicado
            var coreDisponible = await _core.EstaDisponibleAsync();

            var status = new HealthResponse(
                Integracion: "online",
                Core: coreDisponible ? "online" : "offline",
                ModoCache: !coreDisponible,
                UltimaSync: _cache.UltimaActualizacion
            );

            return Ok(FrenosIntegracion.Helpers.ApiResponse<HealthResponse>.Ok(status));
        }

        [HttpPost("login-cliente")]
        public async Task<IActionResult> LoginCliente([FromBody] LoginRequest request)
        {
            try
            {
                var resultado = await _core.AutenticarClienteAsync(request);
                return Ok(FrenosIntegracion.Helpers.ApiResponse<object>.Ok(resultado));
            }
            catch (Exception)
            {
                return StatusCode(503, FrenosIntegracion.Helpers.ApiResponse<object>.Fail(
                    "CORE_UNAVAILABLE",
                    "El sistema central no está disponible."));
            }
        }

        [HttpPost("login-cajero")]
        public async Task<IActionResult> LoginCajero([FromBody] LoginRequest request)
        {
            try
            {
                var resultado = await _core.AutenticarCajeroAsync(request);
                return Ok(FrenosIntegracion.Helpers.ApiResponse<object>.Ok(resultado));
            }
            catch (Exception)
            {
                return StatusCode(503, FrenosIntegracion.Helpers.ApiResponse<object>.Fail(
                    "CORE_UNAVAILABLE",
                    "No se puede validar el acceso al cajero."));
            }
        }

        [HttpPost("registrar-cliente")]
        [AllowAnonymous]
        public async Task<IActionResult> Registrar([FromBody] ClienteRegistroDto modelo)
        {
            try
            {
                var exito = await _core.RegistrarClienteAsync(modelo);
                if (exito)
                    return Ok(FrenosIntegracion.Helpers.ApiResponse<object>.Ok(
                        new { mensaje = "Cliente registrado con éxito" }));

                return BadRequest(FrenosIntegracion.Helpers.ApiResponse<object>.Fail(
                    "REGISTRO_FALLIDO", "No se pudo registrar el cliente."));
            }
            catch (Exception ex)
            {
                return BadRequest(FrenosIntegracion.Helpers.ApiResponse<object>.Fail(
                    "REGISTRO_FALLIDO", ex.Message));
            }
        }
    }
}
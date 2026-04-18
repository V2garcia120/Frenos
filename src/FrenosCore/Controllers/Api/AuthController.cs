using FrenosCore.Helpers;
using FrenosCore.Modelos.Dtos.Auth;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FrenosCore.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        public AuthController(AuthService authService) => _authService = authService;

        [HttpPost("login")]
        public async Task<IActionResult> IniciarSesion([FromBody] IniciarSesionRequest request)
        {
            var resultado = await _authService.IniciarSesionUsuario(request);
            if (resultado.Exito)
                return Ok(ApiResponse<object>.Ok(new { Token = resultado.Token }));
            return Unauthorized(ApiResponse<object>.Fail("AUTH_ERROR", "Credenciales inválidas."));
        }
        [HttpPost("login-cliente")]
        public async Task<IActionResult> IniciarSesionCliente([FromBody] IniciarSesionRequest request)
        {
            var resultado = await _authService.IniciarSesionCliente(request);
            if (resultado.Exito)
                return Ok(ApiResponse<object>.Ok(new { Token = resultado.Token }));
            return Unauthorized(ApiResponse<object>.Fail("AUTH_ERROR", "Credenciales inválidas."));


        }
    }
}
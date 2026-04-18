using FrenosIntegracion.Models.DTOs;
using FrenosIntegracion.Services.Cache;
using FrenosIntegracion.Services.Core;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("int")]
public class AuthIntController(
    ICoreService core, ICacheService cache) : ControllerBase
{
    // GET int/auth/health
    [HttpGet("auth/health")]
    public async Task<IActionResult> Health()
    {
        var coreDisponible = await core.EstaDisponibleAsync();
        return Ok(ApiResponse<object>.Ok(new HealthResponse(
            Integracion: "online",
            Core: coreDisponible ? "online" : "offline",
            ModoCache: !coreDisponible,
            UltimaSync: cache.UltimaActualizacion)));
    }
}

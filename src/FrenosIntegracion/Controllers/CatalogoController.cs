using FrenosIntegracion.Services.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("int/catalogo")]
[Authorize]
public class CatalogoController(ICacheService cache) : ControllerBase
{
    // GET int/catalogo/productos?categoria=Frenos
    [HttpGet("productos")]
    public async Task<IActionResult> Productos([FromQuery] string? categoria)
    {
        var productos = await cache.ObtenerProductosAsync();
        if (!string.IsNullOrWhiteSpace(categoria))
            productos = productos.Where(p =>
                p.Categoria?.Equals(categoria, StringComparison.OrdinalIgnoreCase) == true);
        return Ok(ApiResponse<object>.Ok(productos));
    }

    // GET int/catalogo/buscar?q=pastillas
    [HttpGet("buscar")]
    public async Task<IActionResult> Buscar([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q))
            return BadRequest(ApiResponse<object>.Fail(
                "VALIDATION_ERROR", "El parámetro q es requerido."));

        var termino = q.Trim().ToLower();
        var productos = (await cache.ObtenerProductosAsync())
            .Where(p => p.Nombre.ToLower().Contains(termino));
        var servicios = (await cache.ObtenerServiciosAsync())
            .Where(s => s.Nombre.ToLower().Contains(termino));
        return Ok(ApiResponse<object>.Ok(new { productos, servicios }));
    }
}

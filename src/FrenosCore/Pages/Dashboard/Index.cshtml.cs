using System.IdentityModel.Tokens.Jwt;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenosCore.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly IProductoService _productoService;

        public IndexModel(IProductoService productoService)
        {
            _productoService = productoService;
        }

        public int TotalProductos { get; private set; }
        public string RolUsuario { get; private set; } = "Usuario";
        public bool EsAdmin { get; private set; }
        public bool EsTecnico { get; private set; }

        public async Task OnGetAsync()
        {
            var productos = await _productoService.ListarTodosAsync(string.Empty);
            TotalProductos = productos.Count();

            if (Request.Cookies.TryGetValue("AuthToken", out var token) && !string.IsNullOrWhiteSpace(token))
            {
                var handler = new JwtSecurityTokenHandler();
                if (handler.CanReadToken(token))
                {
                    var jwt = handler.ReadJwtToken(token);
                    RolUsuario = jwt.Claims.FirstOrDefault(c => c.Type == "Rol")?.Value ?? "Usuario";
                }
            }

            var rolNormalizado = RolUsuario.Trim().ToLowerInvariant();
            EsAdmin = rolNormalizado is "admin";
            EsTecnico = rolNormalizado is "tecnico";
        }
    }
}

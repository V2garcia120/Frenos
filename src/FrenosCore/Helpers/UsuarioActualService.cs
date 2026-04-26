using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FrenosCore.Helpers
{
    public class UsuarioActualService(IHttpContextAccessor http) : IUsuarioActualService
    {
        public int Id
        {
            get
            {
                var claim = http.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                         ?? http.HttpContext?.User.FindFirst("sub")?.Value
                         ?? http.HttpContext?.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

                Console.WriteLine($"[UsuarioActual] Claim encontrado: '{claim}'");

                // Si no hay claim válido, usar Id=1 (admin del sistema para llamadas internas)
                if (string.IsNullOrEmpty(claim))
                {
                    var rol = http.HttpContext?.User.FindFirst("Rol")?.Value;
                    if (rol == "Administrador") return 1; // llamada interna del sistema
                }

                return int.TryParse(claim, out var id) ? id : 0;
            }
        }

        public string Nombre => http.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value ?? "Sistema";
        public string Ip => http.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "Desconocida";
        public string Rol => http.HttpContext?.User.FindFirst("Rol")?.Value ?? "Sistema";

    }
}

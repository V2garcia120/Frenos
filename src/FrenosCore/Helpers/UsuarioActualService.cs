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
                         ?? http.HttpContext?.User.FindFirst("sub")?.Value;
                return int.TryParse(claim, out var id) ? id : 0;
            }
        }

        public string Nombre => http.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value ?? "Sistema";
        public string Ip => http.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "Desconocida";
        public string Rol => http.HttpContext?.User.FindFirst("Rol")?.Value ?? "Sistema";

    }
}

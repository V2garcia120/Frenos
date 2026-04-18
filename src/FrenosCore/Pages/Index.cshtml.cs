using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenosCore.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            if (!Request.Cookies.TryGetValue("AuthToken", out var token) || string.IsNullOrWhiteSpace(token))
                return RedirectToPage("/Login/Index");

            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(token))
                return RedirectToPage("/Login/Index");

            var jwt = handler.ReadJwtToken(token);
            var rol = jwt.Claims.FirstOrDefault(c => c.Type == "Rol")?.Value?.Trim().ToLowerInvariant();

            return rol switch
            {
                "admin" or "administrador" => RedirectToPage("/Dashboard/Admin"),
                "tecnico" or "técnico" => RedirectToPage("/Dashboard/Tecnico"),
                "mantenimiento" => RedirectToPage("/Dashboard/Mantenimiento"),
                "consulta" or "caja" => RedirectToPage("/Dashboard/Consulta"),
                _ => RedirectToPage("/Dashboard/Index")
            };
        }
    }
}

using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using FrenosCore.Modelos.Dtos.CuentasPorCobrar;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenosCore.Pages.CuentasPorCobrar
{
    [Authorize(Policy = "Mantenimiento")]
    public class DetalleModel : PageModel
    {
        private readonly ICuentasPorCobrarService _cuentasPorCobrarService;

        public DetalleModel(ICuentasPorCobrarService cuentasPorCobrarService)
        {
            _cuentasPorCobrarService = cuentasPorCobrarService;
        }

        public CuentaPorCobrarDetalleResponse? Cuenta { get; private set; }

        [BindProperty]
        public AbonoInput Abono { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            await CargarAsync(id);
            return Cuenta is null ? RedirectToPage("/CuentasPorCobrar/Index") : Page();
        }

        public async Task<IActionResult> OnPostRegistrarAbonoAsync(int id)
        {
            await CargarAsync(id);
            if (Cuenta is null)
                return RedirectToPage("/CuentasPorCobrar/Index");

            if (!ModelState.IsValid)
                return Page();

            var usuarioId = ObtenerUsuarioIdDesdeToken();
            if (!usuarioId.HasValue)
            {
                TempData["MensajeError"] = "No se pudo identificar el usuario autenticado.";
                return RedirectToPage("/Login/Index");
            }

            try
            {
                Cuenta = await _cuentasPorCobrarService.RegistrarAbonoAsync(
                    id,
                    Abono.Monto,
                    Abono.MetodoPago,
                    usuarioId.Value);

                TempData["Mensaje"] = "Abono registrado correctamente.";
                return RedirectToPage(new { id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        private async Task CargarAsync(int id)
        {
            try
            {
                Cuenta = await _cuentasPorCobrarService.ObtenerPorIdAsync(id);
                if (Abono.Monto <= 0)
                    Abono.Monto = Cuenta.Saldo;
            }
            catch (KeyNotFoundException)
            {
                TempData["MensajeError"] = "No se encontró la cuenta por cobrar.";
                Cuenta = null;
            }
        }

        private int? ObtenerUsuarioIdDesdeToken()
        {
            var claimSub = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub || c.Type == "sub")?.Value;
            if (int.TryParse(claimSub, out var idDesdeClaims))
                return idDesdeClaims;

            if (!Request.Cookies.TryGetValue("AuthToken", out var token) || string.IsNullOrWhiteSpace(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(token))
                return null;

            var jwt = handler.ReadJwtToken(token);
            var sub = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            return int.TryParse(sub, out var idUsuario) ? idUsuario : null;
        }

        public class AbonoInput
        {
            [Range(0.01, double.MaxValue)]
            public decimal Monto { get; set; }

            [Required]
            public string MetodoPago { get; set; } = "Efectivo";
        }
    }
}

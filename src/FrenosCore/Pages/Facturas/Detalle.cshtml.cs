using System.ComponentModel.DataAnnotations;
using FrenosCore.Modelos.Dtos.CuentasPorCobrar;
using FrenosCore.Modelos.Dtos.Factura;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenosCore.Pages.Facturas
{
    [Authorize(Policy = "Mantenimiento")]
    public class DetalleModel : PageModel
    {
        private readonly IFacturaService _facturaService;
        private readonly ICuentasPorCobrarService _cuentasPorCobrarService;

        public DetalleModel(IFacturaService facturaService, ICuentasPorCobrarService cuentasPorCobrarService)
        {
            _facturaService = facturaService;
            _cuentasPorCobrarService = cuentasPorCobrarService;
        }

        public FacturaResponse? Factura { get; private set; }
        public CuentaPorCobrarResponse? CuentaPorCobrar { get; private set; }
        public bool EsAdmin => User.IsInRole("Administrador") || User.IsInRole("Admin");

        [BindProperty]
        public RegistrarPagoInput Pago { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            await CargarAsync(id);
            return Factura is null ? RedirectToPage("/Facturas/Index") : Page();
        }

        public async Task<IActionResult> OnPostMarcarCreditoAsync(int id)
        {
            await CargarAsync(id);
            if (Factura is null)
                return RedirectToPage("/Facturas/Index");

            if (Factura.Estado != "Pendiente")
            {
                TempData["MensajeError"] = "Solo se puede marcar como crédito una factura pendiente.";
                return RedirectToPage(new { id });
            }

            if (CuentaPorCobrar is not null || string.Equals(Factura.MetodoPago, "Credito", StringComparison.OrdinalIgnoreCase))
            {
                TempData["MensajeError"] = "La factura ya fue marcada como crédito.";
                return RedirectToPage(new { id });
            }

            try
            {
                await _facturaService.RegistrarPagoAsync(id, new RegistrarPagoRequest("Credito", Factura.Total));
                TempData["Mensaje"] = "Factura marcada como crédito. Se creó la cuenta por cobrar.";
                return RedirectToPage(new { id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostRegistrarPagoAsync(int id)
        {
            await CargarAsync(id);
            if (Factura is null)
                return RedirectToPage("/Facturas/Index");

            if (CuentaPorCobrar is not null || string.Equals(Factura.MetodoPago, "Credito", StringComparison.OrdinalIgnoreCase))
            {
                TempData["MensajeError"] = "Esta factura es a crédito. Debes registrar los pagos desde la cuenta por cobrar.";
                return RedirectToPage("/CuentasPorCobrar/Detalle", new { id = CuentaPorCobrar!.Id });
            }

            if (!ModelState.IsValid)
                return Page();

            try
            {
                await _facturaService.RegistrarPagoAsync(
                    id,
                    new RegistrarPagoRequest(Pago.MetodoPago, Pago.MontoPagado));

                TempData["Mensaje"] = "Pago registrado correctamente.";
                return RedirectToPage(new { id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAnularAsync(int id)
        {
            await CargarAsync(id);
            if (Factura is null)
                return RedirectToPage("/Facturas/Index");

            if (!EsAdmin)
            {
                TempData["MensajeError"] = "Solo el administrador puede anular facturas.";
                return RedirectToPage(new { id });
            }

            try
            {
                await _facturaService.AnularAsync(id);
                TempData["Mensaje"] = "Factura anulada correctamente.";
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
                Factura = await _facturaService.ObtenerPorIdAsync(id);
                CuentaPorCobrar = await _cuentasPorCobrarService.ObtenerPorFacturaIdAsync(id);

                if (Factura is not null && Pago.MontoPagado <= 0)
                    Pago.MontoPagado = Factura.Total;
            }
            catch (KeyNotFoundException)
            {
                TempData["MensajeError"] = "No se encontró la factura.";
                Factura = null;
            }
        }

        public class RegistrarPagoInput
        {
            [Required]
            public string MetodoPago { get; set; } = "Efectivo";

            [Range(0.01, double.MaxValue)]
            public decimal MontoPagado { get; set; }
        }
    }
}

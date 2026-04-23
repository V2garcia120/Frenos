using FrenosCore.Modelos.Dtos.Factura;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenosCore.Pages.Facturas
{
    [Authorize(Policy = "Mantenimiento")]
    public class ImprimirModel : PageModel
    {
        private readonly IFacturaService _facturaService;

        public ImprimirModel(IFacturaService facturaService)
        {
            _facturaService = facturaService;
        }

        public FacturaResponse? Factura { get; private set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                Factura = await _facturaService.ObtenerPorIdAsync(id);
                return Page();
            }
            catch (KeyNotFoundException)
            {
                TempData["MensajeError"] = "No se encontró la factura.";
                return RedirectToPage("/Facturas/Index");
            }
        }
    }
}

using FrenosCore.Modelos.Dtos.Vehiculo;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenosCore.Pages.Vehiculos
{
    [Authorize(Roles = "Administrador,Mantenimiento,Tecnico")]
    public class HistorialModel : PageModel
    {
        private readonly IVehiculoService _vehiculoService;

        public HistorialModel(IVehiculoService vehiculoService)
        {
            _vehiculoService = vehiculoService;
        }

        public int VehiculoId { get; private set; }
        public int? OrdenIdRegreso { get; private set; }
        public IReadOnlyList<HistorialReparacionResponse> Historial { get; private set; } = [];

        public async Task<IActionResult> OnGetAsync(int vehiculoId, int? ordenId)
        {
            VehiculoId = vehiculoId;
            OrdenIdRegreso = ordenId;

            try
            {
                Historial = await _vehiculoService.ListarHistorialReparacionesAsync(vehiculoId);
                return Page();
            }
            catch (KeyNotFoundException)
            {
                TempData["MensajeError"] = "No se encontró el vehículo solicitado.";
                return ordenId.HasValue
                    ? RedirectToPage("/Ordenes/Detalle", new { id = ordenId.Value })
                    : RedirectToPage("/Ordenes/Index");
            }
        }
    }
}

using FrenosCore.Modelos.Dtos.Servicio;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenosCore.Pages.Servicios
{
    public class IndexModel : PageModel
    {
        private readonly IServiciciosService _serviciosService;

        public IndexModel(IServiciciosService serviciosService)
        {
            _serviciosService = serviciosService;
        }

        public IList<ServicioResponse> Servicios { get; private set; } = [];

        [BindProperty(SupportsGet = true)]
        public string? Termino { get; set; }

        public async Task OnGetAsync()
        {
            await CargarServiciosAsync();
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            try
            {
                await _serviciosService.EliminarAsync(id);
                TempData["Mensaje"] = "Servicio eliminado correctamente.";
            }
            catch (KeyNotFoundException)
            {
                TempData["MensajeError"] = "No se encontró el servicio.";
            }

            return RedirectToPage(new { Termino });
        }

        private async Task CargarServiciosAsync()
        {
            var servicios = string.IsNullOrWhiteSpace(Termino)
                ? await _serviciosService.ListarAsync()
                : await _serviciosService.BuscarAsync(Termino);

            Servicios = servicios.ToList();
        }
    }
}

using FrenosCore.Modelos.Dtos.Dashboard;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenosCore.Pages.Dashboard
{
    public class MantenimientoModel : PageModel
    {
        private readonly IDashboardService _dashboardService;

        public MantenimientoModel(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public MantenimientoDashboardResponse Data { get; private set; } =
            new(0, 0, 0, 0, 0, [], []);

        public async Task OnGetAsync()
        {
            Data = await _dashboardService.ObtenerDashboardMantenimientoAsync();
        }
    }
}

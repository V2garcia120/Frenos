using FrenosCore.Modelos.Dtos.Dashboard;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenosCore.Pages.Dashboard
{
    public class AdminModel : PageModel
    {
        private readonly IDashboardService _dashboardService;

        public AdminModel(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public AdminDashboardResponse Data { get; private set; } =
            new(0, 0, 0, 0, 0, 0, 0, 0, [], [], [], []);

        public decimal VariacionHoy => CalcularVariacion(Data.FacturadoHoy, Data.FacturadoAyer);

        public decimal VariacionMes => CalcularVariacion(Data.FacturadoMes, Data.FacturadoMesAnterior);

        public async Task OnGetAsync()
        {
            Data = await _dashboardService.ObtenerDashboardAdminAsync();
        }

        private static decimal CalcularVariacion(decimal actual, decimal anterior)
        {
            if (anterior <= 0)
                return actual > 0 ? 100 : 0;

            return ((actual - anterior) / anterior) * 100;
        }
    }
}

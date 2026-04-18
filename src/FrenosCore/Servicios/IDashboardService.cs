using FrenosCore.Modelos.Dtos.Dashboard;

namespace FrenosCore.Servicios
{
    public interface IDashboardService
    {
        Task<AdminDashboardResponse> ObtenerDashboardAdminAsync();
        Task<TecnicoDashboardResponse> ObtenerDashboardTecnicoAsync(int? tecnicoId);
        Task<MantenimientoDashboardResponse> ObtenerDashboardMantenimientoAsync();
    }
}

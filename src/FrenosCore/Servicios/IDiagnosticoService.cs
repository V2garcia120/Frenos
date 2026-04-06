using FrenosCore.Modelos.Dtos.Diagnostico;

namespace FrenosCore.Servicios
{
    public interface IDiagnosticoService
    {
        Task<DiagnosticoResponse> CrearAsync(CrearDiagnosticoRequest request);
        Task<DiagnosticoResponse> CompletarDiagnosticoAsync(int id);
        Task<DiagnosticoResponse> ListarPorOrdenAsync(int ordenId);
        Task<DiagnosticoResponse> ObtenerPorIdAsync(int id);
        Task<DiagnosticoResponse> ActualizarAsync(int id, ActualizarDiagnosticoRequest request);

        Task AprobarAsync(int id);
        Task EliminarAsync(int id);
    }
}

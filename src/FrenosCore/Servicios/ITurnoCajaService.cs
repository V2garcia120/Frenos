using FrenosCore.Modelos.Dtos.TurnoCaja;

namespace FrenosCore.Servicios
{
    public interface ITurnoCajaService
    {
        Task<AbrirTurnoResponse?> AbrirTurnoAsync(AbrirTurnoRequest request);
        Task<CerrarTurnoResponse?> CerrarTurnoAsync(CerrarTurnoRequest request);
    }
}

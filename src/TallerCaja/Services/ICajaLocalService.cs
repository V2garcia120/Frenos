using TallerCaja.Models.DTOs;
using TallerCaja.Models.Entities;

namespace TallerCaja.Services
{
    // ── Servicio de datos locales (SQLite) ────────────────────────────────────
    public interface ICajaLocalService
    {
        void SincronizarProductos(List<ProductoDto> productos);
        void SincronizarServicios(List<ServicioDto> servicios);
        List<ProductoLocal> ObtenerProductosLocales(string? filtro = null);
        List<ServicioLocal> ObtenerServiciosLocales(string? filtro = null);
        TurnoLocal? ObtenerTurnoActivo(int cajeroId);
        void GuardarTurnoLocal(TurnoLocal turno);
        void ActualizarTurnoLocal(TurnoLocal turno);
        void GuardarVentaLocal(VentaLocal venta);
        List<VentaLocal> ObtenerVentasDelTurno(int turnoId);
    }
}

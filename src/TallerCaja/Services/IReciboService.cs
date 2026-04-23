using TallerCaja.Models.DTOs;

namespace TallerCaja.Services
{
    // ── Servicio de recibo/factura ────────────────────────────────────────────
    public interface IReciboService
    {
        string GenerarTextoRecibo(CobroResponse cobro, CobroRequest request, string clienteNombre);
    }
}

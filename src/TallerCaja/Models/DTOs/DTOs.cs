namespace TallerCaja.Models.DTOs
{
    // ── Respuesta estándar de la API ──────────────────────────────────────────
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public ApiError? Error { get; set; }
    }

    public class ApiError
    {
        public string Codigo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
    }

    // ── Health Check ──────────────────────────────────────────────────────────
    public class HealthCheckDto
    {
        public string Integracion { get; set; } = "online";
        public string Core { get; set; } = "offline";
        public bool ModoCache { get; set; }
        public DateTime? UltimaSync { get; set; }
    }

    // ── Login Cajero ──────────────────────────────────────────────────────────
    public class LoginCajeroRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginCajeroResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expira { get; set; }
        public int CajeroId { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    // ── Catálogo ──────────────────────────────────────────────────────────────
    public class ProductoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string Categoria { get; set; } = string.Empty;
    }

    public class ServicioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int DuracionMin { get; set; }
        public string Categoria { get; set; } = string.Empty;
    }

    public class BusquedaCatalogoDto
    {
        public List<ProductoDto> Productos { get; set; } = new();
        public List<ServicioDto> Servicios { get; set; } = new();
    }

    // ── Clientes ──────────────────────────────────────────────────────────────
    public class ClienteDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public bool EsAnonimo { get; set; }
    }

    // ── Turno ─────────────────────────────────────────────────────────────────
    public class AbrirTurnoRequest
    {
        public int CajeroId { get; set; }
        public decimal MontoInicial { get; set; }
    }

    public class AbrirTurnoResponse
    {
        public int TurnoId { get; set; }
        public string Estado { get; set; } = "Abierto";
        public DateTime FechaApertura { get; set; }
    }

    public class CerrarTurnoRequest
    {
        public int TurnoId { get; set; }
        public decimal EfectivoContado { get; set; }
        public string? Observaciones { get; set; }
    }

    public class CerrarTurnoResponse
    {
        public int TurnoId { get; set; }
        public decimal EfectivoEsperado { get; set; }
        public decimal EfectivoContado { get; set; }
        public decimal Diferencia { get; set; }
        public decimal TotalVentas { get; set; }
    }

    // ── Cobro en Caja ─────────────────────────────────────────────────────────
    public class ItemCobroDto
    {
        public string Tipo { get; set; } = string.Empty; // Producto | Servicio
        public int ItemId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioSnapshot { get; set; }
        public string NombreSnapshot { get; set; } = string.Empty; // Solo uso local
    }

    public class CobroRequest
    {
        public int TurnoId { get; set; }
        public int ClienteId { get; set; }
        public int? VehiculoId { get; set; }
        public List<ItemCobroDto> Items { get; set; } = new();
        public string MetodoPago { get; set; } = string.Empty;
        public decimal MontoPagado { get; set; }
    }

    public class CobroResponse
    {
        public int? FacturaId { get; set; }
        public string? NumeroFactura { get; set; }
        public decimal Total { get; set; }
        public decimal Cambio { get; set; }
        public string Estado { get; set; } = string.Empty; // Completado | PendienteSync
        public string IdLocal { get; set; } = string.Empty;
    }

    // ── Pago de Factura ───────────────────────────────────────────────────────
    public class PagoFacturaRequest
    {
        public int TurnoId { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
        public decimal MontoPagado { get; set; }
    }

    public class PagoFacturaResponse
    {
        public int FacturaId { get; set; }
        public string Numero { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public decimal Cambio { get; set; }
        public string Estado { get; set; } = string.Empty;
        public int? CxcId { get; set; }
    }

    public class FacturaPendienteDto
    {
        public int Id { get; set; }
        public string Numero { get; set; } = string.Empty;
        public string ClienteNombre { get; set; } = string.Empty;
        public string VehiculoInfo { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public string Estado { get; set; } = string.Empty;
        public List<ItemFacturaDto> Items { get; set; } = new();
    }

    public class ItemFacturaDto
    {
        public string Nombre { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Subtotal { get; set; }
    }

    // ── CxC ───────────────────────────────────────────────────────────────────
    public class AbonoRequest
    {
        public int TurnoId { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
    }

    public class AbonoResponse
    {
        public decimal MontoAbonado { get; set; }
        public decimal SaldoAnterior { get; set; }
        public decimal SaldoActual { get; set; }
        public bool Saldada { get; set; }
        public string EstadoCxC { get; set; } = string.Empty;
    }

    // ── Sincronización ────────────────────────────────────────────────────────
    public class SyncRequest
    {
        public List<SyncItemDto> Transacciones { get; set; } = new();
    }

    public class SyncItemDto
    {
        public string IdLocal { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
    }

    public class SyncResponse
    {
        public int Procesadas { get; set; }
        public int Fallidas { get; set; }
        public List<SyncResultadoDto> Resultados { get; set; } = new();
    }

    public class SyncResultadoDto
    {
        public string IdLocal { get; set; } = string.Empty;
        public bool Exitosa { get; set; }
        public string? Error { get; set; }
    }
}

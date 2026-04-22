namespace TallerCaja.Models.Entities
{
    public class ProductoLocal
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public DateTime UltimaSync { get; set; }
    }

    public class ServicioLocal
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int DuracionMin { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public DateTime UltimaSync { get; set; }
    }

    public class TurnoLocal
    {
        public int Id { get; set; }
        public int CajeroId { get; set; }
        public string CajeroNombre { get; set; } = string.Empty;
        public decimal MontoInicial { get; set; }
        public decimal? EfectivoContado { get; set; }
        public string Estado { get; set; } = "Abierto"; // Abierto | Cerrado
        public DateTime FechaApertura { get; set; }
        public DateTime? FechaCierre { get; set; }
        public string? Observaciones { get; set; }
        public int? TurnoIdCore { get; set; }
    }

    public class TransaccionPendiente
    {
        public int Id { get; set; }
        public string IdLocal { get; set; } = Guid.NewGuid().ToString();
        public string Tipo { get; set; } = string.Empty; // cobro | pago_factura | abono_cxc
        public string Payload { get; set; } = string.Empty; // JSON serializado
        public DateTime FechaLocal { get; set; }
        public bool Procesada { get; set; } = false;
        public string? ErrorDetalle { get; set; }
    }

    public class VentaLocal
    {
        public int Id { get; set; }
        public string IdLocal { get; set; } = Guid.NewGuid().ToString();
        public int? FacturaIdCore { get; set; }
        public string? NumeroFactura { get; set; }
        public int TurnoId { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal ITBIS { get; set; }
        public decimal Total { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
        public decimal MontoPagado { get; set; }
        public decimal Cambio { get; set; }
        public string Estado { get; set; } = "Completado"; // Completado | PendienteSync
        public DateTime FechaVenta { get; set; }
    }
}

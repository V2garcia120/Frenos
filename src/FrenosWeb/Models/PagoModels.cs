namespace FrenosWeb.Models
{
    // Esta es la "caja principal" que Integración espera recibir
    public class CobroRequest
    {
        public List<CobroItemRequest> Items { get; set; } = new();
        public string MetodoPago { get; set; } = "Tarjeta";
        public decimal MontoPagado { get; set; }
        public int? VehiculoId { get; set; }
    }

    // Esta es la estructura de cada producto/servicio dentro de la lista
    public class CobroItemRequest
    {
        public int ServicioId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioSnapshot { get; set; }
    }

    // Esta es la respuesta que Integración enviará de vuelta
    public class CobroResponse
    {
        public int? FacturaId { get; set; }
        public string? NumeroFactura { get; set; }
        public decimal Total { get; set; }
        public decimal Cambio { get; set; }
        public string Estado { get; set; } = ""; // Aquí llegará el "PendienteSync" si el Core está caído
        public string? IdLocal { get; set; }
    }
}
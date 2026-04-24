namespace FrenosWeb.Models
{
    public class CobroRequest
    {
        public int TurnoId { get; set; }
        public int ClienteId { get; set; }
        public int? VehiculoId { get; set; }
        public List<CobroItemRequest> Items { get; set; } = new();
        public string MetodoPago { get; set; } = "Tarjeta";
        public decimal MontoPagado { get; set; }
    }

    public class CobroItemRequest
    {
        public string Tipo { get; set; } = "Servicio";
        public int ItemId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioSnapshot { get; set; }
    }

    public class CobroResponse
    {
        public int? FacturaId { get; set; }
        public string? NumeroFactura { get; set; }
        public decimal Total { get; set; }
        public decimal Cambio { get; set; }
        public string Estado { get; set; } = "";
        public string? IdLocal { get; set; }
    }
}
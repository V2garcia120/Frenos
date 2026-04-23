namespace TallerCaja.Models.Entities
{
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
}

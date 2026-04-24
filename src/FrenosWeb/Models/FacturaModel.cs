namespace FrenosWeb.Models
{
    public class FacturaModel
    {
        public int Id { get; set; }
        public string NumeroFactura { get; set; } = "";
        public string ServicioRealizado { get; set; } = ""; 
        public decimal Total { get; set; }
        public string EstadoPago { get; set; } = "";
        public DateTime Fecha { get; set; }
    }
}

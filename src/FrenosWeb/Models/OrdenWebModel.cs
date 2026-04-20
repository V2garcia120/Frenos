namespace FrenosWeb.Models
{
    public class OrdenWebModel
    {
        public string NumeroOrden { get; set; } = "";
        public DateTime Fecha { get; set; }
        public string Vehiculo { get; set; } = ""; 
        public string Placa { get; set; } = "";
        public decimal Total { get; set; }
        public string EstadoServicio { get; set; } = "Pendiente"; 
        public string EstadoPago { get; set; } = "Pagado";
    }
}
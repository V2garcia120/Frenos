namespace FrenosCore.Modelos.Entidades
{
    public class Cotizacion
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int VehiculoId { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Itbis { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }
        public string Notas { get; set; }
        public DateOnly ValidoHasta { get; set; }
    }
}

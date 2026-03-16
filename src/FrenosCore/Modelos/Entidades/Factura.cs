namespace FrenosCore.Modelos.Entidades
{
    public class Factura
    {
        public int Id { get; set; }
        public int OrdenId { get; set; }
        public int ClienteId { get; set; }
        public string Numero { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Itbis { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }
        public string MetodoPago { get; set; }
        public int EmitidaPor { get; set; }
    }
}

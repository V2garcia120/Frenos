namespace FrenosCore.Modelos.Entidades
{
    public class FacturaItem
    {
        public int Id { get; set; }
        public int FacturaId { get; set; }
        public string Tipo { get; set; }
        public int ItemId { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }

    }
}

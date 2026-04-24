namespace FrenosWeb.Models
{
    public class FacturaModel
    {
        public int Id { get; set; }
        public int? OrdenId { get; set; }
        public string Numero { get; set; } = "";
        public string TipoOrigen { get; set; } = "";
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; } = "";
        public string VehiculoInfo { get; set; } = "";
        public DateTime Fecha { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Itbis { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; } = "";
        public string? MetodoPago { get; set; }
        public string EmitidaPorNombre { get; set; } = "";
        public List<FacturaItemModel> Items { get; set; } = [];

        public bool EsPagada => Estado == "Pagada";

        public string ServicioResumen
        {
            get
            {
                if (Items.Count == 0)
                    return TipoOrigen == "OrdenReparacion" ? "Servicio de Reparación" : "Venta Directa";
                var desc = string.Join(", ", Items.Take(2).Select(i => i.Descripcion));
                return Items.Count > 2 ? desc + "…" : desc;
            }
        }
    }

    public class FacturaItemModel
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}

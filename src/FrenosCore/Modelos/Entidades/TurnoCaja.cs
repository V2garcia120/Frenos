using System.ComponentModel;

namespace FrenosCore.Modelos.Entidades
{
    public class TurnoCaja
    {
        public int Id { get; set; }
        public int CajeroId { get; set; }
        public decimal? EfectivoContado { get; set; }
        public decimal? TotalVentas {  get; set; }
        public decimal Diferencia { get; set; }
        public decimal MontoInicial { get; set; }
        public DateTime? FechaCierre { get; set; }
        public DateTime FechaApertura { get; set; }
        public string Estado { get; set; }
        public string IdLocalCaja { get; set; }
        public ICollection<Factura>? Facturas { get; set; } = new List<Factura>();

    }
}

namespace FrenosCore.Modelos.Entidades
{
    public class CuentasPorCorbrar
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int FacturaId { get; set; }
        public decimal Monto { get; set; }
        public decimal Saldo { get; set; }
        public DateTime Vencimiento { get; set; }
        public string Estado { get; set; }

        public DateTime CreadoEn { get; set; }

        public Cliente Cliente { get; set; } = null!;
        public Factura Factura { get; set; } = null!;
        public ICollection<AbonoCxC> Abonos { get; set; } = [];
    }
}

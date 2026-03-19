namespace FrenosCore.Modelos.Entidades
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public bool EsAnonimo { get; private set; } = false;
        public DateTime CreadoEn { get; set; }

        public ICollection<Vehiculo> Vehiculos { get; set; } = [];
        public ICollection<Orden> Ordenes { get; set; } = [];
        public ICollection<Cotizacion> Cotizaciones { get; set; } = [];
        public ICollection<Factura> Facturas { get; set; } = [];
        public ICollection<CuentasPorCobrar> CuentasPorCorbrar { get; set; } = [];
    }
}

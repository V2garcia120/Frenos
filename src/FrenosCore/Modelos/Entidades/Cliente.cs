namespace FrenosCore.Modelos.Entidades
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public bool EsAnonimo { get; }
        public DateTime CreadoEn { get; set; }

        public ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
        public ICollection<Orden> Ordenes { get; set; } = new List<Orden>();
        public ICollection<Cotizacion> Cotizaciones { get; set; } = new List<Cotizacion>();
        public ICollection<Factura> Facturas { get; set; } = new List<Factura>();
        public ICollection<CuentasPorCorbrar> CuentasPorCorbrar { get; set; } = new List<CuentasPorCorbrar>();

    }
}

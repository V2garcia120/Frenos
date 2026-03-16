namespace FrenosCore.Modelos.Entidades
{
    public class Vehiculo
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string Placa { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Anno { get; set; }
        public string Color { get; set; }
        public string VIN { get; set; }
        public string TipoCombustible { get; set; }
        public int KmActual { get; set; }
        public string Nota { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

    }
}

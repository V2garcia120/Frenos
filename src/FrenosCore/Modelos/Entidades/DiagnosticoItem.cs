namespace FrenosCore.Modelos.Entidades
{
    public class DiagnosticoItem
    {
        public int Id { get; set; }
        public int DiagnosticoId { get; set; }
        public string SistemaVehiculo { get; set; }
        public string Componente { get; set; }
        public string Condicion { get; set; }
        public string AccionRecomendada { get; set; }
        public string Descripccion { get; set; }
        public int ServicioSugeridoId { get; set; }
        public int ProductoSugeridoId { get; set; }
        public int EsUrgente { get; set; }

        public Diagnostico Diagnostico { get; set; } = null!;
        public Servicio ServicioSugerido { get; set; } = null!;
        public Producto ProductoSugerido { get; set; } = null!;

    }
}

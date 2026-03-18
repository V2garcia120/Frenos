namespace FrenosCore.Modelos.Entidades
{
    public class DiagnosticoItem
    {
        public int Id { get; set; }
        public int DiagnosticoId { get; set; }
        public string SistemaVehiculo { get; set; } = string.Empty;
        public string Componente { get; set; } = string.Empty;
        public string Condicion { get; set; } = string.Empty;
        public string AccionRecomendada { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public int? ServicioSugeridoId { get; set; }
        public int? ProductoSugeridoId { get; set; }
        public int CantidadProductoSugerido { get; set; } = 0;
        public bool EsUrgente { get; set; } = false;

        public Diagnostico Diagnostico { get; set; } = null!;
        public Servicio? ServicioSugerido { get; set; } = null!;
        public Producto? ProductoSugerido { get; set; } = null!;

    }
}

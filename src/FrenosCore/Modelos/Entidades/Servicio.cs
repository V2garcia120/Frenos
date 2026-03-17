namespace FrenosCore.Modelos.Entidades
{
    public class Servicio
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int DuracionMinutos { get; set; }
        public string Categoria { get; set; }
        public bool Activo { get; set; }

        public ICollection<DiagnosticoItem> DiagnosticoItemsSugeridos { get; set; } = [];
    }
}

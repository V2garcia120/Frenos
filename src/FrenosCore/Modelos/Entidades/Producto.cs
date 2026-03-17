namespace FrenosCore.Modelos.Entidades
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public decimal Costo { get; set; }
        public int Stock { get; set; }
        public int StockMinimo { get; set; }
        public string Categoria { get; set; }
        public bool Activo { get; set; }
        public DateTime CreadoEn { get; set; } = DateTime.Now;

        public ICollection<DiagnosticoItem> DiagnosticoItemsSugeridos { get; set; } = new List<DiagnosticoItem>();

    }
}

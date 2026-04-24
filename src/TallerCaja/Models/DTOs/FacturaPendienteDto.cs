namespace TallerCaja.Models.DTOs
{
    public class FacturaPendienteDto
    {
        public int Id { get; set; }
        public string Numero { get; set; } = string.Empty;
        public string ClienteNombre { get; set; } = string.Empty;
        public string VehiculoInfo { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public string Estado { get; set; } = string.Empty;
        public List<ItemFacturaDto> Items { get; set; } = new();
    }

}

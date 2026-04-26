namespace FrenosIntegracion.Models.DTOs
{
    public record VentaWebRequest(
    int ClienteId,
    string MetodoPago,
    decimal MontoPagado,
    IEnumerable<CobroItem> Items
);
}

namespace TallerCaja.Models.DTOs
{
    public class SyncRequest
    {
        public List<SyncItemDto> Transacciones { get; set; } = new();
    }
}

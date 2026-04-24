namespace TallerCaja.Models.DTOs
{
    public class LoginCajeroResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expira { get; set; }
        public int CajeroId { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}

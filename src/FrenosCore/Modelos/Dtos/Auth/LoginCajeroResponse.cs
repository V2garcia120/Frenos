namespace FrenosCore.Modelos.Dtos.Auth
{
    public record LoginCajeroResponse
    {
        public string Token { get; init; } = string.Empty;
        public DateTime Expira { get; init; }
        public int CajeroId { get; init; }
        public string Nombre { get; init; } = string.Empty;
    }
}

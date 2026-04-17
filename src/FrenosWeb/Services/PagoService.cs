namespace FrenosWeb.Services
{
    public class PagoService
    {
        private readonly HttpClient _http;

        public PagoService(HttpClient http)
        {
            _http = http;
        }

        public async Task<bool> ProcesarPagoSimuladoAsync(decimal monto)
        {
            await Task.Delay(2500);

            // Aquí iría el POST a api/pagos en el futuro
            return true;
        }
    }
}
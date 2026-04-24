namespace TallerCaja.Models.DTOs
{
    public class AbonoResponse
    {
        public decimal MontoAbonado { get; set; }
        public decimal SaldoAnterior { get; set; }
        public decimal SaldoActual { get; set; }
        public bool Saldada { get; set; }
        public string EstadoCxC { get; set; } = string.Empty;
    }
}

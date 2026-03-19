namespace FrenosCore.Modelos.Entidades
{
    public class AbonoCxC
    {
        public int Id { get; set; }
        public int CxCId { get; set; }
        public CuentasPorCobrar CxC { get; set; } = null!;
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string MetodoPago { get; set; }

        public int RegistradoPor { get; set; }
        public Usuario RegistradoPorUsuario { get; set; } = null!;
    }
}

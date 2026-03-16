namespace FrenosCore.Modelos.Entidades
{
    public class AbonoCxC
    {
        public int Id { get; set; }
        public int CxCId { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string MetodoPago { get; set; }

        public int RegistradoPor { get; set; }
    }
}

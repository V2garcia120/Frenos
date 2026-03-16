namespace FrenosCore.Modelos.Entidades
{
    public class AuditLog
    {
        public int id { get; set; }
        public int UsuarioId { get; set; }

        public string Accion { get; set; }
        public string Tabla { get; set; }
        public string RegistroId { get; set; }
        public string ValorAntes { get; set; }
        public string ValorDespues { get; set; }
        public string Ip { get; set; }
        public DateTime FechaHora { get; set; }

    }
}

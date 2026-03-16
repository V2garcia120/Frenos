namespace FrenosCore.Modelos.Entidades
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public bool EsAnonimo { get; }
        public DateTime CreadoEn { get; set; }

    }
}

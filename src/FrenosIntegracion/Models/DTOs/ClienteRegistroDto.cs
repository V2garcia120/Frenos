namespace FrenosIntegracion.DTOs
{
    public class ClienteRegistroDto
    {
        // Estos campos son los estándar para un registro, 
        // asegúrate de que coincidan con los del Core:
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Password { get; set; }

        // Si el taller asocia clientes con vehículos desde el inicio, 
        // podrías necesitar campos extra, pero estos son los básicos.
    }
}
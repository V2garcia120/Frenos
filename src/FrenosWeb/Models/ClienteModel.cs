using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FrenosWeb.Models
{
    public class ClienteModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = "";

        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string Apellido { get; set; } = "";

        [Required(ErrorMessage = "El cédula es obligatorio")]
        [JsonPropertyName("Cedula")]
        public string Cedula { get; set; } = "";

        public string? Telefono { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato inválido")]
        [JsonPropertyName("Correo")]
        public string Correo { get; set; } = "";
        

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(6, ErrorMessage = "Mínimo 6 caracteres")]
        public string Password { get; set; } = "";

        public string? Direccion { get; set; }
    }
}
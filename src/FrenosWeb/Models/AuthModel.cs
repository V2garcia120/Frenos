using System.ComponentModel.DataAnnotations;

namespace FrenosWeb.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Password { get; set; } = "";
    }

    public class UserSession
    {
        public string Token { get; set; } = "";
        public string Email { get; set; } = "";
        public string Rol { get; set; } = "";
    }
}
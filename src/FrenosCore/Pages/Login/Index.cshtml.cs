using System.ComponentModel.DataAnnotations;
using FrenosCore.Modelos.Dtos.Auth;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenosCore.Pages.Login
{
    public class IndexModel : PageModel
    {
        private readonly AuthService _authService;

        public IndexModel(AuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public LoginInput Input { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var request = new IniciarSesionRequest(Input.Email, Input.Password);
            var resultado = await _authService.IniciarSesionUsuario(request);

            if (!resultado.Exito || string.IsNullOrWhiteSpace(resultado.Token))
            {
                ModelState.AddModelError(string.Empty, "Credenciales inválidas.");
                return Page();
            }

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.Strict,
                Expires = Input.Recordarme
                    ? DateTimeOffset.UtcNow.AddDays(7)
                    : DateTimeOffset.UtcNow.AddHours(2)
            };

            Response.Cookies.Append("AuthToken", resultado.Token, cookieOptions);

            return RedirectToPage("/Index");
        }

        public class LoginInput
        {
            [Required(ErrorMessage = "El correo es obligatorio.")]
            [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "La contraseña es obligatoria.")]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            public bool Recordarme { get; set; }
        }
    }
}

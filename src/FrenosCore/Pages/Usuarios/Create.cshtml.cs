using System.ComponentModel.DataAnnotations;
using FrenosCore.Modelos.Dtos.Usuario;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrenosCore.Pages.Usuarios
{
    [Authorize(Policy = "SoloAdmin")]
    public class CreateModel : PageModel
    {
        private readonly IUsuarioService _usuarioService;

        public CreateModel(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [BindProperty]
        public UsuarioInput Input { get; set; } = new();

        public IList<SelectListItem> Roles { get; private set; } = [];

        public async Task OnGetAsync()
        {
            await CargarRolesAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await CargarRolesAsync();

            if (!ModelState.IsValid)
                return Page();

            var request = new CrearUsuarioRequest(
                Nombre: Input.Nombre,
                Email: Input.Email,
                Password: Input.Password,
                RolId: Input.RolId,
                Activo: Input.Activo
            );

            try
            {
                await _usuarioService.CrearAsync(request);
                TempData["Mensaje"] = "Usuario creado correctamente.";
                return RedirectToPage("/Usuarios/Index");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
            catch (KeyNotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        private async Task CargarRolesAsync()
        {
            var roles = await _usuarioService.ListarRolesAsync();
            Roles = roles
                .Select(r => new SelectListItem(r.Nombre, r.Id.ToString()))
                .ToList();
        }

        public class UsuarioInput
        {
            [Required]
            [StringLength(150)]
            public string Nombre { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            [StringLength(200)]
            public string Email { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un rol.")]
            public int RolId { get; set; }

            public bool Activo { get; set; } = true;
        }
    }
}

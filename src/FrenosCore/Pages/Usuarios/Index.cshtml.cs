using FrenosCore.Modelos.Dtos.Usuario;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenosCore.Pages.Usuarios
{
    [Authorize(Policy = "SoloAdmin")]
    public class IndexModel : PageModel
    {
        private readonly IUsuarioService _usuarioService;

        public IndexModel(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public IList<UsuarioResponse> Usuarios { get; private set; } = [];

        [BindProperty(SupportsGet = true)]
        public string? Termino { get; set; }

        public async Task OnGetAsync()
        {
            await CargarUsuariosAsync();
        }

        public async Task<IActionResult> OnPostDesactivarAsync(int id)
        {
            try
            {
                await _usuarioService.EliminarAsync(id);
                TempData["Mensaje"] = "Usuario desactivado correctamente.";
            }
            catch (KeyNotFoundException)
            {
                TempData["MensajeError"] = "No se encontró el usuario.";
            }

            return RedirectToPage(new { Termino });
        }

        private async Task CargarUsuariosAsync()
        {
            var usuarios = await _usuarioService.ListarAsync(Termino);
            Usuarios = usuarios.ToList();
        }
    }
}

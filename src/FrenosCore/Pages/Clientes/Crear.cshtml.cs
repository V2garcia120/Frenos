using System.ComponentModel.DataAnnotations;
using FrenosCore.Modelos.Dtos.Cliente;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenosCore.Pages.Clientes
{
    [Authorize(Policy = "Mantenimiento")]
    public class CrearModel : PageModel
    {
        private readonly IClienteService _clienteService;

        public CrearModel(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [BindProperty]
        public ClienteInput Input { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var request = new CrearClienteRequest(
                Nombre: Input.Nombre,
                Cedula: Input.Cedula,
                Telefono: Input.Telefono,
                Email: Input.Email,
                Password: Input.Password,
                Direccion: Input.Direccion
            );

            try
            {
                await _clienteService.CrearAsync(request);
                TempData["Mensaje"] = "Cliente creado correctamente.";
                return RedirectToPage("/Clientes/Index");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        public class ClienteInput
        {
            [Required]
            [StringLength(200)]
            public string Nombre { get; set; } = string.Empty;

            [StringLength(20)]
            public string? Cedula { get; set; }

            [StringLength(20)]
            public string? Telefono { get; set; }

            [EmailAddress]
            [StringLength(200)]
            public string? Email { get; set; }

            [DataType(DataType.Password)]
            public string? Password { get; set; }

            [StringLength(300)]
            public string? Direccion { get; set; }
        }
    }
}

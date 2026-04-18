using System.ComponentModel.DataAnnotations;
using FrenosCore.Modelos.Dtos.Cliente;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenosCore.Pages.Clientes
{
    [Authorize(Policy = "Mantenimiento")]
    public class EditModel : PageModel
    {
        private readonly IClienteService _clienteService;

        public EditModel(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [BindProperty]
        public ClienteInput Input { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var cliente = await _clienteService.ObtenerPorIdAsync(id);
                Input = new ClienteInput
                {
                    Nombre = cliente.Nombre,
                    Cedula = cliente.Cedula,
                    Telefono = cliente.Telefono,
                    Email = cliente.Email,
                    Direccion = cliente.Direccion
                };

                return Page();
            }
            catch (KeyNotFoundException)
            {
                TempData["MensajeError"] = "No se encontró el cliente.";
                return RedirectToPage("/Clientes/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
                return Page();

            var request = new ActualizarClienteRequest(
                Nombre: Input.Nombre,
                Cedula: Input.Cedula,
                Telefono: Input.Telefono,
                Email: Input.Email,
                Direccion: Input.Direccion
            );

            try
            {
                await _clienteService.ActualizarAsync(id, request);
                TempData["Mensaje"] = "Cliente actualizado correctamente.";
                return RedirectToPage("/Clientes/Index");
            }
            catch (KeyNotFoundException)
            {
                TempData["MensajeError"] = "No se encontró el cliente.";
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

            [StringLength(300)]
            public string? Direccion { get; set; }
        }
    }
}

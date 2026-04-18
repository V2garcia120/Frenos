using FrenosCore.Modelos.Dtos;
using FrenosCore.Modelos.Dtos.Cliente;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenosCore.Pages.Clientes
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IClienteService _clienteService;

        public IndexModel(IClienteService service)
        {
            _clienteService = service;
        }

        [BindProperty(SupportsGet = true)]
        public int Pagina { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public string Termino { get; set; } = string.Empty;

        public PaginadoResponse<ClienteResponse> Resultado { get; private set; } =
            new([], 1, 20, 0, 0);

        public bool HayPaginaAnterior => Pagina > 1;

        public bool HayPaginaSiguiente => Resultado.TotalPaginas > 0 && Pagina < Resultado.TotalPaginas;

        public async Task CargarClientes()
        {
            Resultado = string.IsNullOrWhiteSpace(Termino)
                ? await _clienteService.ListarAsync(Pagina, 20, null)
                : await _clienteService.ListarAsync(Pagina, 20, Termino);
        }

        public async Task OnGetAsync()
        {
            Pagina = Math.Max(1, Pagina);
            await CargarClientes();
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            var esAdmin = User.IsInRole("Administrador") || User.IsInRole("Admin");
            if (!esAdmin)
            {
                TempData["MensajeError"] = "Solo el administrador puede eliminar clientes.";
                return RedirectToPage(new { Pagina, Termino });
            }

            try
            {
                await _clienteService.EliminarAsync(id);
                TempData["Mensaje"] = "Cliente eliminado correctamente.";
            }
            catch (KeyNotFoundException)
            {
                TempData["MensajeError"] = "No se encontró el cliente.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["MensajeError"] = ex.Message;
            }

            return RedirectToPage(new { Pagina, Termino });
        }
    }
}

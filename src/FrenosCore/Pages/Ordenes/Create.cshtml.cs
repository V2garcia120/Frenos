using System.ComponentModel.DataAnnotations;
using FrenosCore.Modelos.Dtos.Cliente;
using FrenosCore.Modelos.Dtos.Orden;
using FrenosCore.Modelos.Dtos.Usuario;
using FrenosCore.Modelos.Dtos.Vehiculo;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenosCore.Pages.Ordenes
{
    [Authorize(Policy = "Mantenimiento")]
    public class CreateModel : PageModel
    {
        private readonly IOrdenService _ordenService;
        private readonly IClienteService _clienteService;
        private readonly IVehiculoService _vehiculoService;
        private readonly IUsuarioService _usuarioService;

        public CreateModel(
            IOrdenService ordenService,
            IClienteService clienteService,
            IVehiculoService vehiculoService,
            IUsuarioService usuarioService)
        {
            _ordenService = ordenService;
            _clienteService = clienteService;
            _vehiculoService = vehiculoService;
            _usuarioService = usuarioService;
        }

        [BindProperty(SupportsGet = true)]
        public int? ClienteId { get; set; }

        [BindProperty]
        public OrdenInput Input { get; set; } = new();

        public IList<ClienteResponse> Clientes { get; private set; } = [];
        public IList<VehiculoResponse> Vehiculos { get; private set; } = [];
        public IList<UsuarioResponse> Tecnicos { get; private set; } = [];

        public async Task OnGetAsync()
        {
            if (ClienteId.HasValue && ClienteId.Value > 0)
                Input.ClienteId = ClienteId.Value;

            await CargarCatalogosAsync(Input.ClienteId > 0 ? Input.ClienteId : null);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await CargarCatalogosAsync(Input.ClienteId > 0 ? Input.ClienteId : null);

            if (!ModelState.IsValid)
                return Page();

            var tecnicoId = Input.TecnicoId.HasValue && Input.TecnicoId.Value > 0
                ? Input.TecnicoId
                : null;

            var request = new CrearOrdenRequest(
                ClienteId: Input.ClienteId,
                VehiculoId: Input.VehiculoId,
                TecnicoId: tecnicoId,
                CotizacionId: null,
                Prioridad: Input.Prioridad,
                FechaEntregaEstimada: Input.FechaEntregaEstimada,
                Notas: Input.Notas);

            try
            {
                await _ordenService.CrearAsync(request);
                TempData["Mensaje"] = "Orden creada correctamente.";
                return RedirectToPage("/Ordenes/Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        private async Task CargarCatalogosAsync(int? clienteId)
        {
            var clientes = await _clienteService.ListarAsync(1, 100, null);
            Clientes = clientes.Items.OrderBy(c => c.Nombre).ToList();

            if (clienteId.HasValue && clienteId.Value > 0)
                Vehiculos = (await _vehiculoService.ListarPorClienteAsync(clienteId.Value)).ToList();
            else
                Vehiculos = [];

            var usuarios = await _usuarioService.ListarAsync(null);
            Tecnicos = usuarios
                .Where(u => u.Rol.Contains("tecnico", StringComparison.OrdinalIgnoreCase)
                         || u.Rol.Contains("técnico", StringComparison.OrdinalIgnoreCase))
                .OrderBy(u => u.Nombre)
                .ToList();
        }

        public class OrdenInput
        {
            [Required]
            [Range(1, int.MaxValue)]
            public int ClienteId { get; set; }

            [Required]
            [Range(1, int.MaxValue)]
            public int VehiculoId { get; set; }

            public int? TecnicoId { get; set; }

            [Required]
            public string Prioridad { get; set; } = "Normal";

            public DateTime? FechaEntregaEstimada { get; set; }

            [StringLength(500)]
            public string? Notas { get; set; }
        }
    }
}

using FrenosCore.Modelos.Dtos;
using FrenosCore.Modelos.Dtos.Orden;
using FrenosCore.Modelos.Dtos.Usuario;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenosCore.Pages.Ordenes
{
    [Authorize(Policy = "Mantenimiento")]
    public class IndexModel : PageModel
    {
        private readonly IOrdenService _ordenService;
        private readonly IUsuarioService _usuarioService;

        public IndexModel(IOrdenService ordenService, IUsuarioService usuarioService)
        {
            _ordenService = ordenService;
            _usuarioService = usuarioService;
        }

        [BindProperty(SupportsGet = true)]
        public int Pagina { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public string? Estado { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Prioridad { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? TecnicoId { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? Fecha { get; set; }

        public PaginadoResponse<OrdenResponse> Resultado { get; private set; } = new([], 1, 20, 0, 0);

        public IList<UsuarioResponse> Tecnicos { get; private set; } = [];

        public bool HayPaginaAnterior => Pagina > 1;
        public bool HayPaginaSiguiente => Resultado.TotalPaginas > 0 && Pagina < Resultado.TotalPaginas;

        public async Task OnGetAsync()
        {
            Pagina = Math.Max(1, Pagina);

            var usuarios = await _usuarioService.ListarAsync(null);
            Tecnicos = usuarios
                .Where(u => u.Rol.Contains("tecnico", StringComparison.OrdinalIgnoreCase) ||
                            u.Rol.Contains("técnico", StringComparison.OrdinalIgnoreCase))
                .OrderBy(u => u.Nombre)
                .ToList();

            Resultado = await _ordenService.ListarAsync(Pagina, 20, Estado, Prioridad, TecnicoId, Fecha);
        }
    }
}

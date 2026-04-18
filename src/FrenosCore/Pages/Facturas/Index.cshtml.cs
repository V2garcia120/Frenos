using FrenosCore.Modelos.Dtos;
using FrenosCore.Modelos.Dtos.Factura;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenosCore.Pages.Facturas
{
    [Authorize(Policy = "Mantenimiento")]
    public class IndexModel : PageModel
    {
        private readonly IFacturaService _facturaService;

        public IndexModel(IFacturaService facturaService)
        {
            _facturaService = facturaService;
        }

        [BindProperty(SupportsGet = true)]
        public int Pagina { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public string? Estado { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Numero { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? Fecha { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? TipoOrigen { get; set; }

        public PaginadoResponse<FacturaResponse> Resultado { get; private set; } = new([], 1, 20, 0, 0);

        public bool HayPaginaAnterior => Pagina > 1;
        public bool HayPaginaSiguiente => Resultado.TotalPaginas > 0 && Pagina < Resultado.TotalPaginas;

        public async Task OnGetAsync()
        {
            Pagina = Math.Max(1, Pagina);
            Resultado = await _facturaService.ListarAsync(Pagina, 20, Estado, Numero, Fecha, TipoOrigen);
        }
    }
}

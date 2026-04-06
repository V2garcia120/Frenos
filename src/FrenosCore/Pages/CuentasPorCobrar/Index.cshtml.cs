using FrenosCore.Modelos.Dtos;
using FrenosCore.Modelos.Dtos.CuentasPorCobrar;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenosCore.Pages.CuentasPorCobrar
{
    [Authorize(Policy = "Mantenimiento")]
    public class IndexModel : PageModel
    {
        private readonly ICuentasPorCobrarService _cuentasPorCobrarService;

        public IndexModel(ICuentasPorCobrarService cuentasPorCobrarService)
        {
            _cuentasPorCobrarService = cuentasPorCobrarService;
        }

        [BindProperty(SupportsGet = true)]
        public int Pagina { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public string? Estado { get; set; }

        public PaginadoResponse<CuentaPorCobrarResponse> Resultado { get; private set; } = new([], 1, 20, 0, 0);

        public bool HayPaginaAnterior => Pagina > 1;
        public bool HayPaginaSiguiente => Resultado.TotalPaginas > 0 && Pagina < Resultado.TotalPaginas;

        public async Task OnGetAsync()
        {
            Pagina = Math.Max(1, Pagina);
            Resultado = await _cuentasPorCobrarService.ListarAsync(Pagina, 20, Estado);
        }
    }
}

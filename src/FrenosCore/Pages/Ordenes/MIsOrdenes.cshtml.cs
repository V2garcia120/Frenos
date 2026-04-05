using System.IdentityModel.Tokens.Jwt;
using FrenosCore.Modelos.Dtos;
using FrenosCore.Modelos.Dtos.Orden;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenosCore.Pages.Ordenes
{
    [Authorize(Policy = "SoloTecnico")]
    public class MIsOrdenesModel : PageModel
    {
        private readonly IOrdenService _ordenService;

        public MIsOrdenesModel(IOrdenService ordenService)
        {
            _ordenService = ordenService;
        }

        [BindProperty(SupportsGet = true)]
        public int Pagina { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public string? Estado { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Prioridad { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? Fecha { get; set; }

        public int? TecnicoId { get; private set; }

        public PaginadoResponse<OrdenResponse> Resultado { get; private set; } = new([], 1, 20, 0, 0);

        public bool HayPaginaAnterior => Pagina > 1;
        public bool HayPaginaSiguiente => Resultado.TotalPaginas > 0 && Pagina < Resultado.TotalPaginas;

        public async Task<IActionResult> OnGetAsync()
        {
            if (!Request.Cookies.TryGetValue("AuthToken", out var token) || string.IsNullOrWhiteSpace(token))
                return RedirectToPage("/Login/Index");

            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(token))
                return RedirectToPage("/Login/Index");

            var jwt = handler.ReadJwtToken(token);
            var sub = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

            if (!int.TryParse(sub, out var tecnicoId))
                return RedirectToPage("/Login/Index");

            TecnicoId = tecnicoId;
            Pagina = Math.Max(1, Pagina);

            Resultado = await _ordenService.ListarAsync(Pagina, 20, Estado, Prioridad, TecnicoId, Fecha);
            return Page();
        }
    }
}

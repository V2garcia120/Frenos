using System.IdentityModel.Tokens.Jwt;
using FrenosCore.Modelos.Dtos.Dashboard;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenosCore.Pages.Dashboard
{
    public class TecnicoModel : PageModel
    {
        private readonly IDashboardService _dashboardService;

        public TecnicoModel(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public TecnicoDashboardResponse Data { get; private set; } = new(0, 0, 0, 0, [], []);

        public async Task OnGetAsync()
        {
            int? tecnicoId = null;

            if (Request.Cookies.TryGetValue("AuthToken", out var token) && !string.IsNullOrWhiteSpace(token))
            {
                var handler = new JwtSecurityTokenHandler();
                if (handler.CanReadToken(token))
                {
                    var jwt = handler.ReadJwtToken(token);
                    var sub = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
                    if (int.TryParse(sub, out var parsedId))
                        tecnicoId = parsedId;
                }
            }

            Data = await _dashboardService.ObtenerDashboardTecnicoAsync(tecnicoId);
        }
    }
}

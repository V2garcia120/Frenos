using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using FrenosCore.Modelos.Dtos.Diagnostico;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenosCore.Pages.Diagnosticos
{
    [Authorize(Policy = "SoloTecnico")]
    public class EditModel : PageModel
    {
        private readonly IDiagnosticoService _diagnosticoService;

        public EditModel(IDiagnosticoService diagnosticoService)
        {
            _diagnosticoService = diagnosticoService;
        }

        public DiagnosticoResponse? Diagnostico { get; private set; }

        [BindProperty]
        public DiagnosticoInput Input { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                Diagnostico = await _diagnosticoService.ObtenerPorIdAsync(id);

                if (!EsDiagnosticoDelTecnicoActual(Diagnostico))
                {
                    TempData["MensajeError"] = "No tienes permisos para editar este diagnóstico.";
                    return RedirectToPage("/Ordenes/Detalle", new { id = Diagnostico.OrdenId });
                }

                Input = new DiagnosticoInput
                {
                    KmIngreso = Diagnostico.KmIngreso,
                    DescripcionGeneral = Diagnostico.DescripcionGeneral,
                    RequiereAtencionUrgente = Diagnostico.RequiereAtencionUrgente,
                    ObservacionesTecnico = Diagnostico.ObservacionesTecnico
                };

                return Page();
            }
            catch (KeyNotFoundException)
            {
                TempData["MensajeError"] = "No se encontró el diagnóstico.";
                return RedirectToPage("/Ordenes/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            try
            {
                Diagnostico = await _diagnosticoService.ObtenerPorIdAsync(id);
            }
            catch (KeyNotFoundException)
            {
                TempData["MensajeError"] = "No se encontró el diagnóstico.";
                return RedirectToPage("/Ordenes/Index");
            }

            if (!EsDiagnosticoDelTecnicoActual(Diagnostico!))
            {
                TempData["MensajeError"] = "No tienes permisos para editar este diagnóstico.";
                return RedirectToPage("/Ordenes/Detalle", new { id = Diagnostico!.OrdenId });
            }

            if (!ModelState.IsValid)
                return Page();

            var request = new ActualizarDiagnosticoRequest(
                TecnicoId: null,
                KmIngerso: Input.KmIngreso,
                DescripcionGeneral: Input.DescripcionGeneral,
                Estado: null,
                RequiereAtencionUrgente: Input.RequiereAtencionUrgente,
                AprobadoPorCliente: null,
                FechaAprobacion: null,
                ObservacionesTecnico: Input.ObservacionesTecnico);

            try
            {
                await _diagnosticoService.ActualizarAsync(id, request);
                TempData["Mensaje"] = "Diagnóstico actualizado correctamente.";
                return RedirectToPage("/Ordenes/Detalle", new { id = Diagnostico!.OrdenId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        private bool EsDiagnosticoDelTecnicoActual(DiagnosticoResponse diagnostico)
        {
            var tecnicoId = ObtenerUsuarioIdDesdeToken();
            return tecnicoId.HasValue && tecnicoId.Value == diagnostico.TecnicoId;
        }

        private int? ObtenerUsuarioIdDesdeToken()
        {
            if (!Request.Cookies.TryGetValue("AuthToken", out var token) || string.IsNullOrWhiteSpace(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(token))
                return null;

            var jwt = handler.ReadJwtToken(token);
            var sub = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

            return int.TryParse(sub, out var idUsuario) ? idUsuario : null;
        }

        public class DiagnosticoInput
        {
            public int? KmIngreso { get; set; }

            [Required]
            [StringLength(2000)]
            public string DescripcionGeneral { get; set; } = string.Empty;

            public bool RequiereAtencionUrgente { get; set; }

            [StringLength(2000)]
            public string? ObservacionesTecnico { get; set; }
        }
    }
}

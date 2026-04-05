using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using FrenosCore.Modelos.Dtos.Orden;
using FrenosCore.Modelos.Dtos.Diagnostico;
using FrenosCore.Modelos.Dtos.Usuario;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenosCore.Pages.Ordenes
{
    [Authorize(Roles = "Administrador,Mantenimiento,Tecnico")]
    public class DetalleModel : PageModel
    {
        private readonly IOrdenService _ordenService;
        private readonly IDiagnosticoService _diagnosticoService;
        private readonly IUsuarioService _usuarioService;

        public DetalleModel(
            IOrdenService ordenService,
            IDiagnosticoService diagnosticoService,
            IUsuarioService usuarioService)
        {
            _ordenService = ordenService;
            _diagnosticoService = diagnosticoService;
            _usuarioService = usuarioService;
        }

        public OrdenDetalleResponse? Orden { get; private set; }

        public IList<UsuarioResponse> Tecnicos { get; private set; } = [];

        public int? UsuarioId { get; private set; }
        public string RolUsuario { get; private set; } = string.Empty;

        public bool EsTecnico => RolUsuario is "tecnico" or "técnico";
        public bool EsAdminOMantenimiento =>
            RolUsuario is "admin" or "administrador" or "mantenimiento" or "mantemineto";

        public bool PuedeCrearDiagnostico =>
            EsTecnico && UsuarioId.HasValue && Orden is not null && Orden.Diagnostico is null && Orden.Estado == "EnDiagnostico";

        public bool PuedeEditarDiagnostico =>
            EsTecnico
            && UsuarioId.HasValue
            && Orden?.Diagnostico is not null
            && Orden.Diagnostico.Estado == "Borrador"
            && Orden.Diagnostico.TecnicoId == UsuarioId.Value;

        public bool PuedeCambiarEstado => EsAdminOMantenimiento && Orden is not null;

        public bool PuedeCerrar => EsAdminOMantenimiento && Orden?.Estado == "EnReparacion";

        [BindProperty]
        public DiagnosticoInput DiagnosticoForm { get; set; } = new();

        [BindProperty]
        public CambiarEstadoInput EstadoForm { get; set; } = new();

        [BindProperty]
        public CerrarOrdenInput CerrarForm { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            await PrepararPantallaAsync(id);
            return Orden is null ? RedirectToPage("/Ordenes/Index") : Page();
        }

        public async Task<IActionResult> OnPostCrearDiagnosticoAsync(int id)
        {
            await PrepararPantallaAsync(id);
            if (Orden is null)
                return RedirectToPage("/Ordenes/Index");

            if (!PuedeCrearDiagnostico || !UsuarioId.HasValue)
            {
                TempData["MensajeError"] = "No tienes permisos para crear diagnóstico en esta orden.";
                return RedirectToPage(new { id });
            }

            if (!ModelState.IsValid)
                return Page();

            var request = new CrearDiagnosticoRequest(
                OrdenId: id,
                TecnicoId: UsuarioId.Value,
                KmIngreso: DiagnosticoForm.KmIngreso,
                DescripcionGeneral: DiagnosticoForm.DescripcionGeneral,
                RequiereAtencionUrgente: DiagnosticoForm.RequiereAtencionUrgente,
                ObservacionesTecnico: DiagnosticoForm.ObservacionesTecnico,
                Items: []);

            try
            {
                await _diagnosticoService.CrearAsync(request);
                TempData["Mensaje"] = "Diagnóstico creado correctamente.";
                return RedirectToPage(new { id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostEditarDiagnosticoAsync(int id)
        {
            await PrepararPantallaAsync(id);
            if (Orden?.Diagnostico is null)
                return RedirectToPage(new { id });

            if (!PuedeEditarDiagnostico)
            {
                TempData["MensajeError"] = "No tienes permisos para editar este diagnóstico.";
                return RedirectToPage(new { id });
            }

            if (!ModelState.IsValid)
                return Page();

            var request = new ActualizarDiagnosticoRequest(
                TecnicoId: null,
                KmIngerso: DiagnosticoForm.KmIngreso,
                DescripcionGeneral: DiagnosticoForm.DescripcionGeneral,
                Estado: null,
                RequiereAtencionUrgente: DiagnosticoForm.RequiereAtencionUrgente,
                AprobadoPorCliente: null,
                FechaAprobacion: null,
                ObservacionesTecnico: DiagnosticoForm.ObservacionesTecnico);

            try
            {
                await _diagnosticoService.ActualizarAsync(Orden.Diagnostico.Id, request);
                TempData["Mensaje"] = "Diagnóstico actualizado correctamente.";
                return RedirectToPage(new { id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostCambiarEstadoAsync(int id)
        {
            await PrepararPantallaAsync(id);
            if (!PuedeCambiarEstado)
            {
                TempData["MensajeError"] = "No tienes permisos para cambiar el estado.";
                return RedirectToPage(new { id });
            }

            if (!ModelState.IsValid)
                return Page();

            try
            {
                await _ordenService.CambiarEstadoAsync(id, new CambiarEstadoOrdenRequest(EstadoForm.Estado));
                TempData["Mensaje"] = "Estado actualizado correctamente.";
                return RedirectToPage(new { id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostCerrarOrdenAsync(int id)
        {
            await PrepararPantallaAsync(id);
            if (!PuedeCerrar)
            {
                TempData["MensajeError"] = "No tienes permisos para cerrar la orden.";
                return RedirectToPage(new { id });
            }

            if (!ModelState.IsValid)
                return Page();

            var request = new CerrarOrdenRequest(
                TecnicoId: CerrarForm.TecnicoId,
                KmAlServicio: CerrarForm.KmAlServicio,
                TrabajosRealizados: CerrarForm.TrabajosRealizados,
                ProximoServicioKm: CerrarForm.ProximoServicioKm,
                ProximoServicioFecha: CerrarForm.ProximoServicioFecha,
                MetodoPago: CerrarForm.MetodoPago,
                GarantiaDias: CerrarForm.GarantiaDias);

            try
            {
                await _ordenService.CerrarAsync(id, request);
                TempData["Mensaje"] = "Orden cerrada correctamente.";
                return RedirectToPage(new { id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        private async Task PrepararPantallaAsync(int id)
        {
            CargarUsuarioDesdeToken();

            try
            {
                Orden = await _ordenService.ObtenerPorIdAsync(id);
                EstadoForm.Estado = string.IsNullOrWhiteSpace(EstadoForm.Estado)
                    ? Orden.Estado
                    : EstadoForm.Estado;

                if (Orden.Diagnostico is not null && string.IsNullOrWhiteSpace(DiagnosticoForm.DescripcionGeneral))
                {
                    DiagnosticoForm = new DiagnosticoInput
                    {
                        KmIngreso = Orden.Diagnostico.KmIngreso,
                        DescripcionGeneral = Orden.Diagnostico.DescripcionGeneral,
                        RequiereAtencionUrgente = Orden.Diagnostico.RequiereAtencionUrgente,
                        ObservacionesTecnico = Orden.Diagnostico.ObservacionesTecnico
                    };
                }

                if (PuedeCerrar || CerrarForm.TecnicoId <= 0)
                {
                    var usuarios = await _usuarioService.ListarAsync(null);
                    Tecnicos = usuarios
                        .Where(u => u.Rol.Contains("tecnico", StringComparison.OrdinalIgnoreCase)
                                 || u.Rol.Contains("técnico", StringComparison.OrdinalIgnoreCase))
                        .OrderBy(u => u.Nombre)
                        .ToList();
                }
            }
            catch (KeyNotFoundException)
            {
                TempData["MensajeError"] = "No se encontró la orden.";
                Orden = null;
            }
        }

        private void CargarUsuarioDesdeToken()
        {
            RolUsuario = string.Empty;
            UsuarioId = null;

            if (!Request.Cookies.TryGetValue("AuthToken", out var token) || string.IsNullOrWhiteSpace(token))
                return;

            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(token))
                return;

            var jwt = handler.ReadJwtToken(token);
            RolUsuario = jwt.Claims.FirstOrDefault(c => c.Type == "Rol")?.Value?.Trim().ToLowerInvariant() ?? string.Empty;

            var sub = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (int.TryParse(sub, out var idUsuario))
                UsuarioId = idUsuario;
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

        public class CambiarEstadoInput
        {
            [Required]
            public string Estado { get; set; } = string.Empty;
        }

        public class CerrarOrdenInput
        {
            [Range(1, int.MaxValue)]
            public int TecnicoId { get; set; }

            public int? KmAlServicio { get; set; }

            [Required]
            [StringLength(2000)]
            public string TrabajosRealizados { get; set; } = string.Empty;

            public int? ProximoServicioKm { get; set; }

            public DateOnly? ProximoServicioFecha { get; set; }

            [Required]
            public string MetodoPago { get; set; } = "Efectivo";

            [Range(0, 365)]
            public int GarantiaDias { get; set; } = 30;
        }
    }
}

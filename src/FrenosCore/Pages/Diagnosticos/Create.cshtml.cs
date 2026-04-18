using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using FrenosCore.Modelos.Dtos.Diagnostico;
using FrenosCore.Modelos.Dtos.Orden;
using FrenosCore.Modelos.Dtos.Producto;
using FrenosCore.Modelos.Dtos.Servicio;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenosCore.Pages.Diagnosticos
{
    [Authorize(Policy = "SoloTecnico")]
    public class CreateModel : PageModel
    {
        private readonly IDiagnosticoService _diagnosticoService;
        private readonly IOrdenService _ordenService;
        private readonly IServiciciosService _serviciosService;
        private readonly IProductoService _productoService;

        public CreateModel(
            IDiagnosticoService diagnosticoService,
            IOrdenService ordenService,
            IServiciciosService serviciosService,
            IProductoService productoService)
        {
            _diagnosticoService = diagnosticoService;
            _ordenService = ordenService;
            _serviciosService = serviciosService;
            _productoService = productoService;
        }

        public OrdenDetalleResponse? Orden { get; private set; }
        public IList<ServicioResponse> Servicios { get; private set; } = [];
        public IList<ProductoResponse> Productos { get; private set; } = [];

        [BindProperty]
        public DiagnosticoInput Input { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int ordenId)
        {
            try
            {
                Orden = await _ordenService.ObtenerPorIdAsync(ordenId);
                if (Orden.Diagnostico is not null)
                    return RedirectToPage("/Diagnosticos/Edit", new { id = Orden.Diagnostico.Id });

                if (Orden.Estado != "EnDiagnostico")
                {
                    TempData["MensajeError"] = "La orden debe estar en estado EnDiagnostico para crear diagnóstico.";
                    return RedirectToPage("/Ordenes/Detalle", new { id = ordenId });
                }

                if (Input.Items.Count == 0)
                    Input.Items.Add(new DiagnosticoItemInput());

                await CargarCatalogosAsync();

                return Page();
            }
            catch (KeyNotFoundException)
            {
                TempData["MensajeError"] = "No se encontró la orden.";
                return RedirectToPage("/Ordenes/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync(int ordenId)
        {
            try
            {
                Orden = await _ordenService.ObtenerPorIdAsync(ordenId);
            }
            catch (KeyNotFoundException)
            {
                TempData["MensajeError"] = "No se encontró la orden.";
                return RedirectToPage("/Ordenes/Index");
            }

            await CargarCatalogosAsync();

            Input.Items ??= [];
            var itemsConDatos = Input.Items
                .Where(i =>
                    !string.IsNullOrWhiteSpace(i.SistemaVehiculo)
                    || !string.IsNullOrWhiteSpace(i.Componente)
                    || i.ServicioSugeridoId.HasValue
                    || i.ProductoSugeridoId.HasValue)
                .ToList();

            if (itemsConDatos.Count == 0)
                ModelState.AddModelError(string.Empty, "Debe agregar al menos un item con servicio o producto sugerido.");

            foreach (var item in itemsConDatos)
            {
                if (string.IsNullOrWhiteSpace(item.SistemaVehiculo)
                    || string.IsNullOrWhiteSpace(item.Componente)
                    || string.IsNullOrWhiteSpace(item.Condicion)
                    || string.IsNullOrWhiteSpace(item.AccionRecomendada))
                {
                    ModelState.AddModelError(string.Empty, "Cada item debe incluir sistema, componente, condición y acción recomendada.");
                    break;
                }
            }

            if (!ModelState.IsValid)
                return Page();

            var tecnicoId = ObtenerUsuarioIdDesdeToken();
            if (!tecnicoId.HasValue)
            {
                TempData["MensajeError"] = "No se pudo identificar el técnico autenticado.";
                return RedirectToPage("/Login/Index");
            }

            var request = new CrearDiagnosticoRequest(
                OrdenId: ordenId,
                TecnicoId: tecnicoId.Value,
                KmIngreso: Input.KmIngreso,
                DescripcionGeneral: Input.DescripcionGeneral,
                RequiereAtencionUrgente: Input.RequiereAtencionUrgente,
                ObservacionesTecnico: Input.ObservacionesTecnico,
                Items: itemsConDatos.Select(i => new CrearDiagnosticoItemRequest(
                    SistemaVehiculo: i.SistemaVehiculo.Trim(),
                    Componente: i.Componente.Trim(),
                    Condicion: i.Condicion,
                    AccionRecomendada: i.AccionRecomendada,
                    Descripcion: i.Descripcion,
                    ServicioSugeridoId: i.ServicioSugeridoId,
                    ProductoSugeridoId: i.ProductoSugeridoId,
                    EsUrgente: i.EsUrgente,
                    CantidadProductoSugerida: i.CantidadProductoSugerida)));

            try
            {
                await _diagnosticoService.CrearAsync(request);

                TempData["Mensaje"] = "Diagnóstico creado correctamente.";
                return RedirectToPage("/Ordenes/Detalle", new { id = ordenId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        private async Task CargarCatalogosAsync()
        {
            Servicios = (await _serviciosService.ListarAsync()).ToList();
            Productos = (await _productoService.ListarTodosAsync(null)).ToList();
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

            public List<DiagnosticoItemInput> Items { get; set; } = [];
        }

        public class DiagnosticoItemInput
        {
            public string SistemaVehiculo { get; set; } = string.Empty;
            public string Componente { get; set; } = string.Empty;
            public string Condicion { get; set; } = "Bueno";
            public string AccionRecomendada { get; set; } = "Revisar";
            public string? Descripcion { get; set; }
            public int? ServicioSugeridoId { get; set; }
            public int? ProductoSugeridoId { get; set; }
            public bool EsUrgente { get; set; }
            public int CantidadProductoSugerida { get; set; }
        }
    }
}

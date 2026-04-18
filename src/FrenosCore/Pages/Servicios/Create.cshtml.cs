using System.ComponentModel.DataAnnotations;
using FrenosCore.Modelos.Dtos.Servicio;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenosCore.Pages.Servicios
{
    [Authorize(Policy = "Mantenimiento")]
    public class CreateModel : PageModel
    {
        private readonly IServiciciosService _serviciosService;

        public CreateModel(IServiciciosService serviciosService)
        {
            _serviciosService = serviciosService;
        }

        [BindProperty]
        public ServicioInput Input { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var request = new CrearServicioRequest(
                Nombre: Input.Nombre,
                Descripcion: Input.Descripcion,
                Precio: Input.Precio,
                DuracionMinutos: Input.DuracionMinutos,
                Categoria: Input.Categoria,
                Activo: Input.Activo
            );

            await _serviciosService.CrearAsync(request);

            TempData["Mensaje"] = "Servicio creado correctamente.";
            return RedirectToPage("/Servicios/Index");
        }

        public class ServicioInput
        {
            [Required]
            [StringLength(120)]
            public string Nombre { get; set; } = string.Empty;

            [Required]
            [StringLength(400)]
            public string Descripcion { get; set; } = string.Empty;

            [Range(0.01, 9999999)]
            public decimal Precio { get; set; }

            [Range(1, 10080)]
            public int DuracionMinutos { get; set; }

            [Required]
            [StringLength(80)]
            public string Categoria { get; set; } = string.Empty;

            public bool Activo { get; set; } = true;
        }
    }
}

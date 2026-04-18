using System.ComponentModel.DataAnnotations;
using FrenosCore.Modelos.Dtos;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenosCore.Pages.Productos
{
    [Authorize(Policy = "Mantenimiento")]
    public class CreateModel : PageModel
    {
        private readonly IProductoService _productoService;

        public CreateModel(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [BindProperty]
        public ProductoInput Input { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var request = new CrearProductoRequest(
                Nombre: Input.Nombre,
                Descripcion: Input.Descripcion,
                Precio: Input.Precio,
                Costo: Input.Costo,
                Stock: Input.Stock,
                StockMinimo: Input.StockMinimo,
                Categoria: Input.Categoria,
                Activo: Input.Activo
            );

            await _productoService.CrearProductoAsync(request);

            TempData["Mensaje"] = "Producto creado correctamente.";
            return RedirectToPage("/Productos/Index");
        }

        public class ProductoInput
        {
            [Required]
            [StringLength(120)]
            public string Nombre { get; set; } = string.Empty;

            [Required]
            [StringLength(400)]
            public string Descripcion { get; set; } = string.Empty;

            [Range(0.01, 9999999)]
            public decimal Precio { get; set; }

            [Range(0, 9999999)]
            public decimal Costo { get; set; }

            [Range(0, 999999)]
            public int Stock { get; set; }

            [Range(0, 999999)]
            public int StockMinimo { get; set; }

            [Required]
            [StringLength(80)]
            public string Categoria { get; set; } = string.Empty;

            public bool Activo { get; set; } = true;
        }
    }
}

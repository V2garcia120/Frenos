using System.ComponentModel.DataAnnotations;
using FrenosCore.Modelos.Dtos.Producto;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenosCore.Pages.Productos
{
    [Authorize(Policy = "Mantenimiento")]
    public class EditModel : PageModel
    {
        private readonly IProductoService _productoService;

        public EditModel(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [BindProperty]
        public ProductoInput Input { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var producto = await _productoService.ObtenerProductoPorIdAsync(id);
                Input = new ProductoInput
                {
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion,
                    Precio = producto.Precio,
                    Costo = producto.Costo,
                    Stock = producto.Stock,
                    StockMinimo = producto.StockMinimo,
                    Categoria = producto.Categoria,
                    Activo = producto.Activo
                };

                return Page();
            }
            catch (KeyNotFoundException)
            {
                TempData["MensajeError"] = "No se encontró el producto.";
                return RedirectToPage("/Productos/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
                return Page();

            var request = new ActualizarProductoRequest(
                Nombre: Input.Nombre,
                Descripcion: Input.Descripcion,
                Precio: Input.Precio,
                Costo: Input.Costo,
                Stock: Input.Stock,
                StockMinimo: Input.StockMinimo,
                Categoria: Input.Categoria,
                Activo: Input.Activo
            );

            try
            {
                await _productoService.ActualizarProductoAsync(id, request);
                TempData["Mensaje"] = "Producto actualizado correctamente.";
                return RedirectToPage("/Productos/Index");
            }
            catch (KeyNotFoundException)
            {
                TempData["MensajeError"] = "No se encontró el producto.";
                return RedirectToPage("/Productos/Index");
            }
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

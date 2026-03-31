using FrenosCore.Modelos.Dtos.Producto;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenosCore.Pages.Productos
{
    public class IndexModel : PageModel
    {
        private readonly IProductoService _productoService;

        public IndexModel(IProductoService productoService)
        {
            _productoService = productoService;
        }

        public IList<ProductoResponse> Productos { get; private set; } = [];

        [BindProperty(SupportsGet = true)]
        public string? Termino { get; set; }

        public async Task OnGetAsync()
        {
            await CargarProductosAsync();
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            try
            {
                await _productoService.EliminarProductoAsync(id);
                TempData["Mensaje"] = "Producto eliminado correctamente.";
            }
            catch (KeyNotFoundException)
            {
                TempData["MensajeError"] = "No se encontró el producto.";
            }

            return RedirectToPage(new { Termino });
        }

        private async Task CargarProductosAsync()
        {
            var productos = string.IsNullOrWhiteSpace(Termino)
                ? await _productoService.ListarTodosAsync(null)
                : await _productoService.BuscarAsync(Termino);

            Productos = productos.ToList();
        }
    }
}

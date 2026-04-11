using FrenosWeb.Models;

namespace FrenosWeb.Services
{
    public class ProductoService
    {
        public List<Producto> GetProductos()
        {
            return new List<Producto>
            {
                new Producto { Id = 1, Nombre = "Pastillas de freno", Precio = 1500 },
                new Producto { Id = 2, Nombre = "Disco de freno", Precio = 3000 },
                new Producto { Id = 3, Nombre = "Líquido de frenos", Precio = 800 }
            };
        }
    }
}

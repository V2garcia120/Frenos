using FrenosWeb.Models;

namespace FrenosWeb.Services
{
    public class CarritoStateService
    {
        public List<CarritoItem> Items { get; private set; } = new();
        // Evento para avisar a la interfaz que algo cambio (para actualizar el contador)
        public event Action? OnChange;

        public void Agregar(Producto producto)
        {
            var itemExistente = Items.FirstOrDefault(x => x.Producto.Id == producto.Id);

            if (itemExistente != null)
            {
                itemExistente.Cantidad++;
            }
            else
            {
                // Forzamos que la cantidad inicial siempre sea 1
                Items.Add(new CarritoItem { Producto = producto, Cantidad = 1 });
            }
            NotifyStateChanged();
        }

        public void ActualizarCantidad(int productoId, int nuevaCantidad)
        {
            if (nuevaCantidad < 1) return; // Bloqueo de seguridad

            var item = Items.FirstOrDefault(x => x.Producto.Id == productoId);
            if (item != null)
            {
                item.Cantidad = nuevaCantidad;
                NotifyStateChanged();
            }
        }

        public void Eliminar(int productoId)
        {
            var item = Items.FirstOrDefault(x => x.Producto.Id == productoId);
            if (item != null)
            {
                Items.Remove(item);
                NotifyStateChanged();
            }
        }

        public decimal Subtotal
        {
            get
            {
                foreach (var item in Items)
                {
                    if (item.Cantidad < 1)
                    {
                        item.Cantidad = 1;
                    }
                }

                return Items.Sum(x => x.TotalLinea);
            }
        }
        public decimal Impuesto => Subtotal * 0.18m;
        public decimal TotalFinal => Subtotal + Impuesto;

        public void Limpiar() { Items.Clear(); NotifyStateChanged(); }
        private void NotifyStateChanged() => OnChange?.Invoke();

        public void Actualizar()
        {
            NotifyStateChanged();
        }
    }
}

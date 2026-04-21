using FrenosWeb.Models;
using Microsoft.JSInterop;
using System.Text.Json;

namespace FrenosWeb.Services
{
    public class CarritoStateService
    {
        private readonly IJSRuntime _js;
        public List<CarritoItem> Items { get; private set; } = new();
        public event Action? OnChange;

        public CarritoStateService(IJSRuntime js) => _js = js;

        public async Task InicializarAsync()
        {
            try
            {
                var json = await _js.InvokeAsync<string>("localStorage.getItem", "carrito_frenos");
                if (!string.IsNullOrEmpty(json))
                {
                    Items = JsonSerializer.Deserialize<List<CarritoItem>>(json) ?? new();
                    NotifyStateChanged();
                }
            }
            catch (Exception ex)
            {
                // No bloqueamos al usuario, pero dejamos evidencia en la consola (F12)
                Console.WriteLine($"[Cyber-Error] Fallo al cargar el carrito: {ex.Message}");

                // Si el JSON está dañado, mejor limpiamos el storage para evitar que siga fallando
                await _js.InvokeVoidAsync("localStorage.removeItem", "carrito_frenos");
            }
        }

        private async Task GuardarEnLocal()
        {
            var json = JsonSerializer.Serialize(Items);
            await _js.InvokeVoidAsync("localStorage.setItem", "carrito_frenos", json);
        }

        public async Task Agregar(Servicio servicio)
        {
            var itemExistente = Items.FirstOrDefault(x =>
                x.Servicio.Id == servicio.Id &&
                x.Servicio.Nombre == servicio.Nombre &&
                x.Servicio.RequiereVehiculo == servicio.RequiereVehiculo
                );
            if (itemExistente != null)
            {
                itemExistente.Cantidad++;
            }
            else
            {
                Items.Add(new CarritoItem { Servicio = servicio, Cantidad = 1 });
            }

            await GuardarEnLocal();
            NotifyStateChanged();
        }

        public async Task Eliminar(int servicioId)
        {
            var item = Items.FirstOrDefault(x => x.Servicio.Id == servicioId);
            if (item != null)
            {
                Items.Remove(item);
                await GuardarEnLocal(); 
                NotifyStateChanged();
            }
        }

        public async Task Actualizar()
        {
            await GuardarEnLocal(); // Si cambian cantidades en la UI, guardamos
            NotifyStateChanged();
        }

        public async Task Limpiar()
        {
            Items.Clear();
            await GuardarEnLocal();
            NotifyStateChanged();
        }

        public async Task ActualizarCantidad(int servicioId, int nuevaCantidad)
        {
            if (nuevaCantidad < 1) return;

            var item = Items.FirstOrDefault(x => x.Servicio.Id == servicioId);
            if (item != null)
            {
                item.Cantidad = nuevaCantidad;

                await GuardarEnLocal(); 
                NotifyStateChanged(); 
            }
        }

        public decimal Subtotal
        {
            get
            {
                foreach (var item in Items)
                {
                    if (item.Cantidad < 1) item.Cantidad = 1;
                }
                return Items.Sum(x => x.TotalLinea);
            }
        }

        public decimal Impuesto => Subtotal * 0.18m;
        public decimal TotalFinal => Subtotal + Impuesto;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}

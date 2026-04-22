using TallerCaja.Models.DTOs;
using TallerCaja.Helpers;
using Newtonsoft.Json;
using System.Text;

namespace TallerCaja.Services
{
    // ── Interfaz del servicio de Integración ──────────────────────────────────
    // PUNTO DE CONEXIÓN: Cuando Integración entregue sus APIs reales,
    // solo modifica IntegracionService. Esta interfaz no cambia.
    public interface IIntegracionService
    {
        Task<HealthCheckDto?> HealthCheckAsync();
        Task<LoginCajeroResponse?> LoginCajeroAsync(string email, string password);
        Task<List<ProductoDto>> ObtenerProductosAsync(string? categoria = null);
        Task<List<ServicioDto>> ObtenerServiciosAsync();
        Task<BusquedaCatalogoDto?> BuscarCatalogoAsync(string q);
        Task<List<ClienteDto>> BuscarClientesAsync(string q);
        Task<ClienteDto?> ObtenerClienteAnonimoAsync();
        Task<AbrirTurnoResponse?> AbrirTurnoAsync(AbrirTurnoRequest request);
        Task<CerrarTurnoResponse?> CerrarTurnoAsync(CerrarTurnoRequest request);
        Task<CobroResponse?> ProcesarCobroAsync(CobroRequest request);
        Task<FacturaPendienteDto?> BuscarFacturaPendienteAsync(string? placa = null, string? numero = null);
        Task<PagoFacturaResponse?> PagarFacturaAsync(int facturaId, PagoFacturaRequest request);
        Task<AbonoResponse?> RegistrarAbonoAsync(int cxcId, AbonoRequest request);
        Task<SyncResponse?> SincronizarAsync(SyncRequest request);
    }

    // ── Implementación real usando HttpClient ─────────────────────────────────
    public class IntegracionService : IIntegracionService
    {
        private readonly HttpClient _http;

        public IntegracionService(HttpClient http)
        {
            _http = http;
        }

        // Helper para serializar y enviar
        private StringContent ToJson(object obj) =>
            new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

        private async Task<T?> GetAsync<T>(string url)
        {
            var resp = await _http.GetAsync(url);
            if (!resp.IsSuccessStatusCode) return default;
            var json = await resp.Content.ReadAsStringAsync();
            var api = JsonConvert.DeserializeObject<ApiResponse<T>>(json);
            return api != null && api.Success ? api.Data : default;
        }

        private async Task<T?> PostAsync<T>(string url, object body)
        {
            var resp = await _http.PostAsync(url, ToJson(body));
            if (!resp.IsSuccessStatusCode) return default;
            var json = await resp.Content.ReadAsStringAsync();
            var api = JsonConvert.DeserializeObject<ApiResponse<T>>(json);
            return api != null && api.Success ? api.Data : default;
        }

        public async Task<HealthCheckDto?> HealthCheckAsync()
            => await GetAsync<HealthCheckDto>("/int/auth/health");

        public async Task<LoginCajeroResponse?> LoginCajeroAsync(string email, string password)
            => await PostAsync<LoginCajeroResponse>("/int/auth/login-cajero",
               new LoginCajeroRequest { Email = email, Password = password });

        public async Task<List<ProductoDto>> ObtenerProductosAsync(string? categoria = null)
        {
            var url = "/int/catalogo/productos";
            if (!string.IsNullOrEmpty(categoria)) url += $"?categoria={Uri.EscapeDataString(categoria)}";
            return await GetAsync<List<ProductoDto>>(url) ?? new();
        }

        public async Task<List<ServicioDto>> ObtenerServiciosAsync()
            => await GetAsync<List<ServicioDto>>("/int/catalogo/servicios") ?? new();

        public async Task<BusquedaCatalogoDto?> BuscarCatalogoAsync(string q)
            => await GetAsync<BusquedaCatalogoDto>($"/int/catalogo/buscar?q={Uri.EscapeDataString(q)}");

        public async Task<List<ClienteDto>> BuscarClientesAsync(string q)
            => await GetAsync<List<ClienteDto>>($"/int/clientes/buscar?q={Uri.EscapeDataString(q)}") ?? new();

        public async Task<ClienteDto?> ObtenerClienteAnonimoAsync()
        {
            // PUNTO DE CONEXIÓN: Si el endpoint del cliente anónimo existe en Integración, reemplazar
            // Fallback local hasta que esté disponible
            return new ClienteDto { Id = 1, Nombre = "Cliente Anónimo", EsAnonimo = true };
        }

        public async Task<AbrirTurnoResponse?> AbrirTurnoAsync(AbrirTurnoRequest request)
            => await PostAsync<AbrirTurnoResponse>("/int/caja/turno/abrir", request);

        public async Task<CerrarTurnoResponse?> CerrarTurnoAsync(CerrarTurnoRequest request)
            => await PostAsync<CerrarTurnoResponse>("/int/caja/turno/cerrar", request);

        public async Task<CobroResponse?> ProcesarCobroAsync(CobroRequest request)
            => await PostAsync<CobroResponse>("/int/caja/cobro", request);

        public async Task<FacturaPendienteDto?> BuscarFacturaPendienteAsync(string? placa = null, string? numero = null)
        {
            var url = "/int/caja/facturas/buscar?";
            if (!string.IsNullOrEmpty(placa)) url += $"placa={Uri.EscapeDataString(placa)}";
            else if (!string.IsNullOrEmpty(numero)) url += $"numero={Uri.EscapeDataString(numero)}";
            return await GetAsync<FacturaPendienteDto>(url);
        }

        public async Task<PagoFacturaResponse?> PagarFacturaAsync(int facturaId, PagoFacturaRequest request)
            => await PostAsync<PagoFacturaResponse>($"/int/caja/facturas/{facturaId}/pago", request);

        public async Task<AbonoResponse?> RegistrarAbonoAsync(int cxcId, AbonoRequest request)
            => await PostAsync<AbonoResponse>($"/int/caja/cxc/{cxcId}/abono", request);

        public async Task<SyncResponse?> SincronizarAsync(SyncRequest request)
            => await PostAsync<SyncResponse>("/int/caja/sync", request);
    }

    // ── Mock de Integración (usado cuando no existe la API real) ──────────────
    // IMPORTANTE: Reemplazar IntegracionMockService por IntegracionService
    // en Program.cs cuando las APIs reales de Integración estén disponibles.
    public class IntegracionMockService : IIntegracionService
    {
        public Task<HealthCheckDto?> HealthCheckAsync() =>
            Task.FromResult<HealthCheckDto?>(new HealthCheckDto
            {
                Integracion = "online",
                Core = "offline",
                ModoCache = true,
                UltimaSync = DateTime.Now.AddMinutes(-15)
            });

        public Task<LoginCajeroResponse?> LoginCajeroAsync(string email, string password)
        {
            // MOCK: Acepta cualquier cajero para pruebas
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return Task.FromResult<LoginCajeroResponse?>(null);

            return Task.FromResult<LoginCajeroResponse?>(new LoginCajeroResponse
            {
                Token = "mock-token-" + Guid.NewGuid().ToString("N")[..8],
                Expira = DateTime.Now.AddHours(8),
                CajeroId = 1,
                Nombre = email.Contains("@") ? email.Split('@')[0].ToUpper() : "CAJERO DEMO"
            });
        }

        public Task<List<ProductoDto>> ObtenerProductosAsync(string? categoria = null) =>
            Task.FromResult(new List<ProductoDto>
            {
                new() { Id = 1, Nombre = "Pastilla de Freno Delantera", Precio = 1850.00m, Stock = 24, Categoria = "Frenos" },
                new() { Id = 2, Nombre = "Pastilla de Freno Trasera", Precio = 1650.00m, Stock = 18, Categoria = "Frenos" },
                new() { Id = 3, Nombre = "Disco de Freno 280mm", Precio = 3200.00m, Stock = 10, Categoria = "Frenos" },
                new() { Id = 4, Nombre = "Disco de Freno 300mm", Precio = 3800.00m, Stock = 8, Categoria = "Frenos" },
                new() { Id = 5, Nombre = "Líquido de Frenos DOT4 500ml", Precio = 420.00m, Stock = 35, Categoria = "Fluidos" },
                new() { Id = 6, Nombre = "Líquido de Frenos DOT3 500ml", Precio = 380.00m, Stock = 28, Categoria = "Fluidos" },
                new() { Id = 7, Nombre = "Calibrador de Freno Delantero", Precio = 5500.00m, Stock = 6, Categoria = "Frenos" },
                new() { Id = 8, Nombre = "Kit de Reparación de Frenos", Precio = 2200.00m, Stock = 15, Categoria = "Frenos" },
                new() { Id = 9, Nombre = "Manguera de Freno Universal", Precio = 850.00m, Stock = 20, Categoria = "Frenos" },
                new() { Id = 10, Nombre = "Sensor ABS Delantero", Precio = 2800.00m, Stock = 12, Categoria = "Electrónico" },
            });

        public Task<List<ServicioDto>> ObtenerServiciosAsync() =>
            Task.FromResult(new List<ServicioDto>
            {
                new() { Id = 1, Nombre = "Cambio de Pastillas Delanteras", Precio = 1200.00m, DuracionMin = 60, Categoria = "Mantenimiento" },
                new() { Id = 2, Nombre = "Cambio de Pastillas Traseras", Precio = 1000.00m, DuracionMin = 60, Categoria = "Mantenimiento" },
                new() { Id = 3, Nombre = "Rectificación de Discos", Precio = 2500.00m, DuracionMin = 120, Categoria = "Reparación" },
                new() { Id = 4, Nombre = "Cambio de Discos Delanteros", Precio = 1800.00m, DuracionMin = 90, Categoria = "Reparación" },
                new() { Id = 5, Nombre = "Sangrado del Sistema de Frenos", Precio = 800.00m, DuracionMin = 45, Categoria = "Mantenimiento" },
                new() { Id = 6, Nombre = "Inspección General de Frenos", Precio = 500.00m, DuracionMin = 30, Categoria = "Diagnóstico" },
                new() { Id = 7, Nombre = "Cambio de Líquido de Frenos", Precio = 650.00m, DuracionMin = 40, Categoria = "Mantenimiento" },
                new() { Id = 8, Nombre = "Reparación de Calibrador", Precio = 1500.00m, DuracionMin = 90, Categoria = "Reparación" },
            });

        public async Task<BusquedaCatalogoDto?> BuscarCatalogoAsync(string q)
        {
            var productos = await ObtenerProductosAsync();
            var servicios = await ObtenerServiciosAsync();
            var qLower = q.ToLowerInvariant();
            return new BusquedaCatalogoDto
            {
                Productos = productos.Where(p => p.Nombre.ToLowerInvariant().Contains(qLower)).ToList(),
                Servicios = servicios.Where(s => s.Nombre.ToLowerInvariant().Contains(qLower)).ToList()
            };
        }

        public Task<List<ClienteDto>> BuscarClientesAsync(string q) =>
            Task.FromResult(new List<ClienteDto>
            {
                new() { Id = 2, Nombre = "Juan Pérez", Cedula = "001-1234567-8", Telefono = "809-555-0001" },
                new() { Id = 3, Nombre = "María García", Cedula = "002-9876543-1", Telefono = "809-555-0002" },
                new() { Id = 4, Nombre = "Carlos Rodríguez", Cedula = "003-1111111-1", Telefono = "809-555-0003" },
            }.Where(c => c.Nombre.ToLowerInvariant().Contains(q.ToLowerInvariant()) ||
                         c.Cedula.Contains(q)).ToList());

        public Task<ClienteDto?> ObtenerClienteAnonimoAsync() =>
            Task.FromResult<ClienteDto?>(new ClienteDto { Id = 1, Nombre = "Cliente Anónimo", EsAnonimo = true });

        public Task<AbrirTurnoResponse?> AbrirTurnoAsync(AbrirTurnoRequest request) =>
            Task.FromResult<AbrirTurnoResponse?>(new AbrirTurnoResponse
            {
                TurnoId = new Random().Next(100, 999),
                Estado = "Abierto",
                FechaApertura = DateTime.Now
            });

        public Task<CerrarTurnoResponse?> CerrarTurnoAsync(CerrarTurnoRequest request) =>
            Task.FromResult<CerrarTurnoResponse?>(new CerrarTurnoResponse
            {
                TurnoId = request.TurnoId,
                EfectivoEsperado = 15000m,
                EfectivoContado = request.EfectivoContado,
                Diferencia = request.EfectivoContado - 15000m,
                TotalVentas = 12500m
            });

        public Task<CobroResponse?> ProcesarCobroAsync(CobroRequest request)
        {
            var total = request.Items.Sum(i => i.PrecioSnapshot * i.Cantidad);
            var totalConItbis = FacturaCalculator.CalcularTotal(total);
            var cambio = request.MetodoPago == "Efectivo"
                ? FacturaCalculator.CalcularCambio(totalConItbis, request.MontoPagado)
                : 0m;

            return Task.FromResult<CobroResponse?>(new CobroResponse
            {
                FacturaId = new Random().Next(1000, 9999),
                NumeroFactura = $"FAC-{DateTime.Now.Year}-{new Random().Next(1000, 9999):D4}",
                Total = totalConItbis,
                Cambio = cambio,
                Estado = "Completado",
                IdLocal = Guid.NewGuid().ToString()
            });
        }

        public Task<FacturaPendienteDto?> BuscarFacturaPendienteAsync(string? placa = null, string? numero = null) =>
            Task.FromResult<FacturaPendienteDto?>(new FacturaPendienteDto
            {
                Id = 500,
                Numero = "FAC-2026-0500",
                ClienteNombre = "Juan Pérez",
                VehiculoInfo = "Toyota Corolla 2020 - ABC123",
                Total = 8260.00m,
                Estado = "Pendiente",
                Items = new List<ItemFacturaDto>
                {
                    new() { Nombre = "Cambio de Pastillas Delanteras", Cantidad = 1, Precio = 1200m, Subtotal = 1200m },
                    new() { Nombre = "Pastilla de Freno Delantera", Cantidad = 2, Precio = 1850m, Subtotal = 3700m },
                    new() { Nombre = "ITBIS 18%", Cantidad = 1, Precio = 900m, Subtotal = 900m },
                }
            });

        public Task<PagoFacturaResponse?> PagarFacturaAsync(int facturaId, PagoFacturaRequest request) =>
            Task.FromResult<PagoFacturaResponse?>(new PagoFacturaResponse
            {
                FacturaId = facturaId,
                Numero = $"FAC-{DateTime.Now.Year}-{facturaId:D4}",
                Total = request.MontoPagado,
                Cambio = 0m,
                Estado = request.MetodoPago == "Credito" ? "Pendiente" : "Pagada"
            });

        public Task<AbonoResponse?> RegistrarAbonoAsync(int cxcId, AbonoRequest request) =>
            Task.FromResult<AbonoResponse?>(new AbonoResponse
            {
                MontoAbonado = request.Monto,
                SaldoAnterior = 5000m,
                SaldoActual = 5000m - request.Monto,
                Saldada = request.Monto >= 5000m,
                EstadoCxC = request.Monto >= 5000m ? "Pagada" : "PagoParcial"
            });

        public Task<SyncResponse?> SincronizarAsync(SyncRequest request) =>
            Task.FromResult<SyncResponse?>(new SyncResponse
            {
                Procesadas = request.Transacciones.Count,
                Fallidas = 0,
                Resultados = request.Transacciones.Select(t => new SyncResultadoDto
                {
                    IdLocal = t.IdLocal,
                    Exitosa = true
                }).ToList()
            });
    }
}

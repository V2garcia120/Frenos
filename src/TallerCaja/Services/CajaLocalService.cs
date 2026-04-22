using TallerCaja.Data;
using TallerCaja.Models.Entities;
using TallerCaja.Models.DTOs;
using TallerCaja.Helpers;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace TallerCaja.Services
{
    // ── Servicio de datos locales (SQLite) ────────────────────────────────────
    public interface ICajaLocalService
    {
        void SincronizarProductos(List<ProductoDto> productos);
        void SincronizarServicios(List<ServicioDto> servicios);
        List<ProductoLocal> ObtenerProductosLocales(string? filtro = null);
        List<ServicioLocal> ObtenerServiciosLocales(string? filtro = null);
        TurnoLocal? ObtenerTurnoActivo(int cajeroId);
        void GuardarTurnoLocal(TurnoLocal turno);
        void ActualizarTurnoLocal(TurnoLocal turno);
        void GuardarVentaLocal(VentaLocal venta);
        List<VentaLocal> ObtenerVentasDelTurno(int turnoId);
    }

    public class CajaLocalService : ICajaLocalService
    {
        private readonly CajaDbContext _db;

        public CajaLocalService(CajaDbContext db)
        {
            _db = db;
            _db.Database.EnsureCreated();
        }

        public void SincronizarProductos(List<ProductoDto> productos)
        {
            foreach (var p in productos)
            {
                var local = _db.Productos.Find(p.Id);
                if (local == null)
                {
                    _db.Productos.Add(new ProductoLocal
                    {
                        Id = p.Id, Nombre = p.Nombre, Precio = p.Precio,
                        Stock = p.Stock, Categoria = p.Categoria, UltimaSync = DateTime.Now
                    });
                }
                else
                {
                    local.Nombre = p.Nombre; local.Precio = p.Precio;
                    local.Stock = p.Stock; local.Categoria = p.Categoria;
                    local.UltimaSync = DateTime.Now;
                }
            }
            _db.SaveChanges();
        }

        public void SincronizarServicios(List<ServicioDto> servicios)
        {
            foreach (var s in servicios)
            {
                var local = _db.Servicios.Find(s.Id);
                if (local == null)
                {
                    _db.Servicios.Add(new ServicioLocal
                    {
                        Id = s.Id, Nombre = s.Nombre, Precio = s.Precio,
                        DuracionMin = s.DuracionMin, Categoria = s.Categoria, UltimaSync = DateTime.Now
                    });
                }
                else
                {
                    local.Nombre = s.Nombre; local.Precio = s.Precio;
                    local.DuracionMin = s.DuracionMin; local.Categoria = s.Categoria;
                    local.UltimaSync = DateTime.Now;
                }
            }
            _db.SaveChanges();
        }

        public List<ProductoLocal> ObtenerProductosLocales(string? filtro = null)
        {
            var q = _db.Productos.AsQueryable();
            if (!string.IsNullOrEmpty(filtro))
                q = q.Where(p => p.Nombre.ToLower().Contains(filtro.ToLower()));
            return q.ToList();
        }

        public List<ServicioLocal> ObtenerServiciosLocales(string? filtro = null)
        {
            var q = _db.Servicios.AsQueryable();
            if (!string.IsNullOrEmpty(filtro))
                q = q.Where(s => s.Nombre.ToLower().Contains(filtro.ToLower()));
            return q.ToList();
        }

        public TurnoLocal? ObtenerTurnoActivo(int cajeroId)
            => _db.Turnos.FirstOrDefault(t => t.CajeroId == cajeroId && t.Estado == "Abierto");

        public void GuardarTurnoLocal(TurnoLocal turno)
        {
            _db.Turnos.Add(turno);
            _db.SaveChanges();
        }

        public void ActualizarTurnoLocal(TurnoLocal turno)
        {
            _db.Turnos.Update(turno);
            _db.SaveChanges();
        }

        public void GuardarVentaLocal(VentaLocal venta)
        {
            _db.VentasLocales.Add(venta);
            _db.SaveChanges();
        }

        public List<VentaLocal> ObtenerVentasDelTurno(int turnoId)
            => _db.VentasLocales.Where(v => v.TurnoId == turnoId).ToList();
    }

    // ── Servicio de sincronización ────────────────────────────────────────────
    public interface ISyncService
    {
        Task SincronizarPendientesAsync();
    }

    public class SyncService : ISyncService
    {
        private readonly IIntegracionService _integracion;
        private readonly OfflineQueue _queue;

        public SyncService(IIntegracionService integracion, OfflineQueue queue)
        {
            _integracion = integracion;
            _queue = queue;
        }

        public async Task SincronizarPendientesAsync()
        {
            var pendientes = _queue.ObtenerPendientes();
            if (!pendientes.Any()) return;

            var request = new SyncRequest
            {
                Transacciones = pendientes.Select(p => new SyncItemDto
                {
                    IdLocal = p.IdLocal,
                    Tipo = p.Tipo,
                    Payload = p.Payload,
                    Fecha = p.FechaLocal
                }).ToList()
            };

            var resultado = await _integracion.SincronizarAsync(request);
            if (resultado == null) return;

            foreach (var r in resultado.Resultados.Where(r => r.Exitosa))
            {
                var item = pendientes.FirstOrDefault(p => p.IdLocal == r.IdLocal);
                if (item != null) _queue.MarcarProcesada(item.Id);
            }
        }
    }

    // ── Servicio de recibo/factura ────────────────────────────────────────────
    public interface IReciboService
    {
        string GenerarTextoRecibo(CobroResponse cobro, CobroRequest request, string clienteNombre);
    }

    public class ReciboService : IReciboService
    {
        public string GenerarTextoRecibo(CobroResponse cobro, CobroRequest request, string clienteNombre)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("═══════════════════════════════════════");
            sb.AppendLine($"       {AppConfig.NombreTaller}");
            sb.AppendLine("═══════════════════════════════════════");
            sb.AppendLine($" Factura: {cobro.NumeroFactura ?? cobro.IdLocal[..8].ToUpper()}");
            sb.AppendLine($" Fecha:   {DateTime.Now:dd/MM/yyyy HH:mm}");
            sb.AppendLine($" Cajero:  {SessionManager.CajeroNombre}");
            sb.AppendLine($" Cliente: {clienteNombre}");
            sb.AppendLine("───────────────────────────────────────");
            sb.AppendLine($" {"Descripción",-22} {"Cant",4} {"Precio",10}");
            sb.AppendLine("───────────────────────────────────────");

            foreach (var item in request.Items)
            {
                var nombre = item.NombreSnapshot.Length > 22
                    ? item.NombreSnapshot[..22]
                    : item.NombreSnapshot;
                var precio = (item.PrecioSnapshot * item.Cantidad).ToString("N2");
                sb.AppendLine($" {nombre,-22} {item.Cantidad,4} {precio,10}");
            }

            var (subtotal, itbis, total) = FacturaCalculator.Calcular(request.Items);
            sb.AppendLine("───────────────────────────────────────");
            sb.AppendLine($" {"Subtotal:",-22}      {subtotal,10:N2}");
            sb.AppendLine($" {"ITBIS (18%):",-22}      {itbis,10:N2}");
            sb.AppendLine($" {"TOTAL:",-22}      {total,10:N2}");
            sb.AppendLine("───────────────────────────────────────");
            sb.AppendLine($" Método de pago: {request.MetodoPago}");
            sb.AppendLine($" Monto pagado:   RD$ {request.MontoPagado:N2}");
            if (request.MetodoPago == "Efectivo")
                sb.AppendLine($" Cambio:         RD$ {cobro.Cambio:N2}");
            sb.AppendLine($" Estado:         {cobro.Estado}");
            sb.AppendLine("═══════════════════════════════════════");
            sb.AppendLine("    ¡Gracias por su preferencia!");
            sb.AppendLine("═══════════════════════════════════════");
            return sb.ToString();
        }
    }
}

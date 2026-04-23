using TallerCaja.Data;
using TallerCaja.Models.Entities;
using TallerCaja.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace TallerCaja.Services
{
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

}

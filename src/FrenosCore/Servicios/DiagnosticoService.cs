using FrenosCore.Data;
using FrenosCore.Modelos.Dtos.Diagnostico;
using FrenosCore.Modelos.Entidades;
using Microsoft.EntityFrameworkCore;

namespace FrenosCore.Servicios
{
    public class DiagnosticoService : IDiagnosticoService
    {
        private readonly AppDbContext _context;
        private readonly ICotizacionService _CotizcionServ;
        public DiagnosticoService(AppDbContext context, ICotizacionService service)
        {
            _context = context;
            _CotizcionServ = service;
        }

        public async Task<DiagnosticoResponse> CrearAsync(CrearDiagnosticoRequest req)
        {

            var orden = await _context.Orden
                .Include(o => o.Vehiculo)
                .Include(o => o.Cliente)
                .FirstOrDefaultAsync(o => o.Id == req.OrdenId)
                ?? throw new KeyNotFoundException(
                    $"Orden {req.OrdenId} no encontrada.");

            if (orden.Estado != "EnDiagnostico")
                throw new InvalidOperationException(
                    $"La orden debe estar en estado EnDiagnostico para crear un diagnóstico. " +
                    $"Estado actual: {orden.Estado}.");


            var diagnosticoExistente = await _context.Diagnostico
                .AnyAsync(d => d.OrdenId == req.OrdenId);

            if (diagnosticoExistente)
                throw new InvalidOperationException(
                    $"La orden {req.OrdenId} ya tiene un diagnóstico registrado.");

            var tecnico = await _context.Usuario
                .FirstOrDefaultAsync(u => u.Id == req.TecnicoId && u.Activo)
                ?? throw new KeyNotFoundException(
                    $"Técnico {req.TecnicoId} no encontrado o inactivo.");

            if (req.Items.Any())
                await ValidarItemsAsync(req.Items);

  
            var diagnostico = new Diagnostico
            {
                OrdenId = req.OrdenId,
                TecnicoId = req.TecnicoId,
                KmIngreso = req.KmIngreso,
                DescripcionGeneral = req.DescripcionGeneral.Trim(),
                Estado = "Borrador",
                RequiereAtencionUrgente = req.RequiereAtencionUrgente,
                ObservacionesTecnico = req.ObservacionesTecnico?.Trim(),
                FechaDiagnostico = DateTime.UtcNow,
                Items = req.Items.Select(i => new DiagnosticoItem
                {
                    SistemaVehiculo = i.SistemaVehiculo.Trim(),
                    Componente = i.Componente.Trim(),
                    Condicion = i.Condicion,
                    AccionRecomendada = i.AccionRecomendada,
                    Descripcion = i.Descripcion?.Trim(),
                    ServicioSugeridoId = i.ServicioSugeridoId,
                    ProductoSugeridoId = i.ProductoSugeridoId,
                    CantidadProductoSugerido = i.CantidadProductoSugerida,
                    EsUrgente = i.EsUrgente
                }).ToList()
            };


            if (req.KmIngreso.HasValue)
            {
                orden.Vehiculo.KmActual = req.KmIngreso.Value;
            }

            _context.Diagnostico.Add(diagnostico);
            await _context.SaveChangesAsync();


            await _context.Entry(diagnostico)
                .Collection(d => d.Items)
                .Query()
                .Include(i => i.ServicioSugerido)
                .Include(i => i.ProductoSugerido)
                .LoadAsync();

            return ToResponse(diagnostico, orden, tecnico);
        }
        public async Task<DiagnosticoItemResponse> AgregarItemAsync(
            int diagnosticoId, CrearDiagnosticoItemRequest req)
        {
            var diagnostico = await _context.Diagnostico.FindAsync(diagnosticoId)
                ?? throw new KeyNotFoundException(
                    $"Diagnóstico {diagnosticoId} no encontrado.");

            if (diagnostico.Estado != "Borrador")
                throw new InvalidOperationException(
                    "Solo se pueden agregar items a un diagnóstico en estado Borrador.");

   
            await ValidarItemsAsync([req]);

            var item = new DiagnosticoItem
            {
                DiagnosticoId = diagnosticoId,
                SistemaVehiculo = req.SistemaVehiculo.Trim(),
                Componente = req.Componente.Trim(),
                Condicion = req.Condicion,
                AccionRecomendada = req.AccionRecomendada,
                Descripcion = req.Descripcion?.Trim(),
                ServicioSugeridoId = req.ServicioSugeridoId,
                ProductoSugeridoId = req.ProductoSugeridoId,
                CantidadProductoSugerido = req.CantidadProductoSugerida,
                EsUrgente = req.EsUrgente
            };

            _context.DiagnosticoItem.Add(item);
            await _context.SaveChangesAsync();

            // Cargar navegación para la respuesta
            await _context.Entry(item).Reference(i => i.ServicioSugerido).LoadAsync();
            await _context.Entry(item).Reference(i => i.ProductoSugerido).LoadAsync();

            return ToItemResponse(item);
        }

        public async Task<DiagnosticoResponse> ListarPorOrdenAsync(int ordenId)
        {
            var ordenExiste = await _context.Orden.AnyAsync(o => o.Id == ordenId);
            if (!ordenExiste)
                throw new KeyNotFoundException($"Orden con ID {ordenId} no encontrada.");

            var diagnostico = await _context.Diagnostico
                .AsNoTracking()
                .Include(d => d.Tecnico)
                .Include(d => d.Orden)
                    .ThenInclude(o => o.Cliente)
                .Include(d => d.Orden)
                    .ThenInclude(o => o.Vehiculo)
                .Include(d => d.Items)
                    .ThenInclude(i => i.ServicioSugerido)
                .Include(d => d.Items)
                    .ThenInclude(i => i.ProductoSugerido)
                .Where(d => d.OrdenId == ordenId)
                .OrderByDescending(d => d.FechaDiagnostico)
                .FirstOrDefaultAsync()
                ?? throw new KeyNotFoundException($"No existe diagnóstico para la orden {ordenId}.");

            return ToResponse(diagnostico, diagnostico.Orden, diagnostico.Tecnico);
        }

        public async Task<DiagnosticoResponse> ObtenerPorIdAsync(int id)
        {
            var diagnostico = await _context.Diagnostico
                .AsNoTracking()
                .Include(d => d.Tecnico)
                .Include(d => d.Orden)
                    .ThenInclude(o => o.Cliente)
                .Include(d => d.Orden)
                    .ThenInclude(o => o.Vehiculo)
                .Include(d => d.Items)
                    .ThenInclude(i => i.ServicioSugerido)
                .Include(d => d.Items)
                    .ThenInclude(i => i.ProductoSugerido)
                .FirstOrDefaultAsync(d => d.Id == id)
                ?? throw new KeyNotFoundException($"Diagnóstico con ID {id} no encontrado.");

            return ToResponse(diagnostico, diagnostico.Orden, diagnostico.Tecnico);
        }

        public async Task<DiagnosticoResponse> ActualizarAsync(int id, ActualizarDiagnosticoRequest request)
        {
            var diagnostico = await _context.Diagnostico
                .Include(d => d.Tecnico)
                .Include(d => d.Orden)
                    .ThenInclude(o => o.Cliente)
                .Include(d => d.Orden)
                    .ThenInclude(o => o.Vehiculo)
                .Include(d => d.Items)
                    .ThenInclude(i => i.ServicioSugerido)
                .Include(d => d.Items)
                    .ThenInclude(i => i.ProductoSugerido)
                .FirstOrDefaultAsync(d => d.Id == id)
                ?? throw new KeyNotFoundException($"Diagnóstico con ID {id} no encontrado.");

            if (diagnostico.Estado != "Borrador")
                throw new InvalidOperationException("Solo se pueden editar Borradores");

            if (request.TecnicoId.HasValue)
            {
                var tecnico = await _context.Usuario
                    .FirstOrDefaultAsync(u => u.Id == request.TecnicoId.Value && u.Activo)
                    ?? throw new KeyNotFoundException($"Técnico con ID {request.TecnicoId.Value} no encontrado.");

                diagnostico.TecnicoId = request.TecnicoId.Value;
                diagnostico.Tecnico = tecnico;
            }

            if (request.KmIngerso.HasValue) diagnostico.KmIngreso = request.KmIngerso.Value;
            if (request.DescripcionGeneral is not null) diagnostico.DescripcionGeneral = request.DescripcionGeneral.Trim();
            if (request.Estado is not null) diagnostico.Estado = request.Estado.Trim();
            if (request.RequiereAtencionUrgente.HasValue) diagnostico.RequiereAtencionUrgente = request.RequiereAtencionUrgente.Value;
            if (request.ObservacionesTecnico is not null) diagnostico.ObservacionesTecnico = request.ObservacionesTecnico.Trim();

            if (request.Items is not null)
            {
                var nuevosItems = request.Items.ToList();
                if (nuevosItems.Any())
                    await ValidarItemsAsync(nuevosItems);

                _context.DiagnosticoItem.RemoveRange(diagnostico.Items);
                diagnostico.Items = nuevosItems.Select(i => new DiagnosticoItem
                {
                    DiagnosticoId = diagnostico.Id,
                    SistemaVehiculo = i.SistemaVehiculo.Trim(),
                    Componente = i.Componente.Trim(),
                    Condicion = i.Condicion,
                    AccionRecomendada = i.AccionRecomendada,
                    Descripcion = i.Descripcion?.Trim(),
                    ServicioSugeridoId = i.ServicioSugeridoId,
                    ProductoSugeridoId = i.ProductoSugeridoId,
                    CantidadProductoSugerido = i.CantidadProductoSugerida,
                    EsUrgente = i.EsUrgente
                }).ToList();
            }

            await _context.SaveChangesAsync();

            await _context.Entry(diagnostico)
                .Collection(d => d.Items)
                .Query()
                .Include(i => i.ServicioSugerido)
                .Include(i => i.ProductoSugerido)
                .LoadAsync();

            return ToResponse(diagnostico, diagnostico.Orden, diagnostico.Tecnico);
        }

        public async Task EliminarAsync(int id)
        {
            var diagnostico = await _context.Diagnostico
                .FirstOrDefaultAsync(d => d.Id == id)
                ?? throw new KeyNotFoundException($"Diagnóstico con ID {id} no encontrado.");

            _context.Diagnostico.Remove(diagnostico);
            await _context.SaveChangesAsync();
        }

        public async Task<DiagnosticoResponse> CompletarDiagnosticoAsync(int id)
        {
            var diagnostico = await _context.Diagnostico
                .Include(d => d.Tecnico)
                .Include(d => d.Items)
                    .ThenInclude(i => i.ServicioSugerido)
                .Include(d => d.Items)
                    .ThenInclude(i => i.ProductoSugerido)
                .Include(d => d.Orden)
                    .ThenInclude(o => o.Cliente)
                .Include(d => d.Orden)
                    .ThenInclude(o => o.Vehiculo)
                .FirstOrDefaultAsync(d => d.Id == id)
                ?? throw new KeyNotFoundException($"Diagnóstico con ID {id} no encontrado.");

            diagnostico.Estado = "Completado";
            await _context.SaveChangesAsync();

            if (!diagnostico.Orden.CotizacionId.HasValue)
            {
                var cotizacion = await _CotizcionServ.GenerarDesdeDiagnosticoAsync(diagnostico.Id);
                diagnostico.Orden.CotizacionId = cotizacion.Id;
                await _context.SaveChangesAsync();
            }

            return ToResponse(diagnostico, diagnostico.Orden, diagnostico.Tecnico);
        }
        public async Task AprobarAsync(int id)
        {
            var diagnostico = await _context.Diagnostico.FindAsync(id)
                ?? throw new KeyNotFoundException($"El diagnostico con id: {id} no fue encontrado");

            diagnostico.AprobadoPorCliente = true;
            diagnostico.Estado = "Completado";

            await _context.SaveChangesAsync();



        }
        private async Task ValidarItemsAsync(IEnumerable<CrearDiagnosticoItemRequest> items)
        {

            string[] condicionesValidas = ["Bueno", "Regular", "Malo", "Critico"];
            string[] accionesValidas = ["Revisar", "Reparar", "Reemplazar"];

            foreach (var item in items)
            {
                if (!condicionesValidas.Contains(item.Condicion))
                    throw new ArgumentException(
                        $"Condición '{item.Condicion}' no válida. " +
                        $"Valores permitidos: {string.Join(", ", condicionesValidas)}.");

                if (!accionesValidas.Contains(item.AccionRecomendada))
                    throw new ArgumentException(
                        $"Acción '{item.AccionRecomendada}' no válida. " +
                        $"Valores permitidos: {string.Join(", ", accionesValidas)}.");


                if (item.ServicioSugeridoId.HasValue)
                {
                    var servicioExiste = await _context.Servicio
                        .AnyAsync(s => s.Id == item.ServicioSugeridoId.Value && s.Activo);

                    if (!servicioExiste)
                        throw new KeyNotFoundException(
                            $"Servicio {item.ServicioSugeridoId} no encontrado o inactivo.");
                }


                if (item.ProductoSugeridoId.HasValue)
                {
                    var productoExiste = await _context.Producto
                        .AnyAsync(p => p.Id == item.ProductoSugeridoId.Value && p.Activo);

                    if (!productoExiste)
                        throw new KeyNotFoundException(
                            $"Producto {item.ProductoSugeridoId} no encontrado o inactivo.");
                }
            }
        }


        private static DiagnosticoResponse ToResponse(
            Diagnostico d, Orden? orden, Usuario? tecnico) => new(
                Id: d.Id,
                OrdenId: d.OrdenId,
                VehiculoInfo: orden?.Vehiculo is null
                    ? string.Empty
                    : $"{orden.Vehiculo.Marca} {orden.Vehiculo.Modelo} {orden.Vehiculo.Anno} · {orden.Vehiculo.Placa}",
                ClienteNombre: orden?.Cliente?.Nombre ?? string.Empty,
                TecnicoId: d.TecnicoId,
                TecnicoNombre: tecnico?.Nombre ?? string.Empty,
                KmIngreso: d.KmIngreso,
                DescripcionGeneral: d.DescripcionGeneral,
                Estado: d.Estado,
                RequiereAtencionUrgente: d.RequiereAtencionUrgente,
                AprobadoPorCliente: d.AprobadoPorCliente,
                FechaAprobacion: d.FechaAprobacion,
                ObservacionesTecnico: d.ObservacionesTecnico,
                FechaDiagnostico: d.FechaDiagnostico,
                Items: d.Items.Select(ToItemResponse));

        private static DiagnosticoItemResponse ToItemResponse(DiagnosticoItem i) => new(
            Id: i.Id,
            SistemaVehiculo: i.SistemaVehiculo,
            Componente: i.Componente,
            Condicion: i.Condicion,
            AccionRecomendada: i.AccionRecomendada,
            Descripcion: i.Descripcion,
            ServicioSugeridoId: i.ServicioSugeridoId,
            ServicioSugeridoNombre: i.ServicioSugerido?.Nombre,
            ProductoSugeridoId: i.ProductoSugeridoId,
            ProductoSugeridoNombre: i.ProductoSugerido?.Nombre,
            CantidadProductoSugerido: i.CantidadProductoSugerido,
            EsUrgente: i.EsUrgente);

    }
}

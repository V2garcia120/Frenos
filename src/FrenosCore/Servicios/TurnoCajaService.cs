using FrenosCore.Data;
using FrenosCore.Modelos.Entidades;
using FrenosCore.Modelos.Dtos;
using FrenosCore.Modelos.Dtos.TurnoCaja;
using Microsoft.EntityFrameworkCore;

namespace FrenosCore.Servicios
{
    public class TurnoCajaService : ITurnoCajaService
    {
        private readonly AppDbContext _context;

        public TurnoCajaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AbrirTurnoResponse> AbrirTurnoAsync(AbrirTurnoRequest request)
        {
            if (request.MontoInicial < 0)
                throw new ArgumentException("El monto inicial no puede ser negativo.");
            if (await _context.TurnoCaja.AnyAsync(t => t.CajeroId == request.CajeroId && t.Estado == "Abierto"))
                throw new InvalidOperationException("El cajero ya tiene un turno abierto.");
            if (!await _context.Usuario.AnyAsync(c => c.Id == request.CajeroId))
                throw new ArgumentException("Cajero no encontrado.");

            var turno = new TurnoCaja
            {
                CajeroId = request.CajeroId,
                FechaApertura = DateTime.Now,
                IdLocalCaja = request.TurnoLocalCaja.ToString(),
                MontoInicial = request.MontoInicial,
                Estado = "Abierto"
            };
            _context.TurnoCaja.Add(turno);
            await _context.SaveChangesAsync();
            return new AbrirTurnoResponse(
                TurnoId: turno.Id,
                Estado: turno.Estado,
                Fechaapertura: turno.FechaApertura
            );
        }

        public async Task<CerrarTurnoResponse> CerrarTurnoAsync(CerrarTurnoRequest request)
        {
            var turno = await _context.TurnoCaja.FindAsync(request.TurnoId);
            if (turno == null || turno.Estado != "Abierto")
                throw new InvalidOperationException("Turno no encontrado o ya cerrado.");
            turno.FechaCierre = DateTime.Now;
            turno.Estado = "Cerrado";
            turno.EfectivoContado = request.EfectivoContado;
            await _context.SaveChangesAsync();
            return new CerrarTurnoResponse(
                TurnoId: turno.Id,
                Estado: turno.Estado,
                FechaApertura: turno.FechaApertura,
                FechaCierre: turno.FechaCierre ?? DateTime.Now,
                MontoInicial: turno.MontoInicial,
                EfectivoContado: request.EfectivoContado,
                Observaciones: request.Observaciones
            );
        }
    }
}

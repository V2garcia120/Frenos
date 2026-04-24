using FrenosCore.Data;
using FrenosCore.Modelos.Entidades;
using FrenosCore.Modelos.Dtos;
using FrenosCore.Modelos.Dtos.TurnoCaja;

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
            var turno = new TurnoCaja
            {
                CajeroId = request.CajeroId,
                FechaApertura = DateTime.Now,
                Estado = "Abierto"
            };
            _context.TurnoCaja.Add(turno);
            await _context.SaveChangesAsync();
            return new AbrirTurnoResponse(
                TurnoId: turno.CajeroId,
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
            await _context.SaveChangesAsync();
            return new CerrarTurnoResponse(
                TurnoId: turno.CajeroId,
                Estado: turno.Estado,
                FechaApertura: turno.FechaApertura,
                FechaCierre: DateTime.Now,
                MontoInicial: turno.MontoInicial,
                MontoFinal: request.MontoFinal,
                Observaciones: request.Observaciones
            );
        }
    }
}

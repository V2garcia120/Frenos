namespace FrenosCore.Modelos.Dtos.Vehiculo
{
    public record VehiculoResponse(
        int Id,
        int ClienteId,
        string Placa,
        string Marca,
        string Modelo,
        int Anno,
        string Color,
        string VIN,
        string TipoCombustible,
        int KmActual,
        string Nota,
        DateTime FechaCreacion
    );
}

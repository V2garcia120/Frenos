namespace FrenosCore.Modelos.Dtos.Vehiculo
{
    public record ActualizarVehiculoRequest(
        string? Placa,
        string? Marca,
        string? Modelo,
        int? Anno,
        string? Color,
        string? VIN,
        string? TipoCombustible,
        int? KmActual,
        string? Nota
    );
}

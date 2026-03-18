namespace FrenosCore.Modelos.Dtos.Cliente
{
    public record ClienteDetalleResponse(
        int Id,
        string Nombre,
        string? Cedula,
        string? Telefono,
        string? Email,
        string? Direccion,
        bool EsAnonimo,
        DateTime CreadoEn,
        IEnumerable<VehiculoResumen> Vehiculos
    );

    public record VehiculoResumen(
        int Id,
        string Placa,
        string Marca,
        string Modelo,
        int Anno
    );
}

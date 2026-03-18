namespace FrenosCore.Modelos.Dtos.Cliente
{
    public record ClienteResponse(
        int Id,
        string Nombre,
        string? Cedula,
        string? Telefono,
        string? Email,
        string? Direccion,
        bool EsAnonimo,
        int TotalVehiculos,
        int TotalOrdenes,
        DateTime CreadoEn
    );
}

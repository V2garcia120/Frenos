namespace FrenosCore.Modelos.Dtos
{
    public record PaginadoResponse<T>(
        IEnumerable<T> Items,
        int PaginaActual,
        int TamPagina,
        int TotalItems,
        int TotalPaginas
    );
}

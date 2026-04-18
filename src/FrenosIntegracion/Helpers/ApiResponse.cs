namespace FrenosIntegracion.Helpers
{
    public class ApiResponse<T>
    {
        public bool Success { get; init; }
        public T? Data { get; init; }
        public ApiError? Error { get; init; }

        public static ApiResponse<T> Ok(T data) =>
            new() { Success = true, Data = data, Error = null };

        public static ApiResponse<T> Fail(string codigo, string mensaje) =>
            new() { Success = false, Data = default, Error = new(codigo, mensaje) };
    }

    public record ApiError(string Codigo, string Mensaje);

    public class ApiResponse : ApiResponse<object>
    {
        public static ApiResponse<object> Ok() =>
            new() { Success = true, Data = null, Error = null };
    }
}

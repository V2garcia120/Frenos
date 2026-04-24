namespace FrenosWeb.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public ApiError? Error { get; set; }

        public static ApiResponse<T> Ok(T data)
            => new() { Success = true, Data = data, Error = null };

        public static ApiResponse<T> Fail(string codigo, string mensaje)
            => new() { Success = false, Error = new ApiError { Codigo = codigo, Mensaje = mensaje } };
    }

    public class ApiError
    {
        public string Codigo { get; set; } = "";
        public string Mensaje { get; set; } = "";
    }
}
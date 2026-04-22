using FrenosWeb.Services;

namespace FrenosWeb.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? ErrorCode { get; set; }
        public T? Data { get; set; }

        public static ApiResponse<T> Ok(T data, string? message = null)
            => new() { Success = true, Data = data, Message = message };

        public static ApiResponse<T> Fail(string errorCode, string message)
            => new() { Success = false, ErrorCode = errorCode, Message = message };
    }
}

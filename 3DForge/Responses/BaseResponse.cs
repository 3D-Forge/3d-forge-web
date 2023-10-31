using System.Text.Json.Serialization;

namespace Backend3DForge.Responses
{
    public abstract class BaseResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("message")]
        public string? Message { get; set; } = null;
        [JsonPropertyName("data")]
        public object? Data { get; set; } = null;

        public BaseResponse(bool success, string? message, object? data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public class SuccessResponse : BaseResponse
        {
            public SuccessResponse(string? message, object? data = null) : base(true, message, data)
            {
            }
        }

        public class ErrorResponse : BaseResponse
        {
            public ErrorResponse(string? message, object? data = null) : base(false, message, data)
            {
            }
        }
    }
}

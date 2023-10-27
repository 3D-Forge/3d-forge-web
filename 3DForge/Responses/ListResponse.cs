using System.Text.Json.Serialization;

namespace Backend3DForge.Responses
{
    public class ListResponse<T> : BaseResponse
    {
        [JsonPropertyName("data")]
        public List<T> Data { get; set; } = new List<T>();
        public ListResponse(bool success, string? message, List<T> data) : base(success, message, data)
        {
            Data = data;
        }
    }
}

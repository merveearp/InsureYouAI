using System.Text.Json.Serialization;

namespace InsureYouAI.DTOs.GeminiDtos
{
    public class GeminiAboutItemDto
    {
        [JsonPropertyName("detail")]
        public string Detail { get; set; }
    }
}

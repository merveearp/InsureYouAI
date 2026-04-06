using System.Text.Json.Serialization;

namespace InsureYouAI.DTOs.ClaudeDtos
{
    public class ClaudeServiceDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("icon")]
        public string IconUrl { get; set; }
    }
}

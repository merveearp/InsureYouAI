using System.Text.Json.Serialization;

namespace InsureYouAI.DTOs.GeminiDtos
{
    public class GeminiAboutDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

    }
}

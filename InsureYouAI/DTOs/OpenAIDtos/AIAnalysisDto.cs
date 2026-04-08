using System.Text.Json.Serialization;

namespace InsureYouAI.DTOs.OpenAIDtos
{
    public class AIAnalysisDto
    {
        [JsonPropertyName("damageType")]
        public string DamageType { get; set; }

        [JsonPropertyName("severity")]
        public string Severity { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("insuranceStatus")]
        public string InsuranceStatus { get; set; }

        [JsonPropertyName("recommendation")]
        public string Recommendation { get; set; }

        [JsonPropertyName("parts")]
        public string Parts { get; set; }

        [JsonPropertyName("damageRate")]
        public string DamageRate { get; set; } 
    }
}

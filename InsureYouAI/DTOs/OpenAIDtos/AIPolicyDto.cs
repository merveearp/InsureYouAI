using System.Text.Json.Serialization;

namespace InsureYouAI.DTOs.OpenAIDtos
{
    public class AIPolicyDto
    {
        [JsonPropertyName("policyType")]
        public string PolicyType { get; set; }

        [JsonPropertyName("coverage")]
        public string Coverage { get; set; }

        [JsonPropertyName("notCovered")]
        public string NotCovered { get; set; }

        [JsonPropertyName("keyWarnings")]
        public string KeyWarnings { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonPropertyName("isRisky")]
        public string IsRisky { get; set; }
    }
}

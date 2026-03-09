using InsureYouAI.DTOs.GeminiDtos;
using System.Text;
using System.Text.Json;

namespace InsureYouAI.Services.GeminiServices
{
    public class GeminiService : IGeminiService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public GeminiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<GeminiAboutDto> CreateAboutText()
        {
            var apiKey = _configuration["Gemini:GeminiKey"];

            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={apiKey}";

            var prompt = @"
Bir sigorta danışmanlık şirketi için profesyonel bir 'Hakkımızda' içeriği oluştur.

Kurallar:
- Kurumsal ve güven veren bir dil kullan
- Açıklama minimum 300 kelime olsun
- Başlık minimum 3 kelime olsun

SADECE aşağıdaki JSON formatında cevap ver.

{
  ""title"": ""string"",
  ""description"": ""string""
}";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Gemini API Hatası: {error}");
            }

            var responseString = await response.Content.ReadAsStringAsync();

            var jsonResponse = JsonDocument.Parse(responseString);

            var aiText = jsonResponse
                .RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            aiText = aiText
                .Replace("```json", "")
                .Replace("```", "")
                .Trim();

            if (string.IsNullOrWhiteSpace(aiText))
            {
                throw new Exception("Gemini boş cevap döndürdü.");
            }

            var result = JsonSerializer.Deserialize<GeminiAboutDto>(aiText);

            return result;
        }
    }
}
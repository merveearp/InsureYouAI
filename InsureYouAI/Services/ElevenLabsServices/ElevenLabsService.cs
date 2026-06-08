using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace InsureYouAI.Services.ElevenLabsServices
{
    public class ElevenLabsService : IElevenLabsService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ElevenLabsService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> GenerateSpeechAsync(string text)
        {
            var apiKey = _configuration["ElevenLabs:ApiKey"];
            var voiceId = _configuration["ElevenLabs:VoiceId"];

            var url = $"https://api.elevenlabs.io/v1/text-to-speech/{voiceId}";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("xi-api-key", apiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("audio/mpeg"));

            var requestBody = new
            {
                text = text,
                model_id = "eleven_multilingual_v2",
                voice_settings = new
                {
                    stability = 0.55,
                    similarity_boost = 0.75
                }
            };

            var json = JsonSerializer.Serialize(requestBody);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("ElevenLabs Hatası: " + response.StatusCode);
            }

            var audioBytes = await response.Content.ReadAsByteArrayAsync();

            var fileName = $"{Guid.NewGuid()}.mp3";

            var folderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "audio");

            Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);

            await File.WriteAllBytesAsync(filePath, audioBytes);

            return "/audio/" + fileName;
        }
    }
}
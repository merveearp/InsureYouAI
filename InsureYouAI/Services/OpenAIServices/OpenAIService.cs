using InsureYouAI.DTOs;
using InsureYouAI.DTOs.OpenAIDtos;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace InsureYouAI.Services.OpenAIServices
{
    public class OpenAIService : IOpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string url;

        public OpenAIService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;

            _apiKey = configuration["OpenAI:ApiKey"];

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);
            url = "https://api.openai.com/v1/chat/completions";
       }


        public async Task<string> CreateArticleWithAI(string prompt)
        {

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                 {
                    new
                    {
                        role = "system",
                        content = @"Sen profesyonel bir sigorta blog yazarı ve SEO uzmanısın.

                        Kurallar:
                        - SEO uyumlu başlık üret
                        - giriş paragrafı yaz
                        - alt başlıklar kullan
                        - madde işaretleri kullan
                        - sonuç paragrafı yaz
                        - en az 5000 karakter olsun
                        - Türkçe yaz
                        "
                    },
                    new
                    {
                        role = "user",
                        content = prompt
                    }
                },
                    temperature = 0.7
            };
            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("OpenAI API hatası: " + response.StatusCode);
            }

            //var jsonString = await response.Content.ReadAsStringAsync();
            //var result = JsonSerializer.Deserialize<OpenAIResponse>(jsonString);               // ----> readfromjsonasync

            var result = await response.Content.ReadFromJsonAsync<OpenAIResponse>();

            return result.choices[0].message.content;


        }

        public async Task<AIMessageDto> GenerateInsuranceConsultationAsync(string userMessage)
        {
            var requestBody = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
            new
            {
                role = "system",
                content = @"Sen profesyonel bir sigorta danışmanısın.
                Kullanıcıyla sohbet eder gibi konuş.
                Sorular sorarak ihtiyacını anlamaya çalış.
                Kısa ve doğal cevap ver."

            },
            new
            {
                role = "user",
                content = userMessage
            }
        },
                temperature = 0.7
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await _httpClient.PostAsync(url, content);

            if (!httpResponse.IsSuccessStatusCode)
                throw new Exception("OpenAI HATASI : " + httpResponse.StatusCode);

            var jsonString = await httpResponse.Content.ReadAsStringAsync();

            var jsonDoc = JsonDocument.Parse(jsonString);

            var aiText = jsonDoc
                .RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return new AIMessageDto
            {
                Message = aiText
            };
        }
    }
}

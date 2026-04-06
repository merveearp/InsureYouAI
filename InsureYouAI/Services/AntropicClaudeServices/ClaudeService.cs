using InsureYouAI.DTOs.ClaudeDtos;
using System.Text;
using System.Text.Json;

namespace InsureYouAI.Services.AntropicClaudeServices
{
    public class ClaudeService : IClaudeService
    {

        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public ClaudeService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<ClaudeServiceDto> CreateServiceText()
        {
            var apiKey = _configuration["AntropicClaude:ClaudeKey"];
            var url = "https://api.anthropic.com/v1/messages";

            string prompt = @"

                Sen üst düzey bir sigorta içerik stratejisti, UX metin yazarı ve marka dili uzmanısın.

                Bir sigorta şirketinin web sitesinde kullanılacak ""Hizmetler"" bölümü için TEK bir hizmet üret.

                Amaç:
                - Kullanıcıyı güven veren, profesyonel ve modern bir dil ile etkilemek
                - Her üretimde farklı ve yaratıcı içerik sunmak

                Kurallar:
                - Her istekte tamamen BENZERSİZ içerik üret
                - Daha önce kullanılan kelime ve kalıpları tekrar etme
                - İçerik Türkçe olacak
                - Kurumsal, güven veren ve modern bir dil kullan
                - Sigorta, güvenlik, analiz, hız, teknoloji ve müşteri odaklılık vurgulansın
                - Yapay zeka, veri analizi, dijital dönüşüm gibi modern kavramları ara sıra kullan
                - Klişe cümlelerden kaçın (örnek: ""en iyi hizmet"" gibi ifadeler kullanma)

                Title için:
                - Maksimum 5 kelime
                - Güçlü, dikkat çekici ve profesyonel olsun
                - Aynı kelimelerle tekrar üretme

                Description için:
                - Maksimum 120 karakter
                - Tek cümle olacak
                - Açıklayıcı, akıcı ve doğal olsun
                - Aynı cümle yapısını tekrar etme

                Icon için:
                - SADECE Bootstrap Icons class üret
                - Format: ""bi bi-...""
                - Her seferinde FARKLI ikon seç
                - Aynı ikonları tekrar etmemeye çalış

                Kullanabileceğin ikon kategorileri:
                - Güvenlik: bi-shield-check, bi-shield-lock, bi-lock, bi-patch-check
                - Analiz: bi-graph-up, bi-bar-chart, bi-pie-chart, bi-activity
                - Hız: bi-lightning, bi-lightning-charge, bi-speedometer
                - Kullanıcı: bi-person-check, bi-people, bi-person-badge
                - Belge/Poliçe: bi-file-earmark-text, bi-file-earmark-check, bi-folder-check
                - Teknoloji: bi-cpu, bi-gear, bi-diagram-3, bi-database
                - Destek: bi-headset, bi-chat-dots, bi-telephone
                - Genel: bi-star, bi-award, bi-check-circle

                Çıktı formatı:
                SADECE GEÇERLİ JSON döndür.
                Açıklama yazma.
                Markdown kullanma.

                {
                  ""title"": ""string"",
                  ""description"": ""string"",
                  ""icon"": ""bi bi-...""
                }

                ";
            var requestBody = new
            {
                model = "claude-3-haiku-20240307",
                max_tokens = 300,
                messages = new[]
                {
                    new
                    {
                         role="user",
                         content= prompt
                    }


                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Headers.Add("x-api-key", apiKey);
            request.Headers.Add("anthropic-version", "2023-06-01");

            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(responseString);

            var jsonResponse = JsonDocument.Parse(responseString);

            var text = jsonResponse
                 .RootElement
                 .GetProperty("content")[0]
                 .GetProperty("text")
                 .GetString();

                    if (string.IsNullOrEmpty(text))
                        throw new Exception("Claude response boş geldi");

                    text = text.Replace("```json", "")
                               .Replace("```", "")
                               .Trim();


            if (text.StartsWith("\""))
            {
                text = JsonSerializer.Deserialize<string>(text);
            }

            Console.WriteLine(text);

            var result = JsonSerializer.Deserialize<ClaudeServiceDto>(text, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (result == null)
                throw new Exception("Parse başarısız: " + text);

            return result;

        }
    }
}

using InsureYouAI.DTOs.GeminiDtos;
using System.Text;
using System.Text.Json;
using System.Linq;

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

        public async Task<GeminiAboutItemDto> CreateAboutItem()
        {
            var apiKey = _configuration["Gemini:GeminiKey"];

            var model = "gemini-2.5-pro";

            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

            var prompt = @"Sen profesyonel bir sigorta sektörü içerik yazarı ve UX metin uzmanısın.

                Bir sigorta danışmanlık platformunun 'neden bizi tercih etmelisiniz' bölümünde kullanılacak KISA MADDE üret.

                Kurallar:
                - Maksimum 6 kelime
                - Güven veren ve profesyonel olsun
                - Sigorta danışmanlığı vurgulansın
                - Madde formatına uygun olsun
                - SEO uyumlu kelimeler içersin
                - HER SEFERİNDE farklı bir ifade üret
                - Daha önce kullanılan ifadeleri tekrar etme
                - Yaratıcı ve çeşitli kelimeler kullan

                Örnek tarz:
                Uzman Sigorta Danışmanlığı
                Hızlı Poliçe Analizi
                Güvenilir Sigorta Çözümleri
                7/24 Sigorta Desteği
                Kişiye Özel Poliçe Planı

                SADECE JSON döndür.
                Markdown kullanma.

                {
                ""detail"": ""string""
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
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(responseString);
                throw new Exception(responseString);
            }

            var jsonResponse = JsonDocument.Parse(responseString);

            var aiText = jsonResponse
                .RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            aiText = aiText.Replace("```json", "").Replace("```", "").Trim();

            var result = JsonSerializer.Deserialize<GeminiAboutItemDto>(aiText);
            return result;
        }

        public async Task<GeminiAboutDto> CreateAboutText()
        {
            var apiKey = _configuration["Gemini:GeminiKey"];

            var model = "gemini-2.5-pro";

            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

            var prompt = @"Sen profesyonel bir sigorta sektörü içerik yazarı ve SEO uzmanısın.

                Bir sigorta danışmanlık platformu için kurumsal içerik üret.

                Kurallar:
                - Başlık SADECE başlık olsun
                - Başlıkta 'Hakkımızda' kelimesi geçmesin
                - Başlık maksimum 6 kelime olsun
                - Açıklama 120-150 kelime arasında olsun
                - Kurumsal ve güven veren dil kullan
                - Sigorta danışmanlığı, müşteri güveni ve uzmanlık vurgulansın
                - SEO uyumlu yaz

                SADECE JSON döndür.
                Markdown kullanma.

                {
                ""title"":""string"",
                ""description"":""string""
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
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(responseString);
                throw new Exception(responseString);
            }

            var jsonResponse = JsonDocument.Parse(responseString);

            var aiText = jsonResponse
                .RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            aiText = aiText.Replace("```json", "").Replace("```", "").Trim();

            var result = JsonSerializer.Deserialize<GeminiAboutDto>(aiText);
            return result;
        }
    }
}
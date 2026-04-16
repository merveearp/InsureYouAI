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

            var model = "gemini-2.5-flash";

            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

            var prompt = @"Sen alanında uzman, kurumsal dil hakimiyetine sahip bir sigorta sektörü içerik stratejisti ve UX metin yazarısın.

            Bir sigorta danışmanlık platformunun 'Neden Bizi Tercih Etmelisiniz' bölümünde kullanılmak üzere, KURUMSAL DÜZEYDE, GÜVEN VEREN ve PROFESYONEL kısa maddeler üret.

            Kurallar:
            - Her çıktı EN AZ 6 kelimeden oluşmalıdır
            - Kurumsal, güçlü ve güven veren bir dil kullanılmalıdır
            - Sigorta danışmanlığı, risk yönetimi ve güvence vurgusu mutlaka yer almalıdır
            - Madde formatına uygun, tek satırlık ifade olmalıdır
            - SEO uyumlu kelimeler (sigorta, güvence, danışmanlık, risk, poliçe vb.) içermelidir
            - Her istekte TAMAMEN farklı ve özgün bir ifade üretmelisin
            - Daha önce üretilmiş ifadeleri kesinlikle tekrar etme
            - Yaratıcı, profesyonel ve marka değerini yükselten ifadeler kullan
            - Genel ve sıradan cümlelerden kaçın, premium kurumsal dil kullan

            İfade tarzı örnekleri:
            - Kurumsal Sigorta Danışmanlığı ile Güçlü Güvence
            - Kapsamlı Risk Analizi ile Doğru Poliçe Seçimi
            - Sürdürülebilir Güvence için Profesyonel Sigorta Çözümleri

            SADECE JSON formatında çıktı üret.
            Markdown veya açıklama yazma.

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

            var model = "gemini-2.5-flash";

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
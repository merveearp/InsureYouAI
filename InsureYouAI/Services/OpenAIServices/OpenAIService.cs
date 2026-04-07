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

        public async Task<AIAnalysisDto> AnalyzeDamageAsync(IFormFile image)
        {
           if(image == null || image.Length==0)
            {
                throw new Exception("Lütfen geçerli resim ekleyiniz");
            }

            using var ms = new MemoryStream();
            await image.CopyToAsync(ms);

            var imageBytes = ms.ToArray();

            var base64Image = Convert.ToBase64String(imageBytes);
            var requestBody = new
            {
                model = "gpt-4o-mini",
                temperature = 0.3,
                messages = new object[]
                {
                        new
                        {
                            role = "system",
                            content = @"Sen profesyonel bir sigorta eksperisin.
                Araç hasarlarını analiz ederken teknik, objektif ve kurumsal bir dil kullanırsın.
                Cevapların kısa ama açıklayıcı ve güven verici olmalıdır."
                        },
                        new
                        {
            role = "user",
            content = new object[]
            {
                new {
                    type = "text",
                    text = @"Bu bir araç hasar fotoğrafıdır.

                Detaylı bir eksper incelemesi yap.

                Kurallar:
                - Teknik ve kurumsal dil kullan
                - Hasarı analiz et
                - Kısa ama profesyonel rapor yaz
                - description alanı 3-5 cümlelik mini eksper raporu olsun

                Cevabı SADECE aşağıdaki JSON formatında ver:

                {
                  ""damageType"": ""(ör: Çizik, Göçük, Kırık)"",
                  ""severity"": ""(Düşük / Orta / Yüksek)"",
                  ""description"": ""(Profesyonel eksper raporu yaz)"",
                  ""insuranceStatus"": ""(Karşılanır / Belirsiz / Kapsam dışı)"",
                  ""recommendation"": ""(Kullanıcıya öneri)""
                }"
                                },
                                new {
                                    type = "image_url",
                                    image_url = new {
                                        url = $"data:image/jpeg;base64,{base64Image}"
                                    }
                                }
                            }
                        }
                    }
                            };
            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
                throw new Exception("AI HATASI: " + response.StatusCode);

            var jsonString = await response.Content.ReadAsStringAsync();

            var jsonDoc = JsonDocument.Parse(jsonString);

            var aiText = jsonDoc
                 .RootElement
                 .GetProperty("choices")[0]
                 .GetProperty("message")
                 .GetProperty("content")
                 .GetString();

            aiText = aiText.Replace("```json", "")
                           .Replace("```", "")
                           .Trim();

            var result = JsonSerializer.Deserialize<AIAnalysisDto>(aiText);

            return result;
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

            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<OpenAIResponse>(jsonString);             

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

            var result = JsonSerializer.Deserialize<AIMessageDto>(aiText);
            return result;
        }
    }
}

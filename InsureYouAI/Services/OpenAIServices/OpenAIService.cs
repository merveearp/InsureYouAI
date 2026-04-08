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
                temperature = 0.2,
                messages = new object[]
                        {
                            new
                            {
                                role = "system",
                                content = @"Sen uzman bir oto sigorta eksperisin.
                    Araç hasarlarını teknik olarak analiz eder, parça bazlı değerlendirme yapar ve yaklaşık maliyet çıkarırsın.

                    Kullandığın dil:
                    - Kurumsal
                    - Net
                    - Güven veren
                    - Teknik ama anlaşılır

                    Her zaman gerçekçi tahminler yap."
                            },
                            new
                            {
                                role = "user",
                                content = new object[]
                                {
                                    new {
                                        type = "text",
                                        text = @"
                    Bu bir araç hasar fotoğrafıdır.

                    Detaylı eksper analizi yap.

                    İNCELEME KRİTERLERİ:
                    - Hasar türü (göçük, çizik, kırık vb.)
                    - Hasarın bulunduğu parça (tampon, far, kaput vb.)
                    - Hasarın yaygınlığı (% olarak tahmin et)
                    - Araç güvenliğine etkisi
                    - Onarım mı değişim mi gerekir

                    EKSTRA:
                    - Tahmini maliyet aralığı ver (TL)
                    - Parça bazlı kısa yorum ekle

                    Cevabı SADECE aşağıdaki JSON formatında ver:

                    {
                      ""damageType"": ""(Hasar türünü detaylı yaz: örn. ön tamponda kırık, kaputta ciddi göçük, far hasarı gibi)"",
                      ""severity"": ""(Düşük / Orta / Yüksek)"",
                      ""damageRate"": ""% olarak hasar oranı"",
                      ""parts"": ""(Hasarlı parçaları virgülle yaz: tampon, kaput, far vb.)"",
                      ""description"": ""(Minimum 5-6 cümle olacak şekilde detaylı eksper raporu yaz. Hasarın konumu, yaygınlığı, aracın güvenliğine etkisi, sürüş riskleri, onarım süreci ve teknik değerlendirme mutlaka yer alsın.)"",
                      ""insuranceStatus"": ""(Karşılanır / Belirsiz / Kapsam dışı)"",
                      ""recommendation"": ""(Aksiyon önerisi)""
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

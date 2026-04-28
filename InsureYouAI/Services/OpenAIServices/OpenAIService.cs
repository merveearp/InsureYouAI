using Azure;
using InsureYouAI.DTOs;
using InsureYouAI.DTOs.OpenAIDtos;
using InsureYouAI.Entities;
using Microsoft.Build.Tasks;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using UglyToad.PdfPig;
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

        public async Task<string> AnalyzeCommentUserAsync(string comments)
        {
            var prompt = $@"
            Sen kullanıcı davranış analizi yapan bir yapay zeka uzmanısın.
            Analiz Başlıklıkları 
            1-Genel Duygu Durumu (pozitif/negatif/nötr)
            2- Toksik içerik var mı ?(örnekleriyle)
            3-İlgi alanları / konu başlıklarıyla
            4-İletişim tarzı(samimi ,resmi ,agresif vb)
            5-Geliştirilmesi gereken iletişim alanları
            6-5 Maddelik kısa özet

            
            Yorumlar :
            {comments}
                
               Lütfen çıktıyı profesyonel rapor formatında , madde madde ve en sonda 5 maddelik aksiyon listesi ile ver. ";

            var body = new
            {
                model = "gpt-4o-mini",
                messages = new object[]
                {
                    new
                    {
                        role="system",
                        content="Sen kullanıcı yorum analizi yapan bir uzmansın."
                    },
                    new
                    {
                        role="user",
                        content=prompt
                    }
                },
                max_tokens = 1000,
                temperature = 0.2

            };
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponse = await _httpClient.PostAsync(url, content);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("OpenAI API hatası: " + httpResponse.StatusCode);
            }

            var responseText = await httpResponse.Content.ReadAsStringAsync();


            var jsonString = JsonDocument.Parse(responseText);
            var AIText = jsonString.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return AIText;
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

        public async Task<AIPolicyDto> AnalyzePolicyAsync(IFormFile file)
        {
           if(file ==null || file.Length==0)
            {
                throw new Exception("İçerik tanımlanamıyor.Lütfen geçerli bir PDF yükleyiniz!");
            }

            string policyText = "";

            using (var stream = file.OpenReadStream())
            using (var pdf = PdfDocument.Open(stream))
            {
                foreach (var page in pdf.GetPages())
                {
                    policyText += page.Text + "\n";
                }
            }

            if (policyText.Length > 12000)
                policyText = policyText.Substring(0, 12000);

            var requestBody = new
            {
                model = "gpt-4o-mini",
                temperature = 0.2,
                messages = new object[]
            {
                            new
                            {
                                role = "system",
                                content = @"Sen profesyonel bir sigorta poliçe tarama asistanısın.

            Karmaşık poliçe metinlerini sadeleştirir ve kullanıcıya anlaşılır şekilde sunarsın.

            - Hukuki dili basitleştir
            - Gereksiz detay verme
            - Önemli riskleri özellikle belirt
            - Anlaşıllır kurumsal dilde cevap ver
            -Neden sonuç ilişkisi kur soru yanıtı verirken 
            "
                            },
                            new
                            {
                                role = "user",
                                content = $@"
            Aşağıdaki sigorta poliçesini analiz et:

            {policyText}

            Cevabı SADECE JSON formatında ver:

            {{
              ""policyType"": """",
              ""coverage"": """",
              ""notCovered"": """",
              ""keyWarnings"": """",
              ""summary"": ""Bütün poliçeyi tara genel bir özet çıkar yaklaşık 10 - 12 cümle aralığında olsun ."",
              ""isRisky"": ""Riskli mi neden riskli sorulara yanıt verirken neden ilişkisini kur.""
            }}"
                            }
            }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
                throw new Exception("AI HATASI: " + response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(responseString);

            var aiText = jsonDoc
                .RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();


            aiText = aiText.Replace("```json", "")
               .Replace("```", "")
                       .Trim();

            var result = JsonSerializer.Deserialize<AIPolicyDto>(aiText);
            return result;

        }

        public async Task<string> AnalyzeUserAsync(string articles)
        {
            var prompt = $@"Siz bir sigorta sektöründe uzman içerik analistisin.
                Elinizde bir sigorta şirketinin çalışanının yazdığı tüm makaleler var .Bu makaleler üzerinden çalışanın içerik üretim tarzını analiz et.
                Analiz Başlıklıkları 
                1-Konu çeşitliliği ve odak alanları(sağlık,hayat,kasko ,tamamlayıcı ,BES vb.)
                2- Hedef kitle tahmini (bireysel/kurumsal segment persona)
                3- Dil ve Anlatım Tarzı(tekniklik seviyesi , okunabilirlik ,ikna gücü)
                4-Sigorta terimlerini kullanma ve doğruluk düzeyi 
                5-Müşteri ihtiyaçlarını ve risk yönetimine odaklanma 
                6-Pazarlama / satış vurgusu CTA netliği
                7- Geliştirilmesi gereken alanlar ve net aksiyon maddeleri 
            
            Makaleler :
            {articles}

                
               Lütfen çıktıyı profesyonel rapor formatında , madde madde ve en sonda 5 maddelik aksiyon listesi ile ver. ";

            var body = new
            {
                model = "gpt-4o-mini",
                messages = new object[]
                {
                    new
                    {
                        role="system",
                        content="Sen sigorta sektöründe içerik analizi yapan bir uzmansın."
                    },
                    new
                    {
                        role="user",
                        content=prompt
                    }
                },
                max_tokens = 1000,
                temperature = 0.2

            };
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponse = await _httpClient.PostAsync(url, content);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("OpenAI API hatası: " + httpResponse.StatusCode);
            }

            var responseText = await httpResponse.Content.ReadAsStringAsync();
           

            var jsonString = JsonDocument.Parse(responseText);
            var AIText = jsonString.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return AIText;


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

        public async Task<string> GenerateInsuranceConsultationAsync(string userMessage)
        {
            var requestBody = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                new
                {
                    role = "system",
                    content = @"
                Sen deneyimli ve profesyonel bir sigorta danışmanısın.

                Amacın, kullanıcıların ihtiyaçlarını doğru analiz ederek onlara en uygun sigorta çözümlerini önermektir.

                Davranış kuralları:
                - Kullanıcıyla samimi ama kurumsal bir dilde iletişim kur
                - Kısa, net ve anlaşılır cevaplar ver
                - Gerekirse kullanıcıya yönlendirici sorular sorarak ihtiyacını netleştir
                - Teknik terimleri sade ve anlaşılır şekilde açıkla
                - Güven veren ve profesyonel bir üslup kullan

                Kapsam:
                - Sadece sigorta ile ilgili konularda yardımcı ol (araç, sağlık, konut, seyahat vb.)
                - Sigorta dışı sorular sorulursa kibar ve profesyonel bir şekilde şu şekilde yönlendir:
                  'Bu konuda yardımcı olamıyorum. Size sigorta çözümleri hakkında destek verebilirim.'

                Ek hedef:
                - Kullanıcının ihtiyacını anlamadan doğrudan ürün önerme
                - Önce analiz et, sonra öneride bulun
                "
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

           
            return aiText;
        }
    }
}

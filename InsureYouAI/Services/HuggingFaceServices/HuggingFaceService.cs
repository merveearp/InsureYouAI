using InsureYouAI.Entities;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace InsureYouAI.Services.HuggingFaceServices
{
    public class HuggingFaceService : IHuggingFaceService
    {
        private readonly string _apiKey;
        private readonly HttpClient _client;

        public HuggingFaceService(IConfiguration configuration, HttpClient client)
        {

            _client = client;
            _apiKey = configuration["HuggingFace:FaceKey"]; ;
        }

        public async Task<double> GetToxicScore(string text)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://router.huggingface.co/hf-inference/models/unitary/toxic-bert"
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);

            request.Headers.Add("Accept", "application/json");

            var body = new
            {
                inputs = text
            };

            var json = JsonSerializer.Serialize(body);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            Console.WriteLine("TOXIC RESPONSE: " + responseString);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API Error: {response.StatusCode} - {responseString}");
            }

            double maxScore = 0;

            var doc = JsonDocument.Parse(responseString);

            foreach (var item in doc.RootElement[0].EnumerateArray())
            {
                string label = item.GetProperty("label").GetString().ToLower();
                double score = item.GetProperty("score").GetDouble();

                Console.WriteLine($"LABEL: {label} - SCORE: {score}");

                if (label.Contains("toxic") ||
                    label.Contains("insult") ||
                    label.Contains("obscene") ||
                    label.Contains("threat") ||
                    label.Contains("hate"))
                {
                    if (score > maxScore)
                    {
                        maxScore = score;
                    }
                }
            }

            return maxScore;
        }

        public async Task<string> GetTranslateText(Comment comment)
        {

            var request = new HttpRequestMessage(
                HttpMethod.Post,
               "https://router.huggingface.co/hf-inference/models/Helsinki-NLP/opus-mt-tr-en"
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);

            request.Headers.Add("Accept", "application/json");

            var body = new
            {
                inputs = comment.CommentDetail
            };

            var json = JsonSerializer.Serialize(body);

            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            string englishText = comment.CommentDetail;

            if (responseString.TrimStart().StartsWith("["))
            {
                var doc = JsonDocument.Parse(responseString);
                englishText = doc.RootElement[0]
                    .GetProperty("translation_text")
                    .GetString();
            }

            return englishText;
        }
    }
}
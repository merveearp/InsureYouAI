using InsureYouAI.DTOs;
using System.Net.Http.Headers;

namespace InsureYouAI.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public OpenAIService(IConfiguration configuration)
        {
            _apiKey = configuration["OpenAI:ApiKey"];
            if(string.IsNullOrWhiteSpace(_apiKey))

            {
                throw new Exception("Api Key okunamadı");
            }
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

        }

        public Task<OpenAIArticleDto> CreateArticle(string prompt)
        {
            var requesBody = new
            {
                model = "gbt-3.5-turbo",
                messages = new[]
                {
                    new {role="user",content=prompt}
                },
                temperature = 0.7
            };
        }
    }
}

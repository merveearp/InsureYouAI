using InsureYouAI.DTOs;

namespace InsureYouAI.Services
{
    public interface IOpenAIService
    {
        Task<OpenAIArticleDto> CreateArticle(string prompt);
    }
}

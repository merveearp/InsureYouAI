namespace InsureYouAI.Services.OpenAIServices
{
    public interface IOpenAIService
    {
        Task<string> CreateArticleWithAI(string prompt);
    }
}

using InsureYouAI.DTOs.OpenAIDtos;

namespace InsureYouAI.Services.OpenAIServices
{
    public interface IOpenAIService
    {
        Task<string> CreateArticleWithAI(string prompt);
        Task<string> GenerateInsuranceConsultationAsync(string userMessage);
        Task<AIAnalysisDto> AnalyzeDamageAsync(IFormFile image);
        Task<AIPolicyDto> AnalyzePolicyAsync(IFormFile file);
        Task<string> AnalyzeUserAsync(string articles);
        Task<string> AnalyzeCommentUserAsync(string comments);

    }
}

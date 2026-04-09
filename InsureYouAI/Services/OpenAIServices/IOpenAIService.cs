using InsureYouAI.DTOs.OpenAIDtos;

namespace InsureYouAI.Services.OpenAIServices
{
    public interface IOpenAIService
    {
        Task<string> CreateArticleWithAI(string prompt);
        Task<AIMessageDto> GenerateInsuranceConsultationAsync(string userMessage);
        Task<AIAnalysisDto> AnalyzeDamageAsync(IFormFile image);
        Task<AIPolicyDto> AnalyzePolicyAsync(IFormFile file);
    }
}

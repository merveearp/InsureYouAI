using InsureYouAI.DTOs.GeminiDtos;

namespace InsureYouAI.Services.GeminiServices
{
    public interface IGeminiService
    {
        Task<GeminiAboutDto> CreateAboutText();
    }
}

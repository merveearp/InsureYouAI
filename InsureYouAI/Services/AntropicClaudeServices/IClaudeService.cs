using InsureYouAI.DTOs.ClaudeDtos;

namespace InsureYouAI.Services.AntropicClaudeServices
{
    public interface IClaudeService
    {
        Task<ClaudeServiceDto> CreateServiceText();
    }
}

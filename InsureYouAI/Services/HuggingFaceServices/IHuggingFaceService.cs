using InsureYouAI.Entities;

namespace InsureYouAI.Services.HuggingFaceServices
{
    public interface IHuggingFaceService
    {
        Task<string> GetTranslateText(Comment comment);
        Task<double> GetToxicScore(string text);
    }
}

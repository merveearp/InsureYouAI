namespace InsureYouAI.Services.ElevenLabsServices
{
    public interface IElevenLabsService
    {
        Task<string> GenerateSpeechAsync(string text);
    }
}

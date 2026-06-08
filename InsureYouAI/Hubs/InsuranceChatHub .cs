using InsureYouAI.Services.ElevenLabsServices;
using InsureYouAI.Services.OpenAIServices;
using Microsoft.AspNetCore.SignalR;

namespace InsureYouAI.Hubs
{
    public class InsuranceChatHub : Hub
    {
        private readonly IOpenAIService _openAIService;
        private readonly IElevenLabsService _elevenLabsService;

        public InsuranceChatHub(IOpenAIService openAIService, IElevenLabsService elevenLabsService)
        {
            _openAIService = openAIService;
            _elevenLabsService = elevenLabsService;
        }

        public async Task SendMessage(string userMessage)
        {
            await Clients.Caller.SendAsync("ReceiveUserMessage", userMessage);

            await Clients.Caller.SendAsync("Typing");

            var aiResponse = await _openAIService
                .GenerateInsuranceConsultationAsync(userMessage);

            var audioUrl = await _elevenLabsService
                .GenerateSpeechAsync(aiResponse);

            await Clients.Caller.SendAsync(
                "ReceiveAIMessage",
                aiResponse,
                audioUrl);
        }
    }
}
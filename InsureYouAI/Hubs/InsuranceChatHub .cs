using InsureYouAI.Services.OpenAIServices;
using Microsoft.AspNetCore.SignalR;

namespace InsureYouAI.Hubs
{
    public class InsuranceChatHub : Hub
    {
        private readonly IOpenAIService _openAIService;

        public InsuranceChatHub(IOpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        public async Task SendMessage(string userMessage)
        {
            await Clients.Caller.SendAsync("ReceiveUserMessage", userMessage);

            await Clients.Caller.SendAsync("Typing");

            var aiResponse = await _openAIService.GenerateInsuranceConsultationAsync(userMessage);

            await Clients.Caller.SendAsync("ReceiveAIMessage", aiResponse);
        }
    }
}
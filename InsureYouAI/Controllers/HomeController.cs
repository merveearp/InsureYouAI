using InsureYouAI.Services.OpenAIServices;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InsureYouAI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOpenAIService _aiService;

        public HomeController(IOpenAIService openAIService)
        {
            _aiService = openAIService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetConsultation(string message)
        {
            var result = await _aiService.GenerateInsuranceConsultationAsync(message);
            return Json(result);
        }


    }
}

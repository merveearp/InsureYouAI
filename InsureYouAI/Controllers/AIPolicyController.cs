using InsureYouAI.Services.OpenAIServices;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Controllers
{
    public class AIPolicyController : Controller
    {
        private readonly IOpenAIService _openAIService;

        public AIPolicyController(IOpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> OpenAIAnalyzePolicy(IFormFile file)
        {
            var result = await _openAIService.AnalyzePolicyAsync(file);
            return Json(result);
        }
    }
}

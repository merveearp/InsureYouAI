using InsureYouAI.Services.OpenAIServices;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Controllers
{
    public class AIAnalysisController : Controller
    {
        private readonly IOpenAIService _openAIService;

        public AIAnalysisController(IOpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> OpenAIAnalysis(IFormFile image)
        {
            if(image == null || image.Length == 0)
            {
                return BadRequest("Lütfen Bir Görsel Seçiniz!");
            }
            var result = await _openAIService.AnalyzeDamageAsync(image);
            return Json(result);
        }
    }
}

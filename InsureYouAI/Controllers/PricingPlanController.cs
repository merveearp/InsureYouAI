using InsureYouAI.Models;
using InsureYouAI.Services.OpenAIServices;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Controllers
{
    public class PricingPlanController : Controller
    {
        private readonly IOpenAIService _openAIService;

        public PricingPlanController(IOpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new AIInsuranceRecommendationViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserCustomizePlan(
     AIInsuranceRecommendationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            try
            {
                model = await _openAIService
                    .CreateInsuranceRecommendationAsync(model);

                return View("Index", model);
            }
            catch (Exception ex)
            {
                TempData["Error"] =
                    "AI önerisi oluşturulurken bir hata oluştu.";

                return View("Index", model);
            }
        }
    }
}

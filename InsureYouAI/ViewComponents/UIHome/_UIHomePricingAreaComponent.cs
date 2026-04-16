using InsureYouAI.Repositories.PlanRepositories;
using InsureYouAI.Repositories.PricingPlanRepositories;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.UIHome
{
    public class _UIHomePricingAreaComponent :ViewComponent
    {
        private readonly IPlanRepository _planRepository;

        public _UIHomePricingAreaComponent(IPlanRepository planRepository)
        {
            _planRepository = planRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _planRepository.GetAllAsync();
            return View(values);
        }
    }
}

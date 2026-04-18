using InsureYouAI.Repositories.PricingPlanRepositories;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.UIHome
{
    public class _UIHomePricingPlanComponent :ViewComponent
    {
        private readonly IPricingPlanRepository _pricingPlanRepository;

        public _UIHomePricingPlanComponent(IPricingPlanRepository pricingPlanRepository)
        {
            _pricingPlanRepository = pricingPlanRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _pricingPlanRepository.GetAllAsync();
            var planList = values.Where(x => x.IsFeature).ToList();
            return View(planList);
        }
    }
}

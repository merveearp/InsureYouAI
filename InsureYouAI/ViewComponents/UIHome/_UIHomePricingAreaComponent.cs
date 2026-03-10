using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.UIHome
{
    public class _UIHomePricingAreaComponent :ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}

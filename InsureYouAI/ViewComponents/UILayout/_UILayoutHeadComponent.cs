using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.UILayout
{
    public class _UILayoutHeadComponent :ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}

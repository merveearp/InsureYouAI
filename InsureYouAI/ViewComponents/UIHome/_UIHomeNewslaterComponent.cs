using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.UIHome
{
    public class _UIHomeNewslaterComponent :ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}

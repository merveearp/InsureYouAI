using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.UILayout
{
    public class _UILayoutMenuComponent :ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}

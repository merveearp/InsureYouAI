using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.UIBlogDetail
{
    public class _UIBlogDetailNextEndPrevPostComponent :ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}

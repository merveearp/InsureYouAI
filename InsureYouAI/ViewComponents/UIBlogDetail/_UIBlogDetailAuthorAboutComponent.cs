using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.UIBlogDetail
{
    public class _UIBlogDetailAuthorAboutComponent :ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}

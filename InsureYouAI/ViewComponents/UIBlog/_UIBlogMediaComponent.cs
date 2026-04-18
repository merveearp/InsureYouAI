using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.UIBlog
{
    public class _UIBlogMediaComponent :ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}

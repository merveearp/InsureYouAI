using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.UIBlog
{
    public class _UIBlogTagComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}

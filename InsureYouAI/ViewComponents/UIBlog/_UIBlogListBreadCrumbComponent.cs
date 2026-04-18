using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.UIBlog
{
    public class _UIBlogListBreadCrumbComponent :ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}

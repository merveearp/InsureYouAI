using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Areas.Admin.ViewComponents.AdminDashboard
{
    public class _DashboardWidgetComponent : ViewComponent
    {
        private readonly InsureContext _context;

        public _DashboardWidgetComponent(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.CategoryCount = _context.Categories.Count();

            ViewBag.UserCount = _context.Users.Count();

            ViewBag.ArticleCount = _context.Articles.Count();

            ViewBag.ArticleViewCount = _context.Articles
                .Sum(x => x.ViewCount);

            return View();
        }
    }
}
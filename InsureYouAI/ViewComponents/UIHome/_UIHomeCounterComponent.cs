using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.UIHome
{
    public class _UIHomeCounterComponent :ViewComponent
    {
        private readonly InsureContext _context;

        public _UIHomeCounterComponent(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.CategoryCount = _context.Categories.Count();
            ViewBag.ServiceCount = _context.Services.Count();
            ViewBag.CustomerCount = _context.Users.Count();
            ViewBag.ArticleCount = _context.Articles.Count();

            return View();
        }
    }
}

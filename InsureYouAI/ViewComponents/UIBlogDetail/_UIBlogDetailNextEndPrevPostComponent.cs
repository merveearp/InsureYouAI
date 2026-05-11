using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAI.ViewComponents.UIBlogDetail
{
    public class _UIBlogDetailNextEndPrevPostComponent : ViewComponent
    {
        private readonly InsureContext _context;

        public _UIBlogDetailNextEndPrevPostComponent(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var previousArticle = await _context.Articles
                .Where(x => x.ArticleId < id)
                .OrderByDescending(x => x.ArticleId)
                .FirstOrDefaultAsync();

            var nextArticle = await _context.Articles
                .Where(x => x.ArticleId > id)
                .OrderBy(x => x.ArticleId)
                .FirstOrDefaultAsync();

            ViewBag.PreviousArticle = previousArticle;
            ViewBag.NextArticle = nextArticle;

            return View();
        }
    }
}
using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAI.ViewComponents.UIBlogDetail
{
    public class _UIBlogDetailAuthorAboutComponent :ViewComponent
    {
        private readonly InsureContext _context;

        public _UIBlogDetailAuthorAboutComponent(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var value = await _context.Articles.Where(x => x.ArticleId == id).FirstOrDefaultAsync();
            return View(value);
        }
    }
}

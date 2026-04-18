using InsureYouAI.Repositories.ArticleRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;

namespace InsureYouAI.ViewComponents.UIHome
{
    public class _UIHomeBlogComponent :ViewComponent
    {
        private readonly IArticleRepository _articleRepository;

        public _UIHomeBlogComponent(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _articleRepository.GetAllAsync();
            var recentBlog = values.OrderByDescending(x => x.ArticleId).Take(3).ToList();
            return View(recentBlog);
        }
    }
}

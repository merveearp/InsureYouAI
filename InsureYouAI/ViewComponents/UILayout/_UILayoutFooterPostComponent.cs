using InsureYouAI.Repositories.ArticleRepositories;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.UILayout
{
    public class _UILayoutFooterPostComponent :ViewComponent
    {
        private readonly IArticleRepository _articleRepository;

        public _UILayoutFooterPostComponent(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _articleRepository.GetAllAsync();
            var lastBlogs = values.OrderByDescending(x => x.ArticleId).Skip(4).Take(2).ToList();  
 
            return View(lastBlogs);
        }
    }
}

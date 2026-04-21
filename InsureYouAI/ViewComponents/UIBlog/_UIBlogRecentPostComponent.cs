using InsureYouAI.Repositories.ArticleRepositories;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.UIBlog
{

    public class _UIBlogRecentPostComponent :ViewComponent
    {

        private readonly IArticleRepository _articleRepository;

        public _UIBlogRecentPostComponent(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _articleRepository.GetAllAsync();    
            var recentBlogs= values.OrderByDescending(x=>x.ArticleId).Take(3).ToList();
            return View(recentBlogs);
        }
    }
}

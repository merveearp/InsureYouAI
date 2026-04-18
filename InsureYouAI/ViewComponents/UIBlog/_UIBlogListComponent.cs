using InsureYouAI.Repositories.ArticleRepositories;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.UIBlog
{
    public class _UIBlogListComponent :ViewComponent
    {
        private readonly IArticleRepository _articleRepository;

        public _UIBlogListComponent(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _articleRepository.GetAllAsync();
            return View(values);
        }
           
    }
}

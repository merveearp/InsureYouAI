using InsureYouAI.Context;
using InsureYouAI.Repositories.ArticleRepositories;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.UIBlogDetail
{
    public class _UIBlogDetailContentComponent :ViewComponent
    {
        private readonly IArticleRepository _articleRepository;

        public _UIBlogDetailContentComponent(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var value = await _articleRepository.GetByIdAsync(id);
            return View(value);
        }
    }
}

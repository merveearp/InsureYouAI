using InsureYouAI.Repositories.CategoryRepositories;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.UIBlog
{
    public class _UIBlogCategoryComponent:ViewComponent
    {
        private readonly ICategoryRepository _categoryRepository;

        public _UIBlogCategoryComponent(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _categoryRepository.GetAllAsync();
            return View(values);
        }
    }
}

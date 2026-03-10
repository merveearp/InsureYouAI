using InsureYouAI.Repositories.AboutRepositories;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.UIHome
{
    public class _UIHomeAboutComponent : ViewComponent
    {
        private readonly IAboutRepository _aboutRepository;

        public _UIHomeAboutComponent(IAboutRepository aboutRepository)
        {
            _aboutRepository = aboutRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _aboutRepository.GetAllAsync();
            return View(values);
        }
    }
}

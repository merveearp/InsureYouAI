using InsureYouAI.Repositories.AboutItemRepositories;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.UIHome
{
    public class _UIHomeAboutItemComponent:ViewComponent
    {
        private readonly IAboutItemRepository _aboutItemRepository;

        public _UIHomeAboutItemComponent(IAboutItemRepository aboutItemRepository)
        {
            _aboutItemRepository = aboutItemRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _aboutItemRepository.GetAllAsync();
            return View(values);
        }
    }
}

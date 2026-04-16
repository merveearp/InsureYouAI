using InsureYouAI.Repositories.ContactRepositories;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.UILayout
{
    public class _UILayoutHeaderContactComponent: ViewComponent
    {
        private readonly IContactRepository _contactRepository;

        public _UILayoutHeaderContactComponent(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var value = await _contactRepository.GetAllAsync();
            return View(value);
        }
    }
}

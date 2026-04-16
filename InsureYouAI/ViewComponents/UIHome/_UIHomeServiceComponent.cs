using InsureYouAI.Repositories.ServiceRepositories;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.UIHome
{
    public class _UIHomeServiceComponent :ViewComponent
    {
        private readonly IServiceRepository _serviceRepository;

        public _UIHomeServiceComponent(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _serviceRepository.GetAllAsync();
            return View(values);
        }
    }
}

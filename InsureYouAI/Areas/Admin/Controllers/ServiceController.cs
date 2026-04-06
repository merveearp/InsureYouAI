using InsureYouAI.Entities;
using InsureYouAI.Repositories.ServiceRepositories;
using InsureYouAI.Services.AntropicClaudeServices;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ServiceController : Controller
    {
        private readonly IServiceRepository _repository;
        private readonly IClaudeService _claudeService;

        public ServiceController(IServiceRepository repository, IClaudeService claudeService)
        {
            _repository = repository;
            _claudeService = claudeService;
        }

        public async Task<IActionResult> ServiceList()
        {
            var values = await _repository.GetAllAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Service service)
        {
            await _repository.CreateAsync(service);
            return RedirectToAction("ServiceList");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var value = _repository.GetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Service service)
        {

            await _repository.UpdateAsync(service);
            return RedirectToAction("ServiceList");

        }
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction("ServiceList");
        }

        [HttpGet]
        public async Task<IActionResult> CreateClaudeServiceText()
        {
            var result = await _claudeService.CreateServiceText();
            return Json(result);
        }
    }
}

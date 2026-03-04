using InsureYouAI.Entities;
using InsureYouAI.Repositories.PricingPlanRepositories;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class PricingPlanController : Controller
    {
        private readonly IPricingPlanRepository _repository;

        public PricingPlanController(IPricingPlanRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> PricingPlanList()
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
        public async Task<IActionResult> Create(PricingPlan plan)
        {
            await _repository.CreateAsync(plan);
            return RedirectToAction("PricingPlanList");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var value = _repository.GetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PricingPlan plan)
        {

            await _repository.UpdateAsync(plan);
            return RedirectToAction("PricingPlanList");

        }
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction("PricingPlanList");
        }
    }
}

using InsureYouAI.Entities;
using InsureYouAI.Repositories.PlanRepositories;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace InsureYouAI.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class PlanController : Controller
    {

        private readonly IPlanRepository _repository;

        public PlanController(IPlanRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> PlanList()
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
        public async Task<IActionResult> Create(Plan plan)
        {
            await _repository.CreateAsync(plan);
            return RedirectToAction("PlanList");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var value = _repository.GetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Plan plan)
        {

            await _repository.UpdateAsync(plan);
            return RedirectToAction("PlanList");

        }
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction("PlanList");
        }
    }
}

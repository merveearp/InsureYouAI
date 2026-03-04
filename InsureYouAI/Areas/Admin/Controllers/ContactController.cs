using InsureYouAI.Entities;
using InsureYouAI.Repositories.ContactRepositories;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ContactController : Controller
    {
        private readonly IContactRepository _repository;

        public ContactController(IContactRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> ContactList()
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
        public async Task<IActionResult> Create(Contact contact)
        {
            await _repository.CreateAsync(contact);
            return RedirectToAction("ContactList");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var value = _repository.GetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Contact contact)
        {

            await _repository.UpdateAsync(contact);
            return RedirectToAction("ContactList");

        }
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction("ContactList");
        }
    }
}

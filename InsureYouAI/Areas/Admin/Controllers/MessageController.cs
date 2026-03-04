using InsureYouAI.Entities;
using InsureYouAI.Repositories.MessageRepositories;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class MessageController : Controller
    {
        private readonly IMessageRepository _repository;

        public MessageController(IMessageRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> MessageList()
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
        public async Task<IActionResult> Create(Message message)
        {
            await _repository.CreateAsync(message);
            return RedirectToAction("MessageList");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var value = _repository.GetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Message message)
        {

            await _repository.UpdateAsync(message);
            return RedirectToAction("MessageList");

        }
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction("MessageList");
        }
    }
}

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
            ViewBag.ReadMessage = values.Count(x =>x.IsRead ==true);
            ViewBag.UnReadMessage = values.Count(x =>x.IsRead == false);
            return View(values);
        }

        public async Task<IActionResult> IsReadList()
        {
            var values = await _repository.GetAllAsync();
            var readMessages = values.Where(x => x.IsRead).ToList();
            return View("MessageList", readMessages);
        }

        public async Task<IActionResult> UnReadList()
        {
            var values = await _repository.GetAllAsync();
            var unreadMessages = values.Where(x => !x.IsRead).ToList();
            return View("MessageList", unreadMessages);
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
            message.IsRead = false;
            message.SendDate = DateTime.Now;
            return RedirectToAction("MessageList");
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var value =await _repository.GetByIdAsync(id);
            return View(value);
        }

        public async Task<IActionResult> ChangeStatus(int id)
        {
            await _repository.GetReadMessage(id);
            return RedirectToAction("MessageList");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction("MessageList");
        }
    }
}

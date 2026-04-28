using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Areas.Admin.Controllers
{
    public class ChatController : Controller
    {
        [Area("Admin")]
        public IActionResult SendChatWithAI()
        {
            return View();
        }
    }
}

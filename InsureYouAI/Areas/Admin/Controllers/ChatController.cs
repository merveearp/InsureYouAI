using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ChatController : Controller
    {
        public IActionResult SendChatWithAI()
        {
            ViewBag.ControllerName = "AI Sohbet";

            return View();
        }
    }
}
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Controllers
{
    public class AIPolicyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

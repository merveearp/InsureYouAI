using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ErrorController : Controller
    {
        [Route("Error/404")]
        public IActionResult Page404()
        {
            return View();
        }
    }
}

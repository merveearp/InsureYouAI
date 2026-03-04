using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InsureYouAI.Controllers
{
    public class HomeController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }

        
    }
}

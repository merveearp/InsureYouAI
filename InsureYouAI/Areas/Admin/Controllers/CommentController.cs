using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommentController : Controller
    {
        private readonly InsureContext _context;

        public CommentController(InsureContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> CommentList()
        {
            var values = _context.Comments.Include(x=>x.Article).Include(x=>x.AppUser).ToList();
            return View(values);
        }
    }
}

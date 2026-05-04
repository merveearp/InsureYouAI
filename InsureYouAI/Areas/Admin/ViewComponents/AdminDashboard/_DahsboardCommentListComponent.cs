using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Areas.Admin.ViewComponents.AdminDashboard
{
    public class _DahsboardCommentListComponent: ViewComponent
    {
        private readonly InsureContext _context;

        public _DahsboardCommentListComponent(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = _context.Comments.OrderByDescending(x => x.CommentDate).Take(7).ToList();
            return View(values);
        }
    }
}

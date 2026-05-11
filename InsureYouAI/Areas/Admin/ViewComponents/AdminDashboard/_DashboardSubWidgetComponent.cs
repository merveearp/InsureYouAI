using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Areas.Admin.ViewComponents.AdminDashboard
{
    public class _DashboardSubWidgetComponent : ViewComponent
    {
        private readonly InsureContext _context;

        public _DashboardSubWidgetComponent(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.AIChatCount = _context.ClaudeAIMessages.Count();

            ViewBag.ToxicCommentCount = _context.Comments
                .Count(x => x.CommentStatus == "Toksik Yorum");

            ViewBag.MessageCount = _context.Messages.Count();

            ViewBag.ApprovedCommentCount = _context.Comments
                .Count(x => x.CommentStatus == "Yorum Onaylandı");

            return View();
        }
    }
}
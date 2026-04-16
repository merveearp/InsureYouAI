using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.UILayout
{
    public class _UILayoutMenuComponent :ViewComponent
    {
        private readonly InsureContext _context;

        public _UILayoutMenuComponent(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.Mail = _context.Contacts.Select(x => x.Email).FirstOrDefault();
            ViewBag.Phone = _context.Contacts.Select(x => x.PhoneNumber).FirstOrDefault();
            return View();
        }
    }
}

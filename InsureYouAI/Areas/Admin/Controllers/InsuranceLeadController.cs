using InsureYouAI.Areas.Admin.Models;
using InsureYouAI.Context;
using InsureYouAI.Services.ZohoServices;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InsuranceLeadController : Controller
    {
        private readonly InsureContext _context;
        private readonly IZohoService _zohoService;

        public InsuranceLeadController(InsureContext context, IZohoService zohoService)
        {
            _context = context;
            _zohoService = zohoService;
        }

        public IActionResult Index()
        {
            ViewBag.ControllerName = "Müşteri Adayları";

            var values = _context.InsuranceLeads
                .OrderByDescending(x => x.CreatedDate)
                .Select(x => new InsuranceLeadViewModel
                {
                    InsuranceLeadId = x.InsuranceLeadId,
                    FullName = x.FirstName + " " + x.LastName,
                    Email = x.Email,
                    Phone = x.Phone,
                    InsuranceType = x.InsuranceType,
                    ZohoSyncStatus = x.ZohoSyncStatus ?? "Beklemede",
                    IsSentToZoho = x.IsSentToZoho,
                    CreatedDate = x.CreatedDate,
                    ZohoLeadId = x.ZohoLeadId,
                    ZohoErrorMessage = x.ZohoErrorMessage,
                    LeadStatus = x.LeadStatus
                })
                .ToList();

            return View(values);
        }

        [HttpPost]
        public async Task<IActionResult> SendToZoho(int id)
        {
            var lead = await _context.InsuranceLeads.FindAsync(id);

            if (lead == null)
            {
                return NotFound();
            }

            var zohoResult = await _zohoService.CreateLeadAsync(lead);

            if (zohoResult.IsSuccess)
            {
                lead.IsSentToZoho = true;
                lead.ZohoSyncStatus = "Success";
                lead.ZohoLeadId = zohoResult.ZohoLeadId;
                lead.ZohoErrorMessage = null;
            }
            else
            {
                lead.IsSentToZoho = false;
                lead.ZohoSyncStatus = "Failed";
                lead.ZohoErrorMessage = zohoResult.ErrorMessage;
            }

            _context.Update(lead);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public IActionResult LeadDetail(int id)
        {
            ViewBag.ControllerName = "Müşteri Adayları";

            var value = _context.InsuranceLeads
                .Where(x => x.InsuranceLeadId == id)
                .Select(x => new InsuranceLeadViewModel
                {
                    InsuranceLeadId = x.InsuranceLeadId,
                    FullName = x.FirstName + " " + x.LastName,
                    Email = x.Email,
                    Phone = x.Phone,
                    InsuranceType = x.InsuranceType,
                    ZohoSyncStatus = x.ZohoSyncStatus,
                    IsSentToZoho = x.IsSentToZoho,
                    CreatedDate = x.CreatedDate,
                    ZohoLeadId = x.ZohoLeadId,
                    ZohoErrorMessage = x.ZohoErrorMessage,
                    Message = x.Message,
                    LeadStatus = x.LeadStatus
                })
                .FirstOrDefault();

            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> SyncStatusesFromZoho()
        {
            var accessToken = await _zohoService.GetAccessTokenAsync();

            var leads = _context.InsuranceLeads
                .Where(x => !string.IsNullOrEmpty(x.ZohoLeadId))
                .ToList();

            foreach (var lead in leads)
            {
                var zohoStatus = await _zohoService.GetLeadStatusAsync(
                    lead.ZohoLeadId,
                    accessToken);

                if (!string.IsNullOrEmpty(zohoStatus))
                {
                    lead.LeadStatus = zohoStatus;
                }
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Zoho CRM durumları sisteme aktarıldı.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLeadStatus(int id, string status)
        {
            var lead = await _context.InsuranceLeads.FindAsync(id);

            if (lead == null)
                return NotFound();

            if (string.IsNullOrEmpty(lead.ZohoLeadId))
            {
                TempData["Error"] = "Bu kayıt Zoho CRM ile senkronize edilmemiş.";
                return RedirectToAction("LeadDetail", new { id });
            }

            var result = await _zohoService.UpdateLeadStatusAsync(lead.ZohoLeadId, status);

            if (result)
            {
                lead.LeadStatus = status;
                await _context.SaveChangesAsync();

                TempData["Success"] = "Lead durumu Zoho CRM ve sistemde güncellendi.";
            }
            else
            {
                TempData["Error"] = "Zoho CRM durum güncelleme başarısız oldu.";
            }

            return RedirectToAction("LeadDetail", new { id = lead.InsuranceLeadId });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLeadNote(int id, string adminNote)
        {
            var lead = await _context.InsuranceLeads.FindAsync(id);

            if (lead == null)
                return NotFound();

            lead.AdminNote = adminNote;

            if (!string.IsNullOrEmpty(lead.ZohoLeadId) && !string.IsNullOrWhiteSpace(adminNote))
            {
                await _zohoService.AddLeadNoteAsync(lead.ZohoLeadId, adminNote);
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Admin notu kaydedildi ve Zoho CRM'e gönderildi.";

            return RedirectToAction("LeadDetail", new { id });
        }
    }
}
namespace InsureYouAI.Areas.Admin.Models
{
    public class InsuranceLeadViewModel
    {
        public int InsuranceLeadId { get; set; }
        public string? ZohoLeadId { get; set; }
        public string? ZohoErrorMessage { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string InsuranceType { get; set; }
        public string ZohoSyncStatus { get; set; }
        public bool IsSentToZoho { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Message { get; set; }


        // Zoho CRM Müşteri Adayı Durumu
        public string? LeadStatus { get; set; }
        public string? AdminNote { get; set; }
    }
}

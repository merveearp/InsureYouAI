namespace InsureYouAI.Entities
{
    public class InsuranceLead
    {
        public int InsuranceLeadId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }

        public string InsuranceType { get; set; }
        public string Message { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsSentToZoho { get; set; }

        public string? ZohoLeadId { get; set; }
        public string? ZohoSyncStatus { get; set; }
        public string? ZohoErrorMessage { get; set; }

        // Zoho CRM Müşteri Adayı Durumu
        public string? LeadStatus { get; set; }

        // Zoho CRM Müşteri Adayı Not 
        public string? AdminNote { get; set; }
    }
}
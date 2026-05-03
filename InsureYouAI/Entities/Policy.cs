namespace InsureYouAI.Entities
{
    public class Policy
    {
        public int PolicyId { get; set; }
        public string? PolicyNumber { get; set; }
        public string? PolicyType { get; set; }
        public string AppUserId { get; set; }
        public decimal PremiumAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = "Active";
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;


        //Navigation Property

        public virtual AppUser? AppUser { get; set; }
    }
}

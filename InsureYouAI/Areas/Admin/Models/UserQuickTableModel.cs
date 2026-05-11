namespace InsureYouAI.Areas.Admin.Models
{
    public class UserQuickTableModel
    {
        public string FullName { get; set; }
        public string ImageUrl { get; set; }

        public int PolicyCount { get; set; }

        public decimal TotalPremiumAmount { get; set; }
    }
}

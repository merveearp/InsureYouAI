namespace InsureYouAI.Entities
{
    public class PricingPlan
    {
        public int PricingPlanId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsFeature { get; set; }
        public decimal Price { get; set; }
    }
}

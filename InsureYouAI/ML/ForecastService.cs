namespace InsureYouAI.ML
{
    public class PolicySalesData
    {
        public DateTime Date { get; set; }
        public float SalesCount { get; set; }
    }

    public class PolcySalesForecast
    {
        public float[] ForecastedValues { get; set; }
        public float[] LowerBoundValues { get; set; }
        public float[] UpperBoundValues { get; set; }
    }
    public class ForecastService
    {
    }
}

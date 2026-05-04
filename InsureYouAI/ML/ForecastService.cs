using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;

namespace InsureYouAI.ML
{
    public class PolicySalesData
    {
        public DateTime Date { get; set; }
        public float SalesCount { get; set; }
    }

    public class PolicySalesForecast
    {
        public float[] ForecastedValues { get; set; }
        public float[] LowerBoundValues { get; set; }
        public float[] UpperBoundValues { get; set; }
    }
    public class ForecastService
    {
        private readonly MLContext _mLContext;

        public ForecastService()
        {
            _mLContext = new MLContext();
        }

        public PolicySalesForecast GetForecast(List<PolicySalesData> salesData, int horizon = 12)
        {
            var dataView = _mLContext.Data.LoadFromEnumerable(salesData);

            var forecastingPipeline = _mLContext.Forecasting.ForecastBySsa(
                outputColumnName: nameof(PolicySalesForecast.ForecastedValues),
                inputColumnName: nameof(PolicySalesData.SalesCount),
                windowSize: 3,
                seriesLength: salesData.Count,
                trainSize: salesData.Count,
                horizon: horizon,
                confidenceLevel: 0.95f,
                confidenceLowerBoundColumn: nameof(PolicySalesForecast.LowerBoundValues),
                confidenceUpperBoundColumn: nameof(PolicySalesForecast.UpperBoundValues)
            );

            var model = forecastingPipeline.Fit(dataView);

            var forecastingEngine =
                model.CreateTimeSeriesEngine<PolicySalesData, PolicySalesForecast>(_mLContext);

            return forecastingEngine.Predict();
        }
    }
}

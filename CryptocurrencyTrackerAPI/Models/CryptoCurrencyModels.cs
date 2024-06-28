using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptocurrencyTrackerAPI.Models
{
    public class CryptocurrencyPrice
    {
        public float PreviousPrice { get; set; }
        public float FuturePrice { get; set; }
        public float Price { get; set; }
        public float[] PredictedSpike { get; set; } = Array.Empty<float>();
    }

    /// Represents the prediction result for a cryptocurrency price, including the future price and anomaly detection results.
    public class CryptocurrencyPricePrediction
    {
        public float FuturePrice { get; set; }
        public float[]? PredictedSpike { get; set; }
    }
}
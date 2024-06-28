using CryptocurrencyTrackerAPI.Models;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.TimeSeries;
using System.Collections.Generic;

namespace CryptocurrencyTrackerAPI.Services
{

    /// <summary>
    /// Provides methods to detect anomalies in cryptocurrency prices using machine learning models.
    /// </summary>
    public class AnomalyDetectionService
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _model;

        public AnomalyDetectionService()
        {
            _mlContext = new MLContext();
            var data = LoadTrainingData();
            var pipeline = BuildPipeline();
            _model = TrainModel(data, pipeline);
        }

        private IDataView LoadTrainingData()
        {
            // Load your training data here
            return _mlContext.Data.LoadFromTextFile<CryptocurrencyPrice>("data/crypto_prices.csv", separatorChar: ',', hasHeader: true);
        }

        private IEstimator<ITransformer> BuildPipeline()
        {
            return _mlContext.Transforms.DetectIidSpike(outputColumnName: nameof(CryptocurrencyPrice.PredictedSpike), inputColumnName: nameof(CryptocurrencyPrice.Price), confidence: 0.95, pvalueHistoryLength: 12);
        }

        private ITransformer TrainModel(IDataView data, IEstimator<ITransformer> pipeline)
        {
            return pipeline.Fit(data);
        }

        public bool DetectAnomaly(float[] prices)
        {
            var data = new List<CryptocurrencyPrice>();
            foreach (var price in prices)
            {
                data.Add(new CryptocurrencyPrice { Price = price });
            }

            var dataView = _mlContext.Data.LoadFromEnumerable(data);
            var transformedData = _model.Transform(dataView);

            var predictions = _mlContext.Data.CreateEnumerable<CryptocurrencyPricePrediction>(transformedData, reuseRowObject: false);
            foreach (var prediction in predictions)
            {
                if (prediction.PredictedSpike != null && prediction.PredictedSpike.Length > 0 && prediction.PredictedSpike[0] == 1)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

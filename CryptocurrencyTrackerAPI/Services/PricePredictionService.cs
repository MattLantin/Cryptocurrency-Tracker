using Microsoft.ML;
using Microsoft.ML.Data;
using CryptocurrencyTrackerAPI.Models;

namespace CryptocurrencyTrackerAPI.Services
{
    /// Provides methods to predict future cryptocurrency prices using machine learning models.
    public class PricePredictionService
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _model;

        public PricePredictionService()
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
            return _mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "FuturePrice")
                .Append(_mlContext.Transforms.Concatenate("Features", "PreviousPrice"))
                .Append(_mlContext.Regression.Trainers.FastTree());
        }

        private ITransformer TrainModel(IDataView data, IEstimator<ITransformer> pipeline)
        {
            return pipeline.Fit(data);
        }

        public float Predict(float previousPrice)
        {
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<CryptocurrencyPrice, CryptocurrencyPricePrediction>(_model);
            var prediction = predictionEngine.Predict(new CryptocurrencyPrice { PreviousPrice = previousPrice });
            return prediction.FuturePrice;
        }
    }
}

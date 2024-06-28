using Microsoft.ML;
using Microsoft.ML.Data;
using CryptocurrencyTrackerAPI.Models; // Add this

namespace CryptocurrencyTrackerAPI.Services
{
    /// <summary>
    /// Provides methods to analyze sentiment from text data using machine learning models.
    /// </summary>
    public class SentimentAnalysisService
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _model;

        public SentimentAnalysisService()
        {
            _mlContext = new MLContext();
            var data = LoadTrainingData();
            var pipeline = BuildPipeline();
            _model = TrainModel(data, pipeline);
        }

        private IDataView LoadTrainingData()
        {
            // Load your training data here
            return _mlContext.Data.LoadFromTextFile<SentimentData>("data/sentiment_data.csv", separatorChar: ',', hasHeader: true);
        }

        private IEstimator<ITransformer> BuildPipeline()
        {
            return _mlContext.Transforms.Text.FeaturizeText("Features", nameof(SentimentData.Text))
                .Append(_mlContext.Transforms.CopyColumns("Label", nameof(SentimentData.Sentiment)))
                .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression("Label", "Features"));
        }

        private ITransformer TrainModel(IDataView data, IEstimator<ITransformer> pipeline)
        {
            return pipeline.Fit(data);
        }

        public SentimentPrediction Predict(string text)
        {
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(_model);
            return predictionEngine.Predict(new SentimentData { Text = text });
        }
    }
}

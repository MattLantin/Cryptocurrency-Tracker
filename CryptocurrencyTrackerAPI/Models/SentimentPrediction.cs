namespace CryptocurrencyTrackerAPI.Models
{
    /// Represents the prediction result for sentiment analysis, including the predicted sentiment, probability, and score.
    public class SentimentPrediction : SentimentData
    {
        public bool Prediction { get; set; }
        public float Probability { get; set; }
        public float Score { get; set; }
    }
}

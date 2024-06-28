using Microsoft.AspNetCore.Mvc;
using CryptocurrencyTrackerAPI.Services;
using CryptocurrencyTrackerAPI.Models;
using System.Collections.Generic;

namespace CryptocurrencyTrackerAPI.Controllers
{
    /// Handles API requests for cryptocurrency data, including getting current prices, predicting future prices, detecting anomalies, and analyzing sentiment.
    [ApiController]
    [Route("api/[controller]")]
    public class CryptocurrencyController : ControllerBase
    {
        private readonly CryptocurrencyService _cryptoService;
        private readonly PricePredictionService _pricePredictionService;
        private readonly AnomalyDetectionService _anomalyDetectionService;
        private readonly SentimentAnalysisService _sentimentAnalysisService;

        public CryptocurrencyController(CryptocurrencyService cryptoService, PricePredictionService pricePredictionService, AnomalyDetectionService anomalyDetectionService, SentimentAnalysisService sentimentAnalysisService)
        {
            _cryptoService = cryptoService;
            _pricePredictionService = pricePredictionService;
            _anomalyDetectionService = anomalyDetectionService;
            _sentimentAnalysisService = sentimentAnalysisService;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return _cryptoService.GetCryptocurrencies();
        }

        [HttpGet("{name}")]
        public ActionResult<decimal> GetPrice(string name)
        {
            var price = _cryptoService.GetPrice(name);
            if (price == 0m)
            {
                return NotFound();
            }
            return price;
        }

        [HttpGet("{name}/predict")]
        public ActionResult<float> PredictPrice(string name)
        {
            var currentPrice = _cryptoService.GetPrice(name);
            if (currentPrice == 0m)
            {
                return NotFound();
            }
            var predictedPrice = _pricePredictionService.Predict((float)currentPrice);
            return predictedPrice;
        }

        [HttpPost("detect-anomaly")]
        public ActionResult<bool> DetectAnomaly([FromBody] float[] prices)
        {
            var isAnomaly = _anomalyDetectionService.DetectAnomaly(prices);
            return isAnomaly;
        }

        [HttpPost("analyze-sentiment")]
        public ActionResult<SentimentPrediction> AnalyzeSentiment([FromBody] string text)
        {
            var sentiment = _sentimentAnalysisService.Predict(text);
            return sentiment;
        }
    }
}

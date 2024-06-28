using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptocurrencyTrackerAPI.Models
{
    /// Represents the data used for sentiment analysis, including the text and the sentiment label.
    public class SentimentData
    {
        public string? Text { get; set; }
        public bool Sentiment { get; set; }
    }
}
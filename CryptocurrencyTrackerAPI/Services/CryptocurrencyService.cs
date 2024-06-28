using System.Collections.Generic;

/// Provides methods to get the list of cryptocurrencies and their current prices.

namespace CryptocurrencyTrackerAPI.Services
{
    public class CryptocurrencyService
    {
        private readonly Dictionary<string, decimal> _prices = new Dictionary<string, decimal>
        {
            { "Bitcoin", 45000m },
            { "Ethereum", 3000m },
            { "Ripple", 0.75m }
        };

        public IEnumerable<string> GetCryptocurrencies()
        {
            return _prices.Keys;
        }

        public decimal GetPrice(string name)
        {
            if (_prices.TryGetValue(name, out var price))
            {
                return price;
            }
            return 0m;
        }
    }
}

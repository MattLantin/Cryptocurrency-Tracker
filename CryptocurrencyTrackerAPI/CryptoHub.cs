using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace CryptocurrencyTrackerAPI
{
     /// Provides a SignalR hub for real-time updates of cryptocurrency prices.
    public class CryptoHub : Hub
    {
        public async Task SendPriceUpdate(string cryptocurrency, decimal price)
        {
            await Clients.All.SendAsync("ReceivePriceUpdate", cryptocurrency, price);
        }
    }
}

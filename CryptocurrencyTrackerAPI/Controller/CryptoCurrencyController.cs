using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CryptocurrencyTrackerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CryptocurrencyController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new List<string> { "Bitcoin", "Ethereum", "Ripple" };
        }
    }
}

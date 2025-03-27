using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;
using Services;
using StockApp.Models;
namespace StockApp.Controllers
{
	[Route("[controller]")]
	public class TradeController : Controller
    {
        private readonly IFinnhubServices _finnhubService;
        private readonly IConfiguration _configuration;
        private readonly TradingOptions _tradingOptions;

        public TradeController(IFinnhubServices finnhubService, IConfiguration configuration, IOptions<TradingOptions> tradingOptions)
		{
			_finnhubService = finnhubService;
			_configuration = configuration;
			_tradingOptions = tradingOptions.Value;
		}
		[Route("/")]
		[Route("[action]")]
		[Route("~/[controller]")]
		public IActionResult Index()
        {
            //reset stock symbol value if it is null
            if (string.IsNullOrEmpty(_tradingOptions.DefaultStockSymbol))
            {
                _tradingOptions.DefaultStockSymbol = "MSFT";
			}

            Dictionary<string, object>? companyProfileDict = _finnhubService.GetCompanyProfile(_tradingOptions.DefaultStockSymbol);

            Dictionary<string,object>? stockQuoteDict = _finnhubService.GetStockPriceQuote(_tradingOptions.DefaultStockSymbol);

            //create a model
            StockTrade stockTrade = new StockTrade()
            {
                StockSymbol = _tradingOptions.DefaultStockSymbol
            };
			//load data from finnHubService into model object
			if (companyProfileDict != null && stockQuoteDict != null)
            {
                stockTrade = new StockTrade()
                {
                    StockSymbol = Convert.ToString(companyProfileDict["ticker"]),
                    StockName = Convert.ToString(companyProfileDict["name"]),
                    Price = Convert.ToDouble(stockQuoteDict["c"].ToString()),
                };
            }
            //send finnhub token to the view
            ViewBag.FinnhubToken = _configuration["FinnhubToken"];
			return View(stockTrade);
        }
    }
}

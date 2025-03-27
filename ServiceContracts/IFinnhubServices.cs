namespace ServiceContracts
{
	/// <summary>
	/// represents a service that makes http requests to finnhub.io
	/// </summary>
	public interface IFinnhubServices
	{
		Dictionary<string, object>? GetCompanyProfile(string stockSymbol);

		Dictionary<string, object>? GetStockPriceQuote(string stockSymbol);
	}
}

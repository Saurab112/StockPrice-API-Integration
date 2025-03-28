﻿using ServiceContracts;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;

namespace Services
{
	public class FinnhubServices : IFinnhubServices
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IConfiguration _configuration;

		public FinnhubServices(IHttpClientFactory httpClientFactory, IConfiguration configuration)
		{
			_httpClientFactory = httpClientFactory;
			_configuration = configuration;
		}
		public Dictionary<string, object>? GetCompanyProfile(string stockSymbol)
		{
			//create a new http client
			HttpClient httpClient = _httpClientFactory.CreateClient();

			//create http request
			HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}") //URI includes the secret token

			};

			//send message
			HttpResponseMessage httpResponseMessage = httpClient.Send(httpRequestMessage);

			//read response body
			string responseBody = new StreamReader(httpResponseMessage.Content.ReadAsStream()).ReadToEnd();

			//convert response body (from JSON into Dictionary)
			Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);

			if (responseDictionary == null)
				throw new InvalidOperationException("No response from server");

			if (responseDictionary.ContainsKey("error"))
				throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));

			return responseDictionary;
		}

		public Dictionary<string, object>? GetStockPriceQuote(string stockSymbol)
		{
			//create a new http client
			HttpClient httpClient = _httpClientFactory.CreateClient();

			//create http request
			HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}") //URI includes the secret token
			};
			HttpResponseMessage httpResponseMessage = httpClient.Send(httpRequestMessage);


			//read response body
			string responseBody = new StreamReader(httpResponseMessage.Content.ReadAsStream()).ReadToEnd();

			//convert response body (from JSON into Dictionary)
			Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);


			if (responseDictionary == null)
				throw new InvalidOperationException("No response from server");

			if (responseDictionary.ContainsKey("error"))
				throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));

			//return response dictionary back to the caller
			return responseDictionary;
		}
	}
}

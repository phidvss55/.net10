using System.Net.Http.Json;
using webapi.Contracts;
using webapi.Dtos.Stock;
using webapi.Mapper;
using webapi.Models;

namespace webapi.Services;

public class FMPService : IFMPService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    
    public FMPService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<Stock?> FindStockBySymbolAsync(string symbol)
    {
        try
        {
            var apiKey = _config["FMPKey"];
            var response = await _httpClient.GetFromJsonAsync<FMPStockDto[]>($"https://financialmodelingprep.com/api/v3/profile/{symbol}?apikey={apiKey}");
            
            if (response != null && response.Length > 0)
            {
                var stock = response[0];
                return stock.ToStockFromFMP();
            }
            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}
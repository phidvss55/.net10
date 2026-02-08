using webapi.Models;

namespace webapi.Contracts
{
    public interface IFMPService
    {
        Task<Stock?> FindStockBySymbolAsync(string symbol);
    }
}

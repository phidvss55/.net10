using webapi.Commons;
using webapi.Dtos.Stock;
using webapi.Models;

namespace webapi.Contracts;

public interface IStockRepository
{
    Task<List<Stock>> GetAllAsync(QueryObject query);
    Task<Stock?> GetByIdAsync(int id);
    Task<Stock?> GetBySymbolAsync(string symbol);
    Task<Stock> CreateAsync(Stock stockModel);
    Task<Stock?> UpdateAsync(int id, UpdateStockRequest stockDto);
    Task<Stock?> DeleteAsync(int id);
    Task<bool> StockExists(int id);
}
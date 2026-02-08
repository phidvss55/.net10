using webapi.Commons;
using webapi.Dtos.Stock;
using webapi.Models;

namespace webapi.Contracts;

public interface IStockRepository
{
    Task<Result<List<Stock>>> GetAllAsync(QueryObject query, CancellationToken ct = default);
    Task<Result<Stock>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<Stock>> GetBySymbolAsync(string symbol, CancellationToken ct = default);
    Task<Result<Stock>> CreateAsync(Stock stockModel, CancellationToken ct = default);
    Task<Result<Stock>> UpdateAsync(int id, UpdateStockRequest stockDto, CancellationToken ct = default);
    Task<Result<Stock>> DeleteAsync(int id, CancellationToken ct = default);
    Task<bool> StockExists(int id, CancellationToken ct = default);
}
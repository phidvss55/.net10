using Microsoft.EntityFrameworkCore;
using webapi.Commons;
using webapi.Contracts;
using webapi.Data;
using webapi.Dtos.Stock;
using webapi.Models;

namespace webapi.Repository;

public class StockRepository : IStockRepository
{
    private readonly ApplicationDBContext _context;
    
    public StockRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<Result<Stock>> CreateAsync(Stock stockModel, CancellationToken ct = default)
    {
        await _context.Stocks.AddAsync(stockModel, ct);
        await _context.SaveChangesAsync(ct);
        return Result<Stock>.Success(stockModel);
    }

    public async Task<Result<Stock>> DeleteAsync(int id, CancellationToken ct = default)
    {
        var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id, ct);

        if (stockModel == null)
        {
            return Result<Stock>.Failure("Stock not found.");
        }

        _context.Stocks.Remove(stockModel);
        await _context.SaveChangesAsync(ct);
        return Result<Stock>.Success(stockModel);
    }

    public async Task<Result<List<Stock>>> GetAllAsync(QueryObject query, CancellationToken ct = default)
    {
        var stocks = _context.Stocks
            .Include(c => c.Comments)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.CompanyName)) 
        {
            stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
        }
        
        if (!string.IsNullOrWhiteSpace(query.Symbol))
        {
            stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
        }
        
        if (!string.IsNullOrWhiteSpace(query.SortBy))
        {
            if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
            {
                stocks = query.IsDecsending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
            }
        }

        var skipNumber = (query.PageNumber - 1) * query.PageSize;
        var result = await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync(ct);
        return Result<List<Stock>>.Success(result);
    }

    public async Task<Result<Stock>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var stock = await _context.Stocks
            .Include(c => c.Comments)
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id, ct);

        return stock == null 
            ? Result<Stock>.Failure("Stock not found.") 
            : Result<Stock>.Success(stock);
    }

    public async Task<Result<Stock>> GetBySymbolAsync(string symbol, CancellationToken ct = default)
    {
        var stock = await _context.Stocks
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Symbol == symbol, ct);

        return stock == null 
            ? Result<Stock>.Failure("Stock not found.") 
            : Result<Stock>.Success(stock);
    }

    public Task<bool> StockExists(int id, CancellationToken ct = default)
    {
        return _context.Stocks.AnyAsync(s => s.Id == id, ct);
    }

    public async Task<Result<Stock>> UpdateAsync(int id, UpdateStockRequest stockDto, CancellationToken ct = default)
    {
        var existingStock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id, ct);

        if (existingStock == null)
        {
            return Result<Stock>.Failure("Stock not found.");
        }

        existingStock.Symbol = stockDto.Symbol;
        existingStock.CompanyName = stockDto.CompanyName;
        existingStock.LastDiv = stockDto.LastDiv;
        existingStock.Industry = stockDto.Industry;
        existingStock.MarketCap = stockDto.MarketCap;

        await _context.SaveChangesAsync(ct);

        return Result<Stock>.Success(existingStock);
    }
}

using Microsoft.EntityFrameworkCore;
using webapi.Commons;
using webapi.Contracts;
using webapi.Data;
using webapi.Models;

namespace webapi.Repository;

public class PortfolioRepository:IPortfolioRepository
{
    private readonly ApplicationDBContext _context;
    public PortfolioRepository(ApplicationDBContext context) {
        _context = context;
    }

    public async Task<Result<Portfolio>> CreateAsync(Portfolio portfolio, CancellationToken ct = default)
    {
        await _context.Portfolios.AddAsync(portfolio, ct);
        await _context.SaveChangesAsync(ct);
        return Result<Portfolio>.Success(portfolio);
    }

    public async Task<Result<Portfolio>> DeletePortfolio(AppUser appUser, string symbol, CancellationToken ct = default)
    {
        var portfolioModel = await _context.Portfolios.FirstOrDefaultAsync(x => x.AppUserId == appUser.Id && x.Stock.Symbol.ToLower() == symbol.ToLower(), ct);
        if (portfolioModel == null) return Result<Portfolio>.Failure("Portfolio entry not found.");

        _context.Portfolios.Remove(portfolioModel);
        await _context.SaveChangesAsync(ct);
        return Result<Portfolio>.Success(portfolioModel);
    }

    public async Task<Result<List<Stock>>> GetUserPortfolio(AppUser user, CancellationToken ct = default)
    {
        var result = await _context.Portfolios
            .AsNoTracking()
            .Where(u => u.AppUserId == user.Id)
            .Select(stock => new Stock
            {
                Id = stock.StockId,
                Symbol = stock.Stock.Symbol,
                CompanyName = stock.Stock.CompanyName,
                Price = stock.Stock.Price,
                LastDiv = stock.Stock.LastDiv,
                Industry = stock.Stock.Industry,
                MarketCap = stock.Stock.MarketCap
            }).ToListAsync(ct);
            
        return Result<List<Stock>>.Success(result);
    }
}
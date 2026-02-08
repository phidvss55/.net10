using webapi.Commons;
using webapi.Models;

namespace webapi.Contracts;

public interface IPortfolioRepository
{
    Task<Result<List<Stock>>> GetUserPortfolio(AppUser user, CancellationToken ct = default);
    Task<Result<Portfolio>> CreateAsync(Portfolio portfolio, CancellationToken ct = default);
    Task<Result<Portfolio>> DeletePortfolio(AppUser appUser, string symbol, CancellationToken ct = default);
}
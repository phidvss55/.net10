using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using webapi.Contracts;
using webapi.Extensions;
using webapi.Models;

namespace webapi.Controllers;

[ApiController]
[Route("/portfolios")]
[Authorize]
public class PortfolioController:BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IStockRepository _stockRepository;
    private readonly IPortfolioRepository _portfolioRepo;
    private readonly IFMPService _fmpService;
    
    public PortfolioController(UserManager<AppUser> usermanager, IStockRepository stockrepository, IPortfolioRepository portfoliorepo,IFMPService fmpService) {
        _userManager = usermanager;
        _stockRepository = stockrepository;
        _portfolioRepo = portfoliorepo;
        _fmpService = fmpService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUserPortfolio(CancellationToken ct)
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        if (appUser == null) return Unauthorized();

        var result = await _portfolioRepo.GetUserPortfolio(appUser, ct);
        if (result.IsFailure) return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddPortfolio(string symbol, CancellationToken ct)
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        if (appUser == null) return Unauthorized();

        var stockResult = await _stockRepository.GetBySymbolAsync(symbol, ct);
        Stock stock;

        if (stockResult.IsFailure) {
            stock = await _fmpService.FindStockBySymbolAsync(symbol);
            if (stock == null) {
                return BadRequest("Stock does not exists");
            } else {
                var createResult = await _stockRepository.CreateAsync(stock, ct);
                if (createResult.IsFailure) return BadRequest(createResult.Error);
                stock = createResult.Value;
            }
        }
        else
        {
            stock = stockResult.Value;
        }
    
        var userPortfolioResult = await _portfolioRepo.GetUserPortfolio(appUser, ct);
        if (userPortfolioResult.IsFailure) return BadRequest(userPortfolioResult.Error);
    
        if (userPortfolioResult.Value.Any(e => e.Symbol.ToLower() == symbol.ToLower())) return BadRequest("Cannot add same stock to portfolio");
    
        var portfolioModel = new Portfolio
        {
            StockId = stock.Id,
            AppUserId = appUser.Id
        };
    
        var createResultPortfolio = await _portfolioRepo.CreateAsync(portfolioModel, ct);
        if (createResultPortfolio.IsFailure) return BadRequest(createResultPortfolio.Error);
    
        return Created();
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeletePortfolio(string symbol, CancellationToken ct)
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        if (appUser == null) return Unauthorized();

        var userPortfolioResult = await _portfolioRepo.GetUserPortfolio(appUser, ct);
        if (userPortfolioResult.IsFailure) return BadRequest(userPortfolioResult.Error);

        var filteredStock = userPortfolioResult.Value.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();
    
        if (filteredStock.Count == 1)
        {
            var deleteResult = await _portfolioRepo.DeletePortfolio(appUser, symbol, ct);
            if (deleteResult.IsFailure) return NotFound(deleteResult.Error);
        }
        else
        {
            return BadRequest("Stock not in your portfolio");
        }
    
        return Ok();
    }
}
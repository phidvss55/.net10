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
    public async Task<IActionResult> GetUserPortfolio()
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
        return Ok(userPortfolio);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddPortfolio(string symbol)
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        var stock = await _stockRepository.GetBySymbolAsync(symbol);
    
        if (stock == null) {
            stock = await _fmpService.FindStockBySymbolAsync(symbol);
            if (stock == null) {
                return BadRequest("Stock does not exists");
            } else {
                await _stockRepository.CreateAsync(stock);
            }
        }
    
        if (stock == null) return BadRequest("Stock not found");
    
        var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
    
        if (userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower())) return BadRequest("Cannot add same stock to portfolio");
    
        var portfolioModel = new Portfolio
        {
            StockId = stock.Id,
            AppUserId = appUser.Id
        };
    
        await _portfolioRepo.CreateAsync(portfolioModel);
    
        if (portfolioModel == null) {
            return StatusCode(500, "Could not create");
        } else {
            return Created();
        }
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeletePortfolio(string symbol)
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
        var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();
    
        if (filteredStock.Count() == 1)
        {
            await _portfolioRepo.DeletePortfolio(appUser, symbol);
        }
        else
        {
            return BadRequest("Stock not in your portfolio");
        }
    
        return Ok();
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Dtos.Stock;
using webapi.Mapper;

namespace webapi.Controllers;

[ApiController]
[Route("/stocks")]
public class StockController : BaseApiController
{
    private readonly ApplicationDBContext _context;

    public StockController(ApplicationDBContext context)
    {
        _context = context;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetStocks()
    {
        // var stocks = _context.Stocks.ToList().Select(s => s.ToStockDto());
        var stocks = await _context.Stocks.ToListAsync(); // this is when you use with async/await  J
        var stockDtos = stocks.Select(s => s.ToStockDto());
        return Ok(stockDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStock(int id)
    {
        var stock = await _context.Stocks.FindAsync(id);
        if (stock == null)
        {
            return NotFound();
        }

        return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> CreateStock([FromBody] CreateStockRequest stockRequest)
    {
        var stock = stockRequest.ToStockFromRequest();
        
        await _context.Stocks.AddAsync(stock);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetStock), new { id = stock .Id }, stock);
    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult UpdateStock([FromRoute] int id, [FromBody] UpdateStockRequest stockRequest)
    {
        
        var stock = _context.Stocks.FirstOrDefault(s => s.Id == id);
        if (stock == null)
        {
            return NotFound();
        }

        stock.CompanyName = stockRequest.CompanyName;
        stock.Price = stockRequest.Price;

        _context.SaveChanges();
     
        return Ok(stock.ToStockDto());
    }

    [HttpDelete]
    [Route("{id}")]
    public IActionResult DeleteStock([FromRoute] int id)
    {
        var stock = _context.Stocks.FirstOrDefault(s => s.Id == id);
        if (stock == null)
        {
            return NotFound();
        }

        _context.Stocks.Remove(stock);
        _context.SaveChanges();

        return Ok();
    }
}
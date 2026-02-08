using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Commons;
using webapi.Contracts;
using webapi.Data;
using webapi.Dtos.Stock;
using webapi.Mapper;

namespace webapi.Controllers;

[ApiController]
[Route("/stocks")]
public class StockController : BaseApiController
{
    private readonly IStockRepository _stockRepository;

    public StockController(IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetStocks([FromQuery] QueryObject query, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _stockRepository.GetAllAsync(query, ct);
        if (result.IsFailure) return BadRequest(result.Error);

        var stockDtos = result.Value.Select(s => s.ToStockDto());
        return Ok(stockDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStock([FromRoute] int id, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _stockRepository.GetByIdAsync(id, ct);
        if (result.IsFailure) return NotFound(result.Error);

        return Ok(result.Value.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> CreateStock([FromBody] CreateStockRequest stockRequest, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var stockModel = stockRequest.ToStockFromRequest();
        var result = await _stockRepository.CreateAsync(stockModel, ct);
        if (result.IsFailure) return BadRequest(result.Error);
        
        return CreatedAtAction(nameof(GetStock), new { id = result.Value.Id }, result.Value.ToStockDto());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStock([FromRoute] int id, [FromBody] UpdateStockRequest stockRequest, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _stockRepository.UpdateAsync(id, stockRequest, ct);
        if (result.IsFailure) return NotFound(result.Error);

        return Ok(result.Value.ToStockDto());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStock([FromRoute] int id, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _stockRepository.DeleteAsync(id, ct);
        if (result.IsFailure) return NotFound(result.Error);

        return NoContent();
    }
}
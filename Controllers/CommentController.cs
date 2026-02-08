using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using webapi.Commons;
using webapi.Contracts;
using webapi.Dtos.Comment;
using webapi.Mapper;
using webapi.Models;

namespace webapi.Controllers;

[ApiController]
[Route("/comments")]
[Authorize]
public class CommentController: BaseApiController
{
    private readonly ICommentRepository _commentRepository;
    private readonly IStockRepository _stockRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly IFMPService _fmpService;
    
    public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository, UserManager<AppUser> userManager, IFMPService fmpService) {
        _commentRepository = commentRepository;
        _stockRepository = stockRepository;
        _userManager = userManager;
        _fmpService = fmpService;
    }

    [HttpGet(Name = "GetAllComments")]
    public async Task<IActionResult> GetAll([FromQuery] CommentQueryObject queryObject, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var result = await _commentRepository.GetAllAsync(queryObject, ct);
        if (result.IsFailure) return BadRequest(result.Error);

        var dto = result.Value.Select(s => s.ToCommentDto());
        return Ok(dto);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var result = await _commentRepository.GetByIdAsync(id, ct);
        if (result.IsFailure) return NotFound(result.Error);
        
        return Ok(result.Value.ToCommentDto());
    }
    
    [HttpPost("{stockId:alpha}")]
    public  async Task<IActionResult> CreateComment([FromRoute] int stockId, [FromBody] CreateCommentRequest commentRequest, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        if (!await _stockRepository.StockExists(stockId, ct))
        {
            return NotFound($"Stock with id {stockId} not found.");
        }

        var commentModel = commentRequest.ToCommentFromCreate(stockId);
        var result = await _commentRepository.CreateAsync(commentModel, ct);
        if (result.IsFailure) return BadRequest(result.Error);
        
        return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value.ToCommentDto());
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentRequest commentRequest, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _commentRepository.UpdateAsync(id, commentRequest.ToCommentFromUpdate(id), ct);
        if (result.IsFailure) return NotFound(result.Error);
        
        return Ok(result.Value.ToCommentDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteComment([FromRoute] int id, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var result = await _commentRepository.DeleteAsync(id, ct);
        if (result.IsFailure) return NotFound(result.Error);
        
        return Ok(result.Value);
    }

}
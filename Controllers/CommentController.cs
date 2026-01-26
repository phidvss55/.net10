using Microsoft.AspNetCore.Mvc;
using webapi.Contracts;
using webapi.Dtos.Comment;
using webapi.Mapper;

namespace webapi.Controllers;

[ApiController]
[Route("/comments")]
public class CommentController: BaseApiController
{
    private readonly ICommentRepository _commentRepository;
    private readonly IStockRepository _stockRepository;
    
    
    public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository)
    {
        _commentRepository = commentRepository;
        _stockRepository = stockRepository;
    }

    [HttpGet(Name = "GetAllComments")]
    public async Task<IActionResult> GetAll()
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var comments = await _commentRepository.GetAllAsync();
        return Ok(comments);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var comment = await _commentRepository.GetByIdAsync(id);
        if (comment == null)
        {
            return NotFound();
        }
        
        return Ok(comment.ToCommentDto());
    }
    
    [HttpPost("{stockId}")]
    public  async Task<IActionResult> CreateComment([FromRoute] int stockId, [FromBody] CreateCommentRequest commentRequest)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        if (!await _stockRepository.StockExists(stockId))
        {
            return NotFound($"Stock with id {stockId} not found.");
        }

        var commentModel = commentRequest.ToCommentFromCreate(stockId);
        await _commentRepository.CreateAsync(commentModel);
        
        return CreatedAtAction(nameof(Get), new { id = commentModel.Id }, commentModel.ToCommentDto());
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentRequest commentRequest)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var comment = await _commentRepository.UpdateAsync(id, commentRequest.ToCommentFromUpdate(id));
        Console.WriteLine(@"1234 "+comment);
        if (comment == null)
        {
            return NotFound();
        }
        
        return Ok(comment.ToCommentDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteComment([FromRoute] int id)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var comment =  await _commentRepository.DeleteAsync(id);
        
        if (comment == null)
        {
            return NotFound("Comment not found.");
        }
        
        return Ok(comment);
    }

}
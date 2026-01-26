using webapi.Dtos.Comment;

namespace webapi.Dtos.Stock;

public class StockDto
{
    public int Id { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal LastDiv { get; set; }
    public string Industry { get; set; } = string.Empty;
    public long MarketCap { get; set; }
    
    // Comments are excluded in the DTO for simplicity
    public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
}
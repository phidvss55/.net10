using webapi.Dtos.Comment;

namespace webapi.Dtos.Stock;

public record StockDto(
    int Id,
    string Symbol,
    string CompanyName,
    decimal Price,
    decimal LastDiv,
    string Industry,
    long MarketCap,
    List<CommentDto> Comments
);
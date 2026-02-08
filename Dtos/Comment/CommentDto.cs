namespace webapi.Dtos.Comment
{
    public record CommentDto(
        int Id,
        string Title,
        string Content,
        DateTime CreatedOn,
        int? StockId
    );
}
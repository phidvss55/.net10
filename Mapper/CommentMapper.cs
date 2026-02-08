using webapi.Dtos.Comment;
using webapi.Models;

namespace webapi.Mapper
{
    public static class CommentMapper
    {
        public static CommentDto ToCommentDto(this Comment commentModel)
        {
            return new CommentDto(
                commentModel.Id,
                commentModel.Title,
                commentModel.Content,
                commentModel.CreatedAt,
                commentModel.StockId
            );
        }

        public static Comment ToCommentFromCreate(this CreateCommentRequest commentDto, int stockId)
        {
            return new Comment
            {
                Title = commentDto.Title,
                Content = commentDto.Content,
                StockId = stockId
            };
        }

        public static Comment ToCommentFromUpdate(this UpdateCommentRequest commentDto, int stockId)
        {
            return new Comment
            {
                Title = commentDto.Title,
                Content = commentDto.Content,
                StockId = stockId
            };
        }

    }
}
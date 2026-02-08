using System.ComponentModel.DataAnnotations;

namespace webapi.Dtos.Comment
{
    public record CreateCommentRequest(
        [Required]
        [MinLength(5, ErrorMessage = "Title must be 5 characters")]
        [MaxLength(280, ErrorMessage = "Title cannot be over 280 characters")]
        string Title,
        
        [Required]
        [MinLength(5, ErrorMessage = "Content must be 5 characters")]
        [MaxLength(280, ErrorMessage = "Content cannot be over 280 characters")]
        string Content
    );
}
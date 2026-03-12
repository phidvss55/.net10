using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace webapi.Models;

// this is for testing razor page
public class Books
{
    public int Id { get; set; }
    [Required]
    [DisplayName("Book Title")]
    public string BookTitle { get; set; }
    [DisplayName("Book Description")]
    public string? BookDescription { get; set; }
    [Required]
    [DisplayName("Author Name")]
    public string AuthorName { get; set; }
}
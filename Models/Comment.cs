namespace webapi.Models;

public class Comment
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    
    public int? StockId { get; set; }
    // navigation property 
    public Stock? Stock { get; set; }
    
    public AppUser AppUser { get; set; }


}
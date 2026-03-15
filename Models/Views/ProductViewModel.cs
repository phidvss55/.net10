using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace webapi.Models.Views;

public class ProductViewModel
{
    public int Id { get; set; }
    [Required]
    [DisplayName("Product Name")]
    public string Name { get; set; } = string.Empty;
    [Required]
    public decimal Price { get; set; }
    [Required]
    public int Qty { get; set; }
    
}
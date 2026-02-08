using System.ComponentModel.DataAnnotations;

namespace webapi.Dtos.Stock;

public record UpdateStockRequest(
    [Required]
    [MaxLength(10, ErrorMessage = "Symbol cannot be over 10 characters")]
    string Symbol,
    
    [Required]
    [MaxLength(10, ErrorMessage = "Company Name cannot be over 10 characters")]
    string CompanyName,
    
    [Required]
    [Range(1, 1000000000)]
    decimal Price,
    
    [Required]
    [Range(0.001, 100)]
    decimal LastDiv,
    
    [Required]
    [MaxLength(10, ErrorMessage = "Industry cannot be over 10 characters")]
    string Industry,
    
    [Range(1, 5000000000)]
    long MarketCap
);
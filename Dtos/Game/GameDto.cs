using System.ComponentModel.DataAnnotations;

namespace webapi.Dtos.Game;

public record  GameDto(
    [Required]
    [StringLength(5, ErrorMessage =  "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
    string Name,
    
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
);

